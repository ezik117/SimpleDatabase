namespace simple_database
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.btnDescReload = new System.Windows.Forms.Button();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.btnDescSave = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.splitter3 = new System.Windows.Forms.Splitter();
            this.panel4 = new System.Windows.Forms.Panel();
            this.tvProps = new System.Windows.Forms.TreeView();
            this.imageList2 = new System.Windows.Forms.ImageList(this.components);
            this.panel11 = new System.Windows.Forms.Panel();
            this.btnPropSearch = new System.Windows.Forms.Button();
            this.btnPropCollapseExpand = new System.Windows.Forms.Button();
            this.panelPropCaption = new System.Windows.Forms.Panel();
            this.btnPropAdd = new System.Windows.Forms.Button();
            this.btnPropEdit = new System.Windows.Forms.Button();
            this.btnPropDel = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.panel3 = new System.Windows.Forms.Panel();
            this.tvClasses = new System.Windows.Forms.TreeView();
            this.panel7 = new System.Windows.Forms.Panel();
            this.btnClassAdd = new System.Windows.Forms.Button();
            this.btnClassEdit = new System.Windows.Forms.Button();
            this.btnClassDel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pnlClassEditHolder = new System.Windows.Forms.Panel();
            this.panel8 = new System.Windows.Forms.Panel();
            this.btnClassDescReload = new System.Windows.Forms.Button();
            this.btnClassDescSave = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.pnlParamEditHolder = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel11.SuspendLayout();
            this.panelPropCaption.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel8.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel5);
            this.panel1.Controls.Add(this.splitter3);
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Controls.Add(this.splitter2);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(800, 358);
            this.panel1.TabIndex = 0;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.pnlParamEditHolder);
            this.panel5.Controls.Add(this.panel6);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(453, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(347, 358);
            this.panel5.TabIndex = 4;
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel6.Controls.Add(this.btnDescReload);
            this.panel6.Controls.Add(this.btnDescSave);
            this.panel6.Controls.Add(this.label3);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(0, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(347, 24);
            this.panel6.TabIndex = 3;
            // 
            // btnDescReload
            // 
            this.btnDescReload.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnDescReload.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnDescReload.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnDescReload.FlatAppearance.BorderSize = 0;
            this.btnDescReload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDescReload.ImageKey = "reload_grayed";
            this.btnDescReload.ImageList = this.imageList1;
            this.btnDescReload.Location = new System.Drawing.Point(305, 0);
            this.btnDescReload.Name = "btnDescReload";
            this.btnDescReload.Size = new System.Drawing.Size(20, 22);
            this.btnDescReload.TabIndex = 5;
            this.btnDescReload.UseVisualStyleBackColor = false;
            this.btnDescReload.Click += new System.EventHandler(this.btnDescReload_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "add");
            this.imageList1.Images.SetKeyName(1, "delete");
            this.imageList1.Images.SetKeyName(2, "edit");
            this.imageList1.Images.SetKeyName(3, "save");
            this.imageList1.Images.SetKeyName(4, "notsaved");
            this.imageList1.Images.SetKeyName(5, "class");
            this.imageList1.Images.SetKeyName(6, "collapse");
            this.imageList1.Images.SetKeyName(7, "expand");
            this.imageList1.Images.SetKeyName(8, "reload_grayed");
            this.imageList1.Images.SetKeyName(9, "selected");
            this.imageList1.Images.SetKeyName(10, "search_grayed");
            this.imageList1.Images.SetKeyName(11, "bold_text");
            this.imageList1.Images.SetKeyName(12, "italic_text");
            this.imageList1.Images.SetKeyName(13, "underline_text");
            this.imageList1.Images.SetKeyName(14, "bullet_list");
            this.imageList1.Images.SetKeyName(15, "numbers_list");
            this.imageList1.Images.SetKeyName(16, "textcolor");
            this.imageList1.Images.SetKeyName(17, "backcolor");
            this.imageList1.Images.SetKeyName(18, "font");
            this.imageList1.Images.SetKeyName(19, "attachment");
            this.imageList1.Images.SetKeyName(20, "brush");
            // 
            // btnDescSave
            // 
            this.btnDescSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnDescSave.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnDescSave.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnDescSave.FlatAppearance.BorderSize = 0;
            this.btnDescSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDescSave.ImageKey = "save";
            this.btnDescSave.ImageList = this.imageList1;
            this.btnDescSave.Location = new System.Drawing.Point(325, 0);
            this.btnDescSave.Name = "btnDescSave";
            this.btnDescSave.Size = new System.Drawing.Size(20, 22);
            this.btnDescSave.TabIndex = 4;
            this.btnDescSave.UseVisualStyleBackColor = false;
            this.btnDescSave.Click += new System.EventHandler(this.btnDescSave_Click);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.label3.Size = new System.Drawing.Size(345, 22);
            this.label3.TabIndex = 2;
            this.label3.Text = "Описание свойства";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // splitter3
            // 
            this.splitter3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitter3.Location = new System.Drawing.Point(448, 0);
            this.splitter3.Name = "splitter3";
            this.splitter3.Size = new System.Drawing.Size(5, 358);
            this.splitter3.TabIndex = 3;
            this.splitter3.TabStop = false;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.tvProps);
            this.panel4.Controls.Add(this.panel11);
            this.panel4.Controls.Add(this.panelPropCaption);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel4.Location = new System.Drawing.Point(205, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(243, 358);
            this.panel4.TabIndex = 2;
            // 
            // tvProps
            // 
            this.tvProps.AllowDrop = true;
            this.tvProps.BackColor = System.Drawing.Color.White;
            this.tvProps.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvProps.FullRowSelect = true;
            this.tvProps.HideSelection = false;
            this.tvProps.ImageIndex = 0;
            this.tvProps.ImageList = this.imageList2;
            this.tvProps.Location = new System.Drawing.Point(0, 24);
            this.tvProps.Name = "tvProps";
            this.tvProps.SelectedImageIndex = 0;
            this.tvProps.Size = new System.Drawing.Size(243, 312);
            this.tvProps.TabIndex = 3;
            this.tvProps.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.tvProps_ItemDrag);
            this.tvProps.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvProps_BeforeSelect);
            this.tvProps.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvProps_AfterSelect);
            this.tvProps.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvProps_NodeMouseDoubleClick);
            this.tvProps.DragDrop += new System.Windows.Forms.DragEventHandler(this.tvProps_DragDrop);
            this.tvProps.DragEnter += new System.Windows.Forms.DragEventHandler(this.tvProps_DragEnter);
            this.tvProps.DragOver += new System.Windows.Forms.DragEventHandler(this.tvProps_DragOver);
            this.tvProps.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tvProps_KeyDown);
            // 
            // imageList2
            // 
            this.imageList2.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList2.ImageStream")));
            this.imageList2.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList2.Images.SetKeyName(0, "class");
            this.imageList2.Images.SetKeyName(1, "triangle");
            this.imageList2.Images.SetKeyName(2, "square");
            this.imageList2.Images.SetKeyName(3, "circle");
            this.imageList2.Images.SetKeyName(4, "folder");
            this.imageList2.Images.SetKeyName(5, "folder_blue");
            this.imageList2.Images.SetKeyName(6, "folder_green");
            this.imageList2.Images.SetKeyName(7, "file");
            this.imageList2.Images.SetKeyName(8, "selected");
            this.imageList2.Images.SetKeyName(9, "attachment");
            // 
            // panel11
            // 
            this.panel11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.panel11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel11.Controls.Add(this.btnPropSearch);
            this.panel11.Controls.Add(this.btnPropCollapseExpand);
            this.panel11.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel11.Location = new System.Drawing.Point(0, 336);
            this.panel11.Name = "panel11";
            this.panel11.Size = new System.Drawing.Size(243, 22);
            this.panel11.TabIndex = 6;
            // 
            // btnPropSearch
            // 
            this.btnPropSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnPropSearch.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnPropSearch.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnPropSearch.FlatAppearance.BorderSize = 0;
            this.btnPropSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPropSearch.ImageKey = "search_grayed";
            this.btnPropSearch.ImageList = this.imageList1;
            this.btnPropSearch.Location = new System.Drawing.Point(20, 0);
            this.btnPropSearch.Name = "btnPropSearch";
            this.btnPropSearch.Size = new System.Drawing.Size(20, 20);
            this.btnPropSearch.TabIndex = 8;
            this.btnPropSearch.UseVisualStyleBackColor = false;
            this.btnPropSearch.Click += new System.EventHandler(this.btnPropSearch_Click);
            // 
            // btnPropCollapseExpand
            // 
            this.btnPropCollapseExpand.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnPropCollapseExpand.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnPropCollapseExpand.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnPropCollapseExpand.FlatAppearance.BorderSize = 0;
            this.btnPropCollapseExpand.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPropCollapseExpand.ImageKey = "collapse";
            this.btnPropCollapseExpand.ImageList = this.imageList1;
            this.btnPropCollapseExpand.Location = new System.Drawing.Point(0, 0);
            this.btnPropCollapseExpand.Name = "btnPropCollapseExpand";
            this.btnPropCollapseExpand.Size = new System.Drawing.Size(20, 20);
            this.btnPropCollapseExpand.TabIndex = 7;
            this.btnPropCollapseExpand.UseVisualStyleBackColor = false;
            this.btnPropCollapseExpand.Click += new System.EventHandler(this.btnPropCollapseExpand_Click);
            // 
            // panelPropCaption
            // 
            this.panelPropCaption.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.panelPropCaption.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelPropCaption.Controls.Add(this.btnPropAdd);
            this.panelPropCaption.Controls.Add(this.btnPropEdit);
            this.panelPropCaption.Controls.Add(this.btnPropDel);
            this.panelPropCaption.Controls.Add(this.label2);
            this.panelPropCaption.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelPropCaption.Location = new System.Drawing.Point(0, 0);
            this.panelPropCaption.Name = "panelPropCaption";
            this.panelPropCaption.Size = new System.Drawing.Size(243, 24);
            this.panelPropCaption.TabIndex = 2;
            // 
            // btnPropAdd
            // 
            this.btnPropAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnPropAdd.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnPropAdd.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnPropAdd.FlatAppearance.BorderSize = 0;
            this.btnPropAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPropAdd.ImageKey = "add";
            this.btnPropAdd.ImageList = this.imageList1;
            this.btnPropAdd.Location = new System.Drawing.Point(181, 0);
            this.btnPropAdd.Name = "btnPropAdd";
            this.btnPropAdd.Size = new System.Drawing.Size(20, 22);
            this.btnPropAdd.TabIndex = 3;
            this.btnPropAdd.UseVisualStyleBackColor = false;
            this.btnPropAdd.Click += new System.EventHandler(this.btnPropAdd_Click);
            // 
            // btnPropEdit
            // 
            this.btnPropEdit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnPropEdit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnPropEdit.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnPropEdit.FlatAppearance.BorderSize = 0;
            this.btnPropEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPropEdit.ImageKey = "edit";
            this.btnPropEdit.ImageList = this.imageList1;
            this.btnPropEdit.Location = new System.Drawing.Point(201, 0);
            this.btnPropEdit.Name = "btnPropEdit";
            this.btnPropEdit.Size = new System.Drawing.Size(20, 22);
            this.btnPropEdit.TabIndex = 5;
            this.btnPropEdit.UseVisualStyleBackColor = false;
            this.btnPropEdit.Click += new System.EventHandler(this.btnPropEdit_Click);
            // 
            // btnPropDel
            // 
            this.btnPropDel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnPropDel.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnPropDel.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnPropDel.FlatAppearance.BorderSize = 0;
            this.btnPropDel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPropDel.ImageKey = "delete";
            this.btnPropDel.ImageList = this.imageList1;
            this.btnPropDel.Location = new System.Drawing.Point(221, 0);
            this.btnPropDel.Name = "btnPropDel";
            this.btnPropDel.Size = new System.Drawing.Size(20, 22);
            this.btnPropDel.TabIndex = 4;
            this.btnPropDel.UseVisualStyleBackColor = false;
            this.btnPropDel.Click += new System.EventHandler(this.btnPropDel_Click);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.label2.Size = new System.Drawing.Size(241, 22);
            this.label2.TabIndex = 2;
            this.label2.Text = "Свойства";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // splitter2
            // 
            this.splitter2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitter2.Location = new System.Drawing.Point(200, 0);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(5, 358);
            this.splitter2.TabIndex = 1;
            this.splitter2.TabStop = false;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.tvClasses);
            this.panel3.Controls.Add(this.panel7);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(200, 358);
            this.panel3.TabIndex = 0;
            // 
            // tvClasses
            // 
            this.tvClasses.BackColor = System.Drawing.Color.White;
            this.tvClasses.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvClasses.FullRowSelect = true;
            this.tvClasses.HideSelection = false;
            this.tvClasses.Location = new System.Drawing.Point(0, 24);
            this.tvClasses.Name = "tvClasses";
            this.tvClasses.ShowLines = false;
            this.tvClasses.ShowRootLines = false;
            this.tvClasses.Size = new System.Drawing.Size(200, 334);
            this.tvClasses.TabIndex = 1;
            this.tvClasses.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvClasses_BeforeSelect);
            this.tvClasses.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvClasses_AfterSelect);
            // 
            // panel7
            // 
            this.panel7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.panel7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel7.Controls.Add(this.btnClassAdd);
            this.panel7.Controls.Add(this.btnClassEdit);
            this.panel7.Controls.Add(this.btnClassDel);
            this.panel7.Controls.Add(this.label1);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel7.Location = new System.Drawing.Point(0, 0);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(200, 24);
            this.panel7.TabIndex = 3;
            // 
            // btnClassAdd
            // 
            this.btnClassAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnClassAdd.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnClassAdd.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnClassAdd.FlatAppearance.BorderSize = 0;
            this.btnClassAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClassAdd.ImageKey = "add";
            this.btnClassAdd.ImageList = this.imageList1;
            this.btnClassAdd.Location = new System.Drawing.Point(138, 0);
            this.btnClassAdd.Name = "btnClassAdd";
            this.btnClassAdd.Size = new System.Drawing.Size(20, 22);
            this.btnClassAdd.TabIndex = 3;
            this.btnClassAdd.UseVisualStyleBackColor = false;
            this.btnClassAdd.Click += new System.EventHandler(this.btnClassAdd_Click);
            // 
            // btnClassEdit
            // 
            this.btnClassEdit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnClassEdit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnClassEdit.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnClassEdit.FlatAppearance.BorderSize = 0;
            this.btnClassEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClassEdit.ImageKey = "edit";
            this.btnClassEdit.ImageList = this.imageList1;
            this.btnClassEdit.Location = new System.Drawing.Point(158, 0);
            this.btnClassEdit.Name = "btnClassEdit";
            this.btnClassEdit.Size = new System.Drawing.Size(20, 22);
            this.btnClassEdit.TabIndex = 5;
            this.btnClassEdit.UseVisualStyleBackColor = false;
            this.btnClassEdit.Click += new System.EventHandler(this.btnClassEdit_Click);
            // 
            // btnClassDel
            // 
            this.btnClassDel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnClassDel.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnClassDel.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnClassDel.FlatAppearance.BorderSize = 0;
            this.btnClassDel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClassDel.ImageKey = "delete";
            this.btnClassDel.ImageList = this.imageList1;
            this.btnClassDel.Location = new System.Drawing.Point(178, 0);
            this.btnClassDel.Name = "btnClassDel";
            this.btnClassDel.Size = new System.Drawing.Size(20, 22);
            this.btnClassDel.TabIndex = 4;
            this.btnClassDel.UseVisualStyleBackColor = false;
            this.btnClassDel.Click += new System.EventHandler(this.btnClassDel_Click);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.label1.Size = new System.Drawing.Size(198, 22);
            this.label1.TabIndex = 2;
            this.label1.Text = "Категории";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // splitter1
            // 
            this.splitter1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter1.Location = new System.Drawing.Point(0, 358);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(800, 5);
            this.splitter1.TabIndex = 1;
            this.splitter1.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.pnlClassEditHolder);
            this.panel2.Controls.Add(this.panel8);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 363);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(800, 87);
            this.panel2.TabIndex = 2;
            // 
            // pnlClassEditHolder
            // 
            this.pnlClassEditHolder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlClassEditHolder.Location = new System.Drawing.Point(0, 24);
            this.pnlClassEditHolder.Name = "pnlClassEditHolder";
            this.pnlClassEditHolder.Size = new System.Drawing.Size(800, 63);
            this.pnlClassEditHolder.TabIndex = 4;
            // 
            // panel8
            // 
            this.panel8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.panel8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel8.Controls.Add(this.btnClassDescReload);
            this.panel8.Controls.Add(this.btnClassDescSave);
            this.panel8.Controls.Add(this.label4);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel8.Location = new System.Drawing.Point(0, 0);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(800, 24);
            this.panel8.TabIndex = 3;
            // 
            // btnClassDescReload
            // 
            this.btnClassDescReload.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnClassDescReload.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnClassDescReload.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnClassDescReload.FlatAppearance.BorderSize = 0;
            this.btnClassDescReload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClassDescReload.ImageKey = "reload_grayed";
            this.btnClassDescReload.ImageList = this.imageList1;
            this.btnClassDescReload.Location = new System.Drawing.Point(758, 0);
            this.btnClassDescReload.Name = "btnClassDescReload";
            this.btnClassDescReload.Size = new System.Drawing.Size(20, 22);
            this.btnClassDescReload.TabIndex = 5;
            this.btnClassDescReload.UseVisualStyleBackColor = false;
            this.btnClassDescReload.Click += new System.EventHandler(this.btnClassDescReload_Click);
            // 
            // btnClassDescSave
            // 
            this.btnClassDescSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnClassDescSave.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnClassDescSave.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnClassDescSave.FlatAppearance.BorderSize = 0;
            this.btnClassDescSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClassDescSave.ImageKey = "save";
            this.btnClassDescSave.ImageList = this.imageList1;
            this.btnClassDescSave.Location = new System.Drawing.Point(778, 0);
            this.btnClassDescSave.Name = "btnClassDescSave";
            this.btnClassDescSave.Size = new System.Drawing.Size(20, 22);
            this.btnClassDescSave.TabIndex = 4;
            this.btnClassDescSave.UseVisualStyleBackColor = false;
            this.btnClassDescSave.Click += new System.EventHandler(this.btnClassDescSave_Click);
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.label4.Location = new System.Drawing.Point(0, 0);
            this.label4.Name = "label4";
            this.label4.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.label4.Size = new System.Drawing.Size(798, 22);
            this.label4.TabIndex = 2;
            this.label4.Text = "Описание категории";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlParamEditHolder
            // 
            this.pnlParamEditHolder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlParamEditHolder.Location = new System.Drawing.Point(0, 24);
            this.pnlParamEditHolder.Name = "pnlParamEditHolder";
            this.pnlParamEditHolder.Size = new System.Drawing.Size(347, 334);
            this.pnlParamEditHolder.TabIndex = 4;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Справочник";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel11.ResumeLayout(false);
            this.panelPropCaption.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Splitter splitter3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Splitter splitter2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Panel panel2;
        public System.Windows.Forms.TreeView tvClasses;
        private System.Windows.Forms.Panel panelPropCaption;
        private System.Windows.Forms.Button btnPropAdd;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnPropEdit;
        private System.Windows.Forms.Button btnPropDel;
        private System.Windows.Forms.Panel panel6;
        public System.Windows.Forms.Button btnDescSave;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Button btnClassAdd;
        private System.Windows.Forms.Button btnClassEdit;
        private System.Windows.Forms.Button btnClassDel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel8;
        public System.Windows.Forms.Button btnClassDescSave;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.TreeView tvProps;
        private System.Windows.Forms.Button btnClassDescReload;
        private System.Windows.Forms.Button btnDescReload;
        private System.Windows.Forms.Panel panel11;
        private System.Windows.Forms.Button btnPropCollapseExpand;
        private System.Windows.Forms.Button btnPropSearch;
        private System.Windows.Forms.ImageList imageList2;
        private System.Windows.Forms.Panel pnlClassEditHolder;
        private System.Windows.Forms.Panel pnlParamEditHolder;
    }
}

