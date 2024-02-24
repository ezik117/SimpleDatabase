using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace simple_database
{
    public partial class frmMoveBetweenDb : Form
    {
        /// <summary>
        /// Конструктор формы
        /// </summary>
        public frmMoveBetweenDb()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Кнопка: Переместить
        /// </summary>
        private void btnMove_Click(object sender, EventArgs e)
        {
            if (lbDatabases.SelectedItem == null) return;

            if (!DATABASE.Service_MoveCatalogBetweenDbs((string)lbDatabases.SelectedItem, tbNewName.Text.Trim(), (long)VARS.main_form.tvClasses.SelectedNode.Tag))
            {
                MessageBox.Show("Произошла ошибка при выполнении операции: " + DATABASE.LastError, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show("Выполнено");
            }
        }

        /// <summary>
        /// Действия при загрузке формы
        /// </summary>
        private void frmMoveBetweenDb_Load(object sender, EventArgs e)
        {
            string currentDb = Path.GetFileNameWithoutExtension(DATABASE.FileName);
            string[] files = Directory.GetFiles($@"{Application.StartupPath}\databases");
            Array.Sort(files);
            foreach (string file in files)
            {
                if (Path.GetFileNameWithoutExtension(file).ToLower() != "databases")
                {
                    string s = Path.GetFileNameWithoutExtension(file).ToLower();
                    if (s != currentDb)
                    {
                        lbDatabases.Items.Add(s);
                    }
                }
            }

            tbNewName.Text = VARS.main_form.tvClasses.SelectedNode.Text;
        }
    }
}
