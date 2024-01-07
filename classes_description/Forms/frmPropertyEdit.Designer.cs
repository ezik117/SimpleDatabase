namespace simple_database
{
    partial class frmPropertyEdit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPropertyEdit));
            this.btnOk = new System.Windows.Forms.Button();
            this.tbPropertyName = new System.Windows.Forms.TextBox();
            this.rbTriangle = new System.Windows.Forms.RadioButton();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.rbSquare = new System.Windows.Forms.RadioButton();
            this.rbCircle = new System.Windows.Forms.RadioButton();
            this.rbFolder = new System.Windows.Forms.RadioButton();
            this.rbFolderBlue = new System.Windows.Forms.RadioButton();
            this.rbFolderGreen = new System.Windows.Forms.RadioButton();
            this.rbFile = new System.Windows.Forms.RadioButton();
            this.rbAttachment = new System.Windows.Forms.RadioButton();
            this.rbFolderGrey = new System.Windows.Forms.RadioButton();
            this.rbFolderMagenta = new System.Windows.Forms.RadioButton();
            this.rbFolderRed = new System.Windows.Forms.RadioButton();
            this.rbClass = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.gbStandardItems = new System.Windows.Forms.GroupBox();
            this.gbSpecialItems = new System.Windows.Forms.GroupBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.gbStandardItems.SuspendLayout();
            this.gbSpecialItems.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(316, 12);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(51, 20);
            this.btnOk.TabIndex = 3;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // tbPropertyName
            // 
            this.tbPropertyName.Location = new System.Drawing.Point(12, 12);
            this.tbPropertyName.Name = "tbPropertyName";
            this.tbPropertyName.Size = new System.Drawing.Size(298, 20);
            this.tbPropertyName.TabIndex = 2;
            // 
            // rbTriangle
            // 
            this.rbTriangle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rbTriangle.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rbTriangle.ImageIndex = 1;
            this.rbTriangle.ImageList = this.imageList1;
            this.rbTriangle.Location = new System.Drawing.Point(12, 19);
            this.rbTriangle.Name = "rbTriangle";
            this.rbTriangle.Size = new System.Drawing.Size(50, 24);
            this.rbTriangle.TabIndex = 4;
            this.rbTriangle.Tag = "9";
            this.rbTriangle.UseVisualStyleBackColor = true;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "book");
            this.imageList1.Images.SetKeyName(1, "triangle");
            this.imageList1.Images.SetKeyName(2, "square");
            this.imageList1.Images.SetKeyName(3, "circle");
            this.imageList1.Images.SetKeyName(4, "folder");
            this.imageList1.Images.SetKeyName(5, "folder_blue");
            this.imageList1.Images.SetKeyName(6, "folder_green");
            this.imageList1.Images.SetKeyName(7, "file");
            this.imageList1.Images.SetKeyName(8, "selected");
            this.imageList1.Images.SetKeyName(9, "attachment");
            this.imageList1.Images.SetKeyName(10, "folder_red");
            this.imageList1.Images.SetKeyName(11, "folder_magenta");
            this.imageList1.Images.SetKeyName(12, "folder_grey");
            this.imageList1.Images.SetKeyName(13, "class");
            this.imageList1.Images.SetKeyName(14, "file_important");
            // 
            // rbSquare
            // 
            this.rbSquare.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rbSquare.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rbSquare.ImageIndex = 2;
            this.rbSquare.ImageList = this.imageList1;
            this.rbSquare.Location = new System.Drawing.Point(12, 49);
            this.rbSquare.Name = "rbSquare";
            this.rbSquare.Size = new System.Drawing.Size(50, 24);
            this.rbSquare.TabIndex = 5;
            this.rbSquare.Tag = "13";
            this.rbSquare.UseVisualStyleBackColor = true;
            // 
            // rbCircle
            // 
            this.rbCircle.Cursor = System.Windows.Forms.Cursors.Default;
            this.rbCircle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rbCircle.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rbCircle.ImageIndex = 3;
            this.rbCircle.ImageList = this.imageList1;
            this.rbCircle.Location = new System.Drawing.Point(12, 79);
            this.rbCircle.Name = "rbCircle";
            this.rbCircle.Size = new System.Drawing.Size(50, 24);
            this.rbCircle.TabIndex = 6;
            this.rbCircle.Tag = "11";
            this.rbCircle.UseVisualStyleBackColor = true;
            // 
            // rbFolder
            // 
            this.rbFolder.Cursor = System.Windows.Forms.Cursors.Default;
            this.rbFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rbFolder.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rbFolder.ImageIndex = 4;
            this.rbFolder.ImageList = this.imageList1;
            this.rbFolder.Location = new System.Drawing.Point(68, 19);
            this.rbFolder.Name = "rbFolder";
            this.rbFolder.Size = new System.Drawing.Size(50, 24);
            this.rbFolder.TabIndex = 7;
            this.rbFolder.Tag = "6";
            this.rbFolder.UseVisualStyleBackColor = true;
            // 
            // rbFolderBlue
            // 
            this.rbFolderBlue.Cursor = System.Windows.Forms.Cursors.Default;
            this.rbFolderBlue.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rbFolderBlue.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rbFolderBlue.ImageIndex = 5;
            this.rbFolderBlue.ImageList = this.imageList1;
            this.rbFolderBlue.Location = new System.Drawing.Point(68, 49);
            this.rbFolderBlue.Name = "rbFolderBlue";
            this.rbFolderBlue.Size = new System.Drawing.Size(50, 24);
            this.rbFolderBlue.TabIndex = 8;
            this.rbFolderBlue.Tag = "6";
            this.rbFolderBlue.UseVisualStyleBackColor = true;
            // 
            // rbFolderGreen
            // 
            this.rbFolderGreen.Cursor = System.Windows.Forms.Cursors.Default;
            this.rbFolderGreen.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rbFolderGreen.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rbFolderGreen.ImageIndex = 6;
            this.rbFolderGreen.ImageList = this.imageList1;
            this.rbFolderGreen.Location = new System.Drawing.Point(68, 79);
            this.rbFolderGreen.Name = "rbFolderGreen";
            this.rbFolderGreen.Size = new System.Drawing.Size(50, 24);
            this.rbFolderGreen.TabIndex = 9;
            this.rbFolderGreen.Tag = "6";
            this.rbFolderGreen.UseVisualStyleBackColor = true;
            // 
            // rbFile
            // 
            this.rbFile.Cursor = System.Windows.Forms.Cursors.Default;
            this.rbFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rbFile.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rbFile.ImageIndex = 7;
            this.rbFile.ImageList = this.imageList1;
            this.rbFile.Location = new System.Drawing.Point(180, 19);
            this.rbFile.Name = "rbFile";
            this.rbFile.Size = new System.Drawing.Size(50, 24);
            this.rbFile.TabIndex = 10;
            this.rbFile.Tag = "6";
            this.rbFile.UseVisualStyleBackColor = true;
            // 
            // rbAttachment
            // 
            this.rbAttachment.Cursor = System.Windows.Forms.Cursors.Default;
            this.rbAttachment.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rbAttachment.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rbAttachment.ImageIndex = 9;
            this.rbAttachment.ImageList = this.imageList1;
            this.rbAttachment.Location = new System.Drawing.Point(16, 19);
            this.rbAttachment.Name = "rbAttachment";
            this.rbAttachment.Size = new System.Drawing.Size(50, 24);
            this.rbAttachment.TabIndex = 13;
            this.rbAttachment.TabStop = true;
            this.rbAttachment.Tag = "6";
            this.toolTip1.SetToolTip(this.rbAttachment, "Добавить вложение (пустое название - будет имя файла)");
            this.rbAttachment.UseVisualStyleBackColor = true;
            this.rbAttachment.CheckedChanged += new System.EventHandler(this.rbAttachment_CheckedChanged);
            // 
            // rbFolderGrey
            // 
            this.rbFolderGrey.Cursor = System.Windows.Forms.Cursors.Default;
            this.rbFolderGrey.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rbFolderGrey.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rbFolderGrey.ImageIndex = 12;
            this.rbFolderGrey.ImageList = this.imageList1;
            this.rbFolderGrey.Location = new System.Drawing.Point(124, 79);
            this.rbFolderGrey.Name = "rbFolderGrey";
            this.rbFolderGrey.Size = new System.Drawing.Size(50, 24);
            this.rbFolderGrey.TabIndex = 16;
            this.rbFolderGrey.Tag = "6";
            this.rbFolderGrey.UseVisualStyleBackColor = true;
            // 
            // rbFolderMagenta
            // 
            this.rbFolderMagenta.Cursor = System.Windows.Forms.Cursors.Default;
            this.rbFolderMagenta.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rbFolderMagenta.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rbFolderMagenta.ImageIndex = 11;
            this.rbFolderMagenta.ImageList = this.imageList1;
            this.rbFolderMagenta.Location = new System.Drawing.Point(124, 49);
            this.rbFolderMagenta.Name = "rbFolderMagenta";
            this.rbFolderMagenta.Size = new System.Drawing.Size(50, 24);
            this.rbFolderMagenta.TabIndex = 15;
            this.rbFolderMagenta.Tag = "6";
            this.rbFolderMagenta.UseVisualStyleBackColor = true;
            // 
            // rbFolderRed
            // 
            this.rbFolderRed.Cursor = System.Windows.Forms.Cursors.Default;
            this.rbFolderRed.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rbFolderRed.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rbFolderRed.ImageIndex = 10;
            this.rbFolderRed.ImageList = this.imageList1;
            this.rbFolderRed.Location = new System.Drawing.Point(124, 19);
            this.rbFolderRed.Name = "rbFolderRed";
            this.rbFolderRed.Size = new System.Drawing.Size(50, 24);
            this.rbFolderRed.TabIndex = 14;
            this.rbFolderRed.Tag = "6";
            this.rbFolderRed.UseVisualStyleBackColor = true;
            // 
            // rbClass
            // 
            this.rbClass.Cursor = System.Windows.Forms.Cursors.Default;
            this.rbClass.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rbClass.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rbClass.ImageIndex = 13;
            this.rbClass.ImageList = this.imageList1;
            this.rbClass.Location = new System.Drawing.Point(180, 79);
            this.rbClass.Name = "rbClass";
            this.rbClass.Size = new System.Drawing.Size(50, 24);
            this.rbClass.TabIndex = 17;
            this.rbClass.Tag = "6";
            this.rbClass.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            this.radioButton1.Cursor = System.Windows.Forms.Cursors.Default;
            this.radioButton1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.radioButton1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.radioButton1.ImageIndex = 14;
            this.radioButton1.ImageList = this.imageList1;
            this.radioButton1.Location = new System.Drawing.Point(180, 49);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(50, 24);
            this.radioButton1.TabIndex = 18;
            this.radioButton1.Tag = "6";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // gbStandardItems
            // 
            this.gbStandardItems.Controls.Add(this.rbFolder);
            this.gbStandardItems.Controls.Add(this.radioButton1);
            this.gbStandardItems.Controls.Add(this.rbTriangle);
            this.gbStandardItems.Controls.Add(this.rbClass);
            this.gbStandardItems.Controls.Add(this.rbSquare);
            this.gbStandardItems.Controls.Add(this.rbFolderGrey);
            this.gbStandardItems.Controls.Add(this.rbCircle);
            this.gbStandardItems.Controls.Add(this.rbFolderMagenta);
            this.gbStandardItems.Controls.Add(this.rbFolderBlue);
            this.gbStandardItems.Controls.Add(this.rbFolderRed);
            this.gbStandardItems.Controls.Add(this.rbFolderGreen);
            this.gbStandardItems.Controls.Add(this.rbFile);
            this.gbStandardItems.Location = new System.Drawing.Point(12, 68);
            this.gbStandardItems.Name = "gbStandardItems";
            this.gbStandardItems.Size = new System.Drawing.Size(267, 116);
            this.gbStandardItems.TabIndex = 19;
            this.gbStandardItems.TabStop = false;
            this.gbStandardItems.Text = "Стандартные";
            this.gbStandardItems.Enter += new System.EventHandler(this.gbStandardItems_Enter);
            // 
            // gbSpecialItems
            // 
            this.gbSpecialItems.Controls.Add(this.rbAttachment);
            this.gbSpecialItems.Location = new System.Drawing.Point(285, 68);
            this.gbSpecialItems.Name = "gbSpecialItems";
            this.gbSpecialItems.Size = new System.Drawing.Size(106, 116);
            this.gbSpecialItems.TabIndex = 19;
            this.gbSpecialItems.TabStop = false;
            this.gbSpecialItems.Text = "Специальные";
            this.gbSpecialItems.Enter += new System.EventHandler(this.gbSpecialItems_Enter);
            // 
            // frmPropertyEdit
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(403, 194);
            this.Controls.Add(this.gbSpecialItems);
            this.Controls.Add(this.gbStandardItems);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.tbPropertyName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "frmPropertyEdit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Элемент оглавления";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmPropertyEdit_FormClosing);
            this.Load += new System.EventHandler(this.frmPropertyEdit_Load);
            this.Shown += new System.EventHandler(this.frmPropertyEdit_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmPropertyEdit_KeyDown);
            this.gbStandardItems.ResumeLayout(false);
            this.gbSpecialItems.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        public System.Windows.Forms.TextBox tbPropertyName;
        public System.Windows.Forms.RadioButton rbTriangle;
        private System.Windows.Forms.ImageList imageList1;
        public System.Windows.Forms.RadioButton rbSquare;
        public System.Windows.Forms.RadioButton rbCircle;
        public System.Windows.Forms.RadioButton rbFolder;
        public System.Windows.Forms.RadioButton rbFolderBlue;
        public System.Windows.Forms.RadioButton rbFolderGreen;
        public System.Windows.Forms.RadioButton rbFile;
        public System.Windows.Forms.RadioButton rbAttachment;
        public System.Windows.Forms.RadioButton rbFolderGrey;
        public System.Windows.Forms.RadioButton rbFolderMagenta;
        public System.Windows.Forms.RadioButton rbFolderRed;
        public System.Windows.Forms.RadioButton rbClass;
        public System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.GroupBox gbStandardItems;
        private System.Windows.Forms.GroupBox gbSpecialItems;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}