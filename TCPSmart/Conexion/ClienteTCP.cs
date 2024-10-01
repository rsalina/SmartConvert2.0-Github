using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TCPSmart.Flow;

namespace TCPSmart.Conexion
{
    /// <summary>
    /// Cliente TCP.  
    /// Set the Connected, Disconnected, and DataReceived callbacks.  
    /// Once set, use Connect() to connect to the server.
    /// </summary>
    public class ClienteTCP : IDisposable
    {
        #region Public-Members

        /// <summary>
        /// Callback to call when the connection is established.
        /// </summary>
        public event EventHandler<ForHostEventArgs> Conectado;

        /// <summary>
        /// Callback to call when the connection is destroyed.
        /// </summary>
        public event EventHandler<ForHostEventArgs> Desconectado;

        /// <summary>
        /// Callback to call when byte data has become available from the server.
        /// </summary>
        public event EventHandler<FromServerDataReceivedEventArgs> DataReceived;

        /// <summary>
        /// Receive buffer size to use while reading from the TCP server.
        /// </summary>
        public int ReceiveBufferSize
        {
            get
            {
                return _ReceiveBufferSize;
            }
            set
            {
                if (value < 1) throw new ArgumentException("ReceiveBuffer must be one or greater.");
                if (value > 65536) throw new ArgumentException("ReceiveBuffer must be less than 65,536.");
                _ReceiveBufferSize = value;
            }
        }

        /// <summary>
        /// The number of seconds to wait when attempting to connect.
        /// </summary>
        public int ConnectTimeoutSeconds
        {
            get
            {
                return _ConnectTimeoutSeconds;
            }
            set
            {
                if (value < 1) throw new ArgumentException("ConnectTimeoutSeconds must be greater than zero.");
                _ConnectTimeoutSeconds = value;
            }
        }

        /// <summary>
        /// Enable or disable acceptance of invalid SSL certificates.
        /// </summary>
        public bool AcceptInvalidCertificates = true;

        /// <summary>
        /// Enable or disable mutual authentication of SSL client and server.
        /// </summary>
        public bool MutuallyAuthenticate = true;

        /// <summary>
        /// Indicates whether or not the client is connected to the server.
        /// </summary>
        public bool IsConnected
        {
            get
            {
                return _Connected;
            }
            private set
            {
                _Connected = value;
            }
        }

        /// <summary>
        /// Access TCP statistics.
        /// </summary>
        public Bitacora Stats
        {
            get
            {
                return _Stats;
            }
        }

        public ConexMaster Conex { get; set; }

        public TcpClient Channel
        {
            get
            {
                return _TcpClient;
            }
        }

        /// <summary>
        /// Method to invoke to send a log message.
        /// </summary>
        public Action<string> Log = null;

        #endregion

        #region Private-Members

        private int _ReceiveBufferSize = 4096;
        private int _ConnectTimeoutSeconds = 5;
        private string _ServerIp;
        private IPAddress _IPAddress;
        private int _Port;
        private TcpClient _TcpClient;
        private NetworkStream _NetworkStream;

        private readonly object _SendLock = new object();
        private bool _Connected = false;

        private CancellationTokenSource _TokenSource = new CancellationTokenSource();
        private CancellationToken _Token;

        private Bitacora _Stats = new Bitacora();

        #endregion

        #region Constructors-and-Factories

        /// <summary>
        /// Instantiates the TCP client.  Set the Connected, Disconnected, and DataReceived callbacks.  Once set, use Connect() to connect to the server.
        /// </summary>
        /// <param name="serverIp">The server IP address.</param>
        /// <param name="port">The TCP port on which to connect.</param>
        public ClienteTCP(string serverIp, int port, ConexMaster Cn)
        {
            if (String.IsNullOrEmpty(serverIp)) throw new ArgumentNullException(nameof(serverIp));
            if (port < 0) throw new ArgumentException("Puerto debe ser mayor a 0");

            _Token = _TokenSource.Token;
            _ServerIp = serverIp;
            _IPAddress = IPAddress.Parse(_ServerIp);
            _Port = port;
            _TcpClient = new TcpClient();
            Conex = Cn;
        }

        #endregion

        #region Public-Methods

        /// <summary>
        /// Dispose of the TCP client.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Establish the connection to the server.
        /// </summary>
        public void Connect(FromClienteDataReceived Datos)
        {
            IAsyncResult ar = _TcpClient.BeginConnect(_ServerIp, _Port, null, null);
            WaitHandle wh = ar.AsyncWaitHandle;

            try
            {
                if (!ar.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(_ConnectTimeoutSeconds), false))
                {
                    _TcpClient.Close();
                    Desconectado?.Invoke(this, new ForHostEventArgs(Datos.IPAddress, DesconexionType.Timeout, Datos));
                    throw new TimeoutException("Timeout connecting to " + _ServerIp + ":" + _Port);
                }

                _TcpClient.EndConnect(ar);
                _NetworkStream = _TcpClient.GetStream();

                _Connected = true;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                wh.Close();
            }

