using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Drawing;

namespace simple_database
{
    using SqlRows = List<Dictionary<string, object>>;

    public static class DATABASE
    {
        /// <summary>
        /// Максимальное количество записей в таблице истории
        /// </summary>
        public static readonly int HISTORY_MAX_ROWS = 100;

        /// <summary>
        /// Объект подключения к БД.
        /// </summary>
        private static SQLiteConnection conn;

        /// <summary>
        /// Объект для выполнения запросов SQL.
        /// </summary>
        private static SQLiteCommand cmd;

        /// <summary>
        /// DataAdapter
        /// </summary>
        private static SQLiteDataAdapter da;

        /// <summary>
        /// Объект подключения к БД содержащей информацию о всех БД
        /// </summary>
        private static SQLiteConnection dblist_conn;

        /// <summary>
        /// Объект для выполнения запросов SQL содержащей информацию о всех БД
        /// </summary>
        private static SQLiteCommand dblist_cmd;

        /// <summary>
        /// Имя файла БД и полным путем.
        /// </summary>
        public static string FileName { get; private set; }

        /// <summary>
        /// Для хранения разных наборов данных
        /// </summary>
        public static DataSet ds = new DataSet();

        /// <summary>
        /// Информация об ошибке при вызове функции (работает не для всех функций)
        /// </summary>
        public static string LastError = "";

        /// <summary>
        /// Таблица для хранения закладок (можно также обратиться через "ds").\n
        /// (S)database, (S)class, (L)class_id, (S)property, (L)property_id
        /// </summary>
        public static DataTable bookmarks;

        /// <summary>
        /// Таблица для хранения избранного (можно также обратиться через "ds").\n
        /// (S)database, (S)class, (L)class_id, (S)property, (L)property_id
        /// </summary>
        public static DataTable favourites;

        /// <summary>
        /// Таблица для хранения ключевых слов текущего выбранного узла
        /// </summary>
        public static DataTable keywords;


        // *** PUBLIC мЕТОДЫ ****************************************

        /// <summary>
        /// Конструктор.
        /// </summary>
        public static void Init()
        {
            CreateFolders();

            conn = new SQLiteConnection();
            cmd = new SQLiteCommand(conn);

            dblist_conn = new SQLiteConnection();
            dblist_cmd = new SQLiteCommand(dblist_conn);

            cmd.Parameters.Add("@data", DbType.Binary);

            // добавление переменных для запросов
            cmd.Parameters.Add("@className", DbType.String).IsNullable = true;
            cmd.Parameters.Add("@class_id", DbType.Int64).IsNullable = true;
            cmd.Parameters.Add("@class_id2", DbType.Int64).IsNullable = true;
            cmd.Parameters.Add("@name", DbType.String).IsNullable = true;
            cmd.Parameters.Add("@property_id", DbType.Int64).IsNullable = true;
            cmd.Parameters.Add("@description", DbType.String).IsNullable = true;
            cmd.Parameters.Add("@id", DbType.Int64).IsNullable = true;
            cmd.Parameters.Add("@new_parent_id", DbType.Int64).IsNullable = true;
            cmd.Parameters.Add("@type", DbType.Int64).IsNullable = true;
            cmd.Parameters.Add("@parent_id", DbType.Int64).IsNullable = true;
            cmd.Parameters.Add("@rowid", DbType.Int64).IsNullable = true;
            cmd.Parameters.Add("@fileName", DbType.String).IsNullable = true;
            cmd.Parameters.Add("@lastupdate", DbType.String).IsNullable = true;
            cmd.Parameters.Add("@keyword", DbType.String);
            cmd.Parameters.Add("@keyword_id", DbType.Int64);
            cmd.Parameters.Add("@count", DbType.Int64);
            cmd.Parameters.Add("@stringData", DbType.String); // universal data holder

            cmd.Parameters.Add("@user", DbType.String).IsNullable = true;
            cmd.Parameters.Add("@tablename", DbType.String).IsNullable = true;
            cmd.Parameters.Add("@change", DbType.String).IsNullable = true;
            cmd.Parameters.Add("@path", DbType.String).IsNullable = true;

            bookmarks = ds.Tables.Add("bookmarks");
            bookmarks.Columns.Add("database", typeof(string));
            bookmarks.Columns.Add("class", typeof(string));
            bookmarks.Columns.Add("class_id", typeof(long));
            bookmarks.Columns.Add("property", typeof(string));
            bookmarks.Columns.Add("property_id", typeof(long));

            favourites = ds.Tables.Add("favourites");

            ds.Tables.Add("history");

            keywords = ds.Tables.Add("keywords");

            // открытие специальной БД о всех БД
            dblist_conn.ConnectionString = $@"Data Source={Application.StartupPath}\databases\databases.sqlite; foreign keys=true; nolock=1; auto_vacuum=1, version=3;";
            dblist_conn.Open();

            dblist_cmd.Parameters.Add("@name", DbType.String).IsNullable = true;
            dblist_cmd.Parameters.Add("@oldName", DbType.String).IsNullable = true;
            dblist_cmd.Parameters.Add("@iconFilename", DbType.String).IsNullable = true;
            dblist_cmd.Parameters.Add("@selected", DbType.Int64).IsNullable = true;
            dblist_cmd.Parameters.Add("@icon", DbType.Binary).IsNullable = true;

            dblist_cmd.CommandText = @"CREATE TABLE IF NOT EXISTS databases (
                                name TEXT,
                                icon BLOB DEFAULT NULL,
                                icon_filename TEXT,
                                selected INTEGER DEFAULT 0,
                                creation_date DATETIME)";
            dblist_cmd.ExecuteNonQuery();

        }

        /// <summary>
        /// Создает необходимые папки по умолчанию.
        /// </summary>
        private static void CreateFolders()
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
        public static void Create(string fileName)
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

            c.CommandText = "DROP TABLE IF EXISTS settings";
            c.ExecuteNonQuery();

