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
            foreach (Control c in this.Controls)
            {
                if (c.GetType() == typeof(RadioButton))
                {
                    if (((RadioButton)c).Checked)
                        PropertyType = ((RadioButton)c).ImageIndex;
                }
            }
            Close();
        }

        private void frmPropertyEdit_Load(object sender, EventArgs e)
        {
            foreach (Control c in this.Controls)
            {
                if (c.GetType() == typeof(RadioButton))
                {
                    if (((RadioButton)c).ImageIndex == PropertyType)
                        ((RadioButton)c).Checked = true;
                }
            }
        }
    }
}
