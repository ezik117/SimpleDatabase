﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Text;
using System.Diagnostics;

namespace TextEditorNS
{
    public class TextEditor : Panel
    {
        /// <summary>
        /// Объект RichTextBox для прямого доступа.
        /// </summary>
        public RichTextBox txtBox;

        /// <summary>
        /// Флаг изменения текста в редакторе. Равен true, если текст был изменен.
        /// Должен быть установлен в false с внешней стороны.
        /// </summary>
        public bool textWasChanged;

        // Пользовательские функции
        public Action userAction1;
        public Action userAction2;
        public Action userAction3;

        /// <summary>
        /// Задает положение панели инструментов. По умолчанию панель наверху.
        /// </summary>
        public ToolBoxPosition ToolBoxPos
        {
            get
            {
                return _TextBoxPos;
            }
            set
            {
                _TextBoxPos = value;
                tsMenu.Dock = (value == ToolBoxPosition.OnTop ? DockStyle.Top : DockStyle.Bottom);
            }
        }
        private ToolBoxPosition _TextBoxPos;

        public delegate void ContentChanged(RichTextBox sender);
        /// <summary>
        /// Событие возникающее при любом изменении текста. Вызывается из основного SelectionChanged.
        /// </summary>
        public event ContentChanged OnContentChanged;

        /// <summary>
        /// Доступ к панели инструментов для добавления своих кнопок.
        /// </summary>
        public ToolStrip tsMenu;

        private ImageList Images;
        private Color textColor; // хранит выбранное пользователем значение цвета текста
        public Color textBackgroundColor; // хранит выбранное пользователем значение выделения текста

        // кнопки на панели
        private ToolStripButton btnTextFormatter; // кнопка "По образцу"
        private ToolStripButton btnBoldText; // кнопка "Жирный текст"
        private ToolStripButton btnItalicText; // кнопка "Наклоненный текст"
        private ToolStripButton btnUnderlineText; // кнопка "Подчеркнутый текст"
        private ToolStripButton btnTextProperties; // кнопка "Окно свойст текста"
        private ToolStripSplitButton btnTextColor; // кнопка "Цвет текста"
        private ToolStripSplitButton btnTextBgColor; // кнопка "Цвет выделения текста"
        private ToolStripComboBox btnFontFamily; // кнопка "Выбор фонта"
        private ToolStripComboBox btnFontSize;  // кнопка "Выбор размера текста"
        private ToolStripButton btnAlignLeft; // кнопка "Выравнивание по левому краю"
        private ToolStripButton btnAlignCenter; // кнопка "Выравнивание по центу"
        private ToolStripButton btnAlignRight; // кнопка "Выравнивание по правому краю"
        private ToolStripButton btnBulletList; // кнопка "Ненумерованный список"
        private ToolStripButton btnNumbersList; // кнопка "Нумерованный список"
        private ToolStripButton btnSubscript; // кнопка "Нижний регистр"
        private ToolStripButton btnSuperscript; // кнопка "Верхний регистр"
        private ToolStripDropDownButton btnTextCaps; // кнопка "Управление регистром букв"
        private ToolStripButton btnInsertPicture; // кнопка "Вставить картинку"
        private ToolStripButton btnFindText; // кнопка "Найти в тексте"

        /// <summary>
        /// Последняя операция. Массив, где первый элемент это Enum: LastOperations,
        /// а последующие элементы это параметры операции зависящие от ее типа
        /// </summary>
        public object[] LastOperation = null;

        /// <summary>
        /// Контекстное меню управления текстом
        /// </summary>
        private ContextMenuStrip cxm;

        /// <summary>
        /// Ссылка на панель выбора цвета
        /// </summary>
        private BackColorsPanel bgColorsPanel;

        /// <summary>
        /// Контейнер для хранения формата при копировании по образцу
        /// </summary>
        private FormatterData formatter;

        /// <summary>
        /// Используется для отключения сброса кнопок панели в начальное состояние
        /// </summary>
        private bool lockResetControls;

        /// <summary>
        /// Класс для работы с нумерованными списками и логическими строками
        /// </summary>
        private NumberedList nl;

        /// <summary>
        /// Конструктор. Создает объект для редактирования текста.
        /// </summary>
        /// <param name="parent">Родительский контрол.</param>
        /// <param name="images">Набор картинок для кнопок. Если равен null, то используется
        /// внутренняя коллекция.</param>
        public TextEditor(Control parent, ImageList images = null)
        {
            if (images == null)
            {
                TextEditorImages imagesConstructor = new TextEditorImages();
                Images = imagesConstructor.Images;
            }
            else
                Images = images;

            this.Parent = parent;
            this.Dock = DockStyle.Fill;
            this.BorderStyle = BorderStyle.FixedSingle;
            this.formatter.enabled = false;

            // --- создание компонентов ---

            txtBox = new RichTextBox();
            lockResetControls = false;
            txtBox.Parent = this;
            txtBox.Dock = DockStyle.Fill;
            txtBox.AcceptsTab = true;
            txtBox.HideSelection = false;
            txtBox.Multiline = true;
            txtBox.BorderStyle = BorderStyle.Fixed3D;
            txtBox.DetectUrls = true;
            txtBox.LinkClicked += TxtBox_LinkClicked;
            txtBox.AutoWordSelection = false;
            txtBox.TextChanged += TxtBox_TextChanged;
            txtBox.KeyDown += TxtBox_KeyDown;
            txtBox.MouseUp += TxtBox_MouseUp;
            txtBox.MouseDown += TxtBox_MouseDown;
            txtBox.SelectionChanged += TxtBox_SelectionChanged;

            textColor = Color.Black;
            textBackgroundColor = Color.Yellow;

            tsMenu = new ToolStrip
            {
                Parent = this,
                Dock = DockStyle.Top
            };
            ToolBoxPos = ToolBoxPosition.OnTop;
            tsMenu.ImageList = Images;

            // кнопка: формат по образцу
            btnTextFormatter = new ToolStripButton(Images.Images["brush"])
            {
                DoubleClickEnabled = true
            };
            btnTextFormatter.Click += BtnTextFormatter_Click;
            btnTextFormatter.DoubleClick += BtnTextFormatter_DoubleClick;
            tsMenu.Items.Add(btnTextFormatter);

            // кнопка: разделитель
            tsMenu.Items.Add(new ToolStripSeparator());

            // кнопка: жирный текст
            btnBoldText = new ToolStripButton(Images.Images["bold"]);
            btnBoldText.Click += btnBoldText_Click;
            tsMenu.Items.Add(btnBoldText);

            // кнопка: курсив
            btnItalicText = new ToolStripButton(Images.Images["italic"]);
            btnItalicText.Click += btnItalicText_Click;
            tsMenu.Items.Add(btnItalicText);

            // кнопка: подчеркнутый текст
            btnUnderlineText = new ToolStripButton(Images.Images["underline"]);
            btnUnderlineText.Click += btnUnderlineText_Click;
            tsMenu.Items.Add(btnUnderlineText);

            // кнопка: разделитель
            tsMenu.Items.Add(new ToolStripSeparator());

            // кнопка: цвет текста
            btnTextColor = new ToolStripSplitButton();
            btnTextColor.Paint += BtnTextColor_Paint;
            btnTextColor.AutoSize = false;
            btnTextColor.Width = 34;
            btnTextColor.Click += BtnTextColor_Click;
            btnTextColor.ButtonClick += BtnTextColor_ButtonClick;
            tsMenu.Items.Add(btnTextColor);

            // кнопка: цвет выделения текста
            btnTextBgColor = new ToolStripSplitButton
            {
                Name = "btnTextBgColor"
            };
            btnTextBgColor.Paint += BtnTextBgColor_Paint;
            btnTextBgColor.AutoSize = false;
            btnTextBgColor.Width = 34;
            btnTextBgColor.Click += BtnTextBgColor_Click; ;
            btnTextBgColor.ButtonClick += BtnTextBgColor_ButtonClick;
            tsMenu.Items.Add(btnTextBgColor);

            // панель с выбором цвета выделения
            bgColorsPanel = new BackColorsPanel(170, 1, this);

            // кнопка: разделитель
            tsMenu.Items.Add(new ToolStripSeparator());

            // кнопка: свойства текста
            btnTextProperties = new ToolStripButton(Images.Images["fontedit"]);
            btnTextProperties.Click += BtnTextProperties_Click;
            tsMenu.Items.Add(btnTextProperties);

            // кнопка: выбор шрифта 
            btnFontFamily = new ToolStripComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDown
            };
            InstalledFontCollection installedFontCollection = new InstalledFontCollection();
            foreach (FontFamily fntFamily in installedFontCollection.Families)
            {
                btnFontFamily.Items.Add(fntFamily.Name);
            }
            btnFontFamily.Text = txtBox.SelectionFont.FontFamily.Name;
            btnFontFamily.AutoSize = false;
            btnFontFamily.Size = new Size(110, btnFontFamily.Size.Height);
            btnFontFamily.SelectedIndexChanged += BtnFontFamily_SelectedIndexChanged;
            tsMenu.Items.Add(btnFontFamily);

            // кнопка: размер шрифрта
            btnFontSize = new ToolStripComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDown
            };
            btnFontSize.Items.AddRange(new object[] { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 });
            btnFontSize.SelectedIndex = 2;
            btnFontSize.AutoSize = false;
            btnFontSize.Size = new Size(50, btnFontSize.Size.Height);
            btnFontSize.SelectedIndexChanged += BtnFontSize_SelectedIndexChanged;
            btnFontSize.KeyDown += BtnFontSize_KeyDown;
            tsMenu.Items.Add(btnFontSize);

            // кнопка: разделитель
            tsMenu.Items.Add(new ToolStripSeparator());

            // кнопка: выравнивание по левому краю
            btnAlignLeft = new ToolStripButton(Images.Images["alignleft"]);
            btnAlignLeft.Click += BtnAlignLeft_Click;
            tsMenu.Items.Add(btnAlignLeft);

            // кнопка: выравнивание по центру
            btnAlignCenter = new ToolStripButton(Images.Images["aligncenter"]);
            btnAlignCenter.Click += BtnAlignCenter_Click;
            tsMenu.Items.Add(btnAlignCenter);

            // кнопка: выравнивание по правому краю
            btnAlignRight = new ToolStripButton(Images.Images["alignright"]);
            btnAlignRight.Click += BtnAlignRight_Click;
            tsMenu.Items.Add(btnAlignRight);

            // кнопка: разделитель
            tsMenu.Items.Add(new ToolStripSeparator());

            // кнопка: ненумерованный список
            btnBulletList = new ToolStripButton(Images.Images["bulletlist"]);
            btnBulletList.Click += BtnBulletList_Click;
            tsMenu.Items.Add(btnBulletList);

            // кнопка: нумерованный список
            btnNumbersList = new ToolStripButton(Images.Images["numberslist"]);
            btnNumbersList.Click += btnNumbersList_Click;
            tsMenu.Items.Add(btnNumbersList);

            // кнопка: подстрочный текст
            btnSubscript = new ToolStripButton(Images.Images["subscript"]);
            btnSubscript.Click += BtnSubscript_Click;
            tsMenu.Items.Add(btnSubscript);

            // кнопка: надстрочный текст
            btnSuperscript = new ToolStripButton(Images.Images["superscript"]);
            btnSuperscript.Click += BtnSuperscript_Click;
            tsMenu.Items.Add(btnSuperscript);

            // кнопка: изменение стиля текста
            btnTextCaps = new ToolStripDropDownButton
            {
                DisplayStyle = ToolStripItemDisplayStyle.Image,
                Image = Images.Images["caps"]
            };
            ToolStripMenuItem mnuTextToSentence = new ToolStripMenuItem("Как в предложениях.");
            mnuTextToSentence.Click += MnuTextToSentence_Click;
            ToolStripMenuItem mnuTextToUpper = new ToolStripMenuItem("ВСЕ ЗАГЛАВНЫЕ");
            mnuTextToUpper.Click += MnuTextToUpper_Click;
            ToolStripMenuItem mnuTextToLower = new ToolStripMenuItem("все прописные");
            mnuTextToLower.Click += MnuTextToLower_Click;
            ToolStripMenuItem mnuTextChangeCaps = new ToolStripMenuItem("иЗМЕНИТЬ РЕГИСТР");
            mnuTextChangeCaps.Click += MnuTextChangeCaps_Click;
            ToolStripMenuItem mnuTextChangeZebra = new ToolStripMenuItem("Каждое Слово С Большой Буквы");
            mnuTextChangeZebra.Click += MnuTextChangeZebra_Click;
            btnTextCaps.DropDownItems.Add(mnuTextToSentence);
            btnTextCaps.DropDownItems.Add(mnuTextToUpper);
            btnTextCaps.DropDownItems.Add(mnuTextToLower);
            btnTextCaps.DropDownItems.Add(mnuTextChangeCaps);
            btnTextCaps.DropDownItems.Add(mnuTextChangeZebra);
            tsMenu.Items.Add(btnTextCaps);

            // кнопка: вставить изображение
            btnInsertPicture = new ToolStripButton(Images.Images["picture"]);
            btnInsertPicture.Click += BtnInsertPicture_Click;
            tsMenu.Items.Add(btnInsertPicture);

            // кнопка: найти текст в текущем окне
            btnFindText = new ToolStripButton(Images.Images["find"]);
            btnFindText.Click += BtnFindText_Click;
            tsMenu.Items.Add(btnFindText);

            // контекстное меню
            cxm = new ContextMenuStrip();
            ToolStripMenuItem mnuItemUndo = new ToolStripMenuItem("Отменить");
            mnuItemUndo.Click += MnuItem_Undo_Click;
            cxm.Items.Add(mnuItemUndo);
            cxm.Items.Add(new ToolStripSeparator());
            ToolStripMenuItem mnuItemCut = new ToolStripMenuItem("Вырезать");
            mnuItemCut.Click += MnuItem_Cut_Click;
            cxm.Items.Add(mnuItemCut);
            ToolStripMenuItem mnuItemCopy = new ToolStripMenuItem("Копирование");
            cxm.Items.Add(mnuItemCopy);
            ToolStripMenuItem mnuItemCopy_Copy = new ToolStripMenuItem("Копировать (CTRL-C)");
            mnuItemCopy_Copy.Click += MnuItem_Copy_Copy_Click;
            mnuItemCopy.DropDownItems.Add(mnuItemCopy_Copy);
            ToolStripMenuItem mnuItemCopy_Text = new ToolStripMenuItem("Копировать как текст");
            mnuItemCopy_Text.Click += MnuItem_Copy_Text_Click;
            mnuItemCopy.DropDownItems.Add(mnuItemCopy_Text);
            ToolStripMenuItem mnuItemCopy_Rtf = new ToolStripMenuItem("Копировать как RTF");
            mnuItemCopy_Rtf.Click += MnuItem_Copy_Rtf_Click;
            mnuItemCopy.DropDownItems.Add(mnuItemCopy_Rtf);
            ToolStripMenuItem mnuItemPaste = new ToolStripMenuItem("Вставка");
            cxm.Items.Add(mnuItemPaste);
            ToolStripMenuItem mnuItemPaste_Paste = new ToolStripMenuItem("Вставить (CTRL-V)");
            mnuItemPaste_Paste.Click += MnuItem_Paste_Paste_Click;
            mnuItemPaste.DropDownItems.Add(mnuItemPaste_Paste);
            ToolStripMenuItem mnuItemPaste_Paste_Text = new ToolStripMenuItem("Вставить как текст");
            mnuItemPaste_Paste_Text.Click += MnuItem_Paste_Text_Click;
            mnuItemPaste.DropDownItems.Add(mnuItemPaste_Paste_Text);
            ToolStripMenuItem mnuItemDelete = new ToolStripMenuItem("Удалить");
            mnuItemDelete.Click += MnuItem_Delete_Click;
            cxm.Items.Add(mnuItemDelete);
            cxm.Items.Add(new ToolStripSeparator());
            ToolStripMenuItem mnuItemSelectAll = new ToolStripMenuItem("Выделить все");
            mnuItemSelectAll.Click += MnuItem_SelectAll_Click;
            cxm.Items.Add(mnuItemSelectAll);

            txtBox.ContextMenuStrip = cxm;

            nl = new NumberedList(txtBox);

            textWasChanged = false;
            this.Clear();
        }

        /// <summary>
        /// Изменилась позиция курсора
        /// </summary>
        private void TxtBox_SelectionChanged(object sender, EventArgs e)
        {
            ShowSelectionProperties();
        }

        /// <summary>
        /// Нажата гиперссылка. Осуществим переход
        /// </summary>
        private void TxtBox_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            Process.Start(e.LinkText);
        }

        /// <summary>
        /// Контекстное меню. Отменить последнюю операцию.
        /// </summary>
        private void MnuItem_Undo_Click(object sender, EventArgs e)
        {
            txtBox.Undo();
        }

        /// <summary>
        /// Контекстное меню. Вырезать.
        /// </summary>
        private void MnuItem_Cut_Click(object sender, EventArgs e)
        {
            //var clipData = new DataObject();
            //clipData.SetText(txtBox.SelectedRtf, TextDataFormat.Rtf);
            //clipData.SetText(txtBox.SelectedText.Replace("\n", "\r\n"), TextDataFormat.Text);
            //Clipboard.SetDataObject(clipData);

            txtBox.Copy();

            txtBox.SelectedText = "";
        }

        /// <summary>
        /// Контекстное меню. Копировать.
        /// </summary>
        private void MnuItem_Copy_Copy_Click(object sender, EventArgs e)
        {
            if (txtBox.SelectedText.Length != 0) txtBox.Copy();
        }

        /// <summary>
        /// Контекстное меню. Копировать как текст.
        /// </summary>
        private void MnuItem_Copy_Text_Click(object sender, EventArgs e)
        {
            if (txtBox.SelectedText.Length != 0)
            {
                Clipboard.SetText(txtBox.SelectedText, TextDataFormat.Text);
            }
        }

        /// <summary>
        /// Контекстное меню. Копировать как RTF.
        /// </summary>
        private void MnuItem_Copy_Rtf_Click(object sender, EventArgs e)
        {
            if (txtBox.SelectedText.Length != 0)
            {
                Clipboard.SetText(txtBox.SelectedRtf, TextDataFormat.Rtf);
            }
        }

        /// <summary>
        /// Контекстное меню. Вставить.
        /// </summary>
        private void MnuItem_Paste_Paste_Click(object sender, EventArgs e)
        {
            txtBox.Paste();
        }

        /// <summary>
        /// Контекстное меню. Вставить как текст.
        /// </summary>
        private void MnuItem_Paste_Text_Click(object sender, EventArgs e)
        {
            txtBox.SelectedText = Clipboard.GetText(TextDataFormat.Text);
        }

        /// <summary>
        /// Контекстное меню. Удалить.
        /// </summary>
        private void MnuItem_Delete_Click(object sender, EventArgs e)
        {
            txtBox.SelectedText = "";
        }

        /// <summary>
        /// Контекстное меню. Выделить все.
        /// </summary>
        private void MnuItem_SelectAll_Click(object sender, EventArgs e)
        {
            txtBox.SelectAll();
        }

        /// <summary>
        /// Очищает текст в RichTextBox и устанавливает шрифт по умолчанию.
        /// Если шрифт не задан, то будет произведен сброс со шрифтом LucidaConsole, 10.
        /// Не вызвывайте свойство txtBox.Clear() напрямую!
        /// </summary>
        /// <param name="setFont">Шрифт по умолчанию.</param>
        public void Clear(Font setFont = null)
        {
            txtBox.Rtf = "";
            txtBox.Text = "";
            txtBox.SelectAll();
            if (setFont == null) setFont = new Font("Lucida Console", 10F, FontStyle.Regular);
            txtBox.SelectionFont = txtBox.Font = setFont;

            ShowSelectionProperties();
        }

        /// <summary>
        /// Пользователь изменил текст
        /// </summary>
        private void TxtBox_TextChanged(object sender, EventArgs e)
        {
            ShowSelectionProperties();

            textWasChanged = true;
            OnContentChanged?.Invoke(txtBox);
        }

        /// <summary>
        /// Обработка нажатий клавиш: горячих и обычных.
        /// </summary>
        private void TxtBox_KeyDown(object sender, KeyEventArgs e)
        {
            ShowSelectionProperties();

            if (e.Control)
            {
                switch (e.KeyCode)
                {
                    // Жирный шрифт
                    case Keys.B:
                        btnBoldText.PerformClick();
                        e.SuppressKeyPress = true;
                        break;
                    // Наклоненный шрифт
                    case Keys.I:
                        btnItalicText.PerformClick();
                        e.SuppressKeyPress = true;
                        break;
                    // Подчеркнутый шрифт
                    case Keys.U:
                        btnUnderlineText.PerformClick();
                        e.SuppressKeyPress = true;
                        break;
                    // Выравнивание текста по центру
                    case Keys.E:
                        btnAlignCenter.PerformClick();
                        e.SuppressKeyPress = true;
                        break;
                    // Выравнивание текста по левому краю
                    case Keys.L:
                        btnAlignLeft.PerformClick();
                        e.SuppressKeyPress = true;
                        break;
                    // Выравнивание текста по правому краю
                    case Keys.R:
                        btnAlignRight.PerformClick();
                        e.SuppressKeyPress = true;
                        break;
                    // Поиск текста
                    case Keys.F:
                        btnFindText.PerformClick();
                        e.SuppressKeyPress = true;
                        break;
                }
            }
            else if (e.Shift && e.KeyCode == Keys.Tab)
            {
                // нажатие SHIFT-TAB, убираем табуляцию слева на один знак табуляции или 4 пробела
                // если для табуляции были использованы пробелы
                int[,] linesInfo = GetLineInfo(txtBox.Text);
                int startLine = GetLineOfPosition(ref linesInfo, txtBox.SelectionStart);
                int endLine = GetLineOfPosition(ref linesInfo, txtBox.SelectionStart + txtBox.SelectionLength);

                for (int i = startLine; i <= endLine; i++)
                {
                    if (txtBox.Text[linesInfo[i, 0]] == '\t')
                    {
                        int cPos = linesInfo[i, 0];
                        txtBox.Select(cPos, 1);
                        txtBox.SelectedText = "";
                        linesInfo = GetLineInfo(txtBox.Text);
                    }
                    else if (txtBox.Text[linesInfo[i, 0]] == ' ')
                    {
                        int cPos = linesInfo[i, 0];
                        int j = 0;
                        while (txtBox.Text[j + linesInfo[i, 0]] == ' ' && j < 4) j++;
                        txtBox.Select(cPos, j);
                        txtBox.SelectedText = "";
                        linesInfo = GetLineInfo(txtBox.Text);
                    }
                }

                int cPos1 = linesInfo[startLine, 0];
                int cPos2 = linesInfo[endLine, 0] + linesInfo[endLine, 1] - 1;
                txtBox.Select(cPos1, cPos2 - cPos1);

                e.SuppressKeyPress = true;
                e.Handled = true;
            }
            else if (e.Modifiers == Keys.None && e.KeyCode == Keys.Tab)
            {
                // нажатие TAB. Добавляем табуляцию слева как символ табуляции \t
                // но только если было выбрано больше одной строки
                int[,] linesInfo = GetLineInfo(txtBox.Text);
                int startLine = GetLineOfPosition(ref linesInfo, txtBox.SelectionStart);
                int endLine = GetLineOfPosition(ref linesInfo, txtBox.SelectionStart + txtBox.SelectionLength);

                if (startLine != endLine)
                {
                    for (int i = startLine; i <= endLine; i++)
                    {
                        txtBox.Select(linesInfo[i, 0], 0);
                        txtBox.SelectedText = "\t";
                        linesInfo = GetLineInfo(txtBox.Text);
                    }

                    int cPos1 = linesInfo[startLine, 0];
                    int cPos2 = linesInfo[endLine, 0] + linesInfo[endLine, 1] - 1;
                    txtBox.Select(cPos1, cPos2 - cPos1);

                    e.SuppressKeyPress = true;
                    e.Handled = true;
                }
            }
            else if (e.KeyCode == Keys.F4)
            {
                // Нажата F4, повторим последнюю операцию форматирования
                RepeatLastOperation();

                e.SuppressKeyPress = true;
                e.Handled = true;
            }
            else if (e.Modifiers == Keys.None && e.KeyCode == Keys.Enter)
            {
                // Отключим вывод в окно
                WinAPI.SendMessage(txtBox.Handle, WinAPI.WM_SETREDRAW, 0, IntPtr.Zero);

                // Нажата клавиша ENTER. Проверим не нумерованный ли список?
                LineInfo li = nl.GetLineInfoFromCursorPosition();
                if (li.LineNo < txtBox.Lines.Length)
                {
                    NumberedListInfo nli = nl.GetNumberedList(li.LineNo);

                    if (nli.Number >= 0 && li.Length != nli.ListLength)
                    {
                        // если Enter нажата на нумерованной строке с контентом
                        // - следующая строка тоже будет нумерованной
                        txtBox.SelectedText = Environment.NewLine;
                        txtBox.SelectedText = $"{1 + nli.Number}. ";
                        int lastCursorPosition = txtBox.SelectionStart;

                        // перенумеруем все оставшиеся ниже строки, если они являются частью нумерованного списка
                        bool endOfNumberedList = false;
                        int currentLine = li.LineNo + 2;
                        int currentNumber = 2 + nli.Number;
                        while (!endOfNumberedList)
                        {
                            nli = nl.GetNumberedList(currentLine);
                            if (nli.Number >= 0)
                            {
                                txtBox.SelectionStart = nli.StartIndex;
                                txtBox.SelectionLength = nli.NumberLength;
                                txtBox.SelectedText = $"{currentNumber}";
                                currentLine++;
                                currentNumber++;
                            }
                            else
                            {
                                endOfNumberedList = true;
                            }
                        }
                        txtBox.SelectionStart = lastCursorPosition;
                        txtBox.SelectionLength = 0;
                    }
                    else if (nli.Number >= 0 && li.Length == nli.ListLength)
                    {
                        // если Enter нажата на нумерованной строке без контента -
                        // не переносим строчку, а убираем номер в текущей
                        txtBox.SelectionStart = li.StartIndex;
                        txtBox.SelectionLength = li.Length;
                        txtBox.SelectedText = "";
                        int lastCursorPosition = txtBox.SelectionStart;

                        // перенумеруем все оставшиеся ниже строки, если они являются частью нумерованного списка
                        bool endOfNumberedList = false;
                        int currentLine = li.LineNo + 1;
                        int currentNumber = nli.Number;
                        while (!endOfNumberedList)
                        {
                            nli = nl.GetNumberedList(currentLine);
                            if (nli.Number >= 0)
                            {
                                txtBox.SelectionStart = nli.StartIndex;
                                txtBox.SelectionLength = nli.NumberLength;
                                txtBox.SelectedText = $"{currentNumber}";
                                currentLine++;
                                currentNumber++;
                            }
                            else
                            {
                                endOfNumberedList = true;
                            }
                        }
                        txtBox.SelectionStart = lastCursorPosition;
                        txtBox.SelectionLength = 0;
                    }
                    else
                    {
                        txtBox.SelectedText = Environment.NewLine;
                    }
                    
                    e.Handled = true;
                }

                // Включим обратно вывод в окно
                WinAPI.SendMessage(txtBox.Handle, WinAPI.WM_SETREDRAW, 1, IntPtr.Zero);
                txtBox.Refresh();
            }
        }

        /// <summary>
        /// Повторить последнюю операцию, если она была
        /// </summary>
        private void RepeatLastOperation()
        {
            if (LastOperation == null) return;

            switch ((LastOperations)LastOperation[0])
            {
                case LastOperations.SetTextColor:
                    txtBox.SelectionColor = (Color)LastOperation[1];
                    break;
                case LastOperations.SetBgColor:
                    txtBox.SelectionBackColor = (Color)LastOperation[1];
                    break;
                case LastOperations.SetTextBold:
                    btnBoldText.PerformClick();
                    break;
                case LastOperations.SetTextItalic:
                    btnItalicText.PerformClick();
                    break;
                case LastOperations.SetTextUnderline:
                    btnUnderlineText.PerformClick();
                    break;
            }
        }

        /// <summary>
        /// Возвращает массив информации о линиях в тексте (линия это текст разделенный \r\n)
        /// </summary>
        /// <param name="text">Текст (в формате текста)</param>
        /// <returns>Массив вида [Index, Length], где Index-это начало строки, Length - это длина строки</returns>
        private int[,] GetLineInfo(string text)
        {
            MatchCollection mm = Regex.Matches(text, ".*\n|.+$");
            int[,] linesInfo = new int[mm.Count, 2];
            for (int i = 0; i < mm.Count; i++)
            {
                linesInfo[i, 0] = mm[i].Index;
                linesInfo[i, 1] = mm[i].Length;
            }
            linesInfo[mm.Count - 1, 1]++;

            return linesInfo;
        }

        /// <summary>
        /// Возвращает номер строки для указанной позиции символа
        /// </summary>
        /// <param name="linesInfo">Массив информации о строках. Получается функцией GetLineInfo()</param>
        /// <param name="charPosition">Текущая позиция курсора в тексте</param>
        /// <returns>Номер строки в переменной linesInfo, начиная с нуля</returns>
        private int GetLineOfPosition(ref int[,] linesInfo, int charPosition)
        {
            for (int i = 0; i <= linesInfo.GetUpperBound(0); i++)
            {
                if (linesInfo[i, 0] <= charPosition &&
                    charPosition < linesInfo[i, 0] + linesInfo[i, 1]) return i;
            }
            return -1;
        }

        /// <summary>
        /// Восстановить формат текста
        /// </summary>
        private void TxtBox_MouseUp(object sender, MouseEventArgs e)
        {
            ShowSelectionProperties();

            if (formatter.enabled)
            {
                //btnTextFormatter.Checked = formatter.enabled = false;
                txtBox.SelectionFont = formatter.font;
                txtBox.SelectionColor = formatter.txtColor;
                txtBox.SelectionBackColor = formatter.bgColor;
                txtBox.SelectionBullet = formatter.bullet;
                txtBox.BulletIndent = formatter.indent;
                txtBox.SelectionAlignment = formatter.txtAlign;
            }

            ResetControls();
        }

        /// <summary>
        /// Нажата любая клавиша мыши.
        /// </summary>
        private void TxtBox_MouseDown(object sender, MouseEventArgs e)
        {
            //ShowSelectionProperties();
        }

        /// <summary>
        /// Сброс всяких состояний кнопок и т.п.
        /// </summary>
        /// <param name="excludeFormatter">Если true, то сброс не затрагивает форматтер</param>
        private void ResetControls(bool excludeFormatter=false)
        {
            if (lockResetControls) return;

            bgColorsPanel.Showed = false;

            if (!excludeFormatter)
            {
                if (!formatter.persistent)
                    btnTextFormatter.Checked = formatter.enabled = false;
            }
        }

        /// <summary>
        /// Изменена позиция каретки. Отобразим изменения на панели инструментов.
        /// </summary>
        private void ShowSelectionProperties()
        {
            lockResetControls = true;

            // отследим показания кнопок стиля в зависимости от текста под курсором
            if (txtBox.SelectionFont != null)
            {
                btnFontSize.Text = txtBox.SelectionFont.Size.ToString();
                btnFontFamily.Text = txtBox.SelectionFont.Name;
                btnBoldText.Checked = txtBox.SelectionFont.Bold;
                btnItalicText.Checked = txtBox.SelectionFont.Italic;
                btnUnderlineText.Checked = txtBox.SelectionFont.Underline;
            }
            else
            {
                btnFontSize.Text = "";
                btnFontFamily.Text = "";
                btnBoldText.Checked = false;
                btnItalicText.Checked = false;
                btnUnderlineText.Checked = false;
            }

            // отследим показания других кнопок в зависимости от текста под курсором
            btnBulletList.Checked = txtBox.SelectionBullet;
            btnSubscript.Checked = txtBox.SelectionCharOffset < 0;
            btnSuperscript.Checked = txtBox.SelectionCharOffset > 0;
            btnAlignLeft.Checked = txtBox.SelectionAlignment == HorizontalAlignment.Left;
            btnAlignCenter.Checked = txtBox.SelectionAlignment == HorizontalAlignment.Center;
            btnAlignRight.Checked = txtBox.SelectionAlignment == HorizontalAlignment.Right;

            // отследим показания кнопки нумерованного стиля
            if (txtBox.Lines.Length > 0)
            {
                btnNumbersList.Checked = nl.IsTheTextNumberedInCurrentPosition();
            }

            bgColorsPanel.Showed = false;

            lockResetControls = false;
        }


        /// <summary>
        /// Выбрано выравнивание текста по правому краю
        /// </summary>
        private void BtnAlignRight_Click(object sender, EventArgs e)
        {
            ResetControls();

            txtBox.SelectionAlignment = HorizontalAlignment.Right;

            ShowSelectionProperties();
            txtBox.Focus();
        }

        /// <summary>
        /// Выбрано выравнивание текста по левому краю
        /// </summary>
        private void BtnAlignLeft_Click(object sender, EventArgs e)
        {
            ResetControls();

            txtBox.SelectionAlignment = HorizontalAlignment.Left;

            ShowSelectionProperties();
            txtBox.Focus();
        }

        /// <summary>
        /// Выбрано выравнивание текста по центру
        /// </summary>
        private void BtnAlignCenter_Click(object sender, EventArgs e)
        {
            ResetControls();

            txtBox.SelectionAlignment = HorizontalAlignment.Center;

            ShowSelectionProperties();
            txtBox.Focus();
        }

        /// <summary>
        /// Выбрана опция вставки изображения
        /// </summary>
        private void BtnInsertPicture_Click(object sender, EventArgs e)
        {
            ResetControls();

            OpenFileDialog dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                Image im = Image.FromFile(dlg.FileName);
                Clipboard.SetImage(im);
                txtBox.Paste();
            }

            txtBox.Focus();

            ShowSelectionProperties();
            txtBox.Focus();
        }

        /// <summary>
        /// Меню: Поиск текста.
        /// </summary>
        private void BtnFindText_Click(object sender, EventArgs e)
        {
            frmSearch frm = new frmSearch(txtBox);
            frm.ShowDialog();
        }

        // Сделать текст как в предложениях
        private void MnuTextToSentence_Click(object sender, EventArgs e)
        {
            ResetControls();

            // Отключим вывод в окно
            WinAPI.SendMessage(txtBox.Handle, WinAPI.WM_SETREDRAW, 0, IntPtr.Zero);

            int start = txtBox.SelectionStart;
            int len = txtBox.SelectionLength;

            foreach (Match m in Regex.Matches(txtBox.SelectedText, @"(\S.+?[.!?])|(\S.+)", RegexOptions.Multiline))
            {
                txtBox.Select(start + m.Index, 1);
                txtBox.SelectedText = txtBox.SelectedText.ToUpper();
            }

            txtBox.Select(start, len);

            // Включим вывод в окно
            WinAPI.SendMessage(txtBox.Handle, WinAPI.WM_SETREDRAW, 1, IntPtr.Zero);
            txtBox.Refresh();

            ShowSelectionProperties();
            txtBox.Focus();
        }

        /// <summary>
        /// Сделать каждую первую букву в выделенном тексте заглавной
        /// </summary>
        private void MnuTextChangeZebra_Click(object sender, EventArgs e)
        {
            ResetControls();

            // Отключим вывод в окно
            WinAPI.SendMessage(txtBox.Handle, WinAPI.WM_SETREDRAW, 0, IntPtr.Zero);

            int start = txtBox.SelectionStart;
            int len = txtBox.SelectionLength;

            foreach (Match m in Regex.Matches(txtBox.SelectedText, @"\S+", RegexOptions.Multiline))
            {
                txtBox.Select(start + m.Index, m.Length);
                StringBuilder txt = new StringBuilder(txtBox.SelectedText.ToLower());
                txt[0] = txt[0].ToString().ToUpper()[0];
                txtBox.SelectedText = txt.ToString();
            }

            txtBox.Select(start, len);

            // Включим вывод в окно
            WinAPI.SendMessage(txtBox.Handle, WinAPI.WM_SETREDRAW, 1, IntPtr.Zero);
            txtBox.Refresh();

            ShowSelectionProperties();
            txtBox.Focus();
        }

        /// <summary>
        /// Все буквы к прописным
        /// </summary>
        private void MnuTextToLower_Click(object sender, EventArgs e)
        {
            ResetControls();

            int start = txtBox.SelectionStart;
            int len = txtBox.SelectionLength;

            txtBox.SelectedText = txtBox.SelectedText.ToLower();
            txtBox.Select(start, len);

            ShowSelectionProperties();
            txtBox.Focus();
        }

        /// <summary>
        /// Все буквы к заглавным
        /// </summary>
        private void MnuTextToUpper_Click(object sender, EventArgs e)
        {
            ResetControls();

            int start = txtBox.SelectionStart;
            int len = txtBox.SelectionLength;

            txtBox.SelectedText = txtBox.SelectedText.ToUpper();
            txtBox.Select(start, len);

            ShowSelectionProperties();
            txtBox.Focus();
        }

        // Верхний регистр текста
        private void BtnSuperscript_Click(object sender, EventArgs e)
        {
            ResetControls();

            // restore size
            float newSize = txtBox.SelectionFont.Size;
            if (txtBox.SelectionCharOffset != 0) newSize /= 0.8F;

            if (!btnSuperscript.Checked)
            {
                Font fnt = new Font(txtBox.SelectionFont.FontFamily,
                                    newSize * 0.8F,
                                    txtBox.SelectionFont.Style);
                txtBox.SelectionFont = fnt;
                txtBox.SelectionCharOffset = 3;
            }
            else
            {
                Font fnt = new Font(txtBox.SelectionFont.FontFamily,
                                    newSize,
                                    txtBox.SelectionFont.Style);
                txtBox.SelectionFont = fnt;
                txtBox.SelectionCharOffset = 0;
            }

            ShowSelectionProperties();
            txtBox.Focus();
        }

        private void MnuTextChangeCaps_Click(object sender, EventArgs e)
        {
            ResetControls();

            // Отключим вывод в окно
            WinAPI.SendMessage(txtBox.Handle, WinAPI.WM_SETREDRAW, 0, IntPtr.Zero);

            int start = txtBox.SelectionStart;
            int len = txtBox.SelectionLength;
            int end = txtBox.SelectionStart + txtBox.SelectionLength;

            for (int i = txtBox.SelectionStart; i < end; i++)
            {
                txtBox.Select(i, 1);
                if (char.IsUpper(txtBox.SelectedText[0]))
                {
                    txtBox.SelectedText = txtBox.SelectedText.ToLower();
                }
                else
                {
                    txtBox.SelectedText = txtBox.SelectedText.ToUpper();
                }
            }

            txtBox.Select(start, len);

            // Включим вывод в окно
            WinAPI.SendMessage(txtBox.Handle, WinAPI.WM_SETREDRAW, 1, IntPtr.Zero);
            txtBox.Refresh();

            ShowSelectionProperties();
            txtBox.Focus();
        }

        // Нижний регистр текста
        private void BtnSubscript_Click(object sender, EventArgs e)
        {
            ResetControls();

            // restore size
            float newSize = txtBox.SelectionFont.Size;
            if (txtBox.SelectionCharOffset != 0) newSize /= 0.8F;

            if (!btnSubscript.Checked)
            {
                Font fnt = new Font(txtBox.SelectionFont.FontFamily,
                                    newSize * 0.8F,
                                    txtBox.SelectionFont.Style);
                txtBox.SelectionFont = fnt;
                txtBox.SelectionCharOffset = -3;
            }
            else
            {
                Font fnt = new Font(txtBox.SelectionFont.FontFamily,
                                    newSize,
                                    txtBox.SelectionFont.Style);
                txtBox.SelectionFont = fnt;
                txtBox.SelectionCharOffset = 0;
            }

            ShowSelectionProperties();
            txtBox.Focus();
        }

        /// <summary>
        /// Нажата клавиша ненумерованного списка
        /// </summary>
        private void BtnBulletList_Click(object sender, EventArgs e)
        {
            ResetControls();

            txtBox.SelectionBullet = !txtBox.SelectionBullet;
            btnBulletList.Checked = txtBox.SelectionBullet;

            ShowSelectionProperties();
            txtBox.Focus();
        }

        
        /// <summary>
        /// Нажата клавиша нумерованного списка
        /// </summary>
        private void btnNumbersList_Click(object sender, EventArgs e)
        {
            bool isChecked = btnNumbersList.Checked;
            
            ResetControls();

            // Отключим вывод в окно
            WinAPI.SendMessage(txtBox.Handle, WinAPI.WM_SETREDRAW, 0, IntPtr.Zero);

            // получим номера строк выделенного текста
            LineInfo firstSelectedLine = nl.GetLineInfoFromCursorPosition(txtBox.SelectionStart);
            LineInfo lastSelectedLine = nl.GetLineInfoFromCursorPosition(txtBox.SelectionStart + txtBox.SelectionLength);
            int listNr = 1;
            for (int row_id = firstSelectedLine.LineNo; row_id <= lastSelectedLine.LineNo; row_id++)
            {
                NumberedListInfo nli = nl.GetNumberedList(row_id);

                // если это первая строка выделения, то определим дальнейшее поведение нумерации
                if (row_id == firstSelectedLine.LineNo)
                {
                    if (nli.Number >= 0)
                    {
                        // если есть номер в первой выделенной строке, то вся нумерация начинается от него
                        listNr = nli.Number;
                    }
                    else
                    {
                        // если номера в первой выделенной строке нет, то проверим нет ли его в вышестоящей
                        if (firstSelectedLine.LineNo > 0)
                        {
                            NumberedListInfo nli_prev = nl.GetNumberedList(firstSelectedLine.LineNo - 1);
                            if (nli_prev.Number >= 0) listNr = 1 + nli_prev.Number;
                        }
                    }
                }

                if (isChecked)
                {
                    // денумерация
                    if (nli.Number >= 0)
                    {
                        txtBox.SelectionStart = nli.StartIndex;
                        txtBox.SelectionLength = nli.ListLength;
                        txtBox.SelectedText = "";
                    }
                }
                else
                {
                    // нумерация
                    txtBox.SelectionStart = nli.StartIndex;
                    if (nli.Number >= 0)
                    {
                        txtBox.SelectionLength = nli.NumberLength;
                        txtBox.SelectedText = listNr.ToString();
                    }
                    else
                    {
                        txtBox.SelectionLength = 0;
                        txtBox.SelectedText = $"{listNr}. ";
                    }
                }

                listNr++;
            }

            // выделим измененный блок
            int firstChar = nl.GetFirstCharIndexOfLine(firstSelectedLine.LineNo);
            txtBox.Select(
                firstChar,
                nl.GetFirstCharIndexOfLine(lastSelectedLine.LineNo) + txtBox.Lines[lastSelectedLine.LineNo].Length - firstChar
                );

            // Включим вывод в окно
            WinAPI.SendMessage(txtBox.Handle, WinAPI.WM_SETREDRAW, 1, IntPtr.Zero);
            txtBox.Refresh();

            ShowSelectionProperties();
        }

        /// <summary>
        /// Вызвать диалоговое окно редактирования параметров шрифта
        /// </summary>
        private void BtnTextProperties_Click(object sender, EventArgs e)
        {
            ResetControls();

            FontDialog dlg = new FontDialog
            {
                Font = txtBox.SelectionFont
            };
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                txtBox.SelectionFont = dlg.Font;
            }

            ShowSelectionProperties();
            txtBox.Focus();
        }

        /// <summary>
        /// Одиночный клик по кнопке "Формат по образцу".
        /// Сохраняем текущий формат текта
        /// </summary>
        private void BtnTextFormatter_Click(object sender, EventArgs e)
        {
            ResetControls(excludeFormatter: true);

            if (btnTextFormatter.Checked)
            {
                // выключим режим форматтера
                btnTextFormatter.Checked = formatter.enabled = formatter.persistent = false;
            }
            else
            {
                // включим режим форматтера
                btnTextFormatter.Checked = formatter.enabled = true;

                formatter.font = txtBox.SelectionFont;
                formatter.txtColor = txtBox.SelectionColor;
                formatter.bgColor = txtBox.SelectionBackColor;
                formatter.bullet = txtBox.SelectionBullet;
                formatter.indent = txtBox.BulletIndent;
                formatter.txtAlign = txtBox.SelectionAlignment;
            }           
        }

        /// <summary>
        /// Двойной клик по кнопке "Формат по образцу".
        /// Фиксируем состояние кнопки и сохраняем текущий формат текта
        /// </summary>
        private void BtnTextFormatter_DoubleClick(object sender, EventArgs e)
        {
            formatter.persistent = true;
            btnTextFormatter.Checked = false; // нужно потому что вначале будет Click(), а потом DoubleClick()
            btnTextFormatter.PerformClick();
        }


        /// <summary>
        /// Выбран другой шрифт. Изменить текст.
        /// </summary>
        private void BtnFontFamily_SelectedIndexChanged(object sender, EventArgs e)
        {
            ResetControls();

            try
            {
                // проверим если фонт существует
                bool canChangeFont = true;
                FontFamily newFontFamily = null;
                try { newFontFamily = new FontFamily(((ToolStripComboBox)sender).Text); }
                catch { canChangeFont = false; }

                int start = txtBox.SelectionStart;
                int len = txtBox.SelectionLength;
                int end = txtBox.SelectionStart + txtBox.SelectionLength;

                if (canChangeFont)
                {
                    if (end != 0)
                    {
                        // Отключим вывод в окно
                        WinAPI.SendMessage(txtBox.Handle, WinAPI.WM_SETREDRAW, 0, IntPtr.Zero);

                        txtBox.Select(start, 1);
                        Font oldFont = txtBox.SelectionFont;
                        Font newFont = new Font(newFontFamily, oldFont.Size, oldFont.Style);

                        for (int i = txtBox.SelectionStart; i < end; i++)
                        {
                            txtBox.Select(i, 1);
                            if (txtBox.SelectionFont.Size != oldFont.Size)
                            {
                                oldFont = txtBox.SelectionFont;
                                newFont = new Font(newFontFamily, oldFont.Size, oldFont.Style);
                            }
                            if (txtBox.SelectionFont.FontFamily != newFontFamily)
                            {
                                txtBox.SelectionFont = newFont;
                            }
                        }

                        // Включим вывод в окно
                        WinAPI.SendMessage(txtBox.Handle, WinAPI.WM_SETREDRAW, 1, IntPtr.Zero);
                        txtBox.Refresh();
                        txtBox.Select(start, len);
                    }
                    else
                    {
                        // меняется шритф в месте курсора
                        txtBox.SelectionFont = new Font(newFontFamily, txtBox.SelectionFont.Size, txtBox.SelectionFont.Style);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            };

            ShowSelectionProperties();
            txtBox.Focus();
        }

        /// <summary>
        /// Выбран другой размер шрифта. Изменить текст.
        /// </summary>
        private void BtnFontSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            ResetControls();

            bool canChangeFont = float.TryParse(((ToolStripComboBox)sender).Text, out float newSize);

            if (canChangeFont)
            {
                int start = txtBox.SelectionStart;
                int len = txtBox.SelectionLength;
                int end = txtBox.SelectionStart + txtBox.SelectionLength;

                if (end != 0)
                {
                    // Отключим вывод в окно
                    WinAPI.SendMessage(txtBox.Handle, WinAPI.WM_SETREDRAW, 0, IntPtr.Zero);

                    txtBox.Select(start, 1);
                    Font oldFont = txtBox.SelectionFont;
                    Font newFont = new Font(oldFont.FontFamily, newSize, oldFont.Style);

                    for (int i = txtBox.SelectionStart; i < end; i++)
                    {
                        txtBox.Select(i, 1);
                        if (txtBox.SelectionFont.FontFamily != oldFont.FontFamily)
                        {
                            oldFont = txtBox.SelectionFont;
                            newFont = new Font(oldFont.FontFamily, newSize, oldFont.Style);
                        }
                        if (txtBox.SelectionFont.Size != newSize)
                        {
                            txtBox.SelectionFont = newFont;
                        }
                    }

                    // Включим вывод в окно
                    WinAPI.SendMessage(txtBox.Handle, WinAPI.WM_SETREDRAW, 1, IntPtr.Zero);
                    txtBox.Refresh();
                    txtBox.Select(start, len);
                }
                else
                {
                    // меняется размер в месте курсора
                    txtBox.SelectionFont = new Font(txtBox.SelectionFont.FontFamily, newSize, txtBox.SelectionFont.Style);
                }
            }
            else
            {
                ((ToolStripComboBox)sender).Text = txtBox.SelectionFont.Size.ToString();
            }

            TxtBox_TextChanged(txtBox, null);
            txtBox.Focus();
        }

        /// <summary>
        /// Изменить размер шрифта по ручному вводу
        /// </summary>
        private void BtnFontSize_KeyDown(object sender, KeyEventArgs e)
        {
            ResetControls();

            if (e.KeyCode == Keys.Enter)
                BtnFontSize_SelectedIndexChanged(sender, new EventArgs());
        }

        /// <summary>
        /// Применение цвета выделения текста на выбранный текст
        /// </summary>
        private void BtnTextBgColor_ButtonClick(object sender, EventArgs e)
        {
            ResetControls();

            txtBox.SelectionBackColor = textBackgroundColor;
            LastOperation = new object[] { LastOperations.SetBgColor, textBackgroundColor };

            ShowSelectionProperties();
            txtBox.Focus();
        }

        /// <summary>
        /// Выбор цвета выделения текста
        /// </summary>
        private void BtnTextBgColor_Click(object sender, EventArgs e)
        {
            ResetControls();

            Color a = txtBox.SelectionBackColor;
            ToolStripSplitButton btn = (ToolStripSplitButton)sender;
            if (!btn.ButtonPressed)
            {
                bgColorsPanel.Showed = true;
            }
            txtBox.Focus();
        }

        /// <summary>
        /// Отрисовка кнопки выделения текста
        /// </summary>
        private void BtnTextBgColor_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(Images.Images["backcolor"], 3, 5);
            e.Graphics.FillRectangle(new SolidBrush(textBackgroundColor), 3, 17, 16, 4);
        }

        /// <summary>
        /// Применение цвета текста на выбранный текст
        /// </summary>
        private void BtnTextColor_ButtonClick(object sender, EventArgs e)
        {
            ResetControls();

            txtBox.SelectionColor = textColor;
            LastOperation = new object[] { LastOperations.SetTextColor, textColor };

            ShowSelectionProperties();
            txtBox.Focus();
        }

        /// <summary>
        /// Выбор цвета текста
        /// </summary>
        private void BtnTextColor_Click(object sender, EventArgs e)
        {
            ResetControls();

            if (!((ToolStripSplitButton)sender).ButtonPressed)
            {
                ColorDialog dlg = new ColorDialog
                {
                    Color = txtBox.SelectionColor
                };
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    textColor = dlg.Color;
                    txtBox.SelectionColor = textColor;
                    LastOperation = new object[] { LastOperations.SetTextColor, textColor };
                }
            }

            ShowSelectionProperties();
            txtBox.Focus();
        }

        /// <summary>
        /// Отрисовка кнопки цвета текста
        /// </summary>
        private void BtnTextColor_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(Images.Images["textcolor"], 3, 4);
            e.Graphics.FillRectangle(new SolidBrush(textColor), 3, 17, 16, 4);
        }

        /// <summary>
        /// Нажата клавиша жирного текста
        /// </summary>
        private void btnBoldText_Click(object sender, EventArgs e)
        {
            ResetControls();

            if (txtBox.SelectionFont != null)
            {
                FontStyle newFontStyle = txtBox.SelectionFont.Style ^ FontStyle.Bold;
                txtBox.SelectionFont = new Font(txtBox.SelectionFont.FontFamily, txtBox.SelectionFont.Size, newFontStyle);
                LastOperation = new object[1] { LastOperations.SetTextBold };
            }

            ShowSelectionProperties();
            txtBox.Focus();
        }

        /// <summary>
        /// Нажата клавиша наклонного текста
        /// </summary>
        private void btnItalicText_Click(object sender, EventArgs e)
        {
            ResetControls();

            if (txtBox.SelectionFont != null)
            {
                FontStyle newFontStyle = txtBox.SelectionFont.Style ^ FontStyle.Italic;
                txtBox.SelectionFont = new Font(txtBox.SelectionFont.FontFamily, txtBox.SelectionFont.Size, newFontStyle);
                LastOperation = new object[1] { LastOperations.SetTextItalic };
            }

            ShowSelectionProperties();
            txtBox.Focus();
        }

        /// <summary>
        /// Нажата клавиша подчеркнутого текста
        /// </summary>
        private void btnUnderlineText_Click(object sender, EventArgs e)
        {
            ResetControls();

            if (txtBox.SelectionFont != null)
            {
                FontStyle newFontStyle = txtBox.SelectionFont.Style ^ FontStyle.Underline;
                txtBox.SelectionFont = new Font(txtBox.SelectionFont.FontFamily, txtBox.SelectionFont.Size, newFontStyle);
                LastOperation = new object[1] { LastOperations.SetTextUnderline };
            }

            ShowSelectionProperties();
            txtBox.Focus();
        }

        /// <summary>
        /// Данные для форматера текста
        /// </summary>
        private struct FormatterData
        {
            /// <summary>
            /// Режим форматтера активен
            /// </summary>
            public bool enabled;
            /// <summary>
            /// Форматтер включен постоянно (не отключать после выделения)
            /// </summary>
            public bool persistent;
            public Font font;
            public Color txtColor;
            public Color bgColor;
            public bool bullet;
            public int indent;
            public HorizontalAlignment txtAlign;
        }

        public enum ToolBoxPosition : int
        {
            OnTop,
            OnBottom
        }

        // ====================================================================
        /// <summary>
        /// Иноформация об элементе нумерованного списка 
        /// </summary>
        private class NumberedListInfo
        {
            /// <summary>
            /// Номер элемента в списке. Если -1, то указанная строка не является нумерованной.
            /// </summary>
            public int Number = -1;
            /// <summary>
            /// Номер первого символа в строке где начинается нумерованный список.
            /// </summary>
            public int StartIndex = 0;
            /// <summary>
            /// Номер последнего символа за нумерованным списком
            /// </summary>
            public int EndIndex = 0;
            /// <summary>
            /// Количество символов в числовой части нумерованного списка
            /// </summary>
            public int NumberLength = 0;
            /// <summary>
            /// Общее количество символов в префиксе списка
            /// </summary>
            public int ListLength = 0;
        }

        // --------------------------------------------------------------------
        /// <summary>
        /// Информация о строке в которой находится курсор
        /// </summary>
        private class LineInfo
        {
            /// <summary>
            /// Текущая позиция курсора (индекс символа начиная с 0)
            /// </summary>
            public int CurrentPos = 0;
            /// <summary>
            /// Индекс начала строки начиная с 0
            /// </summary>
            public int StartIndex = 0;
            /// <summary>
            /// Индекс конца строки (индекс символа сразу за последним)
            /// </summary>
            public int EndIndex = 0;
            /// <summary>
            /// Количество симоволов в строке без учета переноса каретки
            /// </summary>
            public int Length = 0;
            /// <summary>
            /// Физический номер строки начиная с 0
            /// </summary>
            public int LineNo = 0;
            /// <summary>
            /// Логический номер строки начиная с 0
            /// </summary>
            public int RowNo = 0;
        }

        // --------------------------------------------------------------------
        /// <summary>
        /// Класс для управления нумерованным списком
        /// </summary>
        private class NumberedList
        {
            /// <summary>
            /// Объект RichTextBox с которым будет работать класс
            /// </summary>
            private RichTextBox content = null;

            /// <summary>
            /// Конструктор.
            /// </summary>
            /// <param name="content">Объект RichTextBox с которым будет работать класс</param>
            public NumberedList(RichTextBox content)
            {
                this.content = content;
            }

            /// <summary>
            /// Проверяет является ли текст в выделенном фрагменте нумерованным списком
            /// </summary>
            /// <returns>True-если выделенный текст является нумерованным списком, иначе False</returns>
            public bool IsTheTextNumberedInCurrentPosition()
            {
                bool ret = false;

                // Найти индекс первой стрки в выделении
                LineInfo firstSelectedLine = GetLineInfoFromCursorPosition(content.SelectionStart);
                // Найти индекс последней стрки в выделении
                LineInfo lastSelectedLine = GetLineInfoFromCursorPosition(content.SelectionStart + content.SelectionLength);

                bool temp = false;
                for (int row_id = firstSelectedLine.LineNo; row_id <= lastSelectedLine.LineNo; row_id++)
                {
                    NumberedListInfo nli = GetNumberedList(row_id);

                    temp = nli.Number >= 0;
                    if (row_id == firstSelectedLine.LineNo)
                    {
                        ret = temp;
                    }
                    else
                    {
                        if (ret != temp)
                        {
                            // если нумерация или ее отстутсвие в первой строчке отличается от любой другой
                            // считаем что весь список не нумерован
                            ret = false;
                            break;
                        }
                    }
                }

                return ret;
            }

            /// <summary>
            /// Возвращает информацию о нумерованном списке указанной строки,
            /// если строка является частью нумерованного списка. Если строка не 
            /// является нумерованным списоком в NumberedListInfo.Number будет
            /// отрицательное число, и только параметр NumberedListInfo.StartIndex
            /// будет возвращен корректно (для вставки номера).
            /// </summary>
            /// <param name="lineNo">Номер искомой строки</param>
            /// <param name="rtb">Ссылка на RichTextBox</param>
            /// <returns>Класс NumberedListInfo</returns>
            public NumberedListInfo GetNumberedList(int lineNo)
            {
                NumberedListInfo nli = new NumberedListInfo();

                LineInfo li = GetLineInfo(lineNo);
                if (li.LineNo >= 0)
                {
                    Match m = Regex.Match(content.Lines[lineNo], @"^(\d+)\. +");
                    if (m.Success)
                    {
                        if (int.TryParse(m.Groups[1].Value, out nli.Number))
                        {
                            nli.StartIndex = GetFirstCharIndexOfLine(lineNo);
                            nli.NumberLength = m.Groups[1].Value.Length;
                            nli.ListLength = m.Value.Length;
                            nli.EndIndex = nli.StartIndex + nli.ListLength;
                        }
                    }
                    else
                    {
                        nli.StartIndex = GetFirstCharIndexOfLine(lineNo);
                    }
                }

                return nli;
            }

            /// <summary>
            /// Получает данные строки в которой находится курсор
            /// </summary>
            /// <param name="charIndex">Если задан, то возвращается информация о строке в указанном месте курсора.
            /// Если не указан, или отрицательный, то берется текущее положение курсора</param>
            /// <returns>Класс LineInfo с информацией</returns>
            public LineInfo GetLineInfoFromCursorPosition(int charIndex = -1)
            {
                LineInfo li = new LineInfo();
                li.CurrentPos = (charIndex < 0 ? content.SelectionStart : charIndex);
                li.RowNo = content.GetLineFromCharIndex(li.CurrentPos);

                for (int i = 0; i < content.Lines.Length; i++)
                {
                    li.LineNo = i;

                    li.StartIndex = li.EndIndex + (i > 0 ? 1 : 0);
                    li.Length = content.Lines[i].Length;
                    li.EndIndex = li.StartIndex + li.Length;

                    if (li.CurrentPos <= li.EndIndex) break;
                }

                return li;
            }

            /// <summary>
            /// Возвращает данные физической строки с указанным номером 
            /// </summary>
            /// <param name="lineNo">Номер физической строки начиная с 0</param>
            /// <returns>Класс LineInfo с информацией. Если указанной строки не существует
            /// в LineInfo.LineNo будет отрицательное число</returns>
            public LineInfo GetLineInfo(int lineNo)
            {
                LineInfo li = new LineInfo();

                if (content.Lines.Length == 0)
                {
                    li.LineNo = -1;
                    return li;
                }
                if (lineNo < 0 || lineNo >= content.Lines.Length)
                {
                    li.LineNo = -1;
                    return li;
                }

                li.LineNo = lineNo;
                li.CurrentPos = GetFirstCharIndexOfLine(lineNo);
                li.RowNo = content.GetLineFromCharIndex(li.CurrentPos);
                li.StartIndex = li.CurrentPos;
                li.Length = content.Lines[lineNo].Length;
                li.EndIndex = li.StartIndex + li.Length;

                return li;
            }

            /// <summary>
            /// Находит позицию первого символа в физической строке по ее номеру.
            /// Не проверяет на выход из диапазона
            /// </summary>
            /// <param name="lineNo">Номер строки, начиная с 0</param>
            /// <param name="rtb"></param>
            /// <returns>Позиция первого символа начиная с 0</returns>
            public int GetFirstCharIndexOfLine(int lineNo)
            {
                int startIndex = 0;
                for (int i = 0; i < lineNo; i++)
                {
                    startIndex += content.Lines[i].Length + 1;
                }
                return startIndex;
            }
        }
    }

    /// <summary>
    /// Вспомогательный класс. Содержит в себе коллекцию картинок необходимую для 
    /// отображения кнопок в TextEditor, а так же статический метод для преобразования
    /// коллекции картинок ImageList в код C#.
    /// </summary>
    public class TextEditorImages
    {
        /// <summary>
        /// Коллекция картинок
        /// </summary>
        public ImageList Images;

        /// <summary>
        /// Конструктор. Создает коллекцию картинок доступную в свойстве Images.
        /// </summary>
        public TextEditorImages()
        {
            Images = new ImageList();
            foreach (KeyValuePair<string, byte[]> imData in data)
            {
                Images.Images.Add(imData.Key, LoadFromBytes(imData.Value));
            }
        }

        private Image LoadFromBytes(byte[] a)
        {
            using (var ms = new MemoryStream(a))
            {
                return Image.FromStream(ms);
            }
        }

        /// <summary>
        /// Преобразует ImageList к тексту (код C#) для вставки в код.
        /// </summary>
        /// <param name="ims">ImageList с картинками</param>
        /// <returns>Строка кода C#.</returns>
        public static string ConvertToCodeDeclaration(ImageList ims)
        {
            byte[] ImageToByteArray(Image image)
            {
                using (var ms = new MemoryStream())
                {
                    image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    return ms.ToArray();
                }
            }

            string ret = "private Dictionary<string, byte[]> data = new Dictionary<string, byte[]>()\r\n{\r\n";
            bool firstEntry = true;

            foreach (string imName in ims.Images.Keys)
            {
                if (firstEntry)
                    firstEntry = false;
                else
                    ret += ",\r\n";

                ret += "\t{ \"" + imName + "\", new byte[] { ";

                ret += string.Join(",", ImageToByteArray(ims.Images[imName]));

                ret += " } }";
            }

            ret += "\r\n};";

            return ret;
        }

        /// <summary>
        /// Байтовый массив с изображениями для кнопок редактора (16х16).
        /// Позволяет портировать класс как один *.cs файл.
        /// Ключ - имя картинки, значение - массив байтов картинки.
        /// Массив строится при помощи метода ConvertToCodeDeclaration.
        /// </summary>
        /// <example>
        /// Добавляем на любую форму компонент ImageList, добавляем нужное изображение.
        /// В форме пишем код:
        /// Clipboard.SetText(TextEditorImages.ConvertToCodeDeclaration(imageList3));
        /// После выполнения вставляем в Блокнот содержимое буфера и достаем оттуда только строку с байтами.
        /// </example>
        private Dictionary<string, byte[]> data = new Dictionary<string, byte[]>()
    {
        { "bold", new byte[] { 137,80,78,71,13,10,26,10,0,0,0,13,73,72,68,82,0,0,0,16,0,0,0,16,8,6,0,0,0,31,243,255,97,0,0,0,1,115,82,71,66,0,174,206,28,233,0,0,0,4,103,65,77,65,0,0,177,143,11,252,97,5,0,0,0,9,112,72,89,115,0,0,14,195,0,0,14,195,1,199,111,168,100,0,0,0,173,73,68,65,84,56,79,221,81,177,13,196,32,12,100,10,42,42,86,96,22,106,58,70,97,4,234,20,153,128,9,24,128,158,138,33,152,128,143,45,19,17,41,64,157,63,233,100,72,124,231,147,97,127,134,82,74,243,222,55,99,12,50,198,136,119,165,20,86,0,181,206,145,82,66,129,115,238,110,6,241,85,154,214,122,111,0,83,165,148,15,131,16,2,26,16,215,0,3,206,121,179,214,98,51,196,134,68,112,164,36,107,128,193,85,80,4,147,41,246,109,184,69,143,59,10,198,29,108,23,249,102,64,34,36,37,156,227,205,128,128,223,233,255,28,221,96,124,178,190,23,33,68,171,181,206,13,114,206,248,124,48,29,120,28,39,178,159,151,226,175,130,177,31,107,19,164,2,51,223,139,2,0,0,0,0,73,69,78,68,174,66,96,130 } },
        { "italic", new byte[] { 137,80,78,71,13,10,26,10,0,0,0,13,73,72,68,82,0,0,0,16,0,0,0,16,8,6,0,0,0,31,243,255,97,0,0,0,1,115,82,71,66,0,174,206,28,233,0,0,0,4,103,65,77,65,0,0,177,143,11,252,97,5,0,0,0,9,112,72,89,115,0,0,14,195,0,0,14,195,1,199,111,168,100,0,0,0,115,73,68,65,84,56,79,99,24,230,224,192,129,3,255,45,45,45,255,139,137,137,253,151,145,145,249,159,150,150,246,31,42,69,60,112,114,114,2,105,250,127,253,250,117,210,53,127,252,248,17,172,25,100,59,68,132,68,176,96,193,18,176,1,211,166,77,35,207,0,99,99,99,176,1,119,239,222,37,221,128,87,175,94,129,53,15,81,231,63,126,252,24,172,25,138,73,3,155,54,109,2,39,24,24,110,107,107,35,221,144,161,4,24,24,0,65,163,71,169,92,219,117,158,0,0,0,0,73,69,78,68,174,66,96,130 } },
        { "underline", new byte[] { 137,80,78,71,13,10,26,10,0,0,0,13,73,72,68,82,0,0,0,16,0,0,0,16,8,6,0,0,0,31,243,255,97,0,0,0,1,115,82,71,66,0,174,206,28,233,0,0,0,4,103,65,77,65,0,0,177,143,11,252,97,5,0,0,0,9,112,72,89,115,0,0,14,195,0,0,14,195,1,199,111,168,100,0,0,0,160,73,68,65,84,56,79,99,24,102,224,238,221,187,255,203,203,203,255,167,165,165,253,63,112,224,192,127,168,48,24,180,181,181,129,248,24,226,24,96,230,204,153,255,227,227,227,49,20,253,7,130,208,208,80,252,154,65,0,100,128,147,147,19,86,133,32,151,65,153,184,1,200,169,198,198,198,148,25,32,35,35,67,190,1,160,64,4,82,228,27,0,85,68,190,1,11,22,44,1,27,0,138,82,136,8,4,60,126,252,248,63,84,14,63,248,248,241,35,56,12,64,81,6,98,131,196,64,154,145,249,4,1,72,225,180,105,211,192,9,10,132,65,54,191,122,245,138,56,205,212,0,32,155,240,97,106,2,6,6,0,105,171,119,59,54,77,110,253,0,0,0,0,73,69,78,68,174,66,96,130 } },
        { "textcolor", new byte[] { 137,80,78,71,13,10,26,10,0,0,0,13,73,72,68,82,0,0,0,16,0,0,0,16,8,6,0,0,0,31,243,255,97,0,0,0,1,115,82,71,66,0,174,206,28,233,0,0,0,4,103,65,77,65,0,0,177,143,11,252,97,5,0,0,0,9,112,72,89,115,0,0,14,195,0,0,14,195,1,199,111,168,100,0,0,0,173,73,68,65,84,56,79,237,146,193,13,195,32,12,69,153,130,1,88,129,13,24,134,21,216,128,29,184,229,208,9,152,32,3,176,2,39,110,220,152,192,237,183,156,170,82,17,77,42,245,214,39,125,225,248,219,1,11,212,79,25,99,16,36,159,215,232,189,163,145,245,213,79,182,237,70,90,107,22,98,73,159,131,30,24,99,40,198,200,66,12,196,254,76,173,149,143,222,90,163,35,150,245,28,33,132,231,174,0,99,32,39,246,26,52,96,65,3,78,0,121,239,57,39,222,154,125,223,185,120,38,241,214,56,231,200,90,203,187,189,130,145,224,73,217,156,227,238,115,206,111,133,41,37,246,164,102,78,41,133,103,159,61,28,52,194,67,141,164,254,48,74,221,1,161,230,164,106,21,127,30,250,0,0,0,0,73,69,78,68,174,66,96,130 } },
        { "backcolor", new byte[] { 137,80,78,71,13,10,26,10,0,0,0,13,73,72,68,82,0,0,0,16,0,0,0,16,8,6,0,0,0,31,243,255,97,0,0,0,1,115,82,71,66,0,174,206,28,233,0,0,0,4,103,65,77,65,0,0,177,143,11,252,97,5,0,0,0,9,112,72,89,115,0,0,14,195,0,0,14,195,1,199,111,168,100,0,0,0,217,73,68,65,84,56,79,229,145,177,13,133,48,12,68,153,130,138,42,93,106,54,160,167,164,166,203,40,140,128,196,26,76,192,0,244,84,169,232,232,152,192,159,103,17,4,18,130,80,255,147,44,39,241,221,217,152,36,6,178,193,123,47,206,57,153,231,153,171,236,165,119,64,158,166,73,234,186,150,182,109,191,153,4,49,34,50,24,134,33,206,132,98,24,155,28,200,228,96,178,44,203,189,1,36,68,85,85,29,157,247,146,214,198,113,148,178,44,239,13,32,32,46,138,226,182,59,119,246,113,126,191,128,66,158,231,210,117,157,172,235,170,139,11,100,242,121,31,187,228,10,132,105,154,234,4,44,138,49,121,235,251,94,59,63,138,1,228,45,105,96,66,87,196,225,252,40,102,100,186,108,199,35,172,181,186,176,168,127,207,120,89,150,169,208,24,163,187,136,234,12,32,96,208,52,141,126,51,34,38,34,94,197,127,141,36,249,1,147,179,63,165,199,182,107,27,0,0,0,0,73,69,78,68,174,66,96,130 } },
        { "brush", new byte[] { 137,80,78,71,13,10,26,10,0,0,0,13,73,72,68,82,0,0,0,16,0,0,0,16,8,6,0,0,0,31,243,255,97,0,0,0,1,115,82,71,66,0,174,206,28,233,0,0,0,4,103,65,77,65,0,0,177,143,11,252,97,5,0,0,0,9,112,72,89,115,0,0,14,195,0,0,14,195,1,199,111,168,100,0,0,0,237,73,68,65,84,56,79,157,147,189,13,131,48,16,133,217,137,13,232,41,169,233,88,193,27,80,167,66,98,132,20,217,129,1,188,2,21,29,29,19,92,242,57,247,162,68,136,191,60,233,100,192,247,158,63,99,200,254,145,189,52,207,51,131,249,163,243,26,199,209,218,182,181,170,170,82,136,63,62,167,101,89,172,235,58,27,134,193,154,166,185,70,64,115,223,247,86,215,181,21,69,97,144,248,212,177,100,6,59,132,96,144,248,212,90,236,11,76,12,172,50,77,83,186,47,203,50,153,145,183,174,69,50,77,143,251,45,97,82,220,51,18,176,107,102,82,102,42,198,55,114,158,231,41,0,18,111,93,139,149,57,26,2,48,41,224,59,100,243,165,177,103,153,193,196,192,155,86,0,5,1,61,110,249,21,171,115,166,50,82,92,83,10,128,104,51,0,129,71,136,2,68,66,201,188,123,116,40,198,248,89,157,17,108,66,169,67,179,164,16,8,24,47,127,174,136,237,200,124,249,103,145,64,62,103,206,178,39,162,111,50,232,100,233,70,12,0,0,0,0,73,69,78,68,174,66,96,130 } },
        { "fontedit", new byte[] { 137,80,78,71,13,10,26,10,0,0,0,13,73,72,68,82,0,0,0,16,0,0,0,16,8,6,0,0,0,31,243,255,97,0,0,0,1,115,82,71,66,0,174,206,28,233,0,0,0,4,103,65,77,65,0,0,177,143,11,252,97,5,0,0,0,9,112,72,89,115,0,0,14,195,0,0,14,195,1,199,111,168,100,0,0,1,18,73,68,65,84,56,79,157,147,177,13,132,48,12,69,51,5,21,21,29,53,21,45,61,37,53,29,43,176,1,35,32,177,6,19,48,0,61,21,21,131,248,120,190,228,8,57,78,135,248,146,149,216,142,191,127,76,48,219,182,73,215,117,39,235,251,94,45,140,99,38,196,186,174,159,195,227,56,202,52,77,82,215,181,12,195,160,49,124,226,109,219,170,217,178,3,203,178,104,2,224,179,66,224,138,245,208,142,121,158,165,105,154,111,2,18,116,176,174,42,42,138,66,173,44,75,169,170,74,9,49,246,24,113,242,151,132,40,202,178,76,210,52,85,101,144,199,113,44,121,158,107,1,62,215,35,15,137,45,59,128,108,10,232,226,174,128,15,169,175,148,28,49,235,30,32,145,36,137,74,117,4,81,20,105,71,159,128,89,17,183,238,25,116,71,174,79,0,169,79,0,32,177,219,51,174,8,176,144,224,39,66,130,29,172,247,8,144,229,38,254,72,1,79,155,207,195,16,121,76,224,54,1,157,25,22,159,199,189,5,140,226,171,33,222,2,10,28,233,35,2,174,68,49,131,117,87,178,169,255,160,136,89,48,80,158,53,43,49,254,159,247,9,99,94,116,74,70,38,156,131,136,27,0,0,0,0,73,69,78,68,174,66,96,130 } },
        { "bulletlist", new byte[] { 137,80,78,71,13,10,26,10,0,0,0,13,73,72,68,82,0,0,0,16,0,0,0,16,8,6,0,0,0,31,243,255,97,0,0,0,1,115,82,71,66,0,174,206,28,233,0,0,0,4,103,65,77,65,0,0,177,143,11,252,97,5,0,0,0,9,112,72,89,115,0,0,14,195,0,0,14,195,1,199,111,168,100,0,0,0,156,73,68,65,84,56,79,205,80,161,17,196,32,16,164,10,20,10,135,70,197,226,145,104,28,45,208,78,196,87,64,5,20,128,71,161,40,228,254,247,39,240,19,25,34,242,59,179,115,115,199,44,183,183,236,63,208,123,167,156,51,161,30,163,107,128,184,148,242,253,4,125,107,141,98,140,147,33,132,73,239,61,57,231,206,139,110,59,120,30,227,4,220,126,140,174,1,98,48,165,52,67,28,97,89,107,201,24,67,219,182,145,214,154,148,82,36,165,60,47,130,0,226,101,7,207,3,214,247,253,181,126,2,196,200,0,21,125,173,117,134,37,132,32,206,249,228,231,121,240,135,219,14,214,193,216,27,100,149,142,172,22,126,72,63,0,0,0,0,73,69,78,68,174,66,96,130 } },
        { "numberslist", new byte[] { 137,80,78,71,13,10,26,10,0,0,0,13,73,72,68,82,0,0,0,16,0,0,0,16,8,6,0,0,0,31,243,255,97,0,0,0,1,115,82,71,66,0,174,206,28,233,0,0,0,4,103,65,77,65,0,0,177,143,11,252,97,5,0,0,0,9,112,72,89,115,0,0,14,195,0,0,14,195,1,199,111,168,100,0,0,0,162,73,68,65,84,56,79,197,146,33,14,3,33,16,69,57,5,10,133,67,163,176,120,36,26,199,21,56,203,94,160,135,105,234,81,220,97,37,110,202,36,12,237,174,99,105,186,47,249,130,33,51,255,103,128,253,132,90,43,60,158,59,244,227,60,216,188,52,0,249,30,80,74,129,148,210,80,140,113,40,132,0,222,251,163,25,37,88,78,113,25,114,199,101,246,210,60,216,188,189,224,218,128,243,51,226,18,105,89,206,57,176,214,130,49,6,180,214,160,148,2,41,229,209,104,107,214,168,216,212,75,127,6,157,73,189,52,15,54,47,191,2,37,200,57,143,101,9,33,128,115,62,212,174,73,31,40,254,13,63,145,177,55,248,153,144,143,192,135,167,66,0,0,0,0,73,69,78,68,174,66,96,130 } },
        { "superscript", new byte[] { 137,80,78,71,13,10,26,10,0,0,0,13,73,72,68,82,0,0,0,16,0,0,0,16,8,6,0,0,0,31,243,255,97,0,0,0,1,115,82,71,66,0,174,206,28,233,0,0,0,4,103,65,77,65,0,0,177,143,11,252,97,5,0,0,0,9,112,72,89,115,0,0,14,195,0,0,14,195,1,199,111,168,100,0,0,0,192,73,68,65,84,56,79,229,145,177,13,131,48,16,69,217,201,21,45,59,80,187,99,5,54,240,8,150,232,40,50,1,19,48,0,189,119,112,231,9,46,121,39,159,20,5,43,33,77,154,60,233,75,230,142,127,223,7,221,207,144,7,251,190,203,182,109,146,82,146,90,190,14,102,192,204,144,90,254,30,204,205,1,214,88,215,155,10,150,101,145,24,163,132,16,212,96,233,165,148,243,128,156,179,190,56,77,147,238,74,205,118,198,64,223,214,224,12,106,124,134,134,247,94,69,154,37,3,131,236,70,168,150,207,144,50,142,163,12,195,208,78,249,4,38,204,136,212,90,190,14,215,62,142,67,250,190,23,231,156,238,91,91,239,33,153,68,204,60,115,102,0,106,126,245,87,72,70,246,213,57,207,243,172,226,207,92,190,201,223,209,117,119,50,108,214,173,96,251,221,220,0,0,0,0,73,69,78,68,174,66,96,130 } },
        { "subscript", new byte[] { 137,80,78,71,13,10,26,10,0,0,0,13,73,72,68,82,0,0,0,16,0,0,0,16,8,6,0,0,0,31,243,255,97,0,0,0,1,115,82,71,66,0,174,206,28,233,0,0,0,4,103,65,77,65,0,0,177,143,11,252,97,5,0,0,0,9,112,72,89,115,0,0,14,195,0,0,14,195,1,199,111,168,100,0,0,0,194,73,68,65,84,56,79,237,146,189,13,131,48,16,70,217,201,149,91,118,160,118,199,10,108,224,17,144,220,81,100,2,38,240,0,244,236,224,206,19,92,244,78,182,130,34,162,156,148,54,79,250,132,185,255,51,12,127,94,236,251,46,104,219,30,42,72,41,201,186,174,18,99,148,22,246,153,82,138,6,206,243,44,57,103,77,224,73,209,90,235,247,2,64,145,16,130,234,60,79,91,231,119,232,58,77,147,140,227,168,107,52,179,29,146,72,70,140,223,204,118,24,251,56,14,241,222,139,115,78,88,11,59,133,251,157,176,158,6,95,33,0,39,201,188,115,166,0,226,18,73,6,146,111,39,163,51,194,73,2,231,101,89,84,124,153,62,9,254,219,2,22,122,119,243,103,189,194,4,125,13,206,208,92,54,232,220,255,76,212,204,191,50,12,79,57,27,214,173,174,89,144,172,0,0,0,0,73,69,78,68,174,66,96,130 } },
        { "picture", new byte[] { 137,80,78,71,13,10,26,10,0,0,0,13,73,72,68,82,0,0,0,16,0,0,0,16,8,6,0,0,0,31,243,255,97,0,0,0,1,115,82,71,66,0,174,206,28,233,0,0,0,4,103,65,77,65,0,0,177,143,11,252,97,5,0,0,0,9,112,72,89,115,0,0,14,195,0,0,14,195,1,199,111,168,100,0,0,0,176,73,68,65,84,56,79,205,146,209,13,195,32,12,68,217,137,157,216,137,5,58,76,229,73,80,63,202,4,174,31,152,182,84,74,72,213,143,230,164,139,67,236,59,140,67,56,23,178,136,166,44,26,119,72,158,58,151,204,160,160,214,170,114,211,70,222,63,9,48,113,201,12,12,128,72,178,71,212,100,75,62,189,19,80,231,146,25,195,64,37,52,147,46,162,229,224,177,167,151,6,93,56,24,53,103,35,241,91,131,203,245,110,98,219,29,3,226,17,3,6,133,112,12,178,181,158,216,93,158,131,221,53,232,162,209,178,179,25,116,146,63,100,144,90,219,47,182,245,202,128,255,203,49,41,218,164,229,55,239,1,231,252,233,38,50,160,82,202,146,212,185,228,239,8,225,1,78,102,157,242,222,205,245,144,0,0,0,0,73,69,78,68,174,66,96,130 } },
        { "aligncenter", new byte[] { 137,80,78,71,13,10,26,10,0,0,0,13,73,72,68,82,0,0,0,16,0,0,0,16,8,6,0,0,0,31,243,255,97,0,0,0,1,115,82,71,66,0,174,206,28,233,0,0,0,4,103,65,77,65,0,0,177,143,11,252,97,5,0,0,0,9,112,72,89,115,0,0,14,195,0,0,14,195,1,199,111,168,100,0,0,0,130,73,68,65,84,56,79,221,208,33,18,0,17,24,5,96,167,144,36,77,150,84,93,148,53,87,112,73,93,114,150,127,231,25,140,221,176,97,135,178,111,230,197,127,62,15,251,81,82,74,52,26,99,156,13,33,180,122,239,201,57,215,106,173,165,126,182,57,181,214,155,12,117,200,80,141,49,84,74,57,164,191,237,133,172,181,110,85,74,145,148,242,208,43,176,111,168,171,12,117,200,57,231,67,250,186,119,149,161,162,66,8,226,156,207,246,179,205,193,31,172,123,161,62,229,115,127,240,45,140,93,229,231,125,173,9,80,235,60,0,0,0,0,73,69,78,68,174,66,96,130 } },
        { "alignleft", new byte[] { 137,80,78,71,13,10,26,10,0,0,0,13,73,72,68,82,0,0,0,16,0,0,0,16,8,6,0,0,0,31,243,255,97,0,0,0,1,115,82,71,66,0,174,206,28,233,0,0,0,4,103,65,77,65,0,0,177,143,11,252,97,5,0,0,0,9,112,72,89,115,0,0,14,195,0,0,14,195,1,199,111,168,100,0,0,0,118,73,68,65,84,56,79,221,143,33,14,0,33,12,4,121,5,10,133,67,163,176,120,36,26,199,23,120,126,47,37,45,217,32,78,65,114,185,73,198,209,12,107,126,196,24,131,212,222,251,178,181,54,173,181,82,41,101,154,115,38,57,59,8,86,181,204,85,45,223,169,34,111,123,83,74,20,99,156,134,16,200,123,127,225,55,88,230,42,150,185,202,202,211,75,224,94,44,243,94,214,57,71,214,218,165,156,29,100,223,171,85,44,203,211,79,97,204,3,77,223,112,51,201,211,201,87,0,0,0,0,73,69,78,68,174,66,96,130 } },
        { "alignright", new byte[] { 137,80,78,71,13,10,26,10,0,0,0,13,73,72,68,82,0,0,0,16,0,0,0,16,8,6,0,0,0,31,243,255,97,0,0,0,1,115,82,71,66,0,174,206,28,233,0,0,0,4,103,65,77,65,0,0,177,143,11,252,97,5,0,0,0,9,112,72,89,115,0,0,14,195,0,0,14,195,1,199,111,168,100,0,0,0,117,73,68,65,84,56,79,221,144,33,14,192,32,12,69,57,5,10,133,67,163,176,120,36,26,199,21,56,126,151,54,124,66,200,50,181,38,203,94,242,28,205,163,53,63,98,140,65,176,247,190,108,173,137,181,86,42,165,136,57,103,154,99,74,160,204,85,148,185,154,82,18,99,140,10,63,120,218,23,85,54,132,64,222,123,229,27,112,117,47,115,21,101,231,156,56,159,190,200,221,165,81,69,217,90,187,156,99,74,156,251,30,101,248,9,140,185,0,162,233,107,26,128,68,158,131,0,0,0,0,73,69,78,68,174,66,96,130 } },
        { "caps", new byte[] { 137,80,78,71,13,10,26,10,0,0,0,13,73,72,68,82,0,0,0,16,0,0,0,16,8,6,0,0,0,31,243,255,97,0,0,0,1,115,82,71,66,0,174,206,28,233,0,0,0,4,103,65,77,65,0,0,177,143,11,252,97,5,0,0,0,9,112,72,89,115,0,0,14,195,0,0,14,195,1,199,111,168,100,0,0,0,200,73,68,65,84,56,79,237,145,177,13,132,48,12,69,51,72,170,116,212,84,180,233,41,169,233,88,129,54,85,70,136,196,54,12,64,159,1,210,165,99,2,31,223,231,64,78,52,92,121,210,61,41,138,253,191,101,155,160,254,220,233,251,158,112,36,253,142,156,51,13,195,64,77,211,16,98,145,159,19,66,160,101,89,200,24,67,136,69,62,217,182,141,61,220,34,125,210,117,29,79,158,166,137,16,139,124,226,189,231,237,112,139,116,145,82,34,107,45,197,24,121,186,214,154,160,137,205,20,31,158,72,23,232,10,163,62,245,36,52,171,189,219,103,96,181,122,226,56,142,188,174,164,60,96,158,103,222,0,158,115,238,237,209,65,219,182,72,248,198,27,172,235,202,121,209,144,227,241,240,135,224,151,122,174,67,3,176,239,59,155,79,14,106,11,71,163,223,70,169,23,244,127,210,232,229,194,147,21,0,0,0,0,73,69,78,68,174,66,96,130 } },
        { "find", new byte[] { 137,80,78,71,13,10,26,10,0,0,0,13,73,72,68,82,0,0,0,16,0,0,0,16,8,6,0,0,0,31,243,255,97,0,0,0,1,115,82,71,66,0,174,206,28,233,0,0,0,4,103,65,77,65,0,0,177,143,11,252,97,5,0,0,0,9,112,72,89,115,0,0,14,195,0,0,14,195,1,199,111,168,100,0,0,1,11,73,68,65,84,56,79,205,145,45,14,132,64,12,133,57,5,10,133,67,163,176,120,36,26,55,87,192,162,56,2,201,56,196,158,128,19,112,128,61,9,142,19,116,231,107,40,127,9,89,75,147,166,51,157,190,215,215,78,244,62,27,199,143,76,211,36,195,48,136,221,57,247,125,175,247,191,230,189,151,121,158,165,105,26,5,180,109,43,228,32,210,130,39,115,206,237,157,136,69,81,40,160,44,75,129,4,39,7,145,153,2,57,44,203,162,32,188,235,58,169,170,74,29,16,196,206,121,113,65,5,228,228,81,120,33,96,102,220,58,81,236,253,119,119,128,74,18,200,56,163,228,66,64,103,164,237,4,218,213,73,150,101,187,146,115,126,131,29,4,44,12,214,186,174,53,230,121,46,204,190,174,171,118,140,227,88,210,52,213,136,39,73,114,16,96,54,63,179,217,23,218,214,249,1,72,236,141,251,166,226,48,147,73,209,77,106,132,58,123,35,199,25,21,10,52,179,175,58,23,49,6,70,52,5,246,22,32,207,4,196,51,1,10,32,56,255,82,128,92,9,108,113,56,155,103,97,182,40,198,219,36,223,253,21,22,69,63,2,64,102,210,202,212,253,203,0,0,0,0,73,69,78,68,174,66,96,130 } }
    };
    }

    /// <summary>
    /// Класс для работы с функциями WinAPI
    /// </summary>
    public static class WinAPI
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, IntPtr lParam);

        public const int WM_SETREDRAW = 0x000B;
    }

    /// <summary>
    /// Класс для отображения панели выбора цвета заднего фона
    /// </summary>
    class BackColorsPanel
    {
        private Panel p;
        private Panel shadow;
        private TextEditor parent;

        public bool Showed
        {
            get
            {
                return p.Visible;
            }
            set
            {
                foreach (Control c in p.Controls)
                {
                    if (c.GetType() == typeof(PictureBox))
                    {
                        if (c.BackColor == this.selectedColor)
                            ((PictureBox)c).Tag = (int)((PictureBox)c).Tag | (int)2;
                        else
                            ((PictureBox)c).Tag = (int)((PictureBox)c).Tag & ~(int)2;
                    }
                }
                p.Visible = shadow.Visible = value;
            }
        }

        public Color selectedColor;

        public BackColorsPanel(int x, int y, TextEditor parent)
        {
            this.parent = parent;

            p = new Panel
            {
                Visible = false,
                Parent = parent.txtBox,
                Location = new Point(x, y),
                BackColor = SystemColors.Control,
                BorderStyle = BorderStyle.FixedSingle,
                Cursor = Cursors.Default,
                Size = new Size(156, 139)
            };


            shadow = new Panel
            {
                Visible = false,
                Parent = parent.txtBox,
                Location = new Point(x + 3, y + 3),
                BackColor = Color.Gray,
                BorderStyle = BorderStyle.None,
                Size = new Size(156, 139)
            };

            PictureBox pb;
            Size pb_size = new Size(24, 24);
            Color[,] cellColor = { { Color.Yellow, Color.Lime, Color.Aqua, Color.Fuchsia, Color.Blue },
                                { Color.Red, Color.Navy, Color.Teal, Color.Green, Color.Purple },
                                { Color.Maroon, Color.Olive, Color.Gray, Color.Silver, Color.Black } };

            for (int row = 0; row < 3; row++)
                for (int col = 0; col < 5; col++)
                {
                    pb = new PictureBox
                    {
                        Parent = p,
                        Location = new Point(5 + col * 30, 5 + row * 30),
                        Size = pb_size,
                        BorderStyle = BorderStyle.FixedSingle,
                        BackColor = cellColor[row, col],
                        Tag = 0
                    };
                    pb.Click += Pb_Click;
                    pb.Paint += Pb_Paint;
                    pb.MouseEnter += Pb_MouseEnter;
                    pb.MouseLeave += Pb_MouseLeave;
                }

            pb = new PictureBox
            {
                Parent = p,
                Location = new Point(5, 105),
                Size = pb_size,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White,
                Tag = 0
            };
            pb.Click += Pb_Click;
            pb.Paint += Pb_Paint;
            pb.MouseEnter += Pb_MouseEnter;
            pb.MouseLeave += Pb_MouseLeave;

            Label lbl = new Label
            {
                Parent = p,
                Location = new Point(35, 108),
                Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular),
                Text = "без цвета"
            };

        }


        private void Pb_MouseLeave(object sender, EventArgs e)
        {
            ((PictureBox)sender).Tag = (int)((PictureBox)sender).Tag & ~(int)1;
            ((PictureBox)sender).Refresh();
        }

        private void Pb_MouseEnter(object sender, EventArgs e)
        {
            ((PictureBox)sender).Tag = (int)((PictureBox)sender).Tag | (int)1;
            ((PictureBox)sender).Refresh();
        }

        private void Pb_Paint(object sender, PaintEventArgs e)
        {
            Rectangle outer = new Rectangle(0, 0, 21, 21);
            Rectangle inner = new Rectangle(1, 1, 19, 19);

            if ((int)((PictureBox)sender).Tag > 0)
            {
                e.Graphics.DrawRectangle(new Pen(Color.Red, 1), inner);
                e.Graphics.DrawRectangle(new Pen(Color.White, 1), outer);
            }
        }

        /// <summary>
        /// Пользователь выбрал цвет на панели цветов выделения текста.
        /// </summary>
        private void Pb_Click(object sender, EventArgs e)
        {
            Color c = ((PictureBox)sender).BackColor;
            this.selectedColor = (c == Color.White ? Color.Transparent : c);
            this.Showed = false;

            parent.textBackgroundColor = this.selectedColor;
            parent.txtBox.SelectionBackColor = this.selectedColor;
            parent.LastOperation = new object[2] { LastOperations.SetBgColor, this.selectedColor }; 
        }
    }



    /// <summary>
    /// Класс окна ввода текста поиска.
    /// </summary>
    class frmSearch : Form
    {
        private Button btnFindNext;
        private TextBox tbSearchPattern;
        private RichTextBox tb;
        private int lastIndex;
        private string tb_text;

        /// <summary>
        /// Конструктор формы.
        /// </summary>
        /// <param name="tb">Ссылка на компонент RichTextBox в котором будет вестись поиск текста.</param>
        public frmSearch(RichTextBox tb)
        {
            lastIndex = -1;
            tb_text = tb.Text.ToLower();
            this.tb = tb;
            //
            btnFindNext = new System.Windows.Forms.Button();
            tbSearchPattern = new System.Windows.Forms.TextBox();
            SuspendLayout();
            // 
            // btnFindNext
            // 
            btnFindNext.Location = new System.Drawing.Point(317, 17);
            btnFindNext.Name = "btnFindNext";
            btnFindNext.Size = new System.Drawing.Size(75, 20);
            btnFindNext.TabIndex = 1;
            btnFindNext.Text = ">>>";
            btnFindNext.UseVisualStyleBackColor = true;
            btnFindNext.Click += BtnFindNext_Click;
            // 
            // tbSearchPattern
            // 
            tbSearchPattern.Location = new System.Drawing.Point(12, 17);
            tbSearchPattern.Name = "tbSearchPattern";
            tbSearchPattern.Size = new System.Drawing.Size(299, 20);
            tbSearchPattern.TabIndex = 0;
            // 
            // frmSearcProperty
            // 
            AcceptButton = btnFindNext;
            AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(403, 56);
            Controls.Add(tbSearchPattern);
            Controls.Add(btnFindNext);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            Name = "frmSearcProperty";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Поиск текста (регистронезависимый)";
            ResumeLayout(false);
            PerformLayout();
        }

        /// <summary>
        /// Нажата кнопка поиска ">>>". Ищем следующее совпадение.
        /// </summary>
        private void BtnFindNext_Click(object sender, EventArgs e)
        {
            int loop = 2;
            while (loop > 0)
            {
                int patternLength = tbSearchPattern.Text.Length;
                int foundIndex = tb_text.IndexOf(tbSearchPattern.Text.ToLower(), lastIndex >= 0 ? lastIndex : 0);
                if (foundIndex >= 0)
                {
                    lastIndex = foundIndex + patternLength;
                    tb.SelectionStart = foundIndex;
                    tb.SelectionLength = patternLength;
                    tb.ScrollToCaret();
                    loop = 0;
                }
                else
                {
                    tb.SelectionLength = 0;
                    lastIndex = -1;
                    loop--;
                }
            }
        }
    }

    /// <summary>
    /// Константы последней операции
    /// </summary>
    enum LastOperations
    {
        SetTextBold,
        SetTextItalic,
        SetTextUnderline,
        SetTextColor,
        SetBgColor,
    }
}

