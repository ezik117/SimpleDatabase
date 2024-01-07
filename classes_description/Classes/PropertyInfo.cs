using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace simple_database
{
    /// <summary>
    /// Класс описывающий свойства текущего выбранного элемента оглавления
    /// </summary>
    public class PropertyInfo
    {
        /// <summary>
        /// Признак того, что элемент является корневым
        /// </summary>
        public bool isRoot = false;

        /// <summary>
        /// Признак того, что элемент добавлен в закладки
        /// </summary>
        public bool isBookmarked = false;

        /// <summary>
        /// Признак того, что элемент добавлен в избранное
        /// </summary>
        public bool isFavourite = false;

        /// <summary>
        /// Признак того, что элемент имеет ключевые слова
        /// </summary>
        public bool thereAreKeywords = false;

        /// <summary>
        /// ID в БД
        /// </summary>
        public long id = -1;

        /// <summary>
        /// Всплывающая подсказка при наведении на панель значков
        /// </summary>
        public string Tip = "";

        /// <summary>
        /// Сбрасывает все параметры
        /// </summary>
        public void Clear()
        {
            isRoot = isBookmarked = isFavourite = thereAreKeywords = false;
            Tip = "";
            id = -1;
        }

        /// <summary>
        /// Собирает строчку для всплывающей подсказки
        /// </summary>
        public void BuildToolTip()
        {
            Tip = "";
            if (isBookmarked) Tip += "Добавлено в закладки." + Environment.NewLine;
            if (isFavourite) Tip += "Добавлено в избранное." + Environment.NewLine;
            var s = from row in DATABASE.keywords.AsEnumerable() select row.Field<string>("keyword");
            if (s.Count() != 0)
                Tip += "Ключевые слова: " + Environment.NewLine + " -" + string.Join($"{Environment.NewLine} -", s);
        }
    }
}
