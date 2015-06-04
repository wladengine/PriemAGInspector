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
    public partial class HistoryCard : Telerik.WinControls.UI.RadForm
    {
        private Guid _PersonId;
        public HistoryCard(Guid PersonId)
        {
            _PersonId = PersonId;
            InitializeComponent();
            FillGrid();
        }

        private void FillGrid()
        {
            string query = "SELECT Action, OldValue, NewValue, convert(nvarchar, Time, 104) AS Time, Owner FROM PersonHistory WHERE PersonId=@PersonId ORDER BY Time";
            DataTable tbl = Util.BDC.GetDataTable(query, new Dictionary<string, object>() { { "@PersonId", _PersonId } });
            rgvHistory.DataSource = tbl;
            rgvHistory.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
            rgvHistory.BestFitColumns();
        }
    }
}
