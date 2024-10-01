using System;

namespace TCPSmart.Conexion
{
    public class ClientConEventArgs : EventArgs
    {
        /// <summary>
        /// Argumentos para los eventos de una conexion cliente.
        /// </summary>
        /// <param name="IP"></param>
        /// <param name="Porto"></param>
        internal ClientConEventArgs(String IP)
        {
            IPAddress = IP;
        }

        /// <summary>
        /// Direccion IP del Socket Cliente
        /// </summary>
        public String IPAddress { get; }

    }
}
