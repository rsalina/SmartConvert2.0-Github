using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;

namespace TCPSmart.Flow
{
    /// <summary>
    /// OBJECT CONEXION
    /// Autor: IAMM
    /// </summary>
    public class ConexMaster
    {
        internal ConexMaster(Int32 DbId, Int32 NetManager, Boolean UseValidatorF52 = false)
        {
            DataTable xet = DBUtil.GetConnectionMasterByID(DbId);
            foreach (DataRow item in xet.Rows)
            {
                NetManagerId = NetManager;
                Id = Convert.ToInt32(item["id"]);
                DescriptionName = item["DescriptionName"].ToString();
                Active = Convert.ToBoolean(item["Active"]);
                TCPClient = Convert.ToBoolean(item["TCPClient"]);
                IPAdress = item["IPAdress"].ToString();
                Port = Convert.ToInt32(item["Port"]);
                TypeMsg = Convert.ToInt32(item["TypeMsg"]);
                ConnectionType = Convert.ToInt32(item["ConnectionType"]);
                Url = item["Url"].ToString();
                TimeOutSec = Convert.ToInt32(item["TimeOutSecs"]);
                BytesHeader = Convert.ToInt32(item["BytesHeader"]);

                BitMaps = new ConcurrentDictionary<String, String>();
                DataTable Bet = DBUtil.GetSQL("SELECT * FROM BitmapsManager WHERE ConexionId=" + Id);

                foreach (DataRow Bm in Bet.Rows)
                {
                    String BMP = Bm["BMP"].ToString();
                    String MTI = Bm["MTI"].ToString();
                    BitMaps.TryAdd(MTI, BMP);
                }

                //Si usa Validador lo metemos antes del EndPoint destino

                if (UseValidatorF52)
                {
                    if (Convert.ToInt32(item["EndConnectionMasterId"]) > 0)
                    {
                        EndConnectionMasterId = new ConexMaster(
                            Id + 12340,
                            NetManager, false,
                            "Validador F52", true, TCPUtil.ParamKeyXMLServer,
                            TCPUtil.ParamKeyXMLPort, ConnectionType, 2,
                            TCPUtil.ParamKeyXMLTimeOut, 0192,
                            new ConexMaster(Convert.ToInt32(item["EndConnectionMasterId"]), NetManager));
                    }
                    else
                    {
                        EndConnectionMasterId = new ConexMaster(
                            Id + 12340,
                            NetManager, false,
                            "Validador F52", true, TCPUtil.ParamKeyXMLServer,
                            TCPUtil.ParamKeyXMLPort, ConnectionType, 2,
                            TCPUtil.ParamKeyXMLTimeOut, 0192,
                            null);
                    }


                }
                else
                {
                    if (Convert.ToInt32(item["EndConnectionMasterId"]) > 0)
                    {
                        EndConnectionMasterId = new ConexMaster(Convert.ToInt32(item["EndConnectionMasterId"]), NetManager);
                    }
                }

                IsFinalCn = EndConnectionMasterId == null; //Si ya no tiene destino, es la ultima conexion
            }
        }
        /// <summary>
        /// Crea conexmaster manualmente sin BD, se crea como un validador, es decir que no continuara hasta que se obtenta una respuesta de aqui
        /// </summary>
        /// <param name="id"></param>
        /// <param name="NetManager"></param>
        /// <param name="description"></param>
        /// <param name="tcpClient"></param>
        /// <param name="IP"></param>
        /// <param name="TypeMsg"></param>
        /// <param name="connType"></param>
        /// <param name="timeout"></param>
        /// <param name="bheader"></param>
        internal ConexMaster(Int32 id, Int32 NetManager, Boolean UseValidatorF52, String description, Boolean tcpClient, String IP, Int32 port, Int32 typeMsg, Int32 connType, Int32 timeout, Int32 bheader, ConexMaster end)
        {
            NetManagerId = NetManager;
            Id = id;
            DescriptionName = description;
            Active = true;
            TCPClient = tcpClient;
            IPAdress = IP;
            Port = port;
            TypeMsg = typeMsg;
            ConnectionType = connType;
            Url = "";
            TimeOutSec = timeout;
            BytesHeader = bheader;
            IsValidator = true;
            EndConnectionMasterId = end; //Envia Resultado al ConexionMaster Destino
        }

        public Int32 Id { get; }
        public String DescriptionName { get; }
        public Boolean Active { get; }
        public Boolean TCPClient { get; }
        public String IPAdress { get; set; }
        public Int32 Port { get; }
        public Int32 TypeMsg { get; set; }
        public Int32 ConnectionType { get; }
        public String Url { get; }
        public Int32 TimeOutSec { get; }
        public Int32 NetManagerId { get; }
        public ConexMaster EndConnectionMasterId { get; }
        public ConcurrentDictionary<String, String> BitMaps { get; }
        public Int32 BytesHeader { get; }
        public Boolean IsValidator { get; set; }

        public Boolean IsFinalCn { get; set; }
        /// <summary>
        /// Array Cadenas Original Parseada, con datos sin traducir.
        /// </summary>
        public Dictionary<Int32, String[]> ISOsParsed { get; set; }
        /// <summary>
        /// Array Cadenas Original Parseada, con datos traducidos.
        /// </summary>
        public Dictionary<Int32, String[]> ISOsTransl { get; set; }


    }
}
