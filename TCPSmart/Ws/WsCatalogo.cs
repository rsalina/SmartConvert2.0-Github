using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace TCPSmart.Ws
{
    public partial class WsCatalogo : Form
    {
        SqlDataAdapter c;
        DataTable Xet;
        //DataRow prevRow;
        public WsCatalogo()
        {
            InitializeComponent();
        }

        private void LoadData()
        {
            try
            {
                string sql_query = @"SELECT Id,Ambiente,PLevl_Typ,PMerchant,PLogin_Id,UserAtm,Url FROM WsParams";
                SqlConnection cn = new SqlConnection(DBUtil.GetAppConnectionString());
                c = new SqlDataAdapter(sql_query, cn);
                Xet = new DataTable();
                c.Fill(Xet);
                long identity = Xet.Select("", "Id DESC").Length > 0 ? Convert.ToInt64(Xet.Select("", "Id DESC")[0][0]) : 0;
                Xet.Columns["Id"].AutoIncrement = true;
                Xet.Columns["Id"].AutoIncrementSeed = identity + 1;
                Xet.Columns["Id"].AutoIncrementStep = 1;
                Xet.PrimaryKey = new DataColumn[] { Xet.Columns["Id"] };
                var view = Xet.DefaultView;
                gridView1.Columns["Id"].DataPropertyName = "Id";
                //gridView1.Columns["WsAlterno"].DataPropertyName = "WsAlterno";
                gridView1.Columns["Ambiente"].DataPropertyName = "Ambiente";
                gridView1.Columns["PLevl_Typ"].DataPropertyName = "PLevl_Typ";
                gridView1.Columns["PMerchant"].DataPropertyName = "PMerchant";
                gridView1.Columns["PLogin_Id"].DataPropertyName = "PLogin_Id";
                gridView1.Columns["UserAtm"].DataPropertyName = "UserAtm";
                gridView1.Columns["Url"].DataPropertyName = "Url";
                gridView1.DataSource = view;
                barStaticItem1.Text = "Conexiones Registradas: " + Xet.Rows.Count;
                gridView1.Refresh();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
        private void WsCatalogo_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            WsManager hijo = new WsManager();
            hijo.ShowDialog();
            LoadData();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (gridView1.Rows.Count > 0)
            {
                if (MessageBox.Show("Desea borrar este registro?", TCPUtil.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    int r = gridView1.CurrentRow.Index;
                    int pc = int.Parse(gridView1.Rows[r].Cells["Id"].Value.ToString());
                    string sql_query = @"DELETE FROM WsParams WHERE Id = @Id";
                    SqlConnection cn = new SqlConnection(DBUtil.GetAppConnectionString());
                    try { cn.Open(); } catch (Exception ex) { MessageBox.Show("Error de Conexion: " + ex.Message.ToString(), TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error); }
                    SqlCommand cmm = new SqlCommand(sql_query, cn);
                    cmm.Parameters.AddWithValue("@Id", pc);
                    int res = cmm.ExecuteNonQuery();
                    if (res != 0)
                    {
                        MessageBox.Show("Registro eliminado exitosamente", TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        cn.Close();
                        LoadData();
                    }
                    else
                    {
                        MessageBox.Show("Error eliminando el Registro", TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (gridView1.Rows.Count > 0)
            {
                int r = gridView1.CurrentRow.Index;
                int id = int.Parse(gridView1.Rows[r].Cells["Id"].Value.ToString());
                var f = new WsManager(Convert.ToInt32(id));
                f.ShowDialog();
                LoadData();
            }
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            if (gridView1.Rows.Count > 0)
            {
                int r = gridView1.CurrentRow.Index;
                int id = int.Parse(gridView1.Rows[r].Cells["Id"].Value.ToString());
                var f = new WsManager(Convert.ToInt32(id));
                f.ShowDialog();
                LoadData();
            }
        }

        private void gridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
