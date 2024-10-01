using System;
using System.Collections.Generic;

namespace TCPSmart.Flow
{
    public static class UtilFlowOperation
    {
        public static String[] BASEiHeaders { get; set; }

        public static String GlobalBMP { get; set; }

        public static int HeaderBytes { get; set; }

        public static int typeMsg { get; set; }

        public static String GetISO8583FromHex(String ISO, Int32 HBytes, Int32 TypeMsj)
        {
            String ChainRes = "";

            Int32 MyLength = 0; Int32 MyPos = 0;
            String NetHeader = "";

            //switch (TypeMsj)
            //{
            //    case 1: //BASE i
            //        MyLength = HBytes;
            //        NetHeader = ISO.Substring(MyPos, MyLength);
            //        MyPos += MyLength;

            //        Int32 HeaderLgt = Int32.Parse(ISO.Substring(MyPos, 2), System.Globalization.NumberStyles.HexNumber) * 2;
            //        //MyPos += 2;
            //        String ISOHeader = ISO.Substring(MyPos, HeaderLgt);
            //        BASEiHeaders = TCPUtil.HEXHeaderParse(NetHeader + ISOHeader, HBytes);
            //        MyPos += HeaderLgt;

            //        ChainRes = ISO.Substring(HeaderLgt + NetHeader.Length);

            //        break;
            //    default:
            MyLength = HBytes;

            NetHeader = ISO.Substring(MyPos, MyLength);
            MyPos += MyLength;

            ChainRes = ISO.Substring(MyPos);
            //sq        break;
            // }

            return ChainRes;
        }

        /// <summary>
        /// Arma Respuesta en Base a BMP e ISO Source
        /// </summary>
        /// <param name="BMP"></param>
        /// <param name="TypeMsj"></param>
        /// <param name="SourceISO"></param>
        /// <returns></returns>
        public static String BuildChainFromBMP(String BMP, Int32 TypeMsj, String[] SourceISO, String Code)
        {
            String Response = "";
            String[] Fields = new String[130];
            if (BMP == "")
            {
                BMP = TCPUtil.Binary2BMP(SourceISO);
                Fields = SourceISO;
                GlobalBMP = BMP;
            }
            else
            {
                Fields = TCPUtil.GetCamposByBMP(BMP, SourceISO, SourceISO, TypeMsj, null);
            }

            Response = BuildChain(Fields, TypeMsj != 1 ? TCPUtil.StringToHEXUno(Code) : Code);

            Int32 Lgt = Response.Length;

            return Response;
        }

        public static String BuildChain(String[] Fields, String MTI)
        {
            String Response = MTI;

            for (int i = 0; i < 130; i++)
            {
                if (Fields[i] != null)
                {
                    Response += Fields[i];
                }
            }

            Int32 L = Response.Length;

            return Response;
        }

        public static String BuildChainFromRC(String RC, ConexMaster Cn, String[] SourceISO, String Code, String F38Val = "", Boolean ModGlobalBMP = true)
        {
            String Chain = "";
            String BMP = "";
            String[] Fields = new String[130];

            Dictionary<Int32, String> FieldsForAdd = new Dictionary<Int32, string>();

            if (Code == "0810")
            {
                FieldsForAdd.Add(39, Cn.TypeMsg == 1 ? TCPUtil.StringToEBCDic("00") : TCPUtil.StringToHEXUno("00"));
                if (!Cn.BitMaps.ContainsKey(Code))
                    BMP = Cn.TypeMsg == 1 ? TCPUtil.BASICBMP810i : TCPUtil.BASICBMP810w;
            }
            else
            {
                if (!Cn.BitMaps.ContainsKey(Code))
                    BMP = Cn.TypeMsg == 1 ? TCPUtil.BASICBMPi : TCPUtil.BASICBMPw;

                if (!ModGlobalBMP)
                {
                    BMP = TCPUtil.Binary2BMP(SourceISO);
                }

                if (RC == "00")
                {
                    if (F38Val != "")
                    {
                        FieldsForAdd.Add(38, F38Val);
                    }
                    else
                    {
                        FieldsForAdd.Add(38, SourceISO[38]);
                    }

                    FieldsForAdd.Add(39, Cn.TypeMsg == 1 ? TCPUtil.StringToEBCDic("00") : TCPUtil.StringToHEXUno("00"));
                }
                else
                {
                    FieldsForAdd.Add(39, Cn.TypeMsg == 1 ? TCPUtil.StringToEBCDic(RC) : TCPUtil.StringToHEXUno(RC));
                }
            }

            if (Cn.BitMaps.ContainsKey(Code))
            {
                Cn.BitMaps.TryGetValue(Code, out BMP);
                Fields = TCPUtil.GetCamposByBMP(BMP, SourceISO, SourceISO, Cn.TypeMsg, FieldsForAdd);
            }
            else
            {
                Fields = TCPUtil.GetCamposByBMP(BMP, SourceISO, SourceISO, Cn.TypeMsg, FieldsForAdd);

            }

            BMP = TCPUtil.Binary2BMP(Fields);
            if (ModGlobalBMP)
            {
                GlobalBMP = BMP;
            }


            Chain = BuildChain(Fields, Cn.TypeMsg != 1 ? TCPUtil.StringToHEXUno(Code) : Code);

            return Chain;
        }


