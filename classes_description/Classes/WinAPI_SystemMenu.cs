using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WinAPI
{
    /// <summary>
    /// Класс предназначен для облегчения добавления и управления пунктами в системном меню.
    /// Реализация обработки событий тем не менее должна производиться в процедуре WndProc главной формы вручную
    /// </summary>
    public static class SystemMenu
    {

        /* https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-insertmenuitema
         * https://learn.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-menuiteminfoa
         * */

        /// <summary>
        /// Получение кода ошибки последней выполненной операции
        /// </summary>
        /// <returns>Код ошибки</returns>
        [DllImport("user32.dll")]
        public static extern uint GetLastError();

        /// <summary>
        /// Enables the application to access the window menu (also known as the system menu or the control menu) for copying and modifying.
        /// </summary>
        /// <param name="hWnd">A handle to the window that will own a copy of the window menu.</param>
        /// <param name="bRevert">The action to be taken. If this parameter is FALSE, GetSystemMenu returns a handle to the copy of the window
        /// menu currently in use. The copy is initially identical to the window menu, but it can be modified.
        /// If this parameter is TRUE, GetSystemMenu resets the window menu back to the default state. The previous window menu, if any, is destroyed.</param>
        /// <returns>If the bRevert parameter is FALSE, the return value is a handle to a copy of the window menu. If the bRevert parameter is TRUE,
        /// the return value is NULL.</returns>
        [DllImport("user32.dll")]
        public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        /// <summary>
        /// Inserts a new menu item into a menu, moving other items down the menu.
        /// </summary>
        /// <param name="hMenu">A handle to the menu to be changed.</param>
        /// <param name="wPosition">The menu item before which the new menu item is to be inserted, as determined by the uFlags parameter.</param>
        /// <param name="wFlags">Controls the interpretation of the uPosition parameter and the content, appearance, and behavior of the new menu item.
        /// This parameter must include one of the following required values.</param>
        /// <param name="wIDNewItem">The identifier of the new menu item or, if the uFlags parameter has the MF_POPUP flag set, a handle to the drop-down menu or submenu.</param>
        /// <param name="lpNewItem">The content of the new menu item. The interpretation of lpNewItem depends on whether the uFlags parameter includes
        /// the MF_BITMAP, MF_OWNERDRAW, or MF_STRING flag, as follows.</param>
        /// <returns>If the function succeeds, the return value is nonzero. 
        /// If the function fails, the return value is zero.To get extended error information, call GetLastError.</returns>
        [DllImport("user32.dll")]
        public static extern bool InsertMenu(IntPtr hMenu, uint wPosition, InsertMenu_uFlags wFlags, uint wIDNewItem, string lpNewItem);

        /// <summary>
        /// Inserts a new menu item at the specified position in a menu.
        /// </summary>
        /// <param name="hMenu">A handle to the menu in which the new menu item is inserted.</param>
        /// <param name="uItem">he identifier or position of the menu item before which to insert the new item. The meaning of this parameter
        /// depends on the value of fByPosition.</param>
        /// <param name="fByPosition">Controls the meaning of item. If this parameter is FALSE, item is a menu item identifier. Otherwise,
        /// it is a menu item position. See Accessing Menu Items Programmatically for more information.</param>
        /// <param name="lpmii">A pointer to a MENUITEMINFO structure that contains information about the new menu item.</param>
        /// <returns>If the function succeeds, the return value is nonzero. If the function fails, the return value is zero.
        /// To get extended error information, use the GetLastError function.</returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool InsertMenuItem(IntPtr hMenu, uint uItem, bool fByPosition, ref MENUITEMINFO lpmii);

        [StructLayout(LayoutKind.Sequential)]
        public struct MENUITEMINFO
        {
            public uint cbSize;                 
            public MenuItemInfo_fMask fMask;
            public MenuItemInfo_fType fType;
            public MenuItemInfo_fState fState;
            public uint wID;
            public IntPtr hSubMenu;
            public IntPtr hbmpChecked;
            public IntPtr hbmpUnchecked;
            public IntPtr dwItemData;
            public IntPtr dwTypeData;
            public uint cch;
            public IntPtr hbmpItem;
        };

        /// <summary>
        /// Флаги параметра wFlags функции InsertMenu
        /// </summary>
        public enum InsertMenu_uFlags : uint
        {
            MF_BYCOMMAND = 0x0,
            MF_BYPOSITION = 0x400,

            MF_BITMAP = 0x4,
            MF_CHECKED = 0x8,
            MF_DISABLED = 0x2,
            MF_ENABLED = 0x0,
            MF_GRAYED = 0x1,
            MF_MENUBARBREAK = 0x20,
            MF_MENUBREAK = 0x40,
            MF_OWNERDRAW = 0x100,
            MF_POPUP = 0x10,
            MF_SEPARATOR = 0x800,
            MF_STRING = 0x0,
            MF_UNCHECKED = 0x0
        }

        /// <summary>
        /// Флаги параметра hItem структуры MENUITEMINFO
        /// </summary>
        public enum MenuItemInfo_hItem
        {
            HBMMENU_CALLBACK = -1,              //A bitmap that is drawn by the window that owns the menu. The application must process the WM_MEASUREITEM and WM_DRAWITEM messages.
            HBMMENU_MBAR_CLOSE = 5,             //Close button for the menu bar.
            HBMMENU_MBAR_CLOSE_D = 6,           //Disabled close button for the menu bar.
            HBMMENU_MBAR_MINIMIZE = 3,          //Minimize button for the menu bar.
            HBMMENU_MBAR_MINIMIZE_D = 7,        //Disabled minimize button for the menu bar.
            HBMMENU_MBAR_RESTORE = 2,           //Restore button for the menu bar.
            HBMMENU_POPUP_CLOSE = 8,            //Close button for the submenu.
            HBMMENU_POPUP_MAXIMIZE = 10,        //Maximize button for the submenu.
            HBMMENU_POPUP_MINIMIZE = 11,        //Minimize button for the submenu.
            HBMMENU_POPUP_RESTORE = 9,          //Restore button for the submenu.
            HBMMENU_SYSTEM = 1,                 //Windows icon or the icon of the window specified in dwItemData.
        }

        /// <summary>
        /// Флаги параметра fState структуры MENUITEMINFO
        /// </summary>
        public enum MenuItemInfo_fState : uint
        {
            MFS_CHECKED = 0x00000008,           //Checks the menu item. For more information about selected menu items, see the hbmpChecked member.
            MFS_DEFAULT = 0x00001000,           //Specifies that the menu item is the default. A menu can contain only one default menu item, which is displayed in bold.
            MFS_DISABLED = 0x00000003,          //Disables the menu item and grays it so that it cannot be selected. This is equivalent to MFS_GRAYED.
            MFS_ENABLED = 0x00000000,           //Enables the menu item so that it can be selected. This is the default state.
            MFS_GRAYED = 0x00000003,            //Disables the menu item and grays it so that it cannot be selected. This is equivalent to MFS_DISABLED.
            MFS_HILITE = 0x00000080,            //Highlights the menu item.
            MFS_UNCHECKED = 0x00000000,         //Unchecks the menu item. For more information about clear menu items, see the hbmpChecked member.
            MFS_UNHILITE = 0x00000000,          //Removes the highlight from the menu item. This is the default state.
        }

        /// <summary>
        /// Флаги параметра fType структуры MENUITEMINFO
        /// </summary>
        public enum MenuItemInfo_fType : uint
        {
            MFT_BITMAP = 0x00000004,            //Displays the menu item using a bitmap. The low-order word of the dwTypeData member is the bitmap handle, and the cch member is ignored.
                                                //MFT_BITMAP is replaced by MIIM_BITMAP and hbmpItem.
            MFT_MENUBARBREAK = 0x00000020,      //Places the menu item on a new line (for a menu bar) or in a new column (for a drop-down menu, submenu, or shortcut menu). For a drop-down menu, submenu, or shortcut menu, a vertical line separates the new column from the old.
            MFT_MENUBREAK = 0x00000040,         //Places the menu item on a new line (for a menu bar) or in a new column (for a drop-down menu, submenu, or shortcut menu). For a drop-down menu, submenu, or shortcut menu, the columns are not separated by a vertical line.
            MFT_OWNERDRAW = 0x00000100,         //Assigns responsibility for drawing the menu item to the window that owns the menu. The window receives a WM_MEASUREITEM message before the menu is displayed for the first time, and a WM_DRAWITEM message whenever the appearance of the menu item must be updated. If this value is specified, the dwTypeData member contains an application-defined value.
            MFT_RADIOCHECK = 0x00000200,        //Displays selected menu items using a radio-button mark instead of a check mark if the hbmpChecked member is NULL.
            MFT_RIGHTJUSTIFY = 0x00004000,      //Right-justifies the menu item and any subsequent items. This value is valid only if the menu item is in a menu bar.
            MFT_RIGHTORDER = 0x00002000,        //Specifies that menus cascade right-to-left (the default is left-to-right). This is used to support right-to-left languages, such as Arabic and Hebrew.
            MFT_SEPARATOR = 0x00000800,         //Specifies that the menu item is a separator. A menu item separator appears as a horizontal dividing line. The dwTypeData and cch members are ignored. This value is valid only in a drop-down menu, submenu, or shortcut menu.
            MFT_STRING = 0x00000000             //Displays the menu item using a text string. The dwTypeData member is the pointer to a null-terminated string, and the cch member is the length of the string.
                                                //MFT_STRING is replaced by MIIM_STRING.
        }

        /// <summary>
        /// Флаги параметра fMask структуры MENUITEMINFO
        /// </summary>
        public enum MenuItemInfo_fMask : uint
        {
            MIIM_BITMAP = 0x00000080,           //Retrieves or sets the hbmpItem member.
            MIIM_CHECKMARKS = 0x00000008,       //Retrieves or sets the hbmpChecked and hbmpUnchecked members.
            MIIM_DATA = 0x00000020,             //Retrieves or sets the dwItemData member.
            MIIM_FTYPE = 0x00000100,            //Retrieves or sets the fType member.
            MIIM_ID = 0x00000002,               //Retrieves or sets the wID member.
            MIIM_STATE = 0x00000001,            //Retrieves or sets the fState member.
            MIIM_STRING = 0x00000040,           //Retrieves or sets the dwTypeData member.
            MIIM_SUBMENU = 0x00000004,          //Retrieves or sets the hSubMenu member.
            MIIM_TYPE = 0x00000010,             //Retrieves or sets the fType and dwTypeData members.
                                                //MIIM_TYPE is replaced by MIIM_BITMAP, MIIM_FTYPE, and MIIM_STRING.
        }

        // ----------------

        /// <summary>
        /// Вставляет текстовый пункт в системное меню
        /// </summary>
        /// <param name="form">Ссылка на объект формы в которую добавляется системное меню</param>
        /// <param name="id">ID меню, для обработки сообщений</param>
        /// <param name="position">Позиция вставляемого пункта</param>
        /// <param name="text">Текст пункта</param>
        /// <returns>True, если пункт меню добавлен, иначе false</returns>
        public static bool InsertSystemMenu_Text(Form form, uint id, uint position, string text)
        {
            IntPtr systemMenuHandle = GetSystemMenu(form.Handle, false);
            if (systemMenuHandle.Equals(IntPtr.Zero)) return false;

            MENUITEMINFO mif = new MENUITEMINFO()
            {
                cbSize = (uint)Marshal.SizeOf(typeof(MENUITEMINFO)),
                fMask = MenuItemInfo_fMask.MIIM_FTYPE |
                        MenuItemInfo_fMask.MIIM_STRING |
                        MenuItemInfo_fMask.MIIM_ID,
                fType = MenuItemInfo_fType.MFT_STRING,
                wID = id,
                dwTypeData = Marshal.StringToHGlobalAuto(text),
                cch = (uint)text.Length,
            };

            return InsertMenuItem(systemMenuHandle, position, true, ref mif);
        }

        /// <summary>
        /// Вставляет текстовый пункт с изображением в системное меню
        /// </summary>
        /// <param name="form">Ссылка на объект формы в которую добавляется системное меню</param>
        /// <param name="id">ID меню, для обработки сообщений</param>
        /// <param name="position">Позиция вставляемого пункта</param>
        /// <param name="text">Текст пункта</param>
        /// <param name="image">Изображение слева от пункта меню. Может содержать прозрачность.</param>
        /// <returns>True, если пункт меню добавлен, иначе false</returns>
        public static bool InsertSystemMenu_TextWithImage(Form form, uint id, uint position, string text, Bitmap image)
        {
            IntPtr systemMenuHandle = GetSystemMenu(form.Handle, false);
            if (systemMenuHandle.Equals(IntPtr.Zero)) return false;

            // для исправления интересного поведения: при выделении пункта меню, оно теряет прозрачность
            // поэтому мы создаем картинку с цветом меню и накладываем на нее прозрачную картинку
            Bitmap bmp = new Bitmap(16, 16, PixelFormat.Format32bppArgb);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.FillRectangle(SystemBrushes.Menu, 0, 0, 16, 16);
                g.DrawImage(image, 0, 0);
            }
            IntPtr hBitmap = bmp.GetHbitmap();

            MENUITEMINFO mif = new MENUITEMINFO()
            {
                cbSize = (uint)Marshal.SizeOf(typeof(MENUITEMINFO)),
                fMask = MenuItemInfo_fMask.MIIM_FTYPE |
                        MenuItemInfo_fMask.MIIM_STRING |
                        MenuItemInfo_fMask.MIIM_ID |
                        MenuItemInfo_fMask.MIIM_BITMAP,
                fType = MenuItemInfo_fType.MFT_STRING,
                wID = id,
                dwTypeData = Marshal.StringToHGlobalAuto(text),
                cch = (uint)text.Length,
                hbmpItem = hBitmap,
                hbmpChecked = hBitmap,
                hbmpUnchecked = hBitmap,
            };
            
            return InsertMenuItem(systemMenuHandle, position, true, ref mif);
        }

        /// <summary>
        /// Вставляет разделитель в системное меню
        /// </summary>
        /// <param name="form">Ссылка на объект формы в которую добавляется системное меню</param>
        /// <param name="id">ID меню, для обработки сообщений</param>
        /// <returns>True, если пункт меню добавлен, иначе false</returns>
        public static bool InsertSystemMenu_Separator(Form form, uint position)
        {
            IntPtr systemMenuHandle = GetSystemMenu(form.Handle, false);
            if (systemMenuHandle.Equals(IntPtr.Zero)) return false;

            MENUITEMINFO mif = new MENUITEMINFO()
            {
                cbSize = (uint)Marshal.SizeOf(typeof(MENUITEMINFO)),
                fMask = MenuItemInfo_fMask.MIIM_FTYPE,
                fType = MenuItemInfo_fType.MFT_SEPARATOR,
            };

            return InsertMenuItem(systemMenuHandle, position, true, ref mif);
        }
    }
}
