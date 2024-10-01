using System;

namespace TCPSmart.Conexion
{
    public class ClientDesConexEventArgs : EventArgs
    {
        /// <summary>
        /// Argumentos para los eventos de la desconexion de un cliente.
        /// </summary>
        /// <param name="IP"></param>
        /// <param name="Porto"></param>
        /// <param name="Razon"></param>
        internal ClientDesConexEventArgs(String IP, DesconexionType Razon)
        {
            IPAddress = IP;
            Motivo = Razon;
        }
        /// <summary>
        /// Direccion IP
        /// </summary>
        public String IPAddress { get; }

        /// <summary>
        /// Motivo por el que se desconecta al cliente.
        /// </summary>
        public DesconexionType Motivo { get; }
    }
}
