using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace simple_database
{

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
        Transparent = 16,
        Gray = 17
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
        public static int DstPreviousImageIndex_Selected = 0;
        /// <summary>
        /// Имя ключа изображения узла в источнике перетаскивания (для восстановления изображения после)
        /// </summary>
        public static int SrcPreviousImageIndex = 0;
        public static int SrcPreviousImageIndex_Selected = 0;
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
            {
                DstSelectedNode.ImageIndex = DstPreviousImageIndex;
                DstSelectedNode.SelectedImageIndex = DstPreviousImageIndex_Selected;
            }
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
                        DstSelectedNode.ImageIndex = DstPreviousImageIndex;
                        DstSelectedNode.SelectedImageIndex = DstPreviousImageIndex_Selected;
                        SrcSelectedNode.ImageIndex = SrcPreviousImageIndex;
                        SrcSelectedNode.SelectedImageIndex = SrcPreviousImageIndex_Selected;
                    }
                    DstPreviousImageIndex = selected.ImageIndex;
                    DstPreviousImageIndex_Selected = selected.SelectedImageIndex;
                    selected.ImageIndex = selected.SelectedImageIndex = (int)IconTypes.Selected;
                    Debug.WriteLine(selected.Text);
                }
                else if (selected == null)
                {
                    if (DstSelectedNode != null)
                    {
                        DstSelectedNode.ImageIndex = DstPreviousImageIndex;
                        DstSelectedNode.SelectedImageIndex = DstPreviousImageIndex_Selected;
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
            DstPreviousImageIndex_Selected = SrcPreviousImageIndex_Selected = sourceNode.SelectedImageIndex;
        }

        public static void End()
        {
            IsDragStarted = false;
            if (DstSelectedNode != null)
            {
                DstSelectedNode.ImageIndex = DstPreviousImageIndex;
                DstSelectedNode.SelectedImageIndex = DstPreviousImageIndex_Selected;
            }
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
        /// Добавляет к стандартному контекстному меню редактора, подменю подсветки синтаксиса
        /// </summary>
        public static void AddPropertiesContextMenuItems()
        {
            ToolStripItem item;
            ToolStripMenuItem it;

            // Удалим старые контролы
            RemoveAllExtraContextMenus("extra", VARS.main_form.paramTextEditor.txtBox.ContextMenuStrip);

            // Добавим базовые
            item = new ToolStripSeparator();
            item.Name = "extra01";
            VARS.main_form.paramTextEditor.txtBox.ContextMenuStrip.Items.Add(item);

            item = new ToolStripMenuItem("Подсветка синтаксиса");
            item.Name = "extra02";
            VARS.main_form.paramTextEditor.txtBox.ContextMenuStrip.Items.Add(item);
            it = (ToolStripMenuItem)item;
            it.DropDownItems.Add("Редактор", null, customCtxMenu_Syntax_Editor_Click);
            it.DropDownItems.Add("-");
            it.DropDownItems.Add("default 1", null, customCtxMenu_Syntax_Basic1_Click);

            // Добавим пользовательские
            foreach (SyntaxRule rule in VARS.syntaxRules.Rules)
            {
                ToolStripMenuItem user_item = new ToolStripMenuItem(rule.Name);
                user_item.Tag = rule;
                it.DropDownItems.Add(user_item);
                user_item.Click += User_item_Click;
            }
        }

        /// <summary>
        /// Функция обработки вызова пользовательского меню синтаксической раскраски
        /// </summary>
        private static void User_item_Click(object sender, EventArgs e)
        {
            SyntaxRule rule = (SyntaxRule)((ToolStripMenuItem)sender).Tag;
            RichTextBox rtb = VARS.main_form.paramTextEditor.txtBox;
            TextEditorNS.WinAPI.SendMessage(rtb.Handle, TextEditorNS.WinAPI.WM_SETREDRAW, 0, IntPtr.Zero);

            int selLenght = rtb.SelectionLength;
            int selStart = selLenght == 0 ? 0 : rtb.SelectionStart;
            
            string selectedText = selLenght == 0 ? rtb.Text : rtb.SelectedText;

            foreach (RuleRow rr in rule.Rules)
            {
                if (rr.Enabled)
                {
                    RegexOptions ro = rr.SingleLine ? RegexOptions.Multiline : RegexOptions.Singleline;
                    if (rr.Case) ro |= RegexOptions.IgnoreCase;
                    foreach (string kw in rr.RuleElements)
                    {
                        try
                        {
                            MatchCollection mm = Regex.Matches(selectedText, kw, ro);
                            foreach (Match m in mm)
                            {
                                rtb.Select(selStart + m.Index, m.Length);
                                rtb.SelectionColor = Color.FromArgb(rr.Color);
                                if (rr.FBold || rr.FItalic)
                                {
                                    rtb.SelectionFont = new Font(rtb.SelectionFont ?? rtb.Font,
                                        (rr.FBold ? FontStyle.Bold : FontStyle.Regular) |
                                        (rr.FItalic ? FontStyle.Italic : FontStyle.Regular)
                                        );
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }

            rtb.Select(selStart, selLenght);
            TextEditorNS.WinAPI.SendMessage(rtb.Handle, TextEditorNS.WinAPI.WM_SETREDRAW, 1, IntPtr.Zero);
            rtb.Refresh();
        }

        /// <summary>
        /// Выбор контекстного меню: вызвать форму редактирования синтаксических правил
        /// </summary>
        private static void customCtxMenu_Syntax_Editor_Click(object sender, EventArgs e)
        {
            frmSyntaxBuilder frm = new frmSyntaxBuilder();
            if (VARS.main_form.paramTextEditor.txtBox.SelectedText.Length != 0)
                frm.rtb.Text = VARS.main_form.paramTextEditor.txtBox.SelectedText;
            frm.ShowDialog();
        }

        /// <summary>
        /// Выбор контекстного меню: встроенное базовое синтаксическое правило
        /// Стандартная подсветка описания команды
        /// </summary>
        private static void customCtxMenu_Syntax_Basic1_Click(object sender, EventArgs e)
        {
            RichTextBox rtb = VARS.main_form.paramTextEditor.txtBox;
            string text = rtb.SelectedText;
            int selectionStart = rtb.SelectionStart;
            int selectionLength = rtb.SelectionLength;

            rtb.SelectionColor = Color.Blue;

            // Подстановочное значение
            MatchCollection mm = Regex.Matches(text, @"<.*?>");
            foreach (Match m in mm)
            {
                rtb.Select(selectionStart + m.Index, m.Length);
                rtb.SelectionColor = Color.Magenta;
            }

            // Результат вывода на экран
            mm = Regex.Matches(text, @">>>.*?$", RegexOptions.Multiline);
            foreach (Match m in mm)
            {
                rtb.Select(selectionStart + m.Index, m.Length);
                rtb.SelectionColor = Color.Gray;
            }

            // комментарий #...
            mm = Regex.Matches(text, @"#.*?$", RegexOptions.Multiline);
            foreach (Match m in mm)
            {
                rtb.Select(selectionStart + m.Index, m.Length);
                rtb.SelectionColor = Color.Green;
            }

            // комментарий //...
            mm = Regex.Matches(text, @"//.*?$", RegexOptions.Multiline);
            foreach (Match m in mm)
            {
                rtb.Select(selectionStart + m.Index, m.Length);
                rtb.SelectionColor = Color.Green;
            }

            // комментарий /*...*/
            mm = Regex.Matches(text, @"\/\*.*?\*\/", RegexOptions.Singleline);
            foreach (Match m in mm)
            {
                rtb.Select(selectionStart + m.Index, m.Length);
                rtb.SelectionColor = Color.Green;
            }

            rtb.Select(selectionStart, selectionLength);
        }

        /// <summary>
        /// Удаляет все элементы контестного меню начинающиеся с определенного слова
        /// </summary>
        /// <param name="nameStartsFrom">Слово с которого начинается имя меню</param>
        /// <param name="ctx">Объект контекстного меню</param>
        private static void RemoveAllExtraContextMenus(string nameStartsFrom, ContextMenuStrip ctx)
        {
            ToolStripItem[] collection = new ToolStripItem[ctx.Items.Count];
            ctx.Items.CopyTo(collection, 0);

            foreach(var item in collection)
            {
                if (item.Name.StartsWith(nameStartsFrom))
                {
                    if (item.GetType() == typeof(ToolStripSeparator))
                    {
                        ctx.Items.Remove(((ToolStripSeparator)item));
                        continue;
                    }
                    else if (item.GetType() == typeof(ToolStripMenuItem))
                    {
                        if (((ToolStripMenuItem)item).DropDownItems.Count > 0)
                            RemoveAllExtraContextSubMenus(((ToolStripMenuItem)item), ctx);
                        ctx.Items.Remove(((ToolStripMenuItem)item));
                    }
                }
            }
        }

        /// <summary>
        /// Рекурсивно удаляет все подэлементы контекстного меню
        /// </summary>
        /// <param name="mnu">Элемент меню</param>
        /// <param name="ctx">Объект контекстного меню</param>
        private static void RemoveAllExtraContextSubMenus(ToolStripMenuItem mnu, ContextMenuStrip ctx)
        {
            ToolStripItem[] collection = new ToolStripItem[mnu.DropDownItems.Count];
            mnu.DropDownItems.CopyTo(collection, 0);

            foreach (var item in collection)
            {
                if (item.GetType() == typeof(ToolStripSeparator))
                {
                    ctx.Items.Remove(((ToolStripSeparator)item));
                    continue;
                }
                else if (item.GetType() == typeof(ToolStripMenuItem))
                {
                    if (((ToolStripMenuItem)item).DropDownItems.Count > 0) RemoveAllExtraContextSubMenus(((ToolStripMenuItem)item), ctx);
                    ctx.Items.Remove(item);
                }
            }
        }

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

        /// <summary>
        /// Сериализует объект в XML-текст
        /// </summary>
        /// <param name="obj">Объект для сериализации</param>
        /// <returns>XML-строка сериализованного класса</returns>
        public static string SerializeSyntaxRules(object obj)
        {
            string serializedRules = "";
            XmlSerializer formatter = new XmlSerializer(obj.GetType());
            using (StringWriter textWriter = new StringWriter())
            {
                formatter.Serialize(textWriter, obj);
                serializedRules = textWriter.ToString();
            }

            return serializedRules;
        }

        /// <summary>
        /// Десериализация объекта из XML-текста
        /// </summary>
        /// <param name="serializedRules">XML-строка сериализованного класса</param>
        /// <param name="obj">Объект для сериализации</param>
        public static void DeserializeSyntaxRules<T>(string serializedRules, ref T obj)
        {
            if (serializedRules == "")
            {
                obj = default(T);
                return;
            }

            XmlSerializer formatter = new XmlSerializer(obj.GetType());
            using (TextReader textReader = new StringReader(serializedRules))
            {
                obj = (T)formatter.Deserialize(textReader);
            }
        }
    }

    // Классы для синтаксических правил
    [Serializable]
    public class RuleRow
    {
        public int ParentId = 0;
        public int Order = 0;
        public string Name = "";
        public List<string> RuleElements = new List<string>();
        public int Color = 0;
        public bool SingleLine = false;
        public bool Case = false;
        public bool FBold = false;
        public bool FItalic = false;
        public bool Enabled = true;
    }

    [Serializable]
    public class SyntaxRule
    {
        public int id = 0;
        public string Name = "";
        public bool Enabled = true;
        public List<RuleRow> Rules = new List<RuleRow>();
    }

    [Serializable]
    public class SyntaxRulesHolder
    {
        public List<SyntaxRule> Rules = new List<SyntaxRule>();
    }


}
