using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace simple_database
{
    /// <summary>
    /// Класс для редактирования кода плагинов
    /// </summary>
    public partial class frmPluginEditor : Form
    {
        /// <summary>
        /// ID редактируемого оглавления плагина
        /// </summary>
        public long property_id;

        /// <summary>
        /// Конструктор
        /// </summary>
        public frmPluginEditor()
        {
            InitializeComponent();
            rtb.AcceptsTab = true;
        }

        /// <summary>
        /// Загружает в редактор текст
        /// </summary>
        public void LoadText(string plugin_code)
        {
            rtb.Text = plugin_code;
        }

        /// <summary>
        /// Изменен текст в редакторе
        /// </summary>
        private void rtb_TextChanged(object sender, EventArgs e)
        {
            if (btnSave.ImageKey == "Save-icon")
                btnSave.ImageKey = "exclamation";
        }

        /// <summary>
        /// Сохранить изменения
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (DATABASE.Plugin_Update(property_id, rtb.Text))
            {
                btnSave.ImageKey = "Save-icon";
            }
            else
            {
                MessageBox.Show("Не удалось сохранить данные.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Обработка нажатия клавиш
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rtb_KeyDown(object sender, KeyEventArgs e)
        {
            // обработка TAB
            if (e.KeyCode == Keys.Tab && !e.Control)
            {
                rtb.SelectedText = "    ";
                e.Handled = true;
                e.SuppressKeyPress = true;
                return;
            }

            // обработка CTRL-S
            if (e.KeyCode == Keys.S && e.Control)
            {
                btnSave.PerformClick();
                e.SuppressKeyPress = true;
                e.Handled = true;
                return;
            }
        }

        /// <summary>
        /// Действия при закрытии формы.
        /// </summary>
        private void frmPluginEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (btnSave.ImageKey == "exclamation")
            {
                DialogResult dr = MessageBox.Show("Имеются несохраненные данные. Сохранить?", "Предупреждение", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                if (dr == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
                else if (dr == DialogResult.No)
                {
                    e.Cancel = false;
                }
                else
                {
                    // сохраняем
                    btnSave.PerformClick();
                }
            }
        }
    }
}
