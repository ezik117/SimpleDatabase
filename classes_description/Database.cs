using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Windows.Forms;
using System.Data;
using System.IO;


namespace classes_description
{
    using SqlRows = List<Dictionary<string, object>>;

    public class Database
    {
        /// <summary>
        /// Объект подключения к БД.
        /// </summary>
        private SQLiteConnection conn;

        /// <summary>
        /// Объект для выполнения запросов SQL.
        /// </summary>
        private SQLiteCommand cmd;

        /// <summary>
        /// Имя файла БД и полным путем.
        /// </summary>
        public string FileName { get; private set; }

        // *** PUBLIC мЕТОДЫ ****************************************

        /// <summary>
        /// Конструктор.
        /// </summary>
        public Database()
        {
            conn = new SQLiteConnection();
            cmd = new SQLiteCommand(conn);
        }

        /// <summary>
        /// Создать базу данных с инициализацией всех таблиц и связей. БД будет создана в текущей папке сборки.
        /// </summary>
        /// <param name="fileName">Имя файла БД без разрешения.</param>
        public void Create(string fileName)
        {
            Close();
            FileName = $@"{ Application.StartupPath}\{ fileName}.sqlite";
            conn.ConnectionString = $@"Data Source={Application.StartupPath}\{fileName}.sqlite; foreign keys=true; nolock=1; version=3;";
            conn.Open();

            //ExecSql("PRAGMA foreign_keys = ON");

            ExecSql("DROP TABLE IF EXISTS classes");
            ExecSql(@"CREATE TABLE classes (id INTEGER PRIMARY KEY AUTOINCREMENT,
                                                name TEXT NOT NULL UNIQUE,
                                                description TEXT DEFAULT NULL)");

            ExecSql("DROP TABLE IF EXISTS properties");
            ExecSql(@"CREATE TABLE properties (id INTEGER PRIMARY KEY AUTOINCREMENT,
						name TEXT NOT NULL,
						description TEXT DEFAULT NULL,
						type INTEGER NOT NULL,
						parent INTEGER,
						class INTEGER NOT NULL,
						FOREIGN KEY(parent) REFERENCES properties(id) ON UPDATE CASCADE ON DELETE CASCADE,
						FOREIGN KEY(class) REFERENCES classes(id) ON UPDATE CASCADE ON DELETE CASCADE)");
        }

        /// <summary>
        /// Пытается открыть БД. Если БД не существует создает ее.
        /// </summary>
        /// <param name="fileName">Имя файла БД без разрешения.</param>
        public void OpenOrCreate(string fileName)
        {
            Close();
            FileName = $@"{ Application.StartupPath}\{ fileName}.sqlite";
            conn.ConnectionString = $@"Data Source={Application.StartupPath}\{fileName}.sqlite; nolock=1; foreign keys=true; version=3;";

            if (File.Exists(FileName))
            {
                conn.Open();
                //ExecSql("PRAGMA foreign_keys = ON"); // это значение выключено каждый раз при входе в БД
            }
            else
            {
                Create(fileName);
            }
        }

        /// <summary>
        /// Закрывает текущее соединение с БД.
        /// </summary>
        public void Close()
        {
            if (conn?.State == ConnectionState.Open) conn?.Close();
        }

        /// <summary>
        /// Сохраняет имя и описание класса (обновляет или создает запись в БД).
        /// </summary>
        /// <param name="id">ROWID записи класса.</param>
        /// <param name="className">Имя класса.</param>
        /// <param name="description">Описание класса.</param>
        /// <returns>ROWID записи класса.</returns>
        public long SaveClass(long id, string className, string description)
        {
            long ret = id;
            if (ExecSql($@"UPDATE classes SET name='{className}', description='{description}' WHERE id={id}") == 0)
            {
                ExecSql($@"INSERT INTO classes (name, description) VALUES('{className}', '{description}')");
                ret = conn.LastInsertRowId;
            }

            return ret;
        }

        /// <summary>
        /// Загружает все записи классов.
        /// </summary>
        /// <returns>Ссылка на таблицу с классами.</returns>
        public SqlRows LoadClasses()
        {
            return ExecSqlReturn("SELECT * FROM classes");
        }

        /// <summary>
        /// Возвращает запись классе.
        /// </summary>
        /// <param name="id">ROWID записи класса.</param>
        /// <returns>Ссылка на таблицу с классом.</returns>
        public SqlRows LoadClass(long id)
        {
            return ExecSqlReturn("SELECT * FROM classes WHERE id=" + id.ToString());
        }

        /// <summary>
        /// Удаляет запись класса.
        /// </summary>
        /// <param name="id">ROWID записи класса.</param>
        public void DeleteClass(long id)
        {
            ExecSql("DELETE FROM classes WHERE id=" + id.ToString());
        }

