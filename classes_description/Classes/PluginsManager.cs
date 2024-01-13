using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.IO;
using System.Diagnostics;

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
        /// Открывает плагин с указанным ROWID
        /// </summary>
        /// <param name="id">ROWID узла из оглавления</param>
        public static void Open(long id)
        {
            parameters.Clear();
            parameters.Add("apptype", "");
            parameters.Add("references", "");
            parameters.Add("class", "");
            parameters.Add("run", "");
            parameters.Add("filename", "");

            errors.Clear();

            string source = DATABASE.Plugin_Read(id);
            parameters["filename"] = Path.GetFileNameWithoutExtension(DATABASE.AttachmentGetFilename(id)) + ".exe";

            // ищем тип приложения
            Match m = Regex.Match(source, @"^\/\/\$TYPE\s([^\x0D\x0A]+).*$", RegexOptions.Multiline);
            if (m.Success && m.Groups.Count == 2)
            {
                parameters["apptype"] = m.Groups[1].Value.ToUpper();
                source = source.Remove(m.Index, m.Length);
            }
            else
            {
                errors.Add("Не найден обязательный параметр //$TYPE {APPLICATION|CLASS}");
                ShowErrors();
                return;
            }

            // ищем ссылки на библиотеки
            m = Regex.Match(source, @"^\/\/\$REFERENCES\s([^\x0D\x0A]+).*$", RegexOptions.Multiline);
            if (m.Success && m.Groups.Count == 2)
            {
                parameters["references"] = m.Groups[1].Value;
                source = source.Remove(m.Index, m.Length);
            }

            // ищем имя класса для запуска
            m = Regex.Match(source, @"^\/\/\$CLASS\s([^\x0D\x0A]+).*$", RegexOptions.Multiline);
            if (m.Success && m.Groups.Count == 2)
            {
                parameters["class"] = m.Groups[1].Value.Trim();
                source = source.Remove(m.Index, m.Length);
            }
            else if (parameters["apptype"] == "CLASS")
            {
                errors.Add("Не найден обязательный параметр //$CLASS <Имя класса>, необходимый для запуска.");
                ShowErrors();
                return;
            }

            // ищем имя стартового метода для запуска
            m = Regex.Match(source, @"^\/\/\$RUN\s([^\x0D\x0A]+).*$", RegexOptions.Multiline);
            if (m.Success && m.Groups.Count == 2)
            {
                parameters["run"] = m.Groups[1].Value.Trim();
                source = source.Remove(m.Index, m.Length);
            }
            else if (parameters["apptype"] == "CLASS")
            {
                errors.Add("Не найден обязательный параметр //$RUN <Имя метода класса>, необходимый для запуска.");
                ShowErrors();
                return;
            }

            // подготавливаем код к компиляции
            CodeDomProvider provider = provider = CodeDomProvider.CreateProvider("CSharp");
            CompilerParameters cp = null;
            CompilerResults cr = null;
            string[] references = parameters["references"].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            switch (parameters["apptype"])
            {
                case "APPLICATION":
                    cp = new CompilerParameters()
                    {
                        GenerateExecutable = true,
                        OutputAssembly = Path.Combine(VARS.temp_folder, parameters["filename"]),
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
                cr = provider.CompileAssemblyFromSource(cp, source);
            }
            catch (Exception ex)
            {
                errors.Add($"Ошибка компиляции файла '{parameters["filename"]}'. Текст ошибки:");
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
                if (parameters["apptype"] == "APPLICATION")
                {
                    try
                    {
                        Process.Start(cr.PathToAssembly);
                    }
                    catch (Exception ex)
                    {
                        errors.Add($"Невозможно запустить файл '{parameters["filename"]}'. Текст ошибки:");
                        errors.Add(ex.Message);
                        ShowErrors();
                        return;
                    }
                }
                else if (parameters["apptype"] == "CLASS")
                {
                    object o = cr.CompiledAssembly.CreateInstance(parameters["class"]);
                    if (o != null)
                    {
                        System.Reflection.MethodInfo mi = o.GetType().GetMethod(parameters["run"]);
                        if (mi != null)
                        {
                            mi.Invoke(o, null);
                        }
                        else
                        {
                            errors.Add($"Метод '{parameters["run"]}()' не определен в классе '{parameters["class"]}' исходного кода");
                            ShowErrors();
                            return;
                        }
                    }
                    else
                    {
                        errors.Add($"Класс '{parameters["class"]}' не найден в исходном коде");
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
    }
}
