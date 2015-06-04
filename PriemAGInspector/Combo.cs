using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace PriemAGInspector
{
    public static class Combo
    {
        public static void FillCombo(this RadDropDownListElement ddl, Dictionary<object, string> source)
        {
            //ddl.Items.Clear();
            ddl.DataSource = source;
            ddl.ValueMember = "Key";
            ddl.DisplayMember = "Value";
        }
        public static void FillCombo(this RadDropDownList ddl, Dictionary<object, string> source)
        {
            //ddl.Items.Clear();
            ddl.DataSource = source;
            ddl.ValueMember = "Key";
            ddl.DisplayMember = "Value";
        }

        public static object Id(this RadDropDownListElement ddl)
        {
            try
            {
                if (ddl.SelectedIndex == -1 && ddl.Items.Count > 0)
                    return ((Dictionary<object, string>)ddl.DataSource).First().Key;
                if (ddl.SelectedValue is KeyValuePair<object, string>)
                    return ((KeyValuePair<object, string>)ddl.SelectedValue).Key;
                else
                    return ddl.SelectedValue;
            }
            catch (Exception ex)
            {
                RadMessageBox.Show("Ошибка", "" + ex.Message, System.Windows.Forms.MessageBoxButtons.OK, RadMessageIcon.Error);
                return null;
            }
        }
        public static void Id(this RadDropDownListElement ddl, object id)
        {
            ddl.SelectedValue = id;
        }
        
        public static object Id(this RadDropDownList ddl)
        {
            try
            {
                if (ddl.SelectedIndex == -1 && ddl.Items.Count > 0)
                    return ((Dictionary<object, string>)ddl.DataSource).First().Key;
                if (ddl.SelectedValue is KeyValuePair<object, string>)
                    return ((KeyValuePair<object, string>)ddl.SelectedValue).Key;
                else
                    return ddl.SelectedValue;
            }
            catch (Exception ex)
            {
                RadMessageBox.Show("Ошибка", "" + ex.Message, System.Windows.Forms.MessageBoxButtons.OK, RadMessageIcon.Error);
                return null;
            }
        }
        public static void Id(this RadDropDownList ddl, object id)
        {
            ddl.SelectedValue = id;
        }
    }
}
