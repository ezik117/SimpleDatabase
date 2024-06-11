using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
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

        /// <summary>
        /// Конструктор формы.
        /// </summary>
        public Form1()
        {
            InitializeComponent();

            VARS.main_form = this;
            VARS.temp_folder = Path.Combine(Application.StartupPath, "temp");

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

            // загрузим синтаксические правила
            try
            {
                string xml = DATABASE.LoadSyntaxRules();
                HELPER.DeserializeSyntaxRules(xml, ref VARS.syntaxRules);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Невозможно загрузить синтаксические правила. Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // добавим контестное меню подсвветки синтаксиса
            HELPER.AddPropertiesContextMenuItems();
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
                string[] files = System.IO.Directory.GetFiles(VARS.temp_folder);
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
        /// Начало перетаскивания элемента оглавления
        /// </summary>
        private void tvProps_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (e.Button == MouseButtons.Left && e.Item.GetType() == typeof(TreeNode))
            {
                DnD.Start((TreeNode)e.Item);
                DoDragDrop(e.Item, DragDropEffects.Move);
            }
        }

        /// <summary>
        /// Для tvProps определяем принимать ли Drag and Drop элемент
        /// </summary>
        private void tvProps_DragEnter(object sender, DragEventArgs e)
        {
            // Вставлять можно как из tvProps так и из tvClasses
            if (DnD.IsDragStarted &&
                (DnD.SourceTV == tvProps || DnD.SourceTV == tvClasses) &&
                e.Data.GetDataPresent(typeof(TreeNode)))
            {
                // нельзя перетаскивать самого себя в себя
                if (DnD.SourceTV == tvClasses && tvClasses.SelectedNode.Tag == ((TreeNode)e.Data.GetData(typeof(TreeNode))).Tag)
                    return;
                else
                    e.Effect = e.AllowedEffect;
            }
        }

        /// <summary>
        /// Для tvProps подсвечиваем в какой каталог добавить оглавление
        /// </summary>
        private void tvProps_DragOver(object sender, DragEventArgs e)
        {
            // Вставлять можно как из tvProps так и из tvClasses
            if (!DnD.IsDragStarted ||
                (DnD.SourceTV != tvProps && DnD.SourceTV != tvClasses) ||
                !e.Data.GetDataPresent(typeof(TreeNode)))
            {
                return;
            }

            // нельзя перетаскивать самого себя в себя
            if (DnD.SourceTV == tvClasses && tvClasses.SelectedNode.Tag == ((TreeNode)e.Data.GetData(typeof(TreeNode))).Tag)
                return;

            // Определяем узел под курсором и выделяем его
            Point targetPoint = ((TreeView)sender).PointToClient(new Point(e.X, e.Y));
            TreeNode selected = ((TreeView)sender).GetNodeAt(targetPoint);

            // выход за пределы 
            if (selected == null)
            {
                e.Effect = DragDropEffects.None;
                DnD.SelectNode(null);
                return;
            }

            e.Effect = DnD.SelectNode(selected);
        }

        /// <summary>
        /// Item is dropped. Finish.
        /// </summary>
        private void tvProps_DragDrop(object sender, DragEventArgs e)
        {
            // Retrieve the client coordinates of the drop location.
            Point targetPoint = ((TreeView)sender).PointToClient(new Point(e.X, e.Y));

            // Retrieve the node at the drop location.
            TreeNode targetNode = ((TreeView)sender).GetNodeAt(targetPoint);

            // Retrieve the node that was dragged.
            TreeNode draggedNode = (TreeNode)e.Data.GetData(typeof(TreeNode));

            if (DnD.SourceTV == tvProps)
            {
                // Confirm that the node at the drop location is not 
                // the dragged node or a descendant of the dragged node.
                if (!draggedNode.Equals(targetNode) && !ContainsNode(draggedNode, targetNode))
                {
                    PROPERTY.ChangeParent((long)draggedNode.Tag, (long)targetNode.Tag, this);
                    draggedNode.Remove();
                    targetNode.Nodes.Add(draggedNode);
                }
            } else if (DnD.SourceTV == tvClasses)
            {
                if (DATABASE.MoveClassToProperty((long)draggedNode.Tag, (long)targetNode.Tag, (long)tvClasses.SelectedNode.Tag, draggedNode.Text))
                {
                    tvClasses.Nodes.Remove(draggedNode);
                    tvClasses_AfterSelect(null, null);
                }
            }

            DnD.End();
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
            if (e.Control)
            {
                // CTRL-F. Поиск параметра
                if (e.KeyCode == Keys.F)
                {
                    btnPropSearch.PerformClick();
                    e.Handled = true;
                    return;
                }
                // CTRL-C. Копирование в буфер
                else if (e.KeyCode == Keys.C)
                {
                    if (tvProps.SelectedNode != null)
                        Clipboard.SetText(tvProps.SelectedNode.Text);
                }
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

            tsmiAttachments.Visible = false;

            if (node.ImageIndex == (int)IconTypes.Attachment)
            {
                tsmiAttachments.Visible = true;
            }
            else if (node.ImageIndex == (int)IconTypes.Plugin)
            {
                tsmiPluginCreate.Visible = false;
                tsmiPluginEdit.Visible = true;
                tsmiPluginExecute.Visible = true;
                tsmiPluginSaveTo.Visible = true;
                tsmiPluginRename.Visible = true;
            }
            else
            {
                tsmiPluginCreate.Visible = true;
                tsmiPluginEdit.Visible = false;
                tsmiPluginExecute.Visible = false;
                tsmiPluginSaveTo.Visible = false;
                tsmiPluginRename.Visible = false;
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
            else if (e.Node.ImageIndex == (int)IconTypes.Plugin)
            {
                PROPERTY.PluginExecute(e.Node);
            }
        }

        /// <summary>
        /// Контекстное меню: Плагин - Запустить
        /// </summary>
        private void tsmiPluginExecute_Click(object sender, EventArgs e)
        {
            PROPERTY.PluginExecute(tvProps.SelectedNode);
        }

        /// <summary>
        /// Контекстное меню: Плагин - Сохранить в файл...
        /// </summary>
        private void tsmiPluginSaveTo_Click(object sender, EventArgs e)
        {
            PROPERTY.PluginSaveTo(tvProps.SelectedNode);
        }

        /// <summary>
        /// Контекстное меню: Плагин - Редактировать
        /// </summary>
        private void tsmiPluginEdit_Click(object sender, EventArgs e)
        {
            PROPERTY.PluginEdit(tvProps.SelectedNode);
        }

        /// <summary>
        /// Контекстное меню: Плагин - Создать
        /// </summary>
        private void tsmiPluginCreate_Click(object sender, EventArgs e)
        {
            PROPERTY.PluginCreate(tvProps.SelectedNode);
        }

        /// <summary>
        /// Контекстное меню: Плагин - Переименовать
        /// </summary>
        private void tsmiPluginRename_Click(object sender, EventArgs e)
        {
            PROPERTY.PluginRename(tvProps.SelectedNode);
        }

        /// <summary>
        /// Кнопка быстрой смены БД
        /// </summary>
        private void btnChangeDb_Click(object sender, EventArgs e)
        {
            frmDbManager frm = new frmDbManager();
            frm.dbName = Path.GetFileNameWithoutExtension(DATABASE.FileName);
            frm.ShowDialog();
            if (frm.action == 3)
            {
                DATABASE.Close();
                DATABASE.OpenOrCreate(frm.dbName);
                ClassItem.Load(this);

                double db_size = DATABASE.GetOpenedDatabaseSize() / 1024.0 / 1024.0;
                slblLastUpdate.Text = $"Last update: {DATABASE.GetLastUpdate()}  |  Size: {db_size:0.0} Mb";
            }
        }

        /// <summary>
        /// Начало перетаскивания из tvClasses
        /// </summary>
        private void tvClasses_ItemDrag(object sender, ItemDragEventArgs e)
        {
            Debug.WriteLine($"Item: {((TreeNode)e.Item).Text}");
            if (e.Button == MouseButtons.Left && e.Item.GetType() == typeof(TreeNode))
            {
                DnD.Start((TreeNode)e.Item);
                DoDragDrop(e.Item, DragDropEffects.Move);
            }
        }

        /// <summary>
        /// Для tvClasses определяем принимать ли Drag and Drop элемент
        /// </summary>
        private void tvClasses_DragEnter(object sender, DragEventArgs e)
        {
            // Вставлять можно только из tvProps
            if (DnD.IsDragStarted &&
                DnD.SourceTV == tvProps &&
                e.Data.GetDataPresent(typeof(TreeNode)))
            {
                e.Effect = e.AllowedEffect;
            }
        }

        /// <summary>
        /// В tvClasses переместили элемент
        /// </summary>
        private void tvClasses_DragDrop(object sender, DragEventArgs e)
        {
            frmConfirmPropertyMove frm = new frmConfirmPropertyMove();
            frm.ShowDialog();
            if (!frm.Cancel)
            {
                if (frm.MoveAsNewClass)
                {
                    long classId = DATABASE.MoveProperyToClass((long)DnD.SrcSelectedNode.Tag, -1, (long)tvClasses.SelectedNode.Tag, "");
                    TreeNode node = new TreeNode(DnD.SrcSelectedNode.Text);
                    node.ImageIndex = (int)IconTypes.Transparent;
                    node.SelectedImageIndex = (int)IconTypes.Book;
                    node.Tag = classId;
                    tvClasses.Nodes.Add(node);
                    tvClasses.SelectedNode = node;
                }
                else
                {
                    if (DnD.DstSelectedNode == null)
                    {
                        MessageBox.Show("Не выбран каталог", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    }
                    else
                    {
                        DATABASE.MoveProperyToClass((long)DnD.SrcSelectedNode.Tag, (long)DnD.DstSelectedNode.Tag, (long)tvClasses.SelectedNode.Tag, DnD.DstSelectedNode.Text);
                        tvClasses_AfterSelect(null, null);
                        DnD.SrcSelectedNode.Remove();
                    }
                }
            }

            DnD.End();
        }

        /// <summary>
        /// Для tvClasses подсвечиваем в какой каталог добавить оглавление
        /// </summary>
        private void tvClasses_DragOver(object sender, DragEventArgs e)
        {
            // Вставлять можно только из tvProps
            if (!DnD.IsDragStarted ||
                DnD.SourceTV != tvProps ||
                !e.Data.GetDataPresent(typeof(TreeNode)))
            {
                e.Effect = DragDropEffects.None;
                return;
            }

            // Определяем узел под курсором и выделяем его
            Point targetPoint = ((TreeView)sender).PointToClient(new Point(e.X, e.Y));
            TreeNode selected = ((TreeView)sender).GetNodeAt(targetPoint);
            DnD.SelectNode(selected);
            e.Effect = DragDropEffects.Move;
        }

        /// <summary>
        /// Для tvClasses операция Dran and Drop вышла за пределы видимости
        /// </summary>
        private void tvClasses_DragLeave(object sender, EventArgs e)
        {
            if (DnD.DstSelectedNode != null)
            {
                DnD.SelectNode(null);
            }
        }

        /// <summary>
        /// Для tvProps операция Dran and Drop вышла за пределы видимости
        /// </summary>
        private void tvProps_DragLeave(object sender, EventArgs e)
        {
            if (DnD.DstSelectedNode != null)
            {
                DnD.SelectNode(null);
            }
        }

        /// <summary>
        /// Нажата кнопка "История перемещения: назад
        /// </summary>
        private void btnPropBackward_Click(object sender, EventArgs e)
        {
            MoveHistoryElement el = MOVEHISTORY.GotoPrevious();
            if (el == null) return;

            btnPropBackward.ImageKey = el.isPrevExists ? "backward" : "backward-gray";
            btnPropForward.ImageKey = el.isNextExists ? "forward" : "forward-gray";

            VARS.moving_over_history = true;
            HELPER.MoveToElement(el.database, el.class_id, el.property_id);
        }

        /// <summary>
        /// Нажата кнопка "История перемещения: вперед
        /// </summary>
        private void btnPropForward_Click(object sender, EventArgs e)
        {
            MoveHistoryElement el = MOVEHISTORY.GotoNext();
            if (el == null) return;

            btnPropBackward.ImageKey = el.isPrevExists ? "backward" : "backward-gray";
            btnPropForward.ImageKey = el.isNextExists ? "forward" : "forward-gray";

            VARS.moving_over_history = true;
            HELPER.MoveToElement(el.database, el.class_id, el.property_id);
        }

        /// <summary>
        /// Обработка глобальных нажатий клавиш
        /// </summary>
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt)
            {
                switch (e.KeyCode)
                {
                    // История перемещений: назад
                    case Keys.Left:
                        btnPropBackward.PerformClick();
                        e.SuppressKeyPress = true;
                        e.Handled = true;
                        break;

                    // История перемещений: вперед
                    case Keys.Right:
                        btnPropForward.PerformClick();
                        e.SuppressKeyPress = true;
                        e.Handled = true;
                        break;
                }
            }
        }


    }


}