        public static Byte[] GetBytesToSend(String Response, Boolean AddNetworkHeader, ConexMaster Cn)
        {
            if (AddNetworkHeader)
            {
                return TCPUtil.ConvertHexStringToByteArray(GetNetWorkHeader(Cn, Response));
            }
            else
            {
                return TCPUtil.ConvertHexStringToByteArray(Response);
            }

        }

        public static String GetNetWorkHeader(ConexMaster Cn, String Response)
        {
            if (Cn.TypeMsg == 1)
            {
                String HeadersHex = TCPUtil.HeaderHEXResponse(BASEiHeaders, (Response.Length / 2) + 22);
                String InitConexHeader = ((Response.Length / 2) + (HeadersHex.Length / 2)).ToString("X4"); //<=== HEX tipo 00 80
                if (HeaderBytes == 8) { InitConexHeader += "0000"; }

                return InitConexHeader + HeadersHex + Response;
            }
            else
            {
                String NetWorkHeader = "";
                if (typeMsg == 4)
                {
                    NetWorkHeader = (Response.Length / 2).ToString("X" + HeaderBytes / 2) + "0000";
                }
                else
                {
                    NetWorkHeader = (Response.Length / 2).ToString("X4");
                }
                return NetWorkHeader + Response;
            }

        }

        public static String GetISOLabel(Boolean IsReverso, String MTIRequest, String[] ISO, String Mode, String ValF0F35)
        {
            if (IsReverso)
            {
                return "Enviando ISO8583 Ws MODO -> " + Mode + " REQ -> " +
                    "F0:" + ValF0F35 + " " +
                                "F1:" + MTIRequest + " " +
                                "F2:" + ISO[2] + " " +
                                "F3:" + ISO[3] + " " +
                                "F4:" + ISO[4] + " " +
                                "F7:" + ISO[7] + " " +
                                "F11:" + ISO[11] + " " +
                                "F19:" + ISO[19] + " " +
                                "F25:" + ISO[25] + " " +
                                "F32:" + ISO[32] + " " +
                                "F35:" + ISO[35] + " " +
                                "F37:" + ISO[37] + " " +
                                "F38:" + ISO[38] + " " +
                                "F41:" + ISO[41] + " " +
                                "F42:" + ISO[42] + " " +
                                "F43:" + ISO[43] + " " +
                                "F49:" + ISO[49] + " " +
                                "F63:" + ISO[63] + " ";
            }
            else
            {
                return "Enviando ISO8583 Ws MODO -> " + Mode + " REQ -> " +
                    "F0:" + ValF0F35 + " " +
                                "F1:" + MTIRequest + " " +
                                "F2:" + ISO[2] + " " +
                                "F3:" + ISO[3] + " " +
                                "F4:" + ISO[4] + " " +
                                "F7:" + ISO[7] + " " +
                                "F11:" + ISO[11] + " " +
                                "F19:" + ISO[19] + " " +
                                "F25:" + ISO[25] + " " +
                                "F32:" + ISO[32] + " " +
                                "F35:" + ISO[35] + " " +
                                "F37:" + ISO[37] + " " +
                                "F38:" + ISO[38] + " " +
                                "F41:" + ISO[41] + " " +
                                "F42:" + ISO[42] + " " +
                                "F43:" + ISO[43] + " " +
                                "F49:" + ISO[49] + " " +
                                "F63:" + ISO[63] + " ";
            }
        }





    }
}
