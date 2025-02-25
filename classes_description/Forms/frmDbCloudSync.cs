using Google.Apis.Drive.v3;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace simple_database
{
    public partial class frmDbCloudSync : Form
    {
        public frmDbCloudSync()
        {
            InitializeComponent();
        }

        /*
        0-select
        1-local name
        2-local date
        3-local displayed size - для отображения как строка
        4-comparison sign
        5-cloud name
        6-cloud date
        7-cloud displayed size - для отображения как строка
        8-cloud id
        9-local real size (размер файла БД) - для расчетов
        10-cloud real size (размер упакованного в zip файла) - для расчетов
        11-cloudFileSize (размер файла в облаке) - для загрузки
        */

        async private void frmDbCloudSync_Shown(object sender, EventArgs e)
        {
            ssStatus.Text = "Получение информации, подождите....";
            panel1.Enabled = dgv.Enabled = false;
            dgv.Rows.Clear();
            Application.DoEvents();

            // получаем список локальных файлов и их характеристик
            DATABASE.CloseAllTemporarily();
            BACKUPS.FuncResult res = BACKUPS.GdCloud.GetLocalItemsList();
            DATABASE.RestoreAllFromTemporary();

            if (res.error != "") throw new Exception(res.error);
            List<BACKUPS.GdItem> localItems = (List<BACKUPS.GdItem>)res.data;

            // выводим локальные файлы в таблицу
            foreach (BACKUPS.GdItem item in localItems)
            {
                dgv.Rows.Add(false, item.name, item.modifiedDateLocal, ConvertSize(item.sizeLocal), "", "", null, "", "", item.sizeLocal, (long)0, (long)0);
            }

            // получаем список файлов в облаке, находим сопоставление локальным и выводим в таблицу
            BACKUPS.GdConfigInfo info = BACKUPS.GdCloud.LoadConfig();

            res = await BACKUPS.GdCloud.GetCloudItemsList(info.folderid, info.sakey);
            if (res.error == "")
            {
                List<BACKUPS.GdItem> cloudItems = (List<BACKUPS.GdItem>)res.data;
                foreach (BACKUPS.GdItem item in cloudItems)
                {
                    bool matched = false;
                    for (int r = 0; r < dgv.Rows.Count; r++)
                    {
                        if ((string)dgv.Rows[r].Cells[1].Value == item.name)
                        {
                            dgv.Rows[r].Cells[0].Value = false;
                            dgv.Rows[r].Cells[5].Value = item.name;
                            dgv.Rows[r].Cells[6].Value = item.modifiedDateLocal;
                            dgv.Rows[r].Cells[7].Value = ConvertSize(item.sizeLocal);
                            dgv.Rows[r].Cells[8].Value = item.id;
                            dgv.Rows[r].Cells[10].Value = item.sizeLocal;
                            dgv.Rows[r].Cells[11].Value = item.sizeCloud;
                            matched = true;
                            break;
                        }
                    }

                    if (!matched)
                    {
                        dgv.Rows.Add(false, "", null, "", "", item.name, item.modifiedDateLocal, ConvertSize(item.sizeLocal), item.id, (long)0, item.sizeLocal, item.sizeCloud);
                    }
                }
            }

            // сравниваем и раскрашиваем таблицу в зависимости от сравнения
            for (int r = 0; r < dgv.Rows.Count; r++)
            {
                DateTime t1 = (DateTime)(dgv.Rows[r].Cells[2].Value ?? DateTime.MinValue);
                DateTime t2 = (DateTime)(dgv.Rows[r].Cells[6].Value ?? DateTime.MinValue);
                long s1 = (long)dgv.Rows[r].Cells[9].Value;
                long s2 = (long)dgv.Rows[r].Cells[10].Value;

                if ((t1 == t2) && (s1 == s2))
                {
                    dgv.Rows[r].DefaultCellStyle.ForeColor = Color.Green;
                    dgv.Rows[r].Cells[4].Value = "=";
                }
                else
                {
                    dgv.Rows[r].DefaultCellStyle.ForeColor = Color.Red;
                    if (t1 < t2)
                        dgv.Rows[r].Cells[4].Value = "<";
                    else
                        dgv.Rows[r].Cells[4].Value = ">";
                }

                if ((string)dgv.Rows[r].Cells[1].Value == "databases" || (string)dgv.Rows[r].Cells[5].Value == "databases")
                    dgv.Rows[r].DefaultCellStyle.Font = new Font(dgv.Font, FontStyle.Underline);
            }

            panel1.Enabled = dgv.Enabled = true;
            ssStatus.Text = "";
        }

        /// <summary>
        /// Преобразует размер к текстовому виду xxx.xxx Кб/Мб/Гб
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        private string ConvertSize(long size)
        {
            string ret = "- Б";
            double K = Math.Pow(2, 10);
            double M = Math.Pow(2, 20);
            double G = Math.Pow(2, 30);

            if (size < K)
                ret = $"{size} Б";
            else if (size < M)
                ret = $"{(size / K):f1} Кб";
            else if (size < G)
                ret = $"{(size / M):f1} Мб";
            else
                ret = $"{(size / G):f1} Гб";
            return ret;
        }

        /// <summary>
        /// Кнопка: Выбрать все БД
        /// </summary>
        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            for (int r = 0; r < dgv.Rows.Count; r++)
                dgv.Rows[r].Cells[0].Value = true;
        }

        /// <summary>
        /// Кнопка: Снять выделение со всех БД
        /// </summary>
        private void btnUncheckAll_Click(object sender, EventArgs e)
        {
            for (int r = 0; r < dgv.Rows.Count; r++)
                dgv.Rows[r].Cells[0].Value = false;
        }

        /// <summary>
        /// Кнопка: "Синхронизация"
        /// </summary>
        private void btnSyncronize_Click(object sender, EventArgs e)
        {
            bool isSomethingSelected = false;
            for (int r = 0; r < dgv.Rows.Count; r++) if ((bool)dgv.Rows[r].Cells[0].Value) isSomethingSelected = true;
            if (!isSomethingSelected)
            {
                MessageBox.Show("Нет выделенных файлов", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

           if (MessageBox.Show("Данные на сервере и локальной машине будут синхронизированы для выделенных файлов.\n(Примечание: Если локальный файл отсутствует в облаке, облачный не будет удален)\nПродолжить?", "Предупреждение",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.No)
                return;

            BACKUPS.GdConfigInfo info = BACKUPS.GdCloud.LoadConfig();
            if (info.sakey == "")
            {
                MessageBox.Show("Не задан JSON файл Drive System Account.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (info.folderid == "")
            {
                MessageBox.Show("Не задана удаленная папка для хранения данных.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DATABASE.CloseAllTemporarily();

            for (int r = 0; r < dgv.Rows.Count; r++)
            {
                if ((bool)dgv.Rows[r].Cells[0].Value)
                {
                    DateTime t1 = (DateTime)(dgv.Rows[r].Cells[2].Value ?? DateTime.MinValue);
                    DateTime t2 = (DateTime)(dgv.Rows[r].Cells[6].Value ?? DateTime.MinValue);
                    string n1 = (string)dgv.Rows[r].Cells[1].Value;
                    string n2 = (string)dgv.Rows[r].Cells[5].Value;

                    string dbName = (n1 == "" ? n2 : n1);

                    // загрузить из облака
                    if (t1 < t2 || n1 == "")
                    {
                        // создадим резервную копию заменяемой БД, если она есть
                        bool restoreIfErrorRequired = false;
                        if (File.Exists(Path.Combine(VARS.db_folder, dbName + ".sqlite")))
                        {
                            File.Copy(Path.Combine(VARS.db_folder, dbName + ".sqlite"), Path.Combine(VARS.temp_folder, dbName + ".sqlite"), true);
                            restoreIfErrorRequired = true;
                        }

                        // покажем форму статуса загрузки
                        frmStatusedProgressBar frm = new frmStatusedProgressBar();
                        frm.userTask = DownloadFileFromGoogleDrive;
                        frm.userTaskParams.Add(info.sakey);
                        frm.userTaskParams.Add(info.folderid);
                        frm.userTaskParams.Add(new BACKUPS.GdItem()
                        {
                            name = dbName,
                            modifiedDateLocal = t2,
                            sizeCloud = (long)dgv.Rows[r].Cells[11].Value,
                            id = (string)dgv.Rows[r].Cells[8].Value
                        });
                        frm.pb1.Maximum = (int)(long)dgv.Rows[r].Cells[11].Value;
                        frm.ShowDialog();

                        // если все ОК - установим даты файла БД как они описаны в облачном архиве и очистим папку temp
                        if (frm.resultMessage == "Выполнено")
                        {
                            File.SetCreationTime(Path.Combine(VARS.db_folder, dbName + ".sqlite"), t2);
                            File.SetLastWriteTime(Path.Combine(VARS.db_folder, dbName + ".sqlite"), t2);
                            File.SetLastAccessTime(Path.Combine(VARS.db_folder, dbName + ".sqlite"), t2);
                            HELPER.CleanUpTemp();
                        }
                        else
                        {
                            // восстановим БД из резервной копии в случае ошибки
                            if (restoreIfErrorRequired)
                            {
                                MessageBox.Show("При загрузке БД произошла ошибка. Локальная БД не будет изменена. Текст ошибки: " + frm.resultMessage, "Ошибка загрузки", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                File.Copy(Path.Combine(VARS.temp_folder, dbName + ".sqlite"), Path.Combine(VARS.db_folder, dbName + ".sqlite"), true);
                                HELPER.CleanUpTemp();
                            }
                        }
                    }
                    // сохранить в облаке
                    else if (t2 < t1 || n2 == "")
                    {
                        frmStatusedProgressBar frm = new frmStatusedProgressBar();
                        frm.userTask = UploadFileToGoogleDrive;
                        frm.userTaskParams.Add(info.sakey);
                        frm.userTaskParams.Add(info.folderid);
                        frm.userTaskParams.Add(new BACKUPS.GdItem()
                        {
                            name = dbName,
                            modifiedDateLocal = t1,
                            sizeLocal = (long)dgv.Rows[r].Cells[9].Value,
                            id = (string)dgv.Rows[r].Cells[8].Value
                        });
                        frm.ShowDialog();

                        HELPER.CleanUpTemp();
                    }
                }
            }

            DATABASE.RestoreAllFromTemporary();

            VARS.main_form.Invoke((MethodInvoker)delegate
            {
                ClassItem.Load(VARS.main_form);
            });

            frmDbCloudSync_Shown(null, null);
        }

        /// <summary>
        /// Кнопка: Выбрать/снять выбор строки БД
        /// </summary>
        private void dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == 0)
            {
                dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = !(bool)dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
            }
        }

        /// <summary>
        /// Кнопка: Выбрать только отличающиеся
        /// </summary>
        private void btnSelectDifferent_Click(object sender, EventArgs e)
        {
            for (int r = 0; r < dgv.Rows.Count; r++)
            {
                if (dgv.Rows[r].DefaultCellStyle.ForeColor == Color.Red)
                    dgv.Rows[r].Cells[0].Value = true;
                else
                    dgv.Rows[r].Cells[0].Value = false;
            }
        }

        /// <summary>
        /// Асинхронная задача по загрузке выбранной базы данных из сервера.
        /// </summary>
        /// <param name="taskParams">Параметры задачи</param>
        public void DownloadFileFromGoogleDrive(object taskParams)
        {
            List<object> param = (List<object>)taskParams;
            frmStatusedProgressBar frm = (frmStatusedProgressBar)param[0];
            string sakey = (string)param[1];
            string parentFolderid = (string)param[2];
            BACKUPS.GdItem cloudItem = (BACKUPS.GdItem)param[3];

            frm.Invoke((MethodInvoker)delegate { frm.Text = $"Загрузка базы '{cloudItem.name.ToUpper()}' из облака"; });

            try
            {
                for (int i = 0; i < 1; i++) // цикл в 1-у итерацию нужен для использования break;
                {
                    // создадим переменные с именами файлов
                    string dbFileWithPath = $@"{ Application.StartupPath}\databases\{cloudItem.name}.sqlite";
                    string zipFile = BACKUPS.GdCloud.ConvertToCloudItemName(cloudItem) + ".zip";
                    string zipFileWithPath = Path.Combine(VARS.temp_folder, zipFile);

                    // удалим временный файл, если есть
                    try
                    {
                        File.Delete(zipFileWithPath);
                    }
                    catch { }

                    // шаг 1 - загружаем файл из облака во временную папку

                    frm.Invoke((MethodInvoker)delegate
                    {
                        frm.lblCurrentAction.Text = "Загрузка файла из облака...";
                        frm.pb0.Maximum = 2;
                        frm.pb0.Value = 0;
                        frm.pb1.Value = 0;
                        frm.UpdateControl();
                    });

                    if (frm.cts.IsCancellationRequested) break;

                    var service = BACKUPS.GdCloud.CreateService(sakey);

                    var request = service.Files.Get(cloudItem.id);
                    request.MediaDownloader.ProgressChanged += (Google.Apis.Download.IDownloadProgress progress) =>
                    {
                        frm.Invoke((MethodInvoker)delegate
                        {
                            frm.pb1.Value = (int)progress.BytesDownloaded;
                        });
                    };

                    using (var memoryStream = new MemoryStream())
                    {
                        var result = request.DownloadWithStatus(memoryStream);
                        if (result.Status == Google.Apis.Download.DownloadStatus.Failed)
                            throw new Exception(result.Exception.Message);
                        System.IO.File.WriteAllBytes(zipFileWithPath, memoryStream.ToArray());
                    }

                    frm.Invoke((MethodInvoker)delegate
                    {
                        frm.pb0.Value++;
                        frm.pb1.Value = 100;
                        frm.UpdateControl();
                    });

                    Thread.Sleep(1000);

                    // шаг 2 - распаковываем сразу в БД

                    if (frm.cts.IsCancellationRequested) break;

                    frm.Invoke((MethodInvoker)delegate
                    {
                        frm.lblCurrentAction.Text = "Распаковка...";
                        frm.pb1.Value = 0;
                        frm.UpdateControl();
                    });

                    using (FileStream src = new FileStream(zipFileWithPath, FileMode.Open))
                    using (FileStream dst = new FileStream(dbFileWithPath, FileMode.Create))
                    using (GZipStream decompress = new GZipStream(src, CompressionMode.Decompress))
                    {
                        decompress.CopyTo(dst);
                    }
                }

                frm.resultMessage = "Выполнено";
            }
            catch (Exception ex)
            {
                frm.resultMessage = ex.Message;
            }

            frm.Invoke((MethodInvoker)delegate
            {
                frm.lblCurrentAction.Text = "Выполнено";
                if (frm.cts.IsCancellationRequested) frm.resultMessage = "Прервано пользователем.";
                frm.pb0.Value = frm.pb0.Maximum;
                frm.pb1.Value = frm.pb1.Maximum;
                frm.UpdateControl();
            });
            frm.abortConfirmed = true;
        }

        /// <summary>
        /// Асинхронная задача по загрузке выбранной базы данных на сервер.
        /// </summary>
        /// <param name="taskParams">Параметры задачи</param>
        public void UploadFileToGoogleDrive(object taskParams)
        {
            List<object> param = (List<object>)taskParams;
            frmStatusedProgressBar frm = (frmStatusedProgressBar)param[0];
            string sakey = (string)param[1];
            string parentFolderid = (string)param[2];
            BACKUPS.GdItem localItem = (BACKUPS.GdItem)param[3];

            frm.Invoke((MethodInvoker)delegate { frm.Text = $"Сохранение базы '{localItem.name.ToUpper()}' в облаке"; });

            try
            {
                for (int i = 0; i < 1; i++) // цикл в 1-у итерацию нужен для использования break;
                {
                    List<BACKUPS.GdItem> objectsList = BACKUPS.GdCloud.GetListOfCloudItems(sakey).Result;
                    if (frm.cts.IsCancellationRequested) break;

                    // создадим переменные с именами файлов
                    string dbFileWithPath = $@"{ Application.StartupPath}\databases\{localItem.name}.sqlite";
                    string zipFile = BACKUPS.GdCloud.ConvertToCloudItemName(localItem) + ".zip";
                    string zipFileWithPath = Path.Combine(VARS.temp_folder, zipFile);

                    // шаг 1 - создаем zip архив

                    frm.Invoke((MethodInvoker)delegate
                    {
                        frm.lblCurrentAction.Text = "Создание Zip-архива...";
                        frm.pb0.Maximum = 2;
                        frm.pb0.Value = 0;
                        frm.pb1.Maximum = 100;
                        frm.pb1.Value = 0;
                        frm.UpdateControl();
                    });

                    if (frm.cts.IsCancellationRequested) break;

                    using (FileStream src = new FileStream(dbFileWithPath, FileMode.Open))
                    using (FileStream dst = new FileStream(zipFileWithPath, FileMode.Create))
                    using (GZipStream compress = new GZipStream(dst, CompressionMode.Compress))
                    {
                        src.CopyTo(compress);
                    }

                    frm.Invoke((MethodInvoker)delegate
                    {
                        frm.pb0.Value++;
                        frm.pb1.Value = 100;
                        frm.UpdateControl();
                    });

                    Thread.Sleep(1000);

                    // шаг 2 - выгружаем в облако

                    if (frm.cts.IsCancellationRequested) break;

                    frm.Invoke((MethodInvoker)delegate
                    {
                        frm.lblCurrentAction.Text = "Отправка в облачное хранилище...";
                        frm.pb1.Value = 0;
                        frm.UpdateControl();
                    });

                    var service = BACKUPS.GdCloud.CreateService(sakey);
                    var driveFile = new Google.Apis.Drive.v3.Data.File()
                    {
                        Name = zipFile,
                        MimeType = BACKUPS.GdCloud.GetMimeType(zipFile)
                    };

                    void ProgressChanged(Google.Apis.Upload.IUploadProgress progress)
                    {
                        frm.Invoke((MethodInvoker)delegate
                        {
                            frm.pb1.Value = (int)progress.BytesSent;
                        });
                    }

                    using (var uploadStream = System.IO.File.OpenRead(zipFileWithPath))
                    {
                        uploadStream.Position = 0;
                        frm.Invoke((MethodInvoker)delegate { frm.pb1.Maximum = (int)uploadStream.Length; });

                        if (localItem.id != "")
                        {
                            FilesResource.UpdateMediaUpload request = service.Files.Update(driveFile, localItem.id, uploadStream, driveFile.MimeType); // должен быть выбран обновляемый файл
                            request.ChunkSize = FilesResource.UpdateMediaUpload.MinimumChunkSize;
                            request.ProgressChanged += ProgressChanged;
                            //request.ResponseReceived += Request_ResponseReceived;
                            request.Upload();
                        }
                        else
                        {
                            driveFile.Parents = new[] { parentFolderid };
                            FilesResource.CreateMediaUpload request = service.Files.Create(driveFile, uploadStream, driveFile.MimeType); // должна быть выбрана родительская папка
                            request.ChunkSize = FilesResource.CreateMediaUpload.MinimumChunkSize;
                            request.ProgressChanged += ProgressChanged;
                            //request.ResponseReceived += Request_ResponseReceived;
                            request.Upload();
                        }
                    }
                }

                frm.resultMessage = "Выполнено";
            }
            catch (Exception ex)
            {
                frm.resultMessage = ex.Message;
            }

            frm.Invoke((MethodInvoker)delegate
            {
                frm.lblCurrentAction.Text = "Выполнено";
                if (frm.cts.IsCancellationRequested) frm.resultMessage = "Прервано пользователем.";
                frm.pb0.Value = frm.pb0.Maximum;
                frm.pb1.Value = frm.pb1.Maximum;
                frm.UpdateControl();
            });
            frm.abortConfirmed = true;
        }

        /// <summary>
        /// Кнопка: Принудительная загрузка из облака
        /// </summary>
        private void btnForceDownload_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count == 0) return;

            string dbName = (string)dgv.SelectedRows[0].Cells[5].Value;

            if (dbName == "") return;

            if (MessageBox.Show($"Вы действительно хотите загрузить БД '{dbName}' из облака?", "Предупреждение", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2)
                == DialogResult.No) return;

            if (MessageBox.Show($"Принудительная загрузка БД '{dbName}' удалит все данные в локальной копии!\nТочно продолжить?", "Предупреждение", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2)
                == DialogResult.No) return;

            BACKUPS.GdConfigInfo info = BACKUPS.GdCloud.LoadConfig();
            if (info.sakey == "")
            {
                MessageBox.Show("Не задан JSON файл Drive System Account.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (info.folderid == "")
            {
                MessageBox.Show("Не задана удаленная папка для хранения данных.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DATABASE.CloseAllTemporarily();

            DateTime t2 = (DateTime)(dgv.SelectedRows[0].Cells[6].Value ?? DateTime.MinValue);

            // создадим резервную копию заменяемой БД, если она есть
            bool restoreIfErrorRequired = false;
            if (File.Exists(Path.Combine(VARS.db_folder, dbName + ".sqlite")))
            {
                File.Copy(Path.Combine(VARS.db_folder, dbName + ".sqlite"), Path.Combine(VARS.temp_folder, dbName + ".sqlite"), true);
                restoreIfErrorRequired = true;
            }

            // покажем форму статуса загрузки
            frmStatusedProgressBar frm = new frmStatusedProgressBar();
            frm.userTask = DownloadFileFromGoogleDrive;
            frm.userTaskParams.Add(info.sakey);
            frm.userTaskParams.Add(info.folderid);
            frm.userTaskParams.Add(new BACKUPS.GdItem()
            {
                name = dbName,
                modifiedDateLocal = t2,
                sizeCloud = (long)dgv.SelectedRows[0].Cells[11].Value,
                id = (string)dgv.SelectedRows[0].Cells[8].Value
            });
            frm.pb1.Maximum = (int)(long)dgv.SelectedRows[0].Cells[11].Value;
            frm.ShowDialog();

            // если все ОК - установим даты файла БД как они описаны в облачном архиве и очистим папку temp
            if (frm.resultMessage == "Выполнено")
            {
                File.SetCreationTime(Path.Combine(VARS.db_folder, dbName + ".sqlite"), t2);
                File.SetLastWriteTime(Path.Combine(VARS.db_folder, dbName + ".sqlite"), t2);
                File.SetLastAccessTime(Path.Combine(VARS.db_folder, dbName + ".sqlite"), t2);
                HELPER.CleanUpTemp();
            }
            else
            {
                // восстановим БД из резервной копии в случае ошибки
                if (restoreIfErrorRequired)
                {
                    MessageBox.Show("При загрузке БД произошла ошибка. Локальная БД не будет изменена. Текст ошибки: " + frm.resultMessage, "Ошибка загрузки", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    File.Copy(Path.Combine(VARS.temp_folder, dbName + ".sqlite"), Path.Combine(VARS.db_folder, dbName + ".sqlite"), true);
                    HELPER.CleanUpTemp();
                }
            }

            DATABASE.RestoreAllFromTemporary();

            VARS.main_form.Invoke((MethodInvoker)delegate
            {
                ClassItem.Load(VARS.main_form);
            });

            frmDbCloudSync_Shown(null, null);
        }

        /// <summary>
        /// Кнопка: Принудительная загрузка в облако
        /// </summary>
        private void btnForceUpload_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count == 0) return;

            string dbName = (string)dgv.SelectedRows[0].Cells[1].Value;

            if (dbName == "") return;

            if (MessageBox.Show($"Вы действительно хотите выгрузить БД '{dbName}' в облако?", "Предупреждение", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2)
                == DialogResult.No) return;

            if (MessageBox.Show($"Принудительная выгрузка БД '{dbName}' удалит все данные в облачной копии!\nТочно продолжить?", "Предупреждение", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2)
                == DialogResult.No) return;

            BACKUPS.GdConfigInfo info = BACKUPS.GdCloud.LoadConfig();
            if (info.sakey == "")
            {
                MessageBox.Show("Не задан JSON файл Drive System Account.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (info.folderid == "")
            {
                MessageBox.Show("Не задана удаленная папка для хранения данных.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DATABASE.CloseAllTemporarily();

            DateTime t1 = (DateTime)(dgv.SelectedRows[0].Cells[2].Value ?? DateTime.MinValue);

            // покажем форму статуса загрузки
            frmStatusedProgressBar frm = new frmStatusedProgressBar();
            frm.userTask = UploadFileToGoogleDrive;
            frm.userTaskParams.Add(info.sakey);
            frm.userTaskParams.Add(info.folderid);
            frm.userTaskParams.Add(new BACKUPS.GdItem()
            {
                name = dbName,
                modifiedDateLocal = t1,
                sizeLocal = (long)dgv.SelectedRows[0].Cells[9].Value,
                id = (string)dgv.SelectedRows[0].Cells[8].Value
            });
            frm.ShowDialog();

            HELPER.CleanUpTemp();

            DATABASE.RestoreAllFromTemporary();

            VARS.main_form.Invoke((MethodInvoker)delegate
            {
                ClassItem.Load(VARS.main_form);
            });

            frmDbCloudSync_Shown(null, null);
        }
    }
}
