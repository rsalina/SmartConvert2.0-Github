using System;
using System.Text;

namespace TCPSmart
{
    public class ISO8583
    {
        int[] DEVarLen = new int[130];
        int[] DEFixLen = new int[130];
        Int32 InputType = 0;

        public ISO8583(Int32 TipoMensaje)
        {
            InputType = TipoMensaje;
            switch (InputType)
            {
                case 1:
                    //Initialize BitMap Var Length Indicator
                    DEVarLen[2] = 2; DEVarLen[32] = 2; DEVarLen[33] = 2; DEVarLen[34] = 2; DEVarLen[35] = 2; DEVarLen[36] = 3;
                    DEVarLen[44] = 2; DEVarLen[45] = 2; DEVarLen[46] = 3; DEVarLen[47] = 3; DEVarLen[48] = 3; DEVarLen[54] = 3;
                    DEVarLen[55] = 3; DEVarLen[56] = 3; DEVarLen[57] = 3; DEVarLen[58] = 3; DEVarLen[59] = 3;
                    DEVarLen[60] = 1; DEVarLen[61] = 3; DEVarLen[62] = 3; DEVarLen[63] = 2; DEVarLen[72] = 3; DEVarLen[99] = 2;
                    DEVarLen[100] = 2; DEVarLen[101] = 2; DEVarLen[102] = 2; DEVarLen[103] = 2; DEVarLen[104] = 3; DEVarLen[105] = 3;
                    DEVarLen[106] = 3; DEVarLen[107] = 3; DEVarLen[108] = 3; DEVarLen[109] = 3; DEVarLen[110] = 3;
                    DEVarLen[111] = 3; DEVarLen[112] = 3; DEVarLen[113] = 2; DEVarLen[114] = 3; DEVarLen[115] = 3;
                    DEVarLen[116] = 3; DEVarLen[117] = 3; DEVarLen[118] = 3; DEVarLen[119] = 3; DEVarLen[120] = 3; DEVarLen[121] = 3;
                    DEVarLen[122] = 3; DEVarLen[123] = 3; DEVarLen[124] = 3; DEVarLen[125] = 2; DEVarLen[126] = 1; DEVarLen[127] = 3;

                    // "-" means not numeric.

                    DEFixLen[0] = 16; DEFixLen[1] = 16; DEFixLen[3] = 6; DEFixLen[4] = 12;
                    DEFixLen[5] = 12; DEFixLen[6] = 12; DEFixLen[7] = 10; DEFixLen[8] = 8;
                    DEFixLen[9] = 8; DEFixLen[10] = 8; DEFixLen[11] = 6; DEFixLen[12] = 6;
                    DEFixLen[13] = 4; DEFixLen[14] = 4; DEFixLen[15] = 4; DEFixLen[16] = 4;
                    DEFixLen[17] = 4; DEFixLen[18] = 4; DEFixLen[19] = 4; DEFixLen[20] = 3;
                    DEFixLen[21] = 3; DEFixLen[22] = 4; DEFixLen[23] = 3; DEFixLen[24] = -3;
                    DEFixLen[25] = 2; DEFixLen[26] = 2; DEFixLen[27] = 1; DEFixLen[28] = -8;
                    DEFixLen[29] = -8; DEFixLen[30] = 8; DEFixLen[31] = 8; DEFixLen[37] = -12;
                    DEFixLen[38] = -6; DEFixLen[39] = -2; DEFixLen[40] = -3; DEFixLen[41] = -8;
                    DEFixLen[42] = -15; DEFixLen[43] = -40; DEFixLen[49] = 4; DEFixLen[50] = -3;
                    DEFixLen[51] = -3; DEFixLen[52] = 16; DEFixLen[53] = 18; DEFixLen[64] = -4;
                    DEFixLen[65] = -16; DEFixLen[66] = 1; DEFixLen[67] = 2; DEFixLen[68] = 3;
                    DEFixLen[69] = 3; DEFixLen[70] = 3; DEFixLen[71] = 4; DEFixLen[73] = 6;
                    DEFixLen[74] = 10; DEFixLen[75] = 10; DEFixLen[76] = 10; DEFixLen[77] = 10;
                    DEFixLen[78] = 10; DEFixLen[79] = 10; DEFixLen[80] = 10; DEFixLen[81] = 10;
                    DEFixLen[82] = 12; DEFixLen[83] = 12; DEFixLen[84] = 12; DEFixLen[85] = 12;
                    DEFixLen[86] = 15; DEFixLen[87] = 15; DEFixLen[88] = 15; DEFixLen[89] = 15;
                    DEFixLen[90] = 42; DEFixLen[91] = -1; DEFixLen[92] = -2; DEFixLen[93] = 5;
                    DEFixLen[94] = -7; DEFixLen[95] = -42; DEFixLen[96] = -8; DEFixLen[97] = 16;
                    DEFixLen[98] = -25; DEFixLen[128] = -16;
                    break;
                case 3:
                    //Initialize BitMap Var Length Indicator
                    DEVarLen[2] = 2; DEVarLen[32] = 2; DEVarLen[33] = 2; DEVarLen[34] = 2; DEVarLen[35] = 2; DEVarLen[36] = 3;
                    DEVarLen[44] = 2; DEVarLen[45] = 2; DEVarLen[46] = 3; DEVarLen[47] = 3; DEVarLen[48] = 3; DEVarLen[54] = 3;
                    DEVarLen[55] = 3; DEVarLen[56] = 3; DEVarLen[57] = 3; DEVarLen[58] = 3; DEVarLen[59] = 3;
                    DEVarLen[60] = 1; DEVarLen[61] = 3; DEVarLen[62] = 3; DEVarLen[63] = 2; DEVarLen[72] = 3; DEVarLen[99] = 2;
                    DEVarLen[100] = 2; DEVarLen[101] = 2; DEVarLen[102] = 2; DEVarLen[103] = 2; DEVarLen[104] = 3; DEVarLen[105] = 3;
                    DEVarLen[106] = 3; DEVarLen[107] = 3; DEVarLen[108] = 3; DEVarLen[109] = 3; DEVarLen[110] = 3;
                    DEVarLen[111] = 3; DEVarLen[112] = 3; DEVarLen[113] = 2; DEVarLen[114] = 3; DEVarLen[115] = 3;
                    DEVarLen[116] = 3; DEVarLen[117] = 3; DEVarLen[118] = 3; DEVarLen[119] = 3; DEVarLen[120] = 3; DEVarLen[121] = 3;
                    DEVarLen[122] = 3; DEVarLen[123] = 3; DEVarLen[124] = 3; DEVarLen[125] = 2; DEVarLen[126] = 1; DEVarLen[127] = 3;

                    // "-" means not numeric.

                    DEFixLen[0] = 16; DEFixLen[1] = 16; DEFixLen[3] = 6; DEFixLen[4] = 12;
                    DEFixLen[5] = 12; DEFixLen[6] = 12; DEFixLen[7] = 10; DEFixLen[8] = 8;
                    DEFixLen[9] = 8; DEFixLen[10] = 8; DEFixLen[11] = 6; DEFixLen[12] = 6;
                    DEFixLen[13] = 4; DEFixLen[14] = 4; DEFixLen[15] = 4; DEFixLen[16] = 4;
                    DEFixLen[17] = 4; DEFixLen[18] = 4; DEFixLen[19] = 4; DEFixLen[20] = 3;
                    DEFixLen[21] = 3; DEFixLen[22] = 3; DEFixLen[23] = 3; DEFixLen[24] = -3;
                    DEFixLen[25] = 2; DEFixLen[26] = 2; DEFixLen[27] = 1; DEFixLen[28] = 8;
                    DEFixLen[29] = 8; DEFixLen[30] = 8; DEFixLen[31] = 8; DEFixLen[37] = -12;
                    DEFixLen[38] = -6; DEFixLen[39] = -2; DEFixLen[40] = -3; DEFixLen[41] = -8;
                    DEFixLen[42] = -15; DEFixLen[43] = -40; DEFixLen[49] = 3; DEFixLen[50] = -3;
                    DEFixLen[51] = -3; DEFixLen[52] = 16; DEFixLen[53] = 18; DEFixLen[64] = 16;
                    DEFixLen[65] = 16; DEFixLen[66] = 1; DEFixLen[67] = 2; DEFixLen[68] = 3;
                    DEFixLen[69] = 3; DEFixLen[70] = 3; DEFixLen[71] = 4; DEFixLen[73] = 6;
                    DEFixLen[74] = 10; DEFixLen[75] = 10; DEFixLen[76] = 10; DEFixLen[77] = 10;
                    DEFixLen[78] = 10; DEFixLen[79] = 10; DEFixLen[80] = 10; DEFixLen[81] = 10;
                    DEFixLen[82] = 12; DEFixLen[83] = 12; DEFixLen[84] = 12; DEFixLen[85] = 12;
                    DEFixLen[86] = 16; DEFixLen[87] = 16; DEFixLen[88] = 16; DEFixLen[89] = 16;
                    DEFixLen[90] = 42; DEFixLen[91] = -1; DEFixLen[92] = -2; DEFixLen[93] = 5;
                    DEFixLen[94] = 7; DEFixLen[95] = 36; DEFixLen[96] = -16; DEFixLen[97] = 16;
                    DEFixLen[98] = -25; DEFixLen[128] = -16;
                    break;
                case 4:
                    //Initialize BitMap Var Length Indicator
                    DEVarLen[2] = 2; DEVarLen[32] = 2; DEVarLen[33] = 2; DEVarLen[34] = 2; DEVarLen[35] = 2; DEVarLen[36] = 3;
                    DEVarLen[44] = 2; DEVarLen[45] = 2; DEVarLen[46] = 3; DEVarLen[47] = 3; DEVarLen[48] = 3; DEVarLen[54] = 3;
                    DEVarLen[55] = 3; DEVarLen[56] = 3; DEVarLen[57] = 3; DEVarLen[58] = 3; DEVarLen[59] = 3;
                    DEVarLen[60] = 1; DEVarLen[61] = 3; DEVarLen[62] = 3; DEVarLen[63] = 2; DEVarLen[72] = 3; DEVarLen[99] = 2;
                    DEVarLen[100] = 2; DEVarLen[101] = 2; DEVarLen[102] = 2; DEVarLen[103] = 2; DEVarLen[104] = 3; DEVarLen[105] = 3;
                    DEVarLen[106] = 3; DEVarLen[107] = 3; DEVarLen[108] = 3; DEVarLen[109] = 3; DEVarLen[110] = 3;
                    DEVarLen[111] = 3; DEVarLen[112] = 3; DEVarLen[113] = 2; DEVarLen[114] = 3; DEVarLen[115] = 3;
                    DEVarLen[116] = 3; DEVarLen[117] = 3; DEVarLen[118] = 3; DEVarLen[119] = 3; DEVarLen[120] = 3; DEVarLen[121] = 3;
                    DEVarLen[122] = 3; DEVarLen[123] = 3; DEVarLen[124] = 3; DEVarLen[125] = 2; DEVarLen[126] = 1; DEVarLen[127] = 3;

                    // "-" means not numeric.

                    DEFixLen[0] = 16; DEFixLen[1] = 16; DEFixLen[3] = 6; DEFixLen[4] = 12;
                    DEFixLen[5] = 12; DEFixLen[6] = 12; DEFixLen[7] = 10; DEFixLen[8] = 8;
                    DEFixLen[9] = 8; DEFixLen[10] = 8; DEFixLen[11] = 6; DEFixLen[12] = 6;
                    DEFixLen[13] = 4; DEFixLen[14] = 4; DEFixLen[15] = 4; DEFixLen[16] = 4;
                    DEFixLen[17] = 4; DEFixLen[18] = 4; DEFixLen[19] = 4; DEFixLen[20] = 3;
                    DEFixLen[21] = 3; DEFixLen[22] = 3; DEFixLen[23] = 3; DEFixLen[24] = -3;
                    DEFixLen[25] = 2; DEFixLen[26] = 2; DEFixLen[27] = 1; DEFixLen[28] = 8;
                    DEFixLen[29] = 8; DEFixLen[30] = 8; DEFixLen[31] = 8; DEFixLen[37] = -12;
                    DEFixLen[38] = -6; DEFixLen[39] = -2; DEFixLen[40] = -3; DEFixLen[41] = -8;
                    DEFixLen[42] = -15; DEFixLen[43] = -40; DEFixLen[49] = 3; DEFixLen[50] = -3;
                    DEFixLen[51] = -3; DEFixLen[52] = 16; DEFixLen[53] = 18; DEFixLen[64] = 16;
                    DEFixLen[65] = 16; DEFixLen[66] = 1; DEFixLen[67] = 2; DEFixLen[68] = 3;
                    DEFixLen[69] = 3; DEFixLen[70] = 3; DEFixLen[71] = 4; DEFixLen[73] = 6;
                    DEFixLen[74] = 10; DEFixLen[75] = 10; DEFixLen[76] = 10; DEFixLen[77] = 10;
                    DEFixLen[78] = 10; DEFixLen[79] = 10; DEFixLen[80] = 10; DEFixLen[81] = 10;
                    DEFixLen[82] = 12; DEFixLen[83] = 12; DEFixLen[84] = 12; DEFixLen[85] = 12;
                    DEFixLen[86] = 16; DEFixLen[87] = 16; DEFixLen[88] = 16; DEFixLen[89] = 16;
                    DEFixLen[90] = 42; DEFixLen[91] = -1; DEFixLen[92] = -2; DEFixLen[93] = 5;
                    DEFixLen[94] = 7; DEFixLen[95] = 36; DEFixLen[96] = -16; DEFixLen[97] = 16;
                    DEFixLen[98] = -25; DEFixLen[128] = -16;
                    break;
            }






        }
        public string Build(string[] DE, string MTI)
        {
            string newISO = "";
            if (InputType == 4 || InputType == 3)
            {
                newISO = TCPUtil.StringToHEXUno(MTI);
            }
            else
            {
                newISO = MTI;
            }


            string newDE1 = "";
            for (int I = 2; I <= 64; I++) { if (DE[I] != null) { newDE1 += "1"; } else { newDE1 += "0"; } }


            string newDE2 = "";
            for (int I = 65; I <= 128; I++) { if (DE[I] != null) { newDE2 += "1"; } else { newDE2 += "0"; } }

            if (newDE2 == "0000000000000000000000000000000000000000000000000000000000000000")
            { newDE1 = "0" + newDE1; }
            else { newDE1 = "1" + newDE1; }



            string DE1Hex = String.Format("{0:X1}", Convert.ToInt64(newDE1, 2));
            DE1Hex = DE1Hex.PadLeft(16, '0'); //Pad-Left
            DE[0] = DE1Hex;

            string DE2Hex = String.Format("{0:X1}", Convert.ToInt64(newDE2, 2));
            DE2Hex = DE2Hex.PadLeft(16, '0'); //Pad-Left
            DE[1] = DE2Hex;

            if (DE2Hex == "0000000000000000") DE[1] = null;



            for (int I = 0; I <= 128; I++)
            {
                if (DE[I] != null)
                {
                    if (DEVarLen[I] < 1)
                    {
                        if (DEFixLen[I] < 0)
                        {
                            if (I != 49)
                            {
                                if (InputType == 1)
                                {
                                    string BMPadded = DE[I].PadRight(Math.Abs(DEFixLen[I]), ' ');
                                    string sBM = BMPadded.Substring(0, Math.Abs(DEFixLen[I]));

                                    sBM = TCPUtil.StringToEBCDic(BMPadded);
                                    newISO += sBM;
                                }
                                else if (InputType == 3 || InputType == 4)
                                {
                                    Int32 Lnt = DEFixLen[I] * -1;
                                    string BMPadded = DE[I].PadLeft(Lnt, '0');
                                    string sBM = BMPadded.Substring(BMPadded.Length - Math.Abs(Lnt), Math.Abs(Lnt));

                                    newISO += TCPUtil.StringToHEXUno(sBM);
                                }

                            }
                            else
                            {
                                string BMPadded = DE[I].PadLeft(DEFixLen[I], '0');
                            }
                        }
                        else
                        {
                            if (InputType == 1)
                            {
                                if (I == 22)
                                {
                                    String CPinPan = DE[I];
                                    if (DE[I].Length == 3) //EWA ||Bebertec
                                    {
                                        CPinPan = F22Traduce(DE[I].Substring(0, 2), DE[I].Substring(2, 1));
                                    }
                                    string BMPadded = CPinPan.PadRight(DEFixLen[I], '0');
                                    string sBM = BMPadded.Substring(BMPadded.Length - Math.Abs(DEFixLen[I]), Math.Abs(DEFixLen[I]));
                                    newISO += sBM;
                                }
                                else if (I == 28 || I == 29)
                                {
                                    Int32 Lnt = DEFixLen[I] + 1;
                                    string BMPadded = DE[I].PadLeft(Lnt, '0');
                                    string sBM = BMPadded.Substring(BMPadded.Length - Math.Abs(Lnt), Math.Abs(Lnt));

                                    newISO += sBM;
                                }
                                else
                                {
                                    string BMPadded = DE[I].PadLeft(DEFixLen[I], '0');
                                    string sBM = BMPadded.Substring(BMPadded.Length - Math.Abs(DEFixLen[I]), Math.Abs(DEFixLen[I]));
                                    newISO += sBM;
                                }
                            }
                            else if (InputType == 3 || InputType == 4)
                            {
                                if (I == 0 || I == 1)
                                {
                                    Int32 Lnt = DEFixLen[I];
                                    string BMPadded = DE[I].PadLeft(Lnt, '0');
                                    string sBM = BMPadded.Substring(BMPadded.Length - Math.Abs(Lnt), Math.Abs(Lnt));
                                    if (InputType == 4)
                                    {
                                        newISO += TCPUtil.StringToHEXUno(sBM);
                                    }
                                    else
                                    {
                                        newISO += sBM;
                                    }

                                }
                                else
                                {
                                    if (I == 22)
                                    {
                                        String CPinPan = DE[I];
                                        if (DE[I].Length == 4) //Es un BaseI
                                        {
                                            CPinPan = TraduceF22(DE[I].Substring(0, 2), DE[I].Substring(2, 1));
                                        }
                                        Int32 Lnt = CPinPan.Length;
                                        string BMPadded = CPinPan.PadLeft(Lnt, '0');
                                        string sBM = BMPadded.Substring(BMPadded.Length - Math.Abs(Lnt), Math.Abs(Lnt));

                                        newISO += TCPUtil.StringToHEXUno(sBM);
                                    }
                                    else if (I == 28 || I == 29)
                                    {
                                        Int32 Lnt = DEFixLen[I] + 1;
                                        string BMPadded = DE[I].PadLeft(Lnt, '0');
                                        string sBM = BMPadded.Substring(BMPadded.Length - Math.Abs(Lnt), Math.Abs(Lnt));

                                        newISO += TCPUtil.StringToHEXUno(sBM);
                                    }
                                    else if (I == 53)
                                    {
                                        if (DE[52] != null)
                                        {
                                            Int32 Lnt = DEFixLen[I];
                                            string BMPadded = DE[I].PadLeft(Lnt, '0');
                                            string sBM = BMPadded.Substring(BMPadded.Length - Math.Abs(Lnt), Math.Abs(Lnt));

                                            newISO += TCPUtil.StringToHEXUno(sBM);
                                        }
                                        else
                                        {
                                            Int32 Lnt = 16;
                                            string BMPadded = 0.ToString().PadLeft(Lnt, '0');
                                            string sBM = BMPadded.Substring(BMPadded.Length - Math.Abs(Lnt), Math.Abs(Lnt));

                                            newISO += TCPUtil.StringToHEXUno(sBM);
                                        }
                                    }
                                    else
                                    {
                                        Int32 Lnt = DEFixLen[I];
                                        string BMPadded = DE[I].PadLeft(Lnt, '0');
                                        string sBM = BMPadded.Substring(BMPadded.Length - Math.Abs(Lnt), Math.Abs(Lnt));

                                        newISO += TCPUtil.StringToHEXUno(sBM);
                                    }

                                }
                            }

                        }
                    }
                    else
                    {
                        if (InputType == 1)
                        {
                            Int32 Lengt = DE[I].Length;
                            if (I == 63)
                            {
                                Lengt = Lengt / 2;
                            }
                            string paddedli = Lengt.ToString("X");
                            if (paddedli.Length < DEVarLen[I])
                            {
                                paddedli = Lengt.ToString("X").PadLeft(DEVarLen[I], '0');
                            }

                            if (I == 63)
                            {
                                newISO += (paddedli + DE[I]);
                            }
                            else
                            {
                                if (Lengt % 2 != 0) //Si es Impar añadir un 0
                                {
                                    newISO += (paddedli + "0" + DE[I]);
                                }
                                else
                                {
                                    if (I == 102)
                                    {
                                        newISO += (paddedli + TCPUtil.StringToEBCDic(DE[I]));
                                    }
                                    else
                                    {
                                        newISO += (paddedli + DE[I]);
                                    }
                                }
                            }
                        }
                        else if (InputType == 3 || InputType == 4)
                        {
                            String L = "";
                            Int32 Lengt = DE[I].Length;
                            if (Lengt < 10)
                            {
                                L = "0" + Lengt.ToString();
                            }
                            else
                            {
                                L = Lengt.ToString();
                            }

                            String PadLi = TCPUtil.StringToHEXUno(L);
                            newISO += (PadLi + TCPUtil.StringToHEXUno(DE[I]));
                        }

                    }
                }
            }

            return newISO;


        }

        public String F22Traduce(String CodePAN, String CodePIN)
        {
            String Ret = "";
            switch (CodePAN)
            {
                case "00":
                    Ret = "00";
                    break;
                case "01":
                    Ret = "01";
                    break;
                case "02":
                    Ret = "90";
                    break;
                case "03":
                    Ret = "03";
                    break;
                case "04":
                    Ret = "04";
                    break;
                case "05":
                    Ret = "05";
                    break;
            }
            switch (CodePIN)
            {
                case "0":
                    Ret += "0";
                    break;
                case "1":
                    Ret += "1";
                    break;
                case "2":
                    Ret += "2";
                    break;
            }
            return Ret;
        }

        public String TraduceF22(String CodePAN, String CodePIN)
        {
            String Ret = "";
            switch (CodePAN)
            {
                case "00":
                case "06":
                case "10":
                case "96":
                    Ret = "00";
                    break;
                case "01":
                    Ret = "01";
                    break;
                case "02":
                case "90":
                case "91":
                    Ret = "02";
                    break;
                case "03":
                    Ret = "03";
                    break;
                case "04":
                    Ret = "04";
                    break;
                case "05":
                case "07":
                case "95":
                    Ret = "05";
                    break;
            }
            switch (CodePIN)
            {
                case "0":
                case "9":
                    Ret += "0";
                    break;
                case "1":
                    Ret += "1";
                    break;
                case "2":
                case "8":
                    Ret += "2";
                    break;
            }
            return Ret;
        }


        private Int32 GetLength(Int32 Length)
        {
            Int32 Ret = 0;
            switch (InputType)
            {
                case 1:
                    Ret = Length;
                    break;
                case 3:
                case 4:
                    Ret = Length * 2;
                    break;
            }
            return Ret;
        }

        public string[] Parse(String ISOmsg, Boolean Translate)
        {
            String[] DE = new String[130];

            string de1Binary = "";
            string de2Binary = "";
            int FieldNo;

            Boolean ConvertToAscii = InputType == 3 || InputType == 4;

            int myPos;
            int myLenght;
            int len;
            //Get MTI

            myPos = 0;

            myLenght = GetLength(4);
            string MTI = ISOmsg.Substring(myPos, myLenght);
            if (ConvertToAscii)
            {
                MTI = Encoding.ASCII.GetString(TCPUtil.ConvertHexStringToByteArray(MTI));
            }

            //========BM 129 is the MTI============
            FieldNo = 129;


            DE[FieldNo] = MTI;
            //========================================
            //Get BitMap 1a
            myPos += myLenght;
            if (InputType == 4)
            {
                myLenght = GetLength(16);
                String Str = Encoding.ASCII.GetString(TCPUtil.ConvertHexStringToByteArray(ISOmsg.Substring(myPos, myLenght)));
                DE[0] = Str;

            }
            else
            {
                myLenght = 16;  //<==========Ajustado para Visa
                DE[0] = ISOmsg.Substring(myPos, myLenght);
            }

            //Convert BM0 to Binary
            de1Binary = DEtoBinary(DE[0]);
            //BitMap #1

            FieldNo = 1;
            if (de1Binary.Substring(FieldNo - 1, 1) == "1")
            {
                myPos += myLenght;
                if (InputType == 4)
                {
                    myLenght = GetLength(16);
                    DE[FieldNo] = Encoding.ASCII.GetString(TCPUtil.ConvertHexStringToByteArray(ISOmsg.Substring(myPos, myLenght)));
                }
                else
                {
                    myLenght = 16;
                    DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                }
                de2Binary = DEtoBinary(DE[FieldNo]);
            }

            //------------BM2--------------
            FieldNo = 2;

            if (de1Binary.Substring(FieldNo - 1, 1) == "1")
            {
                myPos += myLenght;
                if (ConvertToAscii)
                {
                    String Str = Encoding.ASCII.GetString(TCPUtil.ConvertHexStringToByteArray(ISOmsg.Substring(myPos, GetLength(2))));
                    myLenght = Int32.Parse(Str);

                    if (Translate)
                    {
                        myPos += 4;
                        DE[FieldNo] = Encoding.ASCII.GetString(TCPUtil.ConvertHexStringToByteArray(ISOmsg.Substring(myPos, GetLength(myLenght))));
                        myLenght = myLenght * 2;
                    }
                    else
                    {
                        myLenght += 2;
                        myLenght = myLenght * 2;
                        DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                    }

                }
                else
                {
                    myLenght = Int32.Parse(ISOmsg.Substring(myPos, 2), System.Globalization.NumberStyles.HexNumber);

                    if (Translate)
                    {
                        myPos += 2;
                    }
                    else
                    {
                        myLenght += 2;
                    }

                    DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                }
            }

            //------------BM3--------------
            FieldNo = 3;

            if (de1Binary.Substring(FieldNo - 1, 1) == "1")
            {
                myPos += myLenght;
                myLenght = GetLength(6);
                if (ConvertToAscii)
                {
                    if (Translate)
                    {
                        DE[FieldNo] = Encoding.ASCII.GetString(TCPUtil.ConvertHexStringToByteArray(ISOmsg.Substring(myPos, myLenght)));
                    }
                    else
                    {
                        DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                    }

                }
                else
                {
                    DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                }
            }


            //------------BM4--------------
            FieldNo = 4;

            if (de1Binary.Substring(FieldNo - 1, 1) == "1")
            {
                myPos += myLenght;
                myLenght = GetLength(12);
                if (ConvertToAscii)
                {
                    if (Translate)
                    {
                        DE[FieldNo] = Encoding.ASCII.GetString(TCPUtil.ConvertHexStringToByteArray(ISOmsg.Substring(myPos, myLenght)));
                    }
                    else
                    {
                        DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                    }

                }
                else
                {
                    DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                }

            }

            //------------BM5--------------
            FieldNo = 5;

            if (de1Binary.Substring(FieldNo - 1, 1) == "1")
            {
                myPos += myLenght;
                myLenght = GetLength(12);
                if (ConvertToAscii)
                {
                    if (Translate)
                    {
                        DE[FieldNo] = Encoding.ASCII.GetString(TCPUtil.ConvertHexStringToByteArray(ISOmsg.Substring(myPos, myLenght)));
                    }
                    else
                    {
                        DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                    }
                }
                else
                {
                    DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                }
            }

            //------------BM6--------------
            FieldNo = 6;

            if (de1Binary.Substring(FieldNo - 1, 1) == "1")
            {
                myPos += myLenght;
                myLenght = GetLength(12);
                if (ConvertToAscii)
                {
                    if (Translate)
                    {
                        DE[FieldNo] = Encoding.ASCII.GetString(TCPUtil.ConvertHexStringToByteArray(ISOmsg.Substring(myPos, myLenght)));
                    }
                    else
                    {
                        DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                    }
                }
                else
                {
                    DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                }
            }

            //------------BM7--------------
            FieldNo = 7;

            if (de1Binary.Substring(FieldNo - 1, 1) == "1")
            {
                myPos += myLenght; myLenght = GetLength(10);
                if (ConvertToAscii)
                {
                    if (Translate)
                    {
                        DE[FieldNo] = Encoding.ASCII.GetString(TCPUtil.ConvertHexStringToByteArray(ISOmsg.Substring(myPos, myLenght)));
                    }
                    else
                    {
                        DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                    }
                }
                else
                {
                    DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                }
            }

            //------------BM8--------------
            FieldNo = 8;

            if (de1Binary.Substring(FieldNo - 1, 1) == "1") { myPos += myLenght; myLenght = 8; DE[FieldNo] = ISOmsg.Substring(myPos, myLenght); }

            //------------BM9--------------
            FieldNo = 9;

            if (de1Binary.Substring(FieldNo - 1, 1) == "1") { myPos += myLenght; myLenght = 8; DE[FieldNo] = ISOmsg.Substring(myPos, myLenght); }

            //------------BM 10--------------
            FieldNo = 10;

            if (de1Binary.Substring(FieldNo - 1, 1) == "1")
            {
                myPos += myLenght;
                myLenght = GetLength(8);
                if (ConvertToAscii)
                {
                    if (Translate)
                    {
                        DE[FieldNo] = Encoding.ASCII.GetString(TCPUtil.ConvertHexStringToByteArray(ISOmsg.Substring(myPos, myLenght)));
                    }
                    else
                    {
                        DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                    }
                }
                else
                {
                    DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                }
            }

            //------------BM 11--------------
            FieldNo = 11;

            if (de1Binary.Substring(FieldNo - 1, 1) == "1")
            {
                myPos += myLenght; myLenght = GetLength(6);
                if (ConvertToAscii)
                {
                    if (Translate)
                    {
                        DE[FieldNo] = Encoding.ASCII.GetString(TCPUtil.ConvertHexStringToByteArray(ISOmsg.Substring(myPos, myLenght)));
                    }
                    else
                    {
                        DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                    }
                }
                else
                {
                    DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                }
            }

            //------------BM 12--------------
            FieldNo = 12;

            if (de1Binary.Substring(FieldNo - 1, 1) == "1")
            {
                myPos += myLenght;
                myLenght = GetLength(6);
                if (ConvertToAscii)
                {
                    if (Translate)
                    {
                        DE[FieldNo] = Encoding.ASCII.GetString(TCPUtil.ConvertHexStringToByteArray(ISOmsg.Substring(myPos, myLenght)));
                    }
                    else
                    {
                        DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                    }
                }
                else
                {
                    DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                }
            }

            //------------BM 13--------------
            FieldNo = 13;

            if (de1Binary.Substring(FieldNo - 1, 1) == "1")
            {
                myPos += myLenght;
                myLenght = GetLength(4);
                if (ConvertToAscii)
                {
                    if (Translate)
                    {
                        DE[FieldNo] = Encoding.ASCII.GetString(TCPUtil.ConvertHexStringToByteArray(ISOmsg.Substring(myPos, myLenght)));
                    }
                    else
                    {
                        DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                    }
                }
                else
                {
                    DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                }
            }

            //------------BM 14--------------
            FieldNo = 14;

            if (de1Binary.Substring(FieldNo - 1, 1) == "1")
            {
                myPos += myLenght;
                myLenght = GetLength(4);
                if (ConvertToAscii)
                {
                    if (Translate)
                    {
                        DE[FieldNo] = Encoding.ASCII.GetString(TCPUtil.ConvertHexStringToByteArray(ISOmsg.Substring(myPos, myLenght)));
                    }
                    else
                    {
                        DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                    }
                }
                else
                {
                    DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                }
            }

            //------------BM 15--------------
            FieldNo = 15;

            if (de1Binary.Substring(FieldNo - 1, 1) == "1")
            {
                myPos += myLenght;
                myLenght = GetLength(4);
                if (ConvertToAscii)
                {
                    if (Translate)
                    {
                        DE[FieldNo] = Encoding.ASCII.GetString(TCPUtil.ConvertHexStringToByteArray(ISOmsg.Substring(myPos, myLenght)));
                    }
                    else
                    {
                        DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                    }
                }
                else
                {
                    DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                }
            }

            //------------BM 16--------------
            FieldNo = 16;

            if (de1Binary.Substring(FieldNo - 1, 1) == "1")
            {
                myPos += myLenght;
                myLenght = GetLength(4);
                if (ConvertToAscii)
                {
                    if (Translate)
                    {
                        DE[FieldNo] = Encoding.ASCII.GetString(TCPUtil.ConvertHexStringToByteArray(ISOmsg.Substring(myPos, myLenght)));
                    }
                    else
                    {
                        DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                    }
                }
                else
                {
                    DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                }
            }

            //------------BM 17--------------
            FieldNo = 17;

            if (de1Binary.Substring(FieldNo - 1, 1) == "1")
            {
                myPos += myLenght;
                myLenght = GetLength(4);
                if (ConvertToAscii)
                {
                    if (Translate)
                    {
                        DE[FieldNo] = Encoding.ASCII.GetString(TCPUtil.ConvertHexStringToByteArray(ISOmsg.Substring(myPos, myLenght)));
                    }
                    else
                    {
                        DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                    }
                }
                else
                {
                    DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                }


            }

            //------------BM 18--------------
            FieldNo = 18;

            if (de1Binary.Substring(FieldNo - 1, 1) == "1")
            {
                myPos += myLenght;
                myLenght = GetLength(4);
                if (ConvertToAscii)
                {
                    if (Translate)
                    {
                        DE[FieldNo] = Encoding.ASCII.GetString(TCPUtil.ConvertHexStringToByteArray(ISOmsg.Substring(myPos, myLenght)));
                    }
                    else
                    {
                        DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                    }
                }
                else
                {
                    DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                }
            }
            //------------BM 19--------------
            FieldNo = 19;

            if (de1Binary.Substring(FieldNo - 1, 1) == "1")
            {
                myPos += myLenght;
                if (ConvertToAscii)
                {
                    if (InputType == 3)
                    {
                        myLenght = GetLength(3);
                    }
                    else
                    {
                        myLenght = GetLength(3);
                    }
                    if (Translate)
                    {
                        DE[FieldNo] = Encoding.ASCII.GetString(TCPUtil.ConvertHexStringToByteArray(ISOmsg.Substring(myPos, myLenght)));
                    }
                    else
                    {
                        DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                    }
                }
                else
                {
                    myLenght = 4;
                    DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                }
            }

            //------------BM 20--------------
            FieldNo = 20;

            if (de1Binary.Substring(FieldNo - 1, 1) == "1") { myPos += myLenght; myLenght = 3; DE[FieldNo] = ISOmsg.Substring(myPos, myLenght); }
            //------------BM 21--------------
            FieldNo = 21;

            if (de1Binary.Substring(FieldNo - 1, 1) == "1") { myPos += myLenght; myLenght = 3; DE[FieldNo] = ISOmsg.Substring(myPos, myLenght); }
            //------------BM 22--------------
            FieldNo = 22;

            if (de1Binary.Substring(FieldNo - 1, 1) == "1")
            {
                myPos += myLenght;

                if (ConvertToAscii)
                {
                    myLenght = GetLength(3);
                    if (Translate)
                    {
                        DE[FieldNo] = Encoding.ASCII.GetString(TCPUtil.ConvertHexStringToByteArray(ISOmsg.Substring(myPos, myLenght)));
                    }
                    else
                    {
                        DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                    }
                }
                else
                {
                    myLenght = GetLength(4);
                    DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                }
            }
            //--------------------------
            FieldNo = 23;

            if (de1Binary.Substring(FieldNo - 1, 1) == "1") { myPos += myLenght; myLenght = 4; DE[FieldNo] = ISOmsg.Substring(myPos, myLenght); }
            //--------------------------
            FieldNo = 24;

            if (de1Binary.Substring(FieldNo - 1, 1) == "1")
            {
                myPos += myLenght;
                myLenght = GetLength(3);
                if (ConvertToAscii)
                {
                    if (Translate)
                    {
                        DE[FieldNo] = Encoding.ASCII.GetString(TCPUtil.ConvertHexStringToByteArray(ISOmsg.Substring(myPos, myLenght)));
                    }
                    else
                    {
                        DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                    }
                }
                else
                {
                    DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                }
            }
            //--------------------------
            FieldNo = 25;

            if (de1Binary.Substring(FieldNo - 1, 1) == "1")
            {
                myPos += myLenght;
                myLenght = GetLength(2);
                if (ConvertToAscii)
                {
                    if (Translate)
                    {
                        DE[FieldNo] = Encoding.ASCII.GetString(TCPUtil.ConvertHexStringToByteArray(ISOmsg.Substring(myPos, myLenght)));
                    }
                    else
                    {
                        DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                    }
                }
                else
                {
                    DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                }
            }
            //--------------------------
            FieldNo = 26;

            if (de1Binary.Substring(FieldNo - 1, 1) == "1") { myPos += myLenght; myLenght = 2; DE[FieldNo] = ISOmsg.Substring(myPos, myLenght); }
            //--------------------------
            FieldNo = 27;

            if (de1Binary.Substring(FieldNo - 1, 1) == "1") { myPos += myLenght; myLenght = 1; DE[FieldNo] = ISOmsg.Substring(myPos, myLenght); }
            //--------------------------
            FieldNo = 28; //Campos EBCDIC Considerar su lenght fijo *2 ej. 9 = 18, 12 = 24

            if (de1Binary.Substring(FieldNo - 1, 1) == "1")
            {
                myPos += myLenght;
                if (ConvertToAscii)
                {
                    myLenght = GetLength(9);
                    if (Translate)
                    {
                        DE[FieldNo] = Encoding.ASCII.GetString(TCPUtil.ConvertHexStringToByteArray(ISOmsg.Substring(myPos, myLenght)));
                    }
                    else
                    {
                        DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                    }
                }
                else
                {
                    myLenght = 18;

                    if (Translate)
                    {
                        String Dato = ISOmsg.Substring(myPos, myLenght);
                        DE[FieldNo] = TCPUtil.EBDicToString(Dato);
                    }
                    else
                    {
                        DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                    }
                }
            }
            //--------------------------
            FieldNo = 29;

            if (de1Binary.Substring(FieldNo - 1, 1) == "1")
            {
                myPos += myLenght;
                myLenght = GetLength(9);
                if (ConvertToAscii)
                {
                    if (Translate)
                    {
                        DE[FieldNo] = Encoding.ASCII.GetString(TCPUtil.ConvertHexStringToByteArray(ISOmsg.Substring(myPos, myLenght)));
                    }
                    else
                    {
                        DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                    }
                }
                else
                {
                    DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                }
            }
            //--------------------------
            FieldNo = 30;

            if (de1Binary.Substring(FieldNo - 1, 1) == "1")
            {
                myPos += myLenght;
                myLenght = GetLength(9);
                if (ConvertToAscii)
                {
                    if (Translate)
                    {
                        DE[FieldNo] = Encoding.ASCII.GetString(TCPUtil.ConvertHexStringToByteArray(ISOmsg.Substring(myPos, myLenght)));
                    }
                    else
                    {
                        DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                    }
                }
                else
                {
                    DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                }
            }
            //--------------------------
            FieldNo = 31;

            if (de1Binary.Substring(FieldNo - 1, 1) == "1")
            {
                myPos += myLenght;
                myLenght = GetLength(9);
                if (ConvertToAscii)
                {
                    if (Translate)
                    {
                        DE[FieldNo] = Encoding.ASCII.GetString(TCPUtil.ConvertHexStringToByteArray(ISOmsg.Substring(myPos, myLenght)));
                    }
                    else
                    {
                        DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                    }
                }
                else
                {
                    DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                }
            }
            //--------------------------
            FieldNo = 32;

            if (de1Binary.Substring(FieldNo - 1, 1) == "1")
            {
                myPos += myLenght;
                if (ConvertToAscii)
                {
                    myLenght = Int32.Parse(Encoding.ASCII.GetString(TCPUtil.ConvertHexStringToByteArray(ISOmsg.Substring(myPos, GetLength(2)))));
                    if (Translate)
                    {
                        if (myLenght % 2 != 0)
                        {
                            myPos += 2;
                        }
                        myPos += 4;
                        DE[FieldNo] = Encoding.ASCII.GetString(TCPUtil.ConvertHexStringToByteArray(ISOmsg.Substring(myPos, GetLength(myLenght))));
                        myLenght = myLenght * 2;
                    }
                    else
                    {
                        if (myLenght % 2 != 0)
                        {
                            myLenght += 1;
                        }
                        myLenght += 2;
                        myLenght = myLenght * 2;
                        DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                    }
                }
                else
                {
                    myLenght = Int32.Parse(ISOmsg.Substring(myPos, 2), System.Globalization.NumberStyles.HexNumber);
                    if (Translate)
                    {
                        if (myLenght % 2 != 0) //Numero Impar
                        {
                            myPos += 1;//Si el numero es Impar, se debe de poner un cero el cual no es parte de la cadena, se salta
                        }
                        myPos += 2;
                    }
                    else
                    {
                        if (myLenght % 2 != 0) //Numero Impar
                        {
                            myLenght += 1;//Si el numero es Impar, se debe de poner un cero el cual no es parte de la cadena, se salta
                        }
                        myLenght += 2;
                    }
                    DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                }
            }

            //--------------------------
            FieldNo = 33;

            if (de1Binary.Substring(FieldNo - 1, 1) == "1")
            {
                myPos += myLenght;
                if (ConvertToAscii)
                {
                    myLenght = Int32.Parse(Encoding.ASCII.GetString(TCPUtil.ConvertHexStringToByteArray(ISOmsg.Substring(myPos, GetLength(2)))));
                    myLenght = myLenght * 2;
                    if (Translate)
                    {
                        myPos += GetLength(2);
                        DE[FieldNo] = Encoding.ASCII.GetString(TCPUtil.ConvertHexStringToByteArray(ISOmsg.Substring(myPos, myLenght)));
                    }
                    else
                    {
                        myLenght += GetLength(2);
                        DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                    }
                }
                else
                {

                    myLenght = Int32.Parse(ISOmsg.Substring(myPos, 2), System.Globalization.NumberStyles.HexNumber);
                    if (Translate)
                    {
                        myPos += 2;
                    }
                    else
                    {
                        myLenght += 2;
                    }

                    DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                }
            }
            //--------------------------
            FieldNo = 34;

            if (de1Binary.Substring(FieldNo - 1, 1) == "1")
            {
                myPos += myLenght;
                myLenght = Int32.Parse(ISOmsg.Substring(myPos, 2), System.Globalization.NumberStyles.HexNumber);
                if (Translate)
                {
                    myPos += 2;
                }
                else
                {
                    myLenght += 2;
                }
                DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
            }
            //--------------------------
            FieldNo = 35;

            if (de1Binary.Substring(FieldNo - 1, 1) == "1")
            {
                myPos += myLenght;
                len = GetLength(2);
                if (ConvertToAscii)
                {
                    myLenght = Int32.Parse(Encoding.ASCII.GetString(TCPUtil.ConvertHexStringToByteArray(ISOmsg.Substring(myPos, GetLength(2)))));
                    // myLenght = myLenght * 2;
                    if (Translate)
                    {
                        myPos += len;
                        DE[FieldNo] = Encoding.ASCII.GetString(TCPUtil.ConvertHexStringToByteArray(ISOmsg.Substring(myPos, GetLength(myLenght))));
                        myLenght = myLenght * 2;
                    }
                    else
                    {
                        myLenght += 2;
                        myLenght = myLenght * 2;
                        DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                    }
                }
                else
                {
                    myLenght = Int32.Parse(ISOmsg.Substring(myPos, len), System.Globalization.NumberStyles.HexNumber);
                    if (Translate)
                    {
                        if (myLenght % 2 != 0) //Numero Impar
                        {
                            myPos += 1;//Si el numero es Impar, se debe de poner un cero el cual no es parte de la cadena, se salta
                        }
                        myPos += 2;
                    }
                    else
                    {
                        if (myLenght % 2 != 0) //Numero Impar
                        {
                            myLenght += 1;//Si el numero es Impar, se debe de poner un cero el cual no es parte de la cadena, se salta
                        }
                        myLenght += 2;
                    }

                    DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                }
            }
            //--------------------------
            FieldNo = 36;

            if (de1Binary.Substring(FieldNo - 1, 1) == "1")
            {
                myPos += myLenght; len = 3;
                myLenght = Int32.Parse(ISOmsg.Substring(myPos, len), System.Globalization.NumberStyles.HexNumber);
                if (Translate)
                {
                    myPos += len;
                }
                else
                {
                    myLenght += 2;
                }
                DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
            }
            //--------------------------
            FieldNo = 37;  //CONVERTIR A EBCDIC

            if (de1Binary.Substring(FieldNo - 1, 1) == "1")
            {
                myPos += myLenght;

                if (ConvertToAscii)
                {
                    myLenght = GetLength(12);
                    if (Translate)
                    {
                        DE[FieldNo] = Encoding.ASCII.GetString(TCPUtil.ConvertHexStringToByteArray(ISOmsg.Substring(myPos, myLenght)));
                    }
                    else
                    {
                        DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                    }

                }
                else
                {
                    myLenght = 24;
                    if (Translate)
                    {
                        String Dato = ISOmsg.Substring(myPos, myLenght);
                        DE[FieldNo] = TCPUtil.EBDicToString(Dato);
                    }
                    else
                    {
                        DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                    }
                }

            }
            //--------------------------
            FieldNo = 38;

            if (de1Binary.Substring(FieldNo - 1, 1) == "1")
            {
                myPos += myLenght;
                if (ConvertToAscii)
                {
                    myLenght = GetLength(6);
                    if (Translate)
                    {
                        DE[FieldNo] = Encoding.ASCII.GetString(TCPUtil.ConvertHexStringToByteArray(ISOmsg.Substring(myPos, myLenght)));
                    }
                    else
                    {
                        DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                    }
                }
                else
                {
                    myLenght = 12;
                    if (Translate)
                    {
                        String Dato = ISOmsg.Substring(myPos, myLenght);
                        DE[FieldNo] = TCPUtil.EBDicToString(Dato);
                    }
                    else
                    {
                        DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                    }
                }
            }
            //--------------------------
            FieldNo = 39;

            if (de1Binary.Substring(FieldNo - 1, 1) == "1")
            {
                myPos += myLenght;
                myLenght = GetLength(2);
                if (ConvertToAscii)
                {
                    if (Translate)
                    {
                        DE[FieldNo] = Encoding.ASCII.GetString(TCPUtil.ConvertHexStringToByteArray(ISOmsg.Substring(myPos, myLenght)));
                    }
                    else
                    {
                        DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                    }
                }
                else
                {
                    myLenght = 4;
                    if (Translate)
                    {
                        DE[FieldNo] = TCPUtil.EBDicToString(ISOmsg.Substring(myPos, myLenght));
                    }
                    else
                    {
                        DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                    }
                }
            }
            //--------------------------
            FieldNo = 40;

            if (de1Binary.Substring(FieldNo - 1, 1) == "1") { myPos += myLenght; myLenght = 3; DE[FieldNo] = ISOmsg.Substring(myPos, myLenght); }
            //--------------------------
            FieldNo = 41; //EBCDIC

            if (de1Binary.Substring(FieldNo - 1, 1) == "1")
            {
                myPos += myLenght;
                myLenght = 8 * 2;
                if (ConvertToAscii)
                {
                    if (Translate)
                    {
                        DE[FieldNo] = Encoding.ASCII.GetString(TCPUtil.ConvertHexStringToByteArray(ISOmsg.Substring(myPos, myLenght)));
                    }
                    else
                    {
                        DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                    }

                }
                else
                {
                    if (Translate)
                    {
                        String Dato = ISOmsg.Substring(myPos, myLenght);
                        DE[FieldNo] = TCPUtil.EBDicToString(Dato);
                    }
                    else
                    {
                        DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                    }
                }
            }
            //--------------------------
            FieldNo = 42; //EBCDIC

            if (de1Binary.Substring(FieldNo - 1, 1) == "1")
            {
                myPos += myLenght;
                myLenght = 15 * 2;
                if (ConvertToAscii)
                {
                    if (Translate)
                    {
                        DE[FieldNo] = Encoding.ASCII.GetString(TCPUtil.ConvertHexStringToByteArray(ISOmsg.Substring(myPos, myLenght)));
                    }
                    else
                    {
                        DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                    }
                }
                else
                {
                    if (Translate)
                    {
                        String Dato = ISOmsg.Substring(myPos, myLenght);
                        DE[FieldNo] = TCPUtil.EBDicToString(Dato);
                    }
                    else
                    {
                        DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                    }
                }
            }
            //--------------------------
            FieldNo = 43;

            if (de1Binary.Substring(FieldNo - 1, 1) == "1")
            {
                myPos += myLenght;
                myLenght = 40 * 2;
                if (ConvertToAscii)
                {
                    if (Translate)
                    {
                        DE[FieldNo] = Encoding.ASCII.GetString(TCPUtil.ConvertHexStringToByteArray(ISOmsg.Substring(myPos, myLenght)));
                    }
                    else
                    {
                        DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                    }
                }
                else
                {
                    if (Translate) //FIX Issue # 6, Campo F43 ahora esta traducido
                    {
                        String Dato = ISOmsg.Substring(myPos, myLenght);
                        DE[FieldNo] = TCPUtil.EBDicToString(Dato);
                    }
                    else
                    {
                        DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                    }
                }
            }

            //--------------------------
            FieldNo = 44;

            if (de1Binary.Substring(FieldNo - 1, 1) == "1")
            {
                myPos += myLenght;
                if (ConvertToAscii)
                {
                    String Str = Encoding.ASCII.GetString(TCPUtil.ConvertHexStringToByteArray(ISOmsg.Substring(myPos, GetLength(2))));
                    myLenght = Int32.Parse(Str);

                    if (Translate)
                    {
                        myPos += 4;
                        DE[FieldNo] = Encoding.ASCII.GetString(TCPUtil.ConvertHexStringToByteArray(ISOmsg.Substring(myPos, GetLength(myLenght))));
                        myLenght = myLenght * 2;
                    }
                    else
                    {
                        myLenght += 2;
                        myLenght = myLenght * 2;
                        DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                    }
                }
                else
                {
                    len = 2;
                    myLenght = Int32.Parse(ISOmsg.Substring(myPos, len), System.Globalization.NumberStyles.HexNumber) * 2;
                    if (Translate)
                    {
                        myPos += len;
                    }
                    else
                    {
                        myLenght += 2;
                    }

                    DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                }

            }
            //--------------------------
            FieldNo = 45;

            if (de1Binary.Substring(FieldNo - 1, 1) == "1")
            {
                myPos += myLenght; len = 2;
                myLenght = Convert.ToInt16(ISOmsg.Substring(myPos, len));
                myPos += len;
                DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
            }
            //--------------------------
            FieldNo = 46;

            if (de1Binary.Substring(FieldNo - 1, 1) == "1")
            {
                myPos += myLenght; len = 3;
                myLenght = Convert.ToInt16(ISOmsg.Substring(myPos, len));
                myPos += len;
                DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
            }
            //--------------------------
            FieldNo = 47;

            if (de1Binary.Substring(FieldNo - 1, 1) == "1")
            {
                myPos += myLenght; len = 3;
                myLenght = Convert.ToInt16(ISOmsg.Substring(myPos, len));
                myPos += len;
                DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
            }
            //--------------------------
            FieldNo = 48;

            if (de1Binary.Substring(FieldNo - 1, 1) == "1")
            {
                myPos += myLenght; len = 3;
                myLenght = Convert.ToInt16(ISOmsg.Substring(myPos, len));
                myPos += len;
                DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
            }


            //--------------------------
            FieldNo = 49;

            if (de1Binary.Substring(FieldNo - 1, 1) == "1")
            {
                myPos += myLenght;
                if (ConvertToAscii)
                {
                    myLenght = 6;
                    if (Translate)
                    {
                        DE[FieldNo] = Encoding.ASCII.GetString(TCPUtil.ConvertHexStringToByteArray(ISOmsg.Substring(myPos, myLenght)));
                    }
                    else
                    {
                        DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                    }
                }
                else
                {
                    myLenght = 4;
                    if (Translate)
                    {
                        DE[FieldNo] = ISOmsg.Substring(myPos, myLenght).Remove(0, 1);
                    }
                    else
                    {
                        DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                    }
                }
            }
            //--------------------------
            FieldNo = 50;

            if (de1Binary.Substring(FieldNo - 1, 1) == "1")
            {
                myPos += myLenght;
                myLenght = GetLength(3);
                if (ConvertToAscii)
                {
                    if (Translate)
                    {
                        DE[FieldNo] = Encoding.ASCII.GetString(TCPUtil.ConvertHexStringToByteArray(ISOmsg.Substring(myPos, myLenght)));
                    }
                    else
                    {
                        DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                    }
                }
                else
                {
                    DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                }
            }
            //--------------------------
            FieldNo = 51;

            if (de1Binary.Substring(FieldNo - 1, 1) == "1")
            {
                myPos += myLenght;
                myLenght = GetLength(3);
                if (ConvertToAscii)
                {
                    if (Translate)
                    {
                        DE[FieldNo] = Encoding.ASCII.GetString(TCPUtil.ConvertHexStringToByteArray(ISOmsg.Substring(myPos, myLenght)));
                    }
                    else
                    {
                        DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                    }
                }
                else
                {
                    myLenght = 4;
                    if (Translate)
                    {
                        DE[FieldNo] = ISOmsg.Substring(myPos, myLenght).Remove(0, 1);
                    }
                    else
                    {
                        DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                    }
                }
            }
            //--------------------------
            FieldNo = 52;

            if (de1Binary.Substring(FieldNo - 1, 1) == "1")
            {
                myPos += myLenght;
                if (TCPUtil.DebugMode)
                {
                    myLenght = GetLength(16);
                }
                else
                {
                    myLenght = 16;
                }

                if (ConvertToAscii)
                {
                    if (Translate)
                    {
                        DE[FieldNo] = Encoding.ASCII.GetString(TCPUtil.ConvertHexStringToByteArray(ISOmsg.Substring(myPos, myLenght)));
                    }
                    else
                    {
                        DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                    }
                }
                else
                {
                    DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                }
            }
            //--------------------------
            FieldNo = 53;

            if (de1Binary.Substring(FieldNo - 1, 1) == "1")
            {
                myPos += myLenght;
                myLenght = GetLength(16);
                if (ConvertToAscii)
                {
                    if (Translate)
                    {
                        DE[FieldNo] = Encoding.ASCII.GetString(TCPUtil.ConvertHexStringToByteArray(ISOmsg.Substring(myPos, myLenght)));
                    }
                    else
                    {
                        DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                    }
                }
                else
                {
                    DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                }
            }
            //--------------------------
            FieldNo = 54;

            if (de1Binary.Substring(FieldNo - 1, 1) == "1")
            {
                myPos += myLenght; len = 2;
                if (ConvertToAscii)
                {
                    len = 3;
                    String Str = Encoding.ASCII.GetString(TCPUtil.ConvertHexStringToByteArray(ISOmsg.Substring(myPos, GetLength(3))));
                    myLenght = Int32.Parse(Str) * 2;

                    if (Translate)
                    {
                        myPos += len * 2;
                        DE[FieldNo] = Encoding.ASCII.GetString(TCPUtil.ConvertHexStringToByteArray(ISOmsg.Substring(myPos, myLenght)));
                        //myLenght = myLenght * 2;
                    }
                    else
                    {
                        myLenght += len * 2;
                        //myLenght = myLenght * 2;
                        DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                    }

                }
                else
                {
                    myLenght = Int32.Parse(ISOmsg.Substring(myPos, len), System.Globalization.NumberStyles.HexNumber) * 2;

                    if (Translate)
                    {
                        myPos += len;
                        String Dato = TCPUtil.EBDicToString(ISOmsg.Substring(myPos, myLenght));
                        DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                    }
                    else
                    {
                        myLenght += len;
                        DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                    }
                }
            }
            //--------------------------
            FieldNo = 55;

            if (de1Binary.Substring(FieldNo - 1, 1) == "1")
            {
                myPos += myLenght; len = 2;
                if (ConvertToAscii)
                {
                    String Str = Encoding.ASCII.GetString(TCPUtil.ConvertHexStringToByteArray(ISOmsg.Substring(myPos, GetLength(len))));
                    myLenght = Int32.Parse(Str) * 2;
                    if (myLenght > 0)
                    {
                        if (Translate)
                        {
                            myPos += len * 2;
                            DE[FieldNo] = Encoding.ASCII.GetString(TCPUtil.ConvertHexStringToByteArray(ISOmsg.Substring(myPos, myLenght)));
                            //myLenght = myLenght * 2;
                        }
                        else
                        {
                            myLenght += len * 2;
                            //myLenght = myLenght * 2;
                            DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                        }
                    }
                    else
                    {
                        DE[FieldNo] = "EMPTY FIELD";
                    }
                }
                else
                {
                    myLenght = Int32.Parse(ISOmsg.Substring(myPos, len), System.Globalization.NumberStyles.HexNumber) * 2;
                    if (Translate)
                    {
                        myPos += len;
                    }
                    else
                    {
                        myLenght += 2;
                    }

                    DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                }
            }
            //--------------------------
            FieldNo = 56;

            if (de1Binary.Substring(FieldNo - 1, 1) == "1")
            {
                myPos += myLenght; len = 3;
                myLenght = Convert.ToInt16(ISOmsg.Substring(myPos, len)); myPos += len;
                DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
            }
            //--------------------------
            FieldNo = 57;

            if (de1Binary.Substring(FieldNo - 1, 1) == "1")
            {
                myPos += myLenght; len = 3;
                myLenght = Convert.ToInt16(ISOmsg.Substring(myPos, len)); myPos += len;
                DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
            }
            //--------------------------
            FieldNo = 58;

            if (de1Binary.Substring(FieldNo - 1, 1) == "1")
            {
                myPos += myLenght; len = 3;
                myLenght = Convert.ToInt16(ISOmsg.Substring(myPos, len)); myPos += len;
                DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
            }
            //--------------------------
            FieldNo = 59;

            if (de1Binary.Substring(FieldNo - 1, 1) == "1")
            {
                myPos += myLenght; len = 2;
                myLenght = Int32.Parse(ISOmsg.Substring(myPos, len), System.Globalization.NumberStyles.HexNumber) * 2; myPos += len;
                DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
            }
            //--------------------------
            FieldNo = 60;

            if (de1Binary.Substring(FieldNo - 1, 1) == "1")
            {
                myPos += myLenght;
                if (ConvertToAscii)
                {
                    String Str = Encoding.ASCII.GetString(TCPUtil.ConvertHexStringToByteArray(ISOmsg.Substring(myPos, GetLength(2))));
                    myLenght = Int32.Parse(Str);

                    if (Translate)
                    {
                        myPos += 4;
                        DE[FieldNo] = Encoding.ASCII.GetString(TCPUtil.ConvertHexStringToByteArray(ISOmsg.Substring(myPos, GetLength(myLenght))));
                        myLenght = myLenght * 2;
                    }
                    else
                    {
                        myLenght += 2;
                        myLenght = myLenght * 2;
                        DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                    }
                }
                else
                {
                    len = 2;
                    myLenght = Int32.Parse(ISOmsg.Substring(myPos, len), System.Globalization.NumberStyles.HexNumber) * 2;
                    if (Translate)
                    {
                        myPos += len;
                    }
                    else
                    {
                        myLenght += 2;
                    }

                    DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                }
            }
            //--------------------------
            FieldNo = 61;

            if (de1Binary.Substring(FieldNo - 1, 1) == "1")
            {
                myPos += myLenght; len = 3;
                myLenght = Convert.ToInt16(ISOmsg.Substring(myPos, len)); myPos += len;
                DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
            }
            //--------------------------
            FieldNo = 62;

            if (de1Binary.Substring(FieldNo - 1, 1) == "1")
            {
                myPos += myLenght;
                if (ConvertToAscii)
                {
                    String Str = Encoding.ASCII.GetString(TCPUtil.ConvertHexStringToByteArray(ISOmsg.Substring(myPos, GetLength(2))));
                    myLenght = Int32.Parse(Str);

                    if (Translate)
                    {
                        myPos += 4;
                        DE[FieldNo] = Encoding.ASCII.GetString(TCPUtil.ConvertHexStringToByteArray(ISOmsg.Substring(myPos, GetLength(myLenght))));
                        myLenght = myLenght * 2;
                    }
                    else
                    {
                        myLenght += 2;
                        myLenght = myLenght * 2;
                        DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                    }
                }
                else
                {
                    len = 2;
                    myLenght = Int32.Parse(ISOmsg.Substring(myPos, len), System.Globalization.NumberStyles.HexNumber) * 2;
                    if (Translate)
                    {
                        myPos += len;
                    }
                    else
                    {
                        myLenght += 2;
                    }

                    DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                }
            }
            //--------------------------
            FieldNo = 63;

            if (de1Binary.Substring(FieldNo - 1, 1) == "1")
            {
                myPos += myLenght;
                if (ConvertToAscii)
                {
                    String Str = Encoding.ASCII.GetString(TCPUtil.ConvertHexStringToByteArray(ISOmsg.Substring(myPos, GetLength(2))));
                    myLenght = Int32.Parse(Str);

                    if (Translate)
                    {
                        myPos += 4;
                        DE[FieldNo] = Encoding.ASCII.GetString(TCPUtil.ConvertHexStringToByteArray(ISOmsg.Substring(myPos, GetLength(myLenght))));
                        myLenght = myLenght * 2;
                    }
                    else
                    {
                        myLenght += 2;
                        myLenght = myLenght * 2;
                        DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                    }
                }
                else
                {
                    len = 2;
                    myLenght = Int32.Parse(ISOmsg.Substring(myPos, len), System.Globalization.NumberStyles.HexNumber) * 2;
                    if (Translate)
                    {
                        myPos += len;
                    }
                    else
                    {
                        myLenght += 2;
                    }

                    DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                }
            }

            //--------------------------
            FieldNo = 64;

            if (de1Binary.Substring(FieldNo - 1, 1) == "1") { myPos += myLenght; myLenght = 4; DE[FieldNo] = ISOmsg.Substring(myPos, myLenght); }

            /*
            for (int I = 0; I <= 64; I++)
            {
                if (BM[I] != null)

                { Console.WriteLine("BM" + I + ": " + BM[I] + " = " + BMname[I]); }
            }
            */

            //===CHECK POINT FOR BM# 65 to 128============
            if (de2Binary == "") return DE; ///<----IF DATA ELEMENT <= 64
            if (de2Binary == "0000000000000000000000000000000000000000000000000000000000000000") return DE;
            //============================================

            //--------------------------
            int comp = 64;
            FieldNo = 65;

            if (de2Binary.Substring(FieldNo - comp - 1, 1) == "1") { myPos += myLenght; myLenght = 16; DE[FieldNo] = ISOmsg.Substring(myPos, myLenght); }
            FieldNo = 66;

            if (de2Binary.Substring(FieldNo - comp - 1, 1) == "1") { myPos += myLenght; myLenght = 1; DE[FieldNo] = ISOmsg.Substring(myPos, myLenght); }
            FieldNo = 67;

            if (de2Binary.Substring(FieldNo - comp - 1, 1) == "1") { myPos += myLenght; myLenght = 2; DE[FieldNo] = ISOmsg.Substring(myPos, myLenght); }
            FieldNo = 68;

            if (de2Binary.Substring(FieldNo - comp - 1, 1) == "1") { myPos += myLenght; myLenght = 3; DE[FieldNo] = ISOmsg.Substring(myPos, myLenght); }
            FieldNo = 69;

            if (de2Binary.Substring(FieldNo - comp - 1, 1) == "1") { myPos += myLenght; myLenght = 3; DE[FieldNo] = ISOmsg.Substring(myPos, myLenght); }
            FieldNo = 70;

            if (de2Binary.Substring(FieldNo - comp - 1, 1) == "1")
            {
                myPos += myLenght;

                if (ConvertToAscii)
                {
                    myLenght = GetLength(3);
                    if (Translate)
                    {
                        DE[FieldNo] = Encoding.ASCII.GetString(TCPUtil.ConvertHexStringToByteArray(ISOmsg.Substring(myPos, myLenght)));

                    }
                    else
                    {
                        DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                    }
                }
                else
                {
                    myLenght = GetLength(4);
                    DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                }
            }
            FieldNo = 71;

            if (de2Binary.Substring(FieldNo - comp - 1, 1) == "1") { myPos += myLenght; myLenght = 4; DE[FieldNo] = ISOmsg.Substring(myPos, myLenght); }
            //-------------------------------------------------
            FieldNo = 72;

            if (de2Binary.Substring(FieldNo - comp - 1, 1) == "1")
            {
                myPos += myLenght; len = 3;
                myLenght = Convert.ToInt16(ISOmsg.Substring(myPos, len)); myPos += len;
                DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
            }
            //--------------------------
            FieldNo = 73;

            if (de2Binary.Substring(FieldNo - comp - 1, 1) == "1") { myPos += myLenght; myLenght = 6; DE[FieldNo] = ISOmsg.Substring(myPos, myLenght); }
            //--------------------------
            FieldNo = 74;

            if (de2Binary.Substring(FieldNo - comp - 1, 1) == "1") { myPos += myLenght; myLenght = 10; DE[FieldNo] = ISOmsg.Substring(myPos, myLenght); }
            FieldNo = 75;

            if (de2Binary.Substring(FieldNo - comp - 1, 1) == "1") { myPos += myLenght; myLenght = 10; DE[FieldNo] = ISOmsg.Substring(myPos, myLenght); }
            FieldNo = 76;

            if (de2Binary.Substring(FieldNo - comp - 1, 1) == "1") { myPos += myLenght; myLenght = 10; DE[FieldNo] = ISOmsg.Substring(myPos, myLenght); }
            FieldNo = 77;

            if (de2Binary.Substring(FieldNo - comp - 1, 1) == "1") { myPos += myLenght; myLenght = 10; DE[FieldNo] = ISOmsg.Substring(myPos, myLenght); }
            FieldNo = 78;

            if (de2Binary.Substring(FieldNo - comp - 1, 1) == "1") { myPos += myLenght; myLenght = 10; DE[FieldNo] = ISOmsg.Substring(myPos, myLenght); }
            FieldNo = 79;

            if (de2Binary.Substring(FieldNo - comp - 1, 1) == "1") { myPos += myLenght; myLenght = 10; DE[FieldNo] = ISOmsg.Substring(myPos, myLenght); }
            FieldNo = 80;

            if (de2Binary.Substring(FieldNo - comp - 1, 1) == "1") { myPos += myLenght; myLenght = 10; DE[FieldNo] = ISOmsg.Substring(myPos, myLenght); }
            FieldNo = 81;

            if (de2Binary.Substring(FieldNo - comp - 1, 1) == "1") { myPos += myLenght; myLenght = 10; DE[FieldNo] = ISOmsg.Substring(myPos, myLenght); }
            //-----------------------------
            FieldNo = 82;

            if (de2Binary.Substring(FieldNo - comp - 1, 1) == "1") { myPos += myLenght; myLenght = 12; DE[FieldNo] = ISOmsg.Substring(myPos, myLenght); }
            FieldNo = 83;

            if (de2Binary.Substring(FieldNo - comp - 1, 1) == "1") { myPos += myLenght; myLenght = 12; DE[FieldNo] = ISOmsg.Substring(myPos, myLenght); }
            FieldNo = 84;

            if (de2Binary.Substring(FieldNo - comp - 1, 1) == "1") { myPos += myLenght; myLenght = 12; DE[FieldNo] = ISOmsg.Substring(myPos, myLenght); }
            FieldNo = 85;

            if (de2Binary.Substring(FieldNo - comp - 1, 1) == "1") { myPos += myLenght; myLenght = 12; DE[FieldNo] = ISOmsg.Substring(myPos, myLenght); }
            //------------------------------
            FieldNo = 86;

            if (de2Binary.Substring(FieldNo - comp - 1, 1) == "1") { myPos += myLenght; myLenght = 15; DE[FieldNo] = ISOmsg.Substring(myPos, myLenght); }
            FieldNo = 87;

            if (de2Binary.Substring(FieldNo - comp - 1, 1) == "1") { myPos += myLenght; myLenght = 15; DE[FieldNo] = ISOmsg.Substring(myPos, myLenght); }
            FieldNo = 88;

            if (de2Binary.Substring(FieldNo - comp - 1, 1) == "1") { myPos += myLenght; myLenght = 15; DE[FieldNo] = ISOmsg.Substring(myPos, myLenght); }
            FieldNo = 89;

            if (de2Binary.Substring(FieldNo - comp - 1, 1) == "1") { myPos += myLenght; myLenght = 15; DE[FieldNo] = ISOmsg.Substring(myPos, myLenght); }
            FieldNo = 90;

            if (de2Binary.Substring(FieldNo - comp - 1, 1) == "1")
            {
                myPos += myLenght;
                myLenght = GetLength(42);
                if (ConvertToAscii)
                {
                    if (Translate)
                    {
                        DE[FieldNo] = Encoding.ASCII.GetString(TCPUtil.ConvertHexStringToByteArray(ISOmsg.Substring(myPos, myLenght)));
                    }
                    else
                    {
                        DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                    }
                }
                else
                {
                    DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                }
            }
            //------------------------------
            FieldNo = 91;

            if (de2Binary.Substring(FieldNo - comp - 1, 1) == "1") { myPos += myLenght; myLenght = 1; DE[FieldNo] = ISOmsg.Substring(myPos, myLenght); }
            //------------------------------
            FieldNo = 92;

            if (de2Binary.Substring(FieldNo - comp - 1, 1) == "1") { myPos += myLenght; myLenght = 2; DE[FieldNo] = ISOmsg.Substring(myPos, myLenght); }
            //------------------------------
            FieldNo = 93;

            if (de2Binary.Substring(FieldNo - comp - 1, 1) == "1") { myPos += myLenght; myLenght = 5; DE[FieldNo] = ISOmsg.Substring(myPos, myLenght); }
            //------------------------------
            FieldNo = 94;

            if (de2Binary.Substring(FieldNo - comp - 1, 1) == "1") { myPos += myLenght; myLenght = 7; DE[FieldNo] = ISOmsg.Substring(myPos, myLenght); }
            //------------------------------
            FieldNo = 95;

            if (de2Binary.Substring(FieldNo - comp - 1, 1) == "1")
            {

                myPos += myLenght;
                myLenght = GetLength(42);
                if (ConvertToAscii)
                {
                    if (Translate)
                    {
                        DE[FieldNo] = Encoding.ASCII.GetString(TCPUtil.ConvertHexStringToByteArray(ISOmsg.Substring(myPos, myLenght)));
                    }
                    else
                    {
                        DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                    }
                }
                else
                {
                    DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                }
            }
            //------------------------------
            FieldNo = 96;

            if (de2Binary.Substring(FieldNo - comp - 1, 1) == "1") { myPos += myLenght; myLenght = 8; DE[FieldNo] = ISOmsg.Substring(myPos, myLenght); }
            //------------------------------
            FieldNo = 97;

            if (de2Binary.Substring(FieldNo - comp - 1, 1) == "1") { myPos += myLenght; myLenght = 16; DE[FieldNo] = ISOmsg.Substring(myPos, myLenght); }
            //------------------------------
            FieldNo = 98;

            if (de2Binary.Substring(FieldNo - comp - 1, 1) == "1") { myPos += myLenght; myLenght = 25; DE[FieldNo] = ISOmsg.Substring(myPos, myLenght); }
            //-------------------------------------------------
            FieldNo = 99;

            if (de2Binary.Substring(FieldNo - comp - 1, 1) == "1")
            {
                myPos += myLenght; len = 2;
                myLenght = Convert.ToInt16(ISOmsg.Substring(myPos, len)); myPos += len;
                DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
            }
            FieldNo = 100;

            if (de2Binary.Substring(FieldNo - comp - 1, 1) == "1")
            {
                myPos += myLenght;
                if (ConvertToAscii)
                {
                    String Str = Encoding.ASCII.GetString(TCPUtil.ConvertHexStringToByteArray(ISOmsg.Substring(myPos, GetLength(2))));
                    myLenght = Int32.Parse(Str);

                    if (Translate)
                    {
                        myPos += 4;
                        DE[FieldNo] = Encoding.ASCII.GetString(TCPUtil.ConvertHexStringToByteArray(ISOmsg.Substring(myPos, GetLength(myLenght))));
                        myLenght = myLenght * 2;
                    }
                    else
                    {
                        myLenght += 2;
                        myLenght = myLenght * 2;
                        DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                    }
                }
                else
                {
                    len = 2;
                    myLenght = Convert.ToInt16(ISOmsg.Substring(myPos, len)); myPos += len;
                    DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                }
            }
            //------------------------------
            FieldNo = 101;

            if (de2Binary.Substring(FieldNo - comp - 1, 1) == "1") { myPos += myLenght; myLenght = 17; DE[FieldNo] = ISOmsg.Substring(myPos, myLenght); }

            //-------------------------------------------------
            FieldNo = 102;

            if (de2Binary.Substring(FieldNo - comp - 1, 1) == "1")
            {
                myPos += myLenght;
                if (ConvertToAscii)
                {
                    myLenght = Int32.Parse(Encoding.ASCII.GetString(TCPUtil.ConvertHexStringToByteArray(ISOmsg.Substring(myPos, GetLength(2)))));
                    if (Translate)
                    {
                        myPos += 4;
                        DE[FieldNo] = Encoding.ASCII.GetString(TCPUtil.ConvertHexStringToByteArray(ISOmsg.Substring(myPos, GetLength(myLenght))));
                        myLenght = myLenght * 2;
                    }
                    else
                    {
                        myLenght += 2;
                        myLenght = myLenght * 2;
                        DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                    }
                }
                else
                {
                    String L = ISOmsg.Substring(myPos, 2);
                    Int32 Lgt = Int32.Parse(L, System.Globalization.NumberStyles.HexNumber);

                    myLenght = Lgt * 2;
                    if (Translate)
                    {
                        myPos += 2;
                        DE[FieldNo] = TCPUtil.EBDicToString(ISOmsg.Substring(myPos, myLenght));
                    }
                    else
                    {
                        myLenght += 2;
                        DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                    }

                }
            }
            FieldNo = 103;

            if (de2Binary.Substring(FieldNo - comp - 1, 1) == "1")
            {
                myPos += myLenght;
                if (ConvertToAscii)
                {
                    myLenght = Int32.Parse(Encoding.ASCII.GetString(TCPUtil.ConvertHexStringToByteArray(ISOmsg.Substring(myPos, GetLength(2)))));
                    if (Translate)
                    {
                        myPos += 4;
                        DE[FieldNo] = Encoding.ASCII.GetString(TCPUtil.ConvertHexStringToByteArray(ISOmsg.Substring(myPos, GetLength(myLenght))));
                        myLenght = myLenght * 2;
                    }
                    else
                    {
                        myLenght += 2;
                        myLenght = myLenght * 2;
                        DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                    }
                }
                else
                {
                    myLenght = Int32.Parse(ISOmsg.Substring(myPos, 2), System.Globalization.NumberStyles.HexNumber);
                    if (Translate)
                    {
                        myPos += 2;
                    }
                    else
                    {
                        myLenght += 2;
                    }
                    DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                }
            }
            //----------------------------------
            FieldNo = 104;

            if (de2Binary.Substring(FieldNo - comp - 1, 1) == "1")
            {
                myPos += myLenght; len = 3;
                myLenght = Convert.ToInt16(ISOmsg.Substring(myPos, len)); myPos += len;
                DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
            }
            FieldNo = 105;

            if (de2Binary.Substring(FieldNo - comp - 1, 1) == "1")
            {
                myPos += myLenght; len = 3;
                myLenght = Convert.ToInt16(ISOmsg.Substring(myPos, len)); myPos += len;
                DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
            }
            FieldNo = 106;

            if (de2Binary.Substring(FieldNo - comp - 1, 1) == "1")
            {
                myPos += myLenght; len = 3;
                myLenght = Convert.ToInt16(ISOmsg.Substring(myPos, len)); myPos += len;
                DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
            }
            FieldNo = 107;

            if (de2Binary.Substring(FieldNo - comp - 1, 1) == "1")
            {
                myPos += myLenght; len = 3;
                myLenght = Convert.ToInt16(ISOmsg.Substring(myPos, len)); myPos += len;
                DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
            }
            FieldNo = 108;

            if (de2Binary.Substring(FieldNo - comp - 1, 1) == "1")
            {
                myPos += myLenght; len = 3;
                myLenght = Convert.ToInt16(ISOmsg.Substring(myPos, len)); myPos += len;
                DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
            }
            FieldNo = 109;

            if (de2Binary.Substring(FieldNo - comp - 1, 1) == "1")
            {
                myPos += myLenght; len = 3;
                myLenght = Convert.ToInt16(ISOmsg.Substring(myPos, len)); myPos += len;
                DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
            }
            FieldNo = 110;

            if (de2Binary.Substring(FieldNo - comp - 1, 1) == "1")
            {
                myPos += myLenght; len = 3;
                myLenght = Convert.ToInt16(ISOmsg.Substring(myPos, len)); myPos += len;
                DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
            }
            FieldNo = 111;

            if (de2Binary.Substring(FieldNo - comp - 1, 1) == "1")
            {
                myPos += myLenght; len = 3;
                myLenght = Convert.ToInt16(ISOmsg.Substring(myPos, len)); myPos += len;
                DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
            }
            FieldNo = 112;

            if (de2Binary.Substring(FieldNo - comp - 1, 1) == "1")
            {
                myPos += myLenght; len = 3;
                myLenght = Convert.ToInt16(ISOmsg.Substring(myPos, len)); myPos += len;
                DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
            }
            //----------------------------
            FieldNo = 113;

            if (de2Binary.Substring(FieldNo - comp - 1, 1) == "1")
            {
                myPos += myLenght; len = 2;
                myLenght = Convert.ToInt16(ISOmsg.Substring(myPos, len)); myPos += len;
                DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
            }
            //-------------------------
            FieldNo = 114;

            if (de2Binary.Substring(FieldNo - comp - 1, 1) == "1")
            {
                myPos += myLenght; len = 3;
                myLenght = Convert.ToInt16(ISOmsg.Substring(myPos, len)); myPos += len;
                DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
            }
            FieldNo = 115;

            if (de2Binary.Substring(FieldNo - comp - 1, 1) == "1")
            {
                myPos += myLenght; len = 3;
                myLenght = Convert.ToInt16(ISOmsg.Substring(myPos, len)); myPos += len;
                DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
            }
            FieldNo = 116;

            if (de2Binary.Substring(FieldNo - comp - 1, 1) == "1")
            {
                myPos += myLenght; len = 3;
                myLenght = Convert.ToInt16(ISOmsg.Substring(myPos, len)); myPos += len;
                DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
            }
            FieldNo = 117;

            if (de2Binary.Substring(FieldNo - comp - 1, 1) == "1")
            {
                myPos += myLenght; len = 3;
                myLenght = Convert.ToInt16(ISOmsg.Substring(myPos, len)); myPos += len;
                DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
            }
            FieldNo = 118;

            if (de2Binary.Substring(FieldNo - comp - 1, 1) == "1")
            {
                myPos += myLenght; len = 3;
                myLenght = Convert.ToInt16(ISOmsg.Substring(myPos, len)); myPos += len;
                DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
            }
            FieldNo = 119;

            if (de2Binary.Substring(FieldNo - comp - 1, 1) == "1")
            {
                myPos += myLenght;
                len = 2;
                myLenght = Int32.Parse(ISOmsg.Substring(myPos, len), System.Globalization.NumberStyles.HexNumber) * 2; myPos += len;
                DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
            }
            FieldNo = 120;

            if (de2Binary.Substring(FieldNo - comp - 1, 1) == "1")
            {
                myPos += myLenght; len = 3;
                myLenght = Convert.ToInt16(ISOmsg.Substring(myPos, len)); myPos += len;
                DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
            }
            FieldNo = 121;

            if (de2Binary.Substring(FieldNo - comp - 1, 1) == "1")
            {
                myPos += myLenght; len = 3;
                if (ConvertToAscii)
                {
                    //len = 2;
                    String Str = Encoding.ASCII.GetString(TCPUtil.ConvertHexStringToByteArray(ISOmsg.Substring(myPos, GetLength(len))));
                    myLenght = Int32.Parse(Str) * 2;

                    if (Translate)
                    {
                        myPos += len * 2;
                        DE[FieldNo] = Encoding.ASCII.GetString(TCPUtil.ConvertHexStringToByteArray(ISOmsg.Substring(myPos, myLenght)));
                        //myLenght = myLenght * 2;
                    }
                    else
                    {
                        myLenght += len * 2;
                        //myLenght = myLenght * 2;
                        DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                    }

                }
                else
                {
                    myLenght = Convert.ToInt16(ISOmsg.Substring(myPos, len)); myPos += len;
                    DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                }
            }
            FieldNo = 122;

            if (de2Binary.Substring(FieldNo - comp - 1, 1) == "1")
            {
                myPos += myLenght; len = 3;
                myLenght = Convert.ToInt16(ISOmsg.Substring(myPos, len)); myPos += len;
                DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
            }
            FieldNo = 123;

            if (de2Binary.Substring(FieldNo - comp - 1, 1) == "1")
            {
                myPos += myLenght; len = 3;
                myLenght = Convert.ToInt16(ISOmsg.Substring(myPos, len)); myPos += len;
                DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
            }

            FieldNo = 124;

            if (de2Binary.Substring(FieldNo - comp - 1, 1) == "1")
            {
                myPos += myLenght; len = 3;
                myLenght = Convert.ToInt16(ISOmsg.Substring(myPos, len)); myPos += len;
                DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
            }
            //---------------------------------
            FieldNo = 125;

            if (de2Binary.Substring(FieldNo - comp - 1, 1) == "1")
            {
                myPos += myLenght; len = 2;
                myLenght = Convert.ToInt16(ISOmsg.Substring(myPos, len)); myPos += len;
                DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
            }
            //----------------------------------
            FieldNo = 126;

            if (de2Binary.Substring(FieldNo - comp - 1, 1) == "1")
            {
                myPos += myLenght;
                if (ConvertToAscii)
                {
                    myLenght = Int32.Parse(Encoding.ASCII.GetString(TCPUtil.ConvertHexStringToByteArray(ISOmsg.Substring(myPos, GetLength(2)))));
                    if (Translate)
                    {
                        myPos += 4;
                        DE[FieldNo] = Encoding.ASCII.GetString(TCPUtil.ConvertHexStringToByteArray(ISOmsg.Substring(myPos, GetLength(myLenght))));
                        myLenght = myLenght * 2;
                    }
                    else
                    {
                        myLenght += 2;
                        myLenght = myLenght * 2;
                        DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
                    }
                }
                else
                {
                    String L = ISOmsg.Substring(myPos, 2);
                    myLenght = Int32.Parse(L, System.Globalization.NumberStyles.HexNumber);
                    if (Translate)
                    {
                        myPos += 2;
                        myLenght += 2;
                        String Field126 = "";
                        String BMP = ISOmsg.Substring(myPos, myLenght);
                        String BMPHEX = DEtoBinary(BMP);
                        myPos += myLenght;
                        Int32 Position = myPos;
                        for (int i = 0; i < BMPHEX.Length; i++)
                        {
                            if (BMPHEX.Substring(i, 1) == "1")
                            {
                                myLenght += 12;
                                Field126 += TCPUtil.EBDicToString(ISOmsg.Substring(Position, 12));
                                Position += 12;
                            }
                        }
                        DE[FieldNo] = BMP + Field126;
                    }
                    else
                    {
                        myLenght += 2;
                        String Field126 = "";
                        myPos += 2;
                        String BMP = ISOmsg.Substring(myPos, myLenght);
                        String BMPHEX = DEtoBinary(BMP);

                        myPos += myLenght;
                        Int32 Position = myPos;
                        for (int i = 0; i < BMPHEX.Length; i++)
                        {
                            if (BMPHEX.Substring(i, 1) == "1")
                            {
                                myLenght += 12;
                                Field126 += ISOmsg.Substring(Position, 12);
                                Position += 12;
                            }
                        }

                        DE[FieldNo] = L + BMP + Field126;
                    }
                }
            }
            //-----------------------------------
            FieldNo = 127;

            if (de2Binary.Substring(FieldNo - comp - 1, 1) == "1")
            {
                myPos += myLenght; len = 3;
                myLenght = Convert.ToInt16(ISOmsg.Substring(myPos, len)); myPos += len;
                DE[FieldNo] = ISOmsg.Substring(myPos, myLenght);
            }
            //------------------------------
            FieldNo = 128;

            if (de2Binary.Substring(FieldNo - comp - 1, 1) == "1") { myPos += myLenght; myLenght = 4; DE[FieldNo] = ISOmsg.Substring(myPos, myLenght); }
            //-------------------------------------------------



            //==========================================
            return DE;
            //==========================================




        }
        //==========================================================
        private string DEtoBinary(string HexDE)
        {
            string deBinary = "";
            for (int I = 0; I <= 15; I++)
            {
                deBinary = deBinary + Hex2Binary(HexDE.Substring(I, 1));

            }

            return deBinary;

        }
        private string Hex2Binary(string DE)
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
    }
}
