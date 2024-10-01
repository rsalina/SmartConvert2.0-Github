namespace TCPSmart
{
    partial class Main
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.barSubItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.barButtonItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.barButtonItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.finalizarAplicacionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bsiMantenimiento = new System.Windows.Forms.ToolStripMenuItem();
            this.btsModoDebug = new System.Windows.Forms.ToolStripMenuItem();
            this.barButtonItem14 = new System.Windows.Forms.ToolStripMenuItem();
            this.configuracionesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.barButtonItem12 = new System.Windows.Forms.ToolStripMenuItem();
            this.barButtonItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.barButtonItem13 = new System.Windows.Forms.ToolStripMenuItem();
            this.barButtonItem17 = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.barStaticItem1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.barStaticItem2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.barButtonItem15 = new System.Windows.Forms.ToolStripStatusLabel();
            this.barButtonItem9 = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.gridView1 = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.BtnOpenLog = new System.Windows.Forms.ToolStripMenuItem();
            this.BtnDebugOn = new System.Windows.Forms.ToolStripMenuItem();
            this.BtnDebugOff = new System.Windows.Forms.ToolStripMenuItem();
            this.BtnSingOn = new System.Windows.Forms.ToolStripMenuItem();
            this.BtnEndSon = new System.Windows.Forms.ToolStripMenuItem();
            this.LstLog = new System.Windows.Forms.ListBox();
            this.operacionesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.activarDebugToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.desactivarDebugToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NetManagerID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Nombre = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Inicio = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UpTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Recibidas = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Enviadas = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Tipo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "desconectado.png");
            this.imageList1.Images.SetKeyName(1, "en-linea.png");
            this.imageList1.Images.SetKeyName(2, "notificacion.png");
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.barSubItem1,
            this.bsiMantenimiento,
            this.configuracionesToolStripMenuItem,
            this.operacionesToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(877, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // barSubItem1
            // 
            this.barSubItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.barButtonItem2,
            this.barButtonItem3,
            this.finalizarAplicacionToolStripMenuItem});
            this.barSubItem1.Image = ((System.Drawing.Image)(resources.GetObject("barSubItem1.Image")));
            this.barSubItem1.Name = "barSubItem1";
            this.barSubItem1.Size = new System.Drawing.Size(76, 20);
            this.barSubItem1.Text = "Servicio";
            this.barSubItem1.Click += new System.EventHandler(this.barSubItem1_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Image = global::TCPSmart.Properties.Resources.ConnectToDatabase;
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(180, 22);
            this.toolStripMenuItem1.Text = "Conexion a BD";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // barButtonItem2
            // 
            this.barButtonItem2.Image = ((System.Drawing.Image)(resources.GetObject("barButtonItem2.Image")));
            this.barButtonItem2.Name = "barButtonItem2";
            this.barButtonItem2.Size = new System.Drawing.Size(180, 22);
            this.barButtonItem2.Text = "Iniciar Servicios";
            this.barButtonItem2.Click += new System.EventHandler(this.iniciarServiciosToolStripMenuItem_Click);
            // 
            // barButtonItem3
            // 
            this.barButtonItem3.Image = ((System.Drawing.Image)(resources.GetObject("barButtonItem3.Image")));
            this.barButtonItem3.Name = "barButtonItem3";
            this.barButtonItem3.Size = new System.Drawing.Size(180, 22);
            this.barButtonItem3.Text = "Detener Servicios";
            this.barButtonItem3.Click += new System.EventHandler(this.barButtonItem3_Click);
            // 
            // finalizarAplicacionToolStripMenuItem
            // 
            this.finalizarAplicacionToolStripMenuItem.Image = global::TCPSmart.Properties.Resources.Exit;
            this.finalizarAplicacionToolStripMenuItem.Name = "finalizarAplicacionToolStripMenuItem";
            this.finalizarAplicacionToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.finalizarAplicacionToolStripMenuItem.Text = "Finalizar Aplicacion";
            this.finalizarAplicacionToolStripMenuItem.Click += new System.EventHandler(this.finalizarAplicacionToolStripMenuItem_Click);
            // 
            // bsiMantenimiento
            // 
            this.bsiMantenimiento.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btsModoDebug,
            this.barButtonItem14});
            this.bsiMantenimiento.Name = "bsiMantenimiento";
            this.bsiMantenimiento.Size = new System.Drawing.Size(101, 20);
            this.bsiMantenimiento.Text = "Mantenimiento";
            // 
            // btsModoDebug
            // 
            this.btsModoDebug.CheckOnClick = true;
            this.btsModoDebug.Image = ((System.Drawing.Image)(resources.GetObject("btsModoDebug.Image")));
            this.btsModoDebug.Name = "btsModoDebug";
            this.btsModoDebug.Size = new System.Drawing.Size(145, 22);
            this.btsModoDebug.Text = "Modo Debug";
            this.btsModoDebug.Visible = false;
            this.btsModoDebug.Click += new System.EventHandler(this.btsModoDebug_Click);
            // 
            // barButtonItem14
            // 
            this.barButtonItem14.Image = ((System.Drawing.Image)(resources.GetObject("barButtonItem14.Image")));
            this.barButtonItem14.Name = "barButtonItem14";
            this.barButtonItem14.Size = new System.Drawing.Size(145, 22);
            this.barButtonItem14.Text = "Visor ISO8583";
            this.barButtonItem14.Click += new System.EventHandler(this.barButtonItem14_Click);
            // 
            // configuracionesToolStripMenuItem
            // 
            this.configuracionesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.barButtonItem12,
            this.barButtonItem1,
            this.barButtonItem13,
            this.barButtonItem17});
            this.configuracionesToolStripMenuItem.Name = "configuracionesToolStripMenuItem";
            this.configuracionesToolStripMenuItem.Size = new System.Drawing.Size(106, 20);
            this.configuracionesToolStripMenuItem.Text = "Configuraciones";
            // 
            // barButtonItem12
            // 
            this.barButtonItem12.Image = global::TCPSmart.Properties.Resources.DatabaseUnknown;
            this.barButtonItem12.Name = "barButtonItem12";
            this.barButtonItem12.Size = new System.Drawing.Size(152, 22);
            this.barButtonItem12.Text = "Parametros DB";
            this.barButtonItem12.Click += new System.EventHandler(this.barButtonItem12_Click);
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Image = global::TCPSmart.Properties.Resources.SettingsGroup;
            this.barButtonItem1.Name = "barButtonItem1";
            this.barButtonItem1.Size = new System.Drawing.Size(152, 22);
            this.barButtonItem1.Text = "Parametros";
            this.barButtonItem1.Click += new System.EventHandler(this.barButtonItem1_Click);
            // 
            // barButtonItem13
            // 
            this.barButtonItem13.Image = global::TCPSmart.Properties.Resources.Network;
            this.barButtonItem13.Name = "barButtonItem13";
            this.barButtonItem13.Size = new System.Drawing.Size(152, 22);
            this.barButtonItem13.Text = "Net Manager";
            this.barButtonItem13.Click += new System.EventHandler(this.barButtonItem13_Click);
            // 
            // barButtonItem17
            // 
            this.barButtonItem17.Image = global::TCPSmart.Properties.Resources.WebProperty;
            this.barButtonItem17.Name = "barButtonItem17";
            this.barButtonItem17.Size = new System.Drawing.Size(152, 22);
            this.barButtonItem17.Text = "Ws Manager";
            this.barButtonItem17.Click += new System.EventHandler(this.barButtonItem17_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.barStaticItem1,
            this.barStaticItem2,
            this.barButtonItem15,
            this.barButtonItem9});
            this.statusStrip1.Location = new System.Drawing.Point(0, 414);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(877, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // barStaticItem1
            // 
            this.barStaticItem1.Image = global::TCPSmart.Properties.Resources.Statistics;
            this.barStaticItem1.Name = "barStaticItem1";
            this.barStaticItem1.Size = new System.Drawing.Size(58, 17);
            this.barStaticItem1.Text = "Status:";
            // 
            // barStaticItem2
            // 
            this.barStaticItem2.Name = "barStaticItem2";
            this.barStaticItem2.Size = new System.Drawing.Size(0, 17);
            // 
            // barButtonItem15
            // 
            this.barButtonItem15.Image = global::TCPSmart.Properties.Resources.RefreshScript;
            this.barButtonItem15.IsLink = true;
            this.barButtonItem15.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.barButtonItem15.Name = "barButtonItem15";
            this.barButtonItem15.Size = new System.Drawing.Size(73, 17);
            this.barButtonItem15.Text = "Clear Log";
            this.barButtonItem15.Click += new System.EventHandler(this.barButtonItem15_Click);
            // 
            // barButtonItem9
            // 
            this.barButtonItem9.Image = global::TCPSmart.Properties.Resources.DatabaseColumn;
            this.barButtonItem9.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.barButtonItem9.IsLink = true;
            this.barButtonItem9.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.barButtonItem9.Name = "barButtonItem9";
            this.barButtonItem9.Size = new System.Drawing.Size(731, 17);
            this.barButtonItem9.Spring = true;
            this.barButtonItem9.Text = "Bitacora";
            this.barButtonItem9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.barButtonItem9.Click += new System.EventHandler(this.barButtonItem9_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.gridView1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.LstLog);
            this.splitContainer1.Size = new System.Drawing.Size(877, 390);
            this.splitContainer1.SplitterDistance = 292;
            this.splitContainer1.TabIndex = 3;
            // 
            // gridView1
            // 
            this.gridView1.AllowUserToAddRows = false;
            this.gridView1.AllowUserToDeleteRows = false;
            this.gridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridView1.BackgroundColor = System.Drawing.SystemColors.ButtonFace;
            this.gridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ID,
            this.NetManagerID,
            this.Nombre,
            this.Status,
            this.Inicio,
            this.UpTime,
            this.Recibidas,
            this.Enviadas,
            this.Tipo});
            this.gridView1.ContextMenuStrip = this.contextMenuStrip1;
            this.gridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridView1.Location = new System.Drawing.Point(0, 0);
            this.gridView1.MultiSelect = false;
            this.gridView1.Name = "gridView1";
            this.gridView1.ReadOnly = true;
            this.gridView1.Size = new System.Drawing.Size(877, 292);
            this.gridView1.TabIndex = 2;
            this.gridView1.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridView1_RowEnter);
            this.gridView1.SelectionChanged += new System.EventHandler(this.gridView1_SelectionChanged);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Enabled = false;
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.BtnOpenLog,
            this.BtnDebugOn,
            this.BtnDebugOff,
            this.BtnSingOn,
            this.BtnEndSon});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(181, 136);
            // 
            // BtnOpenLog
            // 
            this.BtnOpenLog.Enabled = false;
            this.BtnOpenLog.Image = global::TCPSmart.Properties.Resources.DatabaseColumn;
            this.BtnOpenLog.Name = "BtnOpenLog";
            this.BtnOpenLog.Size = new System.Drawing.Size(180, 22);
            this.BtnOpenLog.Text = "Bitacora";
            this.BtnOpenLog.Visible = false;
            this.BtnOpenLog.Click += new System.EventHandler(this.BtnOpenLog_Click);
            // 
            // BtnDebugOn
            // 
            this.BtnDebugOn.Enabled = false;
            this.BtnDebugOn.Name = "BtnDebugOn";
            this.BtnDebugOn.Size = new System.Drawing.Size(180, 22);
            this.BtnDebugOn.Text = "Activar Debug";
            this.BtnDebugOn.Visible = false;
            this.BtnDebugOn.Click += new System.EventHandler(this.btsModoDebug_Click);
            // 
            // BtnDebugOff
            // 
            this.BtnDebugOff.Enabled = false;
            this.BtnDebugOff.Name = "BtnDebugOff";
            this.BtnDebugOff.Size = new System.Drawing.Size(180, 22);
            this.BtnDebugOff.Text = "Desactivar Debug";
            this.BtnDebugOff.Visible = false;
            this.BtnDebugOff.Click += new System.EventHandler(this.BtnDebugOff_Click);
            // 
            // BtnSingOn
            // 
            this.BtnSingOn.Name = "BtnSingOn";
            this.BtnSingOn.Size = new System.Drawing.Size(166, 22);
            this.BtnSingOn.Text = "Enviar Sign On";
            this.BtnSingOn.Visible = false;
            this.BtnSingOn.Click += new System.EventHandler(this.BtnSingOn_Click);
            // 
            // BtnEndSon
            // 
            this.BtnEndSon.Name = "BtnEndSon";
            this.BtnEndSon.Size = new System.Drawing.Size(166, 22);
            this.BtnEndSon.Text = "Termnar Sign On";
            this.BtnEndSon.Visible = false;
            this.BtnEndSon.Click += new System.EventHandler(this.BtnEndSon_Click);
            // 
            // LstLog
            // 
            this.LstLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LstLog.FormattingEnabled = true;
            this.LstLog.Location = new System.Drawing.Point(0, 0);
            this.LstLog.Name = "LstLog";
            this.LstLog.Size = new System.Drawing.Size(877, 94);
            this.LstLog.TabIndex = 0;
            this.LstLog.KeyDown += new System.Windows.Forms.KeyEventHandler(this.LstLog_KeyDown);
            // 
            // operacionesToolStripMenuItem
            // 
            this.operacionesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.activarDebugToolStripMenuItem,
            this.desactivarDebugToolStripMenuItem});
            this.operacionesToolStripMenuItem.Enabled = false;
            this.operacionesToolStripMenuItem.Name = "operacionesToolStripMenuItem";
            this.operacionesToolStripMenuItem.Size = new System.Drawing.Size(85, 20);
            this.operacionesToolStripMenuItem.Text = "Operaciones";
            // 
            // activarDebugToolStripMenuItem
            // 
            this.activarDebugToolStripMenuItem.Name = "activarDebugToolStripMenuItem";
            this.activarDebugToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.activarDebugToolStripMenuItem.Text = "Activar Debug";
            this.activarDebugToolStripMenuItem.Click += new System.EventHandler(this.activarDebugToolStripMenuItem_Click);
            // 
            // desactivarDebugToolStripMenuItem
            // 
            this.desactivarDebugToolStripMenuItem.Name = "desactivarDebugToolStripMenuItem";
            this.desactivarDebugToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.desactivarDebugToolStripMenuItem.Text = "Desactivar Debug";
            this.desactivarDebugToolStripMenuItem.Click += new System.EventHandler(this.desactivarDebugToolStripMenuItem_Click);
            // 
            // ID
            // 
            this.ID.DataPropertyName = "ID";
            this.ID.FillWeight = 44.99797F;
            this.ID.HeaderText = "ID";
            this.ID.Name = "ID";
            this.ID.ReadOnly = true;
            // 
            // NetManagerID
            // 
            this.NetManagerID.DataPropertyName = "NetManagerID";
            this.NetManagerID.HeaderText = "NetManagerID";
            this.NetManagerID.Name = "NetManagerID";
            this.NetManagerID.ReadOnly = true;
            this.NetManagerID.Visible = false;
            // 
            // Nombre
            // 
            this.Nombre.DataPropertyName = "Nombre";
            this.Nombre.FillWeight = 44.99797F;
            this.Nombre.HeaderText = "Nombre";
            this.Nombre.Name = "Nombre";
            this.Nombre.ReadOnly = true;
            // 
            // Status
            // 
            this.Status.DataPropertyName = "Status";
            this.Status.FillWeight = 44.99797F;
            this.Status.HeaderText = "Status";
            this.Status.Name = "Status";
            this.Status.ReadOnly = true;
            // 
            // Inicio
            // 
            this.Inicio.DataPropertyName = "Inicio";
            this.Inicio.FillWeight = 44.99797F;
            this.Inicio.HeaderText = "Inicio";
            this.Inicio.Name = "Inicio";
            this.Inicio.ReadOnly = true;
            // 
            // UpTime
            // 
            this.UpTime.DataPropertyName = "UpTime";
            this.UpTime.FillWeight = 44.99797F;
            this.UpTime.HeaderText = "UpTime";
            this.UpTime.Name = "UpTime";
            this.UpTime.ReadOnly = true;
            // 
            // Recibidas
            // 
            this.Recibidas.DataPropertyName = "Recibidas";
            this.Recibidas.FillWeight = 44.99797F;
            this.Recibidas.HeaderText = "Recibidas";
            this.Recibidas.Name = "Recibidas";
            this.Recibidas.ReadOnly = true;
            // 
            // Enviadas
            // 
            this.Enviadas.DataPropertyName = "Enviadas";
            this.Enviadas.FillWeight = 44.99797F;
            this.Enviadas.HeaderText = "Enviadas";
            this.Enviadas.Name = "Enviadas";
            this.Enviadas.ReadOnly = true;
            // 
            // Tipo
            // 
            this.Tipo.DataPropertyName = "Tipo";
            this.Tipo.FillWeight = 66.27739F;
            this.Tipo.HeaderText = "Tipo";
            this.Tipo.Name = "Tipo";
            this.Tipo.ReadOnly = true;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(877, 436);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TCPSmart";
            this.Load += new System.EventHandler(this.Main_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem barSubItem1;
        private System.Windows.Forms.ToolStripMenuItem bsiMantenimiento;
        private System.Windows.Forms.ToolStripMenuItem configuracionesToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripMenuItem barButtonItem2;
        private System.Windows.Forms.ToolStripMenuItem barButtonItem3;
        private System.Windows.Forms.ToolStripMenuItem btsModoDebug;
        private System.Windows.Forms.ToolStripMenuItem barButtonItem14;
        private System.Windows.Forms.ToolStripMenuItem barButtonItem12;
        private System.Windows.Forms.ToolStripMenuItem barButtonItem1;
        private System.Windows.Forms.ToolStripMenuItem barButtonItem13;
        private System.Windows.Forms.ToolStripMenuItem barButtonItem17;
        private System.Windows.Forms.ToolStripStatusLabel barStaticItem1;
        private System.Windows.Forms.ToolStripStatusLabel barStaticItem2;
        private System.Windows.Forms.ToolStripStatusLabel barButtonItem15;
        private System.Windows.Forms.ToolStripStatusLabel barButtonItem9;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView gridView1;
        private System.Windows.Forms.ListBox LstLog;
        private System.Windows.Forms.ToolStripMenuItem finalizarAplicacionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem BtnOpenLog;
        private System.Windows.Forms.ToolStripMenuItem BtnDebugOn;
        private System.Windows.Forms.ToolStripMenuItem BtnDebugOff;
        private System.Windows.Forms.ToolStripMenuItem BtnSingOn;
        private System.Windows.Forms.ToolStripMenuItem BtnEndSon;
        private System.Windows.Forms.ToolStripMenuItem operacionesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem activarDebugToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem desactivarDebugToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn NetManagerID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Nombre;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
        private System.Windows.Forms.DataGridViewTextBoxColumn Inicio;
        private System.Windows.Forms.DataGridViewTextBoxColumn UpTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn Recibidas;
        private System.Windows.Forms.DataGridViewTextBoxColumn Enviadas;
        private System.Windows.Forms.DataGridViewTextBoxColumn Tipo;
    }
}