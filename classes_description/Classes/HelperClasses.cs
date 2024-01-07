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
        FileImportant = 14
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


}
