using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace simple_database
{

    public class PropertyDescription : Description
    {
        public PropertyDescription(Button btn, RichTextBox tb) : base(btn, tb)
        {

        }
    }

    public abstract class Description
    {
        public Button SaveButton;
        public RichTextBox tb;
        public bool wasChanged;

        public TextFormatter textFormatter;

        public void TextChanging()
        {
            if (!wasChanged)
            {
                wasChanged = true;
                SaveButton.ImageKey = "notsaved";
            }
        }

        public void TextSaved()
        {
            wasChanged = false;
            SaveButton.ImageKey = "save";
            this.textFormatter.enabled = false;
        }

        public void ClearText()
        {
            tb.Clear();
            TextSaved();
        }

        public Description(Button SaveButton, RichTextBox tb)
        {
            this.SaveButton = SaveButton;
            this.tb = tb;
            this.textFormatter = new TextFormatter();
            this.textFormatter.enabled = false;
        }
    }

    /// <summary>
    /// Класс для операции "по образцу"
    /// </summary>
    public class TextFormatter
    {
        public Font txtFont = null;
        public Color txtColor = Color.Black;
        public Color txtBgColor = Color.White;
        public bool txtBulletIndent = false;
        public bool enabled = false;

        public void SaveFormat(RichTextBox tb)
        {
            enabled = true;
            txtFont = tb.SelectionFont;
            txtColor = tb.SelectionColor;
            txtBgColor = tb.SelectionBackColor;
            txtBulletIndent = tb.SelectionBullet;
        }

        public void CopyFormat(RichTextBox tb)
        {
            enabled = false;
            tb.SelectionFont = txtFont;
            tb.SelectionColor = txtColor;
            tb.SelectionBackColor = txtBgColor;
            tb.SelectionBullet = txtBulletIndent;
        }
    }

    /// <summary>
    /// Типы свойство (значения ImageIndex в Form1.ImageList)
    /// </summary>
    enum IconTypes : int
    {
        Book = 0,
        Triangle = 1,
        Square = 2,
        Cirle = 3,
        Folder = 4,
        FolderBlue = 5,
        FolderGreen = 6,
        File = 7,
        Selected = 8,
        Attachment = 9,
        FolderRed = 10,
        FolderMagenta = 11,
        FolderGrey = 12,
        Class = 13,
        FileImportant = 14,
        Plugin = 15,
        White = 16
    }

    /// <summary>
    /// Вспомогательный класс для ассоциации кнопок RichTextBox (применяется к Tag)
    /// </summary>
    class RichTextRelation
    {
        public RichTextBox TextBox { get; private set; }
        public Color TextColor;
        public Color BgColor;

        public RichTextRelation(RichTextBox tb)
        {
            TextBox = tb;
            TextColor = Color.Black;
            BgColor = Color.Yellow;
        }
    }

    /// <summary>
    /// Класс объектов состояния для обработки Dran and Drop между объектами TreeView
    /// </summary>
    static class DnD
    {
        /// <summary>
        /// Флаг, показывающий что операция перетаскивания начата
        /// </summary>
        public static bool IsDragStarted = false;
        /// <summary>
        /// Узел, который перетаскивается
        /// </summary>
        public static TreeNode SrcSelectedNode = null;
        /// <summary>
        /// Выбранный узел в приемнике перетаскивания
        /// </summary>
        public static TreeNode DstSelectedNode = null;
        /// <summary>
        /// Имя ключа изображения узла в приемнике перетаскивания (для восстановления изображения после)
        /// </summary>
        public static string DstDefaultImageKey = null;
        /// <summary>
        /// TreeView источника перетаскивания
        /// </summary>
        public static TreeView SourceTV = null;
        /// <summary>
        /// Зона узла источника перетаскивания. Выход курсора за пределы этой зоны инициирует перетаскивание
        /// </summary>
        public static Rectangle StartDragArea = Rectangle.Empty;

        /// <summary>
        /// Сброс (отмена) перетаскивания
        /// </summary>
        public static void Reset()
        {
            if (DstSelectedNode != null)
                DstSelectedNode.ImageKey = DstSelectedNode.SelectedImageKey = DstDefaultImageKey;
            IsDragStarted = false;
            SrcSelectedNode = null;
            DstSelectedNode = null;
            SourceTV = null;
            StartDragArea = Rectangle.Empty;
        }

        /// <summary>
        /// Выбрать узел в приемнике перетаскивания (назначить изображение стрелки)
        /// </summary>
        /// <param name="selected">Узел в приемнике перетаскивания</param>
        public static void SelectNode(TreeNode selected)
        {
            if (selected != DstSelectedNode)
            {
                if (selected != null && selected.ImageKey != "selected")
                {
                    if (DstSelectedNode != null)
                    {
                        DstSelectedNode.ImageKey = DstSelectedNode.SelectedImageKey = DstDefaultImageKey;
                    }
                    DstDefaultImageKey = selected.ImageKey;
                    selected.ImageKey = selected.SelectedImageKey = "selected";
                }
                else if (selected == null)
                {
                    if (DstSelectedNode != null)
                    {
                        DstSelectedNode.ImageKey = DstSelectedNode.SelectedImageKey = DstDefaultImageKey;
                    }
                }
                
                DstSelectedNode = selected;
            }
        }
    }


}
