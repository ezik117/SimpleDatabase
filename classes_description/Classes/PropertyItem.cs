using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace simple_database
{
    using SqlRows = List<Dictionary<string, object>>;

    /// <summary>
    /// Класс для управления оглавлением и связанной с ним информацией
    /// </summary>
    public static class PROPERTY
    {
        /// <summary>
        /// Если установлено в True, то события от выбора пользователем класса будут блокироваться.
        /// Нужно для отключения автоматического считывания свойств типа Description при создании TreeView.
        /// </summary>
        private static bool stopEventProcessing = false;

        /// <summary>
        /// Экзепляр класса описывающий свойства текущего выбранного элемента оглавления
        /// </summary>
        public static PropertyInfo propertyInfo = new PropertyInfo();

        /// <summary>
        /// Типы отображаемых значков в узлах оглавления
        /// </summary>
        public enum Types : int
        {
            Document = 7,
            Root = 0,
            TriangleBlue = 1,
            SquarePink = 2,
            CircleRed = 3,
            FolderYellow = 4,
            FolderBlue = 5,
            FolderGreen = 6,
            ArrowRed = 8,
            Attachment = 9,
            FolderRed = 10,
            FolderMagenta = 11,
            FolderGrey = 12,
            Class = 13,
            DocumentImportant = 14,
        }

        /// <summary>
        /// Загружает параметры выбранного каталога в TreeView.
        /// </summary>
        /// <param name="class_id">ROWID записи класса.</param>
        /// <param name="main">Ссылка на главную форму.</param>
        public static void Load(long class_id, Form1 main)
        {
            stopEventProcessing = true;

            SqlRows rows = DATABASE.LoadProperties(class_id);
            TreeNode root = main.tvProps.Nodes[0];

            foreach (Dictionary<string, object> r in rows)
            {
                if (DBNull.Value.Equals(r["parent"]))
                {
                    TreeNode t = new TreeNode();
                    t.Text = (string)r["name"];
                    t.ImageIndex = t.SelectedImageIndex = (int)(long)r["type"];
                    t.Tag = (long)r["id"];
                    root.Nodes.Add(t);
                }
                else
                {
                    SearchNodeByTag((long)r["parent"], main.tvProps.Nodes[0], out TreeNode parent);
                    if (parent != null)
                    {
                        TreeNode t = new TreeNode();
                        t.Text = (string)r["name"];
                        t.ImageIndex = t.SelectedImageIndex = (int)(long)r["type"];
                        t.Tag = (long)r["id"];

                        parent.Nodes.Add(t);
                    }
                }

            }

            main.tvProps.Sort();

            stopEventProcessing = false;

            main.tvProps.SelectedNode = main.tvProps.Nodes[0];
            main.tvProps.Focus();
        }

        /// <summary>
        /// Создает элемент оглавления.
        /// </summary>
        /// <param name="main">Ссылка на главную форму.</param>
        public static void Create(Form1 main)
        {
            TreeNode currentProperty = main.tvProps.SelectedNode;
            if (currentProperty == null) return;

            // найдем нумерацию, если есть
            string numbering = "";
            if (currentProperty.Nodes.Count == 0)
            {
                numbering = "01";
            }
            else
            {
                int max = 0;
                foreach(TreeNode n in currentProperty.Nodes)
                {
                    if (Regex.IsMatch(n.Text, @"^\d+\. +"))
                    {
                        int num = int.Parse(n.Text.Substring(0, 2));
                        max = Math.Max(max, num);
                    }
                }
                max++;
                numbering = max.ToString("D2");
            }

            frmPropertyEdit frm = new frmPropertyEdit();
            frm.IsItNewItem = true;
            frm.tbPropertyName.Text = numbering == "" ? "" : numbering + ". ";
            frm.PropertyType = (int)Types.Document;

            if (frm.ShowDialog() != DialogResult.OK) return;

            long id;
            string propName = frm.tbPropertyName.Text.Trim();

            if (frm.rbAttachment.Checked)
            {
                // выбрано вложение.
                OpenFileDialog of = new OpenFileDialog();
                if (of.ShowDialog() == DialogResult.OK)
                {
                    if (propName == "") propName = Path.GetFileName(of.FileName);

                    id = DATABASE.AttachmentInsert(-1, (long)main.tvClasses.SelectedNode.Tag,
                                                   propName, frm.PropertyType,
                                                   (long)currentProperty.Tag, "", of.FileName);
                }
                else
                    return;
            }
            else
            {
                // выбран значок
                id = DATABASE.SaveProperty(-1, (long)main.tvClasses.SelectedNode.Tag,
                                            propName, frm.PropertyType,
                                            (long)currentProperty.Tag, "");
            }

            main.slblLastUpdate.Text = "Last update: " + DATABASE.SetLastUpdate();

            main.paramTextEditor.userAction2();

            TreeNode t = new TreeNode();
            t.Text = propName;
            t.ImageIndex = t.SelectedImageIndex = frm.PropertyType;
            t.Tag = id;

            currentProperty.Nodes.Add(t);
            main.tvProps.SelectedNode = t;
            main.tvProps.Focus();
        }

        /// <summary>
        /// Обновить (редактировать) свойство.
        /// </summary>
        /// <param name="main">Ссылка на главную форму.</param>
        public static void Update(Form1 main)
        {
            TreeNode currentProperty = main.tvProps.SelectedNode;
            if (currentProperty == null) return;
            if ((long)currentProperty.Tag == -1) return;

            frmPropertyEdit frm = new frmPropertyEdit();
            frm.IsItNewItem = false;
            frm.tbPropertyName.Text = currentProperty.Text;
            frm.PropertyType = currentProperty.ImageIndex;

            if (frm.ShowDialog() != DialogResult.OK) return;

            string propName = frm.tbPropertyName.Text.Trim();

            if (frm.rbAttachment.Checked)
            {
                // выбрано вложение.
                
                OpenFileDialog of = new OpenFileDialog();
                if (of.ShowDialog() == DialogResult.OK)
                {
                    if (propName == "") propName = Path.GetFileName(of.FileName);

                    DATABASE.AttachmentUpdate((long)currentProperty.Tag, (long)main.tvClasses.SelectedNode.Tag,
                                                propName, frm.PropertyType,
                                                (long)currentProperty.Parent.Tag, "", of.FileName);
                }
                else
                    return;
            }
            else
            {
                long id = DATABASE.SaveProperty((long)currentProperty.Tag, (long)main.tvClasses.SelectedNode.Tag,
                                               frm.tbPropertyName.Text.Trim(), frm.PropertyType,
                                               (long)currentProperty.Parent.Tag, main.paramTextEditor.txtBox.Rtf);
            }
            main.paramTextEditor.userAction1();

            currentProperty.Text = propName;
            currentProperty.ImageIndex = currentProperty.SelectedImageIndex = frm.PropertyType;
            main.slblLastUpdate.Text = "Last update: " + DATABASE.SetLastUpdate();
        }

        /// <summary>
        /// Обновляет только описание параметра.
        /// </summary>
        /// <param name="main">Ссылка на главную форму.</param>
        public static void UpdateDescription(Form1 main)
        {
            if (main.tvProps.SelectedNode == null) return;

            DATABASE.UpdatePropertyDescription((long)main.tvProps.SelectedNode.Tag, main.paramTextEditor.txtBox.Rtf);
            main.paramTextEditor.userAction1();
            main.slblLastUpdate.Text = "Last update: " + DATABASE.SetLastUpdate();
        }

        /// <summary>
        /// Удаляет параметр с подчиненными иерархически параметрами, если есть.
        /// </summary>
        /// <param name="main">Ссылка на главную форму.</param>
        public static void Delete(Form1 main)
        {
            if (main.tvProps.SelectedNode == null) return;
            if ((long)main.tvProps.SelectedNode.Tag == -1)
            {
                if (MessageBox.Show("Удалить все параметры выбранного каталога?",
                                "",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Warning,
                                MessageBoxDefaultButton.Button2) == DialogResult.No) return;

                DATABASE.DeleteProperties((long)main.tvClasses.SelectedNode.Tag);
                ClassItem.NodeChanged(main);
            }
            else
            {
                if (MessageBox.Show("Удалить параметр и все зависимые иерархически параметры и элементы?",
                                    "",
                                    MessageBoxButtons.YesNo,
                                    MessageBoxIcon.Warning,
                                    MessageBoxDefaultButton.Button2) == DialogResult.No) return;

                DATABASE.DeleteProperty((long)main.tvProps.SelectedNode.Tag);
                main.tvProps.Nodes.Remove(main.tvProps.SelectedNode);
            }

            main.slblLastUpdate.Text = "Last update: " + DATABASE.SetLastUpdate();
        }

        /// <summary>
        /// Считывает повторно описание.
        /// </summary>
        /// <param name="main">Ссылка на главную форму.</param>
        public static void DescriptionReload(Form1 main)
        {
            TreeNode currentProperty = main.tvProps.SelectedNode;
            if (currentProperty == null) return;
            if (main.btnDescSave.ImageKey == "save") return;

            if (MessageBox.Show("Все несохраненные данные будут потеряны. Продолжить?",
                                "",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Warning,
                                MessageBoxDefaultButton.Button2) == DialogResult.No) return;

            PropertyChanged(main);
        }

        /// <summary>
        /// Выбран элемент оглавления каталога (автоматически или щелчком пользователя).
        /// Считывает все взаимосвязанные со свойством объекты.
        /// </summary>
        /// <param name="main">Ссылка на главную форму.</param>
        public static void PropertyChanged(Form1 main)
        {
            TreeNode currentProperty = main.tvProps.SelectedNode;
            if (stopEventProcessing) return;

            propertyInfo.Clear();
            propertyInfo.id = (long)currentProperty.Tag;
            if (propertyInfo.id < 0) propertyInfo.isRoot = true;

            if ((long)currentProperty.Tag == -1)
            {
                // выбран корневой узел
                VARS.main_form.pbMarks.Refresh();
                main.paramTextEditor.txtBox.ReadOnly = true;
                main.paramTextEditor.userAction2();
                return;
            }
            else
            {
                main.paramTextEditor.txtBox.ReadOnly = false;
            }

            // description
            SqlRows r = DATABASE.LoadProperty((long)currentProperty.Tag);
            main.paramTextEditor.txtBox.Rtf = (DBNull.Value.Equals(r[0]["description"]) ? "" : (string)r[0]["description"]);
            main.paramTextEditor.userAction1();

            // установим свойства текущего элемента оглавления
            DataRow[] dr = DATABASE.bookmarks.Select($"class_id={VARS.main_form.tvClasses.SelectedNode.Tag} and property_id={VARS.main_form.tvProps.SelectedNode.Tag}");
            propertyInfo.isBookmarked = dr.Length != 0;

            dr = DATABASE.favourites.Select($"class_id={VARS.main_form.tvClasses.SelectedNode.Tag} and property_id={VARS.main_form.tvProps.SelectedNode.Tag}");
            propertyInfo.isFavourite = dr.Length != 0;

            DATABASE.ReadParamKeywords((long)currentProperty.Tag, ref DATABASE.keywords);
            propertyInfo.thereAreKeywords = DATABASE.keywords.Rows.Count != 0;

            propertyInfo.BuildToolTip();
            VARS.main_form.toolTip1.SetToolTip(VARS.main_form.pbMarks, propertyInfo.Tip);

            VARS.main_form.pbMarks.Refresh();
        }

        /// <summary>
        /// Меняет родителя параметра.
        /// </summary>
        /// <param name="id">ROWID параметра</param>
        /// <param name="new_parent_id">Новый ROWID родительского элемента</param>
        /// <param name="main">Ссылка на главную форму.</param>
        public static void ChangeParent(long id, long new_parent_id, Form1 main)
        {
            DATABASE.ChangePropertyParent(id, new_parent_id);
        }


        /// <summary>
        /// Распаковывает вложение в папку TEMP и открывает его.
        /// </summary>
        /// <param name="main">Ссылка на главную форму.</param>
        public static void ExtractAttachment(Form1 main)
        {
            TreeNode currentProperty = main.tvProps.SelectedNode;
            if (currentProperty == null) return;

            string fileName = DATABASE.AttachmentGetFilename((long)currentProperty.Tag);

            SaveFileDialog sd = new SaveFileDialog();
            sd.InitialDirectory = Path.Combine(Application.StartupPath, "temp");
            sd.FileName = fileName;
            sd.Filter = $"{Path.GetExtension(fileName).TrimStart('.')}|*{Path.GetExtension(fileName)}|*.*|*.*";
            sd.AddExtension = true;
            if (sd.ShowDialog() != DialogResult.OK) return;

            bool result = DATABASE.AttachmentExtract((long)currentProperty.Tag, sd.FileName);
        }

        /// <summary>
        /// Извлекает и запускает указанное вложение
        /// </summary>
        /// <param name="main">Ссылка на главную форму.</param>
        public static void ExtractAndRunAttachment(Form1 main, bool openWith=false)
        {
            TreeNode currentProperty = main.tvProps.SelectedNode;
            if (currentProperty == null) return;

            string fileName = DATABASE.AttachmentGetFilename((long)currentProperty.Tag);
            fileName = Path.Combine(Application.StartupPath, "temp", fileName);
            bool result = DATABASE.AttachmentExtract((long)currentProperty.Tag, fileName);

            if (result)
            {
                if (openWith)
                    System.Diagnostics.Process.Start("rundll32.exe", $"shell32.dll,OpenAs_RunDLL {fileName} ");
                else
                    System.Diagnostics.Process.Start(fileName);
            }
        }

        /// <summary>
        /// Рекурсивно ищет TreeNode с заданным Tag. Результатом является объект TreeNode или null.
        /// </summary>
        /// <param name="id">Значение Tag.</param>
        /// <param name="first">Родительская TreeNode с которой начнется поиск.</param>
        /// <param name="result">Результат выполнения. Объект TreeNode или null.</param>
        private static void SearchNodeByTag(long id, TreeNode first, out TreeNode result)
        {
            result = null;
            foreach (TreeNode t in first.Nodes)
            {
                if ((long)t.Tag == id)
                {
                    result = t;
                    return;
                }

                if (t.Nodes.Count > 0) SearchNodeByTag(id, t, out result);
                if (result != null) return;
            }
        }

        /// <summary>
        /// Возвращает список всех узлов где текст содержит искомую строчку.
        /// Поиск регистронезависимый. Функция используется только как вызываемая из SearchProperties.
        /// </summary>
        /// <param name="text">Тест для поиска.</param>
        /// <param name="first">Узел с которого ищем.</param>
        /// <param name="result">Список узлов.</param>
        private static void SearchNodesByText(string text, TreeNode first, ref List<TreeNode> result)
        {
            foreach (TreeNode t in first.Nodes)
            {
                if (t.Text.IndexOf(text, StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    result.Add(t);
                }

                if (t.Nodes.Count > 0) SearchNodesByText(text, t, ref result);
            }
        }

        /// <summary>
        /// Ищет все свойства в имени которого содержится искомый текст.
        /// </summary>
        /// <param name="text">Тест для поиска.</param>
        /// <param name="fromNode">Узел с которого ищем.</param>
        /// <returns>Список узлов.</returns>
        public static List<TreeNode> SearchProperties(string text, TreeNode fromNode)
        {
            if (fromNode == null && text.Trim() == "") return null;

            List<TreeNode> result = new List<TreeNode>();
            SearchNodesByText(text, fromNode, ref result);
            return result;
        }

        /// <summary>
        /// Проверяет на несохраненные данные описания свойства. Запрашивает пользователя.И в зависимости от ответа
        /// либо сохраняет, либо не сохраняет данные. Возвращает булево значение для использования в аргументе Cancel,
        /// позволяющее отменить операцию.
        /// </summary>
        /// <param name="main">Ссылка на главную форму.</param>
        /// <returns>True - пользователь хочет отменить действие. False-можно продолжить.</returns>
        public static bool CheckForUnsavedDesc(Form1 main)
        {
            if (main.btnDescSave.ImageKey == "notsaved")
            {
                DialogResult res = MessageBox.Show("Имеются несохраненные данные (описание свойства). Сохранить?",
                                "",
                                MessageBoxButtons.YesNoCancel,
                                MessageBoxIcon.Warning,
                                MessageBoxDefaultButton.Button1);

                if (res == DialogResult.Cancel)
                {
                    return true;
                }
                else if (res == DialogResult.No)
                {
                    main.paramTextEditor.userAction1();
                    return false;
                }

                main.btnDescSave.PerformClick();
            }

            return false;
        }

        /// <summary>
        /// Поднимает узел вверх на одну позицию. Это нужно в частности для нумерации.
        /// </summary>
        /// <param name="tv">Головной компонент TreeView в котором производятся действия.</param>
        /// <param name="node">Перемещаемый узел</param>
        public static void MoveUp(TreeView tv, TreeNode node)
        {
            node = tv.SelectedNode;
            TreeNode parent = node.Parent;
            TreeNode prev = node.PrevNode;

            if (parent == null || prev == null) return;

            tv.BeginUpdate();
            parent.TreeView.Sorted = false;
            node.Remove();
            parent.Nodes.Insert(prev.Index, node);

            tv.EndUpdate();
            tv.SelectedNode = node;
        }

        /// <summary>
        /// Поднимает узел вверх на одну позицию. Это нужно в частности для нумерации.
        /// </summary>
        /// <param name="tv">Головной компонент TreeView в котором производятся действия.</param>
        /// <param name="node">Перемещаемый узел</param>
        public static void MoveDown(TreeView tv, TreeNode node)
        {
            node = tv.SelectedNode;
            TreeNode parent = node.Parent;
            TreeNode next = node.NextNode;

            if (parent == null || next == null) return;

            tv.BeginUpdate();
            parent.TreeView.Sorted = false;
            node.Remove();
            parent.Nodes.Insert(next.Index + 1, node);

            tv.EndUpdate();
            tv.SelectedNode = node;
        }

        /// <summary>
        /// Переименовывает все дочерние узлы (присваивает номер от 01 до 99 для сортировки).
        /// </summary>
        /// <param name="main">Ссылка на главную форму</param>
        /// <param name="tv">Головной компонент TreeView в котором производятся действия.</param>
        /// <param name="node">Родительский узел</param>
        public static void Renumbering(Form1 main, TreeView tv, TreeNode node)
        {
            node = tv.SelectedNode;
            if (node.Nodes.Count == 0) return;

            tv.BeginUpdate();
            node.TreeView.Sorted = false;

            for (int i = 0; i < node.Nodes.Count; i++)
            {
                string text = Regex.Replace(node.Nodes[i].Text, @"^\d+\. +", "");
                text = $"{1 + i:D2}. " + text;
                node.Nodes[i].Text = text;
                DATABASE.UpdatePropertyName((long)node.Nodes[i].Tag, text);
            }

            tv.EndUpdate();
            tv.SelectedNode = node;
        }

        /// <summary>
        /// Удаляет нумерацию всех дочерних узлов
        /// </summary>
        /// <param name="main">Ссылка на главную форму</param>
        /// <param name="tv">Головной компонент TreeView в котором производятся действия.</param>
        /// <param name="node">Родительский узел</param>
        public static void Denumbering(Form1 main, TreeView tv, TreeNode node)
        {
            node = tv.SelectedNode;
            if (node.Nodes.Count == 0) return;

            tv.BeginUpdate();
            node.TreeView.Sorted = false;

            for (int i = 0; i < node.Nodes.Count; i++)
            {
                string text = Regex.Replace(node.Nodes[i].Text, @"^\d+\. +", "");
                node.Nodes[i].Text = text;
                DATABASE.UpdatePropertyName((long)node.Nodes[i].Tag, text);
            }

            tv.EndUpdate();
            tv.SelectedNode = node;
        }

        /// <summary>
        /// Добавляет указанный элемент оглавления в закладки
        /// </summary>
        /// <param name="node"></param>
        public static void AddPropertyToBookmark(TreeNode node)
        {
            string dbname = Path.GetFileNameWithoutExtension(DATABASE.FileName);
            TreeNode class_node = VARS.main_form.tvClasses.SelectedNode;
            TreeNode prop_node = VARS.main_form.tvProps.SelectedNode;
            if (class_node == null || prop_node == null) return;

            // проверим, если такая закладка уже есть
            bool alreadyExists = false;
            foreach(DataRow dr in DATABASE.bookmarks.Rows)
            {
                if ((string)dr["database"] == dbname &&
                    (long)dr["class_id"] == (long)class_node.Tag &&
                    (long)dr["property_id"] == (long)prop_node.Tag)
                {
                    alreadyExists = true;
                    break;
                } 
            }

            if (!alreadyExists)
            {
                DATABASE.bookmarks.Rows.Add(new object[] { dbname, class_node.Text, class_node.Tag, BuildFullNodePath(prop_node), prop_node.Tag });
            }

            // установим признак того, что элемент добавлен в закладки и отобразим значки
            propertyInfo.isBookmarked = true;

            propertyInfo.BuildToolTip();
            VARS.main_form.toolTip1.SetToolTip(VARS.main_form.pbMarks, propertyInfo.Tip);

            VARS.main_form.pbMarks.Refresh();
        }

        /// <summary>
        /// Добавляет указанный элемент оглавления в избранное
        /// </summary>
        /// <param name="node"></param>
        public static void AddPropertyToFavourites(TreeNode node)
        {
            string dbname = Path.GetFileNameWithoutExtension(DATABASE.FileName);
            TreeNode class_node = VARS.main_form.tvClasses.SelectedNode;
            TreeNode prop_node = VARS.main_form.tvProps.SelectedNode;
            if (class_node == null || prop_node == null) return;

            // проверим, если такое избранное уже есть
            bool alreadyExists = false;
            foreach (DataRow dr in DATABASE.favourites.Rows)
            {
                if ((long)dr["class_id"] == (long)class_node.Tag &&
                    (long)dr["property_id"] == (long)prop_node.Tag)
                {
                    alreadyExists = true; 
                    break;
                }
            }

            if (!alreadyExists)
            {
                DATABASE.favourites.Rows.Add(new object[] { class_node.Text, class_node.Tag, prop_node.Tag, BuildFullNodePath(prop_node) });
                DATABASE.AddPropertyToFavourites((long)class_node.Tag, (long)prop_node.Tag, class_node.Text);
            }

            // установим признак того, что элемент добавлен в избранное и отобразим значки
            propertyInfo.isFavourite = true;

            propertyInfo.BuildToolTip();
            VARS.main_form.toolTip1.SetToolTip(VARS.main_form.pbMarks, propertyInfo.Tip);

            VARS.main_form.pbMarks.Refresh();
        }

        /// <summary>
        /// Возвращает полный путь к родительской папке вида: узел / узел / ...
        /// </summary>
        /// <param name="node">Ткущий узел</param>
        /// <returns>Строка полного пути</returns>
        public static string BuildFullNodePath(TreeNode node)
        {
            string ret = "";
            if (node.Parent != null) ret = (node.Parent.Level == 0 ? node.Text : " / " + node.Text);

            if (node.Parent != null)
                ret = BuildFullNodePath(node.Parent) + ret;

            return ret;
        }
    }
}
