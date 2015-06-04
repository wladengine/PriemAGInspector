using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data; 
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using System.Text.RegularExpressions;
namespace PriemAGInspector
{
    public partial class OlympCard : Telerik.WinControls.UI.RadForm
    {
        Guid _PersonId;
        Guid _OlympId;
        bool _isnew;
        public event OnUpdateHandler OnUpdate;

        public OlympCard(Guid id, string olympId)
        {
            this.Name = "Олимпиада";
            if (id == Guid.Empty)
            {
                RadMessageBox.Show(this, "Вы не можете добавить олимпиаду, пока не занесете абитуриента в базу.", "Ошибка данных", MessageBoxButtons.OK, RadMessageIcon.Error);
                this.Close();
            }
            else
            {
                _PersonId = id;
                _isnew = false;
                InitializeComponent();
               // FillOlympType();
                FillOlympValues();
                tbOlympDocNumber.Text = String.Empty;
                tbOlympDocSeries.Text = String.Empty;
                if ((olympId != null) && (olympId!=String.Empty))
                {
                    _OlympId = new Guid(olympId);
                    FillCombosValues();
                    CommonFunction();
                }
                else
                {
                    CommonFunction();
                    _OlympId = Guid.NewGuid();
                    _isnew = true;
                    FillOlympType(); 
                }
                
            } 
        }
        public OlympCard(Guid id)
            :this(id, null) 
        { 
        }

