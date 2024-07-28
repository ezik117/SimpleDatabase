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
    public partial class frmSyntaxRuleEditor : Form
    {
        public List<string> Rules = null;

        public frmSyntaxRuleEditor()
        {
            InitializeComponent();
        }

        public void PrepareRules(List<string> rules)
        {
            lbRules.Items.Clear();
            foreach (string r in rules)
            {
                lbRules.Items.Add(r);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmClassEdit frm = new frmClassEdit();
            frm.Text = "Выражение Regex";
            if (frm.ShowDialog() == DialogResult.OK)
            {
                lbRules.Items.Add(frm.tbClassName.Text);
                btnSave.ImageKey = "exclamation";
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            int idx = lbRules.SelectedIndex;
            if (idx < 0) return;

            frmClassEdit frm = new frmClassEdit();

            frm.Text = "Выражение Regex";
            frm.tbClassName.Text = (string)lbRules.Items[idx];
            if (frm.ShowDialog() == DialogResult.OK)
            {
                lbRules.Items[idx] = frm.tbClassName.Text;
                btnSave.ImageKey = "exclamation";
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int idx = lbRules.SelectedIndex;
            if (idx < 0) return;

            lbRules.Items.RemoveAt(idx);
            btnSave.ImageKey = "exclamation";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (btnSave.ImageKey == "exclamation")
            {
                btnSave.ImageKey = "Save-icon";
                Rules = new List<string>();
                foreach (string r in lbRules.Items)
                    Rules.Add(r);
            }
        }

        private void frmSyntaxRuleEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (btnSave.ImageKey == "exclamation")
            {
                DialogResult res = MessageBox.Show("Имеются несохраненные данные. Сохранить?", "Предупреждение",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button3);

                if (res == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
                else if (res == DialogResult.Yes)
                {
                    e.Cancel = false;
                    btnSave_Click(null, null); ;
                }
                else if (res == DialogResult.No)
                {
                    e.Cancel = false;
                }
            }
        }

        private void btnMoveUp_Click(object sender, EventArgs e)
        {
            int idx = lbRules.SelectedIndex;
            if (idx <= 0) return;

            string item = (string)lbRules.Items[idx];
            lbRules.Items.RemoveAt(idx);
            lbRules.Items.Insert(idx - 1, item);
            lbRules.SelectedIndex = idx - 1;
        }

        private void btnMoveDown_Click(object sender, EventArgs e)
        {
            int idx = lbRules.SelectedIndex;
            if (idx < 0 || idx == lbRules.Items.Count - 1) return;

            string item = (string)lbRules.Items[idx];
            lbRules.Items.RemoveAt(idx);
            lbRules.Items.Insert(idx + 1, item);
            lbRules.SelectedIndex = idx + 1;
        }
    }
}
