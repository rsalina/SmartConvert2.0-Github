using System;

namespace TCPSmart.Conexion
{
    public class ForHostEventArgs : EventArgs
    {
        internal ForHostEventArgs(String IPHostPrincipal, DesconexionType Razon, FromClienteDataReceived Data = null)
        {
            IPAddress = IPHostPrincipal;
            Datos = Data;
        }

        public String IPAddress { get; }

        public DesconexionType Motivo { get; }

        public FromClienteDataReceived Datos { get; }
    }
}
