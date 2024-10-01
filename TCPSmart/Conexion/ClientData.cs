using System;
using System.Net.Sockets;
using System.Threading;

namespace TCPSmart.Conexion
{
    internal class ClientData : IDisposable
    {
        private TcpClient _TcpCliente = null;
        private NetworkStream _NStream = null;
        private string _IP = null;
        private readonly object _Lock = new object();

        internal TcpClient Cliente
        {
            get { return _TcpCliente; }
        }

        internal NetworkStream NStream
        {
            get { return _NStream; }
        }

        internal String IPAddress
        {
            get { return _IP; }
        }


        internal object SendLock
        {
            get { return _Lock; }
        }

        internal CancellationTokenSource TokenSource { get; set; }

        internal CancellationToken Token { get; set; }


        internal ClientData(TcpClient tcp)
        {
            if (tcp == null) throw new ArgumentNullException(nameof(tcp));

            _TcpCliente = tcp;
            _NStream = tcp.GetStream();
            _IP = tcp.Client.RemoteEndPoint.ToString();
        }

        public void Dispose()
        {
            if (_NStream != null)
            {
                _NStream.Close();
                if (_NStream != null)
                {
                    _NStream.Dispose();
                }

                _NStream = null;
            }

            if (_TcpCliente != null)
            {
                _TcpCliente.Close();
                _TcpCliente.Dispose();
                _TcpCliente = null;
            }
        }
    }
}
