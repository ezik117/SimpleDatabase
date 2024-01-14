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
                    e.Cancel = false;
                    Close();
                }
            }
        }

        /// <summary>
        /// Кнопка: Вставить пресет
        /// </summary>
        private void btnInsertPreset_Click(object sender, EventArgs e)
        {
            contextMenuStrip1.Show(Cursor.Position);
        }

        /// <summary>
        /// Контекстное меню. Вставить пресет: VALUE
        /// </summary>
        private void ctxVALUE_Click(object sender, EventArgs e)
        {
            int pos = rtb.SelectionStart;
            rtb.SelectedText = "{#VALUE  #}";
            rtb.SelectionStart = pos + 8;
            rtb.SelectionLength = 0;
            rtb.Focus();
        }

        /// <summary>
        /// Контекстное меню. Вставить пресет: ASK
        /// </summary>
        private void ctxASK_Click(object sender, EventArgs e)
        {
            int pos = rtb.SelectionStart;
            rtb.SelectedText = "{#ASK NAME=\"\" TYPE=\"\" TEXT=\"\" VALUE=\"\" #}";
            rtb.SelectionStart = pos + 12;
            rtb.SelectionLength = 0;
            rtb.Focus();
        }

        /// <summary>
        /// Контекстное меню. Вставить пресет: SET
        /// </summary>
        private void ctxSET_Click(object sender, EventArgs e)
        {
            int pos = rtb.SelectionStart;
            rtb.SelectedText =
                "{#SET = #}";
            rtb.SelectionStart = pos + 6;
            rtb.SelectionLength = 0;
            rtb.Focus();
        }

        /// <summary>
        /// Контекстное меню. Вставить шаблон: C# класс
        /// </summary>
        private void ctxTemplates_CSharpClass_Click(object sender, EventArgs e)
        {
            rtb.SelectedText =
                "/*" + Environment.NewLine +
                "{#SET TYPE=CLASS #}" + Environment.NewLine +
                "{#SET CLASS=Plugin #}" + Environment.NewLine +
                "{#SET RUN=Start #}" + Environment.NewLine +
                "{#SET REFERENCES=System.dll; System.Windows.Forms.dll #}" + Environment.NewLine +
                "" + Environment.NewLine +
                "{#ASK NAME=\"message1\" TYPE=\"TextBox\" TEXT=\"Сообщение для вывода на экран:\" VALUE=\"Hello World!\" #}" + Environment.NewLine +
                "{#ASK NAME=\"btnOK\" TYPE=\"Button\" TEXT=\"OK\" VALUE=\"\" #}" + Environment.NewLine +
                "*/" + Environment.NewLine +
                "" + Environment.NewLine +
                "using System;" + Environment.NewLine +
                "using System.Windows.Forms;" + Environment.NewLine +
                "" + Environment.NewLine +
                "class Plugin" + Environment.NewLine +
                "{" + Environment.NewLine +
                "    public void Start()" + Environment.NewLine +
                "    {" + Environment.NewLine +
                "        if (\"pressed\" == \"{#VALUE btnOK #}\") {" + Environment.NewLine +
                "            MessageBox.Show(\"{#VALUE message1 #}\");" + Environment.NewLine +
                "        } else {" + Environment.NewLine +
                "            MessageBox.Show(\"Button was not pressed\");" + Environment.NewLine +
                "        }" + Environment.NewLine +
                "    }" + Environment.NewLine +
                "}";
        }

        /// <summary>
        /// Контекстное меню. Вставить шаблон: C# консольное приложение
        /// </summary>
        private void ctxTemplates_CSharpConsoleApp_Click(object sender, EventArgs e)
        {
            rtb.SelectedText =
                "/*" + Environment.NewLine +
                "{#SET TYPE=APPLICATION #}" + Environment.NewLine +
                "{#SET REFERENCES=System.dll; #}" + Environment.NewLine +
                "" + Environment.NewLine +
                "{#ASK NAME=\"label1\" TYPE=\"Label\" TEXT=\"Вычисление суммы двух чисел\" VALUE=\"\" #}" + Environment.NewLine +
                "{#ASK NAME=\"valX\" TYPE=\"TextBox\" TEXT=\"X=\" VALUE=\"1\" #}" + Environment.NewLine +
                "{#ASK NAME=\"valY\" TYPE=\"TextBox\" TEXT=\"Y=\" VALUE=\"2\" #}" + Environment.NewLine +
                "{#ASK NAME=\"btnOK\" TYPE=\"Button\" TEXT=\"X+Y=?\" VALUE=\"\" #}" + Environment.NewLine +
                "*/" + Environment.NewLine +
                "" + Environment.NewLine +
                "using System;" + Environment.NewLine +
                "" + Environment.NewLine +
                "namespace plugins" + Environment.NewLine +
                "    {" + Environment.NewLine +
                "        class PluginSample" + Environment.NewLine +
                "        {" + Environment.NewLine +
                "            [STAThread]" + Environment.NewLine +
                "            static void Main(string[] args)" + Environment.NewLine +
                "            {" + Environment.NewLine +
                "                if (\"pressed\" != \"{#VALUE btnOK #}\") return;" + Environment.NewLine +
                "                int x = int.Parse(\"{#VALUE valX #}\");" + Environment.NewLine +
                "                int y = int.Parse(\"{#VALUE valY #}\");" + Environment.NewLine +
                "                int result = x + y;" + Environment.NewLine +
                "                Console.WriteLine(\"X+Y=\" + result.ToString());" + Environment.NewLine +
                "                Console.WriteLine(\"\");" + Environment.NewLine +
                "                Console.WriteLine(\"Press a key to exit...\");" + Environment.NewLine +
                "                Console.ReadKey(true);" + Environment.NewLine +
                "            }" + Environment.NewLine +
                "        }" + Environment.NewLine +
                "    }";
        }

        /// <summary>
        /// Действия при загрузке формы
        /// </summary>
        private void frmPluginEditor_Load(object sender, EventArgs e)
        {
            
        }
    }
}
