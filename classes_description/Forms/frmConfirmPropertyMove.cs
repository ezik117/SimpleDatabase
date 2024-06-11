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
    public partial class frmConfirmPropertyMove : Form
    {
        public bool MoveAsNewClass = false;
        public bool Cancel = true;

        public frmConfirmPropertyMove()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (!rbMoveAsNewClass.Checked && !rbMoveToSelectedClass.Checked) return;

            MoveAsNewClass = rbMoveAsNewClass.Checked;
            Cancel = false;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Cancel = true;
            Close();
        }
    }
}
