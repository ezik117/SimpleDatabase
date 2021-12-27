using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/*
 ТЕРМИНЫ:
  Класс (Class) = Каталог
  Свойство (Property) = Оглавление
  Описание свойства (Property Description) =  Информация
  Описание класса (Class Description) = Описание каталога
     */

namespace simple_database
{
    using SqlRows = List<Dictionary<string, object>>;

    public partial class Form1 : Form
    {
        public Database db;

        public TextEditor classTextEditor;
        public TextEditor paramTextEditor;

        // используется для операций DragDrop, содержит выбранный для Drop узел
        private TreeNode DragDropSelectedNode = null;
        // используется для операций DragDrop, содержит индекс картинки узла (будет заменен на индекс выделения)
        private int DragDropSelectedNodeImageIndex = -1;

        [Serializable]
        public struct ShellExecuteInfo
        {
            public int Size;
            public uint Mask;
            public IntPtr hwnd;
            public string Verb;
            public string File;
            public string Parameters;
            public string Directory;
            public uint Show;
            public IntPtr InstApp;
            public IntPtr IDList;
            public string Class;
            public IntPtr hkeyClass;
            public uint HotKey;
            public IntPtr Icon;
            public IntPtr Monitor;
        }

        // для вызова функции "Open with.."
        [DllImport("shell32.dll", SetLastError = true)]
        extern public static bool ShellExecuteEx(ref ShellExecuteInfo lpExecInfo);

        public Form1()
        {
            InitializeComponent();

            // Редактор описания класса
            classTextEditor = new TextEditor(pnlClassEditHolder);
            classTextEditor.ToolBoxPos = TextEditor.ToolBoxPosition.OnBottom;
            classTextEditor.OnContentChanged += ClassTextEditor_OnContentChanged;
            classTextEditor.txtBox.KeyDown += classTextEditor_KeyDown;
            classTextEditor.userAction1 = delegate
            {
                // Сохранить текст
                btnClassDescSave.ImageKey = "save";
                classTextEditor.textWasChanged = false;
            };
            classTextEditor.userAction2 = delegate
            {
                // Очистить текст
                classTextEditor.txtBox.Clear();
                btnClassDescSave.ImageKey = "save";
                classTextEditor.textWasChanged = false;
            };

            // Редактор описания параметра
            paramTextEditor = new TextEditor(pnlParamEditHolder);
            paramTextEditor.ToolBoxPos = TextEditor.ToolBoxPosition.OnBottom;
            paramTextEditor.OnContentChanged += ParamTextEditor_OnContentChanged;
            paramTextEditor.txtBox.KeyDown += paramTextEditor_KeyDown;
            paramTextEditor.userAction1 = delegate
            {
                // Сохранить текст
                btnDescSave.ImageKey = "save";
                paramTextEditor.textWasChanged = false;
            };
            paramTextEditor.userAction2 = delegate
            {
                // Очистить текст
                paramTextEditor.txtBox.Clear();
                btnDescSave.ImageKey = "save";
                paramTextEditor.textWasChanged = false;
            };

            // очистим папку TEMP, если там что то есть
            CleanUpTemp();

            // создаем или открываем БД по умолчанию
            db = new Database();
            db.OpenOrCreate("default");

            // загружаем данные в форму
            ClassItem.Load(this);


        }

        // Обработка нажатий некоторых клавиш
        private void paramTextEditor_KeyDown(object sender, KeyEventArgs e)
        {
            // CTRL-S. Сохраним данные.
            if (e.Control && e.KeyCode == Keys.S)
            {
                PropertyItem.UpdateDescription(this);
                e.Handled = false;
                return;
            }
        }

        // Обработка нажатий некоторых клавиш
        private void classTextEditor_KeyDown(object sender, KeyEventArgs e)
        {
            // CTRL-S. Сохраним данные.
            if (e.Control && e.KeyCode == Keys.S)
            {
                ClassItem.Update(this);
                e.Handled = false;
                return;
            }
        }

        // Изменим иконку сохранения текста на предупреждение
        private void ParamTextEditor_OnContentChanged(RichTextBox sender)
        {
            if (btnDescSave.ImageKey != "notsaved") btnDescSave.ImageKey = "notsaved";
        }

        // Изменим иконку сохранения текста на предупреждение
        private void ClassTextEditor_OnContentChanged(RichTextBox sender)
        {
            if (btnClassDescSave.ImageKey != "notsaved") btnClassDescSave.ImageKey = "notsaved";
        }

