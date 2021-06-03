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
            this.rbTriangle.ImageIndex = 0;
            this.rbTriangle.ImageList = this.imageList1;
            this.rbTriangle.Location = new System.Drawing.Point(12, 40);
            this.rbTriangle.Name = "rbTriangle";
            this.rbTriangle.Size = new System.Drawing.Size(50, 24);
            this.rbTriangle.TabIndex = 4;
            this.rbTriangle.TabStop = true;
            this.rbTriangle.Tag = "9";
            this.rbTriangle.UseVisualStyleBackColor = true;
            this.rbTriangle.CheckedChanged += new System.EventHandler(this.rbTriangle_CheckedChanged);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "triangle-icon-16.png");
            this.imageList1.Images.SetKeyName(1, "Square-icon-16.png");
            this.imageList1.Images.SetKeyName(2, "Circle-icon-16.png");
            this.imageList1.Images.SetKeyName(3, "Folder-icon-16.png");
            // 
            // rbSquare
            // 
            this.rbSquare.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rbSquare.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rbSquare.ImageIndex = 1;
            this.rbSquare.ImageList = this.imageList1;
            this.rbSquare.Location = new System.Drawing.Point(12, 70);
            this.rbSquare.Name = "rbSquare";
            this.rbSquare.Size = new System.Drawing.Size(50, 24);
            this.rbSquare.TabIndex = 5;
            this.rbSquare.TabStop = true;
            this.rbSquare.Tag = "13";
            this.rbSquare.UseVisualStyleBackColor = true;
            this.rbSquare.CheckedChanged += new System.EventHandler(this.rbTriangle_CheckedChanged);
            // 
            // rbCircle
            // 
            this.rbCircle.Cursor = System.Windows.Forms.Cursors.Default;
            this.rbCircle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rbCircle.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rbCircle.ImageIndex = 2;
            this.rbCircle.ImageList = this.imageList1;
            this.rbCircle.Location = new System.Drawing.Point(12, 100);
            this.rbCircle.Name = "rbCircle";
            this.rbCircle.Size = new System.Drawing.Size(50, 24);
            this.rbCircle.TabIndex = 6;
            this.rbCircle.TabStop = true;
            this.rbCircle.Tag = "11";
            this.rbCircle.UseVisualStyleBackColor = true;
            this.rbCircle.CheckedChanged += new System.EventHandler(this.rbTriangle_CheckedChanged);
            // 
            // rbFolder
            // 
            this.rbFolder.Cursor = System.Windows.Forms.Cursors.Default;
            this.rbFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rbFolder.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rbFolder.ImageIndex = 3;
            this.rbFolder.ImageList = this.imageList1;
            this.rbFolder.Location = new System.Drawing.Point(68, 70);
            this.rbFolder.Name = "rbFolder";
            this.rbFolder.Size = new System.Drawing.Size(50, 24);
            this.rbFolder.TabIndex = 7;
            this.rbFolder.TabStop = true;
            this.rbFolder.Tag = "6";
            this.rbFolder.UseVisualStyleBackColor = true;
            this.rbFolder.CheckedChanged += new System.EventHandler(this.rbTriangle_CheckedChanged);
            // 
            // frmPropertyEdit
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(383, 133);
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
    }
}