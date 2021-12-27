using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Windows.Forms;
using System.Data;
using System.IO;


namespace simple_database
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
            cmd.Parameters.Add("@data", DbType.Binary);
        }

        /// <summary>
        /// Создает необходимые папки по умолчанию.
        /// </summary>
        private void CreateFolders()
        {
            if (!Directory.Exists($@"{Application.StartupPath}\databases"))
                Directory.CreateDirectory($@"{Application.StartupPath}\databases");
            if (!Directory.Exists($@"{Application.StartupPath}\temp"))
                Directory.CreateDirectory($@"{Application.StartupPath}\temp");
        }

        /// <summary>
        /// Создать базу данных с инициализацией всех таблиц и связей.
        /// Если файд БД существует он будет перезаписан.
        /// Соединение с БД не будет открыто.
        /// </summary>
        /// <param name="fileName">Имя файла БД без разрешения.</param>
        public void Create(string fileName)
        {
            CreateFolders();

            SQLiteConnection cn = new SQLiteConnection();
            SQLiteCommand c = new SQLiteCommand(cn);
            cn.ConnectionString = $@"Data Source={Application.StartupPath}\databases\{fileName}.sqlite; foreign keys=true; nolock=1; auto_vacuum=1, version=3;";
            cn.Open();

            c.CommandText = "DROP TABLE IF EXISTS classes";
            c.ExecuteNonQuery();

            c.CommandText = @"CREATE TABLE classes (id INTEGER PRIMARY KEY AUTOINCREMENT,
                                                name TEXT NOT NULL UNIQUE,
                                                description TEXT DEFAULT NULL)";
            c.ExecuteNonQuery();

            c.CommandText = "DROP TABLE IF EXISTS properties";
            c.ExecuteNonQuery();

            c.CommandText = @"CREATE TABLE properties (id INTEGER PRIMARY KEY AUTOINCREMENT,
						name TEXT NOT NULL,
						description TEXT DEFAULT NULL,
						type INTEGER NOT NULL,
						parent INTEGER,
						class INTEGER NOT NULL,
						FOREIGN KEY(parent) REFERENCES properties(id) ON DELETE CASCADE,
						FOREIGN KEY(class) REFERENCES classes(id) ON DELETE CASCADE)";
            c.ExecuteNonQuery();

            c.CommandText = "DROP TABLE IF EXISTS attachments";
            c.ExecuteNonQuery();

            c.CommandText = @"CREATE TABLE attachments (id INTEGER PRIMARY KEY AUTOINCREMENT,
                        property INTEGER NOT NULL,
                        filename TEXT NOT NULL,
                        data BLOB,
                        FOREIGN KEY(property) REFERENCES properties(id) ON DELETE CASCADE)";
            c.ExecuteNonQuery();

            cn.Close();
        }

        /// <summary>
        /// Пытается открыть БД. Если БД не существует создает ее.
        /// Соединение с БД будет открыто.
        /// </summary>
        /// <param name="fileName">Имя файла БД без расширения и пути.</param>
        public void OpenOrCreate(string fileName)
        {
            Close();

            CreateFolders();

            FileName = $@"{Application.StartupPath}\databases\{fileName}.sqlite";
            conn.ConnectionString = $@"Data Source={FileName}; foreign keys=true; nolock=1; auto_vacuum=1, version=3;";

            if (!File.Exists(FileName))
            {
                Create(fileName);
            }

            conn.Open();
        }

        /// <summary>
        /// Закрывает текущее соединение с БД.
        /// </summary>
        public void Close()
        {
            if (conn?.State == ConnectionState.Open) conn?.Close();
        }

        /// <summary>
        /// Уплотниь базу данных
        /// </summary>
        public void Vacuum()
        {
            ExecSql("VACUUM");
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
                                    SELECT id, parent, name, type FROM cte ORDER BY type, name");
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

        /// <summary>
        /// Меняет родителя параметра.
        /// </summary>
        /// <param name="id">ROWID параметра</param>
        /// <param name="new_parent_id">Новое значение родителя.</param>
        public void ChangePropertyParent(long id, long new_parent_id)
        {
            ExecSql($"UPDATE properties SET parent={(new_parent_id >= 0 ? new_parent_id.ToString() : "NULL")} WHERE id={id}");
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
        /// Добавляет вложение в БД в виде бинарных данных. В качестве имени файла будет сохранено только имя и расширение.
        /// Путь будет удален.
        /// </summary>
        /// <param name="fileName">Полное имя файла с путем.</param>
        /// <returns>ROWID созданного свойства.</returns>
        public long AttachmentInsert(long id, long class_id, string name, long type, long parent_id, string description, string fileName)
        {
            SQLiteTransaction trans = conn.BeginTransaction();
            long rowid = -1;

            try
            {
                rowid = SaveProperty(id, class_id, name, type, parent_id, description);

                cmd.CommandText = $@"INSERT INTO attachments (property, filename, data)
                                       VALUES({rowid}, '{Path.GetFileName(fileName)}', @data)";
                cmd.Parameters["@data"].Value = File.ReadAllBytes(fileName);
                cmd.ExecuteNonQuery();
                cmd.Parameters["@data"].Value = null;
                trans.Commit();
            }
            catch
            {
                trans.Rollback();
            }
            
            return rowid;
        }

        /// <summary>
        /// Сохраняет вложение по указанному пути
        /// </summary>
        /// <param name="property_id">ID вложения</param>
        /// <param name="fileName">Полный путь для сохранения</param>
        /// <returns>True, если файла распакован иначе False</returns>
        public bool AttachmentExtract(long property_id, string fileName)
        {
            SqlRows rows = ExecSqlReturn("SELECT data FROM attachments WHERE property=" + property_id.ToString());
            try
            {
                File.WriteAllBytes(fileName, (byte[])rows[0]["data"]);
            }
            catch
            {
                MessageBox.Show("Ошибка при сохранении файла. Возможно существующий файл заблокирован для удаления.", "",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Возвращает имя файла вложения по его ID
        /// </summary>
        /// <param name="property_id">ID вложения</param>
        /// <returns>Имя файла без пути или null, если вложения не существует</returns>
        public string AttachmentGetFilename(long property_id)
        {
            SqlRows rows = ExecSqlReturn("SELECT filename, data FROM attachments WHERE property=" + property_id.ToString());
            if (rows.Count == 0) return null;
            return rows[0]["filename"].ToString();
        }

        /// <summary>
        /// 
        /// 
        /// 
        ///
        /// </summary>
        /// <param name="text">Тект для экранирования.</param>
        /// <returns>Текст для вставки в SQL запрос.</returns>
        public string Escape(string text)
        {
            return text.Replace("'", "''");
        }
    }
}
