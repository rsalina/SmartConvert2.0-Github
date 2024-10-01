namespace TCPSmart
{
    partial class ParametrosCnSQL
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.TxtBDPass = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.TxtDBUser = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.TxtDBName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.TxtBDPuerto = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.TxtBDServer = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnSaveDBParams = new System.Windows.Forms.Button();
            this.btnTestDBParams = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // TxtBDPass
            // 
            this.TxtBDPass.Location = new System.Drawing.Point(309, 71);
            this.TxtBDPass.Name = "TxtBDPass";
            this.TxtBDPass.PasswordChar = '*';
            this.TxtBDPass.Size = new System.Drawing.Size(148, 20);
            this.TxtBDPass.TabIndex = 21;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(239, 74);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 13);
            this.label5.TabIndex = 20;
            this.label5.Text = "Contraseña:";
            // 
            // TxtDBUser
            // 
            this.TxtDBUser.Location = new System.Drawing.Point(79, 71);
            this.TxtDBUser.Name = "TxtDBUser";
            this.TxtDBUser.Size = new System.Drawing.Size(154, 20);
            this.TxtDBUser.TabIndex = 19;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 74);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 13);
            this.label4.TabIndex = 18;
            this.label4.Text = "Usuario:";
            // 
            // TxtDBName
            // 
            this.TxtDBName.Location = new System.Drawing.Point(79, 45);
            this.TxtDBName.Name = "TxtDBName";
            this.TxtDBName.Size = new System.Drawing.Size(378, 20);
            this.TxtDBName.TabIndex = 17;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 13);
            this.label3.TabIndex = 16;
            this.label3.Text = "Nombre DB:";
            // 
            // TxtBDPuerto
            // 
            this.TxtBDPuerto.Location = new System.Drawing.Point(398, 19);
            this.TxtBDPuerto.Name = "TxtBDPuerto";
            this.TxtBDPuerto.Size = new System.Drawing.Size(59, 20);
            this.TxtBDPuerto.TabIndex = 15;
            this.TxtBDPuerto.Text = "1433";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(351, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Puerto:";
            // 
            // TxtBDServer
            // 
            this.TxtBDServer.Location = new System.Drawing.Point(79, 19);
            this.TxtBDServer.Name = "TxtBDServer";
            this.TxtBDServer.Size = new System.Drawing.Size(266, 20);
            this.TxtBDServer.TabIndex = 13;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Servidor IP:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnTestDBParams);
            this.groupBox1.Controls.Add(this.btnSaveDBParams);
            this.groupBox1.Controls.Add(this.TxtBDPuerto);
            this.groupBox1.Controls.Add(this.TxtBDPass);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.TxtBDServer);
            this.groupBox1.Controls.Add(this.TxtDBUser);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.TxtDBName);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(590, 101);
            this.groupBox1.TabIndex = 22;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Servidor de Base de Datos";
            // 
            // btnSaveDBParams
            // 
            this.btnSaveDBParams.Image = global::TCPSmart.Properties.Resources.DatabaseRunning;
            this.btnSaveDBParams.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSaveDBParams.Location = new System.Drawing.Point(463, 19);
            this.btnSaveDBParams.Name = "btnSaveDBParams";
            this.btnSaveDBParams.Size = new System.Drawing.Size(121, 29);
            this.btnSaveDBParams.TabIndex = 22;
            this.btnSaveDBParams.Text = "Guardar";
            this.btnSaveDBParams.UseVisualStyleBackColor = true;
            this.btnSaveDBParams.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnTestDBParams
            // 
            this.btnTestDBParams.Image = global::TCPSmart.Properties.Resources.DatabaseSettings;
            this.btnTestDBParams.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnTestDBParams.Location = new System.Drawing.Point(463, 62);
            this.btnTestDBParams.Name = "btnTestDBParams";
            this.btnTestDBParams.Size = new System.Drawing.Size(121, 29);
            this.btnTestDBParams.TabIndex = 23;
            this.btnTestDBParams.Text = "Probar";
            this.btnTestDBParams.UseVisualStyleBackColor = true;
            this.btnTestDBParams.Click += new System.EventHandler(this.button2_Click);
            // 
            // ParametrosCnSQL
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(614, 125);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ParametrosCnSQL";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Parametros de conexión SQL";
            this.Load += new System.EventHandler(this.ParametrosCnSQL_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox TxtBDPass;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox TxtDBUser;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox TxtDBName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox TxtBDPuerto;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TxtBDServer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnTestDBParams;
        private System.Windows.Forms.Button btnSaveDBParams;
    }
}