namespace classes_description
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
            this.rbTriangle.Location = new System.Drawing.Point(12, 40);
            this.rbTriangle.Name = "rbTriangle";
            this.rbTriangle.Size = new System.Drawing.Size(50, 24);
            this.rbTriangle.TabIndex = 4;
            this.rbTriangle.TabStop = true;
            this.rbTriangle.Tag = "9";
            this.rbTriangle.UseVisualStyleBackColor = true;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "class");
            this.imageList1.Images.SetKeyName(1, "triangle");
            this.imageList1.Images.SetKeyName(2, "square");
            this.imageList1.Images.SetKeyName(3, "circle");
            this.imageList1.Images.SetKeyName(4, "folder");
            this.imageList1.Images.SetKeyName(5, "folder_blue");
            this.imageList1.Images.SetKeyName(6, "folder_green");
            this.imageList1.Images.SetKeyName(7, "file");
            // 
            // rbSquare
            // 
            this.rbSquare.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rbSquare.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rbSquare.ImageIndex = 2;
            this.rbSquare.ImageList = this.imageList1;
            this.rbSquare.Location = new System.Drawing.Point(12, 70);
            this.rbSquare.Name = "rbSquare";
            this.rbSquare.Size = new System.Drawing.Size(50, 24);
            this.rbSquare.TabIndex = 5;
            this.rbSquare.TabStop = true;
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
            this.rbCircle.Location = new System.Drawing.Point(12, 100);
            this.rbCircle.Name = "rbCircle";
            this.rbCircle.Size = new System.Drawing.Size(50, 24);
            this.rbCircle.TabIndex = 6;
            this.rbCircle.TabStop = true;
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
            this.rbFolder.Location = new System.Drawing.Point(68, 40);
            this.rbFolder.Name = "rbFolder";
            this.rbFolder.Size = new System.Drawing.Size(50, 24);
            this.rbFolder.TabIndex = 7;
            this.rbFolder.TabStop = true;
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
            this.rbFolderBlue.Location = new System.Drawing.Point(68, 70);
            this.rbFolderBlue.Name = "rbFolderBlue";
            this.rbFolderBlue.Size = new System.Drawing.Size(50, 24);
            this.rbFolderBlue.TabIndex = 8;
            this.rbFolderBlue.TabStop = true;
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
            this.rbFolderGreen.Location = new System.Drawing.Point(68, 100);
            this.rbFolderGreen.Name = "rbFolderGreen";
            this.rbFolderGreen.Size = new System.Drawing.Size(50, 24);
            this.rbFolderGreen.TabIndex = 9;
            this.rbFolderGreen.TabStop = true;
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
            this.rbFile.Location = new System.Drawing.Point(124, 70);
            this.rbFile.Name = "rbFile";
            this.rbFile.Size = new System.Drawing.Size(50, 24);
            this.rbFile.TabIndex = 10;
            this.rbFile.TabStop = true;
            this.rbFile.Tag = "6";
            this.rbFile.UseVisualStyleBackColor = true;
            // 
            // frmPropertyEdit
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(383, 133);
            this.Controls.Add(this.rbFile);
            this.Controls.Add(this.rbFolderGreen);
            this.Controls.Add(this.rbFolderBlue);
            this.Controls.Add(this.rbFolder);
            this.Controls.Add(this.rbCircle);
            this.Controls.Add(this.rbSquare);
            this.Controls.Add(this.rbTriangle);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.tbPropertyName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmPropertyEdit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Свойство";
            this.Load += new System.EventHandler(this.frmPropertyEdit_Load);
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
    }
}