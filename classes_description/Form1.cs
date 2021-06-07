using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace simple_database
{
    using SqlRows = List<Dictionary<string, object>>;

    public partial class Form1 : Form
    {
        public PropertyDescription propDescr;
        public PropertyDescription classDescr;
        public Database db;

        // используется для операций DragDrop, содержит выбранный для Drop узел
        private TreeNode DragDropSelectedNode = null;
        // используется для операций DragDrop, содержит индекс картинки узла (будет заменен на индекс выделения)
        private int DragDropSelectedNodeImageIndex = -1;

        public Form1()
        {
            InitializeComponent();

            // зададим принадлежность к двум разных RichTextBoxам для кнопок их управления
            btnTextColorApply.Tag = new RichTextRelation(tbDescEdit);
            btnBgColorApply.Tag = new RichTextRelation(tbDescEdit);
            btnTextColorApply2.Tag = new RichTextRelation(tbClassDescEdit);
            btnBgColorApply2.Tag = new RichTextRelation(tbClassDescEdit);

            btnTextBold.Tag = btnTextItalic.Tag = btnTextUnderline.Tag = btnTextBulletList.Tag = btnTextFont.Tag = tbDescEdit;
            btnTextBold2.Tag = btnTextItalic2.Tag = btnTextUnderline2.Tag = btnTextBulletList2.Tag = btnTextFont2.Tag = tbClassDescEdit;

            btnBgColorPickup.Tag = btnBgColorApply;
            btnBgColorPickup2.Tag = btnBgColorApply2;

            btnTextColorPickup.Tag = btnTextColorApply;
            btnTextColorPickup2.Tag = btnTextColorApply2;

            propDescr = new PropertyDescription(btnDescSave, tbDescEdit);
            classDescr = new PropertyDescription(btnClassDescSave, tbClassDescEdit);

            // очистим папку TEMP, если там что то есть
            CleanUpTemp();

            // создаем или открываем БД по умолчанию
            db = new Database();
            db.OpenOrCreate("default");

            // загружаем данные в форму
            ClassItem.Load(this);
            
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
            if (btnPropCollapseExpand.ImageKey == "collapse")
            {
                btnPropCollapseExpand.ImageKey = "expand";
                tvProps.CollapseAll();
            }
            else
            {
                btnPropCollapseExpand.ImageKey = "collapse";
                tvProps.ExpandAll();
            }
        }

        // Выбрано другое свойство. Считать связанные данные.
        private void tvProps_AfterSelect(object sender, TreeViewEventArgs e)
        {
            PropertyItem.PropertyChanged(this);
        }

        // Нажата клавиша в окне описания свойства. Определить действия
        private void tbDescEdit_KeyDown(object sender, KeyEventArgs e)
        {
            // CTRL-S. Сохраним данные.
            if (e.Control && e.KeyCode == Keys.S)
            {
                PropertyItem.UpdateDescription(this);
                e.Handled = true;
                return;
            }

            // CTRL-C. Скопируем в буфер обмена.
            if (e.Control && e.KeyCode == Keys.C)
            {
                DataObject dto = new DataObject();
                dto.SetText(tbDescEdit.SelectedRtf, TextDataFormat.Rtf);
                dto.SetText(tbDescEdit.SelectedText, TextDataFormat.UnicodeText);
                Clipboard.Clear();
                Clipboard.SetDataObject(dto);
                e.Handled = true;
                return;
            }
        }

        // Нажата клавиша в окне описания класса. Определить действия
        private void tbClassDescEdit_KeyDown(object sender, KeyEventArgs e)
        {
            // CTRL-S. Сохраним данные.
            if (e.Control && e.KeyCode == Keys.S)
            {
                ClassItem.Update(this);
                e.Handled = true;
                return;
            }

            // CTRL-C. Скопируем в буфер обмена.
            if (e.Control && e.KeyCode == Keys.C)
            {
                DataObject dto = new DataObject();
                dto.SetText(tbClassDescEdit.SelectedRtf, TextDataFormat.Rtf);
                dto.SetText(tbClassDescEdit.SelectedText, TextDataFormat.UnicodeText);
                Clipboard.Clear();
                Clipboard.SetDataObject(dto);
                e.Handled = true;
                return;
            }
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

        // Тест описания свойства изменен.
        private void tbDescEdit_TextChanged(object sender, EventArgs e)
        {
            propDescr.TextChanging();
        }

        // Текст описания класса изменен.
        private void tbClassDescEdit_TextChanged(object sender, EventArgs e)
        {
            classDescr.TextChanging();
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

        // Редактор. Жирный текст.
        private void btnTextBold_Click(object sender, EventArgs e)
        {
            RichTextBox tb = (RichTextBox)((Button)sender).Tag;

            if (tb.SelectionFont != null)
            {
                FontStyle newFontStyle = tb.SelectionFont.Style ^ FontStyle.Bold;
                tb.SelectionFont = new Font(tb.SelectionFont.FontFamily, tb.SelectionFont.Size, newFontStyle);
            }
        }

        // Редактор. Наклонный текст.
        private void btnTextItalic_Click(object sender, EventArgs e)
        {
            RichTextBox tb = (RichTextBox)((Button)sender).Tag;

            if (tb.SelectionFont != null)
            {
                FontStyle newFontStyle = tb.SelectionFont.Style ^ FontStyle.Italic;
                tb.SelectionFont = new Font(tb.SelectionFont.FontFamily, tb.SelectionFont.Size, newFontStyle);
            }
        }

        // Редактор. Подкчеркнутый текст.
        private void btnTextUnderline_Click(object sender, EventArgs e)
        {
            RichTextBox tb = (RichTextBox)((Button)sender).Tag;

            if (tb.SelectionFont != null)
            {
                FontStyle newFontStyle = tb.SelectionFont.Style ^ FontStyle.Underline;
                tb.SelectionFont = new Font(tb.SelectionFont.FontFamily, tb.SelectionFont.Size, newFontStyle);
            }
        }

        // Редактор. Ненумерованный список.
        private void btnTextBulletList_Click(object sender, EventArgs e)
        {
            RichTextBox tb = (RichTextBox)((Button)sender).Tag;

            tb.SelectionBullet = !tb.SelectionBullet;
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

        // Редактор. отрисовка цвета текста в кнопке "Применить цвет текста"
        private void btnTextColorApply_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(new SolidBrush(((RichTextRelation)((Button)sender).Tag).TextColor),
                                     new Rectangle(4, 4, 12, 12));
        }

        // Редактор. Выбор цвета для текста
        private void btnTextColorPickup_Click(object sender, EventArgs e)
        {
            Button btnAppy = (Button)((Button)sender).Tag;
            ColorDialog cd = new ColorDialog();
            if (cd.ShowDialog() == DialogResult.OK)
            {
                ((RichTextRelation)btnAppy.Tag).TextColor = 
                    ((RichTextRelation)btnAppy.Tag).TextBox.SelectionColor = 
                        cd.Color;
                btnAppy.Invalidate();
            }
        }

        // Редактор. Применить цвет текста к выделенному тексту
        private void btnTextColorApply_Click(object sender, EventArgs e)
        {
            ((RichTextRelation)((Button)sender).Tag).TextBox.SelectionColor =
                ((RichTextRelation)((Button)sender).Tag).TextColor;
        }

        // Редактор. отрисовка цвета выделения в кнопке "Применить цвет выделения"
        private void btnBgColorApply_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(new SolidBrush(((RichTextRelation)((Button)sender).Tag).BgColor),
                         new Rectangle(4, 4, 12, 12));
        }

        // Редактор. Выбрать цвет выделения
        private void btnBgColorPickup_Click(object sender, EventArgs e)
        {
            Button btnAppy = (Button)((Button)sender).Tag;
            ColorDialog cd = new ColorDialog();
            if (cd.ShowDialog() == DialogResult.OK)
            {
                ((RichTextRelation)btnAppy.Tag).BgColor =
                    ((RichTextRelation)btnAppy.Tag).TextBox.SelectionBackColor =
                        cd.Color;
                btnAppy.Invalidate();
            }
        }

        // Редактор. Применить цвет выделения к выделенному тексту
        private void btnBgColorApply_Click(object sender, EventArgs e)
        {
            ((RichTextRelation)((Button)sender).Tag).TextBox.SelectionBackColor =
                ((RichTextRelation)((Button)sender).Tag).BgColor;
        }

        // Редактор. Выбор фонта
        private void btnTextFont_Click(object sender, EventArgs e)
        {
            RichTextBox tb = (RichTextBox)((Button)sender).Tag;

            FontDialog fd = new FontDialog();
            fd.Font = tb.SelectionFont;
            if (fd.ShowDialog() == DialogResult.OK)
            {
                tb.SelectionFont = fd.Font;
            }
        }

        // Извлечь вложение.
        private void btnAttachment_Click(object sender, EventArgs e)
        {
            PropertyItem.ExtractAttachment(this);
        }

        // Двойной клик на свойстве. Открыть если вложение.
        private void tvProps_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.ImageIndex == (int)IconTypes.Attachment)
                btnAttachment.PerformClick();
        }

        // Редактор. Сохранить Формат по образцу 
        private void btnTextAutoFormat_Click(object sender, EventArgs e)
        {
            propDescr.textFormatter.SaveFormat(tbDescEdit);
        }

        // Редактор. Восстановить Формат по образцу
        private void tbDescEdit_MouseUp(object sender, MouseEventArgs e)
        {
            if (propDescr.textFormatter.enabled)
                propDescr.textFormatter.CopyFormat(tbDescEdit);
        }

        // Редактор. Сохранить Формат по образцу 
        private void btnTextAutoFormat2_Click(object sender, EventArgs e)
        {
            classDescr.textFormatter.SaveFormat(tbClassDescEdit);
        }

        // Редактор. Сохранить Формат по образцу 
        private void tbClassDescEdit_MouseUp(object sender, MouseEventArgs e)
        {
            if (classDescr.textFormatter.enabled)
                classDescr.textFormatter.CopyFormat(tbClassDescEdit);
        }
    }


}