        private void CommonFunction()
        {
            this.ddlOlympType.SelectedIndexChanged += new Telerik.WinControls.UI.Data.PositionChangedEventHandler(this.ddlOlympType_SelectedIndexChanged);
            this.ddlOlympName.SelectedIndexChanged += new Telerik.WinControls.UI.Data.PositionChangedEventHandler(this.ddlOlympName_SelectedIndexChanged);
            this.ddlOlympSubject.SelectedIndexChanged += new Telerik.WinControls.UI.Data.PositionChangedEventHandler(this.ddlOlympSubject_SelectedIndexChanged);
        }
        private void FillOlympType()
        {
            string query = "SELECT DISTINCT [OlympType].[Id], [OlympType].[Name]  FROM [OlympBook] INNER JOIN [OlympType] ON [OlympType].Id=[OlympBook].OlympTypeId";
            DataTable tbl = Util.BDC.GetDataTable(query, null);
            Dictionary<object, string> source =
                (from DataRow rw in tbl.Rows
                 select new
                 {
                     Id = rw.Field<int>("Id"),
                     Name = rw.Field<string>("Name")
                 }).ToDictionary(x => (object)x.Id, y => y.Name);
            this.ddlOlympType.FillCombo(source);
            ddlOlympType.DropDownStyle = RadDropDownStyle.DropDownList;

        }
        private void FillOlympNames()
        {
            int TypeId = (int)this.ddlOlympType.Id();
            string query = "SELECT distinct OlympNameId AS Id, OlympName.Name FROM [OlympBook] INNER JOIN [OlympName] on OlympName.Id = OlympNameId where OlympTypeId = @OlympTypeId";
            DataTable tbl = Util.BDC.GetDataTable(query, new Dictionary<string, object> { { "@OlympTypeId", TypeId } });
            Dictionary<object, string> bind = (from DataRow rw in tbl.Rows
                                               select new
                                               {
                                                   Id = rw.Field<int>("Id"),
                                                   Name = rw.Field<string>("Name")
                                               }).ToDictionary(x => (object)x.Id, y => y.Name);
            ddlOlympName.FillCombo(bind);
            ddlOlympName.DropDownStyle = RadDropDownStyle.DropDownList;
        }
        private void FillOlympSubjects()
        {
            int TypeId = (int)ddlOlympType.Id();
            int NameId = (int)ddlOlympName.Id();
            string query = "SELECT OlympSubjectId AS Id, OlympSubject.Name FROM OlympBook INNER JOIN OlympSubject on OlympSubject.Id = OlympSubjectId where OlympTypeId = @OlympTypeId and OlympNameId = @OlympNameId";
            DataTable tbl = Util.BDC.GetDataTable(query, new Dictionary<string, object> { { "@OlympNameId", NameId }, { "@OlympTypeId", TypeId } });
            Dictionary<object, string> bind = (from DataRow rw in tbl.Rows
                                               select new
                                               {
                                                   Id = rw.Field<int>("Id"),
                                                   Name = rw.Field<string>("Name")
                                               }).ToDictionary(x => (object)x.Id, y => y.Name);
            ddlOlympSubject.FillCombo(bind);

            ddlOlympSubject.DropDownStyle = RadDropDownStyle.DropDownList;
        } 
        private void FillOlympValues()
        { 
            string query = "SELECT Id, Name FROM OlympValue";
            DataTable tbl = Util.BDC.GetDataTable(query, null);
            Dictionary<object, string> bind = (from DataRow rw in tbl.Rows
                    select new
                    {
                        Id = rw.Field<int>("Id"),
                        Name = rw.Field<string>("Name")
                    }).ToDictionary(x => (object)x.Id, y => y.Name);
            ddlOlympValue.FillCombo(bind);

            ddlOlympValue.DropDownStyle = RadDropDownStyle.DropDownList;
        }
        private void FillCombosValues()
        {
            string query = "SELECT OlympTypeId, DocumentSeries, DocumentNumber, DocumentDate, OlympNameId, OlympSubjectId, OlympValueId FROM Olympiads where Id=@Id and PersonId=@PersonId";
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("@PersonId", _PersonId);
            dict.Add("@Id", _OlympId);
            DataTable tbl = Util.BDC.GetDataTable(query, dict);
            if (tbl.Rows.Count == 0)
            {
                this.Close();
            }
            else
            {
                int OlympTypeId = (int)tbl.Rows[0].Field<int>("OlympTypeId");
                int OlympNameId = (int)tbl.Rows[0].Field<int>("OlympNameId");
                int OlympSubjectId = (int)tbl.Rows[0].Field<int>("OlympSubjectId");
                int OlympValueId = (int)tbl.Rows[0].Field<int>("OlympValueId");
                string DocumentSeries = (string)tbl.Rows[0].Field<string>("DocumentSeries");
                string DocumentNumber = (string)tbl.Rows[0].Field<string>("DocumentNumber");
                DateTime DocumentDate = (DateTime)tbl.Rows[0].Field<DateTime>("DocumentDate");

                FillOlympType();
                ddlOlympType.SelectedValue = OlympTypeId;
                FillOlympNames();
                ddlOlympName.SelectedValue = OlympNameId;
                FillOlympSubjects();
                ddlOlympSubject.SelectedValue = OlympSubjectId;
               
               
                ddlOlympValue.SelectedValue = OlympValueId;
                tbOlympDocNumber.Text = DocumentSeries;
                tbOlympDocSeries.Text = DocumentNumber;
                dtpOpympDocDate.Value = DocumentDate;
            }
        }
        private void btnOlympSave_Click(object sender, EventArgs e)
        {
            if (CheckValues())
            {
                if (_isnew)
                {
                    if (InsertNewRec())
                        UpdateRec();
                }
                else
                    UpdateRec();
                this.Close();
            }
        }
        private bool CheckValues()
        {
            if (ddlOlympType.SelectedValue == null)
                return false;
            else if (ddlOlympName.SelectedValue == null)
                return false;
            else if (ddlOlympSubject.SelectedValue == null)
                return false;
            else if (ddlOlympValue.SelectedValue == null)
                return false; 

            return true;
        }
        private void UpdateRec()
        {
            int res = 0;
            string query = "";
            Dictionary<string, object> dict = new Dictionary<string, object>();
            query = "Update [Olympiads] set ";

            dict.Add("@Id", _OlympId); query += " Id = @Id";
            dict.Add("@PersonId", _PersonId); query += ", PersonId=@PersonId";
            dict.Add("@OlympTypeId", (int)ddlOlympType.SelectedValue); query += ", OlympTypeId=@OlympTypeId";
            dict.Add("@OlympNameId", (int)ddlOlympName.SelectedValue); query += ", OlympNameId= @OlympNameId";
            dict.Add("@OlympSubjectId", (int)ddlOlympSubject.SelectedValue); query += ", OlympSubjectId= @OlympSubjectId";
            dict.Add("@OlympValueId", (int)ddlOlympValue.SelectedValue); query += ", OlympValueId=@OlympValueId";

            dict.Add("@DocumentSeries", tbOlympDocSeries.Text.ToString()); query += ", DocumentSeries=@DocumentSeries";
            dict.Add("@DocumentNumber", tbOlympDocNumber.Text.ToString()); query += ", DocumentNumber=@DocumentNumber";
            dict.Add("@DocumentDate", dtpOpympDocDate.Value); query += ", DocumentDate=@DocumentDate";

            query += " where Id = @Id";
                

            try
            {
                if (query != "")
                    res = Util.BDC.ExecuteQuery(query, dict);

                if (OnUpdate != null)
                    OnUpdate();
            }
            catch
            {
                RadMessageBox.Show("Не удалось одобрить заявление", "Ошибка", MessageBoxButtons.OK, RadMessageIcon.Error);
            }
        }
        private bool InsertNewRec()
        {
            int res = 0;
            string query = "";
            Dictionary<string, object> dict = new Dictionary<string, object>();
            
            query = "INSERT INTO [Olympiads]  ";
            query += "(Id, PersonId, OlympTypeId, OlympNameId, OlympSubjectId, OlympValueId, DocumentSeries, DocumentNumber, DocumentDate) VALUES (";

            dict.Add("@Id", _OlympId); query += "@Id";
            dict.Add("@PersonId", _PersonId); query += ", @PersonId";
            
            dict.Add("@OlympTypeId", (int)ddlOlympType.SelectedValue); query += ", @OlympTypeId";
            dict.Add("@OlympNameId", (int)ddlOlympName.SelectedValue); query += ", @OlympNameId";
            dict.Add("@OlympSubjectId", (int)ddlOlympSubject.SelectedValue); query += ", @OlympSubjectId";
            dict.Add("@OlympValueId", (int)ddlOlympValue.SelectedValue); query += ", @OlympValueId";

            dict.Add("@DocumentSeries", tbOlympDocSeries.Text.ToString()); query += ", @DocumentSeries";
            dict.Add("@DocumentNumber", tbOlympDocNumber.Text.ToString()); query += ", @DocumentNumber";
            dict.Add("@DocumentDate", dtpOpympDocDate.Value); query += ", @DocumentDate )";

            try
            {
                if (query != "")
                    res = Util.BDC.ExecuteQuery(query, dict);

                if (OnUpdate != null)
                    OnUpdate();
            }
            catch
            {
                RadMessageBox.Show("Не удалось одобрить заявление", "Ошибка", MessageBoxButtons.OK, RadMessageIcon.Error);
            }
            if (res != -1)
                return true;
            else
                return false;
        }

        private void ddlOlympType_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            FillOlympNames();
        }
        private void ddlOlympName_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            FillOlympSubjects();
        }
        private void ddlOlympSubject_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        { 
        }

    }
}
