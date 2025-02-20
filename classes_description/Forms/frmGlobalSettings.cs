using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


//https://developers.google.com/api-client-library/dotnet/apis/drive/v2?hl=ru


namespace simple_database
{
    /// <summary>
    /// Форма глобальных настроек программы
    /// </summary>
    public partial class frmGlobalSettings : Form
    {
        TabPage lastTab = null;
        List<string> foldersId = new List<string>();
        bool GdDisableEvents = false;

        /// <summary>
        /// Конструктор
        /// </summary>
        public frmGlobalSettings()
        {
            InitializeComponent();
            tabControl1.TabPages.Clear();
            lastTab = tabGeneral;
            lbGroups.SelectedIndex = 0;

            LoadSettings();
        }

        /// <summary>
        /// Выбрана новая группа - покажем соответствующую вкладку с настройками
        /// </summary>
        private void lbGroups_SelectedIndexChanged(object sender, EventArgs e)
        {
            tabControl1.TabPages.Clear();

            switch (lbGroups.SelectedIndex)
            {
                case 0:
                    tabControl1.TabPages.Add(tabGeneral);
                    break;

                case 1:
                    tabControl1.TabPages.Add(tabGoogleDrive);
                    break;
            }
        }

        /// <summary>
        /// Пользователь щелкнул на выпадающем списке удаленных папок - считаем их из GoogleDrive и покажем
        /// </summary>
        async private void cbGdRemoteDir_DropDown(object sender, EventArgs e)
        {
            if (GdDisableEvents) return;
            if (tbGdSAKey.Text.Trim() == "") return;

            lbGdLoading.Visible = true;
            cbGdRemoteDir.Items.Clear();
            foldersId.Clear();

            string sakey = DATABASE.GlobalSettingsRead("Google Drive", "SAKey");
            if (sakey == "") sakey = tbGdSAKey.Text;
            if (sakey == "")
            {
                MessageBox.Show("JSON файл Drive System Account пустой.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var list = await BACKUPS.GdCloud.GetListOfCloudItems(sakey, BACKUPS.CloudListOptions.Folders);
            lbGdLoading.Visible = false;

            foreach (BACKUPS.GdItem item in list)
            {
                cbGdRemoteDir.Items.Add(item.name);
                foldersId.Add(item.id);
            }

        }

        /// <summary>
        /// Нажата кнопка сохранения настроек. В зависимости от активной вкладки (группы) сохраним настройки.
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (lbGroups.SelectedIndex == 0)
            {

            }
            else if (lbGroups.SelectedIndex == 1)
            {
                try
                {
                    string result = "";
                    string remoteDirName = (string)(cbGdRemoteDir.SelectedItem ?? "");
                    string remoteDirId = (cbGdRemoteDir.SelectedItem != null ? foldersId[cbGdRemoteDir.SelectedIndex] : "");

                    if ((result = DATABASE.GlobalSettingsWrite("Google Drive", "activate", (cbGdActivate.Checked ? "1" : "0"))) != "") throw new Exception(result);
                    if ((result = DATABASE.GlobalSettingsWrite("Google Drive", "SAKey", tbGdSAKey.Text)) != "") throw new Exception(result);
                    if ((result = DATABASE.GlobalSettingsWrite("Google Drive", "checkOnStartup", (cbGdCheckUpdatesOnStart.Checked ? "1" : "0"))) != "") throw new Exception(result);
                    if ((result = DATABASE.GlobalSettingsWrite("Google Drive", "remoteDirName", remoteDirName)) != "") throw new Exception(result);
                    if ((result = DATABASE.GlobalSettingsWrite("Google Drive", "remoteDirId", remoteDirId)) != "") throw new Exception(result);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Произошла ошибка при сохранении одного или более параметров: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                MessageBox.Show("Сохранено");
            }
        }

        /// <summary>
        /// Загрузим настройки
        /// </summary>
        private void LoadSettings()
        {
            // загрузим настройки GoogleDrive
            GdDisableEvents = true;
            BACKUPS.GdConfigInfo config = BACKUPS.GdCloud.LoadConfig();

            cbGdActivate.Checked = config.activated;
            cbGdCheckUpdatesOnStart.Checked = config.checkupdatesonstart;
            tbGdSAKey.Text = config.sakey;
            if (config.folderid != "") foldersId.Add(config.folderid);
            if (config.foldername != "")
            {
                cbGdRemoteDir.Items.Add(config.foldername);
                cbGdRemoteDir.SelectedIndex = 0;
            }

            GdDisableEvents = false;

            // ---
        }
    }
}
