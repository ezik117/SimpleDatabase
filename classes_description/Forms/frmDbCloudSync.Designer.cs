namespace simple_database
{
    partial class frmDbCloudSync
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDbCloudSync));
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSelectDifferent = new System.Windows.Forms.Button();
            this.btnSyncronize = new System.Windows.Forms.Button();
            this.btnUncheckAll = new System.Windows.Forms.Button();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.dgv = new System.Windows.Forms.DataGridView();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.ssStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.Column7 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column12 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.принудительнаяЗагрузкаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnForceUpload = new System.Windows.Forms.ToolStripMenuItem();
            this.btnForceDownload = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnSelectDifferent);
            this.panel1.Controls.Add(this.btnSyncronize);
            this.panel1.Controls.Add(this.btnUncheckAll);
            this.panel1.Controls.Add(this.btnSelectAll);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(805, 45);
            this.panel1.TabIndex = 0;
            // 
            // btnSelectDifferent
            // 
            this.btnSelectDifferent.Location = new System.Drawing.Point(12, 11);
            this.btnSelectDifferent.Name = "btnSelectDifferent";
            this.btnSelectDifferent.Size = new System.Drawing.Size(142, 23);
            this.btnSelectDifferent.TabIndex = 3;
            this.btnSelectDifferent.Text = "Выбрать отличающиеся";
            this.btnSelectDifferent.UseVisualStyleBackColor = true;
            this.btnSelectDifferent.Click += new System.EventHandler(this.btnSelectDifferent_Click);
            // 
            // btnSyncronize
            // 
            this.btnSyncronize.Location = new System.Drawing.Point(452, 12);
            this.btnSyncronize.Name = "btnSyncronize";
            this.btnSyncronize.Size = new System.Drawing.Size(120, 23);
            this.btnSyncronize.TabIndex = 2;
            this.btnSyncronize.Text = "Синхронизировать";
            this.btnSyncronize.UseVisualStyleBackColor = true;
            this.btnSyncronize.Click += new System.EventHandler(this.btnSyncronize_Click);
            // 
            // btnUncheckAll
            // 
            this.btnUncheckAll.Location = new System.Drawing.Point(270, 12);
            this.btnUncheckAll.Name = "btnUncheckAll";
            this.btnUncheckAll.Size = new System.Drawing.Size(104, 23);
            this.btnUncheckAll.TabIndex = 1;
            this.btnUncheckAll.Text = "Сбросить";
            this.btnUncheckAll.UseVisualStyleBackColor = true;
            this.btnUncheckAll.Click += new System.EventHandler(this.btnUncheckAll_Click);
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Location = new System.Drawing.Point(160, 12);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(104, 23);
            this.btnSelectAll.TabIndex = 0;
            this.btnSelectAll.Text = "Выбрать все";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // dgv
            // 
            this.dgv.AllowUserToAddRows = false;
            this.dgv.AllowUserToDeleteRows = false;
            this.dgv.AllowUserToResizeRows = false;
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column7,
            this.Column1,
            this.Column2,
            this.Column5,
            this.Column11,
            this.Column3,
            this.Column4,
            this.Column8,
            this.Column6,
            this.Column9,
            this.Column10,
            this.Column12});
            this.dgv.ContextMenuStrip = this.contextMenuStrip1;
            this.dgv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgv.Location = new System.Drawing.Point(0, 45);
            this.dgv.MultiSelect = false;
            this.dgv.Name = "dgv";
            this.dgv.ReadOnly = true;
            this.dgv.RowHeadersVisible = false;
            this.dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv.Size = new System.Drawing.Size(805, 383);
            this.dgv.TabIndex = 1;
            this.dgv.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_CellClick);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ssStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 428);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(805, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // ssStatus
            // 
            this.ssStatus.Name = "ssStatus";
            this.ssStatus.Size = new System.Drawing.Size(223, 17);
            this.ssStatus.Text = "Получение информации, подождите....";
            // 
            // Column7
            // 
            this.Column7.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.Column7.HeaderText = "Sync";
            this.Column7.Name = "Column7";
            this.Column7.ReadOnly = true;
            this.Column7.Width = 37;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Локальная БД";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 150;
            // 
            // Column2
            // 
            this.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Column2.HeaderText = "Дата";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Width = 58;
            // 
            // Column5
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.Column5.DefaultCellStyle = dataGridViewCellStyle1;
            this.Column5.HeaderText = "Размер";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            // 
            // Column11
            // 
            this.Column11.HeaderText = "";
            this.Column11.Name = "Column11";
            this.Column11.ReadOnly = true;
            this.Column11.Width = 30;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "Облачная БД";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Width = 150;
            // 
            // Column4
            // 
            this.Column4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Column4.HeaderText = "Дата";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.Width = 58;
            // 
            // Column8
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.Column8.DefaultCellStyle = dataGridViewCellStyle2;
            this.Column8.HeaderText = "Размер";
            this.Column8.Name = "Column8";
            this.Column8.ReadOnly = true;
            // 
            // Column6
            // 
            this.Column6.HeaderText = "cloudid";
            this.Column6.Name = "Column6";
            this.Column6.ReadOnly = true;
            this.Column6.Visible = false;
            // 
            // Column9
            // 
            this.Column9.HeaderText = "local real size";
            this.Column9.Name = "Column9";
            this.Column9.ReadOnly = true;
            this.Column9.Visible = false;
            // 
            // Column10
            // 
            this.Column10.HeaderText = "cloud real size";
            this.Column10.Name = "Column10";
            this.Column10.ReadOnly = true;
            this.Column10.Visible = false;
            // 
            // Column12
            // 
            this.Column12.HeaderText = "cloudFileSize";
            this.Column12.Name = "Column12";
            this.Column12.ReadOnly = true;
            this.Column12.Visible = false;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.принудительнаяЗагрузкаToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(216, 26);
            // 
            // принудительнаяЗагрузкаToolStripMenuItem
            // 
            this.принудительнаяЗагрузкаToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnForceUpload,
            this.btnForceDownload});
            this.принудительнаяЗагрузкаToolStripMenuItem.Name = "принудительнаяЗагрузкаToolStripMenuItem";
            this.принудительнаяЗагрузкаToolStripMenuItem.Size = new System.Drawing.Size(215, 22);
            this.принудительнаяЗагрузкаToolStripMenuItem.Text = "Принудительная загрузка";
            // 
            // btnForceUpload
            // 
            this.btnForceUpload.Image = global::simple_database.Properties.Resources.upload_16;
            this.btnForceUpload.Name = "btnForceUpload";
            this.btnForceUpload.Size = new System.Drawing.Size(185, 22);
            this.btnForceUpload.Text = "Загрузить в облако";
            // 
            // btnForceDownload
            // 
            this.btnForceDownload.Image = global::simple_database.Properties.Resources.download_16;
            this.btnForceDownload.Name = "btnForceDownload";
            this.btnForceDownload.Size = new System.Drawing.Size(185, 22);
            this.btnForceDownload.Text = "Загрузить из облака";
            // 
            // frmDbCloudSync
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(805, 450);
            this.Controls.Add(this.dgv);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmDbCloudSync";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Синхронизация баз данных с облаком";
            this.Shown += new System.EventHandler(this.frmDbCloudSync_Shown);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnSelectAll;
        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel ssStatus;
        private System.Windows.Forms.Button btnUncheckAll;
        private System.Windows.Forms.Button btnSyncronize;
        private System.Windows.Forms.Button btnSelectDifferent;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column11;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column8;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column9;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column10;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column12;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem принудительнаяЗагрузкаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem btnForceUpload;
        private System.Windows.Forms.ToolStripMenuItem btnForceDownload;
    }
}