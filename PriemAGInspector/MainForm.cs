using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace PriemAGInspector
{
    public partial class MainForm : RadForm
    {
        public MainForm()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            Util.MainForm = this;

            this.statusLabel.Text = System.Environment.UserDomainName + @"\"+ System.Environment.UserName;
            Util.OpenApplicationList();
        }

        private void rmiPersons_Click(object sender, EventArgs e)
        {
            Util.OpenApplicationList();
            //new PersonList().Show();
        }
    }
}