        // Очистка папки temp
        private void CleanUpTemp()
        {
            try
            {
                string[] files = System.IO.Directory.GetFiles($@"{Application.StartupPath}\temp");
                foreach (string file in files)
                {
                    try
                    {
                        System.IO.File.Delete(file);
                    }
                    catch { };
                }
            }
            catch { };
        }

        // Добавить класс
        private void btnClassAdd_Click(object sender, EventArgs e)
        {
            ClassItem.Create(this);
        }

        // Сохранить описание свойства
        private void btnDescSave_Click(object sender, EventArgs e)
        {
            PropertyItem.UpdateDescription(this);
        }

        // Сохранить описание класса
        private void btnClassDescSave_Click(object sender, EventArgs e)
        {
            ClassItem.Update(this);
        }

        // Выбран другой класса. Считать связанные данные.
        private void tvClasses_AfterSelect(object sender, TreeViewEventArgs e)
        {
            ClassItem.NodeChanged(this);
        }

        // Редактировать название класса.
        private void btnClassEdit_Click(object sender, EventArgs e)
        {
            ClassItem.Edit(this);
        }

        // Удалить класс
        private void btnClassDel_Click(object sender, EventArgs e)
        {
            ClassItem.Delete(this);
        }

        // Добавить свойство класса
        private void btnPropAdd_Click(object sender, EventArgs e)
        {
            PropertyItem.Create(this);
        }

        // Удалить свойство и подсвойства
        private void btnPropDel_Click(object sender, EventArgs e)
        {

            PropertyItem.Delete(this);
        }

        // Редактировать свойство
        private void btnPropEdit_Click(object sender, EventArgs e)
        {
            PropertyItem.Update(this);
        }

        // Свернуть / развернуть список
        private void btnPropCollapseExpand_Click(object sender, EventArgs e)
        {
            if (tvProps.SelectedNode == null) return;

            if (tvProps.SelectedNode.Nodes.Count != 0)
            {
                if (tvProps.SelectedNode.IsExpanded)
                {
                    btnPropCollapseExpand.ImageKey = "expand";
                    tvProps.SelectedNode.Collapse();
                }
                else
                {
                    btnPropCollapseExpand.ImageKey = "collapse";
                    tvProps.SelectedNode.ExpandAll();
                }
            }
        }

        // Выбрано другое свойство. Считать связанные данные.
        private void tvProps_AfterSelect(object sender, TreeViewEventArgs e)
        {
            PropertyItem.PropertyChanged(this);
            if (tvProps.SelectedNode.IsExpanded)
                btnPropCollapseExpand.ImageKey = "collapse";
            else
                btnPropCollapseExpand.ImageKey = "expand";
        }

        // Считать повторно описание класса.
        private void btnClassDescReload_Click(object sender, EventArgs e)
        {
            ClassItem.DescriptionReload(this);
        }

        // Считать повторно описание свойства.
        private void btnDescReload_Click(object sender, EventArgs e)
        {
            PropertyItem.DescriptionReload(this);
        }

        // Проверка несохраненных данных при закрытии формы.
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = ClassItem.CheckForUnsavedDesc(this);
            if (e.Cancel) return;
            e.Cancel = PropertyItem.CheckForUnsavedDesc(this);
        }

