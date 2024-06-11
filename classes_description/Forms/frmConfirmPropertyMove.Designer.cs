namespace simple_database
{
    partial class frmConfirmPropertyMove
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
            this.rbMoveAsNewClass = new System.Windows.Forms.RadioButton();
            this.rbMoveToSelectedClass = new System.Windows.Forms.RadioButton();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // rbMoveAsNewClass
            // 
            this.rbMoveAsNewClass.AutoSize = true;
            this.rbMoveAsNewClass.Location = new System.Drawing.Point(26, 13);
            this.rbMoveAsNewClass.Name = "rbMoveAsNewClass";
            this.rbMoveAsNewClass.Size = new System.Drawing.Size(131, 17);
            this.rbMoveAsNewClass.TabIndex = 0;
            this.rbMoveAsNewClass.TabStop = true;
            this.rbMoveAsNewClass.Text = "Создать как каталог";
            this.rbMoveAsNewClass.UseVisualStyleBackColor = true;
            // 
            // rbMoveToSelectedClass
            // 
            this.rbMoveToSelectedClass.AutoSize = true;
            this.rbMoveToSelectedClass.Location = new System.Drawing.Point(26, 36);
            this.rbMoveToSelectedClass.Name = "rbMoveToSelectedClass";
            this.rbMoveToSelectedClass.Size = new System.Drawing.Size(206, 17);
            this.rbMoveToSelectedClass.TabIndex = 1;
            this.rbMoveToSelectedClass.TabStop = true;
            this.rbMoveToSelectedClass.Text = "Переместить в выбранный каталог";
            this.rbMoveToSelectedClass.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(69, 87);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(151, 23);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "Переместить";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(226, 87);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // frmConfirmPropertyMove
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(314, 120);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.rbMoveToSelectedClass);
            this.Controls.Add(this.rbMoveAsNewClass);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmConfirmPropertyMove";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Перемещение оглавления";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton rbMoveAsNewClass;
        private System.Windows.Forms.RadioButton rbMoveToSelectedClass;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
    }
}