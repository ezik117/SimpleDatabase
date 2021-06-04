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
    public partial class frmSearcProperty : Form
    {
        private TreeNode parentNode;
        private TreeView tv;
        private bool initiateSearch = true;
        private List<TreeNode> found;
        private int foundIndex;


        public frmSearcProperty()
        {
            InitializeComponent();
        }

        private void btnFindNext_Click(object sender, EventArgs e)
        {
            if (initiateSearch)
            {
                found = PropertyItem.SearchProperties(tbSearchPattern.Text, parentNode);
                initiateSearch = false;
            }

            if (foundIndex < found.Count)
            {
                tv.SelectedNode = found[foundIndex++];
                Application.DoEvents();
            }
            else
            {
                if (MessageBox.Show("Поиск закончен. Начать заново?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    InitSearch(tv, true);
                    btnFindNext.PerformClick();
                }
            }
        }

        public void InitSearch(TreeView tv, bool again)
        {
            this.tv = tv;
            if (!again) parentNode = tv.SelectedNode;
            initiateSearch = true;
            found = null;
            foundIndex = 0;
        }
    }
}