        /// <summary>
        /// Считывает запись свойства.
        /// </summary>
        /// <param name="id">ROWID свойства.</param>
        /// <returns></returns>
        public SqlRows LoadProperty(long id)
        {
            return ExecSqlReturn("SELECT * FROM properties WHERE id=" + id.ToString());
        }


        /// <summary>
        /// Загружает рекурсивно иерархическую структуру записей свойств.
        /// </summary>
        /// <param name="class_id">ROWID записи класса.</param>
        /// <returns>Множество записей.</returns>
        public SqlRows LoadProperties(long class_id)
        {
             return ExecSqlReturn($@"WITH cte AS (
	                                    SELECT id, parent, name, type FROM properties WHERE parent IS NULL AND class={class_id}
                                      UNION ALL
	                                    SELECT properties.id, properties.parent, properties.name, properties.type
	                                    FROM cte, properties on properties.parent = cte.id
                                    )
                                    SELECT id, parent, name, type FROM cte");
        }

        /// <summary>
        /// Соохраняет значение свойства (обновляет или создает запись в БД).
        /// </summary>
        /// <param name="id">ROWID параметра.</param>
        /// <param name="class_id">Ссылка на ROWID класса.</param>
        /// <param name="name">Название параметра.</param>
        /// <param name="type">Тип (номер в ImageList) параметра.</param>
        /// <param name="parent_id">Ссылка на родительский параметр (ROWID).</param>
        /// <param name="description">Описание параметра.</param>
        /// <returns></returns>
        public long SaveProperty(long id, long class_id, string name, long type, long parent_id, string description)
        {
            long ret = id;
            if (ExecSql($@"UPDATE properties SET name='{name}',
                                                 type={type},
                                                 parent={(parent_id == -1 ? "NULL" : parent_id.ToString())},
                                                 class={class_id},
                                                 description='{description}'
                                             WHERE id={id}") == 0)
            {
                ExecSql($@"INSERT INTO properties (name, type, parent, class, description)
                           VALUES('{name}', {type}, {(parent_id == -1 ? "NULL" : parent_id.ToString())}, {class_id}, '{description}')");
                ret = conn.LastInsertRowId;
            }

            return ret;
        }

        /// <summary>
        /// Удаляет запись параметра в БД.
        /// </summary>
        /// <param name="id">ROWID параметра.</param>
        public void DeleteProperty(long id)
        {
            ExecSql("DELETE FROM properties WHERE id=" + id.ToString());
        }

        /// <summary>
        /// Удаляет все параметры класса.
        /// </summary>
        /// <param name="class_id"></param>
        public void DeleteProperties(long class_id)
        {
            ExecSql("DELETE FROM properties WHERE class=" + class_id.ToString());
        }

        /// <summary>
        /// Обновляет описание к параметру.
        /// </summary>
        /// <param name="id">ROWDI параметра.</param>
        /// <param name="description">Описание параметра.</param>
        public void UpdatePropertyDescription(long id, string description)
        {
            ExecSql($"UPDATE properties SET description='{description}' WHERE id={id}");
        }

        // *** PRIVATE мЕТОДЫ ***************************************

        /// <summary>
        /// Выполнить запрос SQL без возврата результатов.
        /// </summary>
        /// <param name="query">Строка запроса.</param>
        /// <returns>Количество обработанных строк.</returns>
        private int ExecSql(string query)
        {
            cmd.CommandText = query;
            return cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Выполнить команду SQL и вернуть всю запись как массив именованных пар: столбец-значение.
        /// </summary>
        /// <param name="query">Запрос SQL.</param>
        /// <returns>Список (массив) записей с полями вида столбец=значение.</returns>
        private SqlRows ExecSqlReturn(string query)
        {
            cmd.CommandText = query;
            SQLiteDataReader dr = cmd.ExecuteReader();
            SqlRows result = new SqlRows();
            while (dr.Read())
            {
                result.Add(Enumerable.Range(0, dr.FieldCount).ToDictionary(dr.GetName, dr.GetValue));
            }
            dr.Close();
            return result;
        }


        /// <summary>
        /// Выполнить команду SQL и вернуть первое значение, если есть.
        /// </summary>
        /// <param name="query">Запрос SQL.</param>
        /// <returns>Первое значение возвращенных данных, если пусто, возвращается NULL.</returns>
        private object ExecSqlSingle(string query)
        {
            cmd.CommandText = query;
            try
            {
                return cmd.ExecuteScalar();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Экранирует одиночные кавычки.
        /// </summary>
        /// <param name="text">Тект для экранирования.</param>
        /// <returns>Текст для вставки в SQL запрос.</returns>
        public string Escape(string text)
        {
            return text.Replace("'", "''");
        }
    }
}
