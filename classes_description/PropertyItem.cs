using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace classes_description
{
    using SqlRows = List<Dictionary<string, object>>;

    public static class PropertyItem
    {
        /// <summary>
        /// Если установлено в True, то события от выбора пользователем класса будут блокироваться.
        /// Нужно для отключения автоматического считывания свойств типа Description при создании TreeView.
        /// </summary>
        private static bool stopEventProcessing = false;

        /// <summary>
        /// Загружает параметры выбранного класса в TreeView.
        /// </summary>
        /// <param name="class_id">ROWID записи класса.</param>
        /// <param name="main">Ссылка на главную форму.</param>
        public static void Load(long class_id, Form1 main)
        {
            stopEventProcessing = true;

            SqlRows rows = main.db.LoadProperties(class_id);
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

            main.tvProps.ExpandAll();

            stopEventProcessing = false;

            main.tvProps.SelectedNode = main.tvProps.Nodes[0];
            main.tvProps.Focus();
        }

        /// <summary>
        /// Создает параметр.
        /// </summary>
        /// <param name="main">Ссылка на главную форму.</param>
        public static void Create(Form1 main)
        {
            TreeNode currentProperty = main.tvProps.SelectedNode;
            if (currentProperty == null) return;

            frmPropertyEdit frm = new frmPropertyEdit();
            frm.tbPropertyName.Text = "";
            frm.rbTriangle.Checked = true;

            if (frm.ShowDialog() != DialogResult.OK) return;

            main.propDescr.ClearText();

            long id = main.db.SaveProperty(-1, (long)main.tvClasses.SelectedNode.Tag,
                                           frm.tbPropertyName.Text.Trim(), frm.PropertyType,
                                           (long)currentProperty.Tag, "");

            TreeNode t = new TreeNode();
            t.Text = frm.tbPropertyName.Text.Trim();
            t.ImageIndex = t.SelectedImageIndex = frm.PropertyType;
            t.Tag = id;

            currentProperty.Nodes.Add(t);
            main.tvProps.SelectedNode = t;
            main.tvProps.Focus();
        }

        /// <summary>
        /// Обновить свойство.
        /// </summary>
        /// <param name="main">Ссылка на главную форму.</param>
        public static void Update(Form1 main)
        {
            TreeNode currentProperty = main.tvProps.SelectedNode;
            if (currentProperty == null) return;
            if ((long)currentProperty.Tag == -1) return;

            frmPropertyEdit frm = new frmPropertyEdit();
            frm.tbPropertyName.Text = currentProperty.Text;
            switch ((long)currentProperty.ImageIndex)
            {
                case (long)IconTypes.Cirle:
                    frm.rbCircle.Checked = true;
                    break;
                case (long)IconTypes.Square:
                    frm.rbSquare.Checked = true;
                    break;
                case (long)IconTypes.Triangle:
                    frm.rbTriangle.Checked = true;
                    break;
                case (long)IconTypes.Folder:
                    frm.rbFolder.Checked = true;
                    break;
            }
            if (frm.ShowDialog() != DialogResult.OK) return;

            long id = main.db.SaveProperty((long)currentProperty.Tag, (long)main.tvClasses.SelectedNode.Tag,
                                           frm.tbPropertyName.Text.Trim(), frm.PropertyType,
                                           (long)currentProperty.Parent.Tag, main.db.Escape(main.tbDescEdit.Rtf));
            main.propDescr.TextSaved();

            currentProperty.Text = frm.tbPropertyName.Text;
            currentProperty.ImageIndex = currentProperty.SelectedImageIndex = frm.PropertyType;
        }

        /// <summary>
        /// Обновляет только описание параметра.
        /// </summary>
        /// <param name="main">Ссылка на главную форму.</param>
        public static void UpdateDescription(Form1 main)
        {
            if (main.tvProps.SelectedNode == null) return;

            main.db.UpdatePropertyDescription((long)main.tvProps.SelectedNode.Tag, main.db.Escape(main.tbDescEdit.Rtf));
            main.propDescr.TextSaved();
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
                if (MessageBox.Show("Удалить все параметры класса?",
                                "",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Warning,
                                MessageBoxDefaultButton.Button2) == DialogResult.No) return;

                main.db.DeleteProperties((long)main.tvClasses.SelectedNode.Tag);

                ClassItem.NodeChanged(main);
            }
            else
            {
                if (MessageBox.Show("Удалить параметр и все зависимые иерархически параметры и элементы?",
                                    "",
                                    MessageBoxButtons.YesNo,
                                    MessageBoxIcon.Warning,
                                    MessageBoxDefaultButton.Button2) == DialogResult.No) return;

                main.db.DeleteProperty((long)main.tvProps.SelectedNode.Tag);
                main.tvProps.Nodes.Remove(main.tvProps.SelectedNode);
            }
        }

        /// <summary>
        /// Считывает повторно описание.
        /// </summary>
        /// <param name="main">Ссылка на главную форму.</param>
        public static void DescriptionReload(Form1 main)
        {
            TreeNode currentProperty = main.tvProps.SelectedNode;
            if (currentProperty == null) return;
            if (main.btnDescSave.ImageIndex == (int)IconTypes.SaveIcon) return;

            if (MessageBox.Show("Все несохраненные данные будут потеряны. Продолжить?",
                                "",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Warning,
                                MessageBoxDefaultButton.Button2) == DialogResult.No) return;

            PropertyChanged(main);
        }

        /// <summary>
        /// считывает все взаимосвязанные со свойством объекты.
        /// </summary>
        /// <param name="main">Ссылка на главную форму.</param>
        public static void PropertyChanged(Form1 main)
        {
            TreeNode currentProperty = main.tvProps.SelectedNode;
            if (stopEventProcessing) return;
            if (currentProperty == null) return;
            if ((long)currentProperty.Tag == -1)
            {
                main.tbDescEdit.Rtf = "";
                main.tbDescEdit.ReadOnly = true;
                main.propDescr.TextSaved();
                return;
            }
            else
            {
                main.tbDescEdit.ReadOnly = false;
            }

            // description
            SqlRows r = main.db.LoadProperty((long)currentProperty.Tag);
            main.tbDescEdit.Rtf = (DBNull.Value.Equals(r[0]["description"]) ? "" : (string)r[0]["description"]);
            main.propDescr.TextSaved();
        }

        /// <summary>
        /// Меняет родителя параметра.
        /// </summary>
        /// <param name="id">ROWID параметра</param>
        /// <param name="new_parent_id">Новый ROWID родительского элемента</param>
        /// <param name="main">Ссылка на главную форму.</param>
        public static void ChangeParent(long id, long new_parent_id, Form1 main)
        {
            main.db.ChangePropertyParent(id, new_parent_id);
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
            if (main.btnDescSave.ImageIndex == (int)IconTypes.NotSavedIcon)
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
                    main.propDescr.TextSaved();
                    return false;
                }

                main.btnDescSave.PerformClick();
            }

            return false;
        }
    }
}
