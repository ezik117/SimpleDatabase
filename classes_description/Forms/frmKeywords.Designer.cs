namespace simple_database
{
    partial class frmKeywords
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmKeywords));
            this.lbKeyWords = new System.Windows.Forms.ListBox();
            this.pnlHotButtons = new System.Windows.Forms.Panel();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.pnlMoveButtons = new System.Windows.Forms.Panel();
            this.btnRemoveAllKeywords = new System.Windows.Forms.Button();
            this.btnRemoveKeyword = new System.Windows.Forms.Button();
            this.btnAppendKeyword = new System.Windows.Forms.Button();
            this.lbAvailableKeywords = new System.Windows.Forms.ListBox();
            this.pnlAvailableKeywords = new System.Windows.Forms.Panel();
            this.tbFastSearch = new System.Windows.Forms.TextBox();
            this.pnlHotButtons.SuspendLayout();
            this.pnlMoveButtons.SuspendLayout();
            this.pnlAvailableKeywords.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbKeyWords
            // 
            this.lbKeyWords.Dock = System.Windows.Forms.DockStyle.Left;
            this.lbKeyWords.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbKeyWords.FormattingEnabled = true;
            this.lbKeyWords.ItemHeight = 17;
            this.lbKeyWords.Location = new System.Drawing.Point(0, 22);
            this.lbKeyWords.Name = "lbKeyWords";
            this.lbKeyWords.ScrollAlwaysVisible = true;
            this.lbKeyWords.Size = new System.Drawing.Size(229, 367);
            this.lbKeyWords.TabIndex = 2;
            this.lbKeyWords.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lbKeyWords_MouseDoubleClick);
            // 
            // pnlHotButtons
            // 
            this.pnlHotButtons.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.pnlHotButtons.Controls.Add(this.btnAdd);
            this.pnlHotButtons.Controls.Add(this.btnEdit);
            this.pnlHotButtons.Controls.Add(this.btnDelete);
            this.pnlHotButtons.Controls.Add(this.btnSave);
            this.pnlHotButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHotButtons.Location = new System.Drawing.Point(0, 0);
            this.pnlHotButtons.Name = "pnlHotButtons";
            this.pnlHotButtons.Size = new System.Drawing.Size(520, 22);
            this.pnlHotButtons.TabIndex = 3;
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnAdd.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnAdd.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnAdd.FlatAppearance.BorderSize = 0;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.Image = global::simple_database.Properties.Resources.Button_Add_icon_16_grayed;
            this.btnAdd.Location = new System.Drawing.Point(440, 0);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(20, 22);
            this.btnAdd.TabIndex = 6;
            this.btnAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnEdit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnEdit.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnEdit.FlatAppearance.BorderSize = 0;
            this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEdit.Image = global::simple_database.Properties.Resources.Gear_icon_16_grayed;
            this.btnEdit.Location = new System.Drawing.Point(460, 0);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(20, 22);
            this.btnEdit.TabIndex = 5;
            this.btnEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnEdit.UseVisualStyleBackColor = false;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnDelete.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnDelete.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnDelete.FlatAppearance.BorderSize = 0;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.Image = global::simple_database.Properties.Resources.Button_Close_icon_16_grayed;
            this.btnDelete.Location = new System.Drawing.Point(480, 0);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(20, 22);
            this.btnDelete.TabIndex = 4;
            this.btnDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSave.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.ImageKey = "Save-icon";
            this.btnSave.ImageList = this.imageList1;
            this.btnSave.Location = new System.Drawing.Point(500, 0);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(20, 22);
            this.btnSave.TabIndex = 7;
            this.btnSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Button-Add");
            this.imageList1.Images.SetKeyName(1, "Gear-icon");
            this.imageList1.Images.SetKeyName(2, "Button-Close");
            this.imageList1.Images.SetKeyName(3, "Save-icon");
            this.imageList1.Images.SetKeyName(4, "exclamation");
            // 
            // pnlMoveButtons
            // 
            this.pnlMoveButtons.Controls.Add(this.btnRemoveAllKeywords);
            this.pnlMoveButtons.Controls.Add(this.btnRemoveKeyword);
            this.pnlMoveButtons.Controls.Add(this.btnAppendKeyword);
            this.pnlMoveButtons.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlMoveButtons.Location = new System.Drawing.Point(229, 22);
            this.pnlMoveButtons.Name = "pnlMoveButtons";
            this.pnlMoveButtons.Size = new System.Drawing.Size(62, 367);
            this.pnlMoveButtons.TabIndex = 1;
            // 
            // btnRemoveAllKeywords
            // 
            this.btnRemoveAllKeywords.Location = new System.Drawing.Point(7, 109);
            this.btnRemoveAllKeywords.Name = "btnRemoveAllKeywords";
            this.btnRemoveAllKeywords.Size = new System.Drawing.Size(45, 23);
            this.btnRemoveAllKeywords.TabIndex = 4;
            this.btnRemoveAllKeywords.Text = ">>>";
            this.btnRemoveAllKeywords.UseVisualStyleBackColor = true;
            this.btnRemoveAllKeywords.Click += new System.EventHandler(this.btnRemoveAllKeywords_Click);
            // 
            // btnRemoveKeyword
            // 
            this.btnRemoveKeyword.Location = new System.Drawing.Point(7, 56);
            this.btnRemoveKeyword.Name = "btnRemoveKeyword";
            this.btnRemoveKeyword.Size = new System.Drawing.Size(45, 23);
            this.btnRemoveKeyword.TabIndex = 3;
            this.btnRemoveKeyword.Text = ">";
            this.btnRemoveKeyword.UseVisualStyleBackColor = true;
            this.btnRemoveKeyword.Click += new System.EventHandler(this.btnRemoveKeyword_Click);
            // 
            // btnAppendKeyword
            // 
            this.btnAppendKeyword.Location = new System.Drawing.Point(7, 27);
            this.btnAppendKeyword.Name = "btnAppendKeyword";
            this.btnAppendKeyword.Size = new System.Drawing.Size(45, 23);
            this.btnAppendKeyword.TabIndex = 2;
            this.btnAppendKeyword.Text = "<";
            this.btnAppendKeyword.UseVisualStyleBackColor = true;
            this.btnAppendKeyword.Click += new System.EventHandler(this.btnAppendKeyword_Click);
            // 
            // lbAvailableKeywords
            // 
            this.lbAvailableKeywords.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbAvailableKeywords.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbAvailableKeywords.FormattingEnabled = true;
            this.lbAvailableKeywords.ItemHeight = 17;
            this.lbAvailableKeywords.Location = new System.Drawing.Point(0, 0);
            this.lbAvailableKeywords.Name = "lbAvailableKeywords";
            this.lbAvailableKeywords.ScrollAlwaysVisible = true;
            this.lbAvailableKeywords.Size = new System.Drawing.Size(229, 347);
            this.lbAvailableKeywords.TabIndex = 5;
            this.lbAvailableKeywords.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lbAvailableKeywords_MouseDoubleClick);
            // 
            // pnlAvailableKeywords
            // 
            this.pnlAvailableKeywords.Controls.Add(this.lbAvailableKeywords);
            this.pnlAvailableKeywords.Controls.Add(this.tbFastSearch);
            this.pnlAvailableKeywords.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlAvailableKeywords.Location = new System.Drawing.Point(291, 22);
            this.pnlAvailableKeywords.Name = "pnlAvailableKeywords";
            this.pnlAvailableKeywords.Size = new System.Drawing.Size(229, 367);
            this.pnlAvailableKeywords.TabIndex = 0;
            // 
            // tbFastSearch
            // 
            this.tbFastSearch.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tbFastSearch.Location = new System.Drawing.Point(0, 347);
            this.tbFastSearch.Name = "tbFastSearch";
            this.tbFastSearch.Size = new System.Drawing.Size(229, 20);
            this.tbFastSearch.TabIndex = 0;
            this.tbFastSearch.TextChanged += new System.EventHandler(this.tbFastSearch_TextChanged);
            this.tbFastSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbFastSearch_KeyDown);
            // 
            // frmKeywords
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(520, 389);
            this.Controls.Add(this.pnlAvailableKeywords);
            this.Controls.Add(this.pnlMoveButtons);
            this.Controls.Add(this.lbKeyWords);
            this.Controls.Add(this.pnlHotButtons);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmKeywords";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Ключевые слова";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmHashtags_FormClosing);
            this.Load += new System.EventHandler(this.frmHashtags_Load);
            this.pnlHotButtons.ResumeLayout(false);
            this.pnlMoveButtons.ResumeLayout(false);
            this.pnlAvailableKeywords.ResumeLayout(false);
            this.pnlAvailableKeywords.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lbKeyWords;
        private System.Windows.Forms.Panel pnlHotButtons;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Panel pnlMoveButtons;
        private System.Windows.Forms.Button btnRemoveKeyword;
        private System.Windows.Forms.Button btnAppendKeyword;
        private System.Windows.Forms.ListBox lbAvailableKeywords;
        private System.Windows.Forms.Button btnRemoveAllKeywords;
        private System.Windows.Forms.Panel pnlAvailableKeywords;
        private System.Windows.Forms.TextBox tbFastSearch;
    }
}