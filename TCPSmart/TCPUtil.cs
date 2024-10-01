
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using TCPSmart.Conexion;
using TCPSmart.Flow;
using TCPSmart.Ws;

namespace TCPSmart
{
    /// <summary>
    /// Clase Helper para Cadenas y Parametros
    /// </summary>

    public class TCPUtil
    {
        public static String AppName = "SmartConvert - V.2024Q3M3"; //2024 año + Q3 trimetres 3 + M3 Mes 3 control de version compilado
        public static String ConfigFile = Environment.CurrentDirectory + Path.DirectorySeparatorChar;

        public static List<ConexStatics> Statics;

        private static List<Int32> PuertosReservados =
            new List<Int32>() { 1, 21, 22, 23, 25, 53, 80, 110, 115, 135, 139, 143, 194, 443, 445, 1433, 3306, 3389, 5900 };

        public static String ServerBD = "";
        public static String UserBD = "";
        public static String PassBD = "";
        public static Int32 PuertoBD = 1433;
        public static String NameBD = "";

        public static Int32 ContadorSession = 100;

        public static String BASICBMP810i = "022000000A0000020400000000000000";
        public static String BASICBMPi = "722020810EC08006";
        public static String BASICBMP810w = "02200000820000000400000000000000";
        public static String BASICBMPw = "723880810EC00000";

        public static String BASEFORSIGNON = "003E0000160102003E00000088191600000000000000000000000800822000000800000004000000000000000113220845010063F2F0F1F3F2F2F0F8F4F5F3F20071";

        public static Boolean MTI110F2 = false;
        public static Boolean MTI110F38 = false;
        public static Boolean MTI110F54 = false;
        public static String Pin_Ver_Req = "";
        public static String Wk_ID = "";
        public static Int32 HeaderBytesCount = 0;

        public static Boolean DebugMode = false;
        public static ConcurrentDictionary<Int32, Int32> ConexsUnderMonitor = new ConcurrentDictionary<Int32, Int32>();
        public static DataTable ManagerStats;
        //<========= PARAMETROS
        public static Int32 ParamBMPTimeOut = 0;
        public static Int32 ParamConsecutiveXML = 0;
        public static String ParamNetworkAdapter = "";
        public static String ParamDirectIPServer = "";
        public static String ParamKeyXMLServer = "";
        public static Int32 ParamKeyXMLPort = 0;
        public static Int32 ParamKeyXMLTimeOut = 0;
        public static Int32 ParamSignOnMinutes = 0;
        public static String BMPEWA200Response = "";
        public static WsAmbiente WsAmbientForOperation = new WsAmbiente();



        //==========>>>>>>>>>>>>>>>>

        public static String FileLog = "";
        public static String DataInLog = "";

        public static Dictionary<String, WsAmbiente> WsAmbientes = new Dictionary<string, WsAmbiente>();

        public static void SetupWsAmbientes()
        {
            DataTable ws = DBUtil.GetSQL("SELECT Id,Ambiente,PLevl_Typ,PMerchant,PLogin_Id,UserAtm,Url,PwdAtm,PPasword,Papp_Vers,Papp_Code FROM WsParams WHERE Active = 1");
            //DataTable ws = DBUtil.GetSQL("SELECT * FROM WsParams WHERE Active = 1");

            foreach (DataRow item in ws.Rows)
            {
                WsAmbiente nWsAmbient = new WsAmbiente();
                nWsAmbient.Ambiente = item["Ambiente"].ToString();
                nWsAmbient.URL = item["Url"].ToString();
                nWsAmbient.UserAtm = item["UserAtm"].ToString();
                nWsAmbient.PwdAtm = item["PwdAtm"].ToString();
                nWsAmbient.PLoginId = item["PLogin_Id"].ToString();
                nWsAmbient.PApp_Code = item["Papp_Code"].ToString();
                nWsAmbient.PLevl_Typ = item["PLevl_Typ"].ToString();
                nWsAmbient.PPasword = item["PPasword"].ToString();
                nWsAmbient.PMerchant = item["PMerchant"].ToString();
                nWsAmbient.PAppVers = item["Papp_Vers"].ToString();
                //nWsAmbient.WsCashOutGuip = Convert.IsDBNull(item["WsAlterno"]) ? false : Convert.ToBoolean(item["WsAlterno"]);
                if (!WsAmbientes.ContainsKey(nWsAmbient.Ambiente))
                {
                    WsAmbientes.Add(nWsAmbient.Ambiente, nWsAmbient);
                }
            }
        }

        public static Boolean ConexSQLReady(Boolean verifyConnection = false)
        {
            Boolean retVal; ;
            try
            {
                DBUtil.TestConnectionParameters(ServerBD, PuertoBD, NameBD, UserBD, PassBD, verifyConnection);
                retVal = true;
            }
            catch
            {
                retVal = false;
            }
            return retVal;
        }

        /// <summary>
        /// Guarda los parametros por bloque
        /// </summary>
        /// <param name="Bloque"></param>
        public static void SaveConfigSQL()
        {
            String XMLFile = ConfigFile + "SQL.xml";

            if (File.Exists(XMLFile))
            {
                File.Delete(XMLFile);
            }

            DataTable Xr = new DataTable("SQL");
            Xr.Columns.Add("001");
            Xr.Columns.Add("002");
            Xr.Columns.Add("003");
            Xr.Columns.Add("004");
            Xr.Columns.Add("005");

            DataRow Dr4 = Xr.NewRow();
            Dr4["001"] = ServerBD;
            Dr4["002"] = UserBD;
            Dr4["003"] = PassBD;
            Dr4["004"] = PuertoBD;
            Dr4["005"] = NameBD;

            Xr.Rows.Add(Dr4);

            Xr.WriteXml(XMLFile);
        }

        public static void LoadConfigSQL()
        {
            String XMLFile = ConfigFile + "SQL.xml";

            DataSet Xet = new DataSet();
            if (File.Exists(XMLFile))
            {
                Xet.ReadXml(XMLFile);

                DataRow Dr4 = Xet.Tables[0].Rows[0];
                ServerBD = Dr4["001"].ToString();
                UserBD = Dr4["002"].ToString();
                PassBD = Dr4["003"].ToString();
                PuertoBD = Convert.ToInt32(Dr4["004"]);
                NameBD = Dr4["005"].ToString();
            }
        }

        public static int GetTypeChain(string text)
        {

            switch (text)
            {
                case "BASEi":
                    return 1;
                case "XML":
                    return 2;
                case "EWA":
                    return 3;
                case "BEBERTEC":
                    return 4;
                default:
                    return 0;
            }
        }

        public static String GetTypeChainStr(int text)
        {
            switch (text)
            {
                case 1:
                    return "BASEi";
                case 2:
                    return "XML";
                case 3:
                    return "EWA";
                case 4:
                    return "BEBERTEC";
                default:
                    return "";
            }
        }

        public static Boolean ReservedPort(Int32 Porto)
        {
            return PuertosReservados.Contains(Porto);
        }

        public static void LoadGlobalParameters()
        {
            DataTable xet = DBUtil.GetParameters();
            foreach (DataRow item in xet.Rows)
            {
                switch (item["ParameterCode"].ToString())
                {
                    case "ConsecutiveXML":
                        TCPUtil.ParamConsecutiveXML = Convert.ToInt32(item["ParameterValue"]);
                        break;
                    case "TimeoutConexBMP":
                        TCPUtil.ParamBMPTimeOut = Convert.ToInt32(item["ParameterValue"]);
                        break;
                    case "HeaderByte":
                        TCPUtil.HeaderBytesCount = Convert.ToInt32(item["ParameterValue"]);
                        break;
                    case "KeyXMLServerIP":
                        TCPUtil.ParamKeyXMLServer = item["ParameterValue"].ToString();
                        break;
                    case "KeyXMLServerPort":
                        TCPUtil.ParamKeyXMLPort = Convert.ToInt32(item["ParameterValue"]);
                        break;
                    case "KeyXMLServerTimeOut":
                        TCPUtil.ParamKeyXMLTimeOut = Convert.ToInt32(item["ParameterValue"]);
                        break;
                    case "NetworkAdapterName":
                        TCPUtil.ParamNetworkAdapter = item["ParameterValue"].ToString();
                        break;
                    case "EWAResponseBMP":
                        TCPUtil.BMPEWA200Response = item["ParameterValue"].ToString();
                        break;
                    case "DirectIP":
                        TCPUtil.ParamDirectIPServer = item["ParameterValue"].ToString();
                        break;

                    case "SignOn_Minutes":
                        TCPUtil.ParamSignOnMinutes = Convert.ToInt32(item["ParameterValue"]);
                        break;
                }
            }
        }


        //public static String GetIPByNetWorkMulticast() //direcciones entre 224.0.0.0 hasta 239.255.255.255
        //{
        //    String Ret = "";
        //    foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
        //    {
        //        if (item.Name == TCPUtil.ParamNetworkAdapter)
        //        {
        //            foreach (MulticastIPAddressInformation ip in item.GetIPProperties().MulticastAddresses)
        //            {
        //                if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
        //                {
        //                    Ret = ip.Address.ToString();
        //                    if (Ret != null) break;
        //                }
        //                if (Ret != null) break;
        //            }
        //        }
        //    }
        //    return Ret;
        //}

        public static String GetIpByLastNetworkAdapter(Boolean Unicast) //direcciones entre 0.0.0.0 hasta 223.255.255.255
        {
            String Ret = "";
            foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces()
                .Where(itemStat => itemStat.OperationalStatus == OperationalStatus.Up &&
                itemStat.NetworkInterfaceType != NetworkInterfaceType.Loopback))
            {
                if (item.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 || item.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                {
                    if (Unicast)
                    {
                        foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses
                            .Where(itemStat => !itemStat.Address.IsIPv6LinkLocal))
                        {
                            if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                            {
                                Ret = ip.Address.ToString();
                            }
                        }
                    }
                    else
                    {
                        foreach (MulticastIPAddressInformation ip in item.GetIPProperties().MulticastAddresses)
                        {
                            if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                            {
                                Ret = ip.Address.ToString();
                            }
                        }
                    }
                }
            }
            return Ret;
        }



        //FUNCIONES PARA TRATA DE CADENA
        /// <summary>
        /// Convierte Cadena HEX a Array de Bytes
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static byte[] ConvertHexStringToByteArray(string hex)
        {
            string errorMsg;

            hex = CleanHexString(hex);

            if (!IsValidHexString(hex, out errorMsg))
            {
                throw new Exception(errorMsg);
            }

            byte[] arr = new byte[hex.Length / 2];

            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = (byte)((ConvertHexCharToInt(hex[i << 1]) << 4) + (ConvertHexCharToInt(hex[(i << 1) + 1])));
            }
            return arr;
        }

        private static string CleanHexString(string hex)
        {
            if (string.IsNullOrWhiteSpace(hex))
            {
                return hex;
            }
            return hex.Replace("-", string.Empty).Replace(" ", string.Empty).ToUpper();
        }

        private static bool IsValidHexString(string hex, out string errorMsg)
        {
            if (string.IsNullOrWhiteSpace(hex))
            {
                errorMsg = "HEX Vacio o Nulo";
                return false;
            }
            if (hex.Length % 2 == 1)
            {
                errorMsg = "HEX Invalido, no puede tener un numero impar de caracteres";
                return false;
            }
            if (Regex.IsMatch(hex, @"[^0-9a-fA-F]"))
            {
                errorMsg = "HEX contiene caracteres invalidos";
                return false;
            }
            errorMsg = "";
            return true;
        }

        private static int ConvertHexCharToInt(char hex)
        {
            int val = hex;
            //For uppercase A-F letters:
            return val - (val < 58 ? 48 : 55);
            //For lowercase a-f letters:
            //return val - (val < 58 ? 48 : 87);
            //Or the two combined, but a bit slower:
            //return val - (val < 58 ? 48 : (val < 97 ? 55 : 87));
        }

        private static string BytesToEBCDic(byte[] ebcdicBytes)
        {
            if (ebcdicBytes.All(p => p == 0x00 || p == 0xFF))
            {
                //Every byte is either 0x00 or 0xFF (fillers)
                return string.Empty;
            }

            Encoding ebcdicEnc = Encoding.GetEncoding("IBM037");
            string result = ebcdicEnc.GetString(ebcdicBytes); // convert EBCDIC Bytes -> Unicode string
            return result;
        }

        /// <summary>
        /// Convierte String HEX > EBCDIC
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        public static String EBDicToString(String Input)
        {
            Byte[] Bites = ConvertHexStringToByteArray(Input);

            return BytesToEBCDic(Bites);
        }
        /// <summary>
        /// Convierte String ASCII > EBCDic HEX
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        public static String StringToEBCDic(String Input)
        {
            var sb = new StringBuilder();

            Encoding ebcdic = Encoding.GetEncoding("IBM037");

            var bytes = ebcdic.GetBytes(Input);
            foreach (var t in bytes)
            {
                sb.Append(t.ToString("X2"));
            }

            return sb.ToString();
        }

        /// <summary>
        /// Convierte Cadena a HEX 2, ASCII
        /// </summary>
        /// <param name="Str"></param>
        /// <returns></returns>
        public static String StringToHEXDos(String Str)
        {
            var sb = new StringBuilder();

            var bytes = Encoding.ASCII.GetBytes(Str);
            foreach (var t in bytes)
            {
                sb.Append(t.ToString("X2"));
            }

            return sb.ToString();
        }

        /// <summary>
        /// Convierte Cadena a HEX, UNICODE
        /// </summary>
        /// <param name="Str"></param>
        /// <returns></returns>
        public static String StringToHEXOne(String Str)
        {
            var sb = new StringBuilder();

            var bytes = Encoding.Unicode.GetBytes(Str);
            foreach (var t in bytes)
            {
                sb.Append(t.ToString("X"));
            }

            return sb.ToString();
        }

        /// <summary>
        /// Convierte Cadena a HEX, ASCII
        /// </summary>
        /// <param name="Str"></param>
        /// <returns></returns>
        public static String StringToHEXUno(String Str)
        {
            var sb = new StringBuilder();

            var bytes = Encoding.ASCII.GetBytes(Str);
            foreach (var t in bytes)
            {
                sb.Append(t.ToString("X"));
            }

            return sb.ToString();
        }


        /// <summary>
        /// Convierte HEX a Cadena, UNICODE
        /// </summary>
        /// <param name="HEXStr"></param>
        /// <returns></returns>
        public static String HEXToString(String HEXStr)
        {
            var bytes = new byte[HEXStr.Length / 2];
            for (var i = 0; i < bytes.Length; i++)
            {
                bytes[i] = Convert.ToByte(HEXStr.Substring(i * 2, 2), 16);
            }

            return Encoding.Unicode.GetString(bytes);
        }

        public static String HexToXML(String BytesHex)
        {
            String Ret = "";
            try
            {
                var sb = new StringBuilder();
                for (var i = 0; i < BytesHex.Length; i += 2)
                {
                    var hexChar = BytesHex.Substring(i, 2);
                    sb.Append((char)Convert.ToByte(hexChar, 16));
                }
                Ret = sb.ToString();
            }
            catch (Exception)
            {

            }

            return Ret.Substring(4);//elimina los 4 caracteres del HEADER LENGTH;
        }

        public static String GenResponse(String[] ISOP, String[] ISOT, String MTIResponse, Int32 TipoM, String RC, String AutID = "000000")
        {
            String[] NewIso = new String[130];

            switch (TipoM)
            {
                case 1: //BASE I
                    switch (MTIResponse)
                    {
                        case "0810":
                            NewIso[7] = ISOT[7];
                            NewIso[11] = DateTime.Now.ToString("HHmmss");
                            NewIso[37] = ISOP[37];
                            NewIso[39] = TCPUtil.StringToEBCDic(RC);
                            NewIso[63] = ISOP[63];
                            NewIso[70] = "0071";
                            break;
                        default:
                            NewIso[2] = ISOT[2];
                            NewIso[3] = ISOP[3];
                            NewIso[4] = ISOP[4];
                            NewIso[7] = ISOT[7];
                            NewIso[11] = DateTime.Now.ToString("HHmmss");
                            NewIso[19] = ISOP[19];
                            NewIso[25] = ISOP[25];
                            NewIso[32] = ISOP[32];
                            NewIso[37] = ISOP[37];
                            NewIso[39] = TCPUtil.StringToEBCDic(RC);
                            if (RC == "")
                            {
                                NewIso[38] = TCPUtil.StringToEBCDic(AutID);
                            }
                            NewIso[41] = ISOP[41];
                            NewIso[42] = ISOP[42];
                            NewIso[49] = ISOP[49];
                            NewIso[62] = ISOP[62];
                            NewIso[63] = ISOP[63];
                            break;
                    }
                    break;
                case 3:
                    switch (MTIResponse)
                    {
                        case "0830":
                        case "0810":
                            NewIso[7] = ISOP[7];
                            NewIso[11] = ISOP[11];
                            NewIso[33] = ISOP[33];
                            NewIso[39] = TCPUtil.StringToHEXUno(RC);
                            NewIso[70] = ISOP[70];
                            break;
                        default:
                            NewIso[2] = ISOT[2];
                            NewIso[3] = ISOP[3];
                            NewIso[4] = ISOP[4];
                            NewIso[7] = ISOP[7];
                            NewIso[11] = ISOP[11];
                            NewIso[12] = ISOP[12];
                            NewIso[13] = ISOP[13];
                            NewIso[17] = ISOP[13];
                            NewIso[25] = ISOP[25];
                            NewIso[32] = ISOP[32];
                            NewIso[37] = ISOP[37];
                            if (RC == "00")
                            {
                                NewIso[38] = TCPUtil.StringToHEXUno(AutID);
                            }
                            NewIso[39] = TCPUtil.StringToHEXUno(RC);
                            NewIso[41] = ISOP[41];
                            NewIso[42] = ISOP[42];
                            break;
                    }
                    break; //EWA
                default:
                    break;
            }

            string newDE1 = "";
            for (int I = 2; I <= 64; I++) { if (NewIso[I] != null) { newDE1 += "1"; } else { newDE1 += "0"; } }

            string newDE2 = "";
            for (int I = 65; I <= 128; I++) { if (NewIso[I] != null) { newDE2 += "1"; } else { newDE2 += "0"; } }

            if (newDE2 == "0000000000000000000000000000000000000000000000000000000000000000")
            { newDE1 = "0" + newDE1; }
            else { newDE1 = "1" + newDE1; }

            String Response = "";

            for (int i = 0; i < 130; i++)
            {
                if (i == 0)
                {
                    if (TipoM == 3)
                    {
                        Response += TCPUtil.StringToHEXUno(MTIResponse);
                    }
                    else
                    {
                        Response += MTIResponse;
                    }
                }
                else if (i == 1)
                {
                    string DE1Hex = String.Format("{0:X1}", Convert.ToInt64(newDE1, 2));
                    DE1Hex = DE1Hex.PadLeft(16, '0'); //Pad-Left
                    Response += DE1Hex;

                    string DE2Hex = String.Format("{0:X1}", Convert.ToInt64(newDE2, 2));
                    DE2Hex = DE2Hex.PadLeft(16, '0'); //Pad-Left
                    if (DE2Hex != "0000000000000000")
                    {
                        Response += DE2Hex;
                    }
                }
                else if (NewIso[i] != null)
                {
                    if (i == 2) //PAN
                    {
                        if (TipoM == 3)
                        {
                            Response += TCPUtil.StringToHEXUno(NewIso[i].Length.ToString());
                            Response += TCPUtil.StringToHEXUno(NewIso[i]);
                        }
                        else
                        {
                            Response += NewIso[i].Length.ToString("X");
                            Response += NewIso[i];
                        }
                    }
                    else
                    {
                        Response += NewIso[i];
                    }

                }
            }
            return Response;
        }

        /// <summary>
        /// Convierte Bytes a Cadena, ENCODING DEFAULT
        /// </summary>
        /// <param name="Bytes"></param>
        /// <returns></returns>
        public static String GetRealStringFromByte(Byte[] Bytes)
        {
            if (Bytes == null)
            {
                return "";
            }
            else if (Bytes.Length != 0)
            {
                Char[] nCArray = new Char[Bytes.Length];
                int Lgt = Bytes.Length - 1;
                for (int i = 0; i < Lgt; i = checked(i + 1))
                {
                    nCArray[i] = Convert.ToChar(Bytes[i]);
                }

                return new String(nCArray);
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Convierte Cadena a Bytes, ENCODING DEFAULT
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        public static Byte[] GetBytesFromRealString(String Input)
        {
            if (Input.Length != 0)
            {
                Byte[] nArrayB = new Byte[Input.Length];
                Char[] nArrayC = Input.ToCharArray();

                int Lgt = nArrayC.Length - 1;
                for (int i = 0; i < Lgt; i = checked(i + 1))
                {
                    nArrayB[i] = Convert.ToByte((int)nArrayC[i]);
                }
                return nArrayB;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Genera un Stream XML para ser enviado por WS
        /// </summary>
        /// <param name="PinVer"></param>
        /// <param name="Track"></param>
        /// <param name="ID"></param>
        /// <param name="Pin"></param>
        /// <returns></returns>


        /// <summary>
        /// Convierte BMP HEX a BINRIO
        /// </summary>
        /// <param name="HexDE"></param>
        /// <returns></returns>
        public static string DEtoBinary(string HexDE)
        {
            string deBinary = "";
            for (int I = 0; I <= 15; I++)
            {
                deBinary = deBinary + Hex2Binary(HexDE.Substring(I, 1));
            }
            return deBinary;
        }

        public static String PrepareISO8583FromHEX(String ISO, Int32 HBytes, Int32 TypeMsj, out Int32 Xtra)
        {
            String ChainRes = "";

            Int32 MyLength = 0; Int32 MyPos = 0;
            String NetHeader = "";

            // Boolean Limpiar = false;
            Xtra = 0;
            switch (TypeMsj)
            {
                case 1: //BASE i
                    MyLength = HBytes;
                    NetHeader = ISO.Substring(MyPos, MyLength);
                    MyPos += MyLength;

                    Int32 HeaderLgt = Int32.Parse(ISO.Substring(MyPos, 2), System.Globalization.NumberStyles.HexNumber) * 2;
                    //MyPos += 2;
                    String ISOHeader = ISO.Substring(MyPos, HeaderLgt);
                    String[] BASEiHeaders = TCPUtil.HEXHeaderParse(NetHeader + ISOHeader, HBytes);
                    MyPos += HeaderLgt;

                    ChainRes = ISO.Substring(HeaderLgt + NetHeader.Length);
                    Xtra = HeaderLgt;
                    break;
                default:
                    MyLength = HBytes;

                    NetHeader = ISO.Substring(MyPos, MyLength);
                    MyPos += MyLength;

                    ChainRes = ISO.Substring(MyPos);
                    break;
            }

            return ChainRes;
        }

        private static string Hex2Binary(string DE)
        {

            string myBinary = "";
            switch (DE)
            {
                case "0":
                    myBinary = "0000";
                    break;

                case "1":
                    myBinary = "0001";
                    break;

                case "2":
                    myBinary = "0010";
                    break;

                case "3":
                    myBinary = "0011";
                    break;

                case "4":
                    myBinary = "0100";
                    break;

                case "5":
                    myBinary = "0101";
                    break;

                case "6":
                    myBinary = "0110";
                    break;

                case "7":
                    myBinary = "0111";
                    break;

                case "8":
                    myBinary = "1000";
                    break;

                case "9":
                    myBinary = "1001";
                    break;

                case "A":
                    myBinary = "1010";
                    break;

                case "B":
                    myBinary = "1011";
                    break;

                case "C":
                    myBinary = "1100";
                    break;

                case "D":
                    myBinary = "1101";
                    break;

                case "E":
                    myBinary = "1110";
                    break;

                case "F":
                    myBinary = "1111";
                    break;


            }


            return myBinary;

        }

        public static String Binary2BMP(String[] Fields)
        {
            String Response = "";
            string newDE1 = "";
            for (int I = 2; I <= 64; I++) { if (Fields[I] != null) { newDE1 += "1"; } else { newDE1 += "0"; } }

            string newDE2 = "";
            for (int I = 65; I <= 128; I++) { if (Fields[I] != null) { newDE2 += "1"; } else { newDE2 += "0"; } }

            if (newDE2 == "0000000000000000000000000000000000000000000000000000000000000000")
            { newDE1 = "0" + newDE1; }
            else { newDE1 = "1" + newDE1; }

            string DE1Hex = String.Format("{0:X1}", Convert.ToInt64(newDE1, 2));
            DE1Hex = DE1Hex.PadLeft(16, '0'); //Pad-Left
            Response += DE1Hex;

            string DE2Hex = String.Format("{0:X1}", Convert.ToInt64(newDE2, 2));
            DE2Hex = DE2Hex.PadLeft(16, '0'); //Pad-Left
            if (DE2Hex != "0000000000000000")
            {
                Response += DE2Hex;
            }

            return Response;
        }

        public static String[] GetCamposByBMP(String BMP, String[] Source, String[] SourceTrans, Int32 TypeMsj, Dictionary<Int32, String> Fields2Add)
        {
            String BinaryBMP = TCPUtil.DEtoBinary(BMP);

            Boolean isBebertec = TypeMsj == 4;

            String[] Campos = new String[130];
            if (BMP.Length == 16) //Son dos BMP?
            {
                Campos[0] = isBebertec ? StringToHEXUno(BMP) : BMP;
            }
            else
            {
                String BMP2 = BMP.Substring(16);
                Campos[0] = isBebertec ? StringToHEXUno(BMP.Substring(0, 16)) : BMP.Substring(0, 16);
                Campos[1] = isBebertec ? StringToHEXUno(BMP.Substring(16)) : BMP.Substring(16);
                BinaryBMP += TCPUtil.DEtoBinary(BMP2);
            }
            Decimal ValorReq = 0;
            Decimal Comision = 0;

            for (int i = 1; i < BinaryBMP.Length; i++)
            {
                if (BinaryBMP.Substring(i - 1, 1) == "1")
                {
                    if (Source[i] != null)
                    {
                        if (i == 4)
                        {
                            if (Comision > Convert.ToDecimal(0))
                            {
                                String ValZero = (Comision + ValorReq).ToString().Replace(".", "").PadLeft(12, '0');
                                Campos[i] = isBebertec ? TCPUtil.StringToHEXUno(ValZero) : ValZero;
                            }
                            else { Campos[i] = Source[i]; }
                        }
                        else { Campos[i] = Source[i]; }
                    }
                }
            }

            if (Fields2Add != null)
            {
                Fields2Add.ToList().ForEach(f =>
                {
                    Campos[f.Key] = f.Value;
                });
            }

            String BMPC = Binary2BMP(Campos); //SI el Bitmap pide un campo que no viene en el source, se debe recalcular el BMP
            if (BMPC != BMP)
            {
                if (BMPC.Length == 16) //Son dos BMP?
                {
                    Campos[0] = isBebertec ? StringToHEXUno(BMPC) : BMPC;
                    Campos[1] = null;
                }
                else
                {
                    Campos[0] = isBebertec ? StringToHEXUno(BMPC.Substring(0, 16)) : BMPC.Substring(0, 16);
                    Campos[1] = isBebertec ? StringToHEXUno(BMPC.Substring(16)) : BMPC.Substring(16);
                }
            }

            return Campos;
        }

        /// <summary>
        /// Funcion para Armar un XML en base a un BMP
        /// </summary>
        /// <param name="Campos"></param>
        /// <returns></returns>
        //public static XmlDocument GetXmlByBMP(String[] Campos, String MTI)
        //{
        //    XmlDocument xDoc = new XmlDocument();

        //    XmlDeclaration xDec = xDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
        //    XmlElement Root = xDoc.DocumentElement;
        //    xDoc.InsertBefore(xDec, Root);

        //    XmlElement rNode = xDoc.CreateElement("", "SmartConvert", "");
        //    XmlAttribute hNode = xDoc.CreateAttribute("MTI");
        //    hNode.Value = MTI;
        //    rNode.Attributes.Append(hNode);
        //    xDoc.AppendChild(rNode);

        //    for (int i = 0; i < Campos.Length; i++)
        //    {
        //        if (Campos[i] != null)
        //        {
        //            XmlElement nX = xDoc.CreateElement("F" + i);
        //            XmlText nXT = xDoc.CreateTextNode(Campos[i]);

        //            nX.AppendChild(nXT);
        //            rNode.AppendChild(nX);
        //        }
        //    }

        //    return xDoc;
        //}

        /// <summary>
        /// Funcion para Parsear los headers
        /// </summary>
        /// <param name="Header"></param>
        /// <returns></returns>
        public static String[] HEXHeaderParse(String Header, Int32 HeaderByte)
        {
            String[] Headers = new String[13]; //<=== Tope Maximo de Headers

            Int32 Posicion = 0;
            Int32 Length = HeaderByte; //<===== ES 8Byte

            Headers[0] = Header.Substring(Posicion, Length);
            Posicion += Length; //<=== Complementa la Cadena al inicio para marcar la longitud de la cadena

            Length = 2;
            Headers[1] = Header.Substring(Posicion, Length);
            Posicion += Length;

            Length = 2;
            Headers[2] = Header.Substring(Posicion, Length);
            Posicion += Length;

            Length = 2;
            Headers[3] = Header.Substring(Posicion, Length);
            Posicion += Length;

            Length = 4;
            Headers[4] = Header.Substring(Posicion, Length);
            Posicion += Length; //<=== SE REEMPLAZA Con La lOngitud REAL

            Length = 6;
            Headers[5] = Header.Substring(Posicion, Length);
            Posicion += Length;

            Length = 6;
            Headers[6] = Header.Substring(Posicion, Length);
            Posicion += Length;

            Length = 2;
            Headers[7] = Header.Substring(Posicion, Length);
            Posicion += Length;

            Length = 4;
            Headers[8] = Header.Substring(Posicion, Length);
            Posicion += Length;

            Length = 6;
            Headers[9] = Header.Substring(Posicion, Length);
            Posicion += Length;

            Length = 2;
            Headers[10] = Header.Substring(Posicion, Length);
            Posicion += Length;

            Length = 6;
            Headers[11] = Header.Substring(Posicion, Length);
            Posicion += Length;

            Length = 2;
            Headers[12] = Header.Substring(Posicion, Length);
            Posicion += Length;

            return Headers;
        }

        public static String GetSpecialFInt(String RC)
        {
            String Ret = "";
            for (int i = 0; i < RC.Length; i++)
            {
                Ret += "F" + RC.Substring(i, 1);
            }
            return Ret;
        }


        public static String HeaderHEXResponse(String[] Headers, Int32 ISOResponseL)
        {
            String RespuestaHEX = "";

            for (int i = 1; i < Headers.Length; i++)
            {
                if (i == 4)
                {
                    RespuestaHEX += ISOResponseL.ToString("X4");
                }
                else
                {
                    RespuestaHEX += Headers[i];
                }
            }

            return RespuestaHEX;
        }

        public static WsAlt.DT_CashOutGuip GetDTAtmAlt(WsAlt.DT_CashOut Fields)
        {
            var DT_iCol = new WsAlt.DT_identificadorColeccion()
            {
                was = "?",
                pi = "?",
                omniCanal = "?",
                recibo = "?",
                numeroTransaccion = "?"
            };

            var DT_iItem = new WsAlt.DT_parametroAdicionalItem()
            {
                linea = "1",
                tipoRegistro = "Login_Id",
                valor = TCPUtil.WsAmbientForOperation.PLoginId
            };

            var DT_iItem1 = new WsAlt.DT_parametroAdicionalItem()
            {
                linea = "2",
                tipoRegistro = "Application_Code",
                valor = TCPUtil.WsAmbientForOperation.PApp_Code
            };
            var DT_iItem2 = new WsAlt.DT_parametroAdicionalItem()
            {
                linea = "3",
                tipoRegistro = "Level_Type",
                valor = TCPUtil.WsAmbientForOperation.PLevl_Typ
            };

            var DtColItems = new List<WsAlt.DT_parametroAdicionalItem>() { DT_iItem, DT_iItem1, DT_iItem2 }.ToArray();

            var Dt_LItem = new WsAlt.DT_logItem()
            {
                identificadorWas = "?",
                identificadorPi = "?",
                identificadorOmniCanal = "?",
                identificadorRecibo = "?",
                identificadorNumeroTransaccion = "?",
                aplicacionId = "?",
                canalId = "?",
                ambienteId = "?",
                transaccionId = "?",
                accion = "?",
                tipo = "?",
                fecha = "?",
                hora = "?",
                fechaSistema = "?",
                horaSistema = "?",
                auxiliar1 = "?",
                auxiliar2 = "?",
                parametroAdicionalColeccion = new List<WsAlt.DT_parametroAdicionalItem>() {
                                new WsAlt.DT_parametroAdicionalItem() {
                                linea = "?",
                                tipoRegistro = "?",
                                valor = "?" }
                            }.ToArray()
            };

            var DtColLogs = new List<WsAlt.DT_logItem>() { Dt_LItem }.ToArray();

            return new WsAlt.DT_CashOutGuip()
            {
                transaccionId = "?",
                aplicacionId = "?",
                paisId = "?",
                empresaId = "?",
                regionId = "?",
                canalId = "?",
                version = TCPUtil.WsAmbientForOperation.PMerchant,
                llaveSesion = TCPUtil.WsAmbientForOperation.PPasword,
                usuarioId = "?",
                token = TCPUtil.WsAmbientForOperation.PAppVers,
                dispositivoId = "?",
                identificadorColeccion = new List<WsAlt.DT_identificadorColeccion>() { DT_iCol }.ToArray(),
                parametroAdicionalColeccion = DtColItems,
                logColeccion = DtColLogs,
                AtmCashOut = Fields,
                clienteCoreId = "?"

            };

        }
        public static DT_ATM GetDTATM(DT_ATM_CashOut Fields)
        {
            DT_identificadorColeccion DT_iCol = new DT_identificadorColeccion()
            {
                was = "?",
                pi = "?",
                omniCanal = "?",
                recibo = "?",
                numeroTransaccion = "?"
            };



            DT_parametroAdicionalItem DT_iItem = new DT_parametroAdicionalItem()
            {
                linea = "1",
                tipoRegistro = "Login_Id",
                valor = TCPUtil.WsAmbientForOperation.PLoginId
            };

            DT_parametroAdicionalItem DT_iItem1 = new DT_parametroAdicionalItem()
            {
                linea = "2",
                tipoRegistro = "Application_Code",
                valor = TCPUtil.WsAmbientForOperation.PApp_Code
            };

            DT_parametroAdicionalItem DT_iItem2 = new DT_parametroAdicionalItem()
            {
                linea = "3",
                tipoRegistro = "Level_Type",
                valor = TCPUtil.WsAmbientForOperation.PLevl_Typ
            };

            var DtColItems = new List<DT_parametroAdicionalItem>() { DT_iItem, DT_iItem1, DT_iItem2 }.ToArray();

            DT_logItem Dt_LItem = new DT_logItem()
            {
                identificadorWas = "?",
                identificadorPi = "?",
                identificadorOmniCanal = "?",
                identificadorRecibo = "?",
                identificadorNumeroTransaccion = "?",
                aplicacionId = "?",
                canalId = "?",
                ambienteId = "?",
                transaccionId = "?",
                accion = "?",
                tipo = "?",
                fecha = "?",
                hora = "?",
                fechaSistema = "?",
                horaSistema = "?",
                auxiliar1 = "?",
                auxiliar2 = "?",
                parametroAdicionalColeccion = new List<DT_parametroAdicionalItem>() {
                                new DT_parametroAdicionalItem() {
                                linea = "?",
                                tipoRegistro = "?",
                                valor = "?" }
                            }.ToArray()
            };

            var DtColLogs = new List<DT_logItem>() { Dt_LItem }.ToArray();

            return new DT_ATM()
            {
                transaccionId = "?",
                aplicacionId = "?",
                paisId = "?",
                empresaId = "?",
                regionId = "?",
                canalId = "?",
                version = TCPUtil.WsAmbientForOperation.PMerchant,
                llaveSesion = TCPUtil.WsAmbientForOperation.PPasword,
                ClienteCoreId = "?",
                usuarioId = "?",
                token = TCPUtil.WsAmbientForOperation.PAppVers,
                dispositivoId = "?",
                identificadorColeccion = new List<DT_identificadorColeccion>() { DT_iCol }.ToArray(),
                parametroAdicionalColeccion = DtColItems,
                logColeccion = DtColLogs,
                AtmCashOut = Fields


            };

        }

        #region FLOW HELPER

        public static ClienteTCP ConvertToClienteTCP(ConexMaster Conex, Boolean SignOn = false)
        {
            ClienteTCP Ret = null;
            try
            {
                if (Conex.IsValidator)
                {
                    if (SignOn)
                    {
                        Ret = new ClienteTCP(TCPUtil.ParamKeyXMLServer,
                            TCPUtil.ParamKeyXMLPort, Conex);
                    }
                    else
                    {
                        Ret = new ClienteTCP(Conex.IPAdress, Conex.Port, Conex);
                    }
                }
                else
                {
                    Ret = new ClienteTCP(Conex.IPAdress, Conex.Port, Conex);
                }
                //if (Conex.TimeOutSec > 0)
                //{
                //    Ret.ConnectTimeoutSeconds = Conex.TimeOutSec;
                //}
            }
            catch (Exception)
            {
                Ret = null;
            }

            return Ret;
        }



        #endregion

        public static void CreaFileLog()
        {
            String mPrefix = "Log_";
            String txtD = DateTime.Now.ToString("ddMMyyyy");
            FileLog = TCPUtil.ConfigFile + Path.DirectorySeparatorChar + "Logs" + Path.DirectorySeparatorChar + mPrefix + txtD + ".sql";
            if (!Directory.Exists(TCPUtil.ConfigFile + Path.DirectorySeparatorChar + "Logs"))
            {
                Directory.CreateDirectory(TCPUtil.ConfigFile + Path.DirectorySeparatorChar + "Logs");
            }
            if (!File.Exists(FileLog))
            {
                File.Create(FileLog);
            }
        }

        public static void SaveBitacoraOffline(SqlCommand sql, Int32 Zesion = 0)
        {


            String DataToSave = sql.CommandText;

            foreach (SqlParameter item in sql.Parameters)
            {
                if (item.DbType == DbType.String || item.DbType == DbType.Boolean)
                {
                    DataToSave = DataToSave.Replace("@" + item.ParameterName, "'" + item.Value.ToString() + "'");
                }
                else if (item.DbType == DbType.DateTime)
                {
                    String nDate = Convert.ToDateTime(item.Value).ToString("yyyy-MM-dd HH:mm:ss");
                    DataToSave = DataToSave.Replace("@" + item.ParameterName, "'" + nDate + "'");
                }
                else
                {
                    DataToSave = DataToSave.Replace("@" + item.ParameterName, item.Value.ToString());
                }
            }

            DataToSave += ";" + Environment.NewLine;

            DataInLog += DataToSave;

            if (DBUtil.SQLOffline)
            {
                String mPrefix = "Log_";
                String txtD = DateTime.Now.ToString("ddMMyyyy");
                if (FileLog != TCPUtil.ConfigFile + Path.DirectorySeparatorChar + "Logs" + Path.DirectorySeparatorChar + mPrefix + txtD + ".sql")
                {
                    if (!File.Exists(FileLog))
                    {
                        File.Create(FileLog);
                        FileLog = TCPUtil.ConfigFile + Path.DirectorySeparatorChar + "Logs" + Path.DirectorySeparatorChar + mPrefix + txtD + ".sql";
                    }
                }

                File.AppendAllText(FileLog, DataInLog);
                DataInLog = "";
            }
            else
            {
                DataInLog = "";
            }



        }

        public static String TreatF35(String F35)
        {
            String x = "";
            F35.ToList().ForEach(C =>
            {
                if (Char.IsDigit(C))
                {
                    x += C;
                }
                else
                {
                    x += "=";
                }
            });

            return x;
        }

        public static String GenNewIsoForSigOn()
        {
            ISO8583 HelperISO = new ISO8583(1);

            JulianCalendar myCal = new JulianCalendar();

            String NHeader = BASEFORSIGNON.Substring(0, 8);
            String FormatedHEX = TCPUtil.PrepareISO8583FromHEX(BASEFORSIGNON, 8, 1, out Int32 HR);

            String[] Parsed = HelperISO.Parse(FormatedHEX, false);
            String[] TransL = HelperISO.Parse(FormatedHEX, true);

            String NetHeader = BASEFORSIGNON.Substring(0, 8);
            String Headers = BASEFORSIGNON.Substring(8, HR);
            String[] BaseiHeaders = TCPUtil.HEXHeaderParse(NetHeader + Headers, 8);

            Random generator = new Random();
            String r = generator.Next(0, 1000000).ToString("D6");

            DateTime myDT = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, new GregorianCalendar());
            String jDate = myCal.GetYear(myDT).ToString().Substring(0, 1) + DateTime.Now.DayOfYear.ToString().PadLeft(3, '0');

            Parsed[7] = DateTime.Now.ToString("MMddhhmmss");
            Parsed[11] = r;
            Parsed[37] = StringToEBCDic(jDate + Parsed[7].Substring(4, 2) + Parsed[11]);

            String nChain = TransL[129];
            for (int i = 0; i < Parsed.Length; i++)
            {
                if (i != 129)
                {
                    if (Parsed[i] != null)
                    {
                        nChain += Parsed[i];
                    }
                }
            }

            Headers = TCPUtil.HeaderHEXResponse(BaseiHeaders, (nChain.Length / 2) + 22);
            String InitConex = ((nChain.Length / 2) + (Headers.Length / 2)).ToString("X4");
            InitConex += "0000";

            return InitConex + Headers + nChain;

        }



    }


}
