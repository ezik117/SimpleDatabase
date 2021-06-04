using System;
using System.Collections.Generic;
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
                SaveButton.ImageIndex = (int)IconTypes.NotSavedIcon;
            }
        }

        public void TextSaved()
        {
            wasChanged = false;
            SaveButton.ImageIndex = (int)IconTypes.SaveIcon;
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
    enum IconTypes : long
    {
        SaveIcon = 3,
        NotSavedIcon = 4,
        Triangle = 9,
        Square = 13,
        Cirle = 11,
        Folder = 6,
        CollapseAll = 14,
        ExpandAll = 15,
        DragDrop = 17,
        Search = 18
    }
}
