using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace simple_database
{
    /// <summary>
    /// Статический класс для хранения глобальных переменных
    /// </summary>
    public static class VARS
    {
        /// <summary>
        /// Ссылка на главную форму
        /// </summary>
        public static Form1 main_form;

        /// <summary>
        /// Флаг показывающий что содержимое оглавления выбранного элемента каталога завершено.
        /// Требует ручного сброса.
        /// </summary>
        public static bool class_update_finished;

        /// <summary>
        /// Флаг показывающий что содержимое текста выбранного элемента оглавления завершено.
        /// Требует ручного сброса.
        /// </summary>
        public static bool property_update_finished;

        /// <summary>
        /// Папка temp программы
        /// </summary>
        public static string temp_folder;

        /// <summary>
        /// Флаг, показывающий является ли переход результатом нажатия кнопок навигации по истории
        /// </summary>
        public static bool moving_over_history = false;

        /// <summary>
        /// Класс для хранения синтаксических правил
        /// </summary>
        public static SyntaxRulesHolder syntaxRules = new SyntaxRulesHolder();
    }
}
