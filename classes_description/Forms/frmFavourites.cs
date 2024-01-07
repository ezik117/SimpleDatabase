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
    public partial class frmFavourites : Form
    {

        /// <summary>
        /// Конструктор формы
        /// </summary>
        public frmFavourites()
        {
            InitializeComponent();

            dgv.DataSource = DATABASE.favourites.DefaultView;
            DATABASE.favourites.DefaultView.Sort = "class ASC, property ASC";
        }

        /// <summary>
        /// Добавить текущий узел оглавления в закладки.
        /// </summary>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            PROPERTY.AddPropertyToFavourites(VARS.main_form.tvProps.SelectedNode);

            SelectCurrentPropertyIfAny();
        }

        /// <summary>
        /// Удалить строку из закладок
        /// </summary>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count == 0) return;

            DataRowView row = (DataRowView)dgv.SelectedRows[0].DataBoundItem;
            DATABASE.DeleteFavourites(false, (long)row["class_id"], (long)row["property_id"]);

            // снимем признак того, что элемент добавлен в закладки и отобразим значки
            PROPERTY.propertyInfo.isFavourite = false;
            VARS.main_form.pbMarks.Refresh();

            SelectCurrentPropertyIfAny();
        }

        /// <summary>
        /// Ищет заданный узел в каталоге
        /// </summary>
        /// <param name="rowid">ROWID (Tag) нужного узла</param>
        /// <returns>Найденный объект TreeNode или null</returns>
        private TreeNode FindClassNode(long rowid)
        {
            TreeNode ret = null;
            foreach (TreeNode node in VARS.main_form.tvClasses.Nodes)
                if ((long)node.Tag == rowid)
                {
                    ret = node;
                    break;
                }
            return ret;
        }

        /// <summary>
        /// Ищет заданный узел в TV.
        /// </summary>
        /// <param name="node">Родительский узел</param>
        /// <param name="rowid">ROWID (Tag) нужного узла</param>
        /// <returns>Найденный объект TreeNode или null</returns>
        private TreeNode FindNodeRecursively(TreeNode node, long rowid)
        {
            TreeNode ret = null;
            foreach (TreeNode n in node.Nodes)
            {
                if ((long)n.Tag == rowid)
                {
                    ret = n;
                    break;
                }
                else
                {
                    if (n.Nodes.Count > 0)
                    {
                        ret = FindNodeRecursively(n, rowid);
                        if (ret != null) break;
                    }
                }
            }

            return ret;
        }

        /// <summary>
        /// Удалить все
        /// </summary>
        private void btnDeleteAll_Click(object sender, EventArgs e)
        {
            if (dgv.Rows.Count == 0) return;

            if (MessageBox.Show("Очистить все закладки?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) return;

            DATABASE.DeleteFavourites(true, -1, -1);

            // снимем признак того, что элемент добавлен в закладки и отобразим значки
            PROPERTY.propertyInfo.isFavourite = false;
            VARS.main_form.pbMarks.Refresh();
        }

        /// <summary>
        /// Закрыть форму нажатием ESC
        /// </summary>
        private void frmFavourites_KeyDown(object sender, KeyEventArgs e)
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

            TreeNode class_node = FindClassNode((long)row["class_id"]);
            if (class_node != null)
            {
                VARS.property_update_finished = false;
                VARS.main_form.tvClasses.SelectedNode = null;
                VARS.main_form.tvClasses.SelectedNode = class_node;
                while (VARS.property_update_finished != true)
                    Application.DoEvents();
            }

            TreeNode prop_node = FindNodeRecursively(VARS.main_form.tvProps.Nodes[0], (long)row["property_id"]);
            if (prop_node != null)
            {
                VARS.main_form.tvProps.SelectedNode = prop_node;
            }

            VARS.main_form.tvProps.Focus();
            VARS.main_form.tvProps.SelectedNode.Expand();

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
                if ((long)dr.Cells["favourites_class_id"].Value == (long)VARS.main_form.tvClasses.SelectedNode.Tag &&
                    (long)dr.Cells["favourites_property_id"].Value == (long)VARS.main_form.tvProps.SelectedNode.Tag)
                    dr.Selected = true;
            }
        }

        /// <summary>
        /// Действия после загрузки формы
        /// </summary>
        private void frmFavourites_Load(object sender, EventArgs e)
        {
            SelectCurrentPropertyIfAny();
        }
    }
}
