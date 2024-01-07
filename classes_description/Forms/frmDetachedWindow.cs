using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace simple_database
{
    public partial class frmDetachedWindow : Form
    {
        public frmDetachedWindow()
        {
            InitializeComponent();

            rtb.DetectUrls = true;
            rtb.HideSelection = false;
            rtb.AutoWordSelection = false;
        }

        /// <summary>
        /// Открытие ссылки URL по щелчку
        /// </summary>
        private void rtb_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            Process.Start(e.LinkText);
        }
    }
}
