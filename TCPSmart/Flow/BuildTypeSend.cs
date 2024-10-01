namespace TCPSmart.Flow
{
    /// <summary>
    /// Tipo de Armado de cadena y Envio
    /// Autor: IAMM
    /// </summary>
    public enum BuildTypeSend
    {
        /// <summary>
        /// Cuando La propiedad F52 es true y la connectionmanagerid en typemsg y connectiontype = 2
        /// </summary>
        BuildF52XML = 234,
        /// <summary>
        /// Cuando la propiedad F52 es false y la connectionmasterid en connectiontype <> 1
        /// </summary>
        BuildByBMP = 876,
        /// <summary>
        /// Cuando la propiedad F52 es false y la connectionmasterid en connectiontype = 1
        /// </summary>
        ByPass = 539
    }
}
