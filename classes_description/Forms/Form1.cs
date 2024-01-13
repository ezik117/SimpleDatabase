using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using TextEditorNS;

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
        /// <summary>
        /// Объект RichTextBox оглавления.
        /// </summary>
        public TextEditor paramTextEditor;

        // используется для операций DragDrop, содержит выбранный для Drop узел
        private TreeNode DragDropSelectedNode = null;
        // используется для операций DragDrop, содержит индекс картинки узла (будет заменен на индекс выделения)
        private int DragDropSelectedNodeImageIndex = -1;

        /// <summary>
        /// Конструктор формы.
        /// </summary>
        public Form1()
        {
            InitializeComponent();

            VARS.main_form = this;

            slblEmpty.Text = "";
            slblEmpty.Spring = true;

            slblVersion.Text = $"";

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
            DATABASE.Init();
            DATABASE.OpenOrCreate("default");

            // загружаем данные в форму
            ClassItem.Load(this);

            double db_size = DATABASE.GetOpenedDatabaseSize() / 1024.0/ 1024.0;
            slblLastUpdate.Text = $"Last update: {DATABASE.GetLastUpdate()}  |  Size: {db_size:0.0} Mb";


        }

        // Обработка нажатий некоторых клавиш
        private void paramTextEditor_KeyDown(object sender, KeyEventArgs e)
        {
            // CTRL-S. Сохраним данные.
            if (e.Control && e.KeyCode == Keys.S)
            {
                PROPERTY.UpdateDescription(this);
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
            PROPERTY.UpdateDescription(this);
        }

        // Сохранить описание класса
        private void btnClassDescSave_Click(object sender, EventArgs e)
        {
            ClassItem.Update(this);
        }

        /// <summary>
        /// Кнопка окна описания: Открыть в отдельном окне
        /// </summary>
        private void btnDeskOpenInNewWindow_Click(object sender, EventArgs e)
        {
            frmDetachedWindow frm = new frmDetachedWindow();
            frm.rtb.Rtf = paramTextEditor.txtBox.Rtf;
            frm.rtb.BackColor = Color.White;

            string dbname = Path.GetFileNameWithoutExtension(DATABASE.FileName);
            frm.Text = dbname + " / " + PROPERTY.BuildFullNodePath(tvProps.SelectedNode);

            frm.Show();
        }

        /// <summary>
        /// Кнопка окна описания: Увеличить\уменьшить ширину
        /// </summary>
        private void btnDescExpandShrinkWindow_Click(object sender, EventArgs e)
        {
            this.SuspendLayout();
            panel1.SuspendLayout();
            panelDescriptionHolder.SuspendLayout();

            if (btnDescExpandShrinkWindow.ImageKey == "go_left")
            {
                panelContentHolder.Visible = false;
                panelCatalogHolder.Visible = false;
                btnDescExpandShrinkWindow.ImageKey = "go_right";
            }
            else
            {
                panelCatalogHolder.Visible = true;
                panelContentHolder.Visible = true;
                btnDescExpandShrinkWindow.ImageKey = "go_left";
            }

            panelDescriptionHolder.ResumeLayout();
            panel1.ResumeLayout();
            this.ResumeLayout();
        }

        // Выбран другой класса. Считать связанные данные.
        private void tvClasses_AfterSelect(object sender, TreeViewEventArgs e)
        {
            ClassItem.NodeChanged(this);
            VARS.class_update_finished = true;
            if (tvProps.Nodes.Count > 0) tvProps.Nodes[0].Expand();
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
            PROPERTY.Create(this);
        }

        // Удалить свойство и подсвойства
        private void btnPropDel_Click(object sender, EventArgs e)
        {
            PROPERTY.Delete(this);
        }

        // Редактировать свойство
        private void btnPropEdit_Click(object sender, EventArgs e)
        {
            PROPERTY.Update(this);
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
            PROPERTY.PropertyChanged(this);
            if (tvProps.SelectedNode.IsExpanded)
                btnPropCollapseExpand.ImageKey = "collapse";
            else
                btnPropCollapseExpand.ImageKey = "expand";
            VARS.property_update_finished = true;
        }

        // Считать повторно описание свойства.
        private void btnDescReload_Click(object sender, EventArgs e)
        {
            PROPERTY.DescriptionReload(this);
        }

        /// <summary>
        /// Проверка несохраненных данных при закрытии формы.
        /// </summary>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = PROPERTY.CheckForUnsavedDesc(this);
            if (!e.Cancel)
            {
                DATABASE.CloseAll();
            }
        }

        /// <summary>
        /// Меняется выбор свойства. Проверить на сохранение свойства.
        /// </summary>
        private void tvProps_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            e.Cancel = PROPERTY.CheckForUnsavedDesc(this);
        }

        /// <summary>
        /// Меняется выбор класса. Проверить на сохранение описаний класса и свойства.
        /// </summary>
        private void tvClasses_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            e.Cancel = PROPERTY.CheckForUnsavedDesc(this);
        }

        /// <summary>
        /// Действия при загрузке главной формы.
        /// </summary>
        private void Form1_Load(object sender, EventArgs e)
        {
            // Добавим пункты в системное меню
            SystemMenu.AddItems(this);
        }

        /// <summary>
        /// Move the dragged node when the left mouse button is used.
        /// </summary>
        private void tvProps_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                DoDragDrop(e.Item, DragDropEffects.Move);
            }
        }

        /// <summary>
        /// Set the target drop effect to the effect 
        /// specified in the ItemDrag event handler.
        /// </summary>
        private void tvProps_DragEnter(object sender, DragEventArgs e)
        {
            // Check if datatype is acceptable
            if (e.Data.GetDataPresent(typeof(TreeNode))) e.Effect = e.AllowedEffect; 
        }

        /// <summary>
        /// Select the node under the mouse pointer to indicate the 
        /// expected drop location.
        /// </summary>
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

        /// <summary>
        /// Item is dropped. Finish.
        /// </summary>
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
                PROPERTY.ChangeParent((long)draggedNode.Tag, (long)targetNode.Tag, this);
                draggedNode.Remove();
                targetNode.Nodes.Add(draggedNode);
            }
        }

        /// <summary>
        /// Determine whether one node is a parent 
        /// or ancestor of a second node.
        /// </summary>
        /// <param name="node1"></param>
        /// <param name="node2"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Поиск в оглавлении или текста в параметрах
        /// </summary>
        private void btnPropSearch_Click(object sender, EventArgs e)
        {
            frmSearchProperty frm = new frmSearchProperty();
            frm.tb = paramTextEditor.txtBox;
            frm.InitSearch(tvProps, false);
            //classTextEditor.txtBox.Focus();
            frm.Left = this.Left + panelContentHolder.Left + panelContentHolder.Width;
            frm.Top = (int)(this.Top + this.Height/2 - frm.Height / 2);
            frm.ShowDialog();
        }

        /// <summary>
        /// Нажата клавиша в окне свойств класса. Определить действия.
        /// </summary>
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

        /// <summary>
        /// Изменился размер главной формы. Уменьшаем панель описания категорий до 30% от высоты окна
        /// </summary>
        private void Form1_Resize(object sender, EventArgs e)
        {
            panel1.Height = (int)(this.Height * 0.7);
        }

        // Окно свойств. Контекстное меню: открыть вложение
        private void tsmSaveAndOpenAttachment_Click(object sender, EventArgs e)
        {
            PROPERTY.ExtractAndRunAttachment(this);
        }

        // Окно свойств. Контекстное меню: сохранить вложение
        private void tsmSaveAttachmentToFile_Click(object sender, EventArgs e)
        {
            PROPERTY.ExtractAttachment(this);
        }

        // Окно свойств. Контекстное меню: открыть с помощью
        private void tsmOpenAttachmentWith_Click(object sender, EventArgs e)
        {
            PROPERTY.ExtractAndRunAttachment(this, true);
        }

        /// <summary>
        /// Контекстное меню оглавления.  Поднимает узел вверх на одну позицию. 
        /// Подготовительное действие для для нумерации.
        /// Не сохраняется в БД.
        /// </summary>
        private void tsmiMoveUp_Click(object sender, EventArgs e)
        {
            PROPERTY.MoveUp(tvProps, tvProps.SelectedNode);
        }

        /// <summary>
        /// Контекстное меню оглавления. Опускает узел вниз на одну позицию. 
        /// Подготовительное действие для для нумерации.
        /// Не сохраняется в БД.
        /// </summary>
        private void tsmiMoveDown_Click(object sender, EventArgs e)
        {
            PROPERTY.MoveDown(tvProps, tvProps.SelectedNode);
        }

        /// <summary>
        /// Контекстное меню оглавления. Нумерует (перенумерует) все дочерние узлы.
        /// Формат нумерации: две цифры с лидирующем нулем и точкой.
        /// Сохраняется в БД.
        /// </summary>
        private void tsmiRenumering_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(
                "Пронумеровать (перенумеровать) список дочерних элементов?",
                "Переименование",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.No) return;

            PROPERTY.Renumbering(this, tvProps, tvProps.SelectedNode);
        }

        /// <summary>
        /// Контекстное меню оглавления. Денумерует (удаляет нумерацию) все дочерние узлы.
        /// Сохраняется в БД.
        /// </summary>
        private void tsmiDenumering_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(
                "Убрать нумерацию дочерних элементов?",
                "Переименование",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.No) return;

            PROPERTY.Denumbering(this, tvProps, tvProps.SelectedNode);
        }

        /// <summary>
        /// Контекстное меню оглавления. Поиск в оглавлении.
        /// </summary>
        private void tsmiSearch_Click(object sender, EventArgs e)
        {
            btnPropSearch.PerformClick();
        }

        /// <summary>
        /// Иконка быстрого доступа оглавления.
        /// Bookmark.
        /// Показывает окно управления закладками
        /// </summary>
        private void btnPropBookmark_Click(object sender, EventArgs e)
        {
            frmBookmarks frm = new frmBookmarks();
            frm.ShowDialog();
        }

        /// <summary>
        /// Иконка быстрого доступа оглавления.
        /// Bookmark.
        /// Добавляет текущий элемент в закладками
        /// </summary>
        private void btnPropFavouritesAdd_Click(object sender, EventArgs e)
        {
            PROPERTY.AddPropertyToBookmark(tvProps.SelectedNode);
        }

        /// <summary>
        /// Контекстное меню оглавления.
        /// Сохраняет выбранный узлел в списке быстрого доступа к узлам.
        /// </summary>
        private void tsmiBookmarkAdd_Click(object sender, EventArgs e)
        {
            PROPERTY.AddPropertyToBookmark(tvProps.SelectedNode);
        }

        /// <summary>
        /// Контекстное меню оглавления.
        /// Управление закладками
        /// </summary>
        private void tsmiBookmarkManagement_Click(object sender, EventArgs e)
        {
            btnPropBookmark.PerformClick();
        }

        /// <summary>
        /// Открытие контекстного меню оглавления. Проверяем какие пункты меню доступны.
        /// </summary>
        private void ctxMenuCharters_Opening(object sender, CancelEventArgs e)
        {
            TreeNode node = tvProps.SelectedNode;
            if (node.ImageIndex == (int)IconTypes.Attachment)
            {
                tsmiAttachments.Visible = true;
            }
            else
            {
                tsmiAttachments.Visible = false;
            }
        }

        /// <summary>
        /// Иконка быстрого доступа оглавления.
        /// Bookmark.
        /// Показывает окно управления избранным
        /// </summary>
        private void btnPropFavourites_Click(object sender, EventArgs e)
        {
            frmFavourites frm = new frmFavourites();
            frm.ShowDialog();
        }

        /// <summary>
        /// Контекстное меню оглавления.
        /// Сохраняет выбранный узлел в списке избранного.
        /// </summary>
        private void tsmiFavouritesAdd_Click(object sender, EventArgs e)
        {
            PROPERTY.AddPropertyToFavourites(tvProps.SelectedNode);
        }

        /// <summary>
        /// Контекстное меню оглавления.
        /// Управление избранным
        /// </summary>
        private void tsmiFavouritesManagement_Click(object sender, EventArgs e)
        {
            btnPropFavourites.PerformClick();
        }

        /// <summary>
        /// Контекстное меню оглавления.
        /// Управление ключевыми словами
        /// </summary>
        private void tsmiHashtags_Click(object sender, EventArgs e)
        {
            frmKeywords frm = new frmKeywords();
            frm.PropertyID = (long)tvProps.SelectedNode.Tag;
            frm.ShowDialog();
        }

        /// <summary>
        /// Выводит индикаторы (значки) текущего элемента оглавления
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pbMarks_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(Brushes.Transparent, pbMarks.ClientRectangle);
            int x = 0;

            if (PROPERTY.propertyInfo.isBookmarked)
            {
                imageList1.Draw(e.Graphics, x, 0, 30);
                x += 20;
            }

            if (PROPERTY.propertyInfo.isFavourite)
            {
                imageList1.Draw(e.Graphics, x, 0, 31);
                x += 20;
            }

            if (PROPERTY.propertyInfo.thereAreKeywords)
            {
                imageList1.Draw(e.Graphics, x, 0, 32);
                x += 20;
            }
        }

        /// <summary>
        /// Открытие вложения по двойному щелчку мыши
        /// </summary>
        private void tvProps_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.ImageIndex == (int)IconTypes.Attachment)
            {
                tsmiAttachmentOpen.PerformClick();
            }
        }
    }


}