        // Меняется выбор свойства. Проверить на сохранение свойства.
        private void tvProps_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            e.Cancel = PropertyItem.CheckForUnsavedDesc(this);
        }

        // Меняется выбор класса. Проверить на сохранение описаний класса и свойства.
        private void tvClasses_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            e.Cancel = ClassItem.CheckForUnsavedDesc(this);
            if (e.Cancel) return;
            e.Cancel = PropertyItem.CheckForUnsavedDesc(this);
        }

        // Добавим пункты в системное меню
        private void Form1_Load(object sender, EventArgs e)
        {
            SystemMenu.AddItem(this);
        }

        // Move the dragged node when the left mouse button is used.
        private void tvProps_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                DoDragDrop(e.Item, DragDropEffects.Move);
            }
        }

        // Set the target drop effect to the effect 
        // specified in the ItemDrag event handler.
        private void tvProps_DragEnter(object sender, DragEventArgs e)
        {
            // Check if datatype is acceptable
            if (e.Data.GetDataPresent(typeof(TreeNode))) e.Effect = e.AllowedEffect; 
        }

        // Select the node under the mouse pointer to indicate the 
        // expected drop location.
        private void tvProps_DragOver(object sender, DragEventArgs e)
        {
            // Do not allow visual effects if data type is not acceptable
            if (!e.Data.GetDataPresent(typeof(TreeNode))) return;

            // Retrieve the client coordinates of the mouse position.
            Point targetPoint = ((TreeView)sender).PointToClient(new Point(e.X, e.Y));

            // Select the node at the mouse position.
            TreeNode targetNode = ((TreeView)sender).GetNodeAt(targetPoint);

            if (DragDropSelectedNode != targetNode)
            {
                if (DragDropSelectedNode != null && DragDropSelectedNodeImageIndex != -1 && DragDropSelectedNode != null)
                    DragDropSelectedNode.ImageIndex = DragDropSelectedNodeImageIndex;

                DragDropSelectedNode = targetNode;
                DragDropSelectedNodeImageIndex = DragDropSelectedNode.ImageIndex;
                DragDropSelectedNode.ImageKey = "selected";
            }
        }

        // Item is dropped. Finish.
        private void tvProps_DragDrop(object sender, DragEventArgs e)
        {
            if (DragDropSelectedNode != null && DragDropSelectedNodeImageIndex != -1)
            {
                DragDropSelectedNode.ImageIndex = DragDropSelectedNodeImageIndex;
                DragDropSelectedNodeImageIndex = -1;
                DragDropSelectedNode = null;
            }

            // Retrieve the client coordinates of the drop location.
            Point targetPoint = ((TreeView)sender).PointToClient(new Point(e.X, e.Y));

            // Retrieve the node at the drop location.
            TreeNode targetNode = ((TreeView)sender).GetNodeAt(targetPoint);

            // Retrieve the node that was dragged.
            TreeNode draggedNode = (TreeNode)e.Data.GetData(typeof(TreeNode));

            // Confirm that the node at the drop location is not 
            // the dragged node or a descendant of the dragged node.
            if (!draggedNode.Equals(targetNode) && !ContainsNode(draggedNode, targetNode))
            {
                PropertyItem.ChangeParent((long)draggedNode.Tag, (long)targetNode.Tag, this);
                draggedNode.Remove();
                targetNode.Nodes.Add(draggedNode);
            }
        }

        // Determine whether one node is a parent 
        // or ancestor of a second node.
        private bool ContainsNode(TreeNode node1, TreeNode node2)
        {
            // Check the parent node of the second node.
            if (node2.Parent == null) return false;
            if (node2.Parent.Equals(node1)) return true;

            // If the parent node is not null or equal to the first node, 
            // call the ContainsNode method recursively using the parent of 
            // the second node.
            return ContainsNode(node1, node2.Parent);
        }

        // Поиск свойства
        private void btnPropSearch_Click(object sender, EventArgs e)
        {
            frmSearcProperty frm = new frmSearcProperty();
            frm.InitSearch(tvProps, false);
            frm.ShowDialog();
        }

        // Нажата клавиша в окне свойств класса. Определить действия.
        private void tvProps_KeyDown(object sender, KeyEventArgs e)
        {
            // CTRL-F. Поиск параметра
            if (e.Control && e.KeyCode == Keys.F)
            {
                btnPropSearch.PerformClick();
                e.Handled = true;
                return;
            }
        }

        // уменьшаем панель описания категорий до 30% от высоты окна
        private void Form1_Resize(object sender, EventArgs e)
        {
            panel1.Height = (int)(this.Height * 0.7);
        }

        // Окно свойств. Вызов контекстного меню
        private void tvProps_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                TreeNode targetNode = ((TreeView)sender).GetNodeAt(e.Location);

                if (targetNode != null && targetNode.ImageIndex == (int)IconTypes.Attachment)
                {
                    tvProps.SelectedNode = targetNode;
                    ctxMenuProps.Show(tvProps, e.Location);
                }
            }
        }

        // Окно свойств. Контекстное меню: открыть вложение
        private void tsmSaveAndOpenAttachment_Click(object sender, EventArgs e)
        {
            PropertyItem.ExtractAndRunAttachment(this);
        }

        // Окно свойств. Контекстное меню: сохранить вложение
        private void tsmSaveAttachmentToFile_Click(object sender, EventArgs e)
        {
            PropertyItem.ExtractAttachment(this);
        }

        // Окно свойств. Контекстное меню: открыть с помощью
        private void tsmOpenAttachmentWith_Click(object sender, EventArgs e)
        {
            //const uint SW_NORMAL = 1;

            //ShellExecuteInfo sei = new ShellExecuteInfo();
            //sei.Size = Marshal.SizeOf(sei);
            //sei.Verb = "openas";
            //sei.File = "";
            //sei.Show = SW_NORMAL;

            //if (!ShellExecuteEx(ref sei)) ;
        }
    }


}
