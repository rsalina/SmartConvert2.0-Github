using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TCPSmart.Flow;

namespace TCPSmart
{
    public partial class VisorISO8583 : Form
    {
        private ConcurrentDictionary<String, DateTime> ConexionesBMP = new ConcurrentDictionary<String, DateTime>();
        DataTable Xet;
        Int32 ConterCols = 1;     
        public VisorISO8583()
        {
            InitializeComponent();
        }

        private void VisorISO8583_Load(object sender, EventArgs e)
        {
            SchemaInit();
        }
        private void SchemaInit()
        {
            Xet = new DataTable();
            Xet.Columns.Add("Campo", typeof(String));
            Xet.Columns.Add("Parsed#" + ConterCols, typeof(String));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Int32 TypeMsj = 0;
            String FormatedHEX = "";
            String HEXResponse = "";
            String NHeader = "";
            String NHeaderRes = "";

            if (Xet.Rows.Count > 0 && !checkEdit2.Checked)
            {
                checkEdit2.Checked = true;
            }
            if (TxtHex.Text == "")
            {
                MessageBox.Show("La cadena HEX no pueder ir en blanco", TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (TxtHex.Text.Contains("-") || TxtHex.Text.Contains(" "))
            {
                MessageBox.Show("Recuerda que la cadena debe sin guiones o espacios", TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (CmbTypeChain.Text == "Seleccione..")
            {
                MessageBox.Show("Debes de seleccionar un tipo de cadenar", TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                switch (CmbTypeChain.Text)
                {
                    case "1 BASEi 2 BYTES":
                        TypeMsj = 1;
                        NHeader = TxtHex.Text.Substring(0, 4);

                        FormatedHEX = TCPUtil.PrepareISO8583FromHEX(TxtHex.Text, 4, 1, out Int32 R);

                        break;
                    case "1 BASEi 4 BYTES":
                        TypeMsj = 1;
                        NHeader = TxtHex.Text.Substring(0, 8);

                        FormatedHEX = TCPUtil.PrepareISO8583FromHEX(TxtHex.Text, 8, 1, out Int32 R2);
                        break;
                    case "3 EWA":
                        TypeMsj = 3;
                        FormatedHEX = TCPUtil.PrepareISO8583FromHEX(TxtHex.Text, 4, 3, out Int32 R3);
                        NHeader = TxtHex.Text.Substring(0, 4);
                        break;
                    case "4 BERBETEC":
                        TypeMsj = 4;
                        FormatedHEX = TCPUtil.PrepareISO8583FromHEX(TxtHex.Text, 8, 4, out Int32 R4);
                        NHeader = TxtHex.Text.Substring(0, 8);
                        break;
                }
            }

            checkEdit1.Enabled = false;
            if (checkEdit3.Checked)
            {
                if (TxtF39.Text == "")
                {
                    MessageBox.Show("Falta capturar el codigo de respuesta del F39", TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                checkEdit2.Checked = false;
                checkEdit2.Enabled = false;
                TxtF39.Enabled = false;
            }

            if (checkEdit2.Checked)
            {

                AdjuntarSchema();
                gridView1.DataSource = Xet;
                //gridView1.PopulateColumns();

            }
            else if (checkEdit1.Checked)
            {
                Xet.Columns.Add("Traducido#" + ConterCols, typeof(String));
                gridView1.DataSource = Xet;
                //gridView1.PopulateColumns();
            }

            if (checkEdit3.Checked)
            {
                Xet.Columns.Add("Parsed#" + (ConterCols + 1), typeof(String));
                if (checkEdit1.Checked)
                {
                    Xet.Columns.Add("Traducido#" + (ConterCols + 1), typeof(String));
                }
                gridView1.DataSource = Xet;
                //gridView1.PopulateColumns();
            }


            try
            {
                ISO8583 IsoHelper = new ISO8583(TypeMsj);

                String[] ISOParsed = IsoHelper.Parse(FormatedHEX, false);
                String[] ISOTransl = IsoHelper.Parse(FormatedHEX, true);
                String[] ISOResParsed = null; String[] ISOResTransl = null;
                AddRow(ConterCols, "MTI", TCPUtil.StringToHEXUno(ISOParsed[129]), ISOTransl[129]);
                if (checkEdit3.Checked)
                {
                    String MResponse = "0" + (Convert.ToInt32(ISOParsed[129]) + 10).ToString();
                    HEXResponse = TCPUtil.GenResponse(ISOParsed, ISOTransl, MResponse, TypeMsj, TxtF39.Text, new Random().Next(111111, 999999).ToString());
                    ISOResParsed = IsoHelper.Parse(HEXResponse, false);
                    ISOResTransl = IsoHelper.Parse(HEXResponse, true);
                    NHeaderRes = (HEXResponse.Length / 2).ToString("X4");// HEXResponse.Substring(0, 4);
                    simpleButton1.Enabled = false;
                    AddRow((ConterCols + 1), "MTI", TCPUtil.StringToHEXUno(ISOResParsed[129]), ISOResTransl[129]);
                }

                //Int32 Rows = 0;
                for (int i = 0; i < ISOParsed.Length; i++)
                {
                    if (i != 129)
                    {
                        if (ISOParsed[i] != null)
                        {
                            if (i == 0)
                            {
                                AddRow(ConterCols, "BMP", ISOParsed[i], ISOTransl[i]);
                            }
                            else if (i == 1)
                            {
                                AddRow(ConterCols, "BMP2", ISOParsed[i], ISOTransl[i]);
                            }
                            else
                            {
                                AddRow(ConterCols, "F" + i, ISOParsed[i], ISOTransl[i]);
                            }
                        }
                        if (checkEdit3.Checked)
                        {
                            if (ISOResParsed[i] != null)
                            {
                                if (i == 0)
                                {
                                    AddRow((ConterCols + 1), "BMP", ISOResParsed[i], ISOResTransl[i]);
                                }
                                else if (i == 1)
                                {
                                    AddRow((ConterCols + 1), "BMP2", ISOResParsed[i], ISOResTransl[i]);
                                }
                                else
                                {
                                    AddRow((ConterCols + 1), "F" + i, ISOResParsed[i], ISOResTransl[i]);
                                }
                            }
                        }

                    }
                }

                AddRow(ConterCols, "NetWorkHeader", NHeader, Int32.Parse(NHeader, System.Globalization.NumberStyles.HexNumber).ToString());
                if (checkEdit3.Checked)
                    AddRow((ConterCols + 1), "NetWorkHeader", NHeaderRes, Int32.Parse(NHeaderRes, System.Globalization.NumberStyles.HexNumber).ToString());

                TxtHex.Text = "";
                checkEdit2.Checked = false;


            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en procesado: " + Environment.NewLine + ex.Message, TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void AdjuntarSchema()
        {
            ConterCols++;
            Xet.Columns.Add("Parsed#" + ConterCols, typeof(String));
            if (checkEdit1.Checked)
            {
                Xet.Columns.Add("Traducido#" + ConterCols, typeof(String));
            }
        }

        private void AddRow(Int32 Col, String Campo, String Parseado, String Traducido)
        {
            DataRow nRow = Xet.NewRow();
            if (Col == 1)
            {
                nRow["Campo"] = Campo;
                nRow["Parsed#" + Col] = Parseado;
                if (checkEdit1.Checked)
                {
                    nRow["Traducido#" + Col] = Traducido;
                }
                Xet.Rows.Add(nRow);
            }
            else
            {
                if (checkEdit2.Checked || checkEdit3.Checked)
                {
                    Boolean AddRo = false;
                    if (Xet.Select("Campo = '" + Campo + "'").Count() == 0)
                    {
                        nRow = Xet.NewRow();
                        nRow["Campo"] = Campo;
                        AddRo = true;
                    }
                    else
                    {
                        nRow = Xet.Select("Campo = '" + Campo + "'")[0];
                    }

                    nRow["Parsed#" + Col] = Parseado;
                    if (checkEdit1.Checked)
                    {
                        nRow["Traducido#" + Col] = Traducido;
                    }
                    if (AddRo)
                    {
                        Xet.Rows.Add(nRow);
                    }
                }
            }

            gridView1.DataSource = Xet;
            gridView1.Refresh();
            Application.DoEvents();
        }

        private void checkEdit1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkEdit3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkEdit3.Checked)
            {
                checkEdit2.Checked = false;
                checkEdit2.Enabled = false;
                LstLog.Visible = true;
            }
            else
            {
                checkEdit2.Checked = true;
                checkEdit2.Enabled = true;
                LstLog.Visible = false;
            }
        }
        private void SendLog(String Msj)
        {
            WriteSafetyLog(Msj);

            Application.DoEvents();
        }
        private void WriteSafetyLog(String Msj)
        {
            if (LstLog.InvokeRequired)
            {
                if (Msj == "***")
                {
                    LstLog.Invoke(new Action(() => LstLog.Items.Clear()));
                }
                else
                {
                    LstLog.Invoke(new Action(() => LstLog.Items.Add(Msj)));
                }
                // var d = new SafeWriteLog(WriteSafetyLog);

                LstLog.Invoke(new Action(() => LstLog.SelectedIndex = LstLog.Items.Count - 1));
            }
            else
            {
                if (Msj == "***")
                {
                    LstLog.Items.Clear();
                }
                else
                {
                    LstLog.Items.Add(Msj);
                }
            }
        }

        private CancellationTokenSource _TokenBMPSource = new CancellationTokenSource();
        private CancellationToken _TokenBMP;
        /// <summary>
        /// Monitor para detectar si una conexion entro en tiempo.
        /// </summary>
        /// <returns></returns>
        private async Task MonitorActiveBMPConexion()
        {
            while (!_TokenBMP.IsCancellationRequested)
            {
                if (TCPUtil.ParamBMPTimeOut > 0 && ConexionesBMP.Count > 0)
                {
                    MonitorActiveBMPConexionTask();
                }
                await Task.Delay(5000, _TokenBMP);
            }
        }

        private void MonitorActiveBMPConexionTask()
        {
            DateTime idleTimestamp = DateTime.Now.AddSeconds(-1 * TCPUtil.ParamBMPTimeOut);

            foreach (KeyValuePair<String, DateTime> curr in ConexionesBMP)
            {
                Int32 _IdleTimeOut = Convert.ToInt32(curr.Key.Split('|')[1]);

                if (curr.Value < idleTimestamp)
                {
                    DateTime Outer = DateTime.Now;
                    SendLog("Cerrando conexion BMP no se recibio respuesta desde IP- " + curr.Key);
                    ConexionesBMP.TryRemove(curr.Key, out Outer);
                }
            }
        }
    }
}
