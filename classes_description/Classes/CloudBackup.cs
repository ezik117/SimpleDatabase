using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using simple_database;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using System.IO;
using System.Globalization;

namespace BACKUPS
{
    /// <summary>
    /// Методы управления Google Drive
    /// </summary>
    static class GdCloud
    {
        /// <summary>
        /// Загружает конфигурацию для Google Drive
        /// </summary>
        /// <returns>Структуру с конфигурацией</returns>
        public static GdConfigInfo LoadConfig()
        {
            GdConfigInfo config = new GdConfigInfo()
            {
                activated = DATABASE.GlobalSettingsRead("Google Drive", "activate") == "1",
                checkupdatesonstart = DATABASE.GlobalSettingsRead("Google Drive", "checkOnStartup") == "1",
                sakey = DATABASE.GlobalSettingsRead("Google Drive", "SAKey"),
                folderid = DATABASE.GlobalSettingsRead("Google Drive", "remoteDirId"),
                foldername = DATABASE.GlobalSettingsRead("Google Drive", "remoteDirName")
            };

            config.validatePass = (config.sakey != "") && (config.folderid != "") && (config.foldername != "");

            return config;
        }

        /// <summary>
        /// Возвращает список объектов и их базовых параметров из GoogleDrive
        /// </summary>
        /// <param name="sakey">JSON данные Google System Account</param>
        /// <param name="parentId">Google Drive ID родительского объекта или пустая строка для получения списка
        /// вообще всех объектов доступных данному аккаунту</param>
        /// <returns>Список объектов GdItem</returns>
        public static async Task<List<GdItem>> GetListOfCloudItems(string sakey, CloudListOptions opts=CloudListOptions.All, string parentId="")
        {
            List<GdItem> ret = new List<GdItem>();

            try
            {
                GoogleCredential credential = GoogleCredential.FromJson(sakey).CreateScoped(new[] { DriveService.Scope.Drive });
                DriveService service = new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "simple_database"
                });

                var request = service.Files.List();
                request.Fields = "files(id,name,mimeType,createdTime,modifiedTime,parents,size)";
                var result = await request.ExecuteAsync();
                byte flags = (byte)opts;

                foreach (var file in result.Files)
                {
                    GdItem item = new GdItem()
                    {
                        id = file.Id,
                        name = file.Name,
                        mimeType = file.MimeType,
                        modifiedDate = file.ModifiedTimeDateTimeOffset ?? DateTimeOffset.MinValue,
                        modifiedDateLocal = (file.ModifiedTimeDateTimeOffset ?? DateTimeOffset.MinValue).ToOffset(DateTimeOffset.Now.Offset).DateTime,
                        createDate = file.CreatedTimeDateTimeOffset ?? DateTimeOffset.MinValue,
                        type = (file.MimeType == "application/vnd.google-apps.folder" ? "folder" : "file"),
                        parentId = (file.Parents == null ? "" : file.Parents[0]),
                        sizeCloud = file.Size ?? 0,
                    };

                    // применяем селекторы
                    if (parentId != "" && item.parentId != parentId) item = null; // если указана родительская папка, но она не совпадает
                    if (item != null && (flags & 1) == 1 && item.type != "file") item = null; // если нужны только файлы, а это не файл
                    if (item != null && (flags & 2) == 2 && item.type != "folder") item = null; // если нужны только папки, а это не папка

                    if (item != null) ret.Add(item);
                }
            }
            catch
            {
                ret.Clear();
            }

