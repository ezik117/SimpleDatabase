using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace simple_database
{
    /// <summary>
    /// Класс для добавления дополнительных пунктов меню в системное (левый верхний угол главного окна).
    /// </summary>
    public static class SystemMenu
    {
        public const Int32 WM_SYSCOMMAND = 0x112;

        public const Int32 _OpenOrCreateDatabaseSysMenuID = 1000;
        public const Int32 _VacuumDatabaseSysMenuID = 1001;
        public const Int32 _AbouteSysMenuID = 1002;
        public const Int32 _ChangeHistory = 1003;

        public static void AddItems(Form1 main)
        {
            WinAPI.SystemMenu.InsertSystemMenu_Separator(main, 10);
            WinAPI.SystemMenu.InsertSystemMenu_TextWithImage(main, _OpenOrCreateDatabaseSysMenuID, 11, "Менеджер баз данных", Properties.Resources.db_manager3_16);
            WinAPI.SystemMenu.InsertSystemMenu_TextWithImage(main, _VacuumDatabaseSysMenuID, 12, "Уплотнить базу данных", Properties.Resources.vacuum_16);
            WinAPI.SystemMenu.InsertSystemMenu_TextWithImage(main, _ChangeHistory, 13, "История изменений", Properties.Resources.history_16);
            WinAPI.SystemMenu.InsertSystemMenu_TextWithImage(main, _AbouteSysMenuID, 14, "О программе", Properties.Resources.about_16);
        }
    }

    // Обработка дополнительного меню
    public partial class Form1 : Form
    {
        protected override void WndProc(ref Message m)
        {
            // Check if a System Command has been executed
            if (m.Msg == SystemMenu.WM_SYSCOMMAND)
            {
                // Execute the appropriate code for the System Menu item that was clicked
                switch (m.WParam.ToInt32())
                {
                    case SystemMenu._OpenOrCreateDatabaseSysMenuID:
                        frmDbManager frm = new frmDbManager();
                        frm.dbName = Path.GetFileNameWithoutExtension(DATABASE.FileName);
                        frm.ShowDialog();
                        if (frm.action == 3)
                        {
                            DATABASE.Close();
                            DATABASE.OpenOrCreate(frm.dbName);
                            ClassItem.Load(this);

                            double db_size = DATABASE.GetOpenedDatabaseSize() / 1024.0 / 1024.0;
                            slblLastUpdate.Text = $"Last update: {DATABASE.GetLastUpdate()}  |  Size: {db_size:0.0} Mb";
                        }
                        break;

                    case SystemMenu._VacuumDatabaseSysMenuID:
                        DATABASE.Vacuum();
                        MessageBox.Show("Выполнено.");
                        break;

                    case SystemMenu._ChangeHistory:
                        frmHistory frmHist = new frmHistory();
                        frmHist.ShowDialog();
                        break;

                    case SystemMenu._AbouteSysMenuID:
                        frmAbout frmAbout = new frmAbout();
                        frmAbout.ShowDialog();
                        DATABASE.ds.Tables["history"].Clear();
                        break;
                }
            }

            base.WndProc(ref m);
        }
    }
}
