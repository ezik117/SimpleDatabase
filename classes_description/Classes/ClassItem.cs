using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace simple_database
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

            // проверим не существует ли уже такое имя
            string newClassName = frm.tbClassName.Text.Trim();
            foreach (TreeNode n in VARS.main_form.tvClasses.Nodes)
                if (n.Text == newClassName)
                {
                    MessageBox.Show("Указанное имя уже существует", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

            stopEventProcessing = true;
            main.paramTextEditor.userAction2();

            long id = DATABASE.SaveClass(-1, newClassName, "");

            TreeNode t = new TreeNode();
            t.Text = frm.tbClassName.Text.Trim();
            t.Tag = id;

            main.tvClasses.Nodes.Add(t);
            main.tvClasses.Sort();
            main.tvClasses.SelectedNode = t;
            main.tvClasses.Focus();

            stopEventProcessing = false;
            NodeChanged(main);

            main.slblLastUpdate.Text = "Last update: " + DATABASE.SetLastUpdate();
        }

        /// <summary>
        /// Загружает записи из БД о классах. Заполняет TreeView.
        /// </summary>
        /// <param name="main">Ссылка на главную форму.</param>
        public static void Load(Form1 main)
        {
            stopEventProcessing = true;

            main.tvClasses.Nodes.Clear();
            SqlRows rows = DATABASE.LoadClasses();

            foreach (Dictionary<string, object> r in rows)
            {
                TreeNode t = new TreeNode();
                t.ImageIndex = t.SelectedImageIndex = (int)IconTypes.Book;
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

            main.Text = $"Справочник. [{System.IO.Path.GetFileName(DATABASE.FileName).ToUpper()}]";
        }

        /// <summary>
        /// Обновить значения записи класса (название, описание).
        /// </summary>
        /// <param name="main">Ссылка на главную форму.</param>
        public static void Update(Form1 main)
        {
            if (main.tvClasses.SelectedNode == null) return;

            DATABASE.SaveClass((long)main.tvClasses.SelectedNode.Tag, main.tvClasses.SelectedNode.Text, "");
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

            long id = DATABASE.SaveClass((long)main.tvClasses.SelectedNode.Tag, frm.tbClassName.Text.Trim(), "");

            main.tvClasses.SelectedNode.Text = main.tvProps.Nodes[0].Text = frm.tbClassName.Text;

            main.slblLastUpdate.Text = "Last update: " + DATABASE.SetLastUpdate();
        }

        /// <summary>
        /// Удаляет класс и его зависимости.
        /// </summary>
        /// <param name="main">Ссылка на главную форму.</param>
        public static void Delete(Form1 main)
        {
            if (main.tvClasses.SelectedNode == null) return;

            if (MessageBox.Show("Удалить каталог и все зависимые элементы?",
                                "",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Warning,
                                MessageBoxDefaultButton.Button2) == DialogResult.No) return;

            DATABASE.DeleteClass((long)main.tvClasses.SelectedNode.Tag);
            main.tvClasses.Nodes.Remove(main.tvClasses.SelectedNode);

            main.slblLastUpdate.Text = "Last update: " + DATABASE.SetLastUpdate();
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
                main.tvProps.Nodes.Clear();
                main.paramTextEditor.userAction2();
                return;
            }

            SqlRows r = DATABASE.LoadClass((long)currentClass.Tag);

            main.tvProps.Nodes.Clear();
            TreeNode t = new TreeNode(currentClass.Text, (int)IconTypes.Book, (int)IconTypes.Book);
            t.Tag = (long)-1;
            main.tvProps.Nodes.Add(t);

            PROPERTY.Load((long)currentClass.Tag, main);

            main.paramTextEditor.txtBox.Rtf = (string)r[0]["description"];
            main.paramTextEditor.userAction1();
        }
    }
}
