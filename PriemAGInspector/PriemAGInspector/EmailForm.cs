using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;

namespace PriemAGInspector
{
    public partial class EmailForm : Telerik.WinControls.UI.RadForm
    {
        public EmailForm(string sEmailTo, string sEmailFrom, string sFIO)
        {
            InitializeComponent();
            this.Icon = PriemAGInspector.Properties.Resources.Mail;
            tbEmailTo.Text =  "\"" + sFIO + "\" <" + sEmailTo + ">";
            //tbEmailFrom.Text = sEmailFrom;
            if (string.IsNullOrEmpty(sEmailFrom))
            {
                string query = "SELECT Value FROM _appsettings WHERE [Key]='MainEmail'";
                string sVal = Util.BDC.GetValue(query, null).ToString();
                tbEmailFrom.Text = "\"Комиссия по приёму документов АГ СПбГУ \" <" + sVal + ">";
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (tbEmailFrom.Text.Length < 1)
            {
                
                RadMessageBox.Show("Не указан адрес отправителя", "Ошибка");
                return;
            }
            if (tbEmailTo.Text.Length < 1)
            {
                RadMessageBox.Show("Не указан адрес получателя", "Ошибка");
                return;
            }
            Util.Email(tbEmailTo.Text, tbEmailBody.Text, tbTheme.Text, tbEmailFrom.Text);
            this.Close();
        }
    }
}
