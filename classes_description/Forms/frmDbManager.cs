using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;

namespace simple_database
{
    public partial class frmDbManager : Form
    {
        /// <summary>
        /// Имя выбранной БД
        /// </summary>
        public string dbName;

        /// <summary>
        /// Тип действия: 0-none, 1-create, 2-rename, 3-open
        /// </summary>
        public int action = 0;

        /// <summary>
        /// Иконка по умолчанию
        /// </summary>
        private Bitmap dbDefaultIcon = new Bitmap(24, 24);

        /// <summary>
        /// В случае переименования БД - содержит начальное название
        /// </summary>
        private string old_dbName = "";

        /// <summary>
        /// Конструктор формы
        /// </summary>
        public frmDbManager()
        {
            InitializeComponent();

            try
            {
                dbDefaultIcon = Properties.Resources.database_24;
            }
            catch {}

            dgv.AutoGenerateColumns = false;
            dgv.RowTemplate.Height = 30;
        }

        /// <summary>
        /// Инициализация формы после загрузки
        /// </summary>
        private void frmDbManager_Load(object sender, EventArgs e)
        {
            dgv.Rows.Clear();
            string[] files = Directory.GetFiles($@"{Application.StartupPath}\databases");
            Array.Sort(files);
            foreach (string file in files)
            {
                if (Path.GetFileNameWithoutExtension(file).ToLower() != "databases")
                {
                    string s = Path.GetFileNameWithoutExtension(file).ToLower();
                    Image im = DATABASE.GetIconForDatabaseRecord(s);
                    int rn = dgv.Rows.Add(im ?? dbDefaultIcon, s);
                    if (s == "default")
                    {
                        dgv.Rows[rn].DefaultCellStyle.ForeColor = Color.Blue;
                    }
                }
            }

            tbDbName.Text = "";
            pnlDbNameAction.Visible = false;
            if (dgv.Rows.Count > 0)
            {
                foreach (DataGridViewRow r in dgv.Rows)
                {
                    if ((string)r.Cells["dbmgrFile"].Value == dbName)
                    {
                        r.Selected = true;
                    }
                }
            }

            action = 0;
        }

        /// <summary>
        /// Кнопка: Создать БД
        /// </summary>
        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (pnlDbNameAction.Visible) return;

            tbDbName.Text = "";
            pnlDbNameAction.Visible = true;
            tbDbName.Focus();
            action = 1;
        }

