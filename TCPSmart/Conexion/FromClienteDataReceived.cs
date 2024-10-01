

using System;

namespace TCPSmart.Conexion
{
    public class FromClienteDataReceived
    {
        /// <summary>
        /// Argumentos para el evento de recepcion de informacion desde el Cliente.
        /// </summary>
        /// <param name="IP"></param>
        /// <param name="Porto"></param>
        /// <param name="Data"></param>
        internal FromClienteDataReceived(String IP, Byte[] Data)
        {
            IPAddress = IP;

            DataOrigen = Data;
        }
        /// <summary>
        /// Direccion IP del Cliente.
        /// </summary>
        public String IPAddress { get; }
        /// <summary>
        /// Datos Originales
        /// </summary>
        public Byte[] DataOrigen { get; }
        /// <summary>
        /// Canal por donde viajan los datos
        /// </summary>      
    }
}
