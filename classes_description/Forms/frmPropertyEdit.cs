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
    public partial class frmPropertyEdit : Form
    {
        /// <summary>
        /// Индекс иконки.
        /// При отрытии формы содержит индекс иконки редактируемого элемента.
        /// При закрытии формы содержит индекс иконки выбранной пользователем.
        /// </summary>
        public int PropertyType = -1;

        /// <summary>
        /// Индикатор, показывающий создается ли новый элемент или редактируется старый
        /// </summary>
        public bool IsItNewItem = true;

        /// <summary>
        /// Флаг возможности закрытия формы
        /// </summary>
        private bool canCloseForm = true;

        public frmPropertyEdit()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Нажата кнопка "ОК".
        /// Проверяем и закрываем форму.
        /// </summary>
        private void btnOk_Click(object sender, EventArgs e)
        {
            canCloseForm = true;

            PropertyType = (int)IconTypes.File;

            foreach (Control c in gbSpecialItems.Controls)
            {
                if (c.GetType() == typeof(RadioButton))
                {
                    if (((RadioButton)c).Checked)
                        PropertyType = ((RadioButton)c).ImageIndex;
                }
            }

            foreach (Control c in gbStandardItems.Controls)
            {
                if (c.GetType() == typeof(RadioButton))
                {
                    if (((RadioButton)c).Checked)
                        PropertyType = ((RadioButton)c).ImageIndex;
                }
            }

            if (tbPropertyName.Text.Trim() == string.Empty &&
                (PropertyType != (int)IconTypes.Attachment && PropertyType != (int)IconTypes.Plugin))
            {
                MessageBox.Show("Название элемента не может быть пустым", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                canCloseForm = false;
                return;
            }

            Close();
        }

        /// <summary>
        /// После загрузки формы, если в переменную PropertyType был загружен индекс
        /// изображения ищет соответствующую RadioButton и отмечает ее
        /// </summary>
        private void frmPropertyEdit_Load(object sender, EventArgs e)
        {
            // в режиме нового элемента можно выбрать любой тип
            // в режиме редактирования, нельзя выбрать специальные типы
            if (IsItNewItem)
            {
                gbSpecialItems.Enabled = true;
                rbFile.Checked = true;
            }
            else
            {
                gbSpecialItems.Enabled = false;

                // если это редактирование и это не специальный тип, то можно менять вид значка
                if (PropertyType != (int)IconTypes.Attachment)
                {
                    gbStandardItems.Enabled = true;
                }
                else
                {
                    gbStandardItems.Enabled = false;
                }
            }

            foreach (Control c in gbStandardItems.Controls)
            {
                if (c.GetType() == typeof(RadioButton))
                {
                    if (((RadioButton)c).ImageIndex == PropertyType)
                        ((RadioButton)c).Checked = true;
                }
            }

            foreach (Control c in gbSpecialItems.Controls)
            {
                if (c.GetType() == typeof(RadioButton))
                {
                    if (((RadioButton)c).ImageIndex == PropertyType)
                        ((RadioButton)c).Checked = true;
                }
            }
        }

        /// <summary>
        /// Форма показывается на экране, настроим параметры отображения
        /// </summary>
        private void frmPropertyEdit_Shown(object sender, EventArgs e)
        {
            tbPropertyName.SelectionStart = tbPropertyName.Text.Length;
            tbPropertyName.SelectionLength = 0;
        }

        /// <summary>
        /// Закрыть форму нажатием ESC
        /// </summary>
        private void frmPropertyEdit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) Close();
        }

        /// <summary>
        /// Сбросить все radiobuttons Специальных значков
        /// </summary>
        private void gbStandardItems_Enter(object sender, EventArgs e)
        {
            foreach (Control c in gbSpecialItems.Controls)
                if (c.GetType() == typeof(RadioButton))
                    ((RadioButton)c).Checked = false;
        }

        /// <summary>
        /// Сбросить все radiobuttons Стандартных значков
        /// </summary>
        private void gbSpecialItems_Enter(object sender, EventArgs e)
        {
            foreach (Control c in gbStandardItems.Controls)
                if (c.GetType() == typeof(RadioButton))
                    ((RadioButton)c).Checked = false;
        }

        /// <summary>
        /// Выбран тип - вложение или плагин
        /// Сбрасываем название
        /// </summary>
        private void rbAttachment_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                tbPropertyName.Text = "";
            }
        }

        /// <summary>
        /// Проверка перед закрытием формы
        /// </summary>
        private void frmPropertyEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !canCloseForm;

            canCloseForm = true;
        }


    }
}
