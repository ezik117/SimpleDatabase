using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace simple_database
{
    /// <summary>
    /// Класс для поиска в оглавлении или по всем текстам оглавления
    /// </summary>
    public partial class frmSearchProperty : Form
    {
        private TreeNode parentNode;
        private TreeView tv;
        private bool initiateSearch = true;

        /// <summary>
        /// Найденные узлы
        /// </summary>
        private List<TreeNode> found;

        /// <summary>
        /// Найденные ROWID узлов
        /// </summary>
        private List<long> found_indexes;

        /// <summary>
        /// Все узлы в TV начиная С выделенного
        /// </summary>
        private List<TreeNode> allNodes = new List<TreeNode>();

        /// <summary>
        /// Индекс найденного узла
        /// </summary>
        private int foundIndex;

        /// <summary>
        /// Индекс последнего найденного узла
        /// </summary>
        private int lastFoundIndex;

        /// <summary>
        /// Позиция найденного слова при поиске текста
        /// </summary>
        private int foundTextIndex;

        /// <summary>
        /// Искомый текст в нижнем регистре
        /// </summary>
        private string searchPattern = "";

        /// <summary>
        /// Загруженный текст текущего узла
        /// </summary>
        private string loadedText = "";

        /// <summary>
        /// Текущий узел (используется только для поиска текста)
        /// </summary>
        TreeNode currentNode = null;

        /// <summary>
        /// Ссылка на компонент RichTextBox с текстом (используется только для поиска текста)
        /// </summary>
        public RichTextBox tb;

        /// <summary>
        /// Направление поиска: 1 = вниз по дереву, -1 = вверх по дереву
        /// </summary>
        private int SearchDirection = 1;

        /// <summary>
        /// Список выбранных ROWID ключевых слов через запятую
        /// </summary>
        private string BuildedKeywordsIds = "";

        /// <summary>
        /// Класс содержащий информацию о последнем выбираемом узле.
        /// </summary>
        private class LastSelectedNode
        {
            /// <summary>
            /// Выбранный узел
            /// </summary>
            public TreeNode node = null;

            /// <summary>
            /// Индекс изображения выбранного узла
            /// </summary>
            public int selectedImageIndex = -1;

            /// <summary>
            /// Сброс переменных
            /// </summary>
            public void Clear()
            {
                node = null;
                selectedImageIndex = -1;
            }

            /// <summary>
            /// Устанавливает внутренние переменные данными из выбранного узла.
            /// Меняет изображение выбранного узла на выделенное.
            /// </summary>
            /// <param name="n"></param>
            public void Select(TreeNode n)
            {
                Deselect();
                if (n != null)
                {
                    node = n;
                    selectedImageIndex = n.SelectedImageIndex;
                    n.SelectedImageIndex = 8;
                }
            }

            /// <summary>
            /// Восстанавливает изображение выбранного узла.
            /// Сбраывает внутренние переменные
            /// </summary>
            public void Deselect()
            {
                if (node == null) return;

                node.SelectedImageIndex = selectedImageIndex;
                Clear();
            }
        }

        /// <summary>
        /// Экземпляр класса содержащий информацию о последнем выбираемом узле.
        /// </summary>
        private LastSelectedNode lastSelectedNode = new LastSelectedNode();

        /// <summary>
        /// Конструктор
        /// </summary>
        public frmSearchProperty()
        {
            InitializeComponent();
            lblStatus.Text = "";
        }

        /// <summary>
        /// Кнопка: Искать вперед
        /// </summary>
        private void btnFindNext_Click(object sender, EventArgs e)
        {
            lastSelectedNode.Deselect();
            SearchDirection = 1;
            FindNext();
        }

        /// <summary>
        /// Кнопка: Искать назад
        /// </summary>
        private void btnFindPrevious_Click(object sender, EventArgs e)
        {
            lastSelectedNode.Deselect();
            SearchDirection = -1;
            FindNext();
        }

        /// <summary>
        /// Искать далее
        /// </summary>
        private void FindNext()
        {
            if (rbSearchInContents.Checked)
            {
                // Поиск в оглавлении
                if ((!initiateSearch) && (searchPattern != tbSearchPattern.Text))
                    InitSearch(VARS.main_form.tvProps, true);

                if (initiateSearch)
                {
                    searchPattern = tbSearchPattern.Text;
                    found = PROPERTY.SearchProperties(searchPattern, parentNode);
                    initiateSearch = false;
                }

                foundIndex += SearchDirection;

                if (foundIndex < found.Count && foundIndex >= 0)
                {
                    // информация найдена
                    tv.SelectedNode = found[foundIndex];
                    lastSelectedNode.Select(tv.SelectedNode);

                    lastFoundIndex = foundIndex;
                    Application.DoEvents();
                }
                else
                {
                    if (MessageBox.Show("Поиск закончен. Начать заново?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    {
                        if (SearchDirection > 0)
                        {
                            foundIndex = -1;
                            btnFindNext.PerformClick();
                            return;
                        }
                        else
                        {
                            foundIndex = found.Count;
                            btnFindPrevious.PerformClick();
                            return;
                        }
                    }
                    else
                    {
                        foundIndex = lastFoundIndex;
                        return;
                    }
                }
            }
            else if (rbSearchInTexts.Checked)
            {
                if (tbSearchPattern.Text.Trim() == "") return;

                // Поиск по тексту
                lblStatus.Text = "Поиск...";
                Application.DoEvents();

                searchPattern = tbSearchPattern.Text.ToLower();

                int loop = 1;

                while (loop > 0)
                {
                    if (foundTextIndex < 0)
                    {
                        foundIndex += SearchDirection;

                        if (foundIndex >= allNodes.Count || foundIndex < 0)
                        {
                            foundTextIndex = -1;
                            loop = 0;
                            tb.SelectionLength = 0;

                            if (MessageBox.Show("Поиск закончен. Начать заново?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                            {
                                if (SearchDirection > 0)
                                {
                                    foundIndex = -1;
                                    btnFindNext.PerformClick();
                                    return;
                                }
                                else
                                {
                                    foundIndex = allNodes.Count;
                                    btnFindPrevious.PerformClick();
                                    return;
                                }
                            }
                            else
                            {
                                return;
                            }
                        }
                        else
                        {
                            loadedText = DATABASE.LoadText((long)allNodes[foundIndex].Tag);
                        }
                    }

                    foundTextIndex = loadedText.IndexOf(searchPattern, foundTextIndex >= 0 ? foundTextIndex : 0);
                    if (foundTextIndex >= 0)
                    {
                        // информация найдена
                        lastFoundIndex = foundIndex;
                        if (!tv.SelectedNode.Equals(allNodes[foundIndex]))
                        {
                            tv.SelectedNode = allNodes[foundIndex];
                        }
                        lastSelectedNode.Select(tv.SelectedNode);

                        tb.SelectionStart = foundTextIndex;
                        tb.SelectionLength = searchPattern.Length;
                        tb.ScrollToCaret();
                        foundTextIndex += searchPattern.Length;
                        loop = 0;
                    }
                    else
                    {

                    }
                }

                lblStatus.Text = "";
            }
            else if (rbSearchInTags.Checked)
            {
                if (tbSearchPattern.Text.Trim() == "") return;

                // Поиск по ключевым словам
                lblStatus.Text = "Поиск...";
                Application.DoEvents();

                if (initiateSearch)
                {
                    found_indexes = DATABASE.FindParamsWithKeywords(BuildedKeywordsIds);
                    foundIndex = SearchDirection > 0 ? -1 : allNodes.Count;
                    initiateSearch = false;
                }

                bool loop = true;

                while (loop)
                {
                    foundIndex += SearchDirection;

                    if (foundIndex >= allNodes.Count || foundIndex < 0)
                    {
                        loop = false;
                        tb.SelectionLength = 0;
                        
                        if (MessageBox.Show("Поиск закончен. Начать заново?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                        {
                            VARS.main_form.tvProps.SelectedNode = null;
                            Application.DoEvents();

                            if (SearchDirection > 0)
                            {
                                foundIndex = -1;
                                btnFindNext.PerformClick();
                                return;
                            }
                            else
                            {
                                foundIndex = allNodes.Count;
                                btnFindPrevious.PerformClick();
                                return;
                            }
                        }
                        else
                        {
                            foundIndex = lastFoundIndex;
                            return;
                        }
                    }
                    else
                    {
                        if (found_indexes.Contains((long)allNodes[foundIndex].Tag))
                        {
                            // Информация найдена
                            tv.SelectedNode = allNodes[foundIndex];
                            lastSelectedNode.Select(tv.SelectedNode);
                            loop = false;
                        }
                    }
                }

                lblStatus.Text = "";
            }
        }

        /// <summary>
        /// Рекурсивно строит список 'allNodes' содержащий коллекцию TreeNode дочерних к указанному узлу 
        /// </summary>
        /// <param name="node">Родительский узел</param>
        private void FindAllNodes(TreeNode node)
        {
            foreach (TreeNode n in node.Nodes)
            {
                allNodes.Add(n);
                if (n.Nodes.Count != 0)
                    FindAllNodes(n);
            }
        }

        IEnumerable<TreeNode> Collect(TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                yield return node;
                foreach (var child in Collect(node.Nodes))
                    yield return child;
            }
        }

        /// <summary>
        /// Сбрасывает все переменные поиска в начальное состояние.
        /// Заполняет переменную allNodes всеми дочерними узлами от текущего выбранного узла.
        /// </summary>
        /// <param name="tv">Объект TreeView в котором идет поиск</param>
        /// <param name="again">Если True, то это повторный поис</param>
        public void InitSearch(TreeView tv, bool again)
        {
            this.tv = tv;
            if (!again) parentNode = tv.SelectedNode;
            initiateSearch = true;
            found = null;
            foundIndex = -1;
            lastFoundIndex = -1;

            currentNode = tv.SelectedNode;
            // получим все узлы в TV и запишем их в переменной "allNodes"
            FindAllNodes(tv.SelectedNode);
            foundTextIndex = -1;
        }

        /// <summary>
        /// Закрыть форму нажатием ESC
        /// </summary>
        private void frmSearchProperty_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) Close();
        }

        /// <summary>
        /// Выбран или снят режим поиска по ключевым словам
        /// </summary>
        private void rbSearchInTags_CheckedChanged(object sender, EventArgs e)
        {
            if (rbSearchInTags.Checked)
            {
                lblBuildKeywords.Visible = true;
                tbSearchPattern.Enabled = false;
            }
            else
            {
                lblBuildKeywords.Visible = false;
                tbSearchPattern.Enabled = true;
            }
        }

        /// <summary>
        /// Вызвано окно сбора строки запроса по ключевым словам
        /// </summary>
        private void llblBuildKeywords_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmKeywords frm = new frmKeywords();
            frm.KeywordsBuilderModeOnly = true;
            frm.BuildedKeywords = "";
            frm.BuildedKeywordsIds = "";
            frm.ShowDialog();

            tbSearchPattern.Text = frm.BuildedKeywords;
            BuildedKeywordsIds = frm.BuildedKeywordsIds;
            initiateSearch = true;
        }

        /// <summary>
        /// Действия при закрытии формы
        /// </summary>
        private void frmSearchProperty_FormClosing(object sender, FormClosingEventArgs e)
        {
            lastSelectedNode.Deselect();
        }
    }
}