        /// <summary>
        /// Кнопка: Переименовать БД
        /// </summary>
        private void btnRename_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count == 0) return;
            if (pnlDbNameAction.Visible) return;

            tbDbName.Text = (string)dgv.SelectedRows[0].Cells["dbmgrFile"].Value;
            old_dbName = tbDbName.Text;
            pnlDbNameAction.Visible = true;
            tbDbName.Focus();
            action = 2;
        }

        /// <summary>
        /// Кнопка: Удалить БД
        /// </summary>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count == 0) return;
            if (pnlDbNameAction.Visible) return;

            string current_db = (string)dgv.SelectedRows[0].Cells["dbmgrFile"].Value;
            if (dbName == current_db)
            {
                MessageBox.Show("Нельзя удалить открытую базу данных!", "Ошибка",
                                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show($"База данных '{current_db.ToUpper()}' будет безвозратно удалена!\r\nВЫ УВЕРЕНЫ?",
                "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.No)
                return;

            if (MessageBox.Show($"Пожалуйста, еще раз подтвердите удаление БД '{current_db.ToUpper()}'. Действие необратимо.",
                "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.No)
                            return;

            try
            {
                File.Delete($@"{ Application.StartupPath}\databases\{current_db}.sqlite");
                dgv.Rows.Remove(dgv.SelectedRows[0]);

                DATABASE.DeleteDatabaseRecord(current_db);
            }
            catch
            {
                MessageBox.Show("Ошибка при удалении! База данных не удалена.", "Ошибка",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Закрыть окно ввода имени БД при щелчке мыши по списку выбора файлов
        /// </summary>
        private void dgv_MouseDown(object sender, MouseEventArgs e)
        {
            if (pnlDbNameAction.Visible) pnlDbNameAction.Visible = false;
        }

        /// <summary>
        /// Изменить или создать БД при нажатии Enter в поле ввода
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbDbName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                pnlDbNameAction.Visible = false;
                string newDbName = tbDbName.Text.Trim().ToLower().Replace(' ', '_');

                if (newDbName == "") return;

                if (newDbName == "databases")
                {
                    MessageBox.Show("Имя 'databases' зарезервировано. Пожалуйста, выберите другое имя.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                bool dbExists = false;
                foreach (DataGridViewRow dr in dgv.Rows)
                {
                    if (newDbName == (string)dr.Cells["dbmgrFile"].Value)
                    {
                        dbExists = true;
                        break;
                    }
                }
                if (dbExists)
                {
                    MessageBox.Show("Указанная база данных уже существует!", "Ошибка",
                                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (action == 1) // создать БД
                {
                    try
                    {
                        DATABASE.Create(newDbName);
                        DATABASE.CreateDatabaseRecord(newDbName);
                        int newRowId = dgv.Rows.Add(dbDefaultIcon, newDbName);
                        dgv.Rows[newRowId].Selected = true;

                        dgv.Sort(dgv.Columns["dbmgrFile"], ListSortDirection.Ascending);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка при создании! База данных не создана.\r\n" + "Причина:\r\n" + ex.Message, "Ошибка",
                                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else if (action == 2) // переименовать БД
                {
                    if (dbName == old_dbName)
                    {
                        MessageBox.Show("Нельзя переименовать открытую базу данных.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (old_dbName == "default")
                    {
                        if (MessageBox.Show("Вы собираетесь переименовать базу данных открываемую по умолчанию. Это может привести к непредсказуемым последствиям. Продолжить?", "Переименование",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.No)
                                                    return;
                    }

                    try
                    {
                        File.Move($@"{ Application.StartupPath}\databases\{dgv.SelectedRows[0].Cells["dbmgrFile"].Value}.sqlite",
                                  $@"{ Application.StartupPath}\databases\{newDbName}.sqlite");
                        DATABASE.ChangeDatabaseRecordName(old_dbName, newDbName);
                        dgv.SelectedRows[0].Cells["dbmgrFile"].Value = newDbName;

                        dgv.Sort(dgv.Columns["dbmgrFile"], ListSortDirection.Ascending);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка при переименовании! База данных не переименована.\r\n" + "Причина:\r\n" + ex.Message, "Ошибка",
                                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            else if (e.KeyCode == Keys.Escape)
            {
                if (pnlDbNameAction.Visible) pnlDbNameAction.Visible = false;
            }
        }

        /// <summary>
        /// Открытие БД по двойному щелчку
        /// </summary>
        private void dgv_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgv.SelectedRows.Count == 0) return;

            if (pnlDbNameAction.Visible) return;

            // проверим на несохраненные данные 
            if (PROPERTY.CheckForUnsavedDesc(VARS.main_form) == false)
            {
                VARS.main_form.btnDescSave.ImageKey = "save";
                VARS.main_form.paramTextEditor.textWasChanged = false;
            }
            else
            {
                return; // cancel
            }

            dbName = (string)dgv.SelectedRows[0].Cells["dbmgrFile"].Value;
            action = 3;
            DATABASE.SelectDatabaseRecord(dbName);

            Close();
        }

        /// <summary>
        /// Контекстное меню: удалить пиктограмму
        /// </summary>
        private void ctxRemoveIcon_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count == 0) return;

            if (MessageBox.Show("Удалить пиктограмму для выбранной базы данных?", "Удаление",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.No)
                    return;

            string current_db = (string)dgv.SelectedRows[0].Cells["dbmgrFile"].Value;

            DATABASE.SetIconForDatabaseRecord(current_db, null, null);

            dgv.SelectedRows[0].Cells["dbmgrIcon"].Value = dbDefaultIcon;
        }

        /// <summary>
        /// Контекстное меню: добавить\изменить пиктограмму
        /// </summary>
        private void ctxAddIcon_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count == 0) return;

            string current_db = (string)dgv.SelectedRows[0].Cells["dbmgrFile"].Value;

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "BMP; PNG | *.bmp;*.png";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Image b = Image.FromFile(ofd.FileName);
                bool correctSize = (b.Width == 16 && b.Height == 16) || (b.Width == 24 && b.Height == 24);
                if (!correctSize)
                {
                    MessageBox.Show("Размер изображения должен быть (16 х 16) или (24 х 24) точек.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                DATABASE.SetIconForDatabaseRecord(current_db, Path.GetFileName(ofd.FileName), File.ReadAllBytes(ofd.FileName));

                dgv.SelectedRows[0].Cells["dbmgrIcon"].Value = b;
            }
        }

        /// <summary>
        /// Контекстное меню: сохранить пиктограмму
        /// </summary>
        private void ctxSaveIcon_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count == 0) return;

            string current_db = (string)dgv.SelectedRows[0].Cells["dbmgrFile"].Value;

            byte[] raw_data = DATABASE.GetIconRawDataForDatabaseRecord(current_db, out string filename);

            if (raw_data == null)
            {
                ImageConverter converter = new ImageConverter();
                raw_data = (byte[])converter.ConvertTo(dbDefaultIcon, typeof(byte[]));
                filename = "default.png";
            }

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = filename;
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllBytes(sfd.FileName, raw_data);
            }
        }
    }
}
