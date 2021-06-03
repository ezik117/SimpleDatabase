using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace classes_description
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

        public static void AddItem(Form1 main)
        {
            IntPtr systemMenuHandle = GetSystemMenu(main.Handle, false);
            InsertMenu(systemMenuHandle, 10, MF_BYPOSITION | MF_SEPARATOR, 0, string.Empty);
            InsertMenu(systemMenuHandle, 11, MF_BYPOSITION, _OpenOrCreateDatabaseSysMenuID, "Open or Create database...");
            
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
                        OpenFileDialog of = new OpenFileDialog();
                        of.InitialDirectory = Application.StartupPath;
                        of.Filter = "SQLite database (*.sqlite) | *.sqlite";
                        of.CheckFileExists = false;
                        if (of.ShowDialog() == DialogResult.OK)
                        {
                            bool Cancel = ClassItem.CheckForUnsavedDesc(this);
                            if (Cancel) return;
                            Cancel = PropertyItem.CheckForUnsavedDesc(this);
                            if (Cancel) return;

                            db.OpenOrCreate(Path.GetFileNameWithoutExtension(of.SafeFileName));
                            ClassItem.Load(this);
                        }
                        break;
                }
            }

            base.WndProc(ref m);
        }
    }
}
