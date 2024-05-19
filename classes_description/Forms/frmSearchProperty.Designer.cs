namespace simple_database
{
    partial class frmSearchProperty
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSearchProperty));
            this.tbSearchPattern = new System.Windows.Forms.TextBox();
            this.rbSearchInContents = new System.Windows.Forms.RadioButton();
            this.rbSearchInTexts = new System.Windows.Forms.RadioButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.rbSearchInTags = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.lblBuildKeywords = new System.Windows.Forms.LinkLabel();
            this.btnFindNext = new System.Windows.Forms.Button();
            this.btnFindPrevious = new System.Windows.Forms.Button();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbSearchPattern
            // 
            this.tbSearchPattern.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbSearchPattern.Location = new System.Drawing.Point(12, 17);
            this.tbSearchPattern.Name = "tbSearchPattern";
            this.tbSearchPattern.Size = new System.Drawing.Size(299, 23);
            this.tbSearchPattern.TabIndex = 0;
            // 
            // rbSearchInContents
            // 
            this.rbSearchInContents.AutoSize = true;
            this.rbSearchInContents.Checked = true;
            this.rbSearchInContents.Location = new System.Drawing.Point(47, 70);
            this.rbSearchInContents.Name = "rbSearchInContents";
            this.rbSearchInContents.Size = new System.Drawing.Size(131, 17);
            this.rbSearchInContents.TabIndex = 2;
            this.rbSearchInContents.TabStop = true;
            this.rbSearchInContents.Text = "искать в оглавлении";
            this.rbSearchInContents.UseVisualStyleBackColor = true;
            // 
            // rbSearchInTexts
            // 
            this.rbSearchInTexts.AutoSize = true;
            this.rbSearchInTexts.Location = new System.Drawing.Point(47, 93);
            this.rbSearchInTexts.Name = "rbSearchInTexts";
            this.rbSearchInTexts.Size = new System.Drawing.Size(106, 17);
            this.rbSearchInTexts.TabIndex = 3;
            this.rbSearchInTexts.Text = "искать в тексте";
            this.rbSearchInTexts.UseVisualStyleBackColor = true;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 148);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(403, 22);
            this.statusStrip1.TabIndex = 4;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(52, 17);
            this.lblStatus.Text = "lblStatus";
            // 
            // rbSearchInTags
            // 
            this.rbSearchInTags.AutoSize = true;
            this.rbSearchInTags.Location = new System.Drawing.Point(47, 116);
            this.rbSearchInTags.Name = "rbSearchInTags";
            this.rbSearchInTags.Size = new System.Drawing.Size(160, 17);
            this.rbSearchInTags.TabIndex = 5;
            this.rbSearchInTags.Text = "искать в ключевых словах";
            this.toolTip1.SetToolTip(this.rbSearchInTags, "Перечислите части ключевых слов через точку с запятой");
            this.rbSearchInTags.UseVisualStyleBackColor = true;
            this.rbSearchInTags.CheckedChanged += new System.EventHandler(this.rbSearchInTags_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Место поиска:";
            // 
            // lblBuildKeywords
            // 
            this.lblBuildKeywords.AutoSize = true;
            this.lblBuildKeywords.Location = new System.Drawing.Point(213, 118);
            this.lblBuildKeywords.Name = "lblBuildKeywords";
            this.lblBuildKeywords.Size = new System.Drawing.Size(60, 13);
            this.lblBuildKeywords.TabIndex = 7;
            this.lblBuildKeywords.TabStop = true;
            this.lblBuildKeywords.Text = "подобрать";
            this.lblBuildKeywords.Visible = false;
            this.lblBuildKeywords.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llblBuildKeywords_LinkClicked);
            // 
            // btnFindNext
            // 
            this.btnFindNext.Image = global::simple_database.Properties.Resources.arrow_down_16;
            this.btnFindNext.Location = new System.Drawing.Point(317, 15);
            this.btnFindNext.Name = "btnFindNext";
            this.btnFindNext.Size = new System.Drawing.Size(32, 25);
            this.btnFindNext.TabIndex = 1;
            this.btnFindNext.UseVisualStyleBackColor = true;
            this.btnFindNext.Click += new System.EventHandler(this.btnFindNext_Click);
            // 
            // btnFindPrevious
            // 
            this.btnFindPrevious.Image = global::simple_database.Properties.Resources.arrow_up_16;
            this.btnFindPrevious.Location = new System.Drawing.Point(355, 15);
            this.btnFindPrevious.Name = "btnFindPrevious";
            this.btnFindPrevious.Size = new System.Drawing.Size(32, 25);
            this.btnFindPrevious.TabIndex = 8;
            this.btnFindPrevious.UseVisualStyleBackColor = true;
            this.btnFindPrevious.Click += new System.EventHandler(this.btnFindPrevious_Click);
            // 
            // frmSearchProperty
            // 
            this.AcceptButton = this.btnFindNext;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(403, 170);
            this.Controls.Add(this.btnFindPrevious);
            this.Controls.Add(this.lblBuildKeywords);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.rbSearchInTags);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.rbSearchInTexts);
            this.Controls.Add(this.rbSearchInContents);
            this.Controls.Add(this.tbSearchPattern);
            this.Controls.Add(this.btnFindNext);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "frmSearchProperty";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Поиск";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmSearchProperty_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmSearchProperty_KeyDown);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnFindNext;
        private System.Windows.Forms.TextBox tbSearchPattern;
        private System.Windows.Forms.RadioButton rbSearchInContents;
        private System.Windows.Forms.RadioButton rbSearchInTexts;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.RadioButton rbSearchInTags;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.LinkLabel lblBuildKeywords;
        private System.Windows.Forms.Button btnFindPrevious;
    }
}