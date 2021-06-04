using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace classes_description
{
    using SqlRows = List<Dictionary<string, object>>;

    public static class ClassItem
    {
        /// <summary>
        /// Если установлено в True, то события от выбора пользователем класса будут блокироваться.
        /// Нужно для отключения автоматического считывания свойств типа Description при создании TreeView.
        /// </summary>
        private static bool stopEventProcessing = false;

        /// <summary>
        /// Создает новую запись класса. Обновляет TreeView и запись в БД.
        /// </summary>
        /// <param name="main">Ссылка на главную форму.</param>
        public static void Create(Form1 main)
        {
            frmClassEdit frm = new frmClassEdit();
            frm.tbClassName.Text = "";
            if (frm.ShowDialog() != DialogResult.OK) return;

            stopEventProcessing = true;
            main.propDescr.ClearText();

            long id = main.db.SaveClass(-1, frm.tbClassName.Text.Trim(), "");
            main.classDescr.TextSaved();

            TreeNode t = new TreeNode();
            t.Text = frm.tbClassName.Text.Trim();
            t.Tag = id;

            main.tvClasses.Nodes.Add(t);
            main.tvClasses.Sort();
            main.tvClasses.SelectedNode = t;
            main.tvClasses.Focus();

            stopEventProcessing = false;
            NodeChanged(main);
        }

        /// <summary>
        /// Загружает записи из БД о классах. Заполняет TreeView.
        /// </summary>
        /// <param name="main">Ссылка на главную форму.</param>
        public static void Load(Form1 main)
        {
            stopEventProcessing = true;

            main.tvClasses.Nodes.Clear();
            SqlRows rows = main.db.LoadClasses();

            foreach (Dictionary<string, object> r in rows)
            {
                TreeNode t = new TreeNode();
                t.Text = (string)r["name"];
                t.Tag = (long)r["id"];
                main.tvClasses.Nodes.Add(t);
            }

            main.tvClasses.Sort();

            stopEventProcessing = false;

            if (main.tvClasses.Nodes.Count > 0)
                main.tvClasses.SelectedNode = main.tvClasses.Nodes[0];
            else
                NodeChanged(main); // clear all

            main.Text = $"Справочник классов проекта [{System.IO.Path.GetFileName(main.db.FileName).ToUpper()}]";
        }

        /// <summary>
        /// Обновить значения записи класса (название, описание).
        /// </summary>
        /// <param name="main">Ссылка на главную форму.</param>
        public static void Update(Form1 main)
        {
            if (main.tvClasses.SelectedNode == null) return;

            main.db.SaveClass((long)main.tvClasses.SelectedNode.Tag, main.tvClasses.SelectedNode.Text, main.tbClassDescEdit.Text);
            main.classDescr.TextSaved();
        }

        /// <summary>
        /// Редактировать название класса в записи.
        /// </summary>
        /// <param name="main">Ссылка на главную форму.</param>
        public static void Edit(Form1 main)
        {
            if (main.tvClasses.SelectedNode == null) return;

            frmClassEdit frm = new frmClassEdit();
            frm.tbClassName.Text = main.tvClasses.SelectedNode.Text;
            if (frm.ShowDialog() != DialogResult.OK) return;

            long id = main.db.SaveClass((long)main.tvClasses.SelectedNode.Tag, frm.tbClassName.Text.Trim(), main.tbClassDescEdit.Text);
            main.classDescr.TextSaved();

            main.tvClasses.SelectedNode.Text = main.tvProps.Nodes[0].Text = frm.tbClassName.Text;
        }

        /// <summary>
        /// Удаляет класс и его зависимости.
        /// </summary>
        /// <param name="main">Ссылка на главную форму.</param>
        public static void Delete(Form1 main)
        {
            if (main.tvClasses.SelectedNode == null) return;

            if (MessageBox.Show("Удалить класс и все зависимые элементы?",
                                "",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Warning,
                                MessageBoxDefaultButton.Button2) == DialogResult.No) return;

            main.db.DeleteClass((long)main.tvClasses.SelectedNode.Tag);
            main.tvClasses.Nodes.Remove(main.tvClasses.SelectedNode);
        }

        /// <summary>
        /// считывает все взаимосвязанные с классом объекты.
        /// </summary>
        /// <param name="main">Ссылка на главную форму.</param>
        public static void NodeChanged(Form1 main)
        {
            TreeNode currentClass = main.tvClasses.SelectedNode;
            if (stopEventProcessing) return;
            if (currentClass == null)
            {
                main.classDescr.ClearText();
                main.tvProps.Nodes.Clear();
                main.propDescr.ClearText();
                main.tbClassDescEdit.ReadOnly = true;
                return;
            }
            else
            {
                main.tbClassDescEdit.ReadOnly = false;
            }

            // description
            SqlRows r = main.db.LoadClass((long)currentClass.Tag);
            main.tbClassDescEdit.Text = (DBNull.Value.Equals(r[0]["description"]) ? "" : (string)r[0]["description"]);

            main.tvProps.Nodes.Clear();
            TreeNode t = new TreeNode(currentClass.Text, 7, 7);
            t.Tag = (long)-1;
            main.tvProps.Nodes.Add(t);

            PropertyItem.Load((long)currentClass.Tag, main);

            main.classDescr.TextSaved();
        }

        /// <summary>
        /// Считывает повторно описание.
        /// </summary>
        /// <param name="main">Ссылка на главную форму.</param>
        public static void DescriptionReload(Form1 main)
        {
            TreeNode currentClass = main.tvClasses.SelectedNode;
            if (currentClass == null) return;
            if (main.btnClassDescSave.ImageIndex == (int)IconTypes.SaveIcon) return;

            if (MessageBox.Show("Все несохраненные данные будут потеряны. Продолжить?",
                                "",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Warning,
                                MessageBoxDefaultButton.Button2) == DialogResult.No) return;

            NodeChanged(main);
        }

        /// <summary>
        /// Проверяет на несохраненные данные описания класса. Запрашивает пользователя.И в зависимости от ответа
        /// либо сохраняет, либо не сохраняет данные. Возвращает булево значение для использования в аргументе Cancel,
        /// позволяющее отменить операцию.
        /// </summary>
        /// <param name="main">Ссылка на главную форму.</param>
        /// <returns>True - пользователь хочет отменить действие. False-можно продолжить.</returns>
        public static bool CheckForUnsavedDesc(Form1 main)
        {
            if (main.btnClassDescSave.ImageIndex == (int)IconTypes.NotSavedIcon)
            {
                DialogResult res = MessageBox.Show("Имеются несохраненные данные (описание класса). Сохранить?",
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
                    main.classDescr.TextSaved();
                    return false;
                }

                main.btnClassDescSave.PerformClick();
            }

            return false;
        }
    }
}
