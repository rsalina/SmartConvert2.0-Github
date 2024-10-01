namespace TCPSmart
{
    partial class VisorISO8583
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VisorISO8583));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.simpleButton1 = new System.Windows.Forms.Button();
            this.TxtF39 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.checkEdit3 = new System.Windows.Forms.CheckBox();
            this.checkEdit2 = new System.Windows.Forms.CheckBox();
            this.checkEdit1 = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.CmbTypeChain = new System.Windows.Forms.ComboBox();
            this.TxtHex = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.gridView1 = new System.Windows.Forms.DataGridView();
            this.LstLog = new System.Windows.Forms.ListBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.simpleButton1);
            this.groupBox1.Controls.Add(this.TxtF39);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.checkEdit3);
            this.groupBox1.Controls.Add(this.checkEdit2);
            this.groupBox1.Controls.Add(this.checkEdit1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.CmbTypeChain);
            this.groupBox1.Controls.Add(this.TxtHex);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(697, 117);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Datos";
            // 
            // simpleButton1
            // 
            this.simpleButton1.Image = global::TCPSmart.Properties.Resources.ProcessOk;
            this.simpleButton1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.simpleButton1.Location = new System.Drawing.Point(541, 50);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(126, 50);
            this.simpleButton1.TabIndex = 8;
            this.simpleButton1.Text = "Procesar";
            this.simpleButton1.UseVisualStyleBackColor = true;
            this.simpleButton1.Click += new System.EventHandler(this.button1_Click);
            // 
            // TxtF39
            // 
            this.TxtF39.Location = new System.Drawing.Point(488, 50);
            this.TxtF39.Name = "TxtF39";
            this.TxtF39.Size = new System.Drawing.Size(47, 20);
            this.TxtF39.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(454, 53);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(28, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "F39:";
            // 
            // checkEdit3
            // 
            this.checkEdit3.AutoSize = true;
            this.checkEdit3.Location = new System.Drawing.Point(334, 52);
            this.checkEdit3.Name = "checkEdit3";
            this.checkEdit3.Size = new System.Drawing.Size(114, 17);
            this.checkEdit3.TabIndex = 6;
            this.checkEdit3.Text = "Simular Respuesta";
            this.checkEdit3.UseVisualStyleBackColor = true;
            this.checkEdit3.CheckedChanged += new System.EventHandler(this.checkEdit3_CheckedChanged);
            // 
            // checkEdit2
            // 
            this.checkEdit2.AutoSize = true;
            this.checkEdit2.Location = new System.Drawing.Point(263, 52);
            this.checkEdit2.Name = "checkEdit2";
            this.checkEdit2.Size = new System.Drawing.Size(65, 17);
            this.checkEdit2.TabIndex = 5;
            this.checkEdit2.Text = "Adjuntar";
            this.checkEdit2.UseVisualStyleBackColor = true;
            // 
            // checkEdit1
            // 
            this.checkEdit1.AutoSize = true;
            this.checkEdit1.Location = new System.Drawing.Point(192, 52);
            this.checkEdit1.Name = "checkEdit1";
            this.checkEdit1.Size = new System.Drawing.Size(65, 17);
            this.checkEdit1.TabIndex = 4;
            this.checkEdit1.Text = "Traducir";
            this.checkEdit1.UseVisualStyleBackColor = true;
            this.checkEdit1.CheckedChanged += new System.EventHandler(this.checkEdit1_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Tipo de Cadena:";
            // 
            // CmbTypeChain
            // 
            this.CmbTypeChain.FormattingEnabled = true;
            this.CmbTypeChain.Items.AddRange(new object[] {
            "1 BASEi 2 BYTES",
            "1 BASEi 4 BYTES",
            "3 EWA",
            "4 BERBETEC"});
            this.CmbTypeChain.Location = new System.Drawing.Point(98, 50);
            this.CmbTypeChain.Name = "CmbTypeChain";
            this.CmbTypeChain.Size = new System.Drawing.Size(88, 21);
            this.CmbTypeChain.TabIndex = 2;
            this.CmbTypeChain.Text = "Seleccione...";
            // 
            // TxtHex
            // 
            this.TxtHex.Location = new System.Drawing.Point(98, 24);
            this.TxtHex.Name = "TxtHex";
            this.TxtHex.Size = new System.Drawing.Size(569, 20);
            this.TxtHex.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Cadena HEX:";
            // 
            // gridView1
            // 
            this.gridView1.AllowUserToAddRows = false;
            this.gridView1.AllowUserToDeleteRows = false;
            this.gridView1.AllowUserToResizeRows = false;
            this.gridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridView1.BackgroundColor = System.Drawing.SystemColors.ButtonFace;
            this.gridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.gridView1.Location = new System.Drawing.Point(0, 123);
            this.gridView1.MultiSelect = false;
            this.gridView1.Name = "gridView1";
            this.gridView1.ReadOnly = true;
            this.gridView1.RowHeadersWidth = 10;
            this.gridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridView1.Size = new System.Drawing.Size(697, 220);
            this.gridView1.TabIndex = 10;
            // 
            // LstLog
            // 
            this.LstLog.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.LstLog.FormattingEnabled = true;
            this.LstLog.Location = new System.Drawing.Point(0, 349);
            this.LstLog.Name = "LstLog";
            this.LstLog.Size = new System.Drawing.Size(697, 56);
            this.LstLog.TabIndex = 11;
            // 
            // VisorISO8583
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(697, 405);
            this.Controls.Add(this.LstLog);
            this.Controls.Add(this.gridView1);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "VisorISO8583";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Visor ISO8583";
            this.Load += new System.EventHandler(this.VisorISO8583_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox TxtF39;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox checkEdit3;
        private System.Windows.Forms.CheckBox checkEdit2;
        private System.Windows.Forms.CheckBox checkEdit1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox CmbTypeChain;
        private System.Windows.Forms.TextBox TxtHex;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button simpleButton1;
        private System.Windows.Forms.DataGridView gridView1;
        private System.Windows.Forms.ListBox LstLog;
    }
}