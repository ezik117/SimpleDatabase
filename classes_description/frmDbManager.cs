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

namespace simple_database
{
    public partial class frmDbManager : Form
    {
        public string dbName;
        public int action = 0; //0-none, 1-create, 2-rename, 3-open
        public Form1 main;

        public frmDbManager()
        {
            InitializeComponent();
        }

        // Инициализация формы.
        private void frmDbManager_Load(object sender, EventArgs e)
        {
            lb.Items.Clear();
            string[] files = Directory.GetFiles($@"{Application.StartupPath}\databases");
            foreach (string file in files)
                lb.Items.Add(Path.GetFileNameWithoutExtension(file).ToLower());

            tbDbName.Text = "";
            pnlDbNameAction.Visible = false;
            if (lb.Items.Count > 0)
            {
                lb.SelectedIndex = lb.FindStringExact(dbName);
            }

            action = 0;
        }

        // Открыть БД
        private void btnOpen_Click(object sender, EventArgs e)
        {
            if (lb.SelectedIndex < 0) return;
            if (pnlDbNameAction.Visible) return;

            dbName = (string)lb.SelectedItem;
            action = 3;
            Close();
        }

        // Создать БД
        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (pnlDbNameAction.Visible) return;

            tbDbName.Text = "";
            pnlDbNameAction.Visible = true;
            tbDbName.Focus();
            action = 1;
        }

        // Переименовать БД
        private void btnRename_Click(object sender, EventArgs e)
        {
            if (lb.SelectedIndex < 0) return;
            if (pnlDbNameAction.Visible) return;

            tbDbName.Text = (string)lb.SelectedItem;
            pnlDbNameAction.Visible = true;
            tbDbName.Focus();
            action = 2;
        }

        // Удалить БД
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lb.SelectedIndex < 0) return;
            if (pnlDbNameAction.Visible) return;

            if (dbName == (string)lb.SelectedItem)
            {
                MessageBox.Show("Нельзя удалить открытую базу данных!", "Ошибка",
                                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show($"База данных '{((string)lb.SelectedItem).ToUpper()}' будет безвозратно удалена!\r\nВЫ УВЕРЕНЫ?",
                "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.No)
                return;

            try
            {
                File.Delete($@"{ Application.StartupPath}\databases\{lb.SelectedItem}.sqlite");
                lb.Items.Remove(lb.SelectedItem);
            }
            catch
            {
                MessageBox.Show("Ошибка при удалении! База данных не удалена.", "Ошибка",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Закрыть окно ввода имени БД
        private void lb_MouseDown(object sender, MouseEventArgs e)
        {
            if (pnlDbNameAction.Visible) pnlDbNameAction.Visible = false;
        }

        // Изменить или создать БД
        private void tbDbName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                pnlDbNameAction.Visible = false;
                string newDbName = tbDbName.Text.Trim().ToLower();

                if (newDbName == "") return;
                if (lb.FindStringExact(newDbName) >= 0)
                {
                    MessageBox.Show("Указанная база данных уже существует!", "Ошибка",
                                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (action == 1) // создать БД
                {
                    try
                    {
                        main.db.Create(newDbName);
                        lb.Items.Add(newDbName);
                    }
                    catch
                    {
                        MessageBox.Show("Ошибка при создании! База данных не создана.", "Ошибка",
                                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else if (action == 2) // переименовать БД
                {
                    try
                    {
                        File.Move($@"{ Application.StartupPath}\databases\{lb.SelectedItem}.sqlite",
                                  $@"{ Application.StartupPath}\databases\{newDbName}.sqlite");
                        lb.Items[lb.SelectedIndex] = newDbName;
                    }
                    catch
                    {
                        MessageBox.Show("Ошибка при переименовании! База данных не переименована.", "Ошибка",
                                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }
    }
}
