using System;

namespace TCPSmart.Flow
{
    public class ConexStatics
    {
        public Int32 ID { get; set; }
        public Int32 NetManagerID { get; set; }
        public String Nombre { get; set; }
        public Int32 Status { get; set; }
        public DateTime Inicio { get; set; }
        public String UpTime { get; set; }
        public Int32 Recibidas { get; set; }
        public Int32 Enviadas { get; set; }
        public String Tipo { get; set; }
    }
}
