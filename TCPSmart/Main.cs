using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TCPSmart.Conexion;
using TCPSmart.Flow;
using TCPSmart.Ws;

namespace TCPSmart
{
    public partial class Main : Form
    {
        private ConcurrentDictionary<Int32, Flow.NetManager> NetManagers = new ConcurrentDictionary<int, Flow.NetManager>();
        private ConexStatics _FocusRow;
        /*Agregado por José Palomino 17/03/2020*/
        //private string HSMManagerId = "";
        //ClienteTCP WsCliente;
        //private int codigoError = 0;
        //MemoryStream XML4Key;
       
        public Main()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void loaddata()
        {
            gridView1.Update();
            gridView1.Refresh();
        }
        private void SendLog(String Msj)
        {
            WriteSafetyLog(Msj);
            /*try
            {
                if (gridView1.Rows.Count > 0)
                {
                    loaddata();                  
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("..."+ex.Message);
            }*/

            Application.DoEvents();
        }

        private void WriteSafetyLog(String Msj)
        {
            loaddata();
            if (LstLog.InvokeRequired)
            {
                if (Msj == "***")
                {
                    LstLog.Invoke(new Action(() => LstLog.Items.Clear()));
                }
                else
                {
                    LstLog.Invoke(new Action(() => LstLog.Items.Add(Msj)));
                }
                // var d = new SafeWriteLog(WriteSafetyLog);
                int indice = LstLog.Items.Count - 1;
                LstLog.Invoke(new Action(() => LstLog.SelectedIndex = indice));

            }
            else
            {
                if (Msj == "***")
                {
                    LstLog.Items.Clear();

                }
                else
                {
                    LstLog.Items.Add(Msj);

                }
            }
        }

        //public async Task MonitorForDynamicKey()
        //{
        //    while (!_Token.IsCancellationRequested)
        //    {
        //        // MonitorForDinamycKeyTask();
        //        await Task.Delay(TCPUtil.delayExecution);
        //    }
        //}

        //#region "José Palomino"
        //private void MonitorForDinamycKeyTask()
        //{
        //    //DateTime idleTimestamp = DateTime.Now.AddSeconds(-1 * _IdleTimeOut);
        //    //DataTable dt = DBUtil.GetValueConsecutiveXML();
        //    //TCPUtil.tipo = "CCSQuery";
        //    //TCPUtil.primerTag = "req_wk_req";
        //    //TCPUtil.Req_wk_req = dt.Rows[0].ItemArray[0].ToString();
        //    //TCPUtil.segundoTag = "mk_id";
        //    //TCPUtil.valorSegundoTag = HSMManagerId.PadLeft(6, '0');

        //    XML4Key = TCPUtil.GenerarFileKey();
        //    /*Estos valores comentados simulan la conexión a la mulita para la obtención de datos,
        //     estos valores deben llenarse por el lado de conexiones, se asume que ya se tienen los valores.*/
        //    /*TCPUtil.ParamKeyXMLServer = "127.0.0.1";
        //    TCPUtil.ParamKeyXMLPort = 4567;
        //    TCPUtil.ParamKeyXMLTimeOut = 10000;*/
        //    WsCliente = new ClienteTCP(TCPUtil.ParamKeyXMLServer, TCPUtil.ParamKeyXMLPort, null);
        //    WsCliente.Conectado += WsCliente_Conectado;
        //    WsCliente.Desconectado += WsCliente_Desconectado;
        //    //WsCliente.DataReceived += WsCliente_DataRecibida;
        //    WsCliente.ConnectTimeoutSeconds = TCPUtil.ParamKeyXMLTimeOut;
        //    try
        //    {
        //        WsCliente.Connect(null); //<== Ya se puede enviar en null
        //        WsCliente.Send(XML4Key.ToArray());
        //    }
        //    catch (Exception)
        //    {
        //        //LstLog.Items.Add("No fue posible conectar con el Servidor destino, inténtelo luego.");
        //        // SendLog("No fue posible conectar con el Servidor destino, inténtelo luego.");
        //        //XtraMessageBox.Show("No fue posible conectar con el Servidor destino, inténtelo luego.", TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        WsCliente.Dispose();
        //    }
        //}

