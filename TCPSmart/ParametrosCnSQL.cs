using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace TCPSmart
{
    public partial class ParametrosCnSQL : Form
    {
        public ParametrosCnSQL()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                int port = Convert.ToInt32(TxtBDPuerto.Text);
                DBUtil.TestConnectionParameters(TxtBDServer.Text, port, TxtDBName.Text, TxtDBUser.Text, TxtBDPass.Text, true);
                TCPUtil.PuertoBD = port;
                TCPUtil.NameBD = TxtDBName.Text;
                TCPUtil.UserBD = TxtDBUser.Text;
                TCPUtil.ServerBD = TxtBDServer.Text;
                TCPUtil.PassBD = TxtBDPass.Text;
                TCPUtil.SaveConfigSQL();
                TCPUtil.LoadConfigSQL();
                MessageBox.Show("Conexión SQL configurada correctamente y guardada", TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                DBUtil.TestConnectionParameters(TxtBDServer.Text, Convert.ToInt32(TxtBDPuerto.Text), TxtDBName.Text, TxtDBUser.Text, TxtBDPass.Text, true);
                MessageBox.Show("Conexión SQL exitosa. Recuerde guardarla si desea hacerla la conexión por defecto de la aplicación", TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ParametrosCnSQL_Load(object sender, EventArgs e)
        {
            CargaConfig();
        }

        private void CargaConfig()
        {
            TCPUtil.LoadConfigSQL();

            //<====== BLOQUE DB
            TxtBDServer.Text = TCPUtil.ServerBD;
            TxtDBUser.Text = TCPUtil.UserBD;
            TxtBDPass.Text = TCPUtil.PassBD;
            TxtBDPuerto.Text = TCPUtil.PuertoBD.ToString();
            TxtDBName.Text = TCPUtil.NameBD;
        }
    }
}