            return ret;
        }

        /// <summary>
        /// Создает сервис GoogleDrive для обслуживания API запросов
        /// </summary>
        /// <param name="sakey">Google Serverice Account JSON файл</param>
        /// <returns></returns>
        public static DriveService CreateService(string sakey)
        {
            GoogleCredential credential = GoogleCredential.FromJson(sakey).CreateScoped(new[] { DriveService.Scope.Drive });

            return new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "simple_database"
            });
        }

        /// <summary>
        /// Возвращает тип Mime из реестра Windows в зависимости от расширения файла
        /// </summary>
        /// <param name="fileName">имя файла (можно с путем)</param>
        /// <returns>Строка соответствующего типа Mime</returns>
        public static string GetMimeType(string fileName)
        {
            string mimeType = "application/unknown";
            string ext = System.IO.Path.GetExtension(fileName).ToLower();
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
            if (regKey != null && regKey.GetValue("Content Type") != null)
                mimeType = regKey.GetValue("Content Type").ToString();
            return mimeType;
        }

        /// <summary>
        /// Возвращает отсортированный по возрастанию список с данными текущего набора файлов баз данных.
        /// Все базы данных должны быть закрыты к моменту вызова данной функции, т.к.
        /// для получения размера файла, БД будет открыта с помощью FileStream
        /// </summary>
        /// <returns>В данных возвращается список List<GdItem> со следующими полями:
        /// - name: имя файла БД без пути и расширения
        /// - modifiedDateLocal: дата и время последней модификации (без TZ и миллисекунд)
        /// - size: размер файла в байтах</returns>
        public static FuncResult GetLocalItemsList()
        {
            string error = "";
            List<GdItem> localItems = new List<GdItem>();

            try
            {
                DirectoryInfo di = new DirectoryInfo(VARS.db_folder);
                FileSystemInfo[] fsi = di.GetFileSystemInfos("*", SearchOption.TopDirectoryOnly);
                
                foreach (FileSystemInfo entry in fsi)
                {
                    if (!entry.Attributes.HasFlag(FileAttributes.Directory) && entry.Extension == ".sqlite")
                    {
                        GdItem gi = new GdItem()
                        {
                            name = Path.GetFileNameWithoutExtension(entry.Name),
                            modifiedDateLocal = entry.LastWriteTime.AddTicks(-(entry.LastWriteTime.Ticks % TimeSpan.TicksPerSecond)),

                        };
                        using (FileStream fs = File.OpenRead(Path.Combine(VARS.db_folder, gi.name + ".sqlite")))
                        {
                            gi.sizeLocal = fs.Length;
                        }
                        localItems.Add(gi);
                    }
                }

                localItems.Sort((x, y) =>
                {
                    return string.Compare(x.name, y.name);
                });
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }

            return new FuncResult()
            {
                error = error,
                data = localItems,
            };
        }

        /// <summary>
        /// Преобразовывает объект GdItem к виду "name_20250101102030_1024" для загрузки в облако.
        /// Поля разделены нижним подчеркиванием, состоит из трех секций: имя БД, дата изменения, размер в байтах
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static string ConvertToCloudItemName(GdItem item)
        {
            string ret = item.name + "_";
            ret += item.modifiedDateLocal.ToString("yyyyMMddHHmmss") + "_";
            ret += item.sizeLocal.ToString();
            return ret;
        }

        /// <summary>
        /// Преобразовывает облачный объект вида "name_20250101102030_1024.zip" к объекту GdItem
        /// Если имя не содержит расширения .zip оно будет отброшено
        /// </summary>
        /// <param name="item"></param>
        /// <returns>Возвращает объект или null, если преобразовать не удалось или тип файла не ZIP</returns>
        public static GdItem ConvertFromCloudItemName(string item)
        {
            if (Path.GetExtension(item) != ".zip") return null;

            item = Path.GetFileNameWithoutExtension(item);
            GdItem ret = new GdItem();
            string[] parts = item.Split('_');
            if (parts.Length != 3) return null;

            ret.name = parts[0];
            
            if (!DateTime.TryParseExact(parts[1], "yyyyMMddHHmmss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out ret.modifiedDateLocal))
                return null;

            if (!long.TryParse(parts[2], out ret.sizeLocal)) return null;

            return ret;
        }

        /// <summary>
        /// Возвращает список файлов баз данных с облака связанных с выбранной родительской папкой
        /// </summary>
        /// <param name="parentId">Google Drive folder id</param>
        /// <param name="sakey">Ключ SystemAccount</param>
        /// <returns></returns>
        async public static Task<FuncResult> GetCloudItemsList(string parentId, string sakey)
        {
            List<GdItem> items = await BACKUPS.GdCloud.GetListOfCloudItems(sakey, CloudListOptions.Files, parentId);
            List<GdItem> ret = new List<GdItem>();
            foreach (GdItem item in items)
            {
                GdItem i = ConvertFromCloudItemName(item.name);
                if (i != null)
                {
                    i.createDate = item.createDate;
                    i.id = item.id;
                    i.mimeType = item.mimeType;
                    i.modifiedDate = item.modifiedDate;
                    i.parentId = item.parentId;
                    i.type = item.type;
                    i.sizeCloud = item.sizeCloud;
                    ret.Add(i);
                }
            }

            return new FuncResult()
            {
                error = "",
                data = ret
            };
        }
    }

    /// <summary>
    /// Класс для хранения настроек Google Drive
    /// </summary>
    class GdConfigInfo
    {
        public bool validatePass = false;
        public bool activated = false;
        public bool checkupdatesonstart = false;
        /// <summary>
        /// JSON файл с данными System Account
        /// </summary>
        public string sakey = "";
        /// <summary>
        /// Google ID папки в которую будут сохраняться базы данных
        /// </summary>
        public string folderid = "";
        /// <summary>
        /// Имя Google папки в которую будут сохраняться базы данных
        /// </summary>
        public string foldername = "";
    }

    /// <summary>
    /// Информация об облачном объекте
    /// </summary>
    class GdItem
    {
        /// <summary>
        /// ID объекта
        /// </summary>
        public string id;

        /// <summary>
        /// Название объекта
        /// </summary>
        public string name;

        /// <summary>
        /// Тип объекта: 'folder' или 'file'
        /// </summary>
        public string type;

        /// <summary>
        /// MIME тип объекта
        /// </summary>
        public string mimeType;

        /// <summary>
        /// Дата последнего изменения объекта
        /// </summary>
        public DateTimeOffset modifiedDate;

        /// <summary>
        /// Дата последнего изменения объекта в локальном времени без часового пояса
        /// </summary>
        public DateTime modifiedDateLocal;

        /// <summary>
        /// Дата создания объекта
        /// </summary>
        public DateTimeOffset createDate;

        /// <summary>
        /// Google ID родительской папки
        /// </summary>
        public string parentId = null;

        /// <summary>
        /// Размер файла в байтах, если для данного файла есть размер. Иначе ноль.
        /// </summary>
        public long sizeCloud = 0;

        /// <summary>
        /// Размер локального файла в байтах, если для данного файла есть размер. Иначе ноль.
        /// </summary>
        public long sizeLocal = 0;
    }

    /// <summary>
    /// Класс для возврата результатов из функций.
    /// Содержит поле ошибки и поле с данными.
    /// </summary>
    class FuncResult
    {
        /// <summary>
        /// Строка ошибки или пустая строка
        /// </summary>
        public string error;
        /// <summary>
        /// Объект возврата
        /// </summary>
        public object data;
    }

    /// <summary>
    /// Флаги для возврата объектов из функции GetListOfItems()
    /// </summary>
    enum CloudListOptions : byte
    {
        /// <summary>
        /// Включая файлы
        /// </summary>
        Files = 1,
        /// <summary>
        /// Включая папки
        /// </summary>
        Folders = 2,
        /// <summary>
        /// Включая файлы и папки
        /// </summary>
        All = 3,
    }
}
