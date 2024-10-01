using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace TCPSmart
{
    public partial class NetManagerCatalog : Form
    {
        SqlDataAdapter c;
        DataTable Xet;
        public NetManagerCatalog()
        {
            InitializeComponent();
        }

        private void LoadData()
        {
            try
            {
                string sql_query = @"SELECT N.Id,N.DescriptionName,N.ValidatetokenF52,N.ValidateFeeF28,
                N.ValidateCodeF3,N.ValidateTimeOut,N.TimeOut,C.Port AS porto,C.BytesHeader AS HeaderByte, 
                CASE C.TypeMsg WHEN 1 THEN 'BASEi' WHEN 2 THEN 'XML' WHEN 3 THEN 'EWA' WHEN 4 THEN 'BEBERTEC' 
                END AS TypeMsg FROM NetManager N LEFT JOIN ConnectionMaster C ON N.ConnectionMasterId = C.Id ";
                SqlConnection cn = new SqlConnection(DBUtil.GetAppConnectionString());
                c = new SqlDataAdapter(sql_query, cn);
                Xet = new DataTable();
                c.Fill(Xet);
                Xet.PrimaryKey = new DataColumn[] { Xet.Columns["ID"] };
                var view = Xet.DefaultView;
                gridView1.Columns["ID"].DataPropertyName = "Id";
                gridView1.Columns["Nombre"].DataPropertyName = "DescriptionName";
                gridView1.Columns["Tipo"].DataPropertyName = "TypeMsg";
                gridView1.Columns["Puerto"].DataPropertyName = "porto";
                gridView1.Columns["Header"].DataPropertyName = "HeaderByte";
                gridView1.Columns["ValF52"].DataPropertyName = "ValidatetokenF52";
                gridView1.Columns["ValFee"].DataPropertyName = "ValidateFeeF28";
                gridView1.Columns["ValRoute"].DataPropertyName = "ValidateCodeF3";
                gridView1.Columns["ValTimeOut"].DataPropertyName = "ValidateTimeOut";
                gridView1.Columns["TimeOut"].DataPropertyName = "TimeOut";
                gridView1.DataSource = view;
                barStaticItem1.Text = "NetManagers Registrados: " + Xet.Rows.Count;
                gridView1.Refresh();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
        private void NetManagerCatalog_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Desea borrar esta NetManager?", TCPUtil.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    int r = gridView1.CurrentRow.Index;
                    string dataRow = gridView1.Rows[r].Cells["Id"].Value.ToString();

                    Int32 IdConex = Convert.ToInt32(dataRow);

                    DBUtil.ExecuteSQL("DELETE FROM ConnectionMaster WHERE Id = (SELECT ConnectionMasterId FROM NetManager WHERE Id= " + IdConex + ")");
                    DBUtil.ExecuteSQL("DELETE FROM NetManager WHERE Id = " + IdConex);

                    LoadData();

                    MessageBox.Show("Registro borrado exitosamente", TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                catch (Exception ex) { MessageBox.Show(ex.Message, TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error); }
            }
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            int r = gridView1.CurrentRow.Index;
            var id = int.Parse(gridView1.Rows[r].Cells["Id"].Value.ToString());
            var f = new NetManagerF(id);
            f.ShowDialog();
            LoadData();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            int r = gridView1.CurrentRow.Index;
            var id = int.Parse(gridView1.Rows[r].Cells["Id"].Value.ToString());
            var f = new NetManagerF(id);
            f.ShowDialog();
            LoadData();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            new NetManagerF(0).ShowDialog();
            LoadData();
        }
    }
}
