namespace simple_database
{
    partial class frmPluginEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPluginEditor));
            this.pnlHotButtons = new System.Windows.Forms.Panel();
            this.btnInsertPreset = new System.Windows.Forms.Button();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.btnSave = new System.Windows.Forms.Button();
            this.rtb = new System.Windows.Forms.RichTextBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ctxASK = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxSET = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxVALUE = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.шаблоныToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxTemplates_CSharpClass = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxTemplates_CSharpConsoleApp = new System.Windows.Forms.ToolStripMenuItem();
            this.btnRun = new System.Windows.Forms.Button();
            this.pnlHotButtons.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlHotButtons
            // 
            this.pnlHotButtons.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.pnlHotButtons.Controls.Add(this.btnRun);
            this.pnlHotButtons.Controls.Add(this.btnInsertPreset);
            this.pnlHotButtons.Controls.Add(this.btnSave);
            this.pnlHotButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHotButtons.Location = new System.Drawing.Point(0, 0);
            this.pnlHotButtons.Name = "pnlHotButtons";
            this.pnlHotButtons.Size = new System.Drawing.Size(800, 22);
            this.pnlHotButtons.TabIndex = 4;
            // 
            // btnInsertPreset
            // 
            this.btnInsertPreset.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnInsertPreset.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnInsertPreset.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnInsertPreset.FlatAppearance.BorderSize = 0;
            this.btnInsertPreset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInsertPreset.ImageKey = "list";
            this.btnInsertPreset.ImageList = this.imageList1;
            this.btnInsertPreset.Location = new System.Drawing.Point(0, 0);
            this.btnInsertPreset.Name = "btnInsertPreset";
            this.btnInsertPreset.Size = new System.Drawing.Size(20, 22);
            this.btnInsertPreset.TabIndex = 8;
            this.btnInsertPreset.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnInsertPreset.UseVisualStyleBackColor = false;
            this.btnInsertPreset.Click += new System.EventHandler(this.btnInsertPreset_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Save-icon");
            this.imageList1.Images.SetKeyName(1, "exclamation");
            this.imageList1.Images.SetKeyName(2, "list");
            this.imageList1.Images.SetKeyName(3, "play");
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
            this.btnSave.Location = new System.Drawing.Point(780, 0);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(20, 22);
            this.btnSave.TabIndex = 7;
            this.btnSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // rtb
            // 
            this.rtb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtb.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rtb.Location = new System.Drawing.Point(0, 22);
            this.rtb.Name = "rtb";
            this.rtb.Size = new System.Drawing.Size(800, 428);
            this.rtb.TabIndex = 5;
            this.rtb.Text = "";
            this.rtb.TextChanged += new System.EventHandler(this.rtb_TextChanged);
            this.rtb.KeyDown += new System.Windows.Forms.KeyEventHandler(this.rtb_KeyDown);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ctxASK,
            this.ctxSET,
            this.ctxVALUE,
            this.toolStripMenuItem1,
            this.шаблоныToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(129, 98);
            // 
            // ctxASK
            // 
            this.ctxASK.Name = "ctxASK";
            this.ctxASK.Size = new System.Drawing.Size(128, 22);
            this.ctxASK.Text = "ASK";
            this.ctxASK.Click += new System.EventHandler(this.ctxASK_Click);
            // 
            // ctxSET
            // 
            this.ctxSET.Name = "ctxSET";
            this.ctxSET.Size = new System.Drawing.Size(128, 22);
            this.ctxSET.Text = "SET";
            this.ctxSET.Click += new System.EventHandler(this.ctxSET_Click);
            // 
            // ctxVALUE
            // 
            this.ctxVALUE.Name = "ctxVALUE";
            this.ctxVALUE.Size = new System.Drawing.Size(128, 22);
            this.ctxVALUE.Text = "VALUE";
            this.ctxVALUE.Click += new System.EventHandler(this.ctxVALUE_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(125, 6);
            // 
            // шаблоныToolStripMenuItem
            // 
            this.шаблоныToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ctxTemplates_CSharpClass,
            this.ctxTemplates_CSharpConsoleApp});
            this.шаблоныToolStripMenuItem.Name = "шаблоныToolStripMenuItem";
            this.шаблоныToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.шаблоныToolStripMenuItem.Text = "Шаблоны";
            // 
            // ctxTemplates_CSharpClass
            // 
            this.ctxTemplates_CSharpClass.Name = "ctxTemplates_CSharpClass";
            this.ctxTemplates_CSharpClass.Size = new System.Drawing.Size(231, 22);
            this.ctxTemplates_CSharpClass.Text = "C# класс";
            this.ctxTemplates_CSharpClass.Click += new System.EventHandler(this.ctxTemplates_CSharpClass_Click);
            // 
            // ctxTemplates_CSharpConsoleApp
            // 
            this.ctxTemplates_CSharpConsoleApp.Name = "ctxTemplates_CSharpConsoleApp";
            this.ctxTemplates_CSharpConsoleApp.Size = new System.Drawing.Size(231, 22);
            this.ctxTemplates_CSharpConsoleApp.Text = "C# консольное приложение";
            this.ctxTemplates_CSharpConsoleApp.Click += new System.EventHandler(this.ctxTemplates_CSharpConsoleApp_Click);
            // 
            // btnRun
            // 
            this.btnRun.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnRun.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnRun.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnRun.FlatAppearance.BorderSize = 0;
            this.btnRun.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRun.ImageKey = "play";
            this.btnRun.ImageList = this.imageList1;
            this.btnRun.Location = new System.Drawing.Point(20, 0);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(20, 22);
            this.btnRun.TabIndex = 9;
            this.btnRun.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnRun.UseVisualStyleBackColor = false;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // frmPluginEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.rtb);
            this.Controls.Add(this.pnlHotButtons);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmPluginEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Редактор плагина";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmPluginEditor_FormClosing);
            this.pnlHotButtons.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlHotButtons;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.RichTextBox rtb;
        private System.Windows.Forms.Button btnInsertPreset;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ctxASK;
        private System.Windows.Forms.ToolStripMenuItem ctxSET;
        private System.Windows.Forms.ToolStripMenuItem ctxVALUE;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem шаблоныToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ctxTemplates_CSharpClass;
        private System.Windows.Forms.ToolStripMenuItem ctxTemplates_CSharpConsoleApp;
        private System.Windows.Forms.Button btnRun;
    }
}