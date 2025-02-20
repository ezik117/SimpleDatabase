namespace simple_database
{
    partial class frmStatusedProgressBar
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
            this.lblCurrentAction = new System.Windows.Forms.Label();
            this.pb0 = new System.Windows.Forms.ProgressBar();
            this.pb1 = new System.Windows.Forms.ProgressBar();
            this.btnAbort = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblCurrentAction
            // 
            this.lblCurrentAction.AutoSize = true;
            this.lblCurrentAction.Location = new System.Drawing.Point(14, 17);
            this.lblCurrentAction.Name = "lblCurrentAction";
            this.lblCurrentAction.Size = new System.Drawing.Size(10, 13);
            this.lblCurrentAction.TabIndex = 0;
            this.lblCurrentAction.Text = "-";
            // 
            // pb0
            // 
            this.pb0.Location = new System.Drawing.Point(17, 33);
            this.pb0.Name = "pb0";
            this.pb0.Size = new System.Drawing.Size(460, 23);
            this.pb0.TabIndex = 1;
            // 
            // pb1
            // 
            this.pb1.Location = new System.Drawing.Point(17, 62);
            this.pb1.Name = "pb1";
            this.pb1.Size = new System.Drawing.Size(460, 23);
            this.pb1.TabIndex = 2;
            // 
            // btnAbort
            // 
            this.btnAbort.Location = new System.Drawing.Point(201, 110);
            this.btnAbort.Name = "btnAbort";
            this.btnAbort.Size = new System.Drawing.Size(75, 23);
            this.btnAbort.TabIndex = 3;
            this.btnAbort.Text = "Отмена";
            this.btnAbort.UseVisualStyleBackColor = true;
            this.btnAbort.Click += new System.EventHandler(this.btnAbort_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnAbort);
            this.groupBox1.Controls.Add(this.pb1);
            this.groupBox1.Controls.Add(this.pb0);
            this.groupBox1.Controls.Add(this.lblCurrentAction);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(483, 153);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            // 
            // frmStatusedProgressBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(507, 179);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmStatusedProgressBar";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmStatusedProgressBar_FormClosing);
            this.Shown += new System.EventHandler(this.frmStatusedProgressBar_Shown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Label lblCurrentAction;
        public System.Windows.Forms.ProgressBar pb0;
        public System.Windows.Forms.ProgressBar pb1;
        private System.Windows.Forms.Button btnAbort;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}