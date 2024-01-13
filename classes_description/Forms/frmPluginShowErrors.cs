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
    public partial class frmPluginShowErrors : Form
    {
        public frmPluginShowErrors()
        {
            InitializeComponent();
        }

        public void DisplayErrorMessages(List<string> errors)
        {
            tbErrors.Text = "";
            foreach(string error in errors)
            {
                tbErrors.Text += error + Environment.NewLine + Environment.NewLine;
            }
        }
    }
}
