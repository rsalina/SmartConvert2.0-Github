namespace TCPSmart
{
    partial class NetManagerF
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NetManagerF));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.TxtTimeOut = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.ValTimeOut = new System.Windows.Forms.CheckBox();
            this.CmbHByte = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.CmbTCadena = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.TxtPort = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.TxtName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TxtTimeOut)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TxtPort)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.CmbHByte);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.CmbTCadena);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.TxtPort);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.TxtName);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(391, 297);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Informacion de NetManager";
            // 
            // button1
            // 
            this.button1.Image = global::TCPSmart.Properties.Resources.Save;
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.Location = new System.Drawing.Point(15, 264);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(370, 27);
            this.button1.TabIndex = 9;
            this.button1.Text = "Guardar";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.TxtTimeOut);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.ValTimeOut);
            this.groupBox2.Location = new System.Drawing.Point(15, 79);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(370, 179);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Logicas";
            // 
            // TxtTimeOut
            // 
            this.TxtTimeOut.Location = new System.Drawing.Point(277, 29);
            this.TxtTimeOut.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.TxtTimeOut.Name = "TxtTimeOut";
            this.TxtTimeOut.Size = new System.Drawing.Size(87, 20);
            this.TxtTimeOut.TabIndex = 5;
            this.TxtTimeOut.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.TxtTimeOut.Visible = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(188, 33);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "TimeOut Secs:";
            this.label5.Visible = false;
            // 
            // ValTimeOut
            // 
            this.ValTimeOut.AutoSize = true;
            this.ValTimeOut.Location = new System.Drawing.Point(6, 32);
            this.ValTimeOut.Name = "ValTimeOut";
            this.ValTimeOut.Size = new System.Drawing.Size(101, 17);
            this.ValTimeOut.TabIndex = 0;
            this.ValTimeOut.Text = "Validar TimeOut";
            this.ValTimeOut.UseVisualStyleBackColor = true;
            this.ValTimeOut.CheckedChanged += new System.EventHandler(this.ValTimeOut_CheckedChanged);
            // 
            // CmbHByte
            // 
            this.CmbHByte.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbHByte.FormattingEnabled = true;
            this.CmbHByte.Items.AddRange(new object[] {
            "4"});
            this.CmbHByte.Location = new System.Drawing.Point(320, 52);
            this.CmbHByte.Name = "CmbHByte";
            this.CmbHByte.Size = new System.Drawing.Size(65, 21);
            this.CmbHByte.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(245, 55);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Tipo Header:";
            // 
            // CmbTCadena
            // 
            this.CmbTCadena.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbTCadena.FormattingEnabled = true;
            this.CmbTCadena.Items.AddRange(new object[] {
            "EWA"});
            this.CmbTCadena.Location = new System.Drawing.Point(107, 52);
            this.CmbTCadena.Name = "CmbTCadena";
            this.CmbTCadena.Size = new System.Drawing.Size(132, 21);
            this.CmbTCadena.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 55);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Tipo de Cadena: ";
            // 
            // TxtPort
            // 
            this.TxtPort.Location = new System.Drawing.Point(292, 26);
            this.TxtPort.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.TxtPort.Name = "TxtPort";
            this.TxtPort.Size = new System.Drawing.Size(93, 20);
            this.TxtPort.TabIndex = 3;
            this.TxtPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(245, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Puerto:";
            // 
            // TxtName
            // 
            this.TxtName.Location = new System.Drawing.Point(107, 26);
            this.TxtName.Name = "TxtName";
            this.TxtName.Size = new System.Drawing.Size(132, 20);
            this.TxtName.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Nombre:";
            // 
            // NetManagerF
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(415, 321);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NetManagerF";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NetManager";
            this.Load += new System.EventHandler(this.NetManagerF_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TxtTimeOut)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TxtPort)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox CmbHByte;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox CmbTCadena;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown TxtPort;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TxtName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.NumericUpDown TxtTimeOut;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox ValTimeOut;
    }
}