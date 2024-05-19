namespace simple_database
{
    partial class frmHistory
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmHistory));
            this.dgv = new System.Windows.Forms.DataGridView();
            this.histDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.histUser = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.histTablename = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.histName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.histChange = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.histRowId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.histClassId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.histPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lbInfo = new System.Windows.Forms.ListBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.SuspendLayout();
            // 
            // dgv
            // 
            this.dgv.AllowUserToAddRows = false;
            this.dgv.AllowUserToDeleteRows = false;
            this.dgv.AllowUserToResizeRows = false;
            this.dgv.BackgroundColor = System.Drawing.Color.White;
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.histDate,
            this.histUser,
            this.histTablename,
            this.histName,
            this.histChange,
            this.histRowId,
            this.histClassId,
            this.histPath});
            this.dgv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgv.Location = new System.Drawing.Point(0, 0);
            this.dgv.MultiSelect = false;
            this.dgv.Name = "dgv";
            this.dgv.ReadOnly = true;
            this.dgv.RowHeadersVisible = false;
            this.dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv.Size = new System.Drawing.Size(800, 355);
            this.dgv.TabIndex = 0;
            this.dgv.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgv_DataBindingComplete);
            this.dgv.SelectionChanged += new System.EventHandler(this.dgv_SelectionChanged);
            // 
            // histDate
            // 
            this.histDate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.histDate.DataPropertyName = "date";
            this.histDate.HeaderText = "Дата";
            this.histDate.Name = "histDate";
            this.histDate.ReadOnly = true;
            this.histDate.Width = 58;
            // 
            // histUser
            // 
            this.histUser.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.histUser.DataPropertyName = "user";
            this.histUser.HeaderText = "Пользователь";
            this.histUser.Name = "histUser";
            this.histUser.ReadOnly = true;
            this.histUser.Width = 105;
            // 
            // histTablename
            // 
            this.histTablename.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.histTablename.DataPropertyName = "tablename";
            this.histTablename.HeaderText = "Имя таблицы";
            this.histTablename.Name = "histTablename";
            this.histTablename.ReadOnly = true;
            // 
            // histName
            // 
            this.histName.DataPropertyName = "name";
            this.histName.HeaderText = "Элемент";
            this.histName.Name = "histName";
            this.histName.ReadOnly = true;
            // 
            // histChange
            // 
            this.histChange.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.histChange.DataPropertyName = "change";
            this.histChange.HeaderText = "Изменение";
            this.histChange.Name = "histChange";
            this.histChange.ReadOnly = true;
            // 
            // histRowId
            // 
            this.histRowId.DataPropertyName = "row_id";
            this.histRowId.HeaderText = "row_id";
            this.histRowId.Name = "histRowId";
            this.histRowId.ReadOnly = true;
            this.histRowId.Visible = false;
            // 
            // histClassId
            // 
            this.histClassId.DataPropertyName = "class_id";
            this.histClassId.HeaderText = "class_id";
            this.histClassId.Name = "histClassId";
            this.histClassId.ReadOnly = true;
            this.histClassId.Visible = false;
            // 
            // histPath
            // 
            this.histPath.DataPropertyName = "path";
            this.histPath.HeaderText = "path";
            this.histPath.Name = "histPath";
            this.histPath.ReadOnly = true;
            this.histPath.Visible = false;
            // 
            // lbInfo
            // 
            this.lbInfo.BackColor = System.Drawing.Color.White;
            this.lbInfo.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lbInfo.FormattingEnabled = true;
            this.lbInfo.Location = new System.Drawing.Point(0, 355);
            this.lbInfo.Name = "lbInfo";
            this.lbInfo.Size = new System.Drawing.Size(800, 95);
            this.lbInfo.TabIndex = 1;
            // 
            // frmHistory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dgv);
            this.Controls.Add(this.lbInfo);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmHistory";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "История изменений (Последние ... событий)";
            this.Load += new System.EventHandler(this.frmHistory_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.ListBox lbInfo;
        private System.Windows.Forms.DataGridViewTextBoxColumn histDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn histUser;
        private System.Windows.Forms.DataGridViewTextBoxColumn histTablename;
        private System.Windows.Forms.DataGridViewTextBoxColumn histName;
        private System.Windows.Forms.DataGridViewTextBoxColumn histChange;
        private System.Windows.Forms.DataGridViewTextBoxColumn histRowId;
        private System.Windows.Forms.DataGridViewTextBoxColumn histClassId;
        private System.Windows.Forms.DataGridViewTextBoxColumn histPath;
    }
}