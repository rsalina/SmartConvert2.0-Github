
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;

namespace TCPSmart
{
    /// <summary>
    /// DEPRECATED
    /// </summary>
    public static class MasterSocket
    {
        public class SocketClient
        {
            //DELEGADOS
            public delegate void BytesRecibidosHandlerEvento(int Bytes);
            public event BytesRecibidosHandlerEvento BytesRecibidos;

            public delegate void BytesEnviadosHandlerEvento(int Bytes);
            public event BytesRecibidosHandlerEvento BytesEnviados;

            public delegate void ConexionHandlerEvento(Int32 Socket);
            public event ConexionHandlerEvento Conectado;

            public delegate void ConexionErrorHandlerEvento(String Msj, Int32 Socket, Boolean ByTimeOut = false);
            public event ConexionErrorHandlerEvento ConexionError;

            public delegate void DesconexionHandlerEvento();
            public event DesconexionHandlerEvento Desconectado;

            public delegate void DesconexionErrorEventHandler(String Msj, Boolean ByTimeOut = false);
            public event DesconexionErrorEventHandler DesconexionError;

            public delegate void DataBinEntranteEventoHandler(Byte[] Data, Int32 Socket);
            public event DataBinEntranteEventoHandler DataBinEntrante;

            public delegate void DataEntranteErrorEventoHandler(String Msj);
            public event DataEntranteErrorEventoHandler DataEntranteError;

            public delegate void DataEntranteEventoHandler(String Data);
            public event DataEntranteEventoHandler DataEntrante;

            public delegate void EnviaDataErrorEventoHandler(String Msj);
            public event EnviaDataErrorEventoHandler EnviaDataError;

            private static List<WeakReference> _ENCList;
            private Socket SocketCli;
            private Thread MonitorCli;
            private Boolean StopMonitorCli;

            static SocketClient()
            {
                MasterSocket.SocketClient._ENCList = new List<WeakReference>();
            }

            public SocketClient()
            {
                MasterSocket.SocketClient.__ENCAdd2List(this);
                this.StopMonitorCli = false;
            }

            private static void __ENCAdd2List(object Value)
            {
                List<WeakReference> EncLst = MasterSocket.SocketClient._ENCList;
                Monitor.Enter(EncLst);
                try
                {
                    if (MasterSocket.SocketClient._ENCList.Count == MasterSocket.SocketClient._ENCList.Capacity)
                    {
                        int item = 0;
                        int count = MasterSocket.SocketClient._ENCList.Count - 1;
                        for (int i = 0; i <= count; i = checked(i + 1))
                        {
                            if (MasterSocket.SocketClient._ENCList[i].IsAlive)
                            {
                                if (i != item)
                                {
                                    MasterSocket.SocketClient._ENCList[item] = MasterSocket.SocketClient._ENCList[i];
                                }
                                item = checked(item + 1);
                            }
                        }
                        MasterSocket.SocketClient._ENCList.RemoveRange(item, checked(MasterSocket.SocketClient._ENCList.Count - item));
                        MasterSocket.SocketClient._ENCList.Capacity = MasterSocket.SocketClient._ENCList.Count;

                    }
                    MasterSocket.SocketClient._ENCList.Add(new WeakReference(RuntimeHelpers.GetObjectValue(Value)));
                }
                finally
                {
                    Monitor.Exit(_ENCList);
                }
            }

            public void Connect(String Address, Int32 Port, Int32 TimeOut, Int32 Socket)
            {
                if (this.SocketCli != null)
                {
                    if (this.SocketCli.Connected)
                    {
                        ConexionError?.Invoke("Este Socket ya tiene una conexion", Socket);
                    }
                    return;
                }
                if (Port < 0 | Port > 65536)
                {
                    ConexionError?.Invoke("Puerto para conexion cliente invalido", Socket);
                }
                if (Address == "")
                {
                    ConexionError?.Invoke("Direccion cliente invalida", Socket);
                }

                this.SocketCli = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
                {
                    ReceiveBufferSize = 65536,
                    SendBufferSize = 65536
                };

                try
                {
                    this.SocketCli.Connect(Address, Port);
                }
                catch (Exception ex)
                {
                    ConexionError?.Invoke(ex.Message, Socket);
                }

                if (this.SocketCli.Connected)
                {
                    this.StopMonitorCli = false;
                    Conectado?.Invoke(Socket);

                    this.MonitorCli = new Thread(() => this.MonitorSocketForData(Socket));
                    this.MonitorCli.Start();
                }
                else
                {
                    try
                    {
                        this.SocketCli.Close(TimeOut);
                        this.SocketCli = null;
                    }
                    catch (Exception)
                    {
                    }

                    ConexionError?.Invoke("Imposible conectar con la IP " + Address, Socket, true);
                }
            }

            public void Desconecta()
            {
                this.StopMonitorCli = true;
            }

            private void TerminaDesconexion()
            {
                if (this.SocketCli != null)
                {
                    try
                    {
                        this.SocketCli.Close();
                        this.SocketCli = null;
                        Desconectado?.Invoke();
                    }
                    catch (Exception ex)
                    {
                        DesconexionError?.Invoke(ex.Message);
                    }
                }
            }

            public Boolean IsConectado()
            {
                return this.SocketCli.Connected;
            }

            private void MonitorSocketForData(Int32 Socket)
            {
                int Item = 0;
                while (!this.StopMonitorCli)
                {
                    if (this.SocketCli == null)
                    {
                        this.Desconecta();
                    }
                    else if (!this.SocketCli.Connected)
                    {
                        this.Desconecta();
                    }
                    else if (this.SocketCli.Available > 0)
                    {
                        try
                        {
                            Byte[] Ava = new Byte[this.SocketCli.Available];
                            this.SocketCli.Receive(Ava, this.SocketCli.Available, SocketFlags.None);
                            BytesRecibidos?.Invoke(Ava.Length);

                            String RealString = TCPUtil.GetRealStringFromByte(Ava);
                            DataEntrante?.Invoke(RealString);

                            DataBinEntrante?.Invoke(Ava, Socket);
                        }
                        catch (Exception ex)
                        {
                            DataEntranteError?.Invoke(ex.Message);
                        }
                    }
                    Item = checked(Item + 1);
                    if (Item >= 500 & this.SocketCli.Connected)
                    {
                        try
                        {
                            if (this.SocketCli.Poll(-1, SelectMode.SelectRead) & this.SocketCli.Available <= 0)
                            {
                                this.Desconecta();
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        Item = 0;
                    }
                    Thread.Sleep(25);
                }
                this.TerminaDesconexion();
            }

            public void SendCadena(String Data)
            {
                this.SendByte(TCPUtil.GetBytesFromRealString(Data));
            }

            public void SendByte(Byte[] Data)
            {
                if (this.SocketCli == null)
                {
                    EnviaDataError?.Invoke("Este Socket no se ha conectado.");
                }
                else if (!this.SocketCli.Connected)
                {
                    EnviaDataError?.Invoke("Este Socket no esta conectado.");
                }
                else
                {
                    try
                    {
                        this.SocketCli.Send(Data);
                        BytesEnviados?.Invoke(Data.Length);
                    }
                    catch (Exception ex)
                    {
                        EnviaDataError?.Invoke(ex.Message);
                    }
                }

            }







        }

        public class SocketServer
        {
            //DELEGADOS EVENTOS
            public delegate void BytesRecibidosHandlerEvento(int Socket, int Bytes);
            public event BytesRecibidosHandlerEvento BytesRecibidos;

            public delegate void BytesEnviadosHandlerEvento(int Socket, int Bytes);
            public event BytesRecibidosHandlerEvento BytesEnviados;

            public delegate void ConexionHandlerEvento(int Socket, String Remote);
            public event ConexionHandlerEvento Conectado;

            public delegate void ConexionErrorHandlerEvento(int Socket, String Msj);
            public event ConexionErrorHandlerEvento ConexionError;

            public delegate void ConexionRechazadaHandlerEvento(String Msj);
            public event ConexionRechazadaHandlerEvento ConexionRechazada;

            public delegate void DesconexionHandlerEvento(int Socket);
            public event DesconexionHandlerEvento Desconectado;

            public delegate void DesconexionErrorEventHandler(int Socket, String Msj);
            public event DesconexionErrorEventHandler DesconexionError;

            public delegate void DataBinEntranteEventoHandler(int Socket, Byte[] Data);
            public event DataBinEntranteEventoHandler DataBinEntrante;

            public delegate void DataEntranteErrorEventoHandler(int Socket, String Msj);
            public event DataEntranteErrorEventoHandler DataEntranteError;

            public delegate void DataEntranteEventoHandler(int Socket, String Data);
            public event DataEntranteEventoHandler DataEntrante;

            public delegate void MonitorErrorEventHandler(String Msj);
            public event MonitorErrorEventHandler MonitorError;

            public delegate void EnviaDataErrorEventoHandler(int Socket, String Msj);
            public event EnviaDataErrorEventoHandler EnviaDataError;

            private static List<WeakReference> _ENCList;
            private Socket[] Sockets;
            private Boolean[] StopMonitor;
            private Thread[] SocketMonitor;
            private TcpListener Listener;
            private Thread ListenerTH;
            private Boolean StopListening;
            private Boolean AcceptAllIP;
            private String[] ValidRemotes;

            static SocketServer()
            {
                MasterSocket.SocketServer._ENCList = new List<WeakReference>();
            }

            private static void _ENCAdd2List(object value)
            {
                List<WeakReference> ENCLst = MasterSocket.SocketServer._ENCList;
                Monitor.Enter(ENCLst);
                try
                {
                    if (MasterSocket.SocketServer._ENCList.Count == MasterSocket.SocketServer._ENCList.Capacity)
                    {
                        int item = 0;
                        int count = checked(MasterSocket.SocketServer._ENCList.Count - 1);
                        for (int i = 0; i < count; i = checked(i + 1))
                        {
                            if (MasterSocket.SocketServer._ENCList[i].IsAlive)
                            {
                                if (i != item)
                                {
                                    MasterSocket.SocketServer._ENCList[item] = MasterSocket.SocketServer._ENCList[i];
                                }
                                item = checked(item + 1);
                            }
                        }

                        MasterSocket.SocketServer._ENCList.RemoveRange(item, checked(MasterSocket.SocketServer._ENCList.Count - item));
                        MasterSocket.SocketServer._ENCList.Capacity = MasterSocket.SocketServer._ENCList.Count;
                    }

                    MasterSocket.SocketServer._ENCList.Add(new WeakReference(RuntimeHelpers.GetObjectValue(value)));
                }
                finally
                {
                    Monitor.Exit(_ENCList);
                }
            }

            public SocketServer()
            {
                MasterSocket.SocketServer._ENCAdd2List(this);
                this.StopListening = false;
            }

            public void Desconectar(Int32 Socket)
            {
                this.StopMonitor[Socket] = true;
            }

            private void TerminarDesconexion(Int32 Socket)
            {
                if (this.Sockets[Socket] != null)
                {
                    try
                    {
                        this.Sockets[Socket].Close();
                        this.Sockets[Socket] = null;
                        this.Desconectado?.Invoke(Socket);
                    }
                    catch (Exception ex)
                    {
                        this.DesconexionError?.Invoke(Socket, ex.Message);
                    }
                }
            }

            private Int32 GetSocketLibre()
            {
                if (this.Sockets == null)
                {
                    return -1;
                }
                else if (this.Sockets.Length != 0)
                {
                    int lgt = this.Sockets.Length - 1;
                    int num = 0;
                    while (num <= lgt)
                    {
                        if (this.Sockets[num] == null)
                        {
                            return num;
                        }
                        else if (this.Sockets[num].Connected)
                        {
                            return num + 1;
                        }
                        else
                        {
                            return num;
                        }
                    }
                    return -1;
                }
                else
                {
                    return -1;
                }

            }

            public void Listen(Int32 SocketCount, Int32 Port, String[] ValidRemotes = null)
            {
                if (this.Listener != null)
                {
                    this.MonitorError?.Invoke("El Servidor ya fue iniciado");
                }
                else if (SocketCount < 1)
                {
                    this.MonitorError?.Invoke("Debes de Abrir al menos un Socket");
                }
                else if (!(Port < 0 | Port > 65535))
                {
                    if (ValidRemotes == null)
                    {
                        this.ValidRemotes = new string[] { "*.*.*.*" };
                        this.AcceptAllIP = true;
                    }
                    else
                    {
                        this.ValidRemotes = ValidRemotes;
                        this.AcceptAllIP = false;
                    }

                    this.Sockets = new Socket[SocketCount];
                    this.StopMonitor = new Boolean[SocketCount];
                    this.SocketMonitor = new Thread[SocketCount];

                    this.Listener = new TcpListener(IPAddress.Any, Port);
                    this.Listener.Start(SocketCount);

                    this.ListenerTH = new Thread(new ThreadStart(this.MonitorListnerForConex));
                    this.ListenerTH.Start();

                }
                else
                {
                    this.MonitorError?.Invoke("Puerto Invalido");
                }
            }

            private void MonitorListnerForConex()
            {
                while (!this.StopListening)
                {
                    if (this.Listener != null)
                    {
                        for (int i = this.GetSocketLibre(); this.Listener.Pending() & i >= 0; i = this.GetSocketLibre())
                        {
                            try
                            {
                                Socket S = this.Listener.AcceptSocket();
                                if (!AcceptAllIP)
                                {
                                    if (EsIpPermitida(S.RemoteEndPoint.ToString()))
                                    {
                                        String IPCancel = S.RemoteEndPoint.ToString();
                                        S.Close();
                                        this.ConexionRechazada?.Invoke("Direccion remota " + IPCancel + " bloqueada, no es una IP de Confianza");
                                    }
                                }
                                else
                                {
                                    this.StopMonitor[i] = false;
                                    this.Sockets[i] = S;
                                    this.Sockets[i].ReceiveBufferSize = 8192;
                                    this.Sockets[i].SendBufferSize = 192;          //<=== Minimizar numero de bytes enviados                          

                                    this.Conectado?.Invoke(i, this.Sockets[i].RemoteEndPoint.ToString());

                                    this.SocketMonitor[i] = new Thread(new ParameterizedThreadStart(this.MonitorSocketForData));
                                    this.SocketMonitor[i].Start(i);
                                }

                            }
                            catch (Exception ex)
                            {
                                this.ConexionError?.Invoke(i, ex.Message);
                            }
                        }
                    }
                    Thread.Sleep(500);
                    Application.DoEvents();
                }
                this.Listener.Stop();
                this.Listener = null;
            }

            private void MonitorSocketForData(object Args)
            {
                Int32 Val = Convert.ToInt32(Args);

                Int32 Conter = 0;
                while (!this.StopMonitor[Val])
                {
                    if (this.Sockets[Val] == null)
                    {
                        this.Desconectar(Val);
                    }
                    else if (!this.Sockets[Val].Connected)
                    {
                        this.Desconectar(Val);
                    }
                    else if (this.Sockets[Val].Available > 0)
                    {
                        try
                        {
                            Int32 Ava = this.Sockets[Val].Available;
                            Byte[] nByteArray = new Byte[Ava];
                            this.Sockets[Val].Receive(nByteArray, Ava, SocketFlags.None);

                            this.BytesRecibidos?.Invoke(Val, nByteArray.Length);

                            String Data = TCPUtil.GetRealStringFromByte(nByteArray);
                            this.DataEntrante?.Invoke(Val, Data);

                            this.DataBinEntrante?.Invoke(Val, nByteArray);
                        }
                        catch (Exception ex)
                        {
                            DataEntranteError?.Invoke(Val, ex.Message);
                        }
                    }
                    Conter++;
                    if (Conter >= 500 & this.Sockets[Val].Connected)
                    {
                        try
                        {
                            if (this.Sockets[Val].Poll(-1, SelectMode.SelectRead) & this.Sockets[Val].Available <= 0)
                            {
                                this.Desconectar(Val);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error en MonitorSocketForData " + Environment.NewLine + ex.Message);
                        }
                        Conter = 0;
                    }
                    Thread.Sleep(250);
                    Application.DoEvents();
                }
                this.TerminarDesconexion(Val);
            }

            public void EnviaString(Int32 Socket, String Data)
            {
                Byte[] Bytes2Send = TCPUtil.GetBytesFromRealString(Data);
                this.EnviaBytes(Socket, Bytes2Send);
            }

            public void EnviaBytes(Int32 Socket, Byte[] Data, Boolean DobleAction = false)
            {
                if (this.Sockets[Socket] == null)
                {
                    this.EnviaDataError?.Invoke(Socket, "El Socket " + Socket + " ha expirado, no esta conectado a nada");
                }
                else if (!this.Sockets[Socket].Connected)
                {
                    this.EnviaDataError?.Invoke(Socket, "El Socket " + Socket + ", no esta conectado a ningun dispositivo remoto");
                }
                else
                {
                    try
                    {
                        if (DobleAction)
                        {
                            this.Sockets[Socket].Send(Data);
                            this.Sockets[Socket].Blocking = false;
                            this.Sockets[Socket].Send(Data);
                        }
                        else
                        {
                            //this.Sockets[Socket].Blocking = false;
                            this.Sockets[Socket].NoDelay = true;

                            NetworkStream Ns = new NetworkStream(this.Sockets[Socket]);

                            if (Ns.CanWrite)
                            {
                                Ns.Write(Data, 0, Data.Length);
                                Ns.Flush();
                            }
                            //Test Server
                            Ns.Close();


                            int Enviados = this.Sockets[Socket].Send(Data, Data.Length, SocketFlags.DontRoute);



                            //Byte[] nTest = new Byte[32];
                            //this.Sockets[Socket].NoDelay = true;
                            //this.Sockets[Socket].Send(nTest);
                            ////this.Sockets[Socket].BeginSend(Data, 0, Data.Length, 0, new AsyncCallback(SendCallBack), this.Sockets[Socket]);

                            ////EndPoint ipR = new EndPoint();
                            ////ipR.Create(this.Sockets[Sockets].AddressFamily)
                            ////this.Sockets[Socket].Bind(EndPoint)
                            //this.Sockets[Socket].Blocking = false;


                        }

                        this.BytesEnviados?.Invoke(Socket, Data.Length);
                    }
                    catch (Exception ex)
                    {
                        this.EnviaDataError?.Invoke(Socket, ex.Message);
                    }
                }

            }

            private void SendCallBack(IAsyncResult ar)
            {
                Socket Handler = (Socket)ar.AsyncState;
                int byteSent = Handler.EndSend(ar);

                this.BytesEnviados?.Invoke(this.Sockets.Count() - 1, byteSent);

            }

            public void StopListen(Boolean KillAllConex)
            {
                this.StopListening = true;
                if (KillAllConex)
                {
                    this.KillAllConexs();
                }
            }

            public void KillAllConexs()
            {
                if (this.Sockets != null)
                {
                    for (int i = 0; i < this.Sockets.Length; i++)
                    {
                        this.Desconectar(i);
                    }
                }
            }

            public Boolean EsIpPermitida(String RemoteIP)
            {
                return ValidRemotes.Contains(RemoteIP);
            }

        }
    }
}
