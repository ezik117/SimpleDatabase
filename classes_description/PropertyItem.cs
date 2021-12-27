using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace simple_database
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

            main.tvProps.Sort();

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
            frm.PropertyType = 7;

            if (frm.ShowDialog() != DialogResult.OK) return;

            long id;
            string propName = frm.tbPropertyName.Text.Trim();

            if (frm.rbAttachment.Checked)
            {
                // выбрано вложение.
                OpenFileDialog of = new OpenFileDialog();
                if (of.ShowDialog() == DialogResult.OK)
                {
                    if (propName == "") propName = System.IO.Path.GetFileName(of.FileName);

                    id = main.db.AttachmentInsert(-1, (long)main.tvClasses.SelectedNode.Tag,
                                                   propName, frm.PropertyType,
                                                   (long)currentProperty.Tag, "", of.FileName);
                }
                else
                    return;
            }
            else
            {
                // выбран значок
                id = main.db.SaveProperty(-1, (long)main.tvClasses.SelectedNode.Tag,
                                            propName, frm.PropertyType,
                                            (long)currentProperty.Tag, "");
            }

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
            frm.tbPropertyName.Text = currentProperty.Text;
            frm.PropertyType = currentProperty.ImageIndex;

            if (frm.ShowDialog() != DialogResult.OK) return;

            long id = main.db.SaveProperty((long)currentProperty.Tag, (long)main.tvClasses.SelectedNode.Tag,
                                           frm.tbPropertyName.Text.Trim(), frm.PropertyType,
                                           (long)currentProperty.Parent.Tag, main.paramTextEditor.txtBox.Rtf);
            main.paramTextEditor.userAction1();

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

            main.db.UpdatePropertyDescription((long)main.tvProps.SelectedNode.Tag, main.paramTextEditor.txtBox.Rtf);
            main.paramTextEditor.userAction1();
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
            if (main.btnDescSave.ImageKey == "save") return;

            if (MessageBox.Show("Все несохраненные данные будут потеряны. Продолжить?",
                                "",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Warning,
                                MessageBoxDefaultButton.Button2) == DialogResult.No) return;

            PropertyChanged(main);
        }

        /// <summary>
        /// Выбрано свойство класса (автоматически или щелчком пользователя).
        /// Считывает все взаимосвязанные со свойством объекты.
        /// </summary>
        /// <param name="main">Ссылка на главную форму.</param>
        public static void PropertyChanged(Form1 main)
        {
            TreeNode currentProperty = main.tvProps.SelectedNode;
            if (stopEventProcessing) return;

            if ((long)currentProperty.Tag == -1)
            {
                // выбран корневой узел
                main.paramTextEditor.txtBox.ReadOnly = true;
                main.paramTextEditor.userAction2();
                return;
            }
            else
            {
                main.paramTextEditor.txtBox.ReadOnly = false;
            }

            // description
            SqlRows r = main.db.LoadProperty((long)currentProperty.Tag);
            main.paramTextEditor.txtBox.Rtf = (DBNull.Value.Equals(r[0]["description"]) ? "" : (string)r[0]["description"]);
            main.paramTextEditor.userAction1();
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
        /// Распаковывает вложение в папку TEMP и открывает его.
        /// </summary>
        /// <param name="main">Ссылка на главную форму.</param>
        public static void ExtractAttachment(Form1 main)
        {
            TreeNode currentProperty = main.tvProps.SelectedNode;
            if (currentProperty == null) return;

            string fileName = main.db.AttachmentGetFilename((long)currentProperty.Tag);

            SaveFileDialog sd = new SaveFileDialog();
            sd.InitialDirectory = Path.Combine(Application.StartupPath, "temp");
            sd.FileName = fileName;
            sd.Filter = $"{Path.GetExtension(fileName).TrimStart('.')}|*{Path.GetExtension(fileName)}|*.*|*.*";
            sd.AddExtension = true;
            if (sd.ShowDialog() != DialogResult.OK) return;

            bool result = main.db.AttachmentExtract((long)currentProperty.Tag, sd.FileName);
            
            //if (fileName != "") System.Diagnostics.Process.Start(fileName);
        }

        /// <summary>
        /// Извлекает и запускает указанное вложение
        /// </summary>
        /// <param name="main">Ссылка на главную форму.</param>
        public static void ExtractAndRunAttachment(Form1 main)
        {
            TreeNode currentProperty = main.tvProps.SelectedNode;
            if (currentProperty == null) return;

            string fileName = main.db.AttachmentGetFilename((long)currentProperty.Tag);
            fileName = Path.Combine(Application.StartupPath, "temp", fileName);
            bool result = main.db.AttachmentExtract((long)currentProperty.Tag, fileName);

            if (result) System.Diagnostics.Process.Start(fileName);
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
    }
}