            if (_Connected)
            {
                //<< FIX ISSUE #5 : SE CORRIJE LA CLASE PARA USAR EN OTRO TIPO DE FLUJOS
                ForHostEventArgs HEventArgs = null;
                if (Datos != null)
                {
                    HEventArgs = new ForHostEventArgs(Datos.IPAddress, DesconexionType.None, Datos);
                }

                Conectado?.Invoke(this, HEventArgs);

                Task.Run(async () => await DataReceiver(_Token, Datos), _Token);
            }
        }

        /// <summary>
        /// Send data to the server.
        /// </summary> 
        /// <param name="data">Byte array containing data to send.</param>
        public void Send(byte[] data)
        {
            if (data == null || data.Length < 1) throw new ArgumentNullException(nameof(data));
            if (!_Connected) throw new IOException("Not connected to the server; use Connect() first.");

            lock (_SendLock)
            {
                _NetworkStream.Write(data, 0, data.Length);
                _NetworkStream.Flush();
            }

            _Stats.BytesEnviados += data.Length;
        }

        /// <summary>
        /// Send data to the server.
        /// </summary>
        /// <param name="data">String containing data to send.</param>
        public void Send(string data)
        {
            if (String.IsNullOrEmpty(data)) throw new ArgumentNullException(nameof(data));
            Send(Encoding.UTF8.GetBytes(data));
        }

        #endregion

        #region Private-Methods

        /// <summary>
        /// Dispose of the TCP client.
        /// </summary>
        /// <param name="disposing">Dispose of resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _Connected = false;

                if (_TokenSource != null)
                {
                    if (!_TokenSource.IsCancellationRequested) _TokenSource.Cancel();
                    _TokenSource.Dispose();
                    _TokenSource = null;
                }

                if (_NetworkStream != null)
                {
                    _NetworkStream.Close();
                    _NetworkStream.Dispose();
                    _NetworkStream = null;
                }

                if (_TcpClient != null)
                {
                    _TcpClient.Close();
                    _TcpClient.Dispose();
                    _TcpClient = null;
                }

                Log?.Invoke("[ClienteTCP] Dispose complete");
            }
        }


        private async Task DataReceiver(CancellationToken token, FromClienteDataReceived Datos)
        {
            Boolean Recibo = false;
            try
            {
                while (true)
                {
                    if (token.IsCancellationRequested
                        || _TcpClient == null
                        || !_TcpClient.Connected)
                    {
                        Log?.Invoke("[ClienteTCP] Disconnection detected");
                        break;
                    }

                    byte[] data = await DataReadAsync(token);
                    if (data == null)
                    {
                        await Task.Delay(30);
                        continue;
                    }

                    if (Datos != null)
                    {
                        DataReceived?.Invoke(this, new FromServerDataReceivedEventArgs(data, Datos));
                    }
                    else
                    {
                        DataReceived?.Invoke(this, new FromServerDataReceivedEventArgs(data, null));
                    }


                    _Stats.BytesRecibidos += data.Length;
                    Recibo = true;
                }
            }
            catch (SocketException)
            {
                //  if (ex.ErrorCode == 997)
                //  {
                //      Log?.Invoke("[ClienteTCP] Data receiver socket exception (disconnection)");
                //      Desconectado.Invoke(this, new ForHostEventArgs(Datos.IPAddress, DesconexionType.SocketException));
                //   }
                //  else
                //   {
                Log?.Invoke("[ClienteTCP] Data receiver socket exception (disconnection)");
                //throw ex;
                //   }
            }
            catch (Exception e)
            {
                Log?.Invoke("[ClienteTCP] Data receiver exception:" +
                    Environment.NewLine +
                    e.ToString() +
                    Environment.NewLine);
            }

            _Connected = false;
            if (Datos != null)
            {
                Desconectado.Invoke(this, Recibo ? new ForHostEventArgs(Datos.IPAddress, DesconexionType.Normal) : new ForHostEventArgs(Datos.IPAddress, DesconexionType.Kicked, Datos));
            }
            else
            {
                Desconectado.Invoke(this, new ForHostEventArgs("127.0.0.1", DesconexionType.Kicked));
            }

        }

        private async Task<byte[]> DataReadAsync(CancellationToken token)
        {
            if (_TcpClient == null
                || !_TcpClient.Connected
                || token.IsCancellationRequested)
                throw new OperationCanceledException();

            if (!_NetworkStream.CanRead)
                throw new IOException();


            byte[] buffer = new byte[_ReceiveBufferSize];
            int read = 0;

            using (MemoryStream ms = new MemoryStream())
            {
                while (true)
                {
                    read = await _NetworkStream.ReadAsync(buffer, 0, buffer.Length);

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

        #endregion
    }
}

