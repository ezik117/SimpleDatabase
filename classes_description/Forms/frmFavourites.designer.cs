namespace simple_database
{
    partial class frmFavourites
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmFavourites));
            this.panel1 = new System.Windows.Forms.Panel();
            this.dgv = new System.Windows.Forms.DataGridView();
            this.favouritesClass = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.favouritesProperty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.favourites_class_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.favourites_property_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlHotButtons = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.btnDeleteAll = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.pnlHotButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dgv);
            this.panel1.Controls.Add(this.pnlHotButtons);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(800, 450);
            this.panel1.TabIndex = 0;
            // 
            // dgv
            // 
            this.dgv.AllowUserToAddRows = false;
            this.dgv.AllowUserToDeleteRows = false;
            this.dgv.AllowUserToResizeRows = false;
            this.dgv.BackgroundColor = System.Drawing.Color.White;
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.favouritesClass,
            this.favouritesProperty,
            this.favourites_class_id,
            this.favourites_property_id});
            this.dgv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv.Location = new System.Drawing.Point(0, 22);
            this.dgv.MultiSelect = false;
            this.dgv.Name = "dgv";
            this.dgv.ReadOnly = true;
            this.dgv.RowHeadersVisible = false;
            this.dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv.Size = new System.Drawing.Size(800, 428);
            this.dgv.TabIndex = 0;
            this.dgv.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_CellDoubleClick);
            // 
            // favouritesClass
            // 
            this.favouritesClass.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.favouritesClass.DataPropertyName = "class";
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Purple;
            this.favouritesClass.DefaultCellStyle = dataGridViewCellStyle1;
            this.favouritesClass.HeaderText = "Каталог";
            this.favouritesClass.Name = "favouritesClass";
            this.favouritesClass.ReadOnly = true;
            this.favouritesClass.Width = 73;
            // 
            // favouritesProperty
            // 
            this.favouritesProperty.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.favouritesProperty.DataPropertyName = "property";
            this.favouritesProperty.HeaderText = "Оглавление";
            this.favouritesProperty.Name = "favouritesProperty";
            this.favouritesProperty.ReadOnly = true;
            // 
            // favourites_class_id
            // 
            this.favourites_class_id.DataPropertyName = "class_id";
            this.favourites_class_id.HeaderText = "_class_id";
            this.favourites_class_id.Name = "favourites_class_id";
            this.favourites_class_id.ReadOnly = true;
            this.favourites_class_id.Visible = false;
            // 
            // favourites_property_id
            // 
            this.favourites_property_id.DataPropertyName = "property_id";
            this.favourites_property_id.HeaderText = "_property_id";
            this.favourites_property_id.Name = "favourites_property_id";
            this.favourites_property_id.ReadOnly = true;
            this.favourites_property_id.Visible = false;
            // 
            // pnlHotButtons
            // 
            this.pnlHotButtons.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.pnlHotButtons.Controls.Add(this.button1);
            this.pnlHotButtons.Controls.Add(this.btnDelete);
            this.pnlHotButtons.Controls.Add(this.btnDeleteAll);
            this.pnlHotButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHotButtons.Location = new System.Drawing.Point(0, 0);
            this.pnlHotButtons.Name = "pnlHotButtons";
            this.pnlHotButtons.Size = new System.Drawing.Size(800, 22);
            this.pnlHotButtons.TabIndex = 6;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.button1.Dock = System.Windows.Forms.DockStyle.Right;
            this.button1.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Image = global::simple_database.Properties.Resources.Button_Add_icon_16_grayed;
            this.button1.Location = new System.Drawing.Point(740, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(20, 22);
            this.button1.TabIndex = 6;
            this.button1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnDeleteAll
            // 
            this.btnDeleteAll.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnDeleteAll.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnDeleteAll.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnDeleteAll.FlatAppearance.BorderSize = 0;
            this.btnDeleteAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteAll.Image = global::simple_database.Properties.Resources.clear_2_16_grayed;
            this.btnDeleteAll.Location = new System.Drawing.Point(780, 0);
            this.btnDeleteAll.Name = "btnDeleteAll";
            this.btnDeleteAll.Size = new System.Drawing.Size(20, 22);
            this.btnDeleteAll.TabIndex = 5;
            this.btnDeleteAll.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnDeleteAll.UseVisualStyleBackColor = false;
            this.btnDeleteAll.Click += new System.EventHandler(this.btnDeleteAll_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnDelete.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnDelete.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnDelete.FlatAppearance.BorderSize = 0;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.Image = global::simple_database.Properties.Resources.Button_Close_icon_16_grayed;
            this.btnDelete.Location = new System.Drawing.Point(760, 0);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(20, 22);
            this.btnDelete.TabIndex = 4;
            this.btnDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // frmFavourites
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "frmFavourites";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Избранное";
            this.Load += new System.EventHandler(this.frmFavourites_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmFavourites_KeyDown);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.pnlHotButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.DataGridViewTextBoxColumn favouritesClass;
        private System.Windows.Forms.DataGridViewTextBoxColumn favouritesProperty;
        private System.Windows.Forms.DataGridViewTextBoxColumn favourites_class_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn favourites_property_id;
        private System.Windows.Forms.Panel pnlHotButtons;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnDeleteAll;
        private System.Windows.Forms.Button btnDelete;
    }
}