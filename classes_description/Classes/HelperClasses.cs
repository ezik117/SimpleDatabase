using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
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
        public static int DstPreviousImageIndex = 0;
        /// <summary>
        /// Имя ключа изображения узла в источнике перетаскивания (для восстановления изображения после)
        /// </summary>
        public static int SrcPreviousImageIndex = 0;
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
                DstSelectedNode.ImageIndex = DstSelectedNode.SelectedImageIndex = DstPreviousImageIndex;
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
        public static DragDropEffects SelectNode(TreeNode selected)
        {
            // нельзя копировать в самого себя
            if (SrcSelectedNode == selected)
                return DragDropEffects.None;

            // если выбранный узел отличается от выбранного на предыдущем вызове функции
            if (selected != DstSelectedNode)
            {
                // если выбранный узел существует и он не является в статусе "выбран"
                if (selected != null && selected.ImageIndex != (int)IconTypes.Selected)
                {
                    // Восстановим предыдущее состояние
                    if (DstSelectedNode != null)
                    {
                        DstSelectedNode.ImageIndex = DstSelectedNode.SelectedImageIndex = DstPreviousImageIndex;
                        SrcSelectedNode.ImageIndex = SrcSelectedNode.SelectedImageIndex = SrcPreviousImageIndex;
                    }
                    DstPreviousImageIndex = selected.ImageIndex;
                    selected.ImageIndex = selected.SelectedImageIndex = (int)IconTypes.Selected;
                    Debug.WriteLine(selected.Text);
                }
                else if (selected == null)
                {
                    if (DstSelectedNode != null)
                    {
                        DstSelectedNode.ImageIndex = DstSelectedNode.SelectedImageIndex = DstPreviousImageIndex;
                    }
                }

                DstSelectedNode = selected;
            }

            return DragDropEffects.Move;
        }

        public static void Start(TreeNode sourceNode)
        {
            Reset();
            IsDragStarted = true;
            SrcSelectedNode = sourceNode;
            SourceTV = sourceNode.TreeView;
            DstPreviousImageIndex = SrcPreviousImageIndex = sourceNode.ImageIndex;
        }

        public static void End()
        {
            IsDragStarted = false;
            if (DstSelectedNode != null)
                DstSelectedNode.ImageIndex = DstSelectedNode.SelectedImageIndex = DstPreviousImageIndex;
        }
    }

    /// <summary>
    /// Класс для хранения истории перемещений
    /// </summary>
    public static class MOVEHISTORY
    {
        /// <summary>
        /// Список данных о перемещениях
        /// </summary>
        private static List<MoveHistoryElement> list = new List<MoveHistoryElement>();

        /// <summary>
        /// Текущая позиция в списке
        /// </summary>
        private static int currentPos = -1;

        /// <summary>
        /// Сбросить историю
        /// </summary>
        public static void Clear()
        {
            list.Clear();
            currentPos = -1;
        }

        /// <summary>
        /// Возвращает количество элементов в списке перемещений
        /// </summary>
        /// <returns></returns>
        public static int Count()
        {
            return list.Count;
        }

        /// <summary>
        /// Установить новый элемент перемещения. Все элементы после него будут удалены (новая ветвь перемещений).
        /// </summary>
        /// <param name="db">Имя БД</param>
        /// <param name="cid">ROWID каталога</param>
        /// <param name="pid">ROWID оглавления</param>
        public static void Set(string db, long cid, long pid)
        {
            MoveHistoryElement el = new MoveHistoryElement()
            {
                database = db,
                class_id = cid,
                property_id = pid,
                isNextExists = false
            };

            // удалим все элементы после текущего
            while (list.Count > 1 + currentPos) list.RemoveAt(list.Count - 1);

            list.Add(el);
            currentPos++;

            // проверим на лимит
            if (currentPos > 50)
            {
                currentPos--;
                list.RemoveAt(0);
            }
        }

        /// <summary>
        /// Возвращает данные следующего элемента перемещения, если есть или NULL.
        /// </summary>
        /// <returns>Класс элемента перемещения</returns>
        public static MoveHistoryElement GotoNext()
        {
            MoveHistoryElement ret = null;
            if (currentPos < list.Count - 1)
            {
                ret = list[++currentPos];
                ret.isNextExists = currentPos < list.Count - 1;
                ret.isPrevExists = currentPos > 0;
            }

            return ret;
        }

        /// <summary>
        /// Возвращает данные предыдущего элемента перемещения, если есть или NULL.
        /// </summary>
        /// <returns>Класс элемента перемещения</returns>
        public static MoveHistoryElement GotoPrevious()
        {
            MoveHistoryElement ret = null;
            if (currentPos > 0)
            {
                ret = list[--currentPos];
                ret.isNextExists = currentPos < list.Count - 1;
                ret.isPrevExists = currentPos > 0;
            }

            return ret;
        }
    }

    /// <summary>
    /// Вспомогательный класс для MoveHistory
    /// </summary>
    public class MoveHistoryElement
    {
        /// <summary>
        /// Имя БД
        /// </summary>
        public string database = "";
        /// <summary>
        /// ROWID каталога
        /// </summary>
        public long class_id = -1;
        /// <summary>
        /// ROWID оглавления
        /// </summary>
        public long property_id = -1;
        /// <summary>
        /// Флаг, показывающий, есть ли следующий элемент в списке перемещения
        /// </summary>
        public bool isNextExists = false;
        /// <summary>
        /// Флаг, показывающий, есть ли предыдущий элемент в списке перемещения
        /// </summary>
        public bool isPrevExists = false;
    }

    /// <summary>
    /// Класс содержащий вспомогательные функции
    /// </summary>
    public static class HELPER
    {
        /// <summary>
        /// Ищет заданный узел в каталоге
        /// </summary>
        /// <param name="rowid">ROWID (Tag) нужного узла</param>
        /// <returns>Найденный объект TreeNode или null</returns>
        public static TreeNode FindClassNode(long rowid)
        {
            TreeNode ret = null;
            foreach (TreeNode node in VARS.main_form.tvClasses.Nodes)
                if ((long)node.Tag == rowid)
                {
                    ret = node;
                    break;
                }
            return ret;
        }

        /// <summary>
        /// Ищет заданный узел в TV.
        /// </summary>
        /// <param name="node">Родительский узел</param>
        /// <param name="rowid">ROWID (Tag) нужного узла</param>
        /// <returns>Найденный объект TreeNode или null</returns>
        public static TreeNode FindNodeRecursively(TreeNode node, long rowid)
        {
            TreeNode ret = null;
            foreach (TreeNode n in node.Nodes)
            {
                if ((long)n.Tag == rowid)
                {
                    ret = n;
                    break;
                }
                else
                {
                    if (n.Nodes.Count > 0)
                    {
                        ret = FindNodeRecursively(n, rowid);
                        if (ret != null) break;
                    }
                }
            }

            return ret;
        }

        /// <summary>
        /// Перейти к элементу: БД:Каталог:Оглавление
        /// </summary>
        /// <param name="database">Имя БД</param>
        /// <param name="cid">ROWID каталога</param>
        /// <param name="pid">ROWID оглавления</param>
        public static void MoveToElement(string database, long cid, long pid)
        {
            string dbname = Path.GetFileNameWithoutExtension(DATABASE.FileName);
            if (dbname != database)
            {
                // меняем БД
                DATABASE.Close();
                DATABASE.OpenOrCreate(database);
                ClassItem.Load(VARS.main_form);
                VARS.main_form.slblLastUpdate.Text = "Last update: " + DATABASE.GetLastUpdate();
            }

            TreeNode class_node = HELPER.FindClassNode(cid);
            if (class_node != null)
            {
                VARS.property_update_finished = false;
                VARS.main_form.tvClasses.SelectedNode = null;
                VARS.main_form.tvClasses.SelectedNode = class_node;
                while (VARS.property_update_finished != true)
                    Application.DoEvents();
            }

            TreeNode prop_node = HELPER.FindNodeRecursively(VARS.main_form.tvProps.Nodes[0], pid);
            if (prop_node != null)
            {
                VARS.main_form.tvProps.SelectedNode = prop_node;
            }
        }
    }
}
