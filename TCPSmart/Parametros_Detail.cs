using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace TCPSmart
{
    public partial class Parametros_Detail : Form
    {
        public static int OpenValue;
        public static string paramcode, paramvalue;
        public Parametros_Detail()
        {
            InitializeComponent();
        }

        private void Parametros_Detail_Load(object sender, EventArgs e)
        {
            switch (OpenValue)
            {
                case 1:
                    button1.Text = "Guardar";
                    break;
                case 2:
                    textBox1.Text = paramcode;
                    textBox2.Text = paramvalue;
                    button1.Text = "Actualizar";
                    break;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            switch (OpenValue)
            {
                case 1:
                    if (val_param() > 0)
                    {
                        MessageBox.Show("El ParameterCode ya existe", TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (string.IsNullOrEmpty(textBox1.Text))
                    {
                        MessageBox.Show("Debe definir ParameterCode", TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else
                    {
                        string sql_query = @"INSERT INTO Parameter (ParameterCode,ParameterValue,IsSystemParameter)
                        VALUES (@ParameterCode,@ParameterValue,@IsSystemParameter)";
                        SqlConnection cn = new SqlConnection(DBUtil.GetAppConnectionString());
                        try { cn.Open(); } catch (Exception ex) { MessageBox.Show("Error de Conexion: " + ex.Message.ToString(), TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error); }
                        SqlCommand cmm = new SqlCommand(sql_query, cn);
                        cmm.Parameters.AddWithValue("@ParameterCode", textBox1.Text.Replace("*", "").Trim());
                        cmm.Parameters.AddWithValue("@ParameterValue", textBox2.Text.Replace("*", ""));
                        cmm.Parameters.AddWithValue("@IsSystemParameter", "0");
                        int res = cmm.ExecuteNonQuery();
                        if (res != 0)
                        {
                            MessageBox.Show("Registro agregado exitosamente", TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            cn.Close();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Error Agregando el Registro", TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    break;
                case 2:
                    if (string.IsNullOrEmpty(textBox1.Text))
                    {
                        MessageBox.Show("Debe definir ParameterCode", TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else
                    {
                        string sql_query = @"UPDATE Parameter SET ParameterCode = @ParameterCode ,
                        ParameterValue = @ParameterValue  WHERE TRIM(ParameterCode) = @param";
                        SqlConnection cn = new SqlConnection(DBUtil.GetAppConnectionString());
                        try { cn.Open(); } catch (Exception ex) { MessageBox.Show("Error de Conexion: " + ex.Message.ToString(), TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error); }
                        SqlCommand cmm = new SqlCommand(sql_query, cn);
                        cmm.Parameters.AddWithValue("@ParameterCode", textBox1.Text.Replace("*", "").Trim());
                        cmm.Parameters.AddWithValue("@ParameterValue", textBox2.Text.Replace("*", ""));
                        cmm.Parameters.AddWithValue("@param", paramcode);
                        int res = cmm.ExecuteNonQuery();
                        if (res != 0)
                        {
                            MessageBox.Show("Registro actualizado exitosamente", TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            cn.Close();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Error actualizando el Registro", TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    break;
            }
        }

        int val_param()
        {
            int x = 0;
            string sql_query = "SELECT * FROM Parameter WHERE TRIM(ParameterCode) = @param";
            SqlConnection cn = new SqlConnection(DBUtil.GetAppConnectionString());
            try { cn.Open(); } catch (Exception ex) { MessageBox.Show("Error de Conexion: " + ex.Message.ToString(), TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error); }
            SqlCommand cmm = new SqlCommand(sql_query, cn);
            cmm.Parameters.AddWithValue("@param", textBox1.Text.Replace("*", "").Trim());
            SqlDataAdapter da = new SqlDataAdapter(cmm);
            DataTable dt = new DataTable();
            da.Fill(dt);
            x = dt.Rows.Count;
            return x;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
