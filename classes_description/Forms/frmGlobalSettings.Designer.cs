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
            this.tabCloud = new System.Windows.Forms.TabPage();
            this.label3 = new System.Windows.Forms.Label();
            this.tbWdLogin = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbWdUrl = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSave = new System.Windows.Forms.Button();
            this.lbGroups = new System.Windows.Forms.ListBox();
            this.tbWdPassword = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cbWdRemoteDir = new System.Windows.Forms.ComboBox();
            this.btnWdTest = new System.Windows.Forms.Button();
            this.tbWdToken = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.lbWdLoading = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabCloud.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabGeneral);
            this.tabControl1.Controls.Add(this.tabCloud);
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
            // tabCloud
            // 
            this.tabCloud.Controls.Add(this.label6);
            this.tabCloud.Controls.Add(this.lbWdLoading);
            this.tabCloud.Controls.Add(this.tbWdToken);
            this.tabCloud.Controls.Add(this.label5);
            this.tabCloud.Controls.Add(this.btnWdTest);
            this.tabCloud.Controls.Add(this.cbWdRemoteDir);
            this.tabCloud.Controls.Add(this.tbWdPassword);
            this.tabCloud.Controls.Add(this.label4);
            this.tabCloud.Controls.Add(this.label3);
            this.tabCloud.Controls.Add(this.tbWdLogin);
            this.tabCloud.Controls.Add(this.label2);
            this.tabCloud.Controls.Add(this.tbWdUrl);
            this.tabCloud.Controls.Add(this.label1);
            this.tabCloud.Location = new System.Drawing.Point(4, 22);
            this.tabCloud.Name = "tabCloud";
            this.tabCloud.Padding = new System.Windows.Forms.Padding(3);
            this.tabCloud.Size = new System.Drawing.Size(555, 366);
            this.tabCloud.TabIndex = 1;
            this.tabCloud.Text = "Облако";
            this.tabCloud.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 228);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(108, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Удаленный каталог";
            // 
            // tbWdLogin
            // 
            this.tbWdLogin.Location = new System.Drawing.Point(10, 85);
            this.tbWdLogin.Name = "tbWdLogin";
            this.tbWdLogin.Size = new System.Drawing.Size(269, 20);
            this.tbWdLogin.TabIndex = 3;
            this.tbWdLogin.Text = "eziksoft@yandex.ru";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Логин";
            // 
            // tbWdUrl
            // 
            this.tbWdUrl.Location = new System.Drawing.Point(10, 35);
            this.tbWdUrl.Name = "tbWdUrl";
            this.tbWdUrl.Size = new System.Drawing.Size(537, 20);
            this.tbWdUrl.TabIndex = 1;
            this.tbWdUrl.Text = "https://webdav.yandex.ru";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Сервер";
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
            // 
            // lbGroups
            // 
            this.lbGroups.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbGroups.FormattingEnabled = true;
            this.lbGroups.Items.AddRange(new object[] {
            "Общие",
            "Облако"});
            this.lbGroups.Location = new System.Drawing.Point(0, 0);
            this.lbGroups.Name = "lbGroups";
            this.lbGroups.Size = new System.Drawing.Size(169, 342);
            this.lbGroups.TabIndex = 0;
            this.lbGroups.SelectedIndexChanged += new System.EventHandler(this.lbGroups_SelectedIndexChanged);
            // 
            // tbWdPassword
            // 
            this.tbWdPassword.Location = new System.Drawing.Point(12, 138);
            this.tbWdPassword.Name = "tbWdPassword";
            this.tbWdPassword.Size = new System.Drawing.Size(267, 20);
            this.tbWdPassword.TabIndex = 7;
            this.tbWdPassword.Text = "wzumrfkurjqhyyfz";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 121);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Пароль";
            // 
            // cbWdRemoteDir
            // 
            this.cbWdRemoteDir.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWdRemoteDir.FormattingEnabled = true;
            this.cbWdRemoteDir.Location = new System.Drawing.Point(12, 244);
            this.cbWdRemoteDir.Name = "cbWdRemoteDir";
            this.cbWdRemoteDir.Size = new System.Drawing.Size(537, 21);
            this.cbWdRemoteDir.TabIndex = 8;
            this.cbWdRemoteDir.DropDown += new System.EventHandler(this.cbWdRemoteDir_DropDown);
            // 
            // btnWdTest
            // 
            this.btnWdTest.Location = new System.Drawing.Point(402, 297);
            this.btnWdTest.Name = "btnWdTest";
            this.btnWdTest.Size = new System.Drawing.Size(145, 23);
            this.btnWdTest.TabIndex = 9;
            this.btnWdTest.Text = "Тест";
            this.btnWdTest.UseVisualStyleBackColor = true;
            this.btnWdTest.Click += new System.EventHandler(this.btnWdTest_Click);
            // 
            // tbWdToken
            // 
            this.tbWdToken.Location = new System.Drawing.Point(12, 191);
            this.tbWdToken.Name = "tbWdToken";
            this.tbWdToken.Size = new System.Drawing.Size(267, 20);
            this.tbWdToken.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 174);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(38, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Токен";
            // 
            // lbWdLoading
            // 
            this.lbWdLoading.AutoSize = true;
            this.lbWdLoading.ForeColor = System.Drawing.Color.Blue;
            this.lbWdLoading.Location = new System.Drawing.Point(484, 228);
            this.lbWdLoading.Name = "lbWdLoading";
            this.lbWdLoading.Size = new System.Drawing.Size(63, 13);
            this.lbWdLoading.TabIndex = 12;
            this.lbWdLoading.Text = "Загрузка...";
            this.lbWdLoading.Visible = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.Blue;
            this.label6.Location = new System.Drawing.Point(441, 323);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(64, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "Uploading...";
            this.label6.Visible = false;
            // 
            // frmGlobalSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(732, 392);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmGlobalSettings";
            this.Text = "Настройки программы";
            this.tabControl1.ResumeLayout(false);
            this.tabCloud.ResumeLayout(false);
            this.tabCloud.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabGeneral;
        private System.Windows.Forms.TabPage tabCloud;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.ListBox lbGroups;
        private System.Windows.Forms.TextBox tbWdLogin;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbWdUrl;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbWdPassword;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbWdRemoteDir;
        private System.Windows.Forms.Button btnWdTest;
        private System.Windows.Forms.TextBox tbWdToken;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lbWdLoading;
        private System.Windows.Forms.Label label6;
    }
}