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
        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("user32.dll")]
        private static extern bool InsertMenu(IntPtr hMenu, Int32 wPosition, Int32 wFlags, Int32 wIDNewItem, string lpNewItem);

        public const Int32 WM_SYSCOMMAND = 0x112;
        public const Int32 MF_SEPARATOR = 0x800;
        public const Int32 MF_BYPOSITION = 0x400;
        public const Int32 MF_STRING = 0x0;

        public const Int32 _OpenOrCreateDatabaseSysMenuID = 1000;
        public const Int32 _VacuumDatabaseSysMenuID = 1001;
        public const Int32 _AbouteSysMenuID = 1002;
        public const Int32 _ChangeHistory = 1003;

        public static void AddItem(Form1 main)
        {
            IntPtr systemMenuHandle = GetSystemMenu(main.Handle, false);
            InsertMenu(systemMenuHandle, 10, MF_BYPOSITION | MF_SEPARATOR, 0, string.Empty);
            InsertMenu(systemMenuHandle, 11, MF_BYPOSITION, _OpenOrCreateDatabaseSysMenuID, "Менеджер баз данных");
            InsertMenu(systemMenuHandle, 12, MF_BYPOSITION, _VacuumDatabaseSysMenuID, "Уплотнить базу данных");
            InsertMenu(systemMenuHandle, 14, MF_BYPOSITION, _ChangeHistory, "История изменений");
            InsertMenu(systemMenuHandle, 13, MF_BYPOSITION, _AbouteSysMenuID, "О программе");

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
