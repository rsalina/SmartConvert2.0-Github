using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using TCPSmart.Conexion;
using TCPSmart.Ws;

namespace TCPSmart.Flow
{
    public class FlowOperations
    {
        public ConexMaster FlowServer { get; }
        public List<ClienteTCP> EndPoints { get; set; }

        public String IPCliente { get; }

        public TaskCompletionSource<Boolean> Tz { get; set; }

        public Action<String, Boolean> Loger = null;
        public Byte[] Package { get; set; }

        private NetManager NetManager { get; set; }
        private Dictionary<Int32, ConexMaster> FlowConnections { get; set; }
        private Dictionary<Int32, Int32> FlowConnectionsTimeOut { get; set; }

        public String MTIRequest { get; }
        public String MTIResponse { get; }
        private Byte[] DataFromOrigen { get; }
        private String DataFromOrigenHex { get; }
        public Dictionary<Int32, String[]> ISOParsed { get; set; }
        private Dictionary<Int32, String[]> ISOTransl { get; set; }
        private Dictionary<Int32, String[]> ISORespon { get; set; }

        private Boolean FlowIsAlive { get; set; }





        public FlowOperations(NetManager _netManager, Byte[] Data, String IP)
        {
            EndPoints = new List<ClienteTCP>();
            FlowConnectionsTimeOut = new Dictionary<Int32, Int32>();

            NetManager = _netManager;
            FlowServer = NetManager.ConexMServer;
            DataFromOrigen = Data;
            DataFromOrigenHex = BitConverter.ToString(Data).Replace("-", "");

            UtilFlowOperation.HeaderBytes = FlowServer.BytesHeader;
            UtilFlowOperation.typeMsg = FlowServer.TypeMsg;

            IPCliente = IP;

            InitFlowConections();
            SetupISOS();

            FlowIsAlive = false;

            ISOParsed.TryGetValue(FlowServer.TypeMsg, out String[] OrigenISOParsed);
            MTIRequest = OrigenISOParsed[129];
            MTIResponse = "0" + (Convert.ToInt32(MTIRequest) + 10);
        }


        private void InitFlowConections()
        {
            FlowConnections = new Dictionary<Int32, ConexMaster>();

            ConexMaster Conex = FlowServer; //Main NetManager

            FlowConnections.Add(Conex.Id, Conex);

            do
            {
                Conex = Conex.EndConnectionMasterId;
                FlowConnections.Add(Conex.Id, Conex);

                if (Conex.TimeOutSec > 0)
                {
                    FlowConnectionsTimeOut.Add(Conex.Id, Conex.TimeOutSec);
                }

                if (Conex.ConnectionType != 4)
                {
                    EndPoints.Add(TCPUtil.ConvertToClienteTCP(Conex));
                }

            } while (Conex.EndConnectionMasterId != null);

        }

        private String Field3Original { get; set; }
        private void SetupISOS()
        {
            ISO8583 HelperISO = new ISO8583(FlowServer.TypeMsg);

            String ISO = UtilFlowOperation.GetISO8583FromHex(DataFromOrigenHex, FlowServer.BytesHeader, FlowServer.TypeMsg);

            ISOParsed = new Dictionary<Int32, String[]>();
            ISOTransl = new Dictionary<Int32, String[]>();
            ISORespon = new Dictionary<Int32, String[]>();

            FlowConnections.ToList().ForEach(Cn =>
            {
                if (Cn.Value.Id == FlowServer.Id)
                {
                    String[] Parsed = HelperISO.Parse(ISO, false);
                    String[] TransL = HelperISO.Parse(ISO, true);

                    ISOParsed.Add(Cn.Value.TypeMsg, Parsed);
                    ISOTransl.Add(Cn.Value.TypeMsg, TransL);
                }

                if (!ISOParsed.ContainsKey(Cn.Value.TypeMsg) && Cn.Value.TypeMsg > 0)
                {
                    HelperISO = new ISO8583(Cn.Value.TypeMsg);
                    ISOTransl.TryGetValue(FlowServer.TypeMsg, out String[] BaseISO);

                    String BuildIso = HelperISO.Build(BaseISO, BaseISO[129]);

                    ISOParsed.Add(Cn.Value.TypeMsg, HelperISO.Parse(BuildIso, false));
                    ISOTransl.Add(Cn.Value.TypeMsg, HelperISO.Parse(BuildIso, true));
                }

            });
        }

        //public void SendResponseByPassToOrigen(Byte[] Paquete, String RC)
        //{
        //    String LogUI = "Por ByPass Desde Destino";
        //    NetManager.ServerNetManager.Envia(IPCliente, Paquete);

        //    UpdateServerStatic(1, 0); UpdateServerIcon(0);

        //    SaveLog("Cadena enviada (T-" + FlowServer.TypeMsg + "): " + LogUI + " -> " + BitConverter.ToString(Paquete).Replace("-", ""), FlowServer.Id, false);

        //    String L = IPCliente + " Request " + MTIRequest + " -> Response " + MTIResponse + " codigo " + RC + " enviado con exito, termina conexion en socket";
        //    SaveLog(L, FlowServer.Id, true);
        //}

