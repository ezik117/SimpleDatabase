namespace simple_database
{
    partial class frmDbManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDbManager));
            this.tbDbName = new System.Windows.Forms.TextBox();
            this.pnlDbNameAction = new System.Windows.Forms.Panel();
            this.dgv = new System.Windows.Forms.DataGridView();
            this.dbmgrIcon = new System.Windows.Forms.DataGridViewImageColumn();
            this.dbmgrFile = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ctxAddIcon = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxSaveIcon = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ctxRemoveIcon = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlHotButtons = new System.Windows.Forms.Panel();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.pnlDbNameAction.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.pnlHotButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbDbName
            // 
            this.tbDbName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbDbName.Location = new System.Drawing.Point(12, 13);
            this.tbDbName.Name = "tbDbName";
            this.tbDbName.Size = new System.Drawing.Size(475, 22);
            this.tbDbName.TabIndex = 2;
            this.tbDbName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbDbName_KeyDown);
            // 
            // pnlDbNameAction
            // 
            this.pnlDbNameAction.Controls.Add(this.tbDbName);
            this.pnlDbNameAction.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlDbNameAction.Location = new System.Drawing.Point(0, 405);
            this.pnlDbNameAction.Name = "pnlDbNameAction";
            this.pnlDbNameAction.Size = new System.Drawing.Size(499, 45);
            this.pnlDbNameAction.TabIndex = 3;
            this.pnlDbNameAction.Visible = false;
            // 
            // dgv
            // 
            this.dgv.AllowUserToAddRows = false;
            this.dgv.AllowUserToDeleteRows = false;
            this.dgv.AllowUserToOrderColumns = true;
            this.dgv.AllowUserToResizeColumns = false;
            this.dgv.AllowUserToResizeRows = false;
            this.dgv.BackgroundColor = System.Drawing.Color.White;
            this.dgv.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv.ColumnHeadersVisible = false;
            this.dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dbmgrIcon,
            this.dbmgrFile});
            this.dgv.ContextMenuStrip = this.contextMenuStrip1;
            this.dgv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgv.Location = new System.Drawing.Point(0, 22);
            this.dgv.MultiSelect = false;
            this.dgv.Name = "dgv";
            this.dgv.ReadOnly = true;
            this.dgv.RowHeadersVisible = false;
            this.dgv.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv.Size = new System.Drawing.Size(499, 383);
            this.dgv.TabIndex = 4;
            this.dgv.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_CellDoubleClick);
            this.dgv.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgv_MouseDown);
            // 
            // dbmgrIcon
            // 
            this.dbmgrIcon.HeaderText = "icon";
            this.dbmgrIcon.MinimumWidth = 20;
            this.dbmgrIcon.Name = "dbmgrIcon";
            this.dbmgrIcon.ReadOnly = true;
            this.dbmgrIcon.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dbmgrIcon.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dbmgrIcon.Width = 30;
            // 
            // dbmgrFile
            // 
            this.dbmgrFile.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dbmgrFile.DefaultCellStyle = dataGridViewCellStyle1;
            this.dbmgrFile.HeaderText = "file";
            this.dbmgrFile.Name = "dbmgrFile";
            this.dbmgrFile.ReadOnly = true;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ctxAddIcon,
            this.ctxSaveIcon,
            this.toolStripSeparator1,
            this.ctxRemoveIcon});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(276, 76);
            // 
            // ctxAddIcon
            // 
            this.ctxAddIcon.Name = "ctxAddIcon";
            this.ctxAddIcon.Size = new System.Drawing.Size(275, 22);
            this.ctxAddIcon.Text = "Добавить пиктограмму (16x16;24x24)";
            this.ctxAddIcon.Click += new System.EventHandler(this.ctxAddIcon_Click);
            // 
            // ctxSaveIcon
            // 
            this.ctxSaveIcon.Name = "ctxSaveIcon";
            this.ctxSaveIcon.Size = new System.Drawing.Size(275, 22);
            this.ctxSaveIcon.Text = "Сохранить пиктограмму в файл";
            this.ctxSaveIcon.Click += new System.EventHandler(this.ctxSaveIcon_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(272, 6);
            // 
            // ctxRemoveIcon
            // 
            this.ctxRemoveIcon.Name = "ctxRemoveIcon";
            this.ctxRemoveIcon.Size = new System.Drawing.Size(275, 22);
            this.ctxRemoveIcon.Text = "Удалить пиктограмму";
            this.ctxRemoveIcon.Click += new System.EventHandler(this.ctxRemoveIcon_Click);
            // 
            // pnlHotButtons
            // 
            this.pnlHotButtons.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.pnlHotButtons.Controls.Add(this.btnAdd);
            this.pnlHotButtons.Controls.Add(this.btnEdit);
            this.pnlHotButtons.Controls.Add(this.btnDelete);
            this.pnlHotButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHotButtons.Location = new System.Drawing.Point(0, 0);
            this.pnlHotButtons.Name = "pnlHotButtons";
            this.pnlHotButtons.Size = new System.Drawing.Size(499, 22);
            this.pnlHotButtons.TabIndex = 5;
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnAdd.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnAdd.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnAdd.FlatAppearance.BorderSize = 0;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.Image = global::simple_database.Properties.Resources.Button_Add_icon_16_grayed;
            this.btnAdd.Location = new System.Drawing.Point(439, 0);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(20, 22);
            this.btnAdd.TabIndex = 6;
            this.btnAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnEdit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnEdit.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnEdit.FlatAppearance.BorderSize = 0;
            this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEdit.Image = global::simple_database.Properties.Resources.Gear_icon_16_grayed;
            this.btnEdit.Location = new System.Drawing.Point(459, 0);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(20, 22);
            this.btnEdit.TabIndex = 5;
            this.btnEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnEdit.UseVisualStyleBackColor = false;
            this.btnEdit.Click += new System.EventHandler(this.btnRename_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnDelete.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnDelete.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnDelete.FlatAppearance.BorderSize = 0;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.Image = global::simple_database.Properties.Resources.Button_Close_icon_16_grayed;
            this.btnDelete.Location = new System.Drawing.Point(479, 0);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(20, 22);
            this.btnDelete.TabIndex = 4;
            this.btnDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // frmDbManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(499, 450);
            this.Controls.Add(this.dgv);
            this.Controls.Add(this.pnlHotButtons);
            this.Controls.Add(this.pnlDbNameAction);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmDbManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Менеджер баз данных";
            this.Load += new System.EventHandler(this.frmDbManager_Load);
            this.pnlDbNameAction.ResumeLayout(false);
            this.pnlDbNameAction.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.pnlHotButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TextBox tbDbName;
        private System.Windows.Forms.Panel pnlDbNameAction;
        public System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ctxAddIcon;
        private System.Windows.Forms.ToolStripMenuItem ctxRemoveIcon;
        private System.Windows.Forms.ToolStripMenuItem ctxSaveIcon;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.DataGridViewImageColumn dbmgrIcon;
        private System.Windows.Forms.DataGridViewTextBoxColumn dbmgrFile;
        private System.Windows.Forms.Panel pnlHotButtons;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;
    }
}