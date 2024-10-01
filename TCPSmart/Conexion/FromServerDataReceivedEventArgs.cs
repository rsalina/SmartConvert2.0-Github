using System;

namespace TCPSmart.Conexion
{
    /// <summary>
    /// Arguments for data received from server events.
    /// </summary>
    public class FromServerDataReceivedEventArgs : EventArgs
    {
        internal FromServerDataReceivedEventArgs(Byte[] data, FromClienteDataReceived Datos)
        {
            Data = data;
            MainServerConexDatos = Datos;
        }

        /// <summary>
        /// Datos recibidos del servidor
        /// </summary>
        public byte[] Data { get; }
        /// <summary>
        /// Conexion Principal que provoca este llamada.
        /// </summary>
        public FromClienteDataReceived MainServerConexDatos { get; }
    }
}
