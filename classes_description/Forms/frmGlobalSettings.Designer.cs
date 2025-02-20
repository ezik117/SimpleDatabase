namespace simple_database
{
    partial class frmGlobalSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmGlobalSettings));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabGeneral = new System.Windows.Forms.TabPage();
            this.tabGoogleDrive = new System.Windows.Forms.TabPage();
            this.cbGdCheckUpdatesOnStart = new System.Windows.Forms.CheckBox();
            this.cbGdActivate = new System.Windows.Forms.CheckBox();
            this.lbGdLoading = new System.Windows.Forms.Label();
            this.cbGdRemoteDir = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbGdSAKey = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSave = new System.Windows.Forms.Button();
            this.lbGroups = new System.Windows.Forms.ListBox();
            this.tabControl1.SuspendLayout();
            this.tabGoogleDrive.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabGeneral);
            this.tabControl1.Controls.Add(this.tabGoogleDrive);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(169, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(563, 392);
            this.tabControl1.TabIndex = 0;
            // 
            // tabGeneral
            // 
            this.tabGeneral.AccessibleName = "";
            this.tabGeneral.Location = new System.Drawing.Point(4, 22);
            this.tabGeneral.Name = "tabGeneral";
            this.tabGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tabGeneral.Size = new System.Drawing.Size(555, 366);
            this.tabGeneral.TabIndex = 0;
            this.tabGeneral.Text = "Общие";
            this.tabGeneral.UseVisualStyleBackColor = true;
            // 
            // tabGoogleDrive
            // 
            this.tabGoogleDrive.Controls.Add(this.cbGdCheckUpdatesOnStart);
            this.tabGoogleDrive.Controls.Add(this.cbGdActivate);
            this.tabGoogleDrive.Controls.Add(this.lbGdLoading);
            this.tabGoogleDrive.Controls.Add(this.cbGdRemoteDir);
            this.tabGoogleDrive.Controls.Add(this.label3);
            this.tabGoogleDrive.Controls.Add(this.tbGdSAKey);
            this.tabGoogleDrive.Controls.Add(this.label1);
            this.tabGoogleDrive.Location = new System.Drawing.Point(4, 22);
            this.tabGoogleDrive.Name = "tabGoogleDrive";
            this.tabGoogleDrive.Padding = new System.Windows.Forms.Padding(3);
            this.tabGoogleDrive.Size = new System.Drawing.Size(555, 366);
            this.tabGoogleDrive.TabIndex = 1;
            this.tabGoogleDrive.Text = "Google Drive";
            this.tabGoogleDrive.UseVisualStyleBackColor = true;
            // 
            // cbGdCheckUpdatesOnStart
            // 
            this.cbGdCheckUpdatesOnStart.AutoSize = true;
            this.cbGdCheckUpdatesOnStart.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cbGdCheckUpdatesOnStart.Location = new System.Drawing.Point(10, 230);
            this.cbGdCheckUpdatesOnStart.Name = "cbGdCheckUpdatesOnStart";
            this.cbGdCheckUpdatesOnStart.Size = new System.Drawing.Size(293, 17);
            this.cbGdCheckUpdatesOnStart.TabIndex = 15;
            this.cbGdCheckUpdatesOnStart.Text = "Проверять обновления БД при запуске приложения";
            this.cbGdCheckUpdatesOnStart.UseVisualStyleBackColor = true;
            // 
            // cbGdActivate
            // 
            this.cbGdActivate.AutoSize = true;
            this.cbGdActivate.Location = new System.Drawing.Point(12, 12);
            this.cbGdActivate.Name = "cbGdActivate";
            this.cbGdActivate.Size = new System.Drawing.Size(97, 17);
            this.cbGdActivate.TabIndex = 14;
            this.cbGdActivate.Text = "Активировать";
            this.cbGdActivate.UseVisualStyleBackColor = true;
            // 
            // lbGdLoading
            // 
            this.lbGdLoading.AutoSize = true;
            this.lbGdLoading.ForeColor = System.Drawing.Color.Blue;
            this.lbGdLoading.Location = new System.Drawing.Point(482, 162);
            this.lbGdLoading.Name = "lbGdLoading";
            this.lbGdLoading.Size = new System.Drawing.Size(63, 13);
            this.lbGdLoading.TabIndex = 12;
            this.lbGdLoading.Text = "Загрузка...";
            this.lbGdLoading.Visible = false;
            // 
            // cbGdRemoteDir
            // 
            this.cbGdRemoteDir.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbGdRemoteDir.FormattingEnabled = true;
            this.cbGdRemoteDir.Location = new System.Drawing.Point(10, 178);
            this.cbGdRemoteDir.Name = "cbGdRemoteDir";
            this.cbGdRemoteDir.Size = new System.Drawing.Size(537, 21);
            this.cbGdRemoteDir.TabIndex = 8;
            this.cbGdRemoteDir.DropDown += new System.EventHandler(this.cbGdRemoteDir_DropDown);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 162);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(108, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Удаленный каталог";
            // 
            // tbGdSAKey
            // 
            this.tbGdSAKey.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbGdSAKey.Location = new System.Drawing.Point(12, 71);
            this.tbGdSAKey.Multiline = true;
            this.tbGdSAKey.Name = "tbGdSAKey";
            this.tbGdSAKey.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbGdSAKey.Size = new System.Drawing.Size(531, 75);
            this.tbGdSAKey.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(144, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Service Account Key (JSON)";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.lbGroups);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(169, 392);
            this.panel1.TabIndex = 1;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(12, 357);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(145, 23);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Сохранить";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lbGroups
            // 
            this.lbGroups.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbGroups.FormattingEnabled = true;
            this.lbGroups.Items.AddRange(new object[] {
            "Общие",
            "Google Drive"});
            this.lbGroups.Location = new System.Drawing.Point(0, 0);
            this.lbGroups.Name = "lbGroups";
            this.lbGroups.Size = new System.Drawing.Size(169, 342);
            this.lbGroups.TabIndex = 0;
            this.lbGroups.SelectedIndexChanged += new System.EventHandler(this.lbGroups_SelectedIndexChanged);
            // 
            // frmGlobalSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(732, 392);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmGlobalSettings";
            this.Text = "Настройки программы";
            this.tabControl1.ResumeLayout(false);
            this.tabGoogleDrive.ResumeLayout(false);
            this.tabGoogleDrive.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabGeneral;
        private System.Windows.Forms.TabPage tabGoogleDrive;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.ListBox lbGroups;
        private System.Windows.Forms.TextBox tbGdSAKey;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbGdRemoteDir;
        private System.Windows.Forms.Label lbGdLoading;
        private System.Windows.Forms.CheckBox cbGdActivate;
        private System.Windows.Forms.CheckBox cbGdCheckUpdatesOnStart;
    }
}