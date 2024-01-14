using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace simple_database
{
    /// <summary>
    /// Класс для редактирования кода плагинов
    /// </summary>
    public partial class frmPluginEditor : Form
    {
        /// <summary>
        /// ID редактируемого оглавления плагина
        /// </summary>
        public long property_id;

        /// <summary>
        /// Блокировка события изменения текста
        /// </summary>
        private static bool lockUpdate = false;

        // Цветовые константы
        private readonly Color cComment = Color.FromArgb(87, 166, 74);
        private readonly Color cKeyword = Color.FromArgb(86, 156, 214);
        private readonly Color cString = Color.FromArgb(214, 157, 133);

        /// <summary>
        /// Конструктор
        /// </summary>
        public frmPluginEditor()
        {
            InitializeComponent();
            rtb.AcceptsTab = true;
            rtb.WordWrap = false;
            rtb.AutoWordSelection = false;
            lblCaretInfo.Text = $"Позиция 0    Строка 0    Столбец 0";
        }

        /// <summary>
        /// Загружает в редактор текст
        /// </summary>
        public void LoadText(string plugin_code)
        {
            rtb.Text = plugin_code;
        }

        /// <summary>
        /// Изменен текст в редакторе
        /// </summary>
        private void rtb_TextChanged(object sender, EventArgs e)
        {
            if (lockUpdate) return;

            if (btnSave.ImageKey == "Save-icon")
                btnSave.ImageKey = "exclamation";

            // вызовем подсветку синтаксиса для текущей строки, если были введены специальные символы
            int lastSymbolPos = rtb.SelectionStart - 1;

            if (lastSymbolPos >= 0)
            {
                char c = rtb.Text[lastSymbolPos];
                if (wordTerminators.Contains(c) || c == '"')
                {
                    
                    int start = 0;
                    int len = 0;
                    int line = rtb.GetLineFromCharIndex(rtb.SelectionStart);

                    // был перенос на другую строку, вычислим данные верхней строки
                    if (c == '\n')
                    {
                        start = rtb.GetFirstCharIndexFromLine(line - 1);
                        len = rtb.Lines[line - 1].Length;
                    }
                    else // вычислим данные текущей строки
                    {
                        start = rtb.GetFirstCharIndexOfCurrentLine();
                        len = rtb.Lines[line].Length;
                    }
                    
                   CheckSpelling(start, len, true);
                }
            }
        }

        /// <summary>
        /// Сохранить изменения
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            CheckSpelling(0, rtb.Text.Length);

            if (DATABASE.Plugin_Update(property_id, rtb.Text))
            {
                btnSave.ImageKey = "Save-icon";
                VARS.main_form.slblLastUpdate.Text = "Last update: " + DATABASE.SetLastUpdate();
            }
            else
            {
                MessageBox.Show("Не удалось сохранить данные.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Обработка нажатия клавиш
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rtb_KeyDown(object sender, KeyEventArgs e)
        {
            // обработка TAB
            if (e.KeyCode == Keys.Tab && !e.Control)
            {
                rtb.SelectedText = "    ";
                e.Handled = true;
                e.SuppressKeyPress = true;
                return;
            }

            // обработка CTRL-S
            if (e.KeyCode == Keys.S && e.Control)
            {
                btnSave.PerformClick();
                e.SuppressKeyPress = true;
                e.Handled = true;
                return;
            }
        }

        /// <summary>
        /// Действия при закрытии формы.
        /// </summary>
        private void frmPluginEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (btnSave.ImageKey == "exclamation")
            {
                DialogResult dr = MessageBox.Show("Имеются несохраненные данные. Сохранить?", "Предупреждение", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                if (dr == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
                else if (dr == DialogResult.No)
                {
                    e.Cancel = false;
                }
                else
                {
                    // сохраняем
                    btnSave.PerformClick();
                    e.Cancel = false;
                    Close();
                }
            }
        }

        /// <summary>
        /// Кнопка: Вставить пресет
        /// </summary>
        private void btnInsertPreset_Click(object sender, EventArgs e)
        {
            contextMenuStrip1.Show(Cursor.Position);
        }

        /// <summary>
        /// Контекстное меню. Вставить пресет: VALUE
        /// </summary>
        private void ctxVALUE_Click(object sender, EventArgs e)
        {
            int pos = rtb.SelectionStart;
            rtb.SelectedText = "{#VALUE  #}";
            rtb.SelectionStart = pos + 8;
            rtb.SelectionLength = 0;
            rtb.Focus();
        }

        /// <summary>
        /// Контекстное меню. Вставить пресет: ASK
        /// </summary>
        private void ctxASK_Click(object sender, EventArgs e)
        {
            int pos = rtb.SelectionStart;
            rtb.SelectedText = "{#ASK NAME=\"\" TYPE=\"\" TEXT=\"\" VALUE=\"\" #}";
            rtb.SelectionStart = pos + 12;
            rtb.SelectionLength = 0;
            rtb.Focus();
        }

        /// <summary>
        /// Контекстное меню. Вставить пресет: SET
        /// </summary>
        private void ctxSET_Click(object sender, EventArgs e)
        {
            int pos = rtb.SelectionStart;
            rtb.SelectedText =
                "{#SET = #}";
            rtb.SelectionStart = pos + 6;
            rtb.SelectionLength = 0;
            rtb.Focus();
        }

        /// <summary>
        /// Контекстное меню. Вставить шаблон: C# класс
        /// </summary>
        private void ctxTemplates_CSharpClass_Click(object sender, EventArgs e)
        {
            rtb.SelectedText =
                "/*" + Environment.NewLine +
                "{#SET TYPE=CLASS #}" + Environment.NewLine +
                "{#SET CLASS=Plugin #}" + Environment.NewLine +
                "{#SET RUN=Start #}" + Environment.NewLine +
                "{#SET REFERENCES=System.dll; System.Windows.Forms.dll #}" + Environment.NewLine +
                "" + Environment.NewLine +
                "{#SET ASKTITLE=Запрос данных #}" + Environment.NewLine +
                "{#ASK NAME=\"\" TYPE=\"Label\" TEXT=\"Пример запроса данных у пользователя\\n и принцип обработки чисел с плавающей точкой.\" VALUE=\"\" #}" + Environment.NewLine +
                "{#ASK NAME=\"message1\" TYPE=\"TextBox\" TEXT=\"Сообщение для вывода на экран:\" VALUE=\"Hello World!\" #}" + Environment.NewLine +
                "{#ASK NAME=\"числоСПлавающейТочкой\" TYPE=\"TextBox\" TEXT=\"Число с плавающей точкой:\" VALUE=\"0,4\" #}" + Environment.NewLine +
                "{#ASK NAME=\"приставка\" TYPE=\"ComboBox\" TEXT=\"Приставка\" VALUE=\"кило; мега; гига\" #}" + Environment.NewLine +
                "{#ASK NAME=\"btnOK\" TYPE=\"Button\" TEXT=\"OK\" VALUE=\"\" #}" + Environment.NewLine +
                "*/" + Environment.NewLine +
                "" + Environment.NewLine +
                "using System;" + Environment.NewLine +
                "using System.Globalization;" + Environment.NewLine +
                "using System.Windows.Forms;" + Environment.NewLine +
                "" + Environment.NewLine +
                "class Plugin" + Environment.NewLine +
                "{" + Environment.NewLine +
                "    public void Start()" + Environment.NewLine +
                "    {" + Environment.NewLine +
                "        double x;" + Environment.NewLine +
                "        double.TryParse(\"{#VALUE числоСПлавающейТочкой #}\".Replace(',','.'), NumberStyles.Any, CultureInfo.InvariantCulture, out x);" + Environment.NewLine +
                "        " + Environment.NewLine +
                "        if (\"pressed\" == \"{#VALUE btnOK #}\") {" + Environment.NewLine +
                "            MessageBox.Show(String.Format(\"Введенное число={0:f2} с приставкой: '{1}'\", x, \"{#VALUE приставка #}\"));" + Environment.NewLine +
                "            MessageBox.Show(\"{#VALUE message1 #}\");" + Environment.NewLine +
                "        } else {" + Environment.NewLine +
                "            MessageBox.Show(\"Button was not pressed\");" + Environment.NewLine +
                "        }" + Environment.NewLine +
                "    }" + Environment.NewLine +
                "}";
        }

        /// <summary>
        /// Контекстное меню. Вставить шаблон: C# консольное приложение
        /// </summary>
        private void ctxTemplates_CSharpConsoleApp_Click(object sender, EventArgs e)
        {
            rtb.SelectedText =
                "/*" + Environment.NewLine +
                "{#SET TYPE=APPLICATION #}" + Environment.NewLine +
                "{#SET REFERENCES=System.dll; #}" + Environment.NewLine +
                "" + Environment.NewLine +
                "{#ASK NAME=\"\" TYPE=\"Label\" TEXT=\"Вычисление суммы двух чисел\" VALUE=\"\" #}" + Environment.NewLine +
                "{#ASK NAME=\"valX\" TYPE=\"TextBox\" TEXT=\"X=\" VALUE=\"1\" #}" + Environment.NewLine +
                "{#ASK NAME=\"valY\" TYPE=\"TextBox\" TEXT=\"Y=\" VALUE=\"2\" #}" + Environment.NewLine +
                "{#ASK NAME=\"btnOK\" TYPE=\"Button\" TEXT=\"X+Y=?\" VALUE=\"\" #}" + Environment.NewLine +
                "*/" + Environment.NewLine +
                "" + Environment.NewLine +
                "using System;" + Environment.NewLine +
                "" + Environment.NewLine +
                "namespace plugins" + Environment.NewLine +
                "    {" + Environment.NewLine +
                "        class PluginSample" + Environment.NewLine +
                "        {" + Environment.NewLine +
                "            [STAThread]" + Environment.NewLine +
                "            static void Main(string[] args)" + Environment.NewLine +
                "            {" + Environment.NewLine +
                "                if (\"pressed\" != \"{#VALUE btnOK #}\") return;" + Environment.NewLine +
                "                int x = int.Parse(\"{#VALUE valX #}\");" + Environment.NewLine +
                "                int y = int.Parse(\"{#VALUE valY #}\");" + Environment.NewLine +
                "                int result = x + y;" + Environment.NewLine +
                "                Console.WriteLine(\"X+Y=\" + result.ToString());" + Environment.NewLine +
                "                Console.WriteLine(\"\");" + Environment.NewLine +
                "                Console.WriteLine(\"Press a key to exit...\");" + Environment.NewLine +
                "                Console.ReadKey(true);" + Environment.NewLine +
                "            }" + Environment.NewLine +
                "        }" + Environment.NewLine +
                "    }";
        }

        /// <summary>
        /// Запустить скрипт
        /// </summary>
        private void btnRun_Click(object sender, EventArgs e)
        {
            if (btnSave.ImageKey == "exclamation") btnSave.PerformClick();
            PluginsManager.StartPlugin(property_id);
        }

        /// <summary>
        /// Изменение положение курсора
        /// </summary>
        private void rtb_SelectionChanged(object sender, EventArgs e)
        {
            int pos = rtb.SelectionStart;
            int row = rtb.GetLineFromCharIndex(pos);
            int column = pos - rtb.GetFirstCharIndexFromLine(row);

            lblCaretInfo.Text = $"Позиция {1 + pos}    Строка {1 + row}    Столбец {1 + column}";
        }


        /// <summary>
        /// Подсветить синтаксис
        /// </summary>
        private void btnSpellingCheck_Click(object sender, EventArgs e)
        {
            CheckSpelling(0, rtb.Text.Length);
        }

        /// <summary>
        /// Функция подсветки синтаксиса
        /// </summary>
        /// <param name="startPos">Позиция с которой надо проверить</param>
        /// <param name="length">Длина проверяемого участка</param>
        /// <param name="partially">Показывает производится ли проверка частично</param>
        private void CheckSpelling(int startPos, int length, bool partially = false)
        {
            lockUpdate = true;

            // сохранение состояния
            int cursorPos = rtb.SelectionStart;
            string saveStatus = btnSave.ImageKey;

            // отключим вывод в RichTextBox
            TextEditorNS.WinAPI.SendMessage(rtb.Handle, TextEditorNS.WinAPI.WM_SETREDRAW, 0, IntPtr.Zero);

            // если это частичная проверка, то вначале сбросим цвет по умолчанию
            rtb.Select(startPos, length);
            rtb.SelectionColor = rtb.ForeColor;

            // внутренние переменные
            int endTextIndex = startPos + length - 1;
            char c = '\0';
            PBlocks pblocks = new PBlocks();
            PStart pstart = new PStart();

            for (int i = startPos; i < startPos + length; i++)
            {
                c = rtb.Text[i];

                // двойная кавычка
                if (c == '"')
                {
                    if (pstart.doubleQuote < 0)
                    {
                        pstart.doubleQuote = i; // начало строки
                    }
                    else
                    {
                        // продолжение строки
                        if (i > 0 && rtb.Text[i - 1] != '\\')
                        {
                            pblocks.doubleQuote.Add(new Point(pstart.doubleQuote, 1 + i - pstart.doubleQuote));
                            pstart.doubleQuote = -1;
                        }
                    }
                }

                // одинарная кавычка
                if (c == '\'')
                {
                    if (pstart.singleQuote < 0)
                    {
                        pstart.singleQuote = i; // начало строки
                    }
                    else
                    {
                        // продолжение строки
                        if (i > 0 && rtb.Text[i - 1] != '\\')
                        {
                            pblocks.singleQuote.Add(new Point(pstart.singleQuote, 1 + i - pstart.singleQuote));
                            pstart.singleQuote = -1;
                        }
                    }
                }

                // специальный блок
                if (c == '#')
                {
                    if (pstart.specialBlock < 0 && i > 0 && rtb.Text[i - 1] == '{')
                    {
                        pstart.specialBlock = i - 1; // начало
                    }
                    else if (i < endTextIndex && rtb.Text[i + 1] == '}')
                    {
                        pblocks.specialBlock.Add(new Point(pstart.specialBlock, 2 + i - pstart.specialBlock));
                        pstart.specialBlock = -1;
                        i++;
                    }
                }

                // слово
                if (pstart.word < 0 && char.IsLetter(c))
                {
                    pstart.word = i;
                }
                else if (pstart.word >= 0 && wordTerminators.Contains(c))
                {
                    if (keywords.Contains(rtb.Text.Substring(pstart.word, i - pstart.word)))
                    {
                        pblocks.word.Add(new Point(pstart.word, i - pstart.word));
                    }
                    pstart.word = -1;
                }
                else if (pstart.word >= 0 && i == endTextIndex)
                {
                    if (keywords.Contains(rtb.Text.Substring(pstart.word, 1 + i - pstart.word)))
                    {
                        pblocks.word.Add(new Point(pstart.word, 1 + i - pstart.word));
                    }
                    pstart.word = -1;
                }

                // одиночный комментарий
                if (c == '/')
                {
                    if (pstart.singleComment < 0 && i > 0 && rtb.Text[i - 1] == '/')
                    {
                        pstart.singleComment = i - 1; // начало
                    }
                }
                if (pstart.singleComment >= 0)
                {
                    if (c == '\n')
                    {
                        pblocks.singleComment.Add(new Point(pstart.singleComment, i - pstart.singleComment));
                        pstart.singleComment = -1;
                    }
                    else if (i == endTextIndex)
                    {
                        pblocks.singleComment.Add(new Point(pstart.singleComment, 1 + i - pstart.singleComment));
                        pstart.singleComment = -1;
                    }
                }

                // блочный комментарий
                if (c == '*')
                {
                    if (pstart.blockComment < 0 && i > 0 && rtb.Text[i - 1] == '/')
                    {
                        pstart.blockComment = i - 1; // начало
                    }
                    else if (i < endTextIndex && rtb.Text[i + 1] == '/')
                    {
                        pblocks.blockComment.Add(new Point(pstart.blockComment, 2 + i - pstart.blockComment));
                        pstart.blockComment = -1;
                        i++;
                    }
                }
            }

            // Раскрашивание найденных блоков в порядке применения очередности
            foreach (Point p in pblocks.word)
            {
                rtb.Select(p.X, p.Y);
                rtb.SelectionColor = cKeyword;
            }

            foreach (Point p in pblocks.doubleQuote)
            {
                rtb.Select(p.X, p.Y);
                rtb.SelectionColor = cString;
            }

            foreach (Point p in pblocks.singleQuote)
            {
                rtb.Select(p.X, p.Y);
                rtb.SelectionColor = cString;
            }

            foreach (Point p in pblocks.singleComment)
            {
                rtb.Select(p.X, p.Y);
                rtb.SelectionColor = Color.ForestGreen;
            }

            foreach (Point p in pblocks.blockComment)
            {
                rtb.Select(p.X, p.Y);
                rtb.SelectionColor = Color.ForestGreen;
            }

            foreach (Point p in pblocks.specialBlock)
            {
                rtb.Select(p.X, p.Y);
                rtb.SelectionColor = Color.Cyan;
            }

            // восстановление состояния
            rtb.Select(cursorPos, 0);
            rtb.SelectionColor = rtb.ForeColor;
            btnSave.ImageKey = saveStatus;
            rtb.Focus();

            // включим вывод в окно
            TextEditorNS.WinAPI.SendMessage(rtb.Handle, TextEditorNS.WinAPI.WM_SETREDRAW, 1, IntPtr.Zero);
            rtb.Invalidate();

            lockUpdate = false;
        }

        private class PStart
        {
            public int doubleQuote = -1;
            public int singleQuote = -1;
            public int specialBlock = -1;
            public int word = -1;
            public int singleComment = -1;
            public int blockComment = -1;
        }

        private class PBlocks
        {
            public List<Point> doubleQuote = new List<Point>();
            public List<Point> singleQuote = new List<Point>();
            public List<Point> specialBlock = new List<Point>();
            public List<Point> word = new List<Point>();
            public List<Point> singleComment = new List<Point>();
            public List<Point> blockComment = new List<Point>();
        }

        private readonly string[] keywords =
        {
            "using", "class", "namespace", "new",
            "int", "long", "bool", "float", "double", "single", "string", "String", "char", "void", "byte", "enum", "short",
            "true", "false", "null", "fixed", "object", "sizeof", "typeof", 
            "public", "private", "protected", "static", "const", "readonly",
            "for", "foreach", "do", "while", "break", "continue", "if", "else", "in", "out", "is", "ref",
            "try", "catch", "finally", "return", 
            "switch", "case", "default", "where", "this", "throw", 
        };

        private readonly char[] wordTerminators = { ' ', '.', ':', ';', '=', '/', ')', '{', '}', '(', '\r', '\n' };

        /// <summary>
        /// Действия после загрузки и отображения формы
        /// </summary>
        private void frmPluginEditor_Load(object sender, EventArgs e)
        {
            CheckSpelling(0, rtb.Text.Length);
        }

        /// <summary>
        /// Подсветка синтаксиса по нажатию ключевых клавиш
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rtb_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
    }
}
