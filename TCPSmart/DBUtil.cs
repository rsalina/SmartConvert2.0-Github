using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Forms;
using TCPSmart.Flow;


namespace TCPSmart
{
    class DBUtil

    //Esta clase provee acceso a las tablas en la base de datos
    //Las funciones regresan una tabla.
    //por el momento cada llamada abre y cierra la conexión a la base de datos
    //porque las conexiones son caras, pero podría ser necesario cambiar este comportamiento
    //(es decir, mantener la conexión abierta).
    //Idealmente deberíamos usar EF aquí, pero al menos ahora, no parece necesario
    //en esta aplicación (René?)

    {
        public static Boolean SQLOffline = false;
        public static ConcurrentDictionary<Int32, List<SqlCommand>> CmdDictionaryOffLine = new ConcurrentDictionary<Int32, List<SqlCommand>>();
        public static string GetAppConnectionString()
        {
            return BuildConectionString(TCPUtil.ServerBD, TCPUtil.PuertoBD, TCPUtil.NameBD, TCPUtil.UserBD, TCPUtil.PassBD);
        }

        private static string BuildConectionString(string serverBD, int puertoBD, string nameBD, string userBD, string passBD)
        {
            SqlConnectionStringBuilder cnStr = new SqlConnectionStringBuilder();
            cnStr.DataSource = serverBD + "," + puertoBD.ToString();
            cnStr.UserID = userBD;
            cnStr.Password = passBD;
            cnStr.InitialCatalog = nameBD;
            cnStr.ConnectTimeout = 18; //<= 18 Segundos para intento de conexion
            return cnStr.ConnectionString;
        }

        public static void TestConnectionParameters(string serverBD, int puertoBD, string nameBD, string userBD, string passBD, bool testValidConnection = false)
        {
            if (serverBD == "")
            {
                //throw new System.ArgumentException("La URL/IP del Servidor MSSQL no puede ir en blanco");
                MessageBox.Show("La URL/IP del Servidor MSSQL no puede ir en blanco", TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (userBD == "")
            {
                //throw new System.ArgumentException("El Usuario de conexión MSSQL no puede ir en blanco");
                MessageBox.Show("El Usuario de conexión MSSQL no puede ir en blanco", TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (puertoBD == 0)
            {
                //throw new System.ArgumentException("El Puerto de conexión MSSQL no puede ser 0");
                MessageBox.Show("El Puerto de conexión MSSQL no puede ser 0", TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (nameBD == "")
            {
                //throw new System.ArgumentException("El Nombre de la BD, no puede ir en blanco");
                MessageBox.Show("El Nombre de la BD, no puede ir en blanco", TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (testValidConnection)
            {
                SqlConnection Cn = new SqlConnection(BuildConectionString(serverBD, puertoBD, nameBD, userBD, passBD));
                Cn.Open();
                Cn.Close();
            }
        }

        public static DataTable GetDataTable(string cnString, string sql)
        {
            DataTable dt = new DataTable();
            using (SqlConnection cn = new SqlConnection(cnString))
            {
                cn.Open();
                using (SqlDataAdapter da = new SqlDataAdapter(sql, cn))
                {
                    da.Fill(dt);
                    return dt;
                }
            }

        }

        public static int InserDeleteUpdateData(string cnString, string sql, Dictionary<string, object> parameters, bool isStoredProcedure = false)
        {
            using (SqlConnection cn = new SqlConnection(cnString))
            {
                cn.Open();
                var cmd = new SqlCommand(sql, cn);
                cmd.CommandType = isStoredProcedure ? CommandType.StoredProcedure : CommandType.Text;
                if (parameters != null)
                    foreach (var p in parameters)
                        cmd.Parameters.AddWithValue(p.Key, p.Value);

                return cmd.ExecuteNonQuery();
            }
        }

        public static async Task<int> InserDeleteUpdateDataAsync(string cnString, string sql, List<SqlParameter> parameters, bool isStoredProcedure = false, string alterno = "", Int32 Zezion = 0)
        {
            SqlConnection cn = new SqlConnection(cnString);
            var cmd = new SqlCommand(sql, cn);
            cmd.CommandType = isStoredProcedure ? CommandType.StoredProcedure : CommandType.Text;
            if (parameters != null)
            {
                cmd.Parameters.AddRange(parameters.ToArray());
            }
            try
            {
                TCPUtil.SaveBitacoraOffline(cmd, Zezion);
                using (cn)
                {
                    await cn.OpenAsync().ConfigureAwait(false);
                    SQLOffline = false;
                    return await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
                }
            }
            catch (Exception)
            {
                SQLOffline = true;
                return -1;
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                    cn.Close();
            }
        }

        public static DataTable GetNetManager()
        {
            DataTable dt = GetDataTable(GetAppConnectionString(), "SELECT * FROM NetManager"); ;
            dt.TableName = "NetManager";
            return dt;
        }
        /// <summary>
        /// Funcion para Obtener NetManager por ID
        /// Autor: IAMM
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static DataTable GetNetManagerById(Int32 Id)
        {
            DataTable dt = GetDataTable(GetAppConnectionString(), "SELECT * FROM NetManager WHERE Id = " + Id); ;
            dt.TableName = "NetManager";
            return dt;
        }

        public static DataTable GetConnectionMaster()
        {
            DataTable dt = GetDataTable(GetAppConnectionString(), "SELECT * FROM ConnectionMaster"); ;
            dt.TableName = "ConnectionMaster";
            return dt;
        }
        /// <summary>
        /// Funcion para Obtener Conexiones de un grupo NetManager por ID
        /// AUTOR: IAMM
        /// </summary>
        /// <param name="NetManager">ID de NetManager</param>
        /// <returns></returns>
        public static DataTable GetConnectionMaster(Int32 NetManager)
        {
            DataTable dt = GetDataTable(GetAppConnectionString(), "SELECT * FROM ConnectionMaster WHERE NetManagerId = " + NetManager); ;
            dt.TableName = "ConnectionMaster";
            return dt;
        }
        /// <summary>
        /// Funcion para Obtener Objecto ConectionMaster
        /// AUTOR: IAMM
        /// </summary>
        /// <param name="Id">Primary Key ID</param>
        /// <returns></returns>
        public static DataTable GetConnectionMasterByID(Int32 Id)
        {
            DataTable dt = GetDataTable(GetAppConnectionString(), "SELECT * FROM ConnectionMaster WHERE Id = " + Id); ;
            dt.TableName = "ConnectionMaster";
            return dt;
        }


        public static DataTable GetHSMManager()
        {
            DataTable dt = GetDataTable(GetAppConnectionString(), "SELECT * FROM HSMManager"); ;
            dt.TableName = "HSMManager";
            return dt;
        }

        public static DataTable GetHSMDetails()
        {
            DataTable dt = GetDataTable(GetAppConnectionString(), "SELECT * FROM HSMDetails"); ;
            dt.TableName = "HSMDetails";
            return dt;
        }

        public static DataTable GetParameters()
        {
            DataTable dt = GetDataTable(GetAppConnectionString(), "SELECT * FROM Parameter"); ;
            dt.TableName = "Parameter";
            return dt;
        }

        public static DataTable GetParameterByCode(string code)
        {
            DataTable dt = GetDataTable(GetAppConnectionString(), "SELECT * FROM Parameter WHERE ParameterCode = '" + code + "'"); ;
            dt.TableName = "Parameter";
            return dt;
        }

        public static DataTable GetSQL(string sql)
        {
            DataTable dt = GetDataTable(GetAppConnectionString(), sql);
            dt.TableName = "SqlTable";
            return dt;
        }

        public static DataTable GetBitacora(int connectionID = 0, int netManagerID = 0)
        {
            var sql = @"SELECT A.Id,Fecha,Evento,B.DescriptionName + ' (ID ' + CONVERT(VARCHAR(10),B.Id) + ')' ConnectionMaster,
            ClienteIP,C.DescriptionName + ' (ID ' + CONVERT(VARCHAR(10),C.Id) + ')' NetManager,Type,Sync FROM Bitacora A 
            LEFT JOIN ConnectionMaster B ON A.ConexionMasterId = B.Id LEFT JOIN NetManager C ON C.Id = A.NetManagerId";
            DataTable dt;
            if (connectionID == 0 && netManagerID == 0)
                dt = GetDataTable(GetAppConnectionString(), sql + " ORDER BY A.Id,ClienteIP ASC");
            else
                dt = GetDataTable(GetAppConnectionString(), sql + (connectionID == 0 ? " WHERE C.Id = " + netManagerID + " ORDER BY A.Id,ClienteIp ASC" : " WHERE A.ConexionMasterId = " + connectionID + " ORDER BY A.Id,ClienteIP ASC"));
            dt.TableName = "Bitacora";
            return dt;
        }

        public static int InsertBitacora(string evento, int conexionMasterId, string clienteIP, string type, Int32 NetID)
        {
            var sql = "INSERT INTO Bitacora(Fecha,Evento,ConexionMasterId,ClienteIP,NetManagerId,Type,Sync)VALUES(@Fecha,@Evento,@ConexionMasterId,@ClienteIP,@NetManagerId,@Type,@Sync)";
            var parameters = new Dictionary<string, object>();
            parameters.Add("Fecha", DateTime.Now);
            parameters.Add("Evento", evento);
            parameters.Add("ConexionMasterId", conexionMasterId);
            parameters.Add("ClienteIP", clienteIP);
            parameters.Add("NetManagerId", NetID);
            parameters.Add("Type", type);
            parameters.Add("Sync", false);

            return InserDeleteUpdateData(GetAppConnectionString(), sql, parameters);
        }

        public static async Task<int> InsertBitacoraASync(string evento, int conexionMasterId, string clienteIP, string type, Int32 NetID, Int32 Zezion = 0)
        {
            var sql = "INSERT INTO Bitacora(Fecha,Evento,ConexionMasterId,ClienteIP,NetManagerId,Type,Sync)VALUES(@Fecha,@Evento,@ConexionMasterId,@ClienteIP,@NetManagerId,@Type,@Sync)";

            List<SqlParameter> Rango = new List<SqlParameter>();
            Rango.Add(new SqlParameter("Fecha", DateTime.Now));
            Rango.Add(new SqlParameter("Evento", evento));
            Rango.Add(new SqlParameter("ConexionMasterId", conexionMasterId));
            Rango.Add(new SqlParameter("ClienteIP", clienteIP));
            Rango.Add(new SqlParameter("NetManagerId", NetID));
            Rango.Add(new SqlParameter("Type", type));
            Rango.Add(new SqlParameter("Sync", false));

            return await InserDeleteUpdateDataAsync(GetAppConnectionString(), sql, Rango);
        }

        public static int DeleteBitacora(bool allData, int connectionID = 0, int netManagerID = 0)
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("ConexionMasterId", connectionID);
            parameters.Add("NetManagerId", netManagerID);
            parameters.Add("DeleteAll", allData);
            return InserDeleteUpdateData(GetAppConnectionString(), "DeleteBitacora", parameters, true);
        }

        public static int UpdateBitacoraRow(List<int> ids)
        {
            var sql = "";
            foreach (var id in ids)
                sql += $"UPDATE Bitacora SET Sync = 1 WHERE Id = {id};";

            if (!string.IsNullOrWhiteSpace(sql))
                return InserDeleteUpdateData(GetAppConnectionString(), sql, null);
            else
                return 0;
        }

        public static Boolean InsertTransaction(Int32 IdNet, Int32 IdCon, String MTI, String F0, String F2, String F37, String F38, String F41, Decimal Monto)
        {
            Boolean Ret = false;
            SqlConnection Cn = new SqlConnection(GetAppConnectionString());
            var sql = "INSERT INTO Transactions (NetManagerId,ConexionId,MTI,F0,F2,F37,F38,F41,Monto,Reversed) VALUES(@IdNet,@IdCon,@Mti,@F0,@F2,@F37,@F38,@F41,@Mont,@Re)";
            var cmd = new SqlCommand(sql, Cn);
            cmd.Parameters.AddWithValue("IdNet", IdNet);
            cmd.Parameters.AddWithValue("IdCon", IdCon);
            cmd.Parameters.AddWithValue("Mti", MTI);
            cmd.Parameters.AddWithValue("F0", F0);
            cmd.Parameters.AddWithValue("F2", F2);
            cmd.Parameters.AddWithValue("F37", F37);
            cmd.Parameters.AddWithValue("F38", F38);
            cmd.Parameters.AddWithValue("F41", F41);
            cmd.Parameters.AddWithValue("Mont", Monto);
            cmd.Parameters.AddWithValue("Re", 0);
            try
            {
                using (Cn)
                {
                    if (Cn.State == ConnectionState.Closed)
                    {
                        Cn.Open();
                    }
                    cmd.ExecuteNonQuery();
                    Cn.Close();
                }
                Ret = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                TCPUtil.SaveBitacoraOffline(cmd);
                Ret = false;
            }

            return Ret;
        }

        public static Pair<String, String> GetTransaction(String F37, String F41, String F2, out Int32 Id_T)
        {
            Pair<String, String> Ret = new Pair<String, String>("", "");
            Id_T = 0;

            DataTable Xet = GetSQL("SELECT * FROM Transactions WHERE F2 ='" + F2 + "' AND F37='" + F37 + "' AND F41 = '" + F41 + "' AND Reversed = 0");

            foreach (DataRow item in Xet.Rows)
            {
                Ret.First = item["F0"].ToString();
                Ret.Second = item["F38"].ToString();
                Id_T = Convert.ToInt32(item["id"]);
            }

            return Ret;
        }



        /// <summary>
        /// AQUI METER TODAS LAS ACTUALIZACIONES PARA LA DB POR CODIGO
        /// BLOQUE TRYCATCH por SENTENCIA
        /// </summary>
        /// <returns></returns>
        public static void UpdateDataBase()
        {
            //UPDATE BUILD - 030520202311

            //Eliminar Columna BMPXML de Tabla NetManager
            try { ExecuteSQL("ALTER TABLE NetManager DROP COLUMN BMPXML"); }
#pragma warning disable CS0168 // La variable está declarada pero nunca se usa
            catch (Exception ex) { }
#pragma warning restore CS0168 // La variable está declarada pero nunca se usa

            try
            {
                ExecuteSQL("ALTER TABLE ConnectionMaster ADD BytesHeader numeric(18,0) NOT NULL");
                ExecuteSQL("ALTER TABLE ConnectionMaster ADD CONSTRAINT DF_ConnectionMaster_BytesHeader DEFAULT ((2)) FOR BytesHeader");
            }
#pragma warning disable CS0168 // La variable está declarada pero nunca se usa
            catch (Exception ex) { }
#pragma warning restore CS0168 // La variable está declarada pero nunca se usa

            //Creacion de tabla BitMapManager
            try
            {
                ExecuteSQL("CREATE TABLE [dbo].[BitmapsManager]" +
                "([Id][bigint] IDENTITY(1, 1) NOT NULL," +
                "[ConexionId][numeric](18, 0) NOT NULL," +
                "[MTI][nvarchar](5) NOT NULL," +
                "[BMP][nvarchar](50) NOT NULL," +
                "CONSTRAINT[PK_BitmapsManager] PRIMARY KEY CLUSTERED" +
                "(" +
                "[Id] ASC" +
                ")WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY])" +
                " ON[PRIMARY]");
            }
#pragma warning disable CS0168 // La variable está declarada pero nunca se usa
            catch (Exception ex) { }
#pragma warning restore CS0168 // La variable está declarada pero nunca se usa

            //END UPDATE BUILD - 030520202311
            //Update definitivo en CRUD NetManager 08052020

            try
            {
                //ExecuteSQL("ALTER TABLE NetManager ALTER COLUMN EchoMsg DROP CONSTRAINT DEFAULT");
                //ExecuteSQL("ALTER TABLE NetManager DROP COLUMN EchoMsg");
                //ExecuteSQL("ALTER TABLE NetManager DROP COLUMN EchoMsgSeconds");
                //ExecuteSQL("ALTER TABLE NetManager DROP COLUMN DynamicKey");
                //ExecuteSQL("ALTER TABLE NetManager DROP COLUMN DinamicKeyMinutes");
                //ExecuteSQL("ALTER TABLE NetManager DROP COLUMN TypeDinamicKey");
                //ExecuteSQL("ALTER TABLE NetManager DROP COLUMN HSMManagerId");  se comento porque se necesita para la generacion de llaves
                ExecuteSQL("ALTER TABLE NetManager DROP COLUMN ValidationManagerId");

                ExecuteSQL("ALTER TABLE ConnectionMaster ADD isValidator bit NULL");
                DataTable Cet = GetSQL("SELECT * FROM ConnectionMaster WHERE EndConnectionMasterId > 0");
                foreach (DataRow item in Cet.Rows)
                {
                    Int32 IdNew = Convert.ToInt32(item["Id"]);
                    Int32 IdConnection = Convert.ToInt32(item["EndConnectionMasterId"]);

                    ExecuteSQL("UPDATE ConnectionMaster SET EndConnectionMasterId = 0, isValidator = 1  WHERE Id=" + IdNew);
                    ExecuteSQL("UPDATE ConnectionMaster SET EndConnectionMasterId = " + IdNew + ", isValidator = 0 WHERE Id=" + IdConnection);
                }

                ExecuteSQL("UPDATE ConnectionMaster SET isValidator = 0 WHERE isValidator IS NULL");

                ExecuteSQL("ALTER TABLE NetManager ADD ValidateFeeF28 bit NOT NULL default 0");
                ExecuteSQL("ALTER TABLE NetManager ADD ValidateTimeOut bit NOT NULL default 0");
                ExecuteSQL("ALTER TABLE NetManager ADD TimeOut numeric(18,0) NOT NULL default 0");

                DataTable Xet = DBUtil.GetNetManager();
                foreach (DataRow item in Xet.Rows)
                {
                    Flow.NetManager mNetManager = new Flow.NetManager(Convert.ToInt32(item["Id"])); //Convertido a Objeto     

                    DataTable Net = DBUtil.GetSQL("SELECT * FROM ConnectionMaster WHERE TCPClient = 0 AND NetManagerId =" + mNetManager.Id);
                    foreach (DataRow item2 in Net.Rows)
                    {
                        ExecuteSQL("UPDATE NetManager SET ConnectionMasterId = " + Convert.ToInt32(item2["Id"]) + " WHERE Id=" + Convert.ToInt32(item["Id"]));
                    }
                }
            }
#pragma warning disable CS0168 // La variable está declarada pero nunca se usa
            catch (Exception ex) { }
#pragma warning restore CS0168 // La variable está declarada pero nunca se usa
            //UPDATE DEFINITIVO ConnectionMaster 11052020
            try
            {
                DataTable Xet = DBUtil.GetConnectionMaster();
                foreach (DataRow item in Xet.Rows)
                {
                    ConexMaster Cn = new ConexMaster(Convert.ToInt32(item["Id"]), 0);
                    if (Cn.TCPClient)
                    {
                        if (Cn.TypeMsg == 2 && Cn.IsValidator)
                        {
                            ExecuteSQL("UPDATE ConnectionMaster SET ConnectionType = 2 WHERE Id =" + Cn.Id);
                        }
                        if (Cn.TypeMsg != 2 && Cn.IsValidator)
                        {
                            ExecuteSQL("UPDATE ConnectionMaster SET ConnectionType = 1 WHERE Id =" + Cn.Id);
                        }
                        if (Cn.TypeMsg == 2 && !Cn.IsValidator)
                        {
                            ExecuteSQL("UPDATE ConnectionMaster SET ConnectionType = 2 WHERE Id =" + Cn.Id);
                        }
                    }
                }
                ExecuteSQL("ALTER TABLE ConnectionMaster DROP COLUMN NetManagerId");
                ExecuteSQL("ALTER TABLE ConnectionMaster DROP COLUMN isValidator");
            }
#pragma warning disable CS0168 // La variable está declarada pero nunca se usa
            catch (Exception ex) { }
#pragma warning restore CS0168 // La variable está declarada pero nunca se usa 

            try
            {
                ExecuteSQL("ALTER TABLE NetManager ADD ValidateCodeF3 bit NOT NULL default 0");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
            //Creacion de tabla Transactions
            try
            {
                ExecuteSQL("CREATE TABLE [dbo].[Transactions]" +
              "([Id][bigint] IDENTITY(1, 1) NOT NULL," +
              "[NetManagerId][numeric](18, 0) NOT NULL," +
              "[ConexionId][numeric](18, 0) NOT NULL," +
              "[MTI][nvarchar](5) NOT NULL," +
              "[F0][nvarchar](50) NOT NULL," +
              "[F2][nvarchar](50) NOT NULL," +
              "[F37][nvarchar](50) NOT NULL," +
              "[F41][nvarchar](50) NOT NULL," +
              "[F38][nvarchar](50) NOT NULL," +
              "[Reversed][bit] NOT NULL," +
              "[Monto][decimal](11,2) NOT NULL," +
              "CONSTRAINT[PK_Transactions] PRIMARY KEY CLUSTERED" +
              "(" +
              "[Id] ASC" +
              ")WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY])" +
              " ON[PRIMARY]");
            }
#pragma warning disable CS0168 // La variable está declarada pero nunca se usa
            catch (Exception ex) { }
#pragma warning restore CS0168 // La variable está declarada pero nunca se usa

            try
            {
                ExecuteSQL("CREATE TABLE [dbo].[WsParams]" +
              "([Id][bigint] IDENTITY(1, 1) NOT NULL," +
              "[Ambiente][nvarchar](3) NOT NULL," +
              "[Url][nvarchar](450) NOT NULL," +
              "[UserAtm][nvarchar](50) NOT NULL," +
              "[PwdAtm][nvarchar](50) NOT NULL," +
              "[PLogin_Id][nvarchar](100) NOT NULL," +
              "[Papp_Code][nvarchar](50) NOT NULL," +
              "[PLevl_Typ][nvarchar](50) NOT NULL," +
              "[PPasword][nvarchar](100) NOT NULL," +
              "[PMerchant][nvarchar](100) NOT NULL," +
              "[Papp_Vers][nvarchar](50) NOT NULL," +
              "[Active][bit] NOT NULL," +
              "CONSTRAINT[PK_WsParams] PRIMARY KEY CLUSTERED" +
              "(" +
              "[Id] ASC" +
              ")WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY])" +
              " ON[PRIMARY]");
            }
#pragma warning disable CS0168 // La variable está declarada pero nunca se usa
            catch (Exception ex)
#pragma warning disable CS0168 // La variable está declarada pero nunca se usa
            {

            }
            try
            {
                ExecuteSQL("ALTER TABLE WsParams ADD LicToken nvarchar(450) NULL");
            }
#pragma warning disable CS0168 // La variable está declarada pero nunca se usa
            catch (Exception)
#pragma warning disable CS0168 // La variable está declarada pero nunca se usa
            {

            }
            //try
            //{
            //    ExecuteSQL("ALTER TABLE WsParams ADD WsAlterno BIT NULL");
            //}
//#pragma warning disable CS0168 // La variable está declarada pero nunca se usa
//            catch (Exception)
//#pragma warning disable CS0168 // La variable está declarada pero nunca se usa
//            {

//            }
        }
        /// <summary>
        /// Ejecuta Sentencia al servidor
        /// </summary>
        /// <param name="sql"></param>
        public static void ExecuteSQL(string sql)
        {
            using (SqlConnection cn = new SqlConnection(GetAppConnectionString()))
            {
                SqlCommand command = new SqlCommand(sql, cn);
                command.Connection.Open();
                command.ExecuteNonQuery();
            }
        }


       
    }
}
