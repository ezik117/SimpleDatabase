using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace classes_description
{
    public partial class frmPropertyEdit : Form
    {
        public int PropertyType = -1;

        public frmPropertyEdit()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void rbTriangle_CheckedChanged(object sender, EventArgs e)
        {
            PropertyType = Convert.ToInt16(((RadioButton)sender).Tag);
        }
    }
}
