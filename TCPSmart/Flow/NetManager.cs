using System;
using System.Collections.Concurrent;
using System.Data;
using TCPSmart.Conexion;

namespace TCPSmart.Flow
{
    public class NetManager
    {
        internal NetManager(Int32 NetidDB)
        {
            DataTable xet = DBUtil.GetNetManagerById(NetidDB);

            if (xet.Rows.Count > 0)
            {
                DataRow item = xet.Rows[0]; //<== Unica fila

                Id = Convert.ToInt32(item["Id"]);
                DescriptionName = item["DescriptionName"].ToString();
                ValidateTimeOut = Convert.IsDBNull(item["ValidateTimeOut"]) ? false : Convert.ToBoolean(item["ValidateTimeOut"]);
                TimeOut = Convert.IsDBNull(item["TimeOut"]) ? 0 : Convert.ToInt32(item["TimeOut"]);

                if (!Convert.IsDBNull(item["ConnectionMasterId"]))
                {
                    ConnectionMasterId = new ConexMaster(Convert.ToInt32(item["ConnectionMasterId"]), NetidDB); //Servdor Ahora
                }
                else
                {
                    throw new Exception("La conexion MasterId es invalida, revise la configuracion de " + DescriptionName);
                }


            }
            //<=========================TIPO Cadena y envio

            BuildType = BuildTypeSend.BuildByBMP;


            Zesiones = new ConcurrentDictionary<String, Int32>();

            ConexMServer = ConnectionMasterId;
            ServerNetManager = new ServerTCP(this);

            ConexMServer.IPAdress = ServerNetManager.IPLISTENER;

            if (ConexMServer.TimeOutSec > 0)
            {
                ServerNetManager.IdleClienteTimeOut = ConexMServer.TimeOutSec;
            }
            try
            {
                ThreePartConex = ConexMServer.EndConnectionMasterId.EndConnectionMasterId != null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                ThreePartConex = false;
            }
        }

        public Int32 Id { get; }
        public String DescriptionName { get; }
        public ConexMaster ConnectionMasterId { get; }
        public Boolean ValidateTimeOut { get; set; }
        public Int32 TimeOut { get; set; }
        public BuildTypeSend BuildType { get; }
        public ServerTCP ServerNetManager { get; }
        public ConexMaster ConexMServer { get; }
       // public String WK_ID { get; set; }
        public Boolean ThreePartConex { get; }
        public ConcurrentDictionary<String, Int32> Zesiones { get; set; }
    }
}
