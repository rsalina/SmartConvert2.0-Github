
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TCPSmart.Conexion;

namespace TCPSmart.Flow
{
    public class SmartFlow
    {
        public NetManager mNetManager;

        public Boolean ValidNetManager;
        public String LogForUi = "";

        public Action<String> Messenger = null;
        //private Dictionary<String,ForFlowContainer> Cola { get; set; }
        private Dictionary<String, FlowOperations> Cola { get; set; }
        public SmartFlow(NetManager rNetManager)
        {
            mNetManager = rNetManager;
            InitServer();
        }
        private void GetMsjFromFlow(String Mensaje, Boolean ForUI)
        {
            if (ForUI)
            {
                Messenger?.Invoke(Mensaje);
            }
        }

        private void InitServer()
        {
            mNetManager.ServerNetManager.DataRecibida += taskServerNetManager;
            mNetManager.ServerNetManager.ClienteDesconectado += ServerNetManager_ClienteDesconectado;
            mNetManager.ServerNetManager.ClienteConectado += ServerNetManager_ClienteConectado;
            mNetManager.ServerNetManager.Log = RecordLog;
            //------------------------------------------------------------------

            //------------------------------------------------------------------
            try
            {

                mNetManager.ServerNetManager.Start();
                AddItemRowManagerStat(mNetManager.ConexMServer);
                updateIcon(mNetManager.ConexMServer.Id, 0);

                ConexMaster Dop = mNetManager.ConexMServer;

                //if (mNetManager.WK_ID != String.Empty) //Se Agrega validador en caso de existir al monitor
                //{
                //    TCPUtil.Wk_ID = mNetManager.WK_ID;
                //    TCPUtil.Pin_Ver_Req = "000001"; // ?De Donde Sale o dejar fijo
                //}

                do
                {
                    Dop = Dop.EndConnectionMasterId;
                    AddItemRowManagerStat(Dop);
                    updateIcon(Dop.Id, 0);

                } while (Dop.EndConnectionMasterId != null);


                ValidNetManager = true;

                if (ValidNetManager)
                {
                    LogForUi = mNetManager.ConexMServer.DescriptionName + " entra en funcion, esperando conexiones en (" + mNetManager.ConexMServer.IPAdress + ":" + mNetManager.ConexMServer.Port + ") " + TCPUtil.AppName;
                    RecordLog(LogForUi, "-", mNetManager.ConexMServer.Id, true);

                    //Cola = new Dictionary<String, ForFlowContainer>();
                    Cola = new Dictionary<String, FlowOperations>();
                }
            }
            catch (Exception ex)
            {
                ValidNetManager = false;
                String Horror = "Ingrese el nombre del adaptador correcto, en el modulo de parametros, no es posible iniciar el netManager " + mNetManager.DescriptionName;
                LogForUi = Horror + Environment.NewLine + ex.Message;
                RecordLog(Horror, "-", mNetManager.ConexMServer.Id, true);
                LogForUi = "";
            }
        }

        private void ServerNetManager_ClienteConectado(object sender, ClientConEventArgs e)
        {
            LogForUi = e.IPAddress + " conectado, esperando conexiones en (" + mNetManager.ConexMServer.IPAdress + ":" + mNetManager.ConexMServer.Port + ") ";
            RecordLog(LogForUi, "-", mNetManager.ConexMServer.Id, true);
        }

        private void ServerNetManager_ClienteDesconectado(object sender, ClientDesConexEventArgs e)
        {
            if (e.Motivo == DesconexionType.Timeout)
            {
                if (mNetManager.Zesiones.ContainsKey(e.IPAddress))
                {
                    mNetManager.Zesiones.TryGetValue(e.IPAddress, out Int32 Sezion);
                    RecordLog("<<<<<<<<<<<<<<<<<<<<<<<<<<<< " + mNetManager.DescriptionName + ":-> TERMINA SESION " + Sezion + "(TimeOut)>>>>>>>>>>>>>>>>>>>>>>>>>>>>", e.IPAddress, mNetManager.ConexMServer.Id, false);
                }
            }
        }

