namespace simple_database
{
    partial class frmSyntaxBuilder
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSyntaxBuilder));
            this.dgv = new System.Windows.Forms.DataGridView();
            this.rtb = new System.Windows.Forms.RichTextBox();
            this.btnImportSyntax = new System.Windows.Forms.Button();
            this.btnExportSyntax = new System.Windows.Forms.Button();
            this.btnCreateSyntax = new System.Windows.Forms.Button();
            this.btnRemoveSyntax = new System.Windows.Forms.Button();
            this.btnSaveSyntax = new System.Windows.Forms.Button();
            this.btnTest = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnCreate = new System.Windows.Forms.Button();
            this.btnMoveDown = new System.Windows.Forms.Button();
            this.btnMoveUp = new System.Windows.Forms.Button();
            this.dgvList = new System.Windows.Forms.DataGridView();
            this.id0 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.enabled0 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.name0 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.pnlTestSection = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pnlRulesSection = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.pnlGroupsSection = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.id1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnOrder = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnRule = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnEnabled = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.сolumnEditRule = new System.Windows.Forms.DataGridViewButtonColumn();
            this.columnColor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnSingleLine = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.columnCase = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.columnFBold = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.columnFItalic = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvList)).BeginInit();
            this.pnlTestSection.SuspendLayout();
            this.panel1.SuspendLayout();
            this.pnlRulesSection.SuspendLayout();
            this.panel3.SuspendLayout();
            this.pnlGroupsSection.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgv
            // 
            this.dgv.AllowUserToAddRows = false;
            this.dgv.AllowUserToDeleteRows = false;
            this.dgv.AllowUserToOrderColumns = true;
            this.dgv.AllowUserToResizeRows = false;
            this.dgv.BackgroundColor = System.Drawing.Color.White;
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id1,
            this.Column1,
            this.columnOrder,
            this.columnRule,
            this.columnEnabled,
            this.Column5,
            this.сolumnEditRule,
            this.columnColor,
            this.columnSingleLine,
            this.columnCase,
            this.columnFBold,
            this.columnFItalic});
            this.dgv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv.Enabled = false;
            this.dgv.Location = new System.Drawing.Point(104, 0);
            this.dgv.MultiSelect = false;
            this.dgv.Name = "dgv";
            this.dgv.RowHeadersVisible = false;
            this.dgv.Size = new System.Drawing.Size(839, 192);
            this.dgv.TabIndex = 0;
            this.dgv.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_CellClick);
            this.dgv.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgv_DataBindingComplete);
            // 
            // rtb
            // 
            this.rtb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtb.Location = new System.Drawing.Point(104, 0);
            this.rtb.Name = "rtb";
            this.rtb.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.rtb.Size = new System.Drawing.Size(839, 120);
            this.rtb.TabIndex = 1;
            this.rtb.Text = "";
            // 
            // btnImportSyntax
            // 
            this.btnImportSyntax.Location = new System.Drawing.Point(14, 133);
            this.btnImportSyntax.Name = "btnImportSyntax";
            this.btnImportSyntax.Size = new System.Drawing.Size(75, 23);
            this.btnImportSyntax.TabIndex = 10;
            this.btnImportSyntax.Text = "Импорт";
            this.toolTip1.SetToolTip(this.btnImportSyntax, "Импортировать группу синтаксических правил из буфера обмена. Для импорта примера," +
        " импортируйте из буфера строку \'sample\'");
            this.btnImportSyntax.UseVisualStyleBackColor = true;
            this.btnImportSyntax.Click += new System.EventHandler(this.btnImportSyntax_Click);
            // 
            // btnExportSyntax
            // 
            this.btnExportSyntax.Enabled = false;
            this.btnExportSyntax.Location = new System.Drawing.Point(14, 104);
            this.btnExportSyntax.Name = "btnExportSyntax";
            this.btnExportSyntax.Size = new System.Drawing.Size(75, 23);
            this.btnExportSyntax.TabIndex = 9;
            this.btnExportSyntax.Text = "Экспорт";
            this.toolTip1.SetToolTip(this.btnExportSyntax, "Экспортировать группу синтаксических правил в буфер обмена в формате XML");
            this.btnExportSyntax.UseVisualStyleBackColor = true;
            this.btnExportSyntax.Click += new System.EventHandler(this.btnExportSyntax_Click);
            // 
            // btnCreateSyntax
            // 
            this.btnCreateSyntax.Location = new System.Drawing.Point(14, 46);
            this.btnCreateSyntax.Name = "btnCreateSyntax";
            this.btnCreateSyntax.Size = new System.Drawing.Size(75, 23);
            this.btnCreateSyntax.TabIndex = 8;
            this.btnCreateSyntax.Text = "Создать";
            this.toolTip1.SetToolTip(this.btnCreateSyntax, "Создать группу синтаксических правил");
            this.btnCreateSyntax.UseVisualStyleBackColor = true;
            this.btnCreateSyntax.Click += new System.EventHandler(this.btnCreateSyntax_Click);
            // 
            // btnRemoveSyntax
            // 
            this.btnRemoveSyntax.Enabled = false;
            this.btnRemoveSyntax.Location = new System.Drawing.Point(14, 75);
            this.btnRemoveSyntax.Name = "btnRemoveSyntax";
            this.btnRemoveSyntax.Size = new System.Drawing.Size(75, 23);
            this.btnRemoveSyntax.TabIndex = 7;
            this.btnRemoveSyntax.Text = "Удалить";
            this.toolTip1.SetToolTip(this.btnRemoveSyntax, "Удалить группу синтаксичеких правил");
            this.btnRemoveSyntax.UseVisualStyleBackColor = true;
            this.btnRemoveSyntax.Click += new System.EventHandler(this.btnRemoveSyntax_Click);
            // 
            // btnSaveSyntax
            // 
            this.btnSaveSyntax.Location = new System.Drawing.Point(14, 17);
            this.btnSaveSyntax.Name = "btnSaveSyntax";
            this.btnSaveSyntax.Size = new System.Drawing.Size(75, 23);
            this.btnSaveSyntax.TabIndex = 6;
            this.btnSaveSyntax.Text = "Сохранить";
            this.toolTip1.SetToolTip(this.btnSaveSyntax, "Сохранить все группы синтаксических правил в текущей БД");
            this.btnSaveSyntax.UseVisualStyleBackColor = true;
            this.btnSaveSyntax.Click += new System.EventHandler(this.btnSaveSyntax_Click);
            // 
            // btnTest
            // 
            this.btnTest.Enabled = false;
            this.btnTest.Location = new System.Drawing.Point(14, 13);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(75, 23);
            this.btnTest.TabIndex = 5;
            this.btnTest.Text = "Тест";
            this.toolTip1.SetToolTip(this.btnTest, "Протестировать выделенное правило в окне тестирования");
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(14, 42);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 4;
            this.btnClear.Text = "Сброс";
            this.toolTip1.SetToolTip(this.btnClear, "Сбросить подсветку синтаксиса в окне тестирования");
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Enabled = false;
            this.btnDelete.Location = new System.Drawing.Point(14, 44);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 3;
            this.btnDelete.Text = "Удалить";
            this.toolTip1.SetToolTip(this.btnDelete, "Удалить синтаксическое правило");
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnCreate
            // 
            this.btnCreate.Enabled = false;
            this.btnCreate.Location = new System.Drawing.Point(14, 15);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(75, 23);
            this.btnCreate.TabIndex = 2;
            this.btnCreate.Text = "Создать";
            this.toolTip1.SetToolTip(this.btnCreate, "Создать синтаксичекое правило");
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // btnMoveDown
            // 
            this.btnMoveDown.Enabled = false;
            this.btnMoveDown.Location = new System.Drawing.Point(14, 131);
            this.btnMoveDown.Name = "btnMoveDown";
            this.btnMoveDown.Size = new System.Drawing.Size(75, 23);
            this.btnMoveDown.TabIndex = 1;
            this.btnMoveDown.Text = "Вниз";
            this.toolTip1.SetToolTip(this.btnMoveDown, "Переместить правило вниз");
            this.btnMoveDown.UseVisualStyleBackColor = true;
            this.btnMoveDown.Click += new System.EventHandler(this.btnMoveDown_Click);
            // 
            // btnMoveUp
            // 
            this.btnMoveUp.Enabled = false;
            this.btnMoveUp.Location = new System.Drawing.Point(14, 102);
            this.btnMoveUp.Name = "btnMoveUp";
            this.btnMoveUp.Size = new System.Drawing.Size(75, 23);
            this.btnMoveUp.TabIndex = 0;
            this.btnMoveUp.Text = "Вверх";
            this.toolTip1.SetToolTip(this.btnMoveUp, "Переместить правило вверх");
            this.btnMoveUp.UseVisualStyleBackColor = true;
            this.btnMoveUp.Click += new System.EventHandler(this.btnMoveUp_Click);
            // 
            // dgvList
            // 
            this.dgvList.AllowUserToAddRows = false;
            this.dgvList.AllowUserToDeleteRows = false;
            this.dgvList.AllowUserToOrderColumns = true;
            this.dgvList.AllowUserToResizeRows = false;
            this.dgvList.BackgroundColor = System.Drawing.Color.White;
            this.dgvList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id0,
            this.enabled0,
            this.name0});
            this.dgvList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvList.Location = new System.Drawing.Point(104, 0);
            this.dgvList.MultiSelect = false;
            this.dgvList.Name = "dgvList";
            this.dgvList.RowHeadersVisible = false;
            this.dgvList.Size = new System.Drawing.Size(839, 188);
            this.dgvList.TabIndex = 3;
            this.dgvList.SelectionChanged += new System.EventHandler(this.dgvList_SelectionChanged);
            // 
            // id0
            // 
            this.id0.DataPropertyName = "id";
            this.id0.HeaderText = "id";
            this.id0.Name = "id0";
            this.id0.Visible = false;
            // 
            // enabled0
            // 
            this.enabled0.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.enabled0.DataPropertyName = "enabled";
            this.enabled0.HeaderText = "Включено";
            this.enabled0.Name = "enabled0";
            this.enabled0.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.enabled0.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.enabled0.Width = 82;
            // 
            // name0
            // 
            this.name0.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.name0.DataPropertyName = "name";
            this.name0.HeaderText = "Имя";
            this.name0.Name = "name0";
            // 
            // pnlTestSection
            // 
            this.pnlTestSection.Controls.Add(this.rtb);
            this.pnlTestSection.Controls.Add(this.panel1);
            this.pnlTestSection.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlTestSection.Location = new System.Drawing.Point(0, 380);
            this.pnlTestSection.Name = "pnlTestSection";
            this.pnlTestSection.Size = new System.Drawing.Size(943, 120);
            this.pnlTestSection.TabIndex = 4;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnClear);
            this.panel1.Controls.Add(this.btnTest);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(104, 120);
            this.panel1.TabIndex = 2;
            // 
            // pnlRulesSection
            // 
            this.pnlRulesSection.Controls.Add(this.dgv);
            this.pnlRulesSection.Controls.Add(this.panel3);
            this.pnlRulesSection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRulesSection.Location = new System.Drawing.Point(0, 188);
            this.pnlRulesSection.Name = "pnlRulesSection";
            this.pnlRulesSection.Size = new System.Drawing.Size(943, 192);
            this.pnlRulesSection.TabIndex = 5;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnDelete);
            this.panel3.Controls.Add(this.btnCreate);
            this.panel3.Controls.Add(this.btnMoveUp);
            this.panel3.Controls.Add(this.btnMoveDown);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(104, 192);
            this.panel3.TabIndex = 3;
            // 
            // pnlGroupsSection
            // 
            this.pnlGroupsSection.Controls.Add(this.dgvList);
            this.pnlGroupsSection.Controls.Add(this.panel4);
            this.pnlGroupsSection.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlGroupsSection.Location = new System.Drawing.Point(0, 0);
            this.pnlGroupsSection.Name = "pnlGroupsSection";
            this.pnlGroupsSection.Size = new System.Drawing.Size(943, 188);
            this.pnlGroupsSection.TabIndex = 6;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btnImportSyntax);
            this.panel4.Controls.Add(this.btnSaveSyntax);
            this.panel4.Controls.Add(this.btnExportSyntax);
            this.panel4.Controls.Add(this.btnRemoveSyntax);
            this.panel4.Controls.Add(this.btnCreateSyntax);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(104, 188);
            this.panel4.TabIndex = 3;
            // 
            // id1
            // 
            this.id1.DataPropertyName = "id";
            this.id1.HeaderText = "_id";
            this.id1.Name = "id1";
            this.id1.Visible = false;
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "parentId";
            this.Column1.HeaderText = "_parentId";
            this.Column1.Name = "Column1";
            this.Column1.Visible = false;
            // 
            // columnOrder
            // 
            this.columnOrder.DataPropertyName = "order";
            this.columnOrder.HeaderText = "_order";
            this.columnOrder.Name = "columnOrder";
            this.columnOrder.ReadOnly = true;
            this.columnOrder.Visible = false;
            // 
            // columnRule
            // 
            this.columnRule.DataPropertyName = "rule";
            this.columnRule.HeaderText = "_dataRule";
            this.columnRule.Name = "columnRule";
            this.columnRule.Visible = false;
            // 
            // columnEnabled
            // 
            this.columnEnabled.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.columnEnabled.DataPropertyName = "enabled";
            this.columnEnabled.HeaderText = "Включено";
            this.columnEnabled.Name = "columnEnabled";
            this.columnEnabled.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.columnEnabled.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.columnEnabled.Width = 82;
            // 
            // Column5
            // 
            this.Column5.DataPropertyName = "name";
            this.Column5.HeaderText = "Имя";
            this.Column5.Name = "Column5";
            // 
            // сolumnEditRule
            // 
            this.сolumnEditRule.HeaderText = "Правило";
            this.сolumnEditRule.Name = "сolumnEditRule";
            this.сolumnEditRule.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.сolumnEditRule.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // columnColor
            // 
            this.columnColor.DataPropertyName = "color";
            this.columnColor.HeaderText = "Цвет";
            this.columnColor.Name = "columnColor";
            this.columnColor.ReadOnly = true;
            this.columnColor.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.columnColor.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // columnSingleLine
            // 
            this.columnSingleLine.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.columnSingleLine.DataPropertyName = "singleLine";
            this.columnSingleLine.HeaderText = "Multiline";
            this.columnSingleLine.Name = "columnSingleLine";
            this.columnSingleLine.Width = 51;
            // 
            // columnCase
            // 
            this.columnCase.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.columnCase.DataPropertyName = "case";
            this.columnCase.HeaderText = "Insensitive";
            this.columnCase.Name = "columnCase";
            this.columnCase.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.columnCase.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.columnCase.Width = 82;
            // 
            // columnFBold
            // 
            this.columnFBold.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.columnFBold.DataPropertyName = "fbold";
            this.columnFBold.HeaderText = "Bold";
            this.columnFBold.Name = "columnFBold";
            this.columnFBold.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.columnFBold.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.columnFBold.Width = 53;
            // 
            // columnFItalic
            // 
            this.columnFItalic.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.columnFItalic.DataPropertyName = "fitalic";
            this.columnFItalic.HeaderText = "Italic";
            this.columnFItalic.Name = "columnFItalic";
            this.columnFItalic.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.columnFItalic.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.columnFItalic.Width = 54;
            // 
            // frmSyntaxBuilder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(943, 500);
            this.Controls.Add(this.pnlRulesSection);
            this.Controls.Add(this.pnlGroupsSection);
            this.Controls.Add(this.pnlTestSection);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmSyntaxBuilder";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Редактор подсветки синтаксиса";
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvList)).EndInit();
            this.pnlTestSection.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.pnlRulesSection.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.pnlGroupsSection.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgv;
        public System.Windows.Forms.RichTextBox rtb;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Button btnMoveDown;
        private System.Windows.Forms.Button btnMoveUp;
        private System.Windows.Forms.Button btnSaveSyntax;
        private System.Windows.Forms.Button btnRemoveSyntax;
        private System.Windows.Forms.DataGridView dgvList;
        private System.Windows.Forms.Button btnCreateSyntax;
        private System.Windows.Forms.DataGridViewTextBoxColumn id0;
        private System.Windows.Forms.DataGridViewCheckBoxColumn enabled0;
        private System.Windows.Forms.DataGridViewTextBoxColumn name0;
        private System.Windows.Forms.Button btnImportSyntax;
        private System.Windows.Forms.Button btnExportSyntax;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Panel pnlTestSection;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel pnlRulesSection;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel pnlGroupsSection;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.DataGridViewTextBoxColumn id1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnOrder;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnRule;
        private System.Windows.Forms.DataGridViewCheckBoxColumn columnEnabled;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewButtonColumn сolumnEditRule;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnColor;
        private System.Windows.Forms.DataGridViewCheckBoxColumn columnSingleLine;
        private System.Windows.Forms.DataGridViewCheckBoxColumn columnCase;
        private System.Windows.Forms.DataGridViewCheckBoxColumn columnFBold;
        private System.Windows.Forms.DataGridViewCheckBoxColumn columnFItalic;
    }
}