using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing;
using System.Linq;

namespace simple_database
{
    /// <summary>
    /// Класс для управления плагинами
    /// </summary>
    public static class PluginsManager
    {
        /// <summary>
        /// Словарь содержащий параметры из файла плагина. 
        /// Параметры это строки начинающиеся со служебной комбинации: //$
        /// </summary>
        private static Dictionary<string, string> parameters = new Dictionary<string, string>();

        /// <summary>
        /// Список ошибок
        /// </summary>
        private static List<string> errors = new List<string>();

        /// <summary>
        /// Пользовательские переменные которые будут запрошены в форме ввода
        /// </summary>
        private static Dictionary<string, InputField> user_data = new Dictionary<string, InputField>();

        /// <summary>
        /// Запускает плагин
        /// </summary>
        /// <param name="id"></param>
        public static void StartPlugin(long id)
        {
            // очищаем параметры
            errors.Clear();
            parameters.Clear();

            // считываем имя плагина и его код
            string code = DATABASE.Plugin_Read(id);
            string filename = DATABASE.AttachmentGetFilename(id);
            string extension = Path.GetExtension(filename).ToLower();

            // ищем все параметры
            MatchCollection mm = Regex.Matches(code, @"{#SET\s+([A-Z]+)=(.+)(?=#})", RegexOptions.Multiline);
            int shorten = 0;
            foreach (Match m in mm)
            {
                if (m.Groups.Count == 3)
                {
                    parameters.Add(m.Groups[1].Value.Trim(), m.Groups[2].Value.Trim());
                }

                //code = code.Remove(m.Index - shorten, m.Length);
                //shorten += m.Length;
            }

            // получаем список пользовательских параметров
            user_data.Clear();
            mm = Regex.Matches(code, @"{#ASK\s+NAME=""(.*?)""\s+TYPE=""(.*?)""\s+TEXT=""(.*?)""\s+VALUE=""(.*?)""\s*#}", RegexOptions.Multiline);
            shorten = 0;
            foreach (Match m in mm)
            {
                if (m.Groups.Count == 5)
                {
                    user_data.Add(m.Groups[1].Value, new InputField() { Type = m.Groups[2].Value, Text = m.Groups[3].Value, Value = m.Groups[4].Value });
                }

                //code = code.Remove(m.Index - shorten, m.Length);
                //shorten += m.Length;
            }

            // если есть пользовательские параметры, то выводим форму запроса
            if (user_data.Count > 0) ShowUserInputForm();

            // подставляем полученные пользовательские данные в код
            mm = Regex.Matches(code, @"{#VALUE\s+(.+)#}", RegexOptions.Multiline);
            StringBuilder newcode = new StringBuilder(code.Length);
            int index = 0;
            foreach (Match m in mm)
            {
                if (m.Groups.Count == 2)
                {
                    if (user_data.ContainsKey(m.Groups[1].Value.Trim()))
                    {
                        newcode.Append(code.Substring(index, m.Index - index));
                        newcode.Append(user_data[m.Groups[1].Value.Trim()].Value);
                        index = m.Index + m.Length;
                    }
                    else
                    {
                        errors.Add($"Переменная '{m.Groups[1].Value.Trim()}' не определена");
                        ShowErrors();
                        return;
                    }
                }
            }

            newcode.Append(code.Substring(index, code.Length - index));

            code = newcode.ToString();

            // в зависимости от расширения пытаемся его выполнить
            switch (extension)
            {
                case ".cs":
                    // компиляция и запуск C# файлов
                    RunCSharp(filename, code);
                    break;

                case ".py":
                    // запуск файла на языке Python
                    try
                    {
                        string script_filename = Path.Combine(VARS.temp_folder, filename);
                        File.WriteAllText(script_filename, code);
                        Process.Start("python.exe", $"-u \"{script_filename}\"");
                    }
                    catch (Exception ex)
                    {
                        errors.Add(ex.Message);
                        ShowErrors();
                    }
                    break;

                case ".ps1":
                    // запуск файла на языке PowerShell
                    try
                    {
                        string script_filename = Path.Combine(VARS.temp_folder, filename);
                        File.WriteAllText(script_filename, code);
                        Process.Start("powershell.exe", $"-NoProfile -ExecutionPolicy Unrestricted -File \"{script_filename}\"");
                    }
                    catch (Exception ex)
                    {
                        errors.Add(ex.Message);
                        ShowErrors();
                    }
                    break;

                default:
                    // попытка запустить все другие файлы
                    try
                    {
                        string script_filename = Path.Combine(VARS.temp_folder, filename);
                        File.WriteAllText(script_filename, code);
                        Process.Start(script_filename);
                    }
                    catch (Exception ex)
                    {
                        errors.Add(ex.Message);
                        ShowErrors();
                    }
                    break;
            }
        }


        /// <summary>
        /// Открывает плагин с указанным ROWID
        /// </summary>
        /// <param name="id">ROWID узла из оглавления</param>
        private static void RunCSharp(string filename, string code)
        {
            errors.Clear();

            string exename = Path.GetFileNameWithoutExtension(filename) + ".exe";

            // проверяем тип приложения
            if (!parameters.ContainsKey("TYPE"))
            {
                errors.Add("Не найден обязательный параметр TYPE=<APPLICATION|CLASS>.");
                ShowErrors();
                return;
            }

            // проверяем имя класса для запуска

            if (parameters["TYPE"] == "CLASS" && !parameters.ContainsKey("CLASS"))
            {
                errors.Add("Не найден обязательный параметр CLASS=<Имя класса>, необходимый для запуска.");
                ShowErrors();
                return;
            }

            // ищем имя стартового метода для запуска
            if (parameters["TYPE"] == "CLASS" && !parameters.ContainsKey("RUN"))
            {
                errors.Add("Не найден обязательный параметр RUN=<Имя метода класса>, необходимый для запуска.");
                ShowErrors();
                return;
            }

            // проверяем references
            if (!parameters.ContainsKey("REFERENCES")) parameters.Add("REFERENCES", "");

            // подготавливаем код к компиляции
            CodeDomProvider provider = provider = CodeDomProvider.CreateProvider("CSharp");
            CompilerParameters cp = null;
            CompilerResults cr = null;
            string[] references = parameters["REFERENCES"].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            switch (parameters["TYPE"])
            {
                case "APPLICATION":
                    cp = new CompilerParameters()
                    {
                        GenerateExecutable = true,
                        OutputAssembly = Path.Combine(VARS.temp_folder, exename),
                        GenerateInMemory = false,
                        TreatWarningsAsErrors = false
                    };
                    break;

                case "CLASS":
                    cp = new CompilerParameters()
                    {
                        GenerateExecutable = false,
                        GenerateInMemory = true,
                        TreatWarningsAsErrors = false
                    };
                    break;

                default:
                    break;
            }

            // добавим references
            foreach (string reference in references)
                cp.ReferencedAssemblies.Add(reference.Trim());

            // компилируем и запускаем код
            try
            {
                cr = provider.CompileAssemblyFromSource(cp, code);
            }
            catch (Exception ex)
            {
                errors.Add($"Ошибка компиляции файла '{exename}'. Текст ошибки:");
                errors.Add(ex.Message);
                ShowErrors();
                return;
            }

            if (cr.Errors.Count > 0)
            {
                foreach (CompilerError ce in cr.Errors)
                {
                    errors.Add(ce.ToString());
                    ShowErrors();
                    return;
                }
            }
            else
            {
                if (parameters["TYPE"] == "APPLICATION")
                {
                    try
                    {
                        Process.Start(cr.PathToAssembly);
                    }
                    catch (Exception ex)
                    {
                        errors.Add($"Невозможно запустить файл '{exename}'. Текст ошибки:");
                        errors.Add(ex.Message);
                        ShowErrors();
                        return;
                    }
                }
                else if (parameters["TYPE"] == "CLASS")
                {
                    try
                    {
                        object o = cr.CompiledAssembly.CreateInstance(parameters["CLASS"]);
                        if (o != null)
                        {
                            System.Reflection.MethodInfo mi = o.GetType().GetMethod(parameters["RUN"]);
                            if (mi != null)
                            {
                                mi.Invoke(o, null);
                            }
                            else
                            {
                                errors.Add($"Метод '{parameters["RUN"]}()' не определен в классе '{parameters["CLASS"]}' исходного кода");
                                ShowErrors();
                                return;
                            }
                        }
                        else
                        {
                            errors.Add($"Класс '{parameters["CLASS"]}' не найден в исходном коде");
                            ShowErrors();
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        errors.Add(ex.Message);
                        ShowErrors();
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Отображает окно с ошибками
        /// </summary>
        private static void ShowErrors()
        {
            frmPluginShowErrors frm = new frmPluginShowErrors();
            frm.DisplayErrorMessages(errors);
            frm.ShowDialog();
        }

        /// <summary>
        /// Показывает форму ввода пользовательских запросов.
        /// </summary>
        public static void ShowUserInputForm()
        {
            int maxHeight = 0;
            Icon icon = Icon.FromHandle(Properties.Resources.plugin_16.GetHicon());

            // создаем форму
            Form frm = new Form();
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.BackColor = Color.FromArgb(250, 250, 250);
            frm.Padding = new Padding(5);
            frm.Width = 300;
            frm.Height = 400;
            frm.Icon = icon;
            if (parameters.ContainsKey("ASKTITLE")) frm.Text = parameters["ASKTITLE"].Trim();

            frm.SuspendLayout();

            for (int i = user_data.Count - 1; i >= 0; i--)
            {
                KeyValuePair<string, InputField> input = user_data.ElementAt(i);
                switch (input.Value.Type)
                {
                    case "TextBox":
                        TextBox tb = new TextBox();
                        tb.Dock = DockStyle.Top;
                        tb.Text = input.Value.Value;
                        tb.Parent = frm;
                        tb.Tag = input.Key;
                        break;

                    case "DateTimePicker":
                        DateTimePicker dtp = new DateTimePicker();
                        dtp.Dock = DockStyle.Top;
                        if (DateTime.TryParse(input.Value.Value, out DateTime dt))
                        {
                            dtp.Value = dt;
                        }
                        else
                        {
                            dtp.Value = DateTime.Now;
                        }
                        dtp.Tag = input.Key;
                        dtp.Parent = frm;
                        break;

                    case "Button":
                        Panel btnHolder = new Panel();
                        btnHolder.Padding = new Padding(10);
                        btnHolder.Dock = DockStyle.Top;
                        btnHolder.AutoSize = true;
                        btnHolder.Height = 10;
                        
                        Button btn = new Button();
                        btn.Text = input.Value.Text;
                        btn.Dock = DockStyle.Top;
                        btn.Height += 5;
                        btn.Parent = btnHolder;
                        btn.Tag = input.Key;
                        btn.Click += Btn_Click;

                        btnHolder.Parent = frm;

                        break;

                    case "Label":
                        Label lbl = new Label();
                        lbl.Dock = DockStyle.Top;
                        lbl.Text = input.Value.Text.Replace("\\n", Environment.NewLine);
                        lbl.TextAlign = ContentAlignment.MiddleCenter;
                        lbl.AutoSize = true;
                        lbl.Parent = frm;
                        break;
                }

                if (input.Value.Type != "Button" && input.Value.Type != "Label")
                {
                    // создаем метку
                    Label lbl = new Label();
                    lbl.Dock = DockStyle.Top;
                    lbl.Text = input.Value.Text;
                    lbl.ForeColor = Color.Blue;
                    lbl.TextAlign = ContentAlignment.BottomLeft;
                    lbl.Parent = frm;
                }
            }

            frm.ResumeLayout();
            Application.DoEvents();

            foreach (Control c in frm.Controls)
            {
                maxHeight += c.Height;
            }

            frm.Height = maxHeight + 70;

            frm.ShowDialog();

            icon.Dispose();
        }

        /// <summary>
        /// Закрывает форму пользовательского ввода
        /// </summary>
        private static void Btn_Click(object sender, EventArgs e)
        {
            user_data[(string)((Button)sender).Tag].Value = "pressed";

            Form frm = (Form)((Panel)((Button)sender).Parent).Parent;

            foreach (Control c in frm.Controls)
            {
                if (c.GetType() == typeof(TextBox))
                {
                    user_data[(string)c.Tag].Value = ((TextBox)c).Text;
                }
                else if (c.GetType() == typeof(DateTimePicker))
                {
                    user_data[(string)c.Tag].Value = ((DateTimePicker)c).Value.ToString();
                }
            }


            frm.Close();
        }
    }

    /// <summary>
    /// Класс пользовательской переменной
    /// </summary>
    class InputField
    {
        /// <summary>
        /// Надпись над полем запроса
        /// </summary>
        public string Text = "";

        /// <summary>
        /// Значение. Если установлено в директиве ASK, то задает значение для вывода в поле запроса.
        /// После закрытия формы, если была нажата любая кнопка, содержит введенное в поле значение в текстовом виде
        /// </summary>
        public string Value = "";

        /// <summary>
        /// Тип создаваемого поля ввода. Может быть одним из: TextBox, Button, DateTimePicker, Label
        /// </summary>
        public string Type = "";
    }
}