            c.CommandText = @"CREATE TABLE settings (
                        key TEXT NOT NULL,
                        value TEXT)";
            c.ExecuteNonQuery();

            string lastupdate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            c.CommandText = $"INSERT INTO settings VALUES('LASTUPDATE','{lastupdate}')";
            c.ExecuteNonQuery();

            c.Dispose();
            cn.Close();
        }

        /// <summary>
        /// Выполняет запрос к БД и возвращает одиночный результат
        /// </summary>
        /// <param name="query">Строка SQL запроса</param>
        /// <returns>Объект запроса</returns>
        public static object GetSingleValue(string query)
        {
            cmd.CommandText = query;
            return cmd.ExecuteScalar();
        }

        /// <summary>
        /// Записывает в БД дату последнего сохранения записи.
        /// </summary>
        /// <returns>Возвращает дату последнего сохранения записи в виде строки</returns>
        public static string SetLastUpdate()
        {
            string lastupdate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            cmd.CommandText = "UPDATE settings SET value=@lastupdate WHERE key='LASTUPDATE'";
            cmd.Parameters["@lastupdate"].Value = lastupdate;
            cmd.ExecuteNonQuery();
            return lastupdate;
        }

        /// <summary>
        /// Возвращает дату последнего сохранения записи в виде строки.
        /// Если таблица settings не существует или нет ключа LASTUPDATE пытается создать
        /// таблицу и записать текущую дату. В случае ошибки создания возвращает пустое значение.
        /// </summary>
        /// <returns>Дата последнего сохранения записи в виде строки</returns>
        public static string GetLastUpdate()
        {
            string lastupdate = "";
            try
            {
                cmd.CommandText = "SELECT value FROM settings WHERE key='LASTUPDATE'";
                object o = cmd.ExecuteScalar();
                if (o == null) throw new Exception();
                lastupdate = (o == null ? "" : (string)o);
            }
            catch
            {
                try
                {
                    cmd.CommandText = @"CREATE TABLE IF NOT EXISTS settings (
                        key TEXT NOT NULL,
                        value TEXT)";
                    cmd.ExecuteNonQuery();

                    lastupdate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    cmd.CommandText = $"INSERT OR REPLACE INTO settings VALUES('LASTUPDATE','{lastupdate}')";
                    cmd.ExecuteNonQuery();
                }
                catch
                {
                    lastupdate = "";
                }
            }

            return lastupdate;
        }

        /// <summary>
        /// Возвращает размер открытой БД в байтах.
        /// БД должна быть открыта!
        /// </summary>
        /// <returns>Размер в байтах</returns>
        public static long GetOpenedDatabaseSize()
        {
            long ret = 0;
            if (conn != null && conn.State == ConnectionState.Open)
            {
                FileInfo fi = new FileInfo(FileName);
                ret = fi.Length;
            }

            return ret;
        }

        /// <summary>
        /// Пытается открыть БД. Если БД не существует создает ее.
        /// Соединение с БД будет открыто.
        /// </summary>
        /// <param name="fileName">Имя файла БД без расширения и пути.</param>
        public static void OpenOrCreate(string fileName)
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
            da = new SQLiteDataAdapter("SELECT * FROM favourites ORDER BY ", conn);

            // добавляем таблицы которые должны быть, но их может еще нет
            cmd.CommandText = @"CREATE TABLE IF NOT EXISTS favourites (
                                class TEXT,
                                class_id INTEGER,
                                property_id INTEGER,
                                FOREIGN KEY(class_id) REFERENCES classes(id) ON DELETE CASCADE,
                                FOREIGN KEY(property_id) REFERENCES properties(id) ON DELETE CASCADE)";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "CREATE TABLE IF NOT EXISTS keywords (keyword TEXT, class_id INTEGER)";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "CREATE TABLE IF NOT EXISTS keywords_binding (keyword_id INTEGER, property_id INTEGER)";
            cmd.ExecuteNonQuery();

            cmd.CommandText = @"CREATE TABLE IF NOT EXISTS history (
                                date DATETIME,
                                user TEXT,
                                class_id INTEGER,
                                row_id INTEGER,
                                tablename TEXT,
                                change TEXT,
                                name TEXT,
                                path TEXT)";
            cmd.ExecuteNonQuery();

            // загрузим избранное
            ReadFavourites();

            // загрузим синтаксические правила
            try
            {
                string xml = DATABASE.LoadSyntaxRules();
                HELPER.DeserializeSyntaxRules(xml, ref VARS.syntaxRules);
                if (VARS.syntaxRules == null)
                {
                    VARS.syntaxRules = new SyntaxRulesHolder();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Невозможно загрузить синтаксические правила. Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // добавим контестное меню подсвветки синтаксиса
            HELPER.AddPropertiesContextMenuItems();
        }

        /// <summary>
        /// Закрывает текущее соединение с БД.
        /// </summary>
        public static void Close()
        {
            if (conn?.State == ConnectionState.Open) conn?.Close();
        }

        /// <summary>
        /// Закрывает все активные соединения с БД и освобождает ресурсы
        /// </summary>
        public static void CloseAll()
        {
            conn?.Close();
            conn?.Dispose();
            cmd?.Dispose();
            dblist_conn?.Close();
            dblist_conn?.Dispose();
            dblist_cmd?.Dispose();
        }

        /// <summary>
        /// Уплотниь базу данных
        /// </summary>
        public static void Vacuum()
        {
            ExecSql("VACUUM");
        }

        /// <summary>
        /// Сохраняет имя и описание класса (обновляет или создает запись в БД).
        /// </summary>
        /// <param name="id">ROWID записи класса.</param>ChangeProperty
        /// <param name="className">Имя класса.</param>
        /// <param name="description">Описание класса.</param>
        /// <returns>ROWID записи класса.</returns>
        public static long SaveClass(long id, string className, string description)
        {
            long ret = id;
            cmd.CommandText = "UPDATE classes SET name=@className, description=@description WHERE id=@id";
            cmd.Parameters["@className"].Value = className;
            cmd.Parameters["@description"].Value = description;
            cmd.Parameters["@id"].Value = id;
            if (cmd.ExecuteNonQuery() == 0)
            {
                cmd.CommandText = "INSERT INTO classes (name, description) VALUES(@className, @description)";
                cmd.ExecuteNonQuery();
                ret = conn.LastInsertRowId;

                // сохраним в истории
                SaveHistory(ret, ret, "classes", "Создание каталога", className);
            }
            else
            {
                // сохраним в истории
                SaveHistory(id, id, "classes", "Изменение имени каталога", className);
            }

            return ret;
        }

        /// <summary>
        /// Загружает все записи классов.
        /// </summary>
        /// <returns>Ссылка на таблицу с классами.</returns>
        public static SqlRows LoadClasses()
        {
            cmd.CommandText = "SELECT * FROM classes";
            return ExecSqlReturn();
        }

        /// <summary>
        /// Возвращает запись классе.
        /// </summary>
        /// <param name="id">ROWID записи класса.</param>
        /// <returns>Ссылка на таблицу с классом.</returns>
        public static SqlRows LoadClass(long id)
        {
            cmd.CommandText = "SELECT * FROM classes WHERE id=@id";
            cmd.Parameters["@id"].Value = id;
            return ExecSqlReturn();
        }

        /// <summary>
        /// Удаляет запись класса.
        /// </summary>
        /// <param name="id">ROWID записи класса.</param>
        public static void DeleteClass(long id)
        {
            cmd.CommandText = "DELETE FROM classes WHERE id=@id";
            cmd.Parameters["@id"].Value = id;
            cmd.ExecuteNonQuery();

            // сохраним в истории
            SaveHistory(id, id, "classes", "Удаление каталога", VARS.main_form.tvClasses.SelectedNode.Text);
        }

        /// <summary>
        /// Считывает запись свойства.
        /// </summary>
        /// <param name="id">ROWID свойства.</param>
        /// <returns></returns>
        public static SqlRows LoadProperty(long id)
        {
            cmd.CommandText = "SELECT * FROM properties WHERE id=@id";
            cmd.Parameters["@id"].Value = id;
            return ExecSqlReturn();
        }


        /// <summary>
        /// Загружает рекурсивно иерархическую структуру записей свойств.
        /// </summary>
        /// <param name="class_id">ROWID записи класса.</param>
        /// <returns>Множество записей.</returns>
        public static SqlRows LoadProperties(long class_id)
        {
            cmd.CommandText = @"WITH cte AS (
                                    SELECT id, parent, name, type FROM properties WHERE parent IS NULL AND class=@class_id
                                  UNION ALL
                                    SELECT properties.id, properties.parent, properties.name, properties.type
                                    FROM cte, properties on properties.parent = cte.id
                                  )
                                  SELECT id, parent, name, type FROM cte";
            cmd.Parameters["@class_id"].Value = class_id;

            return ExecSqlReturn();
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
        /// <returns>ROWID вставленной строки</returns>
        public static long SaveProperty(long id, long class_id, string name, long type, long parent_id, string description)
        {
            long ret = id;
            cmd.CommandText = $@"UPDATE properties SET name=@name,
                                                 type=@type,
                                                 parent=@parent_id,
                                                 class=@class_id,
                                                 description=@description
                                 WHERE id=@id";
            cmd.Parameters["@name"].Value = name;
            cmd.Parameters["@type"].Value = type;
            if (parent_id == -1) cmd.Parameters["@parent_id"].Value = null; else cmd.Parameters["@parent_id"].Value =parent_id;
            cmd.Parameters["@class_id"].Value = class_id;
            cmd.Parameters["@description"].Value = description;
            cmd.Parameters["@id"].Value = id;

            if (cmd.ExecuteNonQuery() == 0)
            {
                cmd.CommandText = @"INSERT INTO properties (name, type, parent, class, description)
                                    VALUES(@name, @type, @parent_id, @class_id, @description)";
                cmd.ExecuteNonQuery();
                ret = conn.LastInsertRowId;

                // сохраним в истории
                SaveHistory(class_id, ret, "properties", "Создание элемента оглавления", name);
            }
            else
            {
                // сохраним в истории
                SaveHistory(class_id, id, "properties", "Изменение названия элемента оглавления", name);
            }

            

            return ret;
        }

        /// <summary>
        /// Обновляет значение свойства.
        /// </summary>
        /// <param name="id">ROWID параметра.</param>
        /// <param name="name">Название параметра.</param>
        public static void UpdatePropertyName(long id, string name)
        {
            cmd.CommandText = $@"UPDATE properties SET name=@name WHERE id=@id";
            cmd.Parameters["@name"].Value = name;
            cmd.Parameters["@id"].Value = id;
            cmd.ExecuteNonQuery();

            // сохраним в истории
            SaveHistory(-1, id, "properties", "Изменение названия элемента оглавления", name);
        }

        /// <summary>
        /// Удаляет запись параметра в БД.
        /// </summary>
        /// <param name="id">ROWID параметра.</param>
        public static void DeleteProperty(long id)
        {
            cmd.CommandText = "DELETE FROM properties WHERE id=@id";
            cmd.Parameters["@id"].Value = id;
            cmd.ExecuteNonQuery();

            // сохраним в истории
            SaveHistory(-1, id, "properties", "Удаление элемента оглавления", VARS.main_form.tvProps.SelectedNode.Text);
        }

        /// <summary>
        /// Удаляет все параметры класса.
        /// </summary>
        /// <param name="class_id"></param>
        public static void DeleteProperties(long class_id)
        {
            cmd.CommandText = "DELETE FROM properties WHERE class=@class_id";
            cmd.Parameters["@class_id"].Value = class_id;
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Обновляет описание к оглавлению.
        /// </summary>
        /// <param name="id">ROWDI параметра.</param>
        /// <param name="description">Описание параметра.</param>
        public static void UpdatePropertyDescription(long id, string description)
        {
            cmd.Parameters["@id"].Value = id;
            cmd.Parameters["@description"].Value = description;

            cmd.CommandText = "UPDATE properties SET description=@description WHERE id=@id";
            cmd.ExecuteNonQuery();

            // сохраним в истории
            SaveHistory(-1, id, "properties", "Обновление информации", VARS.main_form.tvProps.SelectedNode.Text);
        }

        /// <summary>
        /// Обновляет описание к каталогу.
        /// </summary>
        /// <param name="id">ROWDI параметра.</param>
        /// <param name="description">Описание параметра.</param>
        public static void UpdateClassDescription(long id, string description)
        {
            cmd.Parameters["@id"].Value = id;
            cmd.Parameters["@description"].Value = description;

            cmd.CommandText = "UPDATE classes SET description=@description WHERE id=@id";
            cmd.ExecuteNonQuery();

            // сохраним в истории
            SaveHistory(id, -1, "classes", "Обновление информации", VARS.main_form.tvClasses.SelectedNode.Text);
        }

        /// <summary>
        /// Меняет родителя параметра.
        /// </summary>
        /// <param name="id">ROWID параметра</param>
        /// <param name="new_parent_id">Новое значение родителя.</param>
        public static void ChangePropertyParent(long id, long new_parent_id)
        {
            cmd.CommandText = "UPDATE properties SET parent=@new_parent_id WHERE id=@id";
            cmd.Parameters["@id"].Value = id;
            if (new_parent_id >= 0) cmd.Parameters["@new_parent_id"].Value = new_parent_id; else cmd.Parameters["@new_parent_id"].Value = null;
            cmd.ExecuteNonQuery();

            // сохраним в истории
            SaveHistory(-1, id, "properties", "Перемещение элемента оглавления", VARS.main_form.tvProps.SelectedNode.Text);
        }

        // *** PRIVATE мЕТОДЫ ***************************************

        /// <summary>
        /// Выполнить запрос SQL без возврата результатов.
        /// </summary>
        /// <param name="query">Строка запроса.</param>
        /// <returns>Количество обработанных строк.</returns>
        private static int ExecSql(string query)
        {
            cmd.CommandText = query;
            return cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Выполнить команду SQL и вернуть всю запись как массив именованных пар: столбец-значение.
        /// </summary>
        /// <returns>Список (массив) записей с полями вида столбец=значение.</returns>
        private static SqlRows ExecSqlReturn()
        {
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
        private static object ExecSqlSingle(string query)
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
        /// Вложение может быть файлом вложения или плагином. В случае плагина считывается текст, который должен быть в 
        /// кодировке UTF-8 и сохраняется как массив байтов.
        /// </summary>
        /// <param name="id">В данном контексте всегда равно -1</param>
        /// <param name="class_id">ID элемента каталога</param>
        /// <param name="name">Пользовательское название вложения (или имя файла)</param>
        /// <param name="type">Тип вложения (индекс иконки в структуре IconTypes</param>
        /// <param name="parent_id">ID элемента оглавления</param>
        /// <param name="description">Текст содержимого</param>
        /// <param name="fileName">Полное имя файла с путем.</param>
        /// <returns>ROWID созданного свойства.</returns>
        public static long AttachmentInsert(long id, long class_id, string name, long type, long parent_id, string description, string fileName)
        {
            SQLiteTransaction trans = conn.BeginTransaction();
            long rowid = -1;

            try
            {
                rowid = SaveProperty(id, class_id, name, type, parent_id, description);

                cmd.CommandText = @"INSERT INTO attachments (property, filename, data)
                                    VALUES(@rowid, @fileName, @data)";
                cmd.Parameters["@rowid"].Value = rowid;
                cmd.Parameters["@fileName"].Value = Path.GetFileName(fileName);

                if (type == (int)IconTypes.Attachment)
                {
                    cmd.Parameters["@data"].Value = File.ReadAllBytes(fileName);
                }
                else if (type == (int)IconTypes.Plugin)
                {
                    cmd.Parameters["@data"].Value = Encoding.UTF8.GetBytes(File.ReadAllText(fileName));
                }

                cmd.ExecuteNonQuery();
                cmd.Parameters["@data"].Value = null;

                // сохраним в истории
                SaveHistory(class_id, conn.LastInsertRowId, "attachments", "Создание вложения", Path.GetFileName(fileName));

                trans.Commit();
            }
            catch
            {
                trans.Rollback();
            }
            
            return rowid;
        }

        /// <summary>
        /// Изменяет вложение в БД в виде бинарных данных. В качестве имени файла будет сохранено только имя и расширение.
        /// Путь будет удален.
        /// </summary>
        /// <param name="fileName">Полное имя файла с путем.</param>
        public static void AttachmentUpdate(long id, long class_id, string name, long type, long parent_id, string description, string fileName)
        {
            SQLiteTransaction trans = conn.BeginTransaction();

            try
            {
                SaveProperty(id, class_id, name, type, parent_id, description);

                cmd.Parameters["@rowid"].Value = id;
                cmd.Parameters["@fileName"].Value = Path.GetFileName(fileName);
                cmd.CommandText = @"UPDATE attachments SET filename=@fileName, data=@data
                                    WHERE property=@rowid";
                if (type == (int)IconTypes.Attachment)
                {
                    cmd.Parameters["@data"].Value = File.ReadAllBytes(fileName);
                }
                else if (type == (int)IconTypes.Plugin)
                {
                    cmd.Parameters["@data"].Value = Encoding.UTF8.GetBytes(File.ReadAllText(fileName));
                }

                cmd.ExecuteNonQuery();
                cmd.Parameters["@data"].Value = null;

                // сохраним в истории
                SaveHistory(class_id, id, "attachments", "Изменение вложения", Path.GetFileName(fileName));

                trans.Commit();
            }
            catch
            {
                trans.Rollback();
            }
        }

        /// <summary>
        /// Удалить вложение. Не используется, т.к. используется каскадное удаление
        /// </summary>
        /// <param name="id">ROWID вложения</param>
        /// <remarks>Не используется, т.к. используется каскадное удаление</remarks>
        public static void AttachmentDelete(long id)
        {
            cmd.Parameters["@rowid"].Value = id;
            cmd.CommandText = "DELETE FROM attachments WHERE id=@rowid";
            cmd.ExecuteNonQuery();

            // сохраним в истории
            SaveHistory(-1, id, "attachments", "Удаление вложения", VARS.main_form.tvProps.SelectedNode.Text);
        }

        /// <summary>
        /// Сохраняет вложение по указанному пути
        /// </summary>
        /// <param name="property_id">ID вложения</param>
        /// <param name="fileName">Полный путь для сохранения</param>
        /// <returns>True, если файла распакован иначе False</returns>
        public static bool AttachmentExtract(long property_id, string fileName)
        {
            cmd.CommandText = "SELECT data FROM attachments WHERE property = @property_id";
            cmd.Parameters["@property_id"].Value = property_id;
            SqlRows rows = ExecSqlReturn();
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
        public static string AttachmentGetFilename(long property_id)
        {
            cmd.CommandText = "SELECT filename, data FROM attachments WHERE property = @property_id";
            cmd.Parameters["@property_id"].Value = property_id;
            SqlRows rows = ExecSqlReturn();
            if (rows.Count == 0) return null;
            return rows[0]["filename"].ToString();
        }

        /// <summary>
        /// Возвращает текст выбранного параметра в нижнем регистре
        /// </summary>
        /// <param name="property_id">ROWDI параметра</param>
        /// <returns>Текст параметра в нижнем регистре</returns>
        public static string LoadText(long property_id)
        {
            RichTextBox rtb = new RichTextBox
            {
                Text = ""
            };
            cmd.Parameters["@property_id"].Value = property_id;
            cmd.CommandText = "SELECT description FROM properties WHERE id = @property_id";
            object o = cmd.ExecuteScalar();
            if (o != null)
                if (!Convert.IsDBNull(o))
                    rtb.Rtf = (string)o;
            return rtb.Text.ToLower();
        }

        /// <summary>
        /// Добавляет узел в избранное в БД
        /// </summary>
        /// <param name="class_id">ID каталога</param>
        /// <param name="property_id">ID оглавления</param>
        /// <param name="class_name">Имя каталога</param>
        public static void AddPropertyToFavourites(long class_id, long property_id, string class_name)
        {
            cmd.Parameters["@className"].Value = class_name;
            cmd.Parameters["@class_id"].Value = class_id;
            cmd.Parameters["@property_id"].Value = property_id;
            cmd.CommandText = "INSERT INTO favourites(class, class_id, property_id) VALUES(@className, @class_id, @property_id)";
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="class_id">ID каталога</param>
        /// <param name="property_id">ID оглавления</param>
        /// <returns></returns>
        public static void ReadFavourites()
        {
            string query = @"
                SELECT fvr.*,
                (
                WITH cte AS (
	                SELECT 1 AS num, id, parent, name AS path FROM properties WHERE id=fvr.property_id AND class=fvr.class_id
	                UNION ALL
	                SELECT num + 1, properties.id, properties.parent, properties.name || ' / ' || path FROM cte, properties ON cte.parent = properties.id
                )
                SELECT path FROM cte ORDER BY num DESC LIMIT 1
                ) AS property
                FROM favourites fvr";
            using (SQLiteDataAdapter da = new SQLiteDataAdapter(query, conn))
            {
                favourites.Clear();
                da.Fill(favourites);
            }
        }

        /// <summary>
        /// Удаляет избранное. Все или только выбранный элемент.
        /// </summary>
        /// <param name="all">Если True - удаляет все</param>
        /// <param name="class_id">ID каталога</param>
        /// <param name="property_id">ID оглавления</param>
        public static void DeleteFavourites(bool all, long class_id, long property_id)
        {
            if (all)
            {
                favourites.Clear();
                cmd.CommandText = "DELETE FROM favourites";
                cmd.ExecuteNonQuery();
            }
            else
            {
                DataRow[] rows = favourites.Select($"class_id={class_id} AND property_id={property_id}");
                if (rows.Count() > 0)
                {
                    rows[0].Delete();
                    favourites.AcceptChanges();
                }

                cmd.Parameters["@class_id"].Value = class_id;
                cmd.Parameters["@property_id"].Value = property_id;
                cmd.CommandText = "DELETE FROM favourites WHERE class_id=@class_id AND property_id=@property_id";
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Сохраняет ключевые слова для свойства
        /// </summary>
        /// <param name="id">ROWID параметра</param>
        /// <param name="keywords">Список ключевых слов</param>
        /// <returns>True, если удалось сохранить, иначе False</returns>
        public static bool SaveParamKeywords(long property_id, DataTable keywords)
        {
            bool ret = true;
            SQLiteTransaction trans = conn.BeginTransaction();

            try
            {
                //, long keyword_id
                cmd.Parameters["@property_id"].Value = property_id;

                // вначале удалим все связанные с данным элементом оглавления ключевые слова
                cmd.CommandText = "DELETE FROM keywords_binding WHERE property_id=@property_id";
                cmd.ExecuteNonQuery();

                // теперь добавим все ключевые слова для данного элемента оглавления
                foreach (DataRow dr in keywords.Rows)
                {
                    cmd.Parameters["@keyword_id"].Value = (long)dr["ROWID"];
                    cmd.CommandText = "INSERT INTO keywords_binding VALUES (@keyword_id, @property_id)";
                    cmd.ExecuteNonQuery();
                }

                trans.Commit();
            }
            catch
            {
                trans.Rollback();
                ret = false;
            }

            return ret;
        }

        /// <summary>
        /// Считывает ключевые слова для свойства
        /// </summary>
        /// <param name="id">ROWID параметра</param>
        /// <returns>Список ключевых слов</returns>
        public static void ReadParamKeywords(long id, ref DataTable dt)
        {
            dt.Clear();
            cmd.Parameters["@id"].Value = id;
            cmd.CommandText = "SELECT kw.ROWID, kw.keyword FROM keywords_binding kb, keywords kw WHERE kb.property_id = @id AND kb.keyword_id = kw.ROWID";
            da.SelectCommand = cmd;
            da.Fill(dt);
        }

        /// <summary>
        /// Ищет все элементы оглавления с заданными ключевыми словами
        /// </summary>
        /// <param name="keywords_ids"></param>
        /// <returns></returns>
        public static List<long> FindParamsWithKeywords(string keywords_ids)
        {
            List<long> ret = new List<long>();

            cmd.Parameters["@count"].Value = keywords_ids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Length;

            cmd.CommandText = $@"SELECT property_id FROM
                                (SELECT property_id, COUNT(property_id) as cnt
                                  FROM keywords_binding WHERE keyword_id IN ({keywords_ids})
                                  GROUP BY property_id) WHERE cnt=@count";
            SQLiteDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
                ret.Add((long)dr["property_id"]);
            dr.Close();

            return ret;
        }

        /// <summary>
        /// Возвращает список всех ключевых слов текущей БД
        /// </summary>
        /// <param name="class_id">ROWID каталога</param>
        /// <param name="dt">Таблица куда будут переданы все результаты</param>
        public static void ReadAllKeywords(long class_id, ref DataTable dt)
        {
            cmd.Parameters["@class_id"].Value = class_id;
            cmd.CommandText = "SELECT ROWID, keyword FROM keywords WHERE class_id=@class_id ORDER BY keyword";
            da.SelectCommand = cmd;
            da.Fill(dt);
        }

        /// <summary>
        /// Добавляет в БД новое ключевое слово, если оно не существует
        /// </summary>
        /// <param name="keyword">Ключевое слово</param>
        /// <param name="class_id">ROWID каталога</param>
        /// <returns>-1, если слово не было добавлено, потому что уже существует, или ROWID новой записи</returns>
        public static long NewKeywordAdd(string keyword, long class_id)
        { 
            long ret = -1;
            cmd.Parameters["@keyword"].Value = keyword;
            cmd.Parameters["@class_id"].Value = class_id;

            cmd.CommandText = "SELECT ROWID FROM keywords WHERE keyword=@keyword AND class_id=@class_id";
            object o = cmd.ExecuteScalar();
            if (o == null || o == DBNull.Value)
            {
                cmd.CommandText = "INSERT INTO keywords (keyword, class_id) VALUES(@keyword, @class_id)";
                cmd.ExecuteNonQuery();
                ret = conn.LastInsertRowId;
            }

            return ret;
        }

        /// <summary>
        /// Добавляет в БД новое ключевое слово, если оно не существует
        /// </summary>
        /// <param name="keyword_id">ROWID ключевого слова</param>
        /// <param name="class_id">ROWID каталога</param>
        /// <param name="keyword">Ключевое слово</param>
        /// <returns>False, если слово не было добавлено, потому что уже существует, или True</returns>
        public static bool KeywordEdit(long keyword_id, long class_id, string keyword)
        {
            bool ret = true;

            cmd.Parameters["@keyword_id"].Value = keyword_id;
            cmd.Parameters["@class_id"].Value = class_id;
            cmd.Parameters["@keyword"].Value = keyword;

            cmd.CommandText = "SELECT ROWID FROM keywords WHERE keyword=@keyword AND class_id=@class_id";
            object o = cmd.ExecuteScalar();
            if (o == null || o == DBNull.Value)
            {
                cmd.CommandText = "UPDATE keywords SET keyword=@keyword WHERE ROWID=@keyword_id AND class_id=@class_id";
                cmd.ExecuteNonQuery();
            }
            else
            {
                ret = false;
            }

            return ret;
        }

        /// <summary>
        /// Удаляет из БД ключевое слово, а также все его вхождения во всех элементах оглавления
        /// </summary>
        /// <param name="keyword_id">ROWID ключевого слова</param>
        /// <param name="class_id">ROWID каталога</param>
        /// <returns>False, если удаление прошло с ошибкой, или True</returns>
        public static bool KeywordRemove(long keyword_id, long class_id)
        {
            bool ret = true;
            SQLiteTransaction trans = conn.BeginTransaction();

            cmd.Parameters["@keyword_id"].Value = keyword_id;
            cmd.Parameters["@class_id"].Value = class_id;
            
            try
            {
                cmd.CommandText = "DELETE FROM keywords_binding WHERE keyword_id=@keyword_id";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "DELETE FROM keywords WHERE ROWID=@keyword_id";
                cmd.ExecuteNonQuery();

                trans.Commit();
            }
            catch
            {
                ret = false;
                trans.Rollback();
            }

            return ret;
        }

        /// <summary>
        /// Запиывает в таблицу истории последнее изменение.
        /// Удаляет старые строки меньше количества допустимых в истории
        /// </summary>
        /// <param name="class_id">ROWID каталога</param>
        /// <param name="property_id">ROWID оглавления</param>
        /// <param name="tablename">Имя изменяемой таблицы</param>
        /// <param name="change_type">Текст типа изменения. Должно начинаться с "Изменение, Обновление, Удаление, Создание"</param>
        /// <param name="name">Новое название изменяемого элемента</param>
        public static void SaveHistory(long class_id, long property_id, string tablename, string change_type, string name)
        {
            cmd.Parameters["@class_id"].Value = class_id < 0 ? (long)VARS.main_form.tvClasses.SelectedNode.Tag : class_id;
            cmd.Parameters["@id"].Value = property_id;
            cmd.Parameters["@change"].Value = change_type;
            cmd.Parameters["@tablename"].Value = tablename;
            cmd.Parameters["@user"].Value = Environment.GetEnvironmentVariable("username");
            cmd.Parameters["@name"].Value = name;
            TreeNode node = VARS.main_form.tvProps.SelectedNode;
            cmd.Parameters["@path"].Value = node == null ? "" : PROPERTY.BuildFullNodePath(node);
            cmd.CommandText = "INSERT INTO history VALUES(DATETIME('now', 'localtime'), @user, @class_id, @id, @tablename, @change, @name, @path)";
            cmd.ExecuteNonQuery();

            while (true)
            {
                cmd.CommandText = "SELECT count(*) FROM history";
                long count = (long)cmd.ExecuteScalar();

                if (count > HISTORY_MAX_ROWS)
                {
                    cmd.CommandText = "DELETE FROM history WHERE ROWID = (SELECT ROWID FROM history WHERE date = (SELECT MIN(date) FROM history));";
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    break;
                }

                if (count - 1 == HISTORY_MAX_ROWS) break;
            }
        }

        /// <summary>
        /// Загрузить историю
        /// </summary>
        public static void LoadHistory()
        {
            using (SQLiteDataAdapter da = new SQLiteDataAdapter("SELECT * FROM history ORDER BY date DESC", conn))
            {
                ds.Tables["history"].Clear();
                da.Fill(ds.Tables["history"]);
            }
        }

        /// <summary>
        /// Создает запись о новой базе данных в таблице 'databases'
        /// </summary>
        /// <param name="name">Имя создаваемой БД - файл без пути и расширения</param>
        public static void CreateDatabaseRecord(string name)
        {
            dblist_cmd.Parameters["@name"].Value = name;
            dblist_cmd.CommandText = "INSERT INTO databases VALUES(@name, NULL, NULL, 0, DATETIME('now', 'localtime'))";
            dblist_cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Удаляет запись о базе данных в таблице 'databases'
        /// </summary>
        /// <param name="name">Имя удаляемой БД - файл без пути и расширения</param>
        public static void DeleteDatabaseRecord(string name)
        {
            dblist_cmd.Parameters["@name"].Value = name;
            dblist_cmd.CommandText = "DELETE FROM databases WHERE name=@name";
            dblist_cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Изменяет название базы данных в таблице 'databases'
        /// </summary>
        /// <param name="name">Имя БД - файл без пути и расширения</param>
        public static void ChangeDatabaseRecordName(string old_name, string new_name)
        {
            if (CheckIfDatabaseRecordExists(old_name))
            {
                dblist_cmd.Parameters["@name"].Value = new_name;
                dblist_cmd.Parameters["@oldName"].Value = old_name;
                dblist_cmd.CommandText = "UPDATE databases SET name=@name WHERE name=@oldName";
                dblist_cmd.ExecuteNonQuery();
            }
            else
            {
                CreateDatabaseRecord(new_name);
            }
        }

        /// <summary>
        /// Проверяет существование базы данных в таблице 'databases'
        /// </summary>
        /// <param name="name">Имя БД - файл без пути и расширения</param>
        /// <returns>True, если БД существует</returns>
        private static bool CheckIfDatabaseRecordExists(string name)
        {
            dblist_cmd.Parameters["@name"].Value = name;
            dblist_cmd.CommandText = "SELECT name FROM databases WHERE name=@name";
            object o = dblist_cmd.ExecuteScalar();
            if (o == null)
                return false;
            return true;
        }

        /// <summary>
        /// Устанавливает флаг последней выбранной базы данных в таблице 'databases'
        /// </summary>
        /// <param name="name">Имя БД - файл без пути и расширения</param>
        /// <param name="iteration">Защита от зацикливания, не требует установки пользователем при вызове функции</param>
        public static void SelectDatabaseRecord(string name, int iteration=0)
        {
            if (iteration++ > 1) return;

            if (CheckIfDatabaseRecordExists(name))
            {
                dblist_cmd.Parameters["@name"].Value = name;
                dblist_cmd.CommandText = "UPDATE databases SET selected=0";
                dblist_cmd.ExecuteNonQuery();

                dblist_cmd.CommandText = "UPDATE databases SET selected=1 WHERE name=@name";
                dblist_cmd.ExecuteNonQuery();
            }
            else
            {
                CreateDatabaseRecord(name);
                SelectDatabaseRecord(name, iteration);
            }
        }

        /// <summary>
        /// Задает иконку для базы данных в таблице 'databases'
        /// </summary>
        /// <param name="name">Имя БД - файл без пути и расширения</param>
        /// <param name="filename">Имя файла с расширением, но без пути. Может использоваться для сохранения изображения в файл.</param>
        /// <param name="icon">Массив байтов изображения</param>
        /// <param name="iteration">Защита от зацикливания, не требует установки пользователем при вызове функции</param>
        public static void SetIconForDatabaseRecord(string name, string filename, byte[] icon, int iteration = 0)
        {
            if (iteration++ > 1) return;

            if (CheckIfDatabaseRecordExists(name))
            {
                dblist_cmd.Parameters["@name"].Value = name;
                dblist_cmd.Parameters["@icon"].Value = icon;
                dblist_cmd.Parameters["@iconFilename"].Value = filename;
                dblist_cmd.CommandText = "UPDATE databases SET icon=@icon, icon_filename=@iconFilename WHERE name=@name";
                dblist_cmd.ExecuteNonQuery();
            }
            else
            {
                CreateDatabaseRecord(name);
                SetIconForDatabaseRecord(name, filename, icon, iteration);
            }
        }

        /// <summary>
        /// Загружает иконку для базы данных из таблицы 'databases'
        /// </summary>
        /// <param name="name">Имя БД - файл без пути и расширения</param>
        /// <returns>Объект изображения или null</returns>
        public static Image GetIconForDatabaseRecord(string name)
        {
            Image im = null;

            dblist_cmd.Parameters["@name"].Value = name;
            dblist_cmd.CommandText = "SELECT icon FROM databases WHERE name=@name";
            object o = dblist_cmd.ExecuteScalar();
            if (o != null && !DBNull.Value.Equals(o))
            {
                using (MemoryStream ms = new MemoryStream((byte[])o))
                    im = new Bitmap(ms);
            }
            
            return im;
        }

        /// <summary>
        /// Загружает данные иконки как массив байтов для базы данных из таблицы 'databases'
        /// </summary>
        /// <param name="name">Имя БД - файл без пути и расширения</param>
        /// <param name="filename">Имя файла сохраненного в БД</param>
        /// <returns>Массив или null</returns>
        public static byte[] GetIconRawDataForDatabaseRecord(string name, out string filename)
        {
            byte[] ret1 = null;
            string ret2 = "";

            dblist_cmd.Parameters["@name"].Value = name;
            dblist_cmd.CommandText = "SELECT icon, icon_filename FROM databases WHERE name=@name";
            SQLiteDataReader dr = dblist_cmd.ExecuteReader();
            if (dr.Read())
            {
                ret1 = DBNull.Value.Equals(dr["icon"]) ? null : (byte[])dr["icon"];
                ret2 = DBNull.Value.Equals(dr["icon_filename"]) ? "default.png" : (string)dr["icon_filename"];
            }
            dr.Close();

            filename = ret2;

            return ret1;
        }

        /// <summary>
        /// Загружает текст плагина
        /// </summary>
        /// <param name="property_id">ID оглавления</param>
        /// <returns>Текст кода</returns>
        public static string Plugin_Read(long property_id)
        {
            string ret = "";
            cmd.CommandText = "SELECT data FROM attachments WHERE property = @property_id";
            cmd.Parameters["@property_id"].Value = property_id;
            object o = cmd.ExecuteScalar();
            if (o != null && !DBNull.Value.Equals(o))
            {
                ret = Encoding.UTF8.GetString((byte[])o);
            }

            return ret;
        }

        /// <summary>
        /// Обновляет текст кода плагина. Вызывается из редактора плагина.
        /// </summary>
        /// <param name="property_id">ID оглавления</param>
        /// <param name="text">Программный код</param>
        /// <returns>Если False, то обновление не произошло</returns>
        public static bool Plugin_Update(long property_id, string text)
        {
            long rowsAffected = 0;
            cmd.Parameters["@property_id"].Value = property_id;
            cmd.Parameters["@data"].Value = Encoding.UTF8.GetBytes(text);
            cmd.CommandText = "UPDATE attachments SET data=@data WHERE property = @property_id";
            rowsAffected = cmd.ExecuteNonQuery();
            if (rowsAffected > 0)
            {
                // сохраним в истории
                string name = VARS.main_form.tvProps.SelectedNode.Text;
                SaveHistory((long)VARS.main_form.tvClasses.SelectedNode.Tag, property_id, "properties", "Обновление кода плагина", name);
            }
            return rowsAffected != 0;
        }

        /// <summary>
        /// Изменяет название плагина.
        /// </summary>
        /// <param name="property_id">ID оглавления</param>
        /// <param name="newname">Новое название плагина</param>
        /// <returns>Если False, то обновление не произошло</returns>
        public static bool Plugin_Rename(long property_id, string newname)
        {
            long rowsAffected = 0;

            SQLiteTransaction trans = conn.BeginTransaction();

            try
            {
                cmd.Parameters["@property_id"].Value = property_id;
                cmd.Parameters["@fileName"].Value = newname;
                cmd.CommandText = "UPDATE attachments SET fileName=@fileName WHERE property = @property_id";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "UPDATE properties SET name = @fileName WHERE id=@property_id";
                cmd.ExecuteNonQuery();

                rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                    // сохраним в истории
                    SaveHistory((long)VARS.main_form.tvClasses.SelectedNode.Tag, property_id, "properties", "Изменение названия плагина", newname);

                trans.Commit();
            }
            catch
            {
                trans.Rollback();
                rowsAffected = 0;
            }

            return rowsAffected != 0;
        }

        /// <summary>
        /// Создание пустого плагина
        /// </summary>
        /// <param name="parent_id">ID элемента оглавления</param>
        /// <param name="class_id">ID каталога</param>
        /// <param name="name">Название плагина</param>
        /// <returns>ROWID новой элемента оглавления или -1</returns>
        public static long Plugin_Create(long parent_id, long class_id, string name)
        {
            SQLiteTransaction trans = conn.BeginTransaction();
            long rowid = -1;

            try
            {
                rowid = SaveProperty(-1, class_id, name, (int)IconTypes.Plugin, parent_id, "");

                cmd.Parameters["@rowid"].Value = rowid;
                cmd.Parameters["@fileName"].Value = name;

                cmd.CommandText = @"INSERT INTO attachments (property, filename, data)
                                    VALUES(@rowid, @fileName, @data)";

                cmd.Parameters["@data"].Value = Encoding.UTF8.GetBytes(string.Empty);

                cmd.ExecuteNonQuery();

                // сохраним в истории
                SaveHistory(class_id, conn.LastInsertRowId, "properties", "Создание плагина", name);

                trans.Commit();
            }
            catch
            {
                trans.Rollback();
                rowid = -1;
            }

            return rowid;
        }

        /// <summary>
        /// Перемещает каталог в другую БД.
        /// </summary>
        /// <param name="AnotherDbName">Имя БД в которую перемещается каталог без пути и расширения</param>
        /// <param name="AnotherCatalogName">Новое имя каталога</param>
        /// <param name="id">ID текущего каталога, который будет перемещаться</param>
        /// <returns>True - если перемещение удалось, иначе false. Текст ошибки в LastError</returns>
        public static bool Service_MoveCatalogBetweenDbs(string AnotherDbName, string AnotherCatalogName, long id)
        {
            LastError = "";

            // открываем другую БД
            SQLiteConnection newConn = new SQLiteConnection();
            string anotherDbName = $@"{Application.StartupPath}\databases\{AnotherDbName}.sqlite";
            newConn.ConnectionString = $@"Data Source={anotherDbName}; foreign keys=true; nolock=1; version=3;";
            try
            {
                newConn.Open();
            }
            catch (Exception ex)
            {
                LastError = ex.Message;
                return false;
            }

            // перемещаем
            SQLiteTransaction anotherDbTransaction = null;
            SQLiteTransaction currentDbTransaction = null;
            try
            {
                anotherDbTransaction = newConn.BeginTransaction();
                currentDbTransaction = conn.BeginTransaction();

                SQLiteCommand anotherCmd = newConn.CreateCommand();
                cmd.CommandText = $"ATTACH DATABASE '{anotherDbName}' AS adb1";
                cmd.ExecuteNonQuery();

                //cmd.CommandText = "INSERT INTO classes "


                cmd.CommandText = "DETACH DATABASE adb1";
                cmd.ExecuteNonQuery();

                anotherDbTransaction.Commit();
                currentDbTransaction.Commit();
            }
            catch (Exception ex)
            {
                anotherDbTransaction.Rollback();
                currentDbTransaction.Rollback();
                LastError = ex.Message;
                return false;
            }

            return true;
        }

        /// <summary>
        /// Перемещает выделенный элемент оглавления в каталог
        /// </summary>
        /// <param name="propId">ROWID оглавления</param>
        /// <param name="toClassId">ROWID класса куда перемещаем, если равно -1, то создается новый класс</param>
        /// <param name="fromClassId">ROWID класса из которого перемещаем</param>
        /// <param name="toClassName">Имя класса в которорый перемещаем, не имеет значения при создании нового</param>
        /// <returns>ROWID созданного класса, если создавался новый класс, иначе -1</returns>
        public static long MoveProperyToClass(long propId, long toClassId, long fromClassId, string toClassName)
        {
            long ret = -1;

            cmd.Parameters["@property_id"].Value = propId;
            cmd.Parameters["@class_id"].Value = toClassId;
            

            if (toClassId == -1)
            {
                SQLiteTransaction trans = conn.BeginTransaction();
                try
                {
                    cmd.CommandText = "SELECT name FROM properties WHERE id=@property_id";
                    cmd.Parameters["@name"].Value = (string)cmd.ExecuteScalar();
                    cmd.CommandText = "SELECT description FROM properties WHERE id=@property_id";
                    cmd.Parameters["@description"].Value = (string)cmd.ExecuteScalar();

                    cmd.CommandText = "INSERT INTO classes (name, description) VALUES (@name, @description)";
                    cmd.ExecuteNonQuery();
                    long newClassId = conn.LastInsertRowId;
                    cmd.Parameters["@class_id"].Value = newClassId;

                    cmd.CommandText = @"
                    UPDATE properties SET class=@class_id WHERE parent IN(
                        WITH cte AS
                        (
                            SELECT id FROM properties WHERE id=@property_id
                            UNION ALL
                            SELECT properties.id FROM cte, properties ON  properties.parent = cte.id
                        )
                        SELECT id FROM cte)";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "UPDATE properties SET parent=NULL WHERE parent=@property_id";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "DELETE FROM properties WHERE id=@property_id";
                    cmd.ExecuteNonQuery();

                    // сохраним в истории
                    SaveHistory(newClassId, -1, "classes", "Создание каталога (Drag and Drop)", VARS.main_form.tvProps.SelectedNode.Text);
                    SaveHistory(fromClassId, propId, "properties", "Удаление оглавления (Drag and Drop)", VARS.main_form.tvProps.SelectedNode.Text);

                    ret = newClassId;
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка перемещения", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                }

            }
            else
            {
                SQLiteTransaction trans = conn.BeginTransaction();
                try
                {
                    cmd.CommandText = "UPDATE properties SET parent=NULL WHERE id=@property_id";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = @"
                    UPDATE properties SET class=@class_id WHERE id IN(
                        WITH cte AS
                        (
                            SELECT id FROM properties WHERE id=@property_id
                            UNION ALL
                            SELECT properties.id FROM cte, properties ON  properties.parent = cte.id
                        )
                        SELECT id FROM cte)";
                    cmd.ExecuteNonQuery();

                    // сохраним в истории
                    SaveHistory(toClassId, propId, "classes", "Обновление информации каталога (Drag and Drop)", toClassName);
                    SaveHistory(fromClassId, propId, "properties", "Удаление оглавления (Drag and Drop)", VARS.main_form.tvProps.SelectedNode.Text);

                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка перемещения", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                }
            }

            return ret;
        }

        /// <summary>
        /// Перемещает выделенный каталог в оглавление
        /// </summary>
        /// <param name="classId">ROWID перемещаемого каталога</param>
        /// <param name="toPropId">ROWID элемента оглавления приемника или -1, если в корень</param>
        /// <param name="toClassId">ROWID каталога приемника</param>
        /// /// <param name="toClassName">Имя перемещаемого класса</param>
        /// <returns></returns>
        public static bool MoveClassToProperty(long classId, long toPropId, long toClassId, string сlassName)
        {
            bool ret = true;

            cmd.Parameters["@property_id"].Value = toPropId;
            if (toPropId < 0) cmd.Parameters["@property_id"].Value = DBNull.Value;
            cmd.Parameters["@class_id"].Value = classId;
            cmd.Parameters["@class_id2"].Value = toClassId;

            SQLiteTransaction trans = conn.BeginTransaction();
            try
            {
                cmd.CommandText = @"INSERT INTO properties (name, description, type, parent, class)
                                SELECT name, description, 4, @property_id, @class_id2 FROM classes
                                WHERE id=@class_id";
                cmd.ExecuteNonQuery();
                long lastId = conn.LastInsertRowId;

                cmd.Parameters["@property_id"].Value = lastId;

                cmd.CommandText = @"UPDATE properties SET parent = @property_id WHERE parent IS NULL AND class=@class_id";
                cmd.ExecuteNonQuery();

                cmd.CommandText = @"UPDATE properties SET class=@class_id2 WHERE class=@class_id";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "DELETE FROM classes WHERE id=@class_id";
                cmd.ExecuteNonQuery();

                SaveHistory(toClassId, toPropId, "classes", "Обновление информации каталога (Drag and Drop)", VARS.main_form.tvProps.SelectedNode.Text);
                SaveHistory(classId, -1, "classes", "Удаление каталога (Drag and Drop)", сlassName);

                trans.Commit();
            }
            catch (Exception ex)
            {
                ret = false;
                trans.Rollback();
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка перемещения", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }

            return ret;
        }

        public static void SaveSyntaxRules(string rules)
        {
            cmd.CommandText = "DELETE FROM settings WHERE key='SYNTAX_RULES'";
            cmd.ExecuteNonQuery();
            cmd.Parameters["@stringData"].Value = rules;
            cmd.CommandText = $"INSERT INTO settings VALUES('SYNTAX_RULES',@stringData)";
            cmd.ExecuteNonQuery();
        }

        public static string LoadSyntaxRules()
        {
            string ret = "";
            cmd.CommandText = $"SELECT value FROM settings WHERE key='SYNTAX_RULES'";
            object o = cmd.ExecuteScalar();
            ret = (string)(o ?? "");
            return ret;
        }
    }
}
