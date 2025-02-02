using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


//https://developers.google.com/api-client-library/dotnet/apis/drive/v2?hl=ru


namespace simple_database
{
    public partial class frmGlobalSettings : Form
    {
        TabPage lastTab = null;

        public frmGlobalSettings()
        {
            InitializeComponent();
            tabControl1.TabPages.Remove(tabCloud);
            lastTab = tabGeneral;
            lbGroups.SelectedIndex = 0;
        }

        private void lbGroups_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cbWdRemoteDir_DropDown(object sender, EventArgs e)
        {

        }

        private void btnWdTest_Click(object sender, EventArgs e)
        {

        }

        private string CombineHttpPath(string s1, string s2)
        {
            string ret = s1;
            if (ret.Substring(ret.Length - 1, 1) != "/") ret += "/";
            ret += s2;
            return ret;
        }
    }
}