        public Boolean SendResponseToOrigin(String RC, String Code, Boolean UseISOResponse, String ISOF38Response = "", String LogUI = "")
        {
            String[] ISO4Res;
            try
            {
                String Response = "";
                if (UseISOResponse)
                {
                    ISORespon.TryGetValue(FlowServer.TypeMsg, out ISO4Res);
                    String BMP = TCPUtil.Binary2BMP(ISO4Res);
                    Response = UtilFlowOperation.BuildChainFromBMP(BMP, FlowServer.TypeMsg, ISO4Res, Code);
                }
                else
                {
                    ISOParsed.TryGetValue(FlowServer.TypeMsg, out ISO4Res);
                    Response = UtilFlowOperation.BuildChainFromRC(RC, FlowServer, ISO4Res, Code, ISOF38Response);
                }

                if (RC != "")
                {
                    SaveLog("Enviando LiveResponse " + RC + " -> RES: " + Code + " a " + IPCliente, FlowServer.Id, true);
                }
                else
                {
                    if (ISO4Res[39] != null)
                        RC = FlowServer.TypeMsg == 1 ? TCPUtil.EBDicToString(ISO4Res[39]) : Encoding.ASCII.GetString(TCPUtil.ConvertHexStringToByteArray(ISO4Res[39]));
                }

                Byte[] Paquete = UtilFlowOperation.GetBytesToSend(Response, true, FlowServer);

                NetManager.ServerNetManager.Envia(IPCliente, Paquete);

                UpdateServerStatic(1, 0); UpdateServerIcon(0);

                SaveLog("Cadena enviada (T-" + FlowServer.TypeMsg + "): " + LogUI + " -> " + BitConverter.ToString(Paquete).Replace("-", ""), FlowServer.Id, false);

                String L = IPCliente + " Request " + MTIRequest + " -> Response " + MTIResponse + " codigo " + RC + " enviado con exito, termina conexion en socket";
                SaveLog(L, FlowServer.Id, true);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        private Int32 ConterTry = 0;

        public void HandleFlow()
        {
            ConterTry = 0;

            ConexMaster FConex = FlowServer.EndConnectionMasterId;

            if (InitEndPoint(FConex))
            {
                FConex.ISOsParsed = new Dictionary<Int32, String[]>();
                FConex.ISOsTransl = new Dictionary<Int32, String[]>();

                HandleFlowByWs(FConex);


            }
            else
            {
                CompleteFlow();
            }
        }



        //private void HandleFlowByType(Int32 CnType, ConexMaster mEndPoint)
        //{
        //    ClienteTCP mCli = null;

        //    switch (CnType)
        //    {
        //        case 1:
        //            if (FillFieldsForByPass(mEndPoint))
        //            {
        //                ISORespon.TryGetValue(mEndPoint.TypeMsg, out String[] Fields4ByPass);
        //                String Response = UtilFlowOperation.BuildChainFromBMP("", mEndPoint.TypeMsg, Fields4ByPass, MTIRequest);

        //                if (Response != null)
        //                {
        //                    Package = UtilFlowOperation.GetBytesToSend(Response, true, mEndPoint);
        //                    UpdateConexStatic(mEndPoint.Id, 1, 0);

        //                    SaveLog("Enviando ISO8583 Generado por BMP (" + UtilFlowOperation.GlobalBMP + ") : " + BitConverter.ToString(Package).Replace("-", ""), mEndPoint.Id, false);

        //                    mCli = EndPoints.Where(c => c.Conex.Id == mEndPoint.Id).First();

        //                    mCli.Send(Package);
        //                }
        //                else
        //                {
        //                    SaveLog("Cadena de respuesta mal formada, TipoMsg:" + mEndPoint.TypeMsg + ", posible Bitmap invalido /no configurado", mEndPoint.Id, true);

        //                    SendResponseToOrigin("91", MTIResponse, false);
        //                    KillEndPoint(mEndPoint);
        //                }
        //            }
        //            else
        //            {
        //                KillEndPoint(mEndPoint);
        //            }
        //            break;
        //        case 3:
        //            UpdateConexStatic(mEndPoint.Id, 1, 0);
        //            SaveLog("Conexion " + mEndPoint.DescriptionName + "(" + mEndPoint.IPAdress + ":" + mEndPoint.Port + ") en funcion enviando Data Original,Modo de envio -> ByPass", mEndPoint.Id, false);
        //            Package = DataFromOrigen;

        //            mCli = EndPoints.Where(c => c.Conex.Id == mEndPoint.Id).First();//Se Espera Respuesta
        //            mCli.Send(Package);
        //            break;
        //    }

        //}




        Dictionary<String, ConexMaster> ConexWs = new Dictionary<string, ConexMaster>();
        Dictionary<String, String> ConexF35 = new Dictionary<String, String>();
        private void HandleFlowByWs(ConexMaster mEndPoint)
        {
            Boolean MissingF35 = false;
            Boolean ExistAmbient = false;
            try
            {
                ISOTransl.TryGetValue(FlowServer.TypeMsg, out String[] OrigenISOTransl);

                ExistAmbient = TCPUtil.WsAmbientes.TryGetValue(OrigenISOTransl[41].Substring(0, 2), out WsAmbiente WsAmbient);
                if (ExistAmbient)
                {
                    InitConcentradorWS(WsAmbient, mEndPoint); //(WsAmbient, mEndPoint);
                    UpdateConexStatic(mEndPoint.Id, 1, 0);

                    String ValF0F35 = "0";
                    Pair<String, String> Transaction;
                    Int32 Id_Transaction = 0;

                    String ModeWs = TCPUtil.WsAmbientForOperation.Ambiente; //se utiliza para logica de ruteo.

                    if (!MissingF35)
                    {
                        if (OrigenISOTransl[35] == "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF")
                        {
                            MissingF35 = true;
                            ValF0F35 = "F";
                        }
                        if (OrigenISOTransl[35] == null)
                        {
                            MissingF35 = true;
                            OrigenISOTransl[35] = "0000000000000000=0000000000000000000";
                            ValF0F35 = "0";
                        }
                        if (OrigenISOTransl[35] == "")
                        {
                            MissingF35 = true;
                            OrigenISOTransl[35] = "0000000000000000=0000000000000000000";
                            ValF0F35 = "0";
                        }
                    }

                    if (MTIRequest == "0400")
                    {
                        ModeWs += " - Reversed Transaction";
                        Transaction = DBUtil.GetTransaction(OrigenISOTransl[37], OrigenISOTransl[41], OrigenISOTransl[2], out Id_Transaction);
                        if (Transaction.First == "" || Transaction.Second == "")
                        {
                            SaveLog("Transaccion de Reversa invalida no existe o ya fue revertida con anterioridad, TipoMsg:" + FlowServer.EndConnectionMasterId.TypeMsg + " F37:" + OrigenISOTransl[37] + " F41:" + OrigenISOTransl[41] + " F2:" + OrigenISOTransl[2], FlowServer.EndConnectionMasterId.Id, true);

                            SendResponseToOrigin("05", MTIResponse, false);
                            UpdateConexIcon(FlowServer.EndConnectionMasterId.Id, 0);
                            MonitorConexion.TryRemove(mEndPoint.Id, out DateTime _);
                            KillEndPoint(mEndPoint);
                            CompleteFlow();
                            return;
                        }

                        ValF0F35 = Transaction.First;
                        OrigenISOTransl[38] = Transaction.Second;

                        MissingF35 = true;
                    }

                    if (MissingF35)
                    {
                        SaveLog(UtilFlowOperation.GetISOLabel(false, MTIRequest, OrigenISOTransl, ModeWs, ValF0F35), mEndPoint.Id, false);
                    }
                    else
                    {
                        ValF0F35 = OrigenISOTransl[35].Substring(24, 1);
                        SaveLog(UtilFlowOperation.GetISOLabel(false, MTIRequest, OrigenISOTransl, ModeWs, ValF0F35), mEndPoint.Id, false);
                    }
                    //print ws url
                    SaveLog("Se envio trn al WS: " + WsAmbient.URL, FlowServer.EndConnectionMasterId.Id, false);
                    //end print url
                    if (ModeWs=="BZ") //(WsAmbient.WsCashOutGuip)
                    {
                        var DT_ALT_Object = TCPUtil.GetDTAtmAlt(new WsAlt.DT_CashOut()
                        {
                            F0 = ValF0F35,
                            F1 = MTIRequest,
                            F2 = OrigenISOTransl[2],
                            F3 = OrigenISOTransl[3],
                            F4 = OrigenISOTransl[4],
                            F7 = OrigenISOTransl[7],
                            F11 = OrigenISOTransl[11],
                            F19 = OrigenISOTransl[19],
                            F25 = OrigenISOTransl[25],
                            F32 = OrigenISOTransl[32],
                            F35 = OrigenISOTransl[35],
                            F37 = OrigenISOTransl[37],
                            F38 = OrigenISOTransl[38],
                            F41 = OrigenISOTransl[41],
                            F42 = OrigenISOTransl[42],
                            F43 = OrigenISOTransl[43],
                            F49 = OrigenISOTransl[49],
                            F63 = OrigenISOTransl[63]
                        });

                        ConexWs.Add(mWSAlt.Guido, mEndPoint);
                        ConexF35.Add(mWSAlt.Guido, ValF0F35);
                        Task S = SendWsTransaction(mWSAlt, DT_ALT_Object);
                    }
                    if (ModeWs == "BA")
                    {
                        var DT_Object = TCPUtil.GetDTATM(new DT_ATM_CashOut()
                        {
                            F0 = ValF0F35,
                            F1 = MTIRequest,
                            F2 = OrigenISOTransl[2],
                            F3 = OrigenISOTransl[3],
                            F4 = OrigenISOTransl[4],
                            F7 = OrigenISOTransl[7],
                            F11 = OrigenISOTransl[11],
                            F19 = OrigenISOTransl[19],
                            F25 = OrigenISOTransl[25],
                            F32 = OrigenISOTransl[32],
                            F35 = OrigenISOTransl[35],
                            F37 = OrigenISOTransl[37],
                            F38 = OrigenISOTransl[38],
                            F41 = OrigenISOTransl[41],
                            F42 = OrigenISOTransl[42],
                            F43 = OrigenISOTransl[43],
                            F49 = OrigenISOTransl[49],
                            F63 = OrigenISOTransl[63]
                        });



                        ConexWs.Add(mWS.Guido, mEndPoint);
                        ConexF35.Add(mWS.Guido, ValF0F35);

                        Task S = SendWsTransaction(mWS, DT_Object, mEndPoint.Id);                        
                    }


                }
                else
                {
                    SaveLog("Error en la comunicacion con el Ws, TipoMsg:" + FlowServer.EndConnectionMasterId.TypeMsg + ", Detalle: El Ambiente requerido " + OrigenISOTransl[41].Substring(0, 2) + " no se encuentra configurado", FlowServer.EndConnectionMasterId.Id, true);

                    SendResponseToOrigin("91", MTIResponse, false);
                    UpdateConexIcon(FlowServer.EndConnectionMasterId.Id, 0);
                    CompleteFlow();
                }

            }
            catch (Exception ex)
            {
                SaveLog("Error en la comunicacion con el Ws, TipoMsg:" + FlowServer.EndConnectionMasterId.TypeMsg + ", Detalle:" + ex.Message, FlowServer.EndConnectionMasterId.Id, true);

                SendResponseToOrigin("91", MTIResponse, false);
                UpdateConexIcon(FlowServer.EndConnectionMasterId.Id, 0);
                CompleteFlow();
            }
        }

        private void CompleteFlow()
        {
            Tz.SetResult(true);
        }

        private Boolean InitEndPoint(ConexMaster mEndPoint)
        {
            Boolean Ret = false;
            ClienteTCP EndPoint = EndPoints.Where(c => c.Conex.Id == mEndPoint.Id).FirstOrDefault();
            try
            {
            lopy:

                if (EndPoint != null)
                {
                    if (!EndPoint.IsConnected)
                    {
                        try
                        {
                            EndPoint.Desconectado += EndPoint_Desconectado;
                            EndPoint.DataReceived += EndPoint_DataReceived;

                            UpdateConexIcon(mEndPoint.Id, 75);

                            String L = "Intentando conectar con " + mEndPoint.DescriptionName + "(" + mEndPoint.IPAdress + ":" + mEndPoint.Port + ")";
                            SaveLog(L, mEndPoint.Id, true);


                            EndPoint.Connect(new FromClienteDataReceived(IPCliente, DataFromOrigen));
                            Ret = true;


                            if (FlowConnectionsTimeOut.ContainsKey(mEndPoint.Id))
                            {
                                MonitorConexion = new ConcurrentDictionary<Int32, DateTime>();
                                _TokenBMP = _TokenBMPSource.Token;
                                Task.Run(() => MonitorForConexion(), _TokenBMP);
                            }
                        }
                        catch (Exception)
                        {
                            if (ConterTry == 10)
                            {
                                String vL = "Conexion " + mEndPoint.DescriptionName + "(" + mEndPoint.IPAdress + ":" + mEndPoint.Port + ") no disponible, no es posible continuar comunicacion, Intentos: 10";
                                ConterTry = 0;

                                UpdateConexIcon(mEndPoint.Id, 50);

                                SaveLog(vL, mEndPoint.Id, true);

                                SendResponseToOrigin("91", MTIResponse, false);
                                this.FlowIsAlive = false;
                                Ret = false;
                            }
                            else
                            {
                                String L = "La conexión " + mEndPoint.DescriptionName + " se encuentra en uso o no esta disponible, intento: " + ConterTry + 1;
                                SaveLog(L, mEndPoint.Id, true);
                                ConterTry++;
                                goto lopy;
                            }
                        }
                    }
                    else
                    {
                        if (ConterTry == 10)
                        {
                            String bL = "Conexion " + mEndPoint.DescriptionName + "(" + mEndPoint.IPAdress + ":" + mEndPoint.Port + ") no disponible, no es posible continuar comunicacion, Intentos: 10";
                            ConterTry = 0;

                            UpdateConexIcon(mEndPoint.Id, 50);

                            SaveLog(bL, mEndPoint.Id, true);

                            SendResponseToOrigin("91", MTIResponse, false);
                            this.FlowIsAlive = false;
                            Ret = false;
                        }
                        else
                        {
                            String L = "-La conexión " + mEndPoint.DescriptionName + " se encuentra en uso o no esta disponible, intento: " + ConterTry + 1;
                            SaveLog(L, mEndPoint.Id, true);
                            ConterTry++;
                            goto lopy;
                        }
                    }
                }
                else
                {
                    if (mEndPoint.ConnectionType == 4) //Es un WebService
                    {
                        if (FlowConnectionsTimeOut.ContainsKey(mEndPoint.Id))
                        {
                            MonitorConexion = new ConcurrentDictionary<Int32, DateTime>();
                            _TokenBMP = _TokenBMPSource.Token;
                            Task.Run(() => MonitorForConexion(), _TokenBMP);
                        }

                        UpdateConexIcon(mEndPoint.Id, 75);
                        String L = "Iniciando Conexion con " + mEndPoint.DescriptionName + " (WebService)";

                        SaveLog(L, mEndPoint.Id, true);
                        Ret = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Ret = false;
                UpdateConexIcon(mEndPoint.Id, 50);
                String L = "Conexion " + mEndPoint.DescriptionName + "(" + mEndPoint.IPAdress + ":" + mEndPoint.Port + ") no disponible, no es posible continuar comunicacion, ex: " + ex.Message;
                SaveLog(L, mEndPoint.Id, true);

                SendResponseToOrigin("91", MTIResponse, false);
                this.FlowIsAlive = false;
            }
            return Ret;
        }

        private void EndPoint_DataReceived(object sender, FromServerDataReceivedEventArgs e)
        {
            ClienteTCP Conex = (ClienteTCP)sender;

            ConexMaster mEndPoint = Conex.Conex;

            //if (mEndPoint.IsFinalCn)
            //{
            //    var S = "";
            //}

            //XmlDocument Respuesta = null;
            //MemoryStream Ms = null;

            UpdateConexStatic(mEndPoint.Id, 0, 1);
            UpdateConexIcon(mEndPoint.Id, 0);

            SaveLog("Conexion " + mEndPoint.DescriptionName + " recibe datos (T-" + mEndPoint.TypeMsg + ")", mEndPoint.Id, true);

            switch (mEndPoint.ConnectionType)
            {

                default: //En su Lenguaje                   
                    SaveLog("Data recibida *(T-" + mEndPoint.TypeMsg + "): " + BitConverter.ToString(e.Data).ToString().Replace("-", "") + " en " + mEndPoint.DescriptionName, mEndPoint.Id, false);
                    break;
            }

            // HandleEndPoint_Flow(e, mEndPoint, Respuesta);
            if (!FlowIsAlive)
            {
                CompleteFlow();

            }
        }

        private void HandleResponseISO(ConexMaster mEndPoint, Byte[] Data, Boolean CheckTimeOut, Boolean ValidateBMP)
        {
            String[] ResponseFieldIso8583 = new String[130];
            ISO8583 IsoHelper = new ISO8583(mEndPoint.TypeMsg);

            String ResponseFromEndPoint = BitConverter.ToString(Data).Replace("-", "");

            try
            {
                FlowConnections.ToList().ForEach(cn =>
                {
                    AddISO(ResponseFromEndPoint, cn.Value.TypeMsg, cn.Value.BytesHeader, mEndPoint);
                });

                if (mEndPoint.IsFinalCn) //Enviamos al Origen
                {
                    SaveLog("Preparando respuesta (T-" + FlowServer.TypeMsg + ") para " + FlowServer.DescriptionName, mEndPoint.Id, true);

                    mEndPoint.ISOsParsed.TryGetValue(FlowServer.TypeMsg, out String[] RecivedFieldsIso8583); //Parseados

                    if (ValidateBMP)
                    {
                        if (FlowServer.BitMaps.ContainsKey(RecivedFieldsIso8583[129]))
                        {
                            FlowServer.BitMaps.TryGetValue(RecivedFieldsIso8583[129], out String BMP);

                            ResponseFieldIso8583 = TCPUtil.GetCamposByBMP(BMP, RecivedFieldsIso8583, RecivedFieldsIso8583, FlowServer.TypeMsg, null);

                            String LogUi = "";

                            ISORespon.Clear();
                            ISORespon = new Dictionary<Int32, String[]>();

                            ISORespon.Add(FlowServer.TypeMsg, ResponseFieldIso8583);

                            UpdateConexIcon(mEndPoint.Id, 0);

                            SendResponseToOrigin("", MTIResponse, true, null, LogUi);

                        }
                        else
                        {
                            SaveLog("Conexion " + mEndPoint.DescriptionName + "(" + mEndPoint.IPAdress + ":" + mEndPoint.Port + ") codigo de REQUEST[" + MTIRequest + "];BuildType:BuildBMPXML;TypeMsg:" + FlowServer.TypeMsg + "; no programado, no es posible continuar", mEndPoint.Id, true);
                            SendResponseToOrigin("91", MTIResponse, false);
                            if (!CheckTimeOut)
                                KillEndPoint(mEndPoint);

                        }
                    }
                    else
                    {
                        mEndPoint.ISOsTransl.TryGetValue(FlowServer.TypeMsg, out String[] TranslationFieldsIso8583); //Traduccion

                        String F38F = "";
                        if (RecivedFieldsIso8583[38] != null)
                        {
                            F38F = RecivedFieldsIso8583[38];
                        }

                        SendResponseToOrigin(TranslationFieldsIso8583[39], MTIResponse, false, F38F); //no envia 38

                    }
                }
                else //Enviamos a la siguiente Conexion
                {

                }
            }
            catch (Exception ex)
            {
                SaveLog("Conexion " + mEndPoint.DescriptionName + "(" + mEndPoint.IPAdress + ":" + mEndPoint.Port + ") cadena de respuesta con errores, no es posible continuar flujo ex:" + ex.Message, mEndPoint.Id, true);
                SendResponseToOrigin("91", MTIResponse, false);
                if (!CheckTimeOut)
                    KillEndPoint(mEndPoint);
            }
        }

        private void HandleEndPoint_Flow(FromServerDataReceivedEventArgs Data, ConexMaster mEndPoint, XmlDocument Respuesta = null)
        {
            Boolean inTime = true;
            Boolean CheckTimeOut = false;

            if (FlowConnectionsTimeOut.ContainsKey(mEndPoint.Id))
            {
                inTime = CheckInTime(mEndPoint, Data.Data);
                CheckTimeOut = true;
            }

            if (inTime)
            {
                switch (mEndPoint.ConnectionType)
                {
                    case 1:
                        HandleResponseISO(mEndPoint, Data.Data, CheckTimeOut, true);
                        break;



                    case 3:
                        if (mEndPoint.IsFinalCn) //Enviamos al Server
                        {
                            NetManager.ServerNetManager.Envia(IPCliente, Data.Data);
                            UpdateConexStatic(FlowServer.Id, 1, 0); UpdateConexIcon(FlowServer.Id, 75);

                            SaveLog("Respuesta enviada a cliente " + IPCliente + " con exito", FlowServer.Id, false);
                            SaveLog("Conexion con " + IPCliente + " en " + FlowServer.DescriptionName + "(" + FlowServer.IPAdress + ":" + FlowServer.Port + "), termina, esperando nueva conexion", FlowServer.Id, false);
                        }
                        else //Enviamos a la siguiente conexion
                        {

                        }
                        break;
                }
            }
        }

        private void EndPoint_Desconectado(object sender, ForHostEventArgs e)
        {
            ClienteTCP mEndPoint = (ClienteTCP)sender;

            if (e.Motivo == DesconexionType.Timeout && !MonitorConexion.ContainsKey(mEndPoint.Conex.Id)) //<<<==== Solo se va a disparar cuando no responda la conexion endpoint
            {
                SendResponseToOrigin("91", MTIResponse, false);
                SaveLog(e.Datos.IPAddress + "- Conexion terminada", mEndPoint.Conex.Id, false);
            }
        }







        #region Util
        private void AddISO(String HEXOriginal, Int32 TypeMsg, Int32 HBytes, ConexMaster Container)
        {
            if (TypeMsg != 2) //Evitamos XML
            {
                if (Container.ISOsParsed == null)
                {
                    Container.ISOsParsed = new Dictionary<Int32, String[]>();
                }

                if (Container.ISOsTransl == null)
                {
                    Container.ISOsTransl = new Dictionary<Int32, String[]>();
                }

                ISO8583 HelperISO;
                String RecISO;

                //Primero Agrega su propio lenguaje
                if (Container.ISOsParsed.Count == 0)
                {
                    HelperISO = new ISO8583(Container.TypeMsg);

                    RecISO = UtilFlowOperation.GetISO8583FromHex(HEXOriginal, Container.BytesHeader, Container.TypeMsg);

                    Container.ISOsParsed.Add(Container.TypeMsg, HelperISO.Parse(RecISO, false));
                    Container.ISOsTransl.Add(Container.TypeMsg, HelperISO.Parse(RecISO, true));
                }

                if (!Container.ISOsParsed.ContainsKey(TypeMsg)) //Traducimos
                {
                    HelperISO = new ISO8583(TypeMsg);

                    Container.ISOsTransl.TryGetValue(Container.TypeMsg, out String[] BaseISO);

                    String BuildIso = HelperISO.Build(BaseISO, BaseISO[129]);

                    Container.ISOsParsed.Add(TypeMsg, HelperISO.Parse(BuildIso, false));
                    Container.ISOsTransl.Add(TypeMsg, HelperISO.Parse(BuildIso, true));
                }

            }
        }

        public void SaveLog(String Msj, Int32 ConexId, Boolean ForUI, String Tipo = "Flow")
        {
            if (TCPUtil.ConexsUnderMonitor.ContainsKey(ConexId)) //Modo DEBUG ON
            {
                Loger?.Invoke(Msj, true);
                if (NetManager.Zesiones.ContainsKey(IPCliente))
                {
                    NetManager.Zesiones.TryGetValue(IPCliente, out Int32 Zesion);
                    Task.Run(async () => await DBUtil.InsertBitacoraASync(Msj, ConexId, IPCliente, Zesion + "_" + Tipo, NetManager.Id));
                }
                else
                {
                    Task.Run(async () => await DBUtil.InsertBitacoraASync(Msj, ConexId, IPCliente, Tipo, NetManager.Id));
                }

            }
            else
            {
                Loger?.Invoke(Msj, ForUI);
            }
        }

        public void UpdateServerStatic(Int32 Enviadas, Int32 Recibidas)
        {
            try
            {
                var Dur = DateTime.Now - TCPUtil.Statics.Where(c => c.ID == FlowServer.Id).First().Inicio;
                TCPUtil.Statics.Where(c => c.ID == FlowServer.Id).First().UpTime = Dur.Hours + ":" + Dur.Minutes + ":" + Dur.Seconds;

                Int32 HowMany = TCPUtil.Statics.Where(c => c.ID == FlowServer.Id).First().Recibidas + Recibidas;
                TCPUtil.Statics.Where(c => c.ID == FlowServer.Id).First().Recibidas = HowMany;

                Int32 HowOut = TCPUtil.Statics.Where(c => c.ID == FlowServer.Id).First().Enviadas + Enviadas;
                TCPUtil.Statics.Where(c => c.ID == FlowServer.Id).First().Enviadas = HowOut;
                Application.DoEvents();
            }
            catch (Exception)
            {

            }
        }

        public void UpdateServerIcon(Int32 Valor)
        {
            try
            {
                TCPUtil.Statics.Where(c => c.ID == FlowServer.Id).First().Status = Valor;
                Application.DoEvents();
            }
            catch (Exception)
            {

            }
        }

        public void UpdateConexIcon(Int32 ID, Int32 Valor)
        {
            try
            {
                TCPUtil.Statics.Where(c => c.ID == ID).First().Status = Valor;
                Application.DoEvents();
            }
            catch (Exception)
            {

            }
        }

        public void UpdateConexStatic(Int32 ID, Int32 Enviadas, Int32 Recibidas)
        {
            try
            {
                var Dur = DateTime.Now - TCPUtil.Statics.Where(c => c.ID == ID).First().Inicio;
                TCPUtil.Statics.Where(c => c.ID == ID).First().UpTime = Dur.Hours + ":" + Dur.Minutes + ":" + Dur.Seconds;

                Int32 HowMany = TCPUtil.Statics.Where(c => c.ID == ID).First().Recibidas + Recibidas;
                TCPUtil.Statics.Where(c => c.ID == ID).First().Recibidas = HowMany;

                Int32 HowOut = TCPUtil.Statics.Where(c => c.ID == ID).First().Enviadas + Enviadas;
                TCPUtil.Statics.Where(c => c.ID == ID).First().Enviadas = HowOut;
                Application.DoEvents();
            }
            catch (Exception)
            {

            }
        }

        //  private String Field4Original { get; set; }
        private Boolean FillFieldsForByPass(ConexMaster mEndPoint)
        {
            Boolean Ret = false;

            if (mEndPoint.BitMaps.ContainsKey(MTIRequest))//Esta preparado para contestar?
            {
                mEndPoint.BitMaps.TryGetValue(MTIRequest, out String BMP);

                ISOParsed.TryGetValue(mEndPoint.TypeMsg, out String[] OrigenISOParsed);
                ISOTransl.TryGetValue(mEndPoint.TypeMsg, out String[] OrigenISOTransl);

                ISOParsed.TryGetValue(FlowServer.TypeMsg, out String[] OrigenISOPServer);
                ISOTransl.TryGetValue(FlowServer.TypeMsg, out String[] OrigenISOPServerT);

                try
                {
                    ISORespon.Add(mEndPoint.TypeMsg, TCPUtil.GetCamposByBMP(BMP, OrigenISOParsed, OrigenISOTransl, mEndPoint.TypeMsg, null));
                    Ret = true;
                }
                catch (Exception ex)
                {
                    String L = "REQ:" + MTIRequest + " no es posible generar cadena de respuesta, " + ex.Message;
                    SaveLog(L, mEndPoint.Id, false);

                    SendResponseToOrigin("91", MTIResponse, true);
                    Ret = false;
                }
            }
            else
            {
                String L = "Bitmap no configurado, REQ:" + MTIRequest + " no es posible generar cadena de respuesta";
                SaveLog(L, mEndPoint.Id, false);

                SendResponseToOrigin("91", MTIResponse, true);
            }
            return Ret;
        }

        private void KillEndPoint(ConexMaster mEndPoint)
        {
            UpdateConexIcon(mEndPoint.Id, 0);
            if (mEndPoint.ConnectionType != 4)
            {
                EndPoints.Where(c => c.Conex.Id == mEndPoint.Id).First().Dispose();
            }

            if (FlowConnectionsTimeOut.ContainsKey(mEndPoint.Id))
            {
                if (MonitorConexion.ContainsKey(mEndPoint.Id))
                {
                    MonitorConexion.TryRemove(mEndPoint.Id, out DateTime _);
                }

                FlowConnectionsTimeOut.Remove(mEndPoint.Id);
            }
        }

        //  private XmlDocument OuterXML { get; set; }
        //   private MemoryStream XMLQ { get; set; }


        SI_OS_ConcentradorATMService mWS;
        WsAlt.SI_BPM_CashOutGuipService mWSAlt;

        private void InitConcentradorWS(WsAmbiente WsAmbient, ConexMaster container)
        {
            TCPUtil.WsAmbientForOperation = WsAmbient;

            if (WsAmbient.Ambiente == "BZ") //(WsAmbient.WsCashOutGuip)
            {
                mWSAlt = new WsAlt.SI_BPM_CashOutGuipService();
                mWSAlt.Credentials = new NetworkCredential()
                {
                    UserName = TCPUtil.WsAmbientForOperation.UserAtm,
                    Password = TCPUtil.WsAmbientForOperation.PwdAtm
                };
            }
            if (WsAmbient.Ambiente == "BA")
            {
                mWS = new SI_OS_ConcentradorATMService();
                mWS.Credentials = new NetworkCredential()
                {
                    UserName = TCPUtil.WsAmbientForOperation.UserAtm,
                    Password = TCPUtil.WsAmbientForOperation.PwdAtm
                };
            }


            ISOTransl.TryGetValue(FlowServer.TypeMsg, out string[] OrigenISOTransl);

            var headerSerialized = JsonSerializer.Serialize(WsAmbient);
            var ValF0F35  = OrigenISOTransl[35].Substring(24, 1);

            var payloadSerialized = "F0:" + ValF0F35 + " " +
            "F1:" + MTIRequest + " " +
            "F2:" + OrigenISOTransl[2] + " " +
            "F3:" + OrigenISOTransl[3] + " " +
            "F4:" + OrigenISOTransl[4] + " " +
            "F7:" + OrigenISOTransl[7] + " " +
            "F11:" + OrigenISOTransl[11] + " " +
            "F19:" + OrigenISOTransl[19] + " " +
            "F25:" + OrigenISOTransl[25] + " " +
            "F32:" + OrigenISOTransl[32] + " " +
            "F35:" + OrigenISOTransl[35] + " " +
            "F37:" + OrigenISOTransl[37] + " " +
            "F38:" + OrigenISOTransl[38] + " " +
            "F41:" + OrigenISOTransl[41] + " " +
            "F42:" + OrigenISOTransl[42] + " " +
            "F43:" + OrigenISOTransl[43] + " " +
            "F49:" + OrigenISOTransl[49] + " " +
            "F63:" + OrigenISOTransl[63] + " ";


            SaveLog(
                $"{nameof(InitConcentradorWS)} Header -> {headerSerialized} Payload -> {payloadSerialized}",
                container.Id,
                false);
        }

        private Task SendWsTransaction(SI_OS_ConcentradorATMService Ws, DT_ATM DT, int containerId, Boolean WaitForResponse = true)
        {
            if (WaitForResponse)
            {
                Ws.SI_OS_ConcentradorATMCompleted += Ws_SI_OS_ConcentradorATMCompleted;
            }

            var dtSerialized = JsonSerializer.Serialize(DT);
            SaveLog($"{nameof(SendWsTransaction)} DT -> {dtSerialized}", containerId, false);

            return Task.Run(() => Ws.SI_OS_ConcentradorATMAsync(DT));
        }

        private  Task SendWsTransaction(WsAlt.SI_BPM_CashOutGuipService Wsm, WsAlt.DT_CashOutGuip DTG, Boolean WaitForResponse = true)
        {
            if (WaitForResponse)
            {
                Wsm.SI_BPM_CashOutGuipCompleted += Wsm_SI_BPM_CashOutGuipCompleted;
            }

             return Task.Run(() => Wsm.SI_BPM_CashOutGuipAsync(DTG));
        }

        private void Wsm_SI_BPM_CashOutGuipCompleted(object sender, WsAlt.SI_BPM_CashOutGuipCompletedEventArgs e)
        {
            WsAlt.SI_BPM_CashOutGuipService Ws = sender as WsAlt.SI_BPM_CashOutGuipService;

            ConexWs.TryGetValue(Ws.Guido, out ConexMaster mEndPoint);
            ISOTransl.TryGetValue(FlowServer.TypeMsg, out String[] OrigenISOTransl);
            ConexF35.TryGetValue(Ws.Guido, out String ValF0F35);

            Int32 Id_Transaction = 0;
            if (MTIRequest == "0400")
                DBUtil.GetTransaction(OrigenISOTransl[37], OrigenISOTransl[41], OrigenISOTransl[2], out Id_Transaction);

            if (e.Result != null)
            {
                if (FlowConnectionsTimeOut.ContainsKey(mEndPoint.Id))
                {
                    Byte[] Data = null;
                    try
                    {
                        ISO8583 Helper = new ISO8583(mEndPoint.TypeMsg);
                        String BuildISO = Helper.Build(OrigenISOTransl, OrigenISOTransl[129]);
                        Data = TCPUtil.ConvertHexStringToByteArray(UtilFlowOperation.GetNetWorkHeader(mEndPoint, BuildISO));

                        Boolean EnTiempo = CheckInTime(mEndPoint, Data);

                        if (MTIResponse == "0210" && e.Result.AtmCashOut.F39 == "00") //Solo transacciones Exitosas
                        {
                            String nM = "";
                            if (e.Result.AtmCashOut.F4 == null)
                            {
                                nM = "0.00";
                            }
                            else
                            {
                                nM = e.Result.AtmCashOut.F4.Remove(e.Result.AtmCashOut.F4.Length - 2, 2);
                                nM += ".";
                                nM += e.Result.AtmCashOut.F4.Substring(e.Result.AtmCashOut.F4.Length - 2);
                            }


                            DBUtil.InsertTransaction(NetManager.Id, mEndPoint.Id, "0210", ValF0F35,
                                OrigenISOTransl[2],
                                e.Result.AtmCashOut.F37,
                                e.Result.AtmCashOut.F38,
                                e.Result.AtmCashOut.F41,
                                Convert.ToDecimal(nM));
                        }

                        HandleWs_CATM_Response(e.Result, Id_Transaction, EnTiempo == false);
                        CompleteFlow();

                    }
                    catch (Exception)
                    {

                    }
                }
                else
                {
                    if (MTIResponse == "0210" && e.Result.AtmCashOut.F39 == "00") //Solo transacciones Exitosas
                    {
                        String nM = "";
                        if (e.Result.AtmCashOut.F4 == null)
                        {
                            nM = "0.00";
                        }
                        else
                        {
                            nM = e.Result.AtmCashOut.F4.Remove(e.Result.AtmCashOut.F4.Length - 2, 2);
                            nM += ".";
                            nM += e.Result.AtmCashOut.F4.Substring(e.Result.AtmCashOut.F4.Length - 2);
                        }

                        DBUtil.InsertTransaction(NetManager.Id, mEndPoint.Id, "0210", ValF0F35,
                            OrigenISOTransl[2],
                            e.Result.AtmCashOut.F37,
                            e.Result.AtmCashOut.F38,
                            e.Result.AtmCashOut.F41,
                            Convert.ToDecimal(nM));
                    }
                    HandleWs_CATM_Response(e.Result, Id_Transaction);
                    CompleteFlow();
                }

            }
            else
            {
                UpdateConexIcon(FlowServer.EndConnectionMasterId.Id, 0);
                SaveLog("Error en la comunicacion con el Ws, TipoMsg:" + FlowServer.EndConnectionMasterId.TypeMsg + ", Detalle: respuesta del Ws Nula", FlowServer.EndConnectionMasterId.Id, true);
                SendResponseToOrigin("91", MTIResponse, false);
                CompleteFlow();
            }
        }

        private void Ws_SI_OS_ConcentradorATMCompleted(object sender, SI_OS_ConcentradorATMCompletedEventArgs e)
        {
            SI_OS_ConcentradorATMService Ws = sender as SI_OS_ConcentradorATMService;

            ConexWs.TryGetValue(Ws.Guido, out ConexMaster mEndPoint);
            ISOTransl.TryGetValue(FlowServer.TypeMsg, out String[] OrigenISOTransl);
            ConexF35.TryGetValue(Ws.Guido, out String ValF0F35);

            Int32 Id_Transaction = 0;
            if (MTIRequest == "0400")
                DBUtil.GetTransaction(OrigenISOTransl[37], OrigenISOTransl[41], OrigenISOTransl[2], out Id_Transaction);

            if (e.Result != null)
            {
                if (FlowConnectionsTimeOut.ContainsKey(mEndPoint.Id))
                {
                    Byte[] Data = null;
                    try
                    {
                        ISO8583 Helper = new ISO8583(mEndPoint.TypeMsg);
                        String BuildISO = Helper.Build(OrigenISOTransl, OrigenISOTransl[129]);
                        Data = TCPUtil.ConvertHexStringToByteArray(UtilFlowOperation.GetNetWorkHeader(mEndPoint, BuildISO));

                        Boolean EnTiempo = CheckInTime(mEndPoint, Data);

                        if (MTIResponse == "0210" && e.Result.AtmCashOut.F39 == "00") //Solo transacciones Exitosas
                        {
                            String nM = "";
                            if (e.Result.AtmCashOut.F4 == null)
                            {
                                nM = "0.00";
                            }
                            else
                            {
                                nM = e.Result.AtmCashOut.F4.Remove(e.Result.AtmCashOut.F4.Length - 2, 2);
                                nM += ".";
                                nM += e.Result.AtmCashOut.F4.Substring(e.Result.AtmCashOut.F4.Length - 2);
                            }


                            DBUtil.InsertTransaction(NetManager.Id, mEndPoint.Id, "0210", ValF0F35,
                                OrigenISOTransl[2],
                                e.Result.AtmCashOut.F37,
                                e.Result.AtmCashOut.F38,
                                e.Result.AtmCashOut.F41,
                                Convert.ToDecimal(nM));
                        }

                        HandleWs_CATM_Response(e.Result, Id_Transaction, EnTiempo == false);
                        CompleteFlow();

                    }
                    catch (Exception)
                    {

                    }
                }
                else
                {
                    if (MTIResponse == "0210" && e.Result.AtmCashOut.F39 == "00") //Solo transacciones Exitosas
                    {
                        String nM = "";
                        if (e.Result.AtmCashOut.F4 == null)
                        {
                            nM = "0.00";
                        }
                        else
                        {
                            nM = e.Result.AtmCashOut.F4.Remove(e.Result.AtmCashOut.F4.Length - 2, 2);
                            nM += ".";
                            nM += e.Result.AtmCashOut.F4.Substring(e.Result.AtmCashOut.F4.Length - 2);
                        }

                        DBUtil.InsertTransaction(NetManager.Id, mEndPoint.Id, "0210", ValF0F35,
                            OrigenISOTransl[2],
                            e.Result.AtmCashOut.F37,
                            e.Result.AtmCashOut.F38,
                            e.Result.AtmCashOut.F41,
                            Convert.ToDecimal(nM));
                    }
                    HandleWs_CATM_Response(e.Result, Id_Transaction);
                    CompleteFlow();
                }

            }
            else
            {
                UpdateConexIcon(FlowServer.EndConnectionMasterId.Id, 0);
                SaveLog("Error en la comunicacion con el Ws, TipoMsg:" + FlowServer.EndConnectionMasterId.TypeMsg + ", Detalle: respuesta del Ws Nula", FlowServer.EndConnectionMasterId.Id, true);
                SendResponseToOrigin("91", MTIResponse, false);
                CompleteFlow();
            }

        }

        private void HandleWs_CATM_Response(DT_ATMResponse Result, Int32 IdTrans = 0, Boolean OutOfTime = false)
        {
            if (OutOfTime)
            {
                SaveLog("WS RES Codigo ->" + Result.AtmCashOut.F39 + " Descripcion -> " + Result.Estado.descripcion + " Tipo -> " + Result.Estado.tipo + " Fuera de Tiempo", FlowServer.EndConnectionMasterId.Id, true);

                if (MTIRequest == "0200" && Result.AtmCashOut.F39 == "00")
                {
                    ISOTransl.TryGetValue(FlowServer.TypeMsg, out String[] OrigenISOTransl);

                    String ValF0F35 = "0";
                    Pair<String, String> Transaction;
                    Int32 Id_Transaction = 0;
                    Boolean MissingF35 = true;

                    MissingF35 = true;
                    OrigenISOTransl[35] = "0000000000000000=0000000000000000000";
                    ValF0F35 = "0";

                    Transaction = DBUtil.GetTransaction(OrigenISOTransl[37], OrigenISOTransl[41], OrigenISOTransl[2], out Id_Transaction);

                    ValF0F35 = Transaction.First;
                    OrigenISOTransl[38] = Transaction.Second;

                    if (MissingF35)
                    {
                        SaveLog(UtilFlowOperation.GetISOLabel(true, "0400", OrigenISOTransl, "Reverso 0400", ValF0F35), FlowServer.EndConnectionMasterId.Id, false);
                    }



                    var Dt_Object = TCPUtil.GetDTATM(new DT_ATM_CashOut()
                    {
                        F0 = ValF0F35,
                        F1 = "0400",
                        F2 = OrigenISOTransl[2],
                        F3 = OrigenISOTransl[3],
                        F4 = OrigenISOTransl[4],
                        F7 = OrigenISOTransl[7],
                        F11 = OrigenISOTransl[11],
                        F19 = OrigenISOTransl[19],
                        F25 = OrigenISOTransl[25],
                        F32 = OrigenISOTransl[32],
                        F35 = OrigenISOTransl[35],
                        F37 = OrigenISOTransl[37],
                        F38 = OrigenISOTransl[38],
                        F41 = OrigenISOTransl[41],
                        F42 = OrigenISOTransl[42],
                        F43 = OrigenISOTransl[43],
                        F49 = OrigenISOTransl[49],
                        F63 = OrigenISOTransl[63]
                    });

                    TCPUtil.WsAmbientes.TryGetValue(OrigenISOTransl[41].Substring(0, 2), out WsAmbiente WsAmbient);

                    InitConcentradorWS(WsAmbient, FlowServer);

                    Task S = SendWsTransaction(mWS, Dt_Object, FlowServer.EndConnectionMasterId.Id, false);                    
                }
            }
            else
            {
                SaveLog("WS RES Codigo ->" + Result.AtmCashOut.F39 + " Descripcion -> " + Result.Estado.descripcion + " Tipo -> " + Result.Estado.tipo + " En Tiempo", FlowServer.EndConnectionMasterId.Id, true);
            }

            String[] BaseISO = new String[130];
            BaseISO[2] = Result.AtmCashOut.F2 == "" ? null : Result.AtmCashOut.F2;
            BaseISO[3] = Result.AtmCashOut.F3 == "" ? null : Result.AtmCashOut.F3;//Numero debe de ir 6 digitos
            BaseISO[4] = Result.AtmCashOut.F4 == "" ? null : Result.AtmCashOut.F4;
            BaseISO[7] = Result.AtmCashOut.F7 == "" ? null : Result.AtmCashOut.F7;
            BaseISO[11] = Result.AtmCashOut.F11 == "" ? null : Result.AtmCashOut.F11;
            BaseISO[19] = Result.AtmCashOut.F19 == "" ? null : Result.AtmCashOut.F19;
            BaseISO[25] = Result.AtmCashOut.F25 == "" ? null : Result.AtmCashOut.F25;
            BaseISO[32] = Result.AtmCashOut.F32 == "" ? null : Result.AtmCashOut.F32;
            BaseISO[35] = Result.AtmCashOut.F35 == "" ? null : Result.AtmCashOut.F35;
            BaseISO[37] = Result.AtmCashOut.F37 == "" ? null : Result.AtmCashOut.F37;
            BaseISO[38] = Result.AtmCashOut.F38 == "" ? null : Result.AtmCashOut.F38;
            BaseISO[39] = Result.AtmCashOut.F39 == "" ? null : Result.AtmCashOut.F39;
            BaseISO[41] = Result.AtmCashOut.F41 == "" ? null : Result.AtmCashOut.F41;
            BaseISO[42] = Result.AtmCashOut.F42 == "" ? null : Result.AtmCashOut.F42;
            BaseISO[49] = Result.AtmCashOut.F49 == "" ? null : Result.AtmCashOut.F49;
            BaseISO[63] = Result.AtmCashOut.F63 == "" ? null : Result.AtmCashOut.F63;

            if (BaseISO[35] == "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF")
            {
                BaseISO[35] = null;
            }

            String F2 = BaseISO[2] ?? "-";
            String F3 = BaseISO[3] ?? "-";
            String F4 = BaseISO[4] ?? "-";
            String F7 = BaseISO[7] ?? "-";
            String F11 = BaseISO[11] ?? "-";
            String F19 = BaseISO[19] ?? "-";
            String F25 = BaseISO[25] ?? "-";
            String F32 = BaseISO[32] ?? "-";
            String F35 = BaseISO[35] ?? "-";
            String F37 = BaseISO[37] ?? "-";
            String F38 = BaseISO[38] ?? "-";
            String F39 = BaseISO[39] ?? "-";
            String F41 = BaseISO[41] ?? "-";
            String F42 = BaseISO[42] ?? "-";
            String F43 = BaseISO[43] ?? "-";
            String F49 = BaseISO[49] ?? "-";
            String F63 = BaseISO[63] ?? "-";

            String Log4Ui = OutOfTime ? "Transaccion Fuera de Tiempo " : "Transaccion en Tiempo ";

            Log4Ui += "Campos ISO8583 desde Ws " +
                       "RES -> " +
                       "F2:" + F2 + " " +
                          "F3:" + F3 + " " +
                          "F4:" + F4 + " " +
                          "F7:" + F7 + " " +
                          "F11:" + F11 + " " +
                          "F19:" + F19 + " " +
                          "F25:" + F25 + " " +
                          "F32:" + F32 + " " +
                          "F35:" + F35 + " " +
                          "F37:" + F37 + " " +
                          "F38:" + F38 + " " +
                          "F39:" + F39 + " " +
                          "F41:" + F41 + " " +
                          "F42:" + F42 + " " +
                          "F43:" + F43 + " " +
                          "F49:" + F49 + " " +
                          "F63:" + F63;

            SaveLog(Log4Ui, FlowServer.EndConnectionMasterId.Id, false);

            if (OutOfTime) return;

            if (Result.AtmCashOut.F39 != "00")
            {
                UpdateConexIcon(FlowServer.EndConnectionMasterId.Id, 0);
                SendResponseToOrigin(Result.AtmCashOut.F39, MTIResponse, false);
            }
            else
            {
                if (MTIRequest == "0400" && IdTrans > 0)
                {
                    DBUtil.ExecuteSQL("UPDATE Transactions SET Reversed = 1 WHERE Id=" + IdTrans);
                }
                ISO8583 Helper = new ISO8583(FlowServer.TypeMsg);

                String BuildIso = Helper.Build(BaseISO, MTIResponse);

                SaveLog("Cadena Build ISO8583 desde Ws -> " + BuildIso, FlowServer.EndConnectionMasterId.Id, false);

                try
                {
                    String[] ResponseFieldIso8583 = new String[130];
                    ResponseFieldIso8583 = Helper.Parse(BuildIso, false);

                    ISORespon.Add(FlowServer.TypeMsg, ResponseFieldIso8583);

                    UpdateConexIcon(FlowServer.EndConnectionMasterId.Id, 0);

                    SendResponseToOrigin("", MTIResponse, true, null, "");

                }
                catch (Exception ex)
                {
                    SaveLog("Cadena de respuesta mal formada, TipoMsg:" + FlowServer.EndConnectionMasterId.TypeMsg + ", ex:" + ex.Message, FlowServer.EndConnectionMasterId.Id, true);
                    SendResponseToOrigin("91", MTIResponse, false);
                    UpdateConexIcon(FlowServer.EndConnectionMasterId.Id, 0);
                }
            }

        }

        private void HandleWs_CATM_Response(WsAlt.DT_CashOutGuipResponse Result, Int32 IdTrans = 0, Boolean OutOfTime = false)
        {
            if (OutOfTime)
            {
                SaveLog("WS RES Codigo ->" + Result.AtmCashOut.F39 + " Descripcion -> " + Result.Estado.descripcion + " Tipo -> " + Result.Estado.tipo + " Fuera de Tiempo", FlowServer.EndConnectionMasterId.Id, true);

                if (MTIRequest == "0200" && Result.AtmCashOut.F39 == "00")
                {
                    ISOTransl.TryGetValue(FlowServer.TypeMsg, out String[] OrigenISOTransl);

                    String ValF0F35 = "0";
                    Pair<String, String> Transaction;
                    Int32 Id_Transaction = 0;
                    Boolean MissingF35 = true;

                    MissingF35 = true;
                    OrigenISOTransl[35] = "0000000000000000=0000000000000000000";
                    ValF0F35 = "0";

                    Transaction = DBUtil.GetTransaction(OrigenISOTransl[37], OrigenISOTransl[41], OrigenISOTransl[2], out Id_Transaction);

                    ValF0F35 = Transaction.First;
                    OrigenISOTransl[38] = Transaction.Second;

                    if (MissingF35)
                    {
                        SaveLog(UtilFlowOperation.GetISOLabel(true, "0400", OrigenISOTransl, "Reverso 0400", ValF0F35), FlowServer.EndConnectionMasterId.Id, false);
                    }

                    TCPUtil.WsAmbientes.TryGetValue(OrigenISOTransl[41].Substring(0, 2), out WsAmbiente WsAmbient);
                    InitConcentradorWS(WsAmbient, FlowServer);

                    //if (WsAmbient.WsCashOutGuip)
                    //{
                    //    var DT_ALT_Object = TCPUtil.GetDTAtmAlt(new WsAlt.DT_CashOut()
                    //    {
                    //        F0 = ValF0F35,
                    //        F1 = "0400",
                    //        F2 = OrigenISOTransl[2],
                    //        F3 = OrigenISOTransl[3],
                    //        F4 = OrigenISOTransl[4],
                    //        F7 = OrigenISOTransl[7],
                    //        F11 = OrigenISOTransl[11],
                    //        F19 = OrigenISOTransl[19],
                    //        F25 = OrigenISOTransl[25],
                    //        F32 = OrigenISOTransl[32],
                    //        F35 = OrigenISOTransl[35],
                    //        F37 = OrigenISOTransl[37],
                    //        F38 = OrigenISOTransl[38],
                    //        F41 = OrigenISOTransl[41],
                    //        F42 = OrigenISOTransl[42],
                    //        F43 = OrigenISOTransl[43],
                    //        F49 = OrigenISOTransl[49],
                    //        F63 = OrigenISOTransl[63]
                    //    });

                    //    Task S = SendWsTransaction(mWSAlt, DT_ALT_Object, false);
                    //}
                    //else
                    {
                        var Dt_Object = TCPUtil.GetDTATM(new DT_ATM_CashOut()
                        {
                            F0 = ValF0F35,
                            F1 = "0400",
                            F2 = OrigenISOTransl[2],
                            F3 = OrigenISOTransl[3],
                            F4 = OrigenISOTransl[4],
                            F7 = OrigenISOTransl[7],
                            F11 = OrigenISOTransl[11],
                            F19 = OrigenISOTransl[19],
                            F25 = OrigenISOTransl[25],
                            F32 = OrigenISOTransl[32],
                            F35 = OrigenISOTransl[35],
                            F37 = OrigenISOTransl[37],
                            F38 = OrigenISOTransl[38],
                            F41 = OrigenISOTransl[41],
                            F42 = OrigenISOTransl[42],
                            F43 = OrigenISOTransl[43],
                            F49 = OrigenISOTransl[49],
                            F63 = OrigenISOTransl[63]
                        });

                        Task S = SendWsTransaction(mWS, Dt_Object, FlowServer.EndConnectionMasterId.Id, false);
                    }
                }
            }
            else
            {
                SaveLog("WS RES Codigo ->" + Result.AtmCashOut.F39 + " Descripcion -> " + Result.Estado.descripcion + " Tipo -> " + Result.Estado.tipo + " En Tiempo", FlowServer.EndConnectionMasterId.Id, true);
            }

            String[] BaseISO = new String[130];
            BaseISO[2] = Result.AtmCashOut.F2 == "" ? null : Result.AtmCashOut.F2;
            BaseISO[3] = Result.AtmCashOut.F3 == "" ? null : Result.AtmCashOut.F3;//Numero debe de ir 6 digitos
            BaseISO[4] = Result.AtmCashOut.F4 == "" ? null : Result.AtmCashOut.F4;
            BaseISO[7] = Result.AtmCashOut.F7 == "" ? null : Result.AtmCashOut.F7;
            BaseISO[11] = Result.AtmCashOut.F11 == "" ? null : Result.AtmCashOut.F11;
            BaseISO[19] = Result.AtmCashOut.F19 == "" ? null : Result.AtmCashOut.F19;
            BaseISO[25] = Result.AtmCashOut.F25 == "" ? null : Result.AtmCashOut.F25;
            BaseISO[32] = Result.AtmCashOut.F32 == "" ? null : Result.AtmCashOut.F32;
            BaseISO[35] = Result.AtmCashOut.F35 == "" ? null : Result.AtmCashOut.F35;
            BaseISO[37] = Result.AtmCashOut.F37 == "" ? null : Result.AtmCashOut.F37;
            BaseISO[38] = Result.AtmCashOut.F38 == "" ? null : Result.AtmCashOut.F38;
            BaseISO[39] = Result.AtmCashOut.F39 == "" ? null : Result.AtmCashOut.F39;
            BaseISO[41] = Result.AtmCashOut.F41 == "" ? null : Result.AtmCashOut.F41;
            BaseISO[42] = Result.AtmCashOut.F42 == "" ? null : Result.AtmCashOut.F42;
            BaseISO[49] = Result.AtmCashOut.F49 == "" ? null : Result.AtmCashOut.F49;
            BaseISO[63] = Result.AtmCashOut.F63 == "" ? null : Result.AtmCashOut.F63;

            if (BaseISO[35] == "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF")
            {
                BaseISO[35] = null;
            }

            String F2 = BaseISO[2] ?? "-";
            String F3 = BaseISO[3] ?? "-";
            String F4 = BaseISO[4] ?? "-";
            String F7 = BaseISO[7] ?? "-";
            String F11 = BaseISO[11] ?? "-";
            String F19 = BaseISO[19] ?? "-";
            String F25 = BaseISO[25] ?? "-";
            String F32 = BaseISO[32] ?? "-";
            String F35 = BaseISO[35] ?? "-";
            String F37 = BaseISO[37] ?? "-";
            String F38 = BaseISO[38] ?? "-";
            String F39 = BaseISO[39] ?? "-";
            String F41 = BaseISO[41] ?? "-";
            String F42 = BaseISO[42] ?? "-";
            String F43 = BaseISO[43] ?? "-";
            String F49 = BaseISO[49] ?? "-";
            String F63 = BaseISO[63] ?? "-";

            String Log4Ui = OutOfTime ? "Transaccion Fuera de Tiempo " : "Transaccion en Tiempo ";

            Log4Ui += "Campos ISO8583 desde Ws " +
                       "RES -> " +
                       "F2:" + F2 + " " +
                          "F3:" + F3 + " " +
                          "F4:" + F4 + " " +
                          "F7:" + F7 + " " +
                          "F11:" + F11 + " " +
                          "F19:" + F19 + " " +
                          "F25:" + F25 + " " +
                          "F32:" + F32 + " " +
                          "F35:" + F35 + " " +
                          "F37:" + F37 + " " +
                          "F38:" + F38 + " " +
                          "F39:" + F39 + " " +
                          "F41:" + F41 + " " +
                          "F42:" + F42 + " " +
                          "F43:" + F43 + " " +
                          "F49:" + F49 + " " +
                          "F63:" + F63;

            SaveLog(Log4Ui, FlowServer.EndConnectionMasterId.Id, false);

            if (OutOfTime) return;

            if (Result.AtmCashOut.F39 != "00")
            {
                UpdateConexIcon(FlowServer.EndConnectionMasterId.Id, 0);
                SendResponseToOrigin(Result.AtmCashOut.F39, MTIResponse, false);
            }
            else
            {
                if (MTIRequest == "0400" && IdTrans > 0)
                {
                    DBUtil.ExecuteSQL("UPDATE Transactions SET Reversed = 1 WHERE Id=" + IdTrans);
                }
                ISO8583 Helper = new ISO8583(FlowServer.TypeMsg);

                String BuildIso = Helper.Build(BaseISO, MTIResponse);

                SaveLog("Cadena Build ISO8583 desde Ws -> " + BuildIso, FlowServer.EndConnectionMasterId.Id, false);

                try
                {
                    String[] ResponseFieldIso8583 = new String[130];
                    ResponseFieldIso8583 = Helper.Parse(BuildIso, false);

                    ISORespon.Add(FlowServer.TypeMsg, ResponseFieldIso8583);

                    UpdateConexIcon(FlowServer.EndConnectionMasterId.Id, 0);

                    SendResponseToOrigin("", MTIResponse, true, null, "");

                }
                catch (Exception ex)
                {
                    SaveLog("Cadena de respuesta mal formada, TipoMsg:" + FlowServer.EndConnectionMasterId.TypeMsg + ", ex:" + ex.Message, FlowServer.EndConnectionMasterId.Id, true);
                    SendResponseToOrigin("91", MTIResponse, false);
                    UpdateConexIcon(FlowServer.EndConnectionMasterId.Id, 0);
                }
            }

        }

        private Boolean CheckInTime(ConexMaster mEndPoint, Byte[] Data)
        {
            Boolean inTime = false;
            if (MonitorConexion.ContainsKey(mEndPoint.Id))
            {
                inTime = true;
                SaveLog("Respuesta recibida en tiempo, termina timer de espera", mEndPoint.Id, false);
                MonitorConexion.TryRemove(mEndPoint.Id, out DateTime _);
                KillEndPoint(mEndPoint);
            }
            else
            {
                inTime = false;
                if (Data != null)
                {
                    if (mEndPoint.ConnectionType == 4)
                    {
                        SaveLog("Conexion " + mEndPoint.DescriptionName + " respuesta fuera de tiempo, no es posible continuar flujo", mEndPoint.Id, true);
                    }
                    else
                    {
                        SaveLog("Conexion " + mEndPoint.DescriptionName + "(" + mEndPoint.IPAdress + ":" + mEndPoint.Port + ") respuesta fuera de tiempo " + BitConverter.ToString(Data).Replace("-", "") + ", no es posible continuar flujo", mEndPoint.Id, true);
                    }
                }
                else
                {
                    SaveLog("Conexion " + mEndPoint.DescriptionName + "(" + mEndPoint.IPAdress + ":" + mEndPoint.Port + ") respuesta fuera de tiempo ---, no es posible continuar flujo", mEndPoint.Id, true);
                }

                KillEndPoint(mEndPoint);
            }
            return inTime;
        }



        #endregion


        #region Monitor
        private ConcurrentDictionary<Int32, DateTime> MonitorConexion { get; set; }
        private CancellationTokenSource _TokenBMPSource = new CancellationTokenSource();
        private CancellationToken _TokenBMP;

        /// <summary>
        /// Monitor para detectar si una conexion entro en tiempo.
        /// </summary>
        /// <returns></returns>
        private async Task MonitorForConexion()
        {
            while (!_TokenBMP.IsCancellationRequested)
            {
                if (MonitorConexion.Count > 0)
                {
                    MonitorForConexionTask();
                }
                await Task.Delay(3000, _TokenBMP);
                Application.DoEvents();
            }
        }

        private void MonitorForConexionTask()
        {
            DateTime idleTimestamp = DateTime.Now;//.AddSeconds(-1 * _NetManager.TimeOut);

            foreach (KeyValuePair<Int32, DateTime> curr in MonitorConexion)
            {
                if (idleTimestamp > curr.Value)//    curr.Value < idleTimestamp)
                {
                    DateTime Outer = DateTime.Now;

                    var Cnx = EndPoints.Where(c => c.Conex.Id == curr.Key).FirstOrDefault();

                    if (Cnx != null)
                    {
                        SaveLog("Monitor de conexion cerrado, no se recibio respuesta desde IP- " + Cnx.Conex.IPAdress + ":" + Cnx.Conex.Port, FlowServer.Id, true);
                    }
                    else
                    {
                        ConexMaster mEndPoint = new ConexMaster(curr.Key, 0);
                        SaveLog("Monitor de conexion cerrado, no se recibio respuesta desde Ws " + mEndPoint.DescriptionName, FlowServer.Id, true);
                    }

                    SendResponseToOrigin("91", MTIResponse, false);
                    MonitorConexion.TryRemove(curr.Key, out DateTime _);

                    Application.DoEvents();
                    CompleteFlow();
                }
            }
        }



        #endregion

    }
}
