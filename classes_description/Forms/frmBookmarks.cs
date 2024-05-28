using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace simple_database
{
    /// <summary>
    /// Форма управления закладками
    /// </summary>
    public partial class frmBookmarks : Form
    {

        /// <summary>
        /// Конструктор формы
        /// </summary>
        public frmBookmarks()
        {
            InitializeComponent();

            dgv.DataSource = DATABASE.bookmarks.DefaultView;
            DATABASE.bookmarks.DefaultView.RowFilter = $"database='{Path.GetFileNameWithoutExtension(DATABASE.FileName)}'";
            DATABASE.bookmarks.DefaultView.Sort = "database ASC, class ASC, property ASC";
        }

        /// <summary>
        /// Добавить текущий узел оглавления в закладки.
        /// </summary>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            PROPERTY.AddPropertyToBookmark(VARS.main_form.tvProps.SelectedNode);

            SelectCurrentPropertyIfAny();
        }

        /// <summary>
        /// Удалить строку из закладок
        /// </summary>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count == 0) return;

            DataRowView row = (DataRowView)dgv.SelectedRows[0].DataBoundItem;
            row.Delete();

            // снимем признак того, что элемент добавлен в закладки и отобразим значки
            PROPERTY.propertyInfo.isBookmarked = false;
            VARS.main_form.pbMarks.Refresh();

            SelectCurrentPropertyIfAny();
        }

        /// <summary>
        /// Удалить все
        /// </summary>
        private void btnDeleteAll_Click(object sender, EventArgs e)
        {
            if (dgv.Rows.Count == 0) return;

            if (MessageBox.Show("Очистить все закладки?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) return;

            DATABASE.bookmarks.Clear();

            // снимем признак того, что элемент добавлен в закладки и отобразим значки
            PROPERTY.propertyInfo.isBookmarked = false;
            VARS.main_form.pbMarks.Refresh();
        }

        /// <summary>
        /// Закрыть форму нажатием ESC
        /// </summary>
        private void frmBookmarks_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) Close();
        }

        /// <summary>
        /// Быстрый переход по ссылке по двойному щелчку мыши
        /// </summary>
        private void dgv_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgv.SelectedRows.Count == 0) return;

            DataRowView row = (DataRowView)dgv.SelectedRows[0].DataBoundItem;

            string dbname = Path.GetFileNameWithoutExtension(DATABASE.FileName);

            HELPER.MoveToElement((string)row["database"], (long)row["class_id"], (long)row["property_id"]);

            Close();
        }

        /// <summary>
        /// Выделить выбранный в основном окне элемент оглавления
        /// </summary>
        private void SelectCurrentPropertyIfAny()
        {
            dgv.ClearSelection();

            foreach (DataGridViewRow dr in dgv.Rows)
            {
                if ((long)dr.Cells["bookmark_class_id"].Value == (long)VARS.main_form.tvClasses.SelectedNode.Tag &&
                    (long)dr.Cells["bookmark_property_id"].Value == (long)VARS.main_form.tvProps.SelectedNode.Tag)
                    dr.Selected = true;
            }
        }

        /// <summary>
        /// Действия после загрузки формы
        /// </summary>
        private void frmBookmarks_Load(object sender, EventArgs e)
        {
            SelectCurrentPropertyIfAny();
        }

        /// <summary>
        /// Кнопка "Показать\скрыть закладки всех баз данных.
        /// </summary>
        private void btnShowCurrentDbOnly_Click(object sender, EventArgs e)
        {
            if ((string)btnShowCurrentDbOnly.Tag == "0")
            {
                btnShowCurrentDbOnly.Tag = "1";
                dgv.Columns["bookmarkDb"].Visible = true;
                btnShowCurrentDbOnly.Image = Properties.Resources.database_16_grayed;
                DATABASE.bookmarks.DefaultView.RowFilter = "";

            }
            else
            {
                btnShowCurrentDbOnly.Tag = "0";
                dgv.Columns["bookmarkDb"].Visible = false;
                btnShowCurrentDbOnly.Image = Properties.Resources.database_deny_16_grayed;
                DATABASE.bookmarks.DefaultView.RowFilter = $"database='{Path.GetFileNameWithoutExtension(DATABASE.FileName)}'";

            }
        }
    }
}
