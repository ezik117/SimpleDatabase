using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace simple_database
{
    public partial class frmKeywords : Form
    {
        /// <summary>
        /// ROWID редактируемого свойства
        /// </summary>
        public long PropertyID = -1;

        /// <summary>
        /// Определяет режим отображения формы: 
        /// - если false - то форма показывается в режиме управления ключевыми словами элемента
        /// - если true - то форма используется для построения строки запроса ключевых слов
        /// </summary>
        public bool KeywordsBuilderModeOnly = false;

        /// <summary>
        /// Если KeywordsBuilderModeOnly = true, то при закрытии формы будет содержать список
        /// выбранных ключевых слов через точку с запятой
        /// </summary>
        public string BuildedKeywords = "";

        /// <summary>
        /// Если KeywordsBuilderModeOnly = true, то при закрытии формы будет содержать список
        /// выбранных ROWID ключевых слов через запятую 
        /// </summary>
        public string BuildedKeywordsIds = "";

        /// <summary>
        /// Все ключевые слова для данной бД
        /// </summary>
        private DataTable keywords = null;

        /// <summary>
        /// BindingSource для списка всех ключевых слов
        /// </summary>
        private BindingSource bs_kw = null;

        /// <summary>
        /// BindingSource для списка всех активных ключевых слов
        /// </summary>
        private BindingSource bs_aw = null;

        /// <summary>
        /// Ключевые слова для данного элемента оглавления
        /// </summary>
        private DataTable property_keywords = null;

        public frmKeywords()
        {
            InitializeComponent();


        }

        /// <summary>
        /// Добавить ключевое слово
        /// </summary>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmClassEdit frm = new frmClassEdit();
            frm.Text = "Добавление ключевого слова";
            frm.tbClassName.Text = "";
            if (frm.ShowDialog() == DialogResult.OK)
            {
                string newKeyword = frm.tbClassName.Text.Trim().ToLower();
                if (newKeyword != "")
                {
                    long rowid = DATABASE.NewKeywordAdd(newKeyword, (long)VARS.main_form.tvClasses.SelectedNode.Tag);
                    if (rowid != -1)
                    {
                        keywords.Rows.Add(rowid, newKeyword);
                        property_keywords.Rows.Add(rowid, newKeyword);
                        btnSave.ImageKey = "exclamation";
                    }
                    else
                    {
                        MessageBox.Show("Ключевое слово уже сущестувует", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        /// <summary>
        /// Редактировать ключевое слово
        /// </summary>
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (lbAvailableKeywords.SelectedIndex < 0) return;

            if (MessageBox.Show("Данное действие будет иметь последствия для всех элементов оглавления. Продолжить?",
                    "Предупреждение", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                return;

            DataRowView drv = (DataRowView)bs_kw.Current;

            frmClassEdit frm = new frmClassEdit();
            frm.Text = "Редактирование ключевого слова";
            frm.tbClassName.Text = (string)drv["keyword"];
            if (frm.ShowDialog() == DialogResult.OK)
            {
                string newKeyword = frm.tbClassName.Text.Trim().ToLower();
                if (newKeyword != "")
                {
                    if (DATABASE.KeywordEdit((long)drv["ROWID"], (long)VARS.main_form.tvClasses.SelectedNode.Tag, newKeyword))
                    {
                        drv["keyword"] = newKeyword;
                        bs_kw.EndEdit();
                        keywords.AcceptChanges();
                        btnSave.ImageKey = "exclamation";
                    }
                    else
                    {
                        MessageBox.Show("Ключевое слово уже сущестувует", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        /// <summary>
        /// Удалить ключевое слово
        /// </summary>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lbAvailableKeywords.SelectedIndex < 0) return;

            if (MessageBox.Show("Данное действие будет иметь последствия для всех элементов оглавления. Продолжить?",
                                "Предупреждение", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                return;

            DataRowView drv = (DataRowView)bs_kw.Current;

            if (DATABASE.KeywordRemove((long)drv["ROWID"], (long)VARS.main_form.tvClasses.SelectedNode.Tag))
            {
                bs_kw.Remove(bs_kw.Current);
                bs_kw.EndEdit();
                keywords.AcceptChanges();
                btnSave.ImageKey = "exclamation";
            }
        }

        /// <summary>
        /// Сохранить ключевые слова
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveKeywords();
            btnSave.ImageKey = "Save-icon";
        }

        /// <summary>
        /// Проверка сохранения на закрытии формы
        /// </summary>
        private void frmHashtags_FormClosing(object sender, FormClosingEventArgs e)
        {
            var s = from row in property_keywords.AsEnumerable() select row.Field<string>("keyword");
            if (s.Count() != 0)
                BuildedKeywords = string.Join(";", s);
            else
                BuildedKeywords = "";

            var v = from row in property_keywords.AsEnumerable() select row.Field<long>("ROWID");
            if (v.Count() != 0)
                BuildedKeywordsIds = string.Join(",", v);
            else
                BuildedKeywordsIds = "";

            if (KeywordsBuilderModeOnly)
            {
                // просто выходим
                return;
            }

            if (btnSave.ImageKey == "exclamation")
            {
                DialogResult res = MessageBox.Show("Список не сохранен. Сохранить перед закрытием?",
                    "Предупреждение",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Warning);

                if (res == DialogResult.Cancel)
                {
                    e.Cancel = true;
                    return;
                }
                else if (res == DialogResult.Yes)
                {
                    SaveKeywords();
                }
            }
        }

        /// <summary>
        /// Сохранение ключевых слов в БД
        /// </summary>
        private void SaveKeywords()
        {
            if (!DATABASE.SaveParamKeywords(PropertyID, property_keywords))
            {
                MessageBox.Show("Ошибка сохранения", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Собирает и применяет фильтр который исключает выбранные ключевые слова для данного свойства 
        /// </summary>
        private void ApplyActiveKeywordsFilter()
        {
            string rowfilter = "";
            var s = from row in property_keywords.AsEnumerable() select row.Field<long>("ROWID");
            if (s.Count() != 0)
            {
                rowfilter = $"ROWID NOT IN ({string.Join(",", s)})";
                if (tbFastSearch.Text.Length != 0)
                    rowfilter += $"AND keyword LIKE '%{tbFastSearch.Text}%'";
            }
            else
            {
                keywords.DefaultView.RowFilter = "";
                if (tbFastSearch.Text.Length != 0)
                    rowfilter += $"keyword LIKE '%{tbFastSearch.Text}%'";
            }

            keywords.DefaultView.RowFilter = rowfilter;
        }

        /// <summary>
        /// Парсинг ключевых слов при загрузке формы
        /// </summary>
        private void frmHashtags_Load(object sender, EventArgs e)
        {
            property_keywords = new DataTable();
            DATABASE.ReadParamKeywords(PropertyID, ref property_keywords);
            bs_aw = new BindingSource();
            bs_aw.DataSource = property_keywords.DefaultView;
            lbKeyWords.DisplayMember = "keyword";
            lbKeyWords.DataSource = bs_aw;
            bs_aw.Sort = "keyword";

            keywords = new DataTable();
            DATABASE.ReadAllKeywords((long)VARS.main_form.tvClasses.SelectedNode.Tag, ref keywords);
            ApplyActiveKeywordsFilter();
            bs_kw = new BindingSource();
            bs_kw.DataSource = keywords;
            lbAvailableKeywords.DisplayMember = "keyword";
            lbAvailableKeywords.DataSource = bs_kw;
            bs_kw.Sort = "keyword";

            if (KeywordsBuilderModeOnly)
            {
                pnlHotButtons.Visible = false;
            }

            tbFastSearch.Focus();
        }

        /// <summary>
        /// Добавить ключевое слово из облака в список ключевых слов элемента
        /// </summary>
        private void btnAppendKeyword_Click(object sender, EventArgs e)
        {
            if (lbAvailableKeywords.SelectedIndex < 0) return;

            DataRowView drv = (DataRowView)bs_kw.Current;
            string newKeyword = (string)drv["keyword"];
            
            property_keywords.Rows.Add((long)drv["ROWID"], newKeyword);
            property_keywords.AcceptChanges();

            btnSave.ImageKey = "exclamation";

            // обновим фильтр
            ApplyActiveKeywordsFilter();
        }

        /// <summary>
        /// Удалить ключевое слово из списка ключевых слов элемента
        /// </summary>
        private void btnRemoveKeyword_Click(object sender, EventArgs e)
        {
            if (lbKeyWords.SelectedIndex < 0) return;

            bs_aw.Remove(bs_aw.Current);
            bs_aw.EndEdit();
            property_keywords.AcceptChanges();
            btnSave.ImageKey = "exclamation";

            // обновим фильтр
            ApplyActiveKeywordsFilter();
        }

        /// <summary>
        /// Удалить все ключевые слова из списка ключевых слов элемента
        /// </summary>
        private void btnRemoveAllKeywords_Click(object sender, EventArgs e)
        {
            if (lbKeyWords.Items.Count == 0) return;
            property_keywords.Rows.Clear();
            property_keywords.AcceptChanges();
            btnSave.ImageKey = "exclamation";

            // обновим фильтр
            ApplyActiveKeywordsFilter();
        }

        /// <summary>
        /// Удалить ключевое слово из списка ключевых слов элемента по двойному щелчку мыши
        /// </summary>
        private void lbKeyWords_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = lbKeyWords.IndexFromPoint(e.Location);
            if (index >= 0)
            {
                lbKeyWords.SelectedItem = lbKeyWords.Items[index];
                btnRemoveKeyword.PerformClick();
            }
        }

        /// <summary>
        /// Добавить ключевое слово из облака в список ключевых слов элемента
        /// </summary>
        private void lbAvailableKeywords_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = lbAvailableKeywords.IndexFromPoint(e.Location);
            if (index >= 0)
            {
                lbAvailableKeywords.SelectedItem = lbAvailableKeywords.Items[index];
                btnAppendKeyword.PerformClick();
            }
        }

        /// <summary>
        /// Быстрый фильтр по ключевым словам
        /// </summary>
        private void tbFastSearch_TextChanged(object sender, EventArgs e)
        {
            ApplyActiveKeywordsFilter();
        }

        /// <summary>
        /// Нажата клавиша в поле быстрого фильтра. Выберем слово
        /// </summary>
        private void tbFastSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && lbAvailableKeywords.Items.Count != 0)
            {
                btnAppendKeyword.PerformClick();
                tbFastSearch.Text = "";
                e.Handled = true;
            }
        }
    }
}
