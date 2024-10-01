using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace TCPSmart
{
    public partial class Bitacora : Form
    {
        int conectionID = 0;
        int netManagerID = 0;

        public Bitacora()
        {
            InitializeComponent();
        }

        public Bitacora(int conectionID = 0, int netManagerID = 0)
        {
            InitializeComponent();

            this.conectionID = conectionID;
            this.netManagerID = netManagerID;
        }

        private void Bitacora_Load(object sender, EventArgs e)
        {
            LoadData();
        }
        private void LoadData()
        {
            try
            {
                gridView1.AutoGenerateColumns = false;
                gridView1.DataSource = DBUtil.GetBitacora(conectionID, netManagerID);
                gridView1.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (gridView1.RowCount == 0)
            {
                MessageBox.Show("No hay registros a Exportar", TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            saveFileDialog1.FileName = "Bitacora " + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + ".csv";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    int columnCount = gridView1.Columns.Count;
                    string columnNames = "";
                    string[] outputCsv = new string[gridView1.Rows.Count + 1];
                    for (int i = 0; i < columnCount; i++)
                    {
                        if (i < 7)
                            columnNames += gridView1.Columns[i].HeaderText.ToString() + ",";
                        else columnNames += gridView1.Columns[i].HeaderText.ToString();
                    }
                    outputCsv[0] += columnNames;
                    var ids = new List<int>();
                    for (int i = 0; i < gridView1.Rows.Count; i++)
                    {
                        ids.Add(int.Parse(gridView1.Rows[i].Cells["Id"].Value.ToString()));
                        for (int j = 0; j < columnCount; j++)
                        {
                            outputCsv[i + 1] += gridView1.Rows[i].Cells[j].Value.ToString().Replace(",", ";") + ",";
                        }
                    }
                    DBUtil.UpdateBitacoraRow(ids);
                    File.WriteAllLines(saveFileDialog1.FileName, outputCsv, Encoding.UTF8);
                    MessageBox.Show("Archivo Exportado Correctamente", TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error :" + ex.Message);
                }
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Desea borrar los registros exportados?", TCPUtil.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    DBUtil.DeleteBitacora(false, conectionID, netManagerID);
                    LoadData();
                    MessageBox.Show("Registros borrados con exito!", TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridView1.RowCount == 0)
                {
                    MessageBox.Show("No hay registros a Exportar", TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (MessageBox.Show("Desea borrar TODOS los registros?", TCPUtil.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    DBUtil.DeleteBitacora(true, conectionID, netManagerID);
                    LoadData();
                    MessageBox.Show("Registros borrados con exito!", TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }
    }
}
