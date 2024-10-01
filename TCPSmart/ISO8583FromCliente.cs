using System;

namespace TCPSmart
{
    /// <summary>
    /// CLASE EXCLUSIVA PARA PARSEO DE ISO8583, SE USARA SOLAMENTE EN CONEXIONES QUE LO REQUIERA
    /// AUTOR : IAMM 10/03/2020
    /// DEPRECATED
    /// </summary>
    public class ISO8583FromCliente
    {
        internal ISO8583FromCliente(Int32 ConexID, Byte[] Data, String IP)
        {
            ConexionID = ConexID;
            IPAddress = IP;

            DataRecibida = Data;
            try
            {
                // ISO8583 iso = new ISO8583(); //<== PARSER

                CadenaOriginalHEX = BitConverter.ToString(Data).Replace("-", "");

                // String Clean = BitConverter.ToString(TCPUtil.ParserHEXByteToISO8583(Data, TCPUtil.HeaderBytesCount)).Replace("-", "");
                // String HexRemoved = CadenaOriginalHEX.Substring(0, (CadenaOriginalHEX.Length - Clean.Length));

            }
            catch (Exception)
            {
                ValidInfoISO8583 = false;
            }

        }
        /// <summary>
        /// ID de Conexion en BD
        /// </summary>
        public Int32 ConexionID { get; }
        /// <summary>
        /// IPADDRESS que Envia la Cadena
        /// </summary>
        public String IPAddress { get; }
        /// <summary>
        /// BYTES recibidos sin ninguna modificacion
        /// </summary>
        public Byte[] DataRecibida { get; }
        /// <summary>
        /// INDICA SI SE PARSEO CORRECTAMENTE a ISO8583
        /// </summary>
        public Boolean ValidInfoISO8583 { get; }

        public String CadenaOriginalHEX { get; }

        public String[] HeadersHEX { get; }

    }
}
