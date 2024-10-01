using System;
using System.IO;

namespace TCPSmart.Conexion
{
    public class FromClienteToken
    {
        public MemoryStream XMLQ { get; }
        public String IPAddress { get; }
        public String[] DataHEXHeaders { get; }
        public ServerTCP ServerFrom { get; }
        public String[] ISO8583Translated { get; }
        public String[] ISO8583Parsed { get; }
        public String MTIResponse { get; }



    }
}
