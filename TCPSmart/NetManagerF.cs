using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace TCPSmart
{
    public partial class NetManagerF : Form
    {
        private Int32 IdNetMan;
        private Int32 IdConMaster;
        private Boolean Edicion;
        public NetManagerF(int IdNet)
        {
            InitializeComponent();
            IdNetMan = IdNet;
            LoadData();
        }

        private void LoadData()
        {
            String Qry = "";
            if (IdNetMan > 0)
            {
                Edicion = true;
                Qry = "SELECT N.DescriptionName,N.ValidatetokenF52,N.ValidateFeeF28,N.ValidateCodeF3,N.ValidateTimeOut,C.TimeOutSecs,C.Port AS porto,C.BytesHeader AS HeaderByte, C.Id AS IdCn, C.TypeMsg, C.EndConnectionMasterId FROM NetManager N LEFT JOIN ConnectionMaster C ON N.ConnectionMasterId = C.Id WHERE N.Id = " + IdNetMan;
                DataTable Xet = new DataTable();
                SqlConnection cn = new SqlConnection(DBUtil.GetAppConnectionString());

                SqlDataAdapter c = new SqlDataAdapter(Qry, cn);
                c.Fill(Xet);

                foreach (DataRow item in Xet.Rows)
                {
                    TxtName.Text = item["DescriptionName"].ToString();
                    TxtPort.Value = Convert.ToInt32(item["porto"]);
                    CmbTCadena.Text = TCPUtil.GetTypeChainStr(Convert.ToInt32(item["TypeMsg"]));
                    CmbHByte.Text = item["HeaderByte"].ToString();
                    IdConMaster = Convert.ToInt32(item["IdCn"]);
                    ValTimeOut.Checked = Convert.ToBoolean(item["ValidateTimeOut"]);
                    TxtTimeOut.Value = Convert.ToInt32(item["TimeOutSecs"]);
                }

            }
        }
        private void NetManagerF_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (TxtName.Text == "")
            {
                MessageBox.Show("Debes escribir un nombre para la NetManager", TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (TCPUtil.ReservedPort(Convert.ToInt32(TxtPort.Value)))
            {
                MessageBox.Show("El puerto " + Convert.ToInt32(TxtPort.Value) + " al parecer esta reservado para la operacion interna del sistema operativo", TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (CmbTCadena.Text == "")
            {
                MessageBox.Show("Debes seleccionar el tipo de cadena", TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (CmbHByte.Text == "")
            {
                MessageBox.Show("Debes seleccionar el Header de la conexion", TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (Convert.ToInt32(TxtTimeOut.Value) == 0 && ValTimeOut.Checked)
            {
                MessageBox.Show("El TimeOut no puede ser 0 si lo requiere validar", TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (Edicion)
            {
                if (DBUtil.GetSQL("SELECT * FROM NetManager WHERE DescriptionName = '" + TxtName.Text + "' AND Id <> " + IdNetMan).Rows.Count > 0)
                {
                    MessageBox.Show("Ya existe una NetManager con el nombre de " + TxtName.Text + " no se puede sobreescribir", TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (DBUtil.GetSQL("SELECT * FROM ConnectionMaster WHERE Port = " + Convert.ToInt32(TxtPort.Value) + " AND Id <> " + IdConMaster).Rows.Count > 0)
                {
                    MessageBox.Show("No es posible usar este puerto, ya existe una conexion que lo usa", TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                DataTable Xet = new DataTable();
                DataTable Cet = new DataTable();

                SqlConnection cn = new SqlConnection(DBUtil.GetAppConnectionString());
                SqlConnection cnc = new SqlConnection(DBUtil.GetAppConnectionString());

                SqlDataAdapter c = new SqlDataAdapter("SELECT * FROM NetManager WHERE Id = " + IdNetMan, cn);
                SqlCommandBuilder cm = new SqlCommandBuilder(c);
                c.UpdateCommand = cm.GetUpdateCommand();
                SqlDataAdapter x = new SqlDataAdapter("SELECT * FROM ConnectionMaster WHERE Id = " + IdConMaster, cnc);
                SqlCommandBuilder xm = new SqlCommandBuilder(x);
                x.UpdateCommand = xm.GetUpdateCommand();

                c.Fill(Cet);
                x.Fill(Xet);

                DataRow nRow = Xet.Rows[0];
                nRow["Port"] = TxtPort.Text;
                nRow["TypeMsg"] = TCPUtil.GetTypeChain(CmbTCadena.Text);
                nRow["TimeOutSecs"] = ValTimeOut.Checked ? Convert.ToInt32(TxtTimeOut.Value) : 0;
                nRow["BytesHeader"] = Convert.ToInt32(CmbHByte.Text);
                x.Update(Xet);

                DataRow nRon = Cet.Rows[0];
                nRon["DescriptionName"] = TxtName.Text;
                nRon["ValidateTimeOut"] = ValTimeOut.Checked;
                nRon["TimeOut"] = ValTimeOut.Checked ? Convert.ToInt32(TxtTimeOut.Value) : 0;

                c.Update(Cet);

                MessageBox.Show("NetManager editada exitosamente", TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();

            }
            else
            {
                if (DBUtil.GetSQL("SELECT * FROM NetManager WHERE DescriptionName = '" + TxtName.Text + "'").Rows.Count > 0)
                {
                    MessageBox.Show("Ya existe una NetManager con el nombre de " + TxtName.Text + " no se puede sobreescribir", TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (DBUtil.GetSQL("SELECT * FROM ConnectionMaster WHERE Port = " + Convert.ToInt32(TxtPort.Value) + "").Rows.Count > 0)
                {
                    MessageBox.Show("No es posible usar este puerto, ya existe una conexion que lo usa", TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                DataTable Xet = new DataTable();
                DataTable Cet = new DataTable();

                SqlConnection cn = new SqlConnection(DBUtil.GetAppConnectionString());
                SqlConnection cnc = new SqlConnection(DBUtil.GetAppConnectionString());

                SqlDataAdapter c = new SqlDataAdapter("SELECT * FROM NetManager", cn);
                SqlCommandBuilder cm = new SqlCommandBuilder(c);
                c.InsertCommand = cm.GetInsertCommand();
                c.UpdateCommand = cm.GetUpdateCommand();
                SqlDataAdapter x = new SqlDataAdapter("SELECT * FROM ConnectionMaster", cnc);
                SqlCommandBuilder xm = new SqlCommandBuilder(x);
                x.InsertCommand = xm.GetInsertCommand();
                x.UpdateCommand = xm.GetUpdateCommand();

                c.Fill(Cet);
                x.Fill(Xet);

                DataRow nRow = Xet.NewRow();
                nRow["DescriptionName"] = "Server_" + TxtName.Text;
                nRow["Active"] = true;
                nRow["TCPClient"] = false;
                nRow["IPAdress"] = "127.0.0.1";
                nRow["Port"] = TxtPort.Text;
                nRow["TypeMsg"] = TCPUtil.GetTypeChain(CmbTCadena.Text);
                nRow["ConnectionType"] = 1;
                nRow["Url"] = "1";
                nRow["TimeOutSecs"] = Convert.ToInt32(TxtTimeOut.Value);
                nRow["BytesHeader"] = Convert.ToInt32(CmbHByte.Text);

                Xet.Rows.Add(nRow);
                x.Update(Xet);

                Int32 IdConex = Convert.ToInt32(DBUtil.GetSQL("SELECT TOP(1) Id FROM ConnectionMaster ORDER BY Id DESC").Rows[0]["Id"]);

                DataRow nRon = Cet.NewRow();
                nRon["DescriptionName"] = TxtName.Text;
                nRon["ConnectionMasterId"] = IdConex;
                nRon["EchoMsg"] = 0;
                nRon["EchoMsgSeconds"] = 0;
                nRon["DynamicKey"] = 0;
                nRon["DinamicKeyMinutes"] = 0;
                nRon["TypeDinamicKey"] = "-";
                nRon["ValidateTimeOut"] = ValTimeOut.Checked;
                nRon["TimeOut"] = ValTimeOut.Checked ? Convert.ToInt32(TxtTimeOut.Value) : 0;

                Cet.Rows.Add(nRon);
                c.Update(Cet);

                MessageBox.Show("NetManager agregada exitosamente", TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
        }

        private void ValTimeOut_CheckedChanged(object sender, EventArgs e)
        {
            if (ValTimeOut.Checked)
            {
                label5.Visible = true;
                TxtTimeOut.Visible = true;
            }
            else
            {
                label5.Visible = false;
                TxtTimeOut.Visible = false;
            }
        }
    }
}
