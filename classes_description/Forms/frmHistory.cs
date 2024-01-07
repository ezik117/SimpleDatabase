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
    public partial class frmHistory : Form
    {
        /// <summary>
        /// Предотвращает обновление данных в DataGridView
        /// </summary>
        bool stopProcessing = true;

        /// <summary>
        /// Конструктор формы
        /// </summary>
        public frmHistory()
        {
            InitializeComponent();
            this.Text = $"История изменений (Последние {DATABASE.HISTORY_MAX_ROWS} событий)";
        }

        /// <summary>
        /// Выбрана строка - загрузим расширенную информацию
        /// </summary>
        private void dgv_SelectionChanged(object sender, EventArgs e)
        {
            if (stopProcessing) return;

            if (dgv.SelectedRows.Count != 1) return;

            lbInfo.Items.Clear();
            object o = null;
            string class_id = dgv.SelectedRows[0].Cells["histClassId"].Value.ToString();
            string row_id = dgv.SelectedRows[0].Cells["histRowId"].Value.ToString();
            string tablename = dgv.SelectedRows[0].Cells["histTablename"].Value.ToString();
            string name = dgv.SelectedRows[0].Cells["histName"].Value.ToString();
            string path = dgv.SelectedRows[0].Cells["histPath"].Value.ToString();
            string class_name = "";
            string row_name = "";

            // получение имени каталога
            o = DATABASE.GetSingleValue($"SELECT name FROM classes WHERE id={class_id}");
            if (o != null)
            {
                class_name = (string)o;
                lbInfo.Items.Add($"Текущее название каталога: {class_name}");
            }
            else
            {
                if (tablename == "classes")
                {
                    lbInfo.Items.Add($"Сохраненное название каталога: {name}");
                }
                lbInfo.Items.Add("Каталога больше не существует.");
            }

            // получение имени изменений
            if (tablename == "properties")
            {
                o = DATABASE.GetSingleValue($"SELECT name FROM properties WHERE id={row_id}");
                if (o != null)
                {
                    row_name = (string)o;
                    lbInfo.Items.Add($"Текущее название элемента оглавления: {row_name}");
                    lbInfo.Items.Add($"Сохраненное название оглавления: {name}");
                    lbInfo.Items.Add($"Сохраненный полный путь к оглавлению: {path}");
                }
                else
                {
                    lbInfo.Items.Add($"Сохраненное название оглавления: {name}");
                    lbInfo.Items.Add($"Сохраненный полный путь к оглавлению: {path}");
                    lbInfo.Items.Add("Элемента оглавления больше не существует.");
                }
            }
            else if (tablename == "attachments")
            {
                o = DATABASE.GetSingleValue($"SELECT filename FROM attachments WHERE id={row_id}");
                if (o != null)
                {
                    row_name = (string)o;
                    lbInfo.Items.Add($"Текущее название вложения: {row_name}");
                    lbInfo.Items.Add($"Сохраненное название вложения: {name}");
                }
                else
                {
                    lbInfo.Items.Add($"Сохраненное название вложения: {name}");
                    lbInfo.Items.Add("Вложения больше не существует.");
                }
            }
        }

        /// <summary>
        /// форма загружена. загрузим историю и отобразим в таблице
        /// </summary>
        private void frmHistory_Load(object sender, EventArgs e)
        {
            stopProcessing = true;
            DATABASE.LoadHistory();
            dgv.AutoGenerateColumns = false;
            dgv.DataSource = DATABASE.ds.Tables["history"].DefaultView;
            stopProcessing = false;

            dgv_SelectionChanged(null, null);
        }

        /// <summary>
        /// Подсветим строчки с разными типами сообщений
        /// </summary>
        private void dgv_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.Cells["histChange"].Value != null)
                {
                    string v = (string)row.Cells["histChange"].Value;

                    if (v.StartsWith("Создание"))
                        row.DefaultCellStyle.ForeColor = Color.Green;
                    else if (v.StartsWith("Изменение"))
                        row.DefaultCellStyle.ForeColor = Color.Blue;
                    else if (v.StartsWith("Обновление"))
                        row.DefaultCellStyle.ForeColor = Color.DarkMagenta;
                    if (v.StartsWith("Удаление"))
                        row.DefaultCellStyle.ForeColor = Color.Red;
                }
            }
        }
    }
}
