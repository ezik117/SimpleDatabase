using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace classes_description
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
        }
    }

    /// <summary>
    /// Типы свойство (значения ImageIndex в Form1.ImageList)
    /// </summary>
    enum IconTypes : int
    {
        Class = 0,
        Triangle = 1,
        Square = 2,
        Cirle = 3,
        Folder = 4,
        FolderBlue = 5,
        FolderGreen = 6,
        File = 7
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
