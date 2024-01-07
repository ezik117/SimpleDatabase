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
    public partial class frmAbout : Form
    {
        public frmAbout()
        {
            InitializeComponent();

            lblAbout.Text = "Универсальный справочник.\n(c)2020-2023. Ермолаев Андрей.";
            lblVersion.Text = $"Версия: {Application.ProductVersion}";
        }
    }
}