        /// <summary>
        /// 1er Step en el flujo
        /// </summary>
        /// <param name="Data"></param>
        private void ServerNetManager_DataRecibida(FromClienteDataReceived Data)
        {
            if (SetupData(Data.DataOrigen, Data.IPAddress)) //Data es un ISO
            {
                // Cola.TryGetValue(Data.IPAddress, out ForFlowContainer Flow);
                Cola.TryGetValue(Data.IPAddress, out FlowOperations Flow);

                Flow.UpdateServerStatic(0, 1); Flow.UpdateServerIcon(75);

                if (Flow.MTIRequest != "0800")
                {
                    String L = "Inicia conexion en " + Flow.FlowServer.DescriptionName + " desde -> " + Flow.IPCliente + " REQ: " + Flow.MTIRequest;
                    Flow.SaveLog(L, Flow.FlowServer.Id, true);

                    Flow.Tz = new TaskCompletionSource<Boolean>();
                    Task<Boolean> t1 = Flow.Tz.Task;

                    Task.Factory.StartNew(() =>
                    {
                        //Flow.HandleServerFlow();
                        Flow.HandleFlow();
                    });

                    t1.Wait();

                    Flow.UpdateServerIcon(0);

                    Cola.Remove(Data.IPAddress);
                }

                mNetManager.Zesiones.TryGetValue(Data.IPAddress, out Int32 Sezion);

                Flow.SaveLog("<<<<<<<<<<<<<<<<<<<<<<<<<<<< " + mNetManager.DescriptionName + ":-> TERMINA SESION " + Sezion + "(" + Flow.MTIRequest + ") -> (" + Flow.MTIResponse + ")>>>>>>>>>>>>>>>>>>>>>>>>>>>>", Flow.FlowServer.Id, false);

            }
            else //Data no es un ISO
            {
                if (LogForUi != "")
                {
                    LogForUi = "";
                }
            }
        }

        private void taskServerNetManager(FromClienteDataReceived Data)
        {           
            Thread tareaPrincipal = new Thread(() =>
            {
                ServerNetManager_DataRecibida(Data);
            });
            tareaPrincipal.Start();
        }
        private Boolean SetupData(Byte[] ReceivedData, String IPCliente)
        {
            Boolean Valid = false;

            try
            {
                //ForFlowContainer Flujo = new ForFlowContainer(mNetManager, ReceivedData, IPCliente)
                //{
                //    Loger = GetMsjFromFlow
                //};

                FlowOperations Flujo = new FlowOperations(mNetManager, ReceivedData, IPCliente)
                {
                    Loger = GetMsjFromFlow
                };

                if (Cola.ContainsKey(IPCliente))
                    Cola.Remove(IPCliente);

                Cola.Add(IPCliente, Flujo);

                if (Flujo.MTIRequest == "0800")
                {
                    Valid = Flujo.SendResponseToOrigin("00", Flujo.MTIResponse, false);
                }
                else if (Flujo.MTIRequest == "0810")
                {
                    LogForUi = "Respuesta 0810 aceptada";
                    Messenger.Invoke(LogForUi);
                    Valid = false;
                }
                else
                {
                    Valid = Flujo.ISOParsed.Count > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                LogForUi = "El paquete recibido desde " + IPCliente + ", no es una cadena leible para SmartConvert, se ignorara";
                Messenger.Invoke(LogForUi);
                Valid = false;
            }
            return Valid;
        }

        #region HelperTools
        private void RecordLog(String Msj, String IP, Int32 ConexId, Boolean Exepcion, String Tipo = "Flow")
        {
            if (TCPUtil.ConexsUnderMonitor.ContainsKey(ConexId))
            {
                Messenger?.Invoke(Msj);

                if (mNetManager.Zesiones.ContainsKey(IP))
                {
                    mNetManager.Zesiones.TryGetValue(IP, out Int32 Sezion);
                    Task.Run(async () => await DBUtil.InsertBitacoraASync(Msj, ConexId, IP, Sezion + "_" + Tipo, mNetManager.Id, Sezion));
                }
                else
                {
                    Task.Run(async () => await DBUtil.InsertBitacoraASync(Msj, ConexId, IP, "Interno", mNetManager.Id));
                }


            }
            else
            {
                if (Exepcion)
                {
                    Messenger?.Invoke(Msj);
                }
            }
        }

        private void AddItemRowManagerStat(ConexMaster Conex)
        {
            if (TCPUtil.Statics.Where(c => c.ID == Conex.Id).Count() == 0)
            {
                TCPUtil.Statics.Add(new ConexStatics()
                {
                    ID = Conex.Id,
                    NetManagerID = Conex.NetManagerId,
                    Nombre = Conex.DescriptionName,
                    Status = 0,
                    Inicio = DateTime.Now,
                    UpTime = "00:00:00",
                    Recibidas = 0,
                    Enviadas = 0,
                    Tipo = Conex.TCPClient ? "EndPoint" : "Servidor"
                });
            }
        }

        private void updateIcon(Int32 Id, Int32 Value)
        {
            try
            {

                TCPUtil.Statics.Where(c => c.ID == Id).First().Status = Value;
                Application.DoEvents();

            }
            catch (Exception)
            {

            }

        }

        #endregion

    }
}