        //private void WsCliente_Desconectado(object sender, EventArgs e)
        //{
        //}
        //private void WsCliente_Conectado(object sender, EventArgs e)
        //{
        //}

        //private void WsCliente_DataRecibida(object sender, FromServerDataReceivedEventArgs e)
        //{
        //    try
        //    {
        //        XmlDocument Respuesta = new XmlDocument();
        //        MemoryStream Ms = new MemoryStream(e.Data);
        //        Respuesta.Load(Ms);
        //        String RC = Respuesta.LastChild.FirstChild.Attributes["rc"].Value;
        //        String codigoTrama = Respuesta.LastChild.FirstChild.Attributes["id"].Value;

        //        WsCliente.Dispose();
        //        if (codigoTrama.Equals(TCPUtil.Req_wk_req))
        //        {
        //            if (RC == "00")
        //            {
        //                XmlNodeList estructura = Respuesta.GetElementsByTagName("CCSQuery");
        //                XmlNodeList lista =
        //                    ((XmlElement)estructura[0]).GetElementsByTagName("req_wk_rep");

        //                foreach (XmlElement nodo in lista)
        //                {
        //                    int i = 0;
        //                    XmlNodeList wk_id =
        //                    nodo.GetElementsByTagName("wk_id");
        //                    XmlNodeList wk =
        //                    nodo.GetElementsByTagName("wk");
        //                    XmlNodeList wk_check =
        //                    nodo.GetElementsByTagName("wk_check");
        //                    TCPUtil.wk_id_Response = wk_id[i].InnerText;
        //                    TCPUtil.wk_Response = wk[i].InnerText;
        //                    TCPUtil.wk_check_Response = wk_check[i].InnerText;

        //                    TCPUtil.Wk_ID = TCPUtil.wk_id_Response; //<<=== Actualiza variable statica 

        //                    if (TCPUtil.wk_id_Response.Trim().Length > 16 || TCPUtil.wk_Response.Trim().Length > 32 || TCPUtil.wk_check_Response.Trim().Length > 16)
        //                    {
        //                        codigoError = 1; //El tamaño de alguno de los campos no corresponde con el configurado
        //                    }

        //                    // DBUtil.UpdateValueHSMDetails(TCPUtil.wk_Response, TCPUtil.wk_id_Response, TCPUtil.wk_check_Response, Convert.ToInt32(HSMManagerId));
        //                    i = i + 1;
        //                }
        //                //   DBUtil.UpdateConsecutiveXML();
        //                //XtraMessageBox.Show("Se actualizó la llave de manera satisfactoria.", TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        //                //LstLog.Items.Add("Se actualizó la llave de manera satisfactoria.");
        //                // SendLog("Se actualizó la llave de manera satisfactoria.");
        //                barButtonItem9.Text = "Bitacora - " + DateTime.Now.ToString();
        //            }
        //            else
        //            {
        //                //XtraMessageBox.Show("El código de respuesta RC es diferente de 00. El RC devuelto es: " + RC.ToString() + ". No se actualizará la llave.", TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //                //LstLog.Items.Add("El código de respuesta RC es diferente de 00. El RC devuelto es: " + RC.ToString() + ". No se actualizará la llave.");
        //                // SendLog("El código de respuesta RC es diferente de 00. El RC devuelto es: " + RC.ToString() + ". No se actualizará la llave.");
        //            }
        //        }
        //        else
        //        {
        //            //XtraMessageBox.Show("El ID del request del envio (" + TCPUtil.Req_wk_req + ") no corresponde con el ID del response devuelto (" + codigoTrama.ToString() + ") por el servicio.", TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //            //LstLog.Items.Add("El ID del request del envio (" + TCPUtil.Req_wk_req + ") no corresponde con el ID del response devuelto (" + codigoTrama.ToString() + ") por el servicio.");
        //            //SendLog("El ID del request del envio (" + TCPUtil.Req_wk_req + ") no corresponde con el ID del response devuelto (" + codigoTrama.ToString() + ") por el servicio.");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        if (codigoError == 1)
        //        {
        //            //XtraMessageBox.Show("Uno de los campos respuesta excede la longitud esperada, no se actualizará la llave.", TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            //LstLog.Items.Add("Uno de los campos respuesta excede la longitud esperada, no se actualizará la llave.");
        //            //SendLog("Uno de los campos respuesta excede la longitud esperada, no se actualizará la llave.");
        //        }
        //        else
        //        {
        //            //XtraMessageBox.Show("Ocurrió un error inesperado en la obtención de la llave, vuelva a intentarlo.", TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            //LstLog.Items.Add("Ocurrió un error inesperado en la obtención de la llave, vuelva a intentarlo.");
        //            // SendLog("Ocurrió un error inesperado en la obtención de la llave, vuelva a intentarlo.");
        //        }
        //    }
        //}
        // <summary>
        // Deprecated
        // </summary>
        // <param name = "idConnectionMaster" ></ param >
        private void ObtenerParametrosServidor(int idConnectionMaster)
        {
            DataTable dt = new DataTable();
            dt = DBUtil.GetConnectionMasterByID(idConnectionMaster);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    TCPUtil.ParamKeyXMLServer = item["IPAdress"].ToString();
                    TCPUtil.ParamKeyXMLPort = Convert.ToInt32(item["Port"].ToString());
                    TCPUtil.ParamKeyXMLTimeOut = Convert.ToInt32(item["TimeOutSecs"].ToString());
                }
            }
        }
        //#endregion
        private void iniciarServiciosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TCPUtil.ConexSQLReady(true))
            {
                Boolean StartSrv = true;
                var parameter = DBUtil.GetParameterByCode("LicenseToken");
                if (parameter.Rows.Count > 0)
                {
                    var token = parameter.Rows[0]["ParameterValue"].ToString();
                    if (!string.IsNullOrWhiteSpace(token))
                    {
                        var tokenData = JWT.ValidarJwtToken(token);
                        if (tokenData.Count > 0)
                        {
                            try
                            {
                                var expDate = (DateTime)tokenData["exp_datetime"];
                                if (DateTime.Now > expDate)
                                {
                                    StartSrv = false;
                                    MessageBox.Show($"Licencia vencida el {expDate.ToString("dd/MM/yyyy")}: debe aplicar una nueva licencia para continuar", TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                    barButtonItem1.PerformClick();
                                    return;
                                }
                            }
                            catch
                            {
                                StartSrv = false;
                                MessageBox.Show("No existe una licencia definida, favor aplicar una licencia para continuar", TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                barButtonItem1.PerformClick();
                                return;
                            }
                        }
                        else
                        {
                            StartSrv = false;
                            MessageBox.Show("No existe una licencia definida, favor aplicar una licencia para continuar", TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            barButtonItem1.PerformClick();
                            return;
                        }
                    }
                    else
                    {
                        StartSrv = false;
                        MessageBox.Show("No existe una licencia definida, favor aplicar una licencia para continuar", TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        barButtonItem1.PerformClick();
                        return;
                    }
                }
                else
                {
                    StartSrv = false;
                    MessageBox.Show("No existe una licencia definida, favor aplicar una licencia para continuar", TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    barButtonItem1.PerformClick();
                    return;
                }

                if (StartSrv)
                {
                    DataTable Wet = DBUtil.GetSQL("SELECT * FROM WsParams WHERE Active = 1");
                    if (Wet.Rows.Count > 0)
                    {
                        foreach (DataRow item in Wet.Rows)
                        {
                            if (StartSrv)
                            {
                                var token = item["LicToken"].ToString();

                                if (!string.IsNullOrWhiteSpace(token))
                                {
                                    var tokenData = JWT.ValidarJwtToken(token);

                                    if (tokenData.Count > 0)
                                    {
                                        try
                                        {
                                            var expDate = (DateTime)tokenData["exp_datetime"];
                                            if (DateTime.Now > expDate)
                                            {
                                                StartSrv = false;
                                                MessageBox.Show("En la conexion Ws " + item["Ambiente"].ToString() + $" su licencia venció el {expDate.ToString("dd /MM/yyyy")}: debe registrar una nueva licencia para continuar", TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                                return;
                                            }
                                        }
                                        catch
                                        {
                                            StartSrv = false;
                                            MessageBox.Show("La conexion Ws " + item["Ambiente"].ToString() + " no tiene una licencia valida, es necesario revisarla para continuar", TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        StartSrv = false;
                                        MessageBox.Show("La conexion Ws " + item["Ambiente"].ToString() + " no tiene definida una licencia, es necesario registrarla para continuar", TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                        return;
                                    }
                                }
                                else
                                {
                                    StartSrv = false;
                                    MessageBox.Show("La conexion Ws " + item["Ambiente"].ToString() + " no tiene definida una licencia, es necesario registrarla para continuar", TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                    return;
                                }
                            }
                        }
                    }

                }

                if (StartSrv)
                {

                    DataTable Xet = DBUtil.GetNetManager(); //<===== Obtenemos Las Instancias Net Manager
                    foreach (DataRow item in Xet.Rows)
                    {
                        try
                        {
                            Flow.NetManager mNetManager = new Flow.NetManager(Convert.ToInt32(item["Id"])); //Convertido a Objeto

                            SmartFlow sFlow = new SmartFlow(mNetManager)
                            {
                                Messenger = SendLog
                            };
                            if (sFlow.ValidNetManager)
                            {
                                Console.WriteLine("Valido");
                                NetManagers.TryAdd(mNetManager.Id, mNetManager);
                                SendLog(sFlow.LogForUi);
                            }
                            else
                            {
                                Console.WriteLine("NO Valido");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            SendLog(ex.Message);
                        }
                    }

                    if (NetManagers.Count > 0)
                    {
                        //barButtonItem11.Enabled = false;
                        barButtonItem2.Enabled = false;
                        barButtonItem3.Enabled = true;
                        barButtonItem13.Enabled = false;
                        barStaticItem1.Text = "Status: Servicios Iniciados";


                        gridView1.DataSource = TCPUtil.Statics;
                        //gridView1.BestFitColumns();

                        TCPUtil.SetupWsAmbientes();
                        operacionesToolStripMenuItem.Enabled = true;
                    }
                    else
                    {
                        MessageBox.Show("No se pueden iniciar los servicios, " + Environment.NewLine + "no hay ninguna conexion configurada, o son invalidas por favor revise.", TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }


                }
            }
            else
            {
                MessageBox.Show("Antes de iniciar los servicios, debes de configurar los parametros", TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
        }

        private void Main_Load(object sender, EventArgs e)
        {
            barButtonItem3.Enabled = true;
            this.Text = TCPUtil.AppName;
            barButtonItem3.Enabled = false;
            bsiMantenimiento.Enabled = false;
            barButtonItem2.Enabled = false;
            barButtonItem1.Enabled = false;
            barButtonItem13.Enabled = false;
            barButtonItem17.Enabled = false;
        }

        #region MANAGERSTATS
        private void InitManagerTable()
        {
            TCPUtil.ManagerStats = new DataTable();

            TCPUtil.ManagerStats.Columns.Add("ID", typeof(Int32));
            TCPUtil.ManagerStats.Columns.Add("NetManagerID", typeof(Int32));
            TCPUtil.ManagerStats.Columns.Add("Nombre", typeof(String));
            TCPUtil.ManagerStats.Columns.Add("Status", typeof(Int32)); //<== Se Agrega Columna de status con ICono
            TCPUtil.ManagerStats.Columns.Add("Inicio", typeof(DateTime));
            TCPUtil.ManagerStats.Columns.Add("UpTime", typeof(String));
            TCPUtil.ManagerStats.Columns.Add("Recibidas", typeof(Int32));
            TCPUtil.ManagerStats.Columns.Add("Enviadas", typeof(Int32));
            TCPUtil.ManagerStats.Columns.Add("Tipo", typeof(String));

            TCPUtil.ManagerStats.PrimaryKey = new DataColumn[] { TCPUtil.ManagerStats.Columns["ID"] };

        }



        #endregion

        private void barSubItem1_Click(object sender, EventArgs e)
        {

        }

        private void barButtonItem3_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Esta seguro de detener los servicios?, todas las conexiones se detendran", TCPUtil.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                gridView1.DataSource = null;
                NetManagers.Values.ToList().ForEach(ec =>
                {

                    foreach (var item in ec.ServerNetManager.GetClientes())
                    {
                        ec.ServerNetManager.DesconectaCliente(item);
                    }

                    ec.ServerNetManager.Dispose();

                });

                NetManagers.Clear();

                // TCPUtil.Statics.Clear();
                TCPUtil.ConexsUnderMonitor.Clear();
                /*
                 *  public Int32 ID { get; set; }
        public Int32 NetManagerID { get; set; }
        public String Nombre { get; set; }
        public Int32 Status { get; set; }
        public DateTime Inicio { get; set; }
        public String UpTime { get; set; }
        public Int32 Recibidas { get; set; }
        public Int32 Enviadas { get; set; }
        public String Tipo { get; set; }
                 */
                //gridView1.DataSource = TCPUtil.Statics;

                SendLog("***");

                barStaticItem1.Text = "Status: Servicios Detenidos";
                //barButtonItem11.Enabled = true;
                barButtonItem2.Enabled = true;
                barButtonItem3.Enabled = false;
                barButtonItem13.Enabled = true;
                operacionesToolStripMenuItem.Enabled = false;
            }
        }

        private void btsModoDebug_Click(object sender, EventArgs e)
        {
            /*TCPUtil.DebugMode =true;
            MessageBox.Show("Modo Debug " + (TCPUtil.DebugMode ? "ACTIVADO" : "DESACTIVADO"), TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);*/
            if (_FocusRow != null)
            {
                Int32 ConexId = _FocusRow.ID;
                Int32 NetManger = _FocusRow.NetManagerID;
                TCPUtil.ConexsUnderMonitor.TryAdd(ConexId, NetManger);
                TCPUtil.CreaFileLog();
                MessageBox.Show("Modo debug Activado ID: " + _FocusRow.ID);
            }
        }

        private void barButtonItem14_Click(object sender, EventArgs e)
        {
            new VisorISO8583().Show();
        }


        private void barButtonItem12_Click(object sender, EventArgs e)
        {
            ParametrosCnSQL hijo = new ParametrosCnSQL();
            hijo.ShowDialog();
            TCPUtil.LoadConfigSQL();
            bsiMantenimiento.Enabled = TCPUtil.ConexSQLReady();
            barButtonItem9.Enabled = bsiMantenimiento.Enabled;
        }

        private void barButtonItem1_Click(object sender, EventArgs e)
        {
            Parametros hijo = new Parametros();
            hijo.ShowDialog();
            TCPUtil.LoadGlobalParameters();
            bsiMantenimiento.Enabled = TCPUtil.ConexSQLReady();
            barButtonItem9.Enabled = bsiMantenimiento.Enabled;
        }

        private void barButtonItem13_Click(object sender, EventArgs e)
        {
            NetManagerCatalog hijo = new NetManagerCatalog();
            hijo.ShowDialog();
        }

        private void barButtonItem17_Click(object sender, EventArgs e)
        {
            new WsCatalogo().ShowDialog();
        }

        private void barButtonItem9_Click(object sender, EventArgs e)
        {
            new Bitacora().ShowDialog();
        }

        private void barButtonItem15_Click(object sender, EventArgs e)
        {
            SendLog("***");
        }

        private void finalizarAplicacionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                TCPUtil.LoadConfigSQL();
                TCPUtil.LoadGlobalParameters();

                bsiMantenimiento.Enabled = TCPUtil.ConexSQLReady(true);
                barButtonItem9.Enabled = bsiMantenimiento.Enabled;
                barButtonItem2.Enabled = bsiMantenimiento.Enabled;
                // InitManagerTable();
                _FocusRow = new ConexStatics();
                TCPUtil.Statics = new List<ConexStatics>();

                var licenseMsg = "Error con la fecha de expiracion";
                if (bsiMantenimiento.Enabled)
                {
                    toolStripMenuItem1.Enabled = false;
                    barButtonItem1.Enabled = true;
                    barButtonItem13.Enabled = true;
                    barButtonItem17.Enabled = true;

                    try
                    {
                        var parameter = DBUtil.GetParameterByCode("LicenseToken");
                        if (parameter.Rows.Count > 0)
                        {
                            var token = parameter.Rows[0]["ParameterValue"].ToString();
                            if (!string.IsNullOrWhiteSpace(token))
                            {
                                var tokenData = JWT.ValidarJwtToken(token);
                                var expDate = (DateTime)tokenData["exp_datetime"];
                                licenseMsg = "Licencia valida hasta: " + expDate.ToString("dd/MM/yyyy");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error al validar licencia: {ex.Message}");
                    }
                }
                else
                {
                    licenseMsg = "Error con la fecha de expiracion";
                    barButtonItem2.Enabled = false;
                }
                barStaticItem2.Text = licenseMsg;
                if (TCPUtil.ConexSQLReady(true)) //Actualizacion de BD
                {
                    DBUtil.UpdateDataBase();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show($"Ocurrió un error al intentar establecer la conexión:\n\n{ex.Message}",
                                TCPUtil.AppName,
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }
        }

        private void BtnDebugOff_Click(object sender, EventArgs e)
        {
            /*TCPUtil.DebugMode = false;
            MessageBox.Show("Modo Debug " + (TCPUtil.DebugMode ? "ACTIVADO" : "DESACTIVADO"), TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);*/
            if (_FocusRow != null)
            {
                Int32 ConexId = _FocusRow.ID;
                TCPUtil.ConexsUnderMonitor.TryRemove(ConexId, out var dateTime);
                MessageBox.Show("Modo debug Desactivado ID: " + _FocusRow.ID);
            }
        }

        private void LstLog_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control == true && e.KeyCode == Keys.C)
            {
                string s = LstLog.SelectedItem.ToString();
                Clipboard.SetData(DataFormats.StringFormat, s);
            }
        }

        private void BtnOpenLog_Click(object sender, EventArgs e)
        {
            if (_FocusRow != null)
            {
                Int32 ConexId = _FocusRow.ID;

                TCPUtil.ConexsUnderMonitor.TryGetValue(ConexId, out Int32 NetMa);
                NetManager mNet = new NetManager(NetMa);
                Int32 Cuantas = 0;
                if (mNet.ThreePartConex) Cuantas = 3; else Cuantas = 2;

                Int32 Hay = 0;

                TCPUtil.ConexsUnderMonitor.ToList().ForEach(c =>
                {
                    if (c.Value == mNet.Id)
                    {
                        Hay++;
                    }

                });

                if (Hay == Cuantas)
                {
                    new Bitacora(0, mNet.Id).ShowDialog();
                }
                else
                {
                    new Bitacora(ConexId).ShowDialog();
                }

            }
        }
        private void ForMonitorTask()
        {
            foreach (KeyValuePair<ConexMaster, DateTime> curr in InMonitor)
            {
                sendSignOn(curr.Key);
                break;
            }
        }
        private void EndPoint_DataReceived(object sender, FromServerDataReceivedEventArgs e)
        {
            ClienteTCP Conex = (ClienteTCP)sender;

            ConexMaster mEndPoint = Conex.Conex;

            ISO8583 HelperISO = new ISO8583(1);
            String FormatedHEX = TCPUtil.PrepareISO8583FromHEX(BitConverter.ToString(e.Data).ToString().Replace("-", ""), 8, 1, out Int32 HR);
            String[] Parsed = HelperISO.Parse(FormatedHEX, false);
            String[] TransL = HelperISO.Parse(FormatedHEX, true);

            String L = "";

            if (TransL[39] == "00")
            {
                L = "Data recibida *(T-" + mEndPoint.TypeMsg + "): Respuesta 0810 aceptada " + " en " + mEndPoint.DescriptionName;
            }
            else
            {
                L = "Data recibida *(T-" + mEndPoint.TypeMsg + "): Respuesta no aceptada " + " en " + mEndPoint.DescriptionName;
            }

            Task.Run(async () => await DBUtil.InsertBitacoraASync(L, mEndPoint.Id, "LocalHost", "Sign On", mEndPoint.NetManagerId));
            SendLog(L);

        }
        private void EndPoint_Desconectado(object sender, ForHostEventArgs e)
        {
            ClienteTCP Conex = (ClienteTCP)sender;

            ConexMaster mEndPoint = Conex.Conex;
            TCPUtil.Statics.Where(c => c.ID == mEndPoint.Id).First().Status = 0;

            String L = "Conexion Sign On " + mEndPoint.DescriptionName + "(" + mEndPoint.IPAdress + ":" + mEndPoint.Port + ") Terminada " + DateTime.Now.ToString("dd/MM hh:mm:ss");
            Task.Run(async () => await DBUtil.InsertBitacoraASync(L, mEndPoint.Id, "LocalHost", "Sign On", mEndPoint.NetManagerId));
            SendLog(L);
        }
        public void sendSignOn(ConexMaster mEndPoint)
        {
            var EndPoint = TCPUtil.ConvertToClienteTCP(mEndPoint, mEndPoint.IsValidator);
            try
            {

                if (EndPoint != null)
                {
                    if (!EndPoint.IsConnected)
                    {
                        EndPoint.DataReceived += EndPoint_DataReceived;
                        EndPoint.Desconectado += EndPoint_Desconectado;

                        TCPUtil.Statics.Where(c => c.ID == mEndPoint.Id).First().Status = 75;
                        String L = "Sign On: Intentando conectar con " + mEndPoint.DescriptionName + "(" + mEndPoint.IPAdress + ":" + mEndPoint.Port + ") " + DateTime.Now.ToString("dd/MM hh:mm:ss");

                        Task.Run(async () => await DBUtil.InsertBitacoraASync(L, mEndPoint.Id, "LocalHost", "Sign On", mEndPoint.NetManagerId));
                        SendLog(L);

                        String sigOn = TCPUtil.GenNewIsoForSigOn();
                        Byte[] Paquete = TCPUtil.ConvertHexStringToByteArray(sigOn);

                        EndPoint.Connect(new FromClienteDataReceived("127.0.0.1", Paquete));

                        L = "Enviando Cadena (T-" + 1 + "):  -> " + BitConverter.ToString(Paquete).Replace("-", "");
                        Task.Run(async () => await DBUtil.InsertBitacoraASync(L, mEndPoint.Id, "LocalHost", "Sign On", mEndPoint.NetManagerId));
                        SendLog(L);

                        EndPoint.Send(Paquete);

                        L = "Sign On enviado a EndPoint " + mEndPoint.IPAdress + " con exito";
                        Task.Run(async () => await DBUtil.InsertBitacoraASync(L, mEndPoint.Id, "LocalHost", "Sign On", mEndPoint.NetManagerId));
                        SendLog(L);

                    }
                }
            }
            catch (Exception ex)
            {
                String L = "Conexion " + mEndPoint.DescriptionName + "(" + mEndPoint.IPAdress + ":" + mEndPoint.Port + ") no disponible, no es posible continuar comunicacion, ex: " + ex.Message;
                Task.Run(async () => await DBUtil.InsertBitacoraASync(L, mEndPoint.Id, "LocalHost", "Sign On", mEndPoint.NetManagerId));
                SendLog(L);
                TCPUtil.Statics.Where(c => c.ID == mEndPoint.Id).First().Status = 50;
            }

            EndPoint.Dispose();     //Importante resetea la conexión para no escuchar los callbacks aqui
        }
        private ConcurrentDictionary<ConexMaster, DateTime> InMonitor = new ConcurrentDictionary<ConexMaster, DateTime>();
        private CancellationTokenSource tokenSource = new CancellationTokenSource();
        private void BtnSingOn_Click(object sender, EventArgs e)
        {
            if (_FocusRow != null)
            {
                if (TCPUtil.ParamSignOnMinutes == 0)
                {
                    MessageBox.Show("Antes debes de configurar el parametro SignOn_Minutes en la configuración", TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (_FocusRow.Tipo == "Servidor")
                {
                    NetManagers.TryGetValue(_FocusRow.NetManagerID, out NetManager mNetMngr);

                    if (mNetMngr.ServerNetManager.LastIPConnected != null)
                    {
                        if (mNetMngr.ServerNetManager.LastIPConnected != "")
                        {
                            mNetMngr.ServerNetManager.SendSignOnToLastConn();
                            mNetMngr.ServerNetManager.sendingSignOn = true;
                        }
                        else
                        {
                            MessageBox.Show("No hay ningun cliente conectado", TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("No hay ningun cliente conectado", TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                }
                else
                {
                    ConexMaster cnForSignOn = new ConexMaster(_FocusRow.ID, _FocusRow.NetManagerID, false);

                    String L = "Inicia envio SignOn en " + cnForSignOn.DescriptionName + "(" + cnForSignOn.IPAdress + ":" + cnForSignOn.Port + ") " + DateTime.Now.ToString("dd/MM hh:mm:ss") + " Se ejecutara cada " + TCPUtil.ParamSignOnMinutes + " minutos";

                    Task.Run(async () => await DBUtil.InsertBitacoraASync(L, cnForSignOn.Id, "LocalHost", "Sign On", cnForSignOn.NetManagerId));
                    SendLog(L);

                    tokenSource = new CancellationTokenSource();

                    CancellationToken token = tokenSource.Token;
                    InMonitor.TryAdd(cnForSignOn, DateTime.Now);

                    Task tarea = Task.Factory.StartNew(async () =>
                    {
                        while (!token.IsCancellationRequested)
                        {
                            if (InMonitor.Count > 0)
                            {
                                await Task.Run(() => ForMonitorTask());
                                Application.DoEvents();
                            }
                            Application.DoEvents();
                            await Task.Delay((TCPUtil.ParamSignOnMinutes * 60) * 1000);
                        }


                    }, token, TaskCreationOptions.LongRunning, TaskScheduler.FromCurrentSynchronizationContext());

                }
            }
        }

        private void BtnEndSon_Click(object sender, EventArgs e)
        {
            if (_FocusRow != null)
            {
                if (_FocusRow.Tipo == "Servidor")
                {
                    NetManagers.TryGetValue(_FocusRow.NetManagerID, out NetManager mNetMngr);

                    mNetMngr.ServerNetManager.StopSignOn();

                }
                else
                {
                    tokenSource.Cancel();

                    ConexMaster cnForSignOn = new ConexMaster(_FocusRow.ID, _FocusRow.NetManagerID, false);

                    String L = "Termina envio SignOn en " + cnForSignOn.DescriptionName + "(" + cnForSignOn.IPAdress + ":" + cnForSignOn.Port + ") " + DateTime.Now.ToString("dd/MM hh:mm:ss");

                    Task.Run(async () => await DBUtil.InsertBitacoraASync(L, cnForSignOn.Id, "LocalHost", "Sign On", cnForSignOn.NetManagerId));
                    SendLog(L);

                    InMonitor.ToList().ForEach(K =>
                    {
                        if (K.Key.Id == cnForSignOn.Id)
                        {
                            InMonitor.TryRemove(K.Key, out DateTime _);
                        }
                    });
                }
            }
        }

        private void gridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {


        }

        private void activarDebugToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int r = gridView1.CurrentRow.Index;
            if (r != -1)
            {
                if (_FocusRow != null)
                {
                    Int32 ConexId = _FocusRow.ID;
                    Int32 NetManger = _FocusRow.NetManagerID;
                    TCPUtil.ConexsUnderMonitor.TryAdd(ConexId, NetManger);
                    TCPUtil.CreaFileLog();
                    MessageBox.Show("Modo Debug Activado ID:" + _FocusRow.ID);
                }
            }
        }

        private void gridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (gridView1.Rows.Count > 0)
            {
                int r = gridView1.CurrentRow.Index;
                if (r != -1)
                {
                    string v = Convert.ToString(gridView1.Rows[r].Cells[0].Value);
                    if (!string.IsNullOrEmpty(v))
                    {
                        _FocusRow.ID = Convert.ToInt32(gridView1.Rows[r].Cells[0].Value);
                        _FocusRow.NetManagerID = Convert.ToInt32(gridView1.Rows[r].Cells[1].Value);
                        _FocusRow.Tipo = Convert.ToString(gridView1.Rows[r].Cells[7].Value);
                    }
                }
            }
        }

        private void desactivarDebugToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_FocusRow != null)
            {
                Int32 ConexId = _FocusRow.ID;
                TCPUtil.ConexsUnderMonitor.TryRemove(ConexId, out var dateTime);
                MessageBox.Show("Modo debug Desactivado ID: " + _FocusRow.ID);
            }
        }
    }
}
