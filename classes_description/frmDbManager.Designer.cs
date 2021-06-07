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
            this.lb = new System.Windows.Forms.ListBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnRename = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnCreate = new System.Windows.Forms.Button();
            this.btnOpen = new System.Windows.Forms.Button();
            this.tbDbName = new System.Windows.Forms.TextBox();
            this.pnlDbNameAction = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.pnlDbNameAction.SuspendLayout();
            this.SuspendLayout();
            // 
            // lb
            // 
            this.lb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb.FormattingEnabled = true;
            this.lb.Items.AddRange(new object[] {
            "default"});
            this.lb.Location = new System.Drawing.Point(0, 0);
            this.lb.Name = "lb";
            this.lb.Size = new System.Drawing.Size(499, 397);
            this.lb.TabIndex = 0;
            this.lb.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lb_MouseDown);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnRename);
            this.panel1.Controls.Add(this.btnDelete);
            this.panel1.Controls.Add(this.btnCreate);
            this.panel1.Controls.Add(this.btnOpen);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 397);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(499, 53);
            this.panel1.TabIndex = 1;
            // 
            // btnRename
            // 
            this.btnRename.Location = new System.Drawing.Point(256, 16);
            this.btnRename.Name = "btnRename";
            this.btnRename.Size = new System.Drawing.Size(106, 23);
            this.btnRename.TabIndex = 3;
            this.btnRename.Text = "Переименовать";
            this.btnRename.UseVisualStyleBackColor = true;
            this.btnRename.Click += new System.EventHandler(this.btnRename_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(412, 16);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 2;
            this.btnDelete.Text = "Удалить";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnCreate
            // 
            this.btnCreate.Location = new System.Drawing.Point(144, 16);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(106, 23);
            this.btnCreate.TabIndex = 1;
            this.btnCreate.Text = "Создать...";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // btnOpen
            // 
            this.btnOpen.Location = new System.Drawing.Point(12, 16);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(126, 23);
            this.btnOpen.TabIndex = 0;
            this.btnOpen.Text = "Открыть";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // tbDbName
            // 
            this.tbDbName.Location = new System.Drawing.Point(12, 13);
            this.tbDbName.Name = "tbDbName";
            this.tbDbName.Size = new System.Drawing.Size(475, 20);
            this.tbDbName.TabIndex = 2;
            this.tbDbName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbDbName_KeyDown);
            // 
            // pnlDbNameAction
            // 
            this.pnlDbNameAction.Controls.Add(this.tbDbName);
            this.pnlDbNameAction.Location = new System.Drawing.Point(0, 352);
            this.pnlDbNameAction.Name = "pnlDbNameAction";
            this.pnlDbNameAction.Size = new System.Drawing.Size(499, 45);
            this.pnlDbNameAction.TabIndex = 3;
            this.pnlDbNameAction.Visible = false;
            // 
            // frmDbManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(499, 450);
            this.Controls.Add(this.pnlDbNameAction);
            this.Controls.Add(this.lb);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmDbManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Менеджер баз данных";
            this.Load += new System.EventHandler(this.frmDbManager_Load);
            this.panel1.ResumeLayout(false);
            this.pnlDbNameAction.ResumeLayout(false);
            this.pnlDbNameAction.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.ListBox lb;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.Button btnRename;
        private System.Windows.Forms.TextBox tbDbName;
        private System.Windows.Forms.Panel pnlDbNameAction;
    }
}