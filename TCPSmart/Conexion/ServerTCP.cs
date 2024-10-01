using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TCPSmart.Conexion
{
    public class ServerTCP : IDisposable
    {
        private int _BufferSizeRec = 1092;
        private int _IdleTimeOut = 0;
        //private int _IdleHostTimeOut = 0;

        private readonly Bitacora _Bitacora = new Bitacora();

        //private ClienteTCP _HostEndPoint;

        private CancellationTokenSource _TokenSource = new CancellationTokenSource();
        private CancellationToken _Token;

        //  private CancellationTokenSource _TokenHostSource = new CancellationTokenSource();
        //  private CancellationToken _TokenHost;

        #region Public Members
        /// <summary>
        /// Evento cuando un cliente se conecta.
        /// </summary>
        public event EventHandler<ClientConEventArgs> ClienteConectado;

        /// <summary>
        /// Evento cuando un cliente se desconecta.
        /// </summary>
        public event EventHandler<ClientDesConexEventArgs> ClienteDesconectado;

        /// <summary>
        /// Evento cuando se recibe informacion, Informacion necesaria para Operar sera obtenida.
        /// </summary>
        public delegate void DataRecibidaHandler(FromClienteDataReceived Data);
        public event DataRecibidaHandler DataRecibida;

        // public event EventHandler<FromServerDataReceivedEventArgs> HostEndPointDataRecibida;

        /// <summary>
        /// Buffer de Recepcion a usar en las conexiones TCP de clientes.
        /// </summary>
        public Int32 BufferSizeRec
        {
            get { return _BufferSizeRec; }
            set
            {
                if (value < 1) throw new ArgumentException("Buffer de recepcion debe ser mayor a 1.");
                if (value > 65536) throw new ArgumentException("Buffer de recepcion debe ser menor a 65,536.");
                _BufferSizeRec = value;
            }
        }

        /// <summary>
        /// Tiempo Maximo en Segundos para considerar desconectar a un cliente inactivo
        /// por Default el tiempo es 0, lo que nunca desconectara al cliente por inactivad.
        /// este tiempo se resetea con cualquier mensaje recibido o enviado al cliente
        /// por lo que si este tiempo es 30 el cliente sera desconectado si no ha recibido un mensaje en 30 segundos.
        /// </summary>
        public Int32 IdleClienteTimeOut
        {
            get { return _IdleTimeOut; }
            set
            {
                if (value < 0) throw new ArgumentException("El tiempo por inactividad debe ser mayor a 0.");
                _IdleTimeOut = value;
            }
        }

        //public Int32 IdleHostTimeOut
        //{
        //    get { return _IdleHostTimeOut; }
        //    set
        //    {
        //        _IdleHostTimeOut = value;
        //    }
        //}

        public Bitacora Bitacora
        {
            get { return _Bitacora; }
        }

        public Flow.NetManager LayerNetManager { get; set; }

        public Int32 Zesion { get; set; }

        public Action<String, String, Int32, Boolean, String> Log = null;

        public String LastIPConnected { get; set; }

        public Boolean sendingSignOn { get; set; }

        #endregion

        private ConcurrentDictionary<String, ClientData> _Clientes = new ConcurrentDictionary<String, ClientData>();
        private readonly ConcurrentDictionary<String, DateTime> _UltimoCliente = new ConcurrentDictionary<String, DateTime>();
        private readonly ConcurrentDictionary<String, DateTime> _ClientesKicked = new ConcurrentDictionary<String, DateTime>();
        private readonly ConcurrentDictionary<String, DateTime> _ClientesTimeout = new ConcurrentDictionary<String, DateTime>();
        // private ConcurrentDictionary<ClienteTCP, DateTime> _HostConexiones = new ConcurrentDictionary<ClienteTCP, DateTime>();

        private readonly IPAddress _IPAddress;
        private readonly string _ListenerIP;
        private  TcpListener _Listener;

        private readonly Int32 _Puerto;
        private readonly bool _Running;
        private readonly Int32 ConexId;

        public String IPLISTENER
        {
            get { return _ListenerIP; }
        }

        public void Dispose()
        {
            //if (_TokenHostSource != null)
            //{
            //    if (!_TokenHostSource.IsCancellationRequested) _TokenHostSource.Cancel();
            //    _TokenHostSource.Dispose();
            //    _TokenHostSource = null;
            //}

            if (_TokenSource != null)
            {
                if (!_TokenSource.IsCancellationRequested) _TokenSource.Cancel();
                _Token = _TokenSource.Token;
                _TokenSource.Dispose();
                _TokenSource = null;

            }

            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Constructor Inicializa el Servidor TCP.
        /// </summary>
        /// <param name="ListenerIP"></param>
        /// <param name="Puerto"></param>
        //public ServerTCP(String ListenerIP, Int32 Puerto)
        //{
        //    if (String.IsNullOrEmpty(ListenerIP)) throw new ArgumentNullException(nameof(ListenerIP));
        //    if (Puerto < 0) throw new ArgumentException("El puerto debe ser mayor a 0");

        //    _ListenerIP = ListenerIP;
        //    _IPAddress = IPAddress.Parse(_ListenerIP);
        //    _Puerto = Puerto;
        //    _Token = _TokenSource.Token;

        //    Task.Run(() => MonitorForIdleClients(), _Token);
        //}

        public ServerTCP(Flow.NetManager CapaNetManager)
        {
            // if (String.IsNullOrEmpty(CapaNetManager.ConexMServer.IPAdress)) throw new ArgumentNullException(nameof(CapaNetManager.ConexMServer.IPAdress));
            if (CapaNetManager.ConexMServer.Port < 0) throw new ArgumentException("El puerto debe ser mayor a 0");

            //if (CapaNetManager.ConexMServer.IPAdress == "127.0.0.1")
            //{
            //    //String HostName = Dns.GetHostName();

            //    if (TCPUtil.ParamDirectIPServer != "")
            //    {
            //        _ListenerIP = TCPUtil.ParamDirectIPServer;
            //    }
            //    else
            //    {
            //        if (TCPUtil.ParamNetworkAdapter != "")
            //        {
            //            _ListenerIP = TCPUtil.GetIPByNetWorkUnicast();
            //            if (_ListenerIP == "")
            //            {
            //                _ListenerIP = TCPUtil.GetIPByNetWorkMulticast();
            //            }
            //            if (_ListenerIP == "")
            //            {
            //                _ListenerIP = TCPUtil.GetIpByLastNetworkAdapter(true);
            //            }
            //            if (_ListenerIP == "")
            //            {
            //                _ListenerIP = TCPUtil.GetIpByLastNetworkAdapter(false);
            //            }
            //        }
            //        else
            //        {
            //            _ListenerIP = TCPUtil.GetIpByLastNetworkAdapter(true);

            //            if (_ListenerIP == "")
            //            {
            //                _ListenerIP = TCPUtil.GetIpByLastNetworkAdapter(false);
            //            }
            //        }
            //    }

            //if (_ListenerIP == "")
            //{
            //    Log?.Invoke("[Server] -> No se pudo obtener IP del adapatador de red, Utilice el parametro DirectIP para operar", _ListenerIP, ConexId, true, "NetManager");
            //}
            //else
            //{
            //    Log?.Invoke("[Server] -> Servidor iniciandose en " + _ListenerIP, _ListenerIP, ConexId, true, "NetManager");
            //}

            //}
            //else
            {
                _ListenerIP = TCPUtil.GetIpByLastNetworkAdapter(true);
                //_ListenerIP = CapaNetManager.ConexMServer.IPAdress;
            }

            if (_ListenerIP != null)
            {
                _IPAddress = IPAddress.Parse(_ListenerIP);
                _Puerto = CapaNetManager.ConexMServer.Port;
                _Token = _TokenSource.Token;

                Log?.Invoke("[Server] -> Servidor iniciandose en " + _ListenerIP, _ListenerIP, ConexId, true, "NetManager");

                LayerNetManager = CapaNetManager;
                ConexId = LayerNetManager.ConexMServer.Id;
            }
            else
            {
                _Running = true;
            }


        }
        /// <summary>
        /// Inicia a escuchar y aceptar conexiones.
        /// </summary>
        public void Start()
        {
            if (_Running) throw new InvalidOperationException("ServerTCP no esta bien configurado, revise la seccion de parametros que el campo de NetworkAdapterName sea valido");


            _Listener = new TcpListener(_IPAddress, _Puerto);
            _Listener.Start();

            _Clientes = new ConcurrentDictionary<String, ClientData>();

            Task.Run(() => AcceptConnections(), _Token);
        }

        /// <summary>
        /// Regresa un listado de clientes IP conectados al Servidor.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<String> GetClientes()
        {
            List<String> Clientes = new List<String>(_Clientes.Keys);
            return Clientes;
        }

        /// <summary>
        /// Indica si un cliente se encuentra conectado.
        /// </summary>
        /// <param name="IP"></param>
        /// <returns></returns>
        public Boolean IsConectado(String IP)
        {
            if (String.IsNullOrEmpty(IP)) throw new ArgumentNullException(nameof(IP));

            ClientData Cliente = null;
            return (_Clientes.TryGetValue(IP, out Cliente));
        }

        public void Envia(String IP, Byte[] Data)
        {
            if (String.IsNullOrEmpty(IP)) throw new ArgumentNullException(nameof(IP));
            if (Data == null || Data.Length < 1) throw new ArgumentNullException(nameof(Data));

            ClientData Cliente = null;
            if (!_Clientes.TryGetValue(IP, out Cliente)) return;
            if (Cliente == null) return;

            lock (Cliente.SendLock)
            {
                Cliente.NStream.Write(Data, 0, Data.Length);
                Cliente.NStream.Flush();
            }

            _Bitacora.BytesEnviados += Data.Length;
        }

        public void DesconectaCliente(String IP)
        {
            if (String.IsNullOrEmpty(IP)) return;//throw new ArgumentNullException(nameof(IP));

            if (LastIPConnected == IP) LastIPConnected = "";

            if (!_Clientes.TryGetValue(IP, out ClientData client))
            {
                Log?.Invoke("[Server] -> El cliente:" + IP + " no esta registrado", IP, ConexId, false, "NetManager_");
            }
            else
            {
                if (!_ClientesTimeout.ContainsKey(IP))
                {
                    Log?.Invoke("[Server] -> Expulsando al cliente:" + IP + " por inactividad", IP, ConexId, false, "NetManager");
                    _ClientesKicked.TryAdd(IP, DateTime.Now);
                }

                _Clientes.TryRemove(client.IPAddress, out ClientData destroyed);
                // Log?.Invoke("[Server] -> Cliente expulsado: " + IP, IP, ConexId,false, "NetManager");
                client.Dispose();
            }
        }


        protected virtual void Dispose(Boolean Disposing)
        {
            if (Disposing)
            {
                try
                {
                    if (_Clientes != null && _Clientes.Count > 0)
                    {
                        foreach (KeyValuePair<string, ClientData> curr in _Clientes)
                        {
                            curr.Value.Dispose();
                            Log?.Invoke("[Server] -> Conexion con: " + curr.Key + " terminada", curr.Key, ConexId, false, "NetManager");
                        }
                    }

                    if (_Listener != null && _Listener.Server != null)
                    {
                        _Listener.Server.Close();
                        _Listener.Server.Dispose();

                    }

                    if (_Listener != null)
                    {
                        _Listener.Stop();

                    }
                }
                catch (Exception ex)
                {
                    Log?.Invoke("[Server] -> Excepcion terminando el servicio: " +
                        ex.ToString(), "-", ConexId, true, "NetManager");
                }
            }
        }

        private bool IsClienteConectado(TcpClient Cliente)
        {
            if (Cliente.Connected)
            {
                if ((Cliente.Client.Poll(0, SelectMode.SelectWrite)) && (!Cliente.Client.Poll(0, SelectMode.SelectError)))
                {
                    byte[] buffer = new byte[1];
                    if (Cliente.Client.Receive(buffer, SocketFlags.Peek) == 0)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private async void AcceptConnections()
        {
            while (!_Token.IsCancellationRequested)
            {
                ClientData client = null;

                try
                {
                    TcpClient tcpClient = await _Listener.AcceptTcpClientAsync();
                    string clientIp = tcpClient.Client.RemoteEndPoint.ToString();

                    client = new ClientData(tcpClient);

                    _Clientes.TryAdd(clientIp, client);
                    _UltimoCliente.TryAdd(clientIp, DateTime.Now);

                    // Log?.Invoke("[Server] -> Inicia conexion con:" + clientIp + " esperando informacion",  clientIp, ConexId, false, "NetManager");
                    ClienteConectado?.Invoke(this, new ClientConEventArgs(clientIp));
                    LastIPConnected = clientIp;
                    if (LayerNetManager.ValidateTimeOut)
                    {
                        _ = Task.Run(() => MonitorForIdleClients(), _Token);
                    }
                    Task unawaited = Task.Run(() => DataReceiver(client), _Token);
                }
                catch (OperationCanceledException)
                {
                    return;
                }
                catch (ObjectDisposedException)
                {
                    if (client != null) client.Dispose();
                    continue;
                }
                catch (Exception e)
                {
                    if (client != null) client.Dispose();
                    Log?.Invoke("[Server] -> Excepcion mientras se esperan conexiones: " + e.ToString(), "dispose", ConexId, true, "NetManager");
                    continue;
                }
            }
        }

        private async Task DataReceiver(ClientData client)
        {
            // Log?.Invoke("[Server] -> Recibiendo informacion del cliente: " + client.IPAddress, client.IPAddress, ConexId, false, "NetManager");

            while (true)
            {
                try
                {
                    if (client.Cliente != null)
                    {
                        if (client.Token.IsCancellationRequested
                            || !IsClienteConectado(client.Cliente))
                        {
                            Log?.Invoke("[Server] -> Cliente " + client.IPAddress + " desconectado", client.IPAddress, ConexId, false, "NetManager");
                            break;
                        }

                        if (client.Token.IsCancellationRequested)
                        {
                            Log?.Invoke("[Server] -> Cancelacion solicitada recibiendo informacion " + client.IPAddress, client.IPAddress, ConexId, false, "NetManager");
                            break;
                        }

                        byte[] data = await DataReadAsync(client);
                        if (data == null)
                        {
                            await Task.Delay(30);
                            continue;
                        }

                        if (data != null)
                        {
                            Boolean ignoreChain = false;
                            if (BitConverter.ToString(data).Replace("-", "").StartsWith("000000"))
                            {
                                ignoreChain = true;
                            }

                            if (!ignoreChain)
                            {
                                Int32 Counter = 100 + LayerNetManager.Zesiones.Count;
                                LayerNetManager.Zesiones.TryAdd(client.IPAddress, Counter);
                                LayerNetManager.Zesiones.TryGetValue(client.IPAddress, out Int32 Zesion);

                                Log?.Invoke("<<<<<<<<<<<<<<<<<<<<<<<<<<<< " + LayerNetManager.DescriptionName + ":-> INICIA SESION " + Zesion + ">>>>>>>>>>>>>>>>>>>>>>>>>>>>", client.IPAddress, ConexId, false, "Flow");
                                Log?.Invoke("Recibiendo cadena(T-" + LayerNetManager.ConexMServer.TypeMsg + "):" + BitConverter.ToString(data).Replace("-", ""), client.IPAddress, ConexId, false, "NetManager");
                            }
                            else
                            {
                                Log?.Invoke("<<<<<<<<<<<<<<<<<<<<<<<<<<<< Recibiendo cadena KeepLiveConnection >>>>>>>>>>>>>>>>>>>>>>>>>>>>", client.IPAddress, ConexId, false, "NetManager");
                            }

                            FromClienteDataReceived ee = new FromClienteDataReceived(client.IPAddress, data);
                            DataRecibida?.Invoke(ee);

                            _Bitacora.BytesRecibidos += data.Length;
                            UpdateClientLastSeen(client.IPAddress);
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                catch (SocketException esx)
                {
                    if (esx.NativeErrorCode != 10004)
                    {
                        Log?.Invoke("[Server] -> Excepcion recibiendo informacion desde :" + client.IPAddress + " posible desconexion/cancelacion desde el cliente; Error:" + esx.Message, client.IPAddress, ConexId, true, "NetManager");
                    }
                }
                catch (Exception e)
                {
                    if (client.Cliente == null)
                    {
                        Log?.Invoke("[Server] -> Cliente " + client.IPAddress + " se desconecto", client.IPAddress, ConexId, true, "NetManager");
                    }
                    else
                    {
                        Log?.Invoke("[Server] -> Excepcion recibiendo informacion desde: " + client.IPAddress + " Error:" +
                            //Environment.NewLine +
                            e.ToString(), client.IPAddress, ConexId, true, "NetManager");

                        //Environment.NewLine);
                    }
                    break;
                }
            }

            //Log?.Invoke("[Server] -> Se ha recibido toda la informacion desde el cliente " + client.IPAddress, client.IPAddress, ConexId, false, "NetManager");

            if (_ClientesKicked.ContainsKey(client.IPAddress))
            {
                ClienteDesconectado?.Invoke(this, new ClientDesConexEventArgs(client.IPAddress, DesconexionType.Kicked));
            }
            else if (_ClientesTimeout.ContainsKey(client.IPAddress))
            {
                Log?.Invoke("[Server] -> Expulsando al cliente:" + client.IPAddress + " por la configuracion de Timeout", client.IPAddress, ConexId, false, "NetManager");
                Log?.Invoke("[Server] -> Cliente expulsado: " + client.IPAddress, client.IPAddress, ConexId, false, "NetManager");
                ClienteDesconectado?.Invoke(this, new ClientDesConexEventArgs(client.IPAddress, DesconexionType.Timeout));

            }
            else
            {
                ClienteDesconectado?.Invoke(this, new ClientDesConexEventArgs(client.IPAddress, DesconexionType.Normal));
            }

            _Clientes.TryRemove(client.IPAddress, out ClientData _);
            _UltimoCliente.TryRemove(client.IPAddress, out _);
            _ClientesKicked.TryRemove(client.IPAddress, out _);
            //_ClientesTimeout.TryRemove(client.IPAddress, out _);
            client.Dispose();
        }

        private async Task<byte[]> DataReadAsync(ClientData client)
        {
            if (client.Token.IsCancellationRequested) throw new OperationCanceledException();
            if (!client.NStream.CanRead) return null;
            if (!client.NStream.DataAvailable) return null;

            byte[] buffer = new byte[_BufferSizeRec];
            int read = 0;

            using (MemoryStream ms = new MemoryStream())
            {
                while (true)
                {
                    read = await client.NStream.ReadAsync(buffer, 0, buffer.Length);

                    if (read > 0)
                    {
                        ms.Write(buffer, 0, read);
                        return ms.ToArray();
                    }
                    else
                    {
                        throw new SocketException();
                    }
                }
            }

        }

        private async Task MonitorForIdleClients()
        {
            while (!_Token.IsCancellationRequested)
            {
                if (_IdleTimeOut > 0 && _UltimoCliente.Count > 0)
                {
                    MonitorForIdleClientsTask();
                }
                await Task.Delay(5000, _Token);
            }
        }

        private void MonitorForIdleClientsTask()
        {
            DateTime idleTimestamp = DateTime.Now.AddSeconds(-1 * _IdleTimeOut);

            foreach (KeyValuePair<string, DateTime> curr in _UltimoCliente)
            {
                if (curr.Value < idleTimestamp)
                {
                    if (!_ClientesTimeout.ContainsKey(curr.Key))
                    {
                        _ClientesTimeout.TryAdd(curr.Key, DateTime.Now);
                        _UltimoCliente.TryRemove(curr.Key, out _);
                        DesconectaCliente(curr.Key);
                        Application.DoEvents();
                    }
                }
            }
        }

        private void UpdateClientLastSeen(string ipPort)
        {
            if (_UltimoCliente.ContainsKey(ipPort))
            {
                DateTime ts;
                _UltimoCliente.TryRemove(ipPort, out ts);
            }

            _UltimoCliente.TryAdd(ipPort, DateTime.Now);
        }

        private readonly List<String> InMonitor = new List<string>();
        private CancellationTokenSource tokenSource = new CancellationTokenSource();

        public void SendSignOnToLastConn()
        {
            if (LastIPConnected != "")
            {
                String L = "Inicia envio SignOn en " + LayerNetManager.DescriptionName + "(" + LastIPConnected + ":" + _Puerto + ") " + DateTime.Now.ToString("dd/MM hh:mm:ss") + " Se ejecutara cada " + TCPUtil.ParamSignOnMinutes + " minutos";
                Log?.Invoke(L, LastIPConnected, ConexId, false, "SignOn NET");

                tokenSource = new CancellationTokenSource();

                CancellationToken token = tokenSource.Token;
                if (!InMonitor.Contains(LastIPConnected))
                {
                    InMonitor.Add(LastIPConnected);
                }

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

        private void ForMonitorTask()
        {
            InMonitor.ForEach(i =>
            {
                SendSignOn(i);
                return;
            });
        }

        private void SendSignOn(String ipCliente)
        {
            try
            {
                String sigOn = TCPUtil.GenNewIsoForSigOn();
                Byte[] Paquete = TCPUtil.ConvertHexStringToByteArray(sigOn);

                String L = "Enviando Cadena (T-" + 1 + "):  -> " + BitConverter.ToString(Paquete).Replace("-", "");
                Log?.Invoke(L, ipCliente, ConexId, false, "SignOn NET");

                Envia(ipCliente, Paquete);

                L = "Sign On enviado a " + ipCliente + " con exito";
                Log?.Invoke(L, ipCliente, ConexId, false, "SignOn NET");

            }
            catch (Exception ex)
            {
                Log?.Invoke(ex.ToString(), ipCliente, ConexId, false, "SignOn Error");
            }
        }

        public void StopSignOn()
        {
            tokenSource.Cancel();
            String L = "Termina envio SignOn en " + LayerNetManager.DescriptionName + "(" + LastIPConnected + ":" + _Puerto + ") " + DateTime.Now.ToString("dd/MM hh:mm:ss");
            Log?.Invoke(L, LastIPConnected, ConexId, false, "SignOn NET");

            InMonitor.Remove(LastIPConnected);

            sendingSignOn = false;
        }
    }
}

