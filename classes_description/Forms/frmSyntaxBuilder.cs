using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace simple_database
{
    public partial class frmSyntaxBuilder : Form
    {
        DataSet ds = new DataSet();
        int selectedRowIndex = -1;

        public frmSyntaxBuilder()
        {
            InitializeComponent();

            DataTable dt0 = ds.Tables.Add("syntax_rules");
            DataColumn c = dt0.Columns.Add("id", typeof(int));
            c.AutoIncrement = true;
            c.AutoIncrementSeed = 1;
            c.AutoIncrementStep = 1;
            dt0.Columns.Add("enabled", typeof(bool));
            dt0.Columns.Add("name", typeof(string));
            dgvList.AutoGenerateColumns = false;
            dgvList.DataSource = ds.Tables["syntax_rules"];

            DataTable dt1 = ds.Tables.Add("rules");
            c = dt1.Columns.Add("id", typeof(int));
            c.AutoIncrement = true;
            c.AutoIncrementSeed = 1;
            c.AutoIncrementStep = 1;
            dt1.PrimaryKey = new DataColumn[] { c };
            c = dt1.Columns.Add("parentId", typeof(int));
            dt1.Columns.Add("order", typeof(int));
            dt1.Columns.Add("name", typeof(string));
            dt1.Columns.Add("rule", typeof(string));
            dt1.Columns.Add("color", typeof(int));
            dt1.Columns.Add("singleLine", typeof(bool));
            dt1.Columns.Add("rgKw", typeof(bool));
            dt1.Columns.Add("enabled", typeof(bool));
            dt1.Columns.Add("case", typeof(bool));
            dgv.AutoGenerateColumns = false;
            ds.Tables["rules"].DefaultView.Sort = "order";
            ds.Tables["rules"].DefaultView.RowFilter = "parentId=NULL";
            dgv.DataSource = ds.Tables["rules"].DefaultView;

            // заполним данными из класса
            foreach (SyntaxRule sr in VARS.syntaxRules.Rules)
            {
                DataRow dr0 = ds.Tables["syntax_rules"].NewRow();
                dr0["name"] = sr.Name;
                dr0["enabled"] = sr.Enabled;

                foreach (RuleRow rr in sr.Rules)
                {
                    DataRow dr1 = ds.Tables["rules"].NewRow();
                    dr1["name"] = rr.Name;
                    dr1["enabled"] = rr.Enabled;
                    dr1["rule"] = rr.Rule;
                    dr1["color"] = rr.Color;
                    dr1["singleLine"] = rr.SingleLine;
                    dr1["rgKw"] = rr.RgKw;
                    dr1["case"] = rr.Case;
                    dr1["parentId"] = dr0["id"];
                    dr1["order"] = rr.Order;
                    ds.Tables["rules"].Rows.Add(dr1);
                }

                ds.Tables["syntax_rules"].Rows.Add(dr0);
            }
            ds.Tables["syntax_rules"].AcceptChanges();
            ds.Tables["rules"].AcceptChanges();

            dgvList.Columns["id0"].Visible = false;
            dgv.Columns["id1"].Visible = false;

            // активация кнопок
            if (ds.Tables["syntax_rules"].Rows.Count > 0)
            {
                btnRemoveSyntax.Enabled = btnExportSyntax.Enabled = true;
                btnCreate.Enabled = true;
                dgv.Enabled = true;
            }

            dgvList_SelectionChanged(null, null);

            rtb.Text = "sample";
        }

        /// <summary>
        /// Кнопка: Сбросить стиль тестового текста
        /// </summary>
        private void btnClear_Click(object sender, EventArgs e)
        {
            rtb.SelectAll();
            rtb.SelectionColor = Color.Black;
            rtb.Select(0, 0);
        }

        /// <summary>
        /// Кнопка: Протестировать применение синтаксиса
        /// </summary>
        private void btnTest_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dr in dgv.Rows)
            {
                if (dr.Cells["columnRule"].Value != null && (bool)dr.Cells["columnEnabled"].Value)
                {
                    RegexOptions ro = (bool)dr.Cells["columnSingleLine"].Value ? RegexOptions.Multiline : RegexOptions.Singleline;
                    if ((bool)dr.Cells["columnCase"].Value) ro |= RegexOptions.IgnoreCase;

                    if ((bool)dr.Cells["columnRegexKeywords"].Value)
                    {
                        string[] kws = dr.Cells["columnRule"].Value.ToString().Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string kw in kws)
                        {
                            try
                            {
                                MatchCollection mm = Regex.Matches(rtb.Text, kw, ro);
                                foreach (Match m in mm)
                                {
                                    rtb.Select(m.Index, m.Length);
                                    rtb.SelectionColor = dr.Cells["columnColor"].Style.BackColor;
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    else
                    {
                        try
                        {
                            MatchCollection mm = Regex.Matches(rtb.Text, dr.Cells["columnRule"].Value.ToString(), ro);
                            foreach (Match m in mm)
                            {
                                rtb.Select(m.Index, m.Length);
                                rtb.SelectionColor = dr.Cells["columnColor"].Style.BackColor;
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Кнопка: создать синтаксическое  правило
        /// </summary>
        private void btnCreate_Click(object sender, EventArgs e)
        {
            int rowId = AddRuleRow(
                (int)dgvList.SelectedCells[0].OwningRow.Cells["id0"].Value,
                -1,
                $"Правило {1 + dgv.Rows.Count}",
                String.Empty,
                Color.Black.ToArgb(),
                false,
                false,
                false,
                true
            );
            
            for (int i = 0; i < dgv.Rows.Count; i++)
                if ((int)dgv.Rows[i].Cells[0].Value == rowId)
                {
                    dgv.Rows[i].Cells["columnEnabled"].Selected = true;
                    break;
                }
        }

        /// <summary>
        /// Добавляет строчку с правилом
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="order">Если -1, то будет присвоен ID создаваемой строки</param>
        /// <param name="name"></param>
        /// <param name="rule"></param>
        /// <param name="color"></param>
        /// <param name="singleLine"></param>
        /// <param name="rgKw"></param>
        /// <param name="Case"></param>
        /// <param name="enabled"></param>
        /// <returns>ID созданной строки</returns>
        private int AddRuleRow(int parentId, int order, string name, string rule, int color, bool singleLine, bool rgKw, bool Case, bool enabled)
        {
            int ret = -1;
            DataRow dr = ds.Tables["rules"].NewRow();
            dr["parentId"] = parentId;
            dr["order"] = order < 0 ? dr["id"] : order;
            dr["name"] = name;
            dr["rule"] = rule;
            dr["color"] = color;
            dr["singleLine"] = singleLine;
            dr["rgKw"] = rgKw;
            dr["case"] = Case;
            dr["enabled"] = enabled;
            ret = (int)dr["id"];
            ds.Tables["rules"].Rows.Add(dr);
            ds.Tables["rules"].AcceptChanges();

            btnDelete.Enabled = btnMoveDown.Enabled = btnMoveUp.Enabled = true;
            btnTest.Enabled = true;
            return ret;
        }

        /// <summary>
        /// Кнопка: Удалить правило
        /// </summary>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgv.CurrentRow == null || dgv.Rows.Count == 0) return;

            ((DataRowView)dgv.CurrentRow.DataBoundItem).Delete();
            ds.Tables["rules"].AcceptChanges();

            if (ds.Tables["rules"].Rows.Count == 0)
            {
                btnDelete.Enabled = btnMoveUp.Enabled = btnMoveDown.Enabled = false;
                btnTest.Enabled = false;
            }
        }

        /// <summary>
        /// Выбрана другая группа синтаксических правил. Отобразить список.
        /// </summary>
        private void dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridView d = (DataGridView)sender;
            if (d.Columns[e.ColumnIndex].DataPropertyName == "color")
            {
                ColorDialog cd = new ColorDialog();
                cd.Color = Color.FromArgb((int)d.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                DialogResult dr = cd.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    if (d.SelectedCells.Count > 0)
                    {
                        DataGridViewRow hostRow = d.SelectedCells[0].OwningRow;
                        DataRowView row = (DataRowView)hostRow.DataBoundItem;
                        row["color"] = cd.Color.ToArgb();
                        ds.Tables["syntax_rules"].AcceptChanges();

                        DataGridViewCellStyle cs = d.Rows[e.RowIndex].Cells["columnColor"].Style;
                        cs.SelectionBackColor = cs.SelectionForeColor = cs.BackColor = cs.ForeColor = cd.Color;
                    }
                }
            }
        }

        /// <summary>
        /// Кнопка: передвинуть правило вверх
        /// </summary>
        private void btnMoveUp_Click(object sender, EventArgs e)
        {
            if (dgv.CurrentRow.Index == 0) return;

            int ord0 = (int)dgv.CurrentRow.Cells["columnOrder"].Value;
            int ord1 = (int)dgv.Rows[dgv.CurrentRow.Index - 1].Cells["columnOrder"].Value;
            int i0 = (int)dgv.CurrentRow.Cells[0].Value;
            int i1 = (int)dgv.Rows[dgv.CurrentRow.Index - 1].Cells[0].Value;

            ds.Tables["rules"].Rows.Find(i0)["order"] = ord1;
            ds.Tables["rules"].Rows.Find(i1)["order"] = ord0;
            ds.Tables["rules"].AcceptChanges();
        }

        /// <summary>
        /// Кнопка: передвинуть правило вниз
        /// </summary>
        private void btnMoveDown_Click(object sender, EventArgs e)
        {
            if (dgv.CurrentRow.Index == dgv.Rows.Count - 1) return;

            int ord0 = (int)dgv.CurrentRow.Cells["columnOrder"].Value;
            int ord1 = (int)dgv.Rows[dgv.CurrentRow.Index + 1].Cells["columnOrder"].Value;
            int i0 = (int)dgv.CurrentRow.Cells[0].Value;
            int i1 = (int)dgv.Rows[dgv.CurrentRow.Index + 1].Cells[0].Value;

            ds.Tables["rules"].Rows.Find(i0)["order"] = ord1;
            ds.Tables["rules"].Rows.Find(i1)["order"] = ord0;
            ds.Tables["rules"].AcceptChanges();
        }

        /// <summary>
        /// Кнопка: Создать синтаксическую группу
        /// </summary>
        private void btnCreateSyntax_Click(object sender, EventArgs e)
        {
            AddSyntaxRule(true, $"Синтаксическая группа {1 + ds.Tables["syntax_rules"].Rows.Count}");
        }

        /// <summary>
        /// Добавляет синтаксическое правило
        /// </summary>
        /// <param name="enabled"></param>
        /// <param name="name"></param>
        /// <returns>ID строки</returns>
        private int AddSyntaxRule(bool enabled, string name)
        {
            int ret = -1;
            DataRow dr = ds.Tables["syntax_rules"].NewRow();
            dr["enabled"] = enabled;
            dr["name"] = name;
            ret = (int)dr["id"];
            ds.Tables["syntax_rules"].Rows.Add(dr);
            ds.Tables["syntax_rules"].AcceptChanges();
            btnRemoveSyntax.Enabled = btnExportSyntax.Enabled = true;
            btnCreate.Enabled = true;
            dgv.Enabled = true;
            return ret;
        }

        /// <summary>
        /// Кнопка: Удалить синтаксическую группу
        /// </summary>
        private void btnRemoveSyntax_Click(object sender, EventArgs e)
        {
            if (dgvList.SelectedCells.Count == 0 || dgvList.Rows.Count == 0) return;

            if (MessageBox.Show("Удалить синтаксическую группу?", "Подтверждение",
                                MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                                MessageBoxDefaultButton.Button2) == DialogResult.No) return;

            try
            {
                int i0 = (int)dgvList.CurrentRow.Cells[0].Value;
                DataRow[] drs = ds.Tables["rules"].Select($"parentId={i0}");
                foreach (DataRow dr in drs) dr.Delete();

                ((DataRowView)dgvList.CurrentRow.DataBoundItem).Delete();
                ds.Tables["syntax_rules"].AcceptChanges();
                ds.Tables["rules"].AcceptChanges();
            }
            catch
            {
                ds.Tables["syntax_rules"].RejectChanges();
                ds.Tables["rules"].RejectChanges();
                return;
            }

            if (ds.Tables["syntax_rules"].Rows.Count == 0)
            {
                btnRemoveSyntax.Enabled = false;
                btnCreate.Enabled = btnDelete.Enabled = btnMoveDown.Enabled = btnMoveUp.Enabled = false;
                btnExportSyntax.Enabled = false;
                btnTest.Enabled = false;
                dgv.Enabled = false;
            }
            else
            {
                selectedRowIndex = -1;
                dgvList_SelectionChanged(null, null);
            }
        }

        public IEnumerable<DataGridViewRow> GetSyntaxChildren(DataGridView d, int id)
        {
            foreach (DataGridViewRow r in d.Rows)
                if ((int)r.Cells[0].Value == id) yield return r;
        }



        private void btnSaveSyntax_Click(object sender, EventArgs e)
        {
            VARS.syntaxRules.Rules.Clear();

            foreach (DataRow dr in ds.Tables["syntax_rules"].Rows)
            {
                SyntaxRule sr = new SyntaxRule();
                sr.id = (int)dr["id"];
                sr.Enabled = (bool)dr["enabled"];
                sr.Name = (string)dr["name"];
                DataRow[] drc = ds.Tables["rules"].Select($"parentId={sr.id}", "order");
                foreach (DataRow r in drc)
                {
                    RuleRow rr = new RuleRow();
                    rr.ParentId = sr.id;
                    rr.Order = (int)r["order"];
                    rr.Enabled = (bool)r["enabled"];
                    rr.Name = (string)r["name"];
                    rr.Rule = (string)r["rule"];
                    rr.Color = (int)r["color"];
                    rr.SingleLine = (bool)r["singleLine"];
                    rr.RgKw = (bool)r["rgKw"];
                    rr.Case = (bool)r["case"];
                    sr.Rules.Add(rr);
                }
                VARS.syntaxRules.Rules.Add(sr);
            }

            string xml = HELPER.SerializeSyntaxRules(VARS.syntaxRules);
            DATABASE.SaveSyntaxRules(xml);

            MessageBox.Show("Сохранено.");

            // Обновляем run-time меню
            HELPER.AddPropertiesContextMenuItems();
        }

        private void dgvList_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvList.CurrentRow != null)
            {
                if (dgvList.CurrentRow.Index != selectedRowIndex)
                {
                    selectedRowIndex = dgvList.CurrentRow.Index;
                    ds.Tables["rules"].DefaultView.RowFilter = $"parentId={dgvList.CurrentRow.Cells[0].Value}";
                    if (ds.Tables["rules"].DefaultView.Count == 0)
                    {
                        btnDelete.Enabled = btnMoveUp.Enabled = btnMoveDown.Enabled = false;
                        btnTest.Enabled = false;
                    }
                    else
                    {
                        btnDelete.Enabled = btnMoveDown.Enabled = btnMoveUp.Enabled = true;
                        btnTest.Enabled = true;
                    }
                }
            }
        }

        /// <summary>
        /// Вывод цвета ячейки стобца "Цвет" при изменении данных в DataSet
        /// </summary>
        private void dgv_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            DataGridView d = (DataGridView)sender;
            foreach (DataGridViewRow row in d.Rows)
            {
                if (row.Cells["columnColor"].Value != null)
                {
                    DataGridViewCellStyle cs = row.Cells["columnColor"].Style;
                    cs.SelectionBackColor = cs.SelectionForeColor = cs.BackColor = cs.ForeColor = Color.FromArgb((int)row.Cells["columnColor"].Value);
                }
            }
        }

        /// <summary>
        /// Кнопка: Экспорт выделенного синтаксического правила в буфер обмена
        /// </summary>
        private void btnExportSyntax_Click(object sender, EventArgs e)
        {
            if (dgvList.CurrentRow == null) return;

            DataRowView dr = (DataRowView)dgvList.CurrentRow.DataBoundItem;
            SyntaxRule sr = new SyntaxRule();
            sr.id = (int)dr["id"];
            sr.Enabled = (bool)dr["enabled"];
            sr.Name = (string)dr["name"];
            DataRow[] drc = ds.Tables["rules"].Select($"parentId={sr.id}", "order");
            foreach (DataRow r in drc)
            {
                RuleRow rr = new RuleRow();
                rr.ParentId = sr.id;
                rr.Order = (int)r["order"];
                rr.Enabled = (bool)r["enabled"];
                rr.Name = (string)r["name"];
                rr.Rule = (string)r["rule"];
                rr.Color = (int)r["color"];
                rr.SingleLine = (bool)r["singleLine"];
                rr.RgKw = (bool)r["rgKw"];
                rr.Case = (bool)r["case"];
                sr.Rules.Add(rr);
            }

            string serializedRules = HELPER.SerializeSyntaxRules(sr);

            Clipboard.SetText(serializedRules);
            MessageBox.Show("Скопировано в буфер");
        }

        /// <summary>
        /// Кнопка: Импорт выделенного синтаксического правила из буфера обмена
        /// </summary>
        private void btnImportSyntax_Click(object sender, EventArgs e)
        {
            try
            {
                SyntaxRule sr = new SyntaxRule();

                string xml = Clipboard.GetText();
                if (xml == null || xml == "") return;
                if (xml == "sample") xml = sampleXML;

                HELPER.DeserializeSyntaxRules(xml, ref sr);

                int id0 = AddSyntaxRule(sr.Enabled, sr.Name);
                foreach (RuleRow rr in sr.Rules)
                    AddRuleRow(
                        id0,
                        rr.Order,
                        rr.Name,
                        rr.Rule,
                        rr.Color,
                        rr.SingleLine,
                        rr.RgKw,
                        rr.Case,
                        rr.Enabled
                    );

                selectedRowIndex = -1;
                dgvList_SelectionChanged(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка импорта: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string sampleXML = @"<?xml version=""1.0"" encoding=""utf-16""?>
<SyntaxRule xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <id>1</id>
  <Name>Python</Name>
  <Enabled>true</Enabled>
  <Rules>
    <RuleRow>
      <ParentId>1</ParentId>
      <Order>1</Order>
      <Name>Код</Name>
      <Rule>.*</Rule>
      <Color>-16776961</Color>
      <SingleLine>false</SingleLine>
      <RgKw>false</RgKw>
      <Case>false</Case>
      <Enabled>true</Enabled>
    </RuleRow>
    <RuleRow>
      <ParentId>1</ParentId>
      <Order>2</Order>
      <Name>Ключевые слова 1</Name>
      <Rule>import |from |if |else:|elif |pass|while |for | in |range|break|continue|True|False| and | or | not | |class | as |def |del |raise|except|try|finally|global |None|with |yield|return|</Rule>
      <Color>-8388353</Color>
      <SingleLine>false</SingleLine>
      <RgKw>true</RgKw>
      <Case>false</Case>
      <Enabled>true</Enabled>
    </RuleRow>
    <RuleRow>
      <ParentId>1</ParentId>
      <Order>3</Order>
      <Name>Строка 1</Name>
      <Rule>"".*?""</Rule>
      <Color>-45233</Color>
      <SingleLine>false</SingleLine>
      <RgKw>false</RgKw>
      <Case>false</Case>
      <Enabled>true</Enabled>
    </RuleRow>
    <RuleRow>
      <ParentId>1</ParentId>
      <Order>4</Order>
      <Name>Строка 2</Name>
      <Rule>'.*?'</Rule>
      <Color>-45233</Color>
      <SingleLine>false</SingleLine>
      <RgKw>false</RgKw>
      <Case>false</Case>
      <Enabled>true</Enabled>
    </RuleRow>
    <RuleRow>
      <ParentId>1</ParentId>
      <Order>5</Order>
      <Name>Комментарий 1</Name>
      <Rule>#.*?$</Rule>
      <Color>-16744384</Color>
      <SingleLine>true</SingleLine>
      <RgKw>false</RgKw>
      <Case>false</Case>
      <Enabled>true</Enabled>
    </RuleRow>
    <RuleRow>
      <ParentId>1</ParentId>
      <Order>6</Order>
      <Name>Комментарий 2</Name>
      <Rule>"""""".*?""""""</Rule>
      <Color>-32640</Color>
      <SingleLine>false</SingleLine>
      <RgKw>false</RgKw>
      <Case>false</Case>
      <Enabled>true</Enabled>
    </RuleRow>
  </Rules>
</SyntaxRule>";
    }
}
