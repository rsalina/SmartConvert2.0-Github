using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace TCPSmart
{
    public partial class Parametros : Form
    {
        SqlDataAdapter c;
        DataTable Xet;
        public static string paramcode, paramvalue;
        public Parametros()
        {
            InitializeComponent();
        }
        private void CargaConfig()
        {
            try
            {
                string sql_query = "SELECT * FROM Parameter WHERE IsSystemParameter = 0";
                SqlConnection cn = new SqlConnection(DBUtil.GetAppConnectionString());
                c = new SqlDataAdapter(sql_query, cn);
                Xet = new DataTable();
                c.Fill(Xet);
                Xet.PrimaryKey = new DataColumn[] { Xet.Columns["ParameterCode"] };
                var view = Xet.DefaultView;
                gridView1.Columns["ParameterCode"].DataPropertyName = "ParameterCode";
                gridView1.Columns["ParameterValue"].DataPropertyName = "ParameterValue";
                gridView1.Columns["IsSystemParameter"].DataPropertyName = "IsSystemParameter";
                gridView1.DataSource = view;
                barStaticItem1.Text = "Parámetros Registrados: " + view.Count;
                gridView1.Refresh();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Parametros_Detail frm = new Parametros_Detail();
            Parametros_Detail.OpenValue = 1;
            frm.ShowDialog();
            CargaConfig();
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            int r = gridView1.CurrentRow.Index;
            string pc = gridView1.Rows[r].Cells["ParameterCode"].Value.ToString();
            string pv = gridView1.Rows[r].Cells["ParameterValue"].Value.ToString();
            Parametros_Detail frm = new Parametros_Detail();
            Parametros_Detail.OpenValue = 2;
            Parametros_Detail.paramcode = pc;
            Parametros_Detail.paramvalue = pv;
            frm.ShowDialog();
            CargaConfig();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            int r = gridView1.CurrentRow.Index;
            string pc = gridView1.Rows[r].Cells["ParameterCode"].Value.ToString();
            string pv = gridView1.Rows[r].Cells["ParameterValue"].Value.ToString();
            Parametros_Detail frm = new Parametros_Detail();
            Parametros_Detail.OpenValue = 2;
            Parametros_Detail.paramcode = pc;
            Parametros_Detail.paramvalue = pv;
            frm.ShowDialog();
            CargaConfig();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Desea borrar este registro?", TCPUtil.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                int r = gridView1.CurrentRow.Index;
                string pc = gridView1.Rows[r].Cells["ParameterCode"].Value.ToString();
                string sql_query = @"DELETE FROM Parameter WHERE TRIM(ParameterCode) = @param";
                SqlConnection cn = new SqlConnection(DBUtil.GetAppConnectionString());
                try { cn.Open(); } catch (Exception ex) { MessageBox.Show("Error de Conexion: " + ex.Message.ToString(), TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error); }
                SqlCommand cmm = new SqlCommand(sql_query, cn);
                cmm.Parameters.AddWithValue("@param", pc);
                int res = cmm.ExecuteNonQuery();
                if (res != 0)
                {
                    MessageBox.Show("Registro eliminado exitosamente", TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cn.Close();
                    CargaConfig();
                }
                else
                {
                    MessageBox.Show("Error eliminando el Registro", TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void Parametros_Load(object sender, EventArgs e)
        {
            toolStripSplitButton1.ToolTipText = "Parametro : TestWsCATM->Valores =" + System.Environment.NewLine +
            "DEV: Desarrollo " + System.Environment.NewLine + "PRD: Produccion" + System.Environment.NewLine + "Q && A: Calidad";
            CargaConfig();
        }
    }
}
