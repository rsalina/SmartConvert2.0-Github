namespace TCPSmart.Conexion
{
    /// <summary>
    /// Razon por la que un cliente se Desconecta.
    /// </summary>
    public enum DesconexionType
    {
        /// <summary>
        /// Desconexion Natural
        /// </summary>
        Normal = 0,
        /// <summary>
        /// Desconexion intencional por servidor o la programacion
        /// </summary>
        Kicked = 1,
        /// <summary>
        /// Desconexion por TimeOut
        /// </summary>
        Timeout = 2,
        /// <summary>
        /// Filler para pasar Argumentos
        /// </summary>
        None = 4,

        SocketException = 6


    }
}
