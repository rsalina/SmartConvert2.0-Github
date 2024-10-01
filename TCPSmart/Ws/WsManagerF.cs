using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace TCPSmart.Ws
{
    public partial class WsManager : Form
    {
        DataTable editRow;
        Dictionary<int, string> typeMsgDic = new Dictionary<int, string>();
        private bool IsEditing { get { return editRow != null; } }
        public WsManager()
        {
            InitializeComponent();
        }
        public WsManager(int id)
        {
            InitializeComponent();

            editRow = DBUtil.GetSQL("SELECT Active,Ambiente,Url,UserAtm,PwdAtm,PLogin_Id,Papp_Code,PLevl_Typ,PMerchant,PPasword,Papp_Vers,LicToken FROM WsParams WHERE Id = " + id);
        }
        private void WsManager_Load(object sender, EventArgs e)
        {
            try
            {
                if (IsEditing)
                {
                    CmbTAmb.Text = GetRowValue("Ambiente").ToString();
                    TxtUrl.Text = GetRowValue("Url").ToString();
                    TxtUserAtm.Text = GetRowValue("UserAtm").ToString();
                    TxtPwdAtm.Text = GetRowValue("PwdAtm").ToString();
                    TxtLoginId.Text = GetRowValue("PLogin_Id").ToString();
                    TxtAppCode.Text = GetRowValue("Papp_Code").ToString();
                    TxtLevelType.Text = GetRowValue("PLevl_Typ").ToString();
                    TxtMerchant.Text = GetRowValue("PMerchant").ToString();
                    TxtPwd.Text = GetRowValue("PPasword").ToString();
                    TxtAppVersion.Text = GetRowValue("Papp_Vers").ToString();
                    TxtLic.Text = GetRowValue("LicToken").ToString();

                    chkActive.Visible = true;
                    chkActive.Checked = (bool)GetRowValue("Active");
                    var tokenData = JWT.ValidarJwtToken(TxtLic.Text);
                    var expDate = (DateTime)tokenData["exp_datetime"];
                    TxtTokenLic.Text = "Licencia valida hasta: " + expDate.ToString("dd/MM/yyyy");
                    //checkEdit1.Visible = true;
                    //checkEdit1.Checked = (bool)GetRowValue("WsAlterno");
                }
                else
                {
                    chkActive.Visible = false;
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
        private object GetRowValue(string field)
        {
            if (editRow == null)
                return null;

            if (editRow.Rows.Count > 0)
                return editRow.Rows[0][field];
            else
                return null;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(CmbTAmb.Text))
                {
                    MessageBox.Show("El identificador del ambiente no puede ir vacio", TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!IsEditing && DBUtil.GetSQL("SELECT * FROM WsParams WHERE Ambiente = '" + CmbTAmb.Text + "' AND Active = 1").Rows.Count > 0)
                {
                    MessageBox.Show("El identificador del ambiente ya esta registrado", TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (string.IsNullOrWhiteSpace(TxtUrl.Text))
                {
                    MessageBox.Show("El URL del Ambiente no puede ir vacio", TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (string.IsNullOrWhiteSpace(TxtUserAtm.Text))
                {
                    MessageBox.Show("El usuario ATM para el ambiente no puede ir vacio", TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (string.IsNullOrWhiteSpace(TxtPwdAtm.Text))
                {
                    MessageBox.Show("La Contraseña ATM para el ambiente no puede ir vacio", TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (string.IsNullOrWhiteSpace(TxtLoginId.Text))
                {
                    MessageBox.Show("El Login Id para el ambiente no puede ir vacio", TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (string.IsNullOrWhiteSpace(TxtAppCode.Text))
                {
                    MessageBox.Show("El App Code para el ambiente no puede ir vacio", TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (string.IsNullOrWhiteSpace(TxtLevelType.Text))
                {
                    MessageBox.Show("El Level Type para el ambiente no puede ir vacio", TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (string.IsNullOrWhiteSpace(TxtMerchant.Text))
                {
                    MessageBox.Show("El Merchant ID para el ambiente no puede ir vacio", TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (string.IsNullOrWhiteSpace(TxtPwd.Text))
                {
                    MessageBox.Show("La Contraseña para el ambiente no puede ir vacio", TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (string.IsNullOrWhiteSpace(TxtAppVersion.Text))
                {
                    MessageBox.Show("El App Version para el ambiente no puede ir vacio", TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (string.IsNullOrWhiteSpace(TxtLic.Text))
                {
                    MessageBox.Show("La Licencia para el ambiente no puede ir vacia", TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }


                if (!IsEditing && DBUtil.GetSQL("SELECT * FROM WsParams WHERE LicToken ='" + TxtLic.Text + "' AND Active = 1 AND Id <> " + GetRowValue("Id").ToString()).Rows.Count > 0)
                {
                    MessageBox.Show("La Licencia para el ambiente debe ser unica", TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var tokenData = JWT.ValidarJwtToken(TxtLic.Text);
                if (tokenData.Count > 0)
                {
                    try
                    {
                        var expDate = (DateTime)tokenData["exp_datetime"];
                        if (DateTime.Now > expDate)
                        {
                            MessageBox.Show($"Licencia venció el {expDate.ToString("dd/MM/yyyy")}: ingrese una licencia vigente y valida", TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                    }
                    catch
                    {
                        MessageBox.Show("La Licencia es invalida, por favor ingrese una licencia valida", TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }

                }
                else
                {
                    MessageBox.Show("La Licencia es invalida, por favor ingrese una licencia valida", TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                var expDateV = (DateTime)tokenData["exp_datetime"];
                TxtTokenLic.Text = "Licencia valida hasta: " + expDateV.ToString("dd/MM/yyyy");

                SqlConnection Cn = new SqlConnection(DBUtil.GetAppConnectionString());
                Cn.Open();
                SqlDataAdapter c = new SqlDataAdapter("SELECT * FROM WsParams", Cn);
                SqlCommandBuilder m = new SqlCommandBuilder(c);
                c.InsertCommand = m.GetInsertCommand();
                c.UpdateCommand = m.GetUpdateCommand();

                DataTable xet = new DataTable();
                c.Fill(xet);

                DataRow nRow;
                if (!IsEditing)
                    nRow = xet.NewRow();
                else
                    nRow = xet.Select("Id = " + GetRowValue("Id").ToString()).FirstOrDefault();

                if (nRow != null)
                {
                    nRow["Ambiente"] = CmbTAmb.Text;
                    nRow["Url"] = TxtUrl.Text;
                    nRow["UserAtm"] = TxtUserAtm.Text;
                    nRow["PwdAtm"] = TxtPwdAtm.Text;
                    nRow["PLogin_Id"] = TxtLoginId.Text;
                    nRow["Papp_Code"] = TxtAppCode.Text;
                    nRow["PLevl_Typ"] = TxtLevelType.Text;
                    nRow["PPasword"] = TxtPwd.Text;
                    nRow["PMerchant"] = TxtMerchant.Text;
                    nRow["Papp_Vers"] = TxtAppVersion.Text;
                    nRow["Active"] = IsEditing ? chkActive.Checked : true;
                    nRow["LicToken"] = TxtLic.Text;
                    //nRow["WsAlterno"] = checkEdit1.Checked;

                    if (!IsEditing)
                        xet.Rows.Add(nRow);

                    c.Update(xet);

                    Cn.Close();
                    MessageBox.Show("Ambiente actualizado correctamente", TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    Cn.Close();
                    MessageBox.Show("El registro ya no existe y no puede ser modificado", TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, TCPUtil.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
    }
}
