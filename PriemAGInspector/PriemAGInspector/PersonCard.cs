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
    public delegate void OnUpdateHandler();
    public partial class PersonCard : Telerik.WinControls.UI.RadForm
    {
        private Guid _PersonId;
        //private string _PersonId;
        private bool _isModified;
        public event OnUpdateHandler OnUpdate;

        public PersonCard(string id)
        {
            if (id != null)
            { 
                _PersonId = new Guid(id);
                _isModified = false;
                InitializeComponent();
                this.Icon = PriemAGInspector.Properties.Resources.Person;
                FillCombos();
                FillCard();
                FillGridOlymp();
                FillGridApplications();
            }
            else
            { 
                _isModified = true;
                InitializeComponent();
                this.Icon = PriemAGInspector.Properties.Resources.Person;
                FillCombos();
                DeleteCardReadOnly();
            }
        }

        private void FillCard()
        {
            string query = @"SELECT [Surname], [Name], [SecondName], [BirthPlace], [BirthDate], [Sex], [NationalityId],
      [PassportTypeId], [PassportSeries], [PassportNumber], [PassportAuthor], [PassportDate], [PassportCode],

      [Phone], [Mobiles], [CountryId], [RegionId], [Code], [City], [Street], [House], [Korpus], [Flat], [CodeReal],
      [CityReal], [StreetReal], [HouseReal], [KorpusReal], [FlatReal],

      [AddInfo], [Privileges], [Parents],

      [RegionEducId], [SchoolName], [SchoolNum], [SchoolExitClassId], [AvgMark], [Series], [Number], 
      [AbitHostel], [CountryEducId], [SchoolCity], 
    
      [User].Email FROM AG_qPerson INNER JOIN [User] ON [User].Id = AG_qPerson.Id WHERE AG_qPerson.Id=@Id";
            DataTable tbl = Util.BDC.GetDataTable(query, new Dictionary<string, object>() { { "@Id", _PersonId } });
            if (tbl.Rows.Count < 1)
            {
                MessageBox.Show("Данные не найдены");
                return;
            }

            DataRow rw = tbl.Rows[0];
            Surname = rw.Field<string>("Surname");
            PersonName = rw.Field<string>("Name");
            Sex = rw.Field<bool>("Sex");
            SecondName = rw.Field<string>("SecondName");
            BirthPlace = rw.Field<string>("BirthPlace");
            BirthDate = rw.Field<DateTime>("BirthDate");
            Nationality = rw.Field<int>("NationalityId");
            PassportType = rw.Field<int>("PassportTypeId");
            PassportSeries = rw.Field<string>("PassportSeries");
            PassportNumber = rw.Field<string>("PassportNumber");
            PassportDate = rw.Field<DateTime>("PassportDate");
            PassportAuthor = rw.Field<string>("PassportAuthor");
            PassportCode = rw.Field<string>("PassportCode");

            MainPhone = (rw.Field<string>("Phone") ?? "");
            AddPhone = (rw.Field<string>("Mobiles") ?? "");
            Email = rw.Field<string>("Email");

            PostCode = rw.Field<string>("Code");
            Country = rw.Field<int?>("CountryId");
            RegionId = rw.Field<int?>("RegionId");
            City = rw.Field<string>("City");
            Street = rw.Field<string>("Street");
            House = rw.Field<string>("House");
            Korpus = rw.Field<string>("Korpus");
            Flat = rw.Field<string>("Flat");

            PostIndexReal = rw.Field<string>("CodeReal");
            CityReal = rw.Field<string>("CityReal");
            StreetReal = rw.Field<string>("StreetReal");
            HouseReal = rw.Field<string>("HouseReal");
            KorpusReal = rw.Field<string>("KorpusReal");
            FlatReal = rw.Field<string>("FlatReal");

            RegionEducId = rw.Field<int?>("RegionEducId");
            SchoolExitClass = rw.Field<int>("SchoolExitClassId");
            CountryEduc = rw.Field<int?>("CountryEducId");
            SchoolName = rw.Field<string>("SchoolName");
            SchoolNum = rw.Field<string>("SchoolNum");
            SchoolCity = rw.Field<string>("SchoolCity");

            DocumentSeries = rw.Field<string>("Series");
            DocumentNumber = rw.Field<string>("Number");
        }
        private void FillCombos()
        {
            string query = "SELECT Id, Name FROM PassportType";
            DataTable tbl = Util.BDC.GetDataTable(query, null);
            Dictionary<object, string> bind = (from DataRow rw in tbl.Rows
                                               select new
                                               {
                                                   Id = rw.Field<int>("Id"),
                                                   Name = rw.Field<string>("Name")
                                               }).ToDictionary(x => (object)x.Id, y => y.Name);
            ddlPassportType.FillCombo(bind);

            query = "SELECT Id, Name FROM Country order by LevelOfUsing desc";
            tbl = Util.BDC.GetDataTable(query, null);
            bind = (from DataRow rw in tbl.Rows
                    select new
                    {
                        Id = rw.Field<int>("Id"),
                        Name = rw.Field<string>("Name")
                    }).ToDictionary(x => (object)x.Id, y => y.Name);
            ddlCountry.FillCombo(bind);
            ddlCountryEduc.FillCombo(bind);
            ddlNationality.FillCombo(bind);

            query = "SELECT Id, Name FROM Region";
            tbl = Util.BDC.GetDataTable(query, null);
            bind = (from DataRow rw in tbl.Rows
                    select new
                    {
                        Id = rw.Field<int>("Id"),
                        Name = rw.Field<string>("Name")
                    }).ToDictionary(x => (object)x.Id, y => y.Name);
            ddlRegion.FillCombo(bind); 
            ddlRegionEduc.FillCombo(bind);

            query = "SELECT Id, Name FROM SchoolExitClass ORDER BY IntValue";
            tbl = Util.BDC.GetDataTable(query, null);
            bind = (from DataRow rw in tbl.Rows
                    select new
                    {
                        Id = rw.Field<int>("Id"),
                        Name = rw.Field<string>("Name")
                    }).ToDictionary(x => (object)x.Id, y => y.Name);
            ddlSchoolExitClass.FillCombo(bind);
        }

        private void FillGridOlymp()
        {
            string query = "SELECT Id, Name AS Олимпиада, Document AS Документ FROM AG_AllOlympiads WHERE PersonId=@PersonId";
            DataTable tbl = Util.BDC.GetDataTable(query, new Dictionary<string, object>() { { "@PersonId", _PersonId } });
            rgvOlymp.DataSource = tbl;
            rgvOlymp.Columns["Id"].IsVisible = false;
            rgvOlymp.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
            rgvOlymp.Columns["Документ"].AutoSizeMode = Telerik.WinControls.UI.BestFitColumnMode.DisplayedDataCells;
        }
        private void FillGridApplications()
        {
            string query = @"SELECT Id, CommitId, EntryClassId, EntryClassName AS Класс, ProgramName AS Направление, ProfileName AS Профиль, Enabled, 
                            CASE WHEN EXISTS(SELECT * FROM AG_Application 
                WHERE AG_Application.PersonId=AG_qAbiturient.PersonId AND AG_Application.Id<>AG_qAbiturient.Id 
                AND AG_Application.Priority=2) THEN 1 ELSE 0 END AS Prior 
                            FROM AG_qAbiturient WHERE PersonId=@PersonId AND IsCommited=1 ";
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("@PersonId", _PersonId);
            if (chbActiveApplications.Checked)
                query += " AND Enabled='True' ";

            DataTable tbl = Util.BDC.GetDataTable(query, dic);
            rgvApplications.DataSource = tbl;

            
            rgvApplications.Columns["Id"].IsVisible = false;
            rgvApplications.Columns["EntryClassId"].IsVisible = false;
           // rgvApplications.Columns["Enabled"].IsVisible = false;
           // rgvApplications.Columns["Prior"].IsVisible = false;
            rgvApplications.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;

            //add a couple of sample formatting objects             
            ConditionalFormattingObject c1 = new ConditionalFormattingObject("Заявление отозвано", ConditionTypes.Equal, "False", "", true);
            c1.RowBackColor = Color.Red;
            c1.CellBackColor = Color.Red;
            rgvApplications.Columns["Enabled"].ConditionalFormattingObjectList.Add(c1);

            ConditionalFormattingObject c2 = new ConditionalFormattingObject("Приоритетное заявление", ConditionTypes.Equal, "1", "", true);
            c2.RowBackColor = Color.Gold;
            c2.CellBackColor = Color.Gold;
            rgvApplications.Columns["Prior"].ConditionalFormattingObjectList.Add(c2);

            if (rgvApplications.Rows.Count == 0 && chbActiveApplications.Checked)
            {
                chbActiveApplications.Enabled = false;
                chbActiveApplications.Checked = false;
            }
        }


        private void radCheckBox1_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            FillGridApplications();
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnHistory_Click(object sender, EventArgs e)
        {
            new HistoryCard(_PersonId).Show();
        }

        private void btnSaveChange_Click(object sender, EventArgs e)
        {
            if (SaveClick())
                CloseCardAfterSave();
        }
        protected virtual bool SaveClick()
        {
            if (btnSaveChange.Enabled)
            {
                if (_PersonId != Guid.Empty)
                {
                    if (!_isModified)
                    {
                        _isModified = true;
                        DeleteCardReadOnly();
                        return true;
                    }
                    else
                    {
                        if (CheckValues())
                        {
                            _isModified = false;
                            ReadOnlyCard();
                            if (OnUpdate != null)
                                OnUpdate();
                        }
                    }
                }
                else
                {
                    if (_isModified)
                    {
                        if (SaveRecord())
                        {
                            _isModified = false;
                            InsertRec();
                            ReadOnlyCard();
                        }
                        else
                            return false;
                    }
                    else
                    {

                    }

                }
            }
            else
            {
                return false;
            }
            return false;
        }
        protected virtual void CloseCardAfterSave()
        {
        }
        protected void DeleteCardReadOnly()
        {
            btnSaveChange.Text = "Сохранить";
            SetAllFieldsEnabled();
            SetReadOnlyFields();
        }
        protected virtual void SetAllFieldsEnabled()
        {
            foreach (RadPageViewPage page in this.radPageView1.Pages)
            {
                foreach (Control control in page.Controls)
                {
                    if (control is RadGroupBox)
                    {
                        foreach (Control ctrl in control.Controls)
                        {
                            ctrl.Enabled = true;
                        }
                    }
                    control.Enabled = true;
                }
            }
        }
        protected virtual void SetReadOnlyFields()
        {
            tbEmail.Enabled = false;
            if (_isModified)
            {
                if ("193" != ddlCountry.SelectedValue.ToString())
                { ddlRegion.Enabled = false; ddlRegion.SelectedValue = ddlCountry.SelectedValue; }
                else
                { ddlRegion.Enabled = true; }
                if ("193" != ddlCountryEduc.SelectedValue.ToString())
                { ddlRegionEduc.Enabled = false; ddlRegionEduc.SelectedValue = ddlCountryEduc.SelectedValue; }
                else
                { ddlRegionEduc.Enabled = true; }
            }
        }
        protected virtual void ReadOnlyCard()
        {
            btnSaveChange.Text = "Изменить";
            SetAllFieldsNotEnabled();
            UpdateRec();
            radPageView1.Enabled = true;
            btnSaveChange.Enabled = true;
        }
        protected virtual void SetAllFieldsNotEnabled()
        {
            foreach (RadPageViewPage page in this.radPageView1.Pages)
            {
                foreach (Control control in page.Controls)
                {
                    if (control is RadGroupBox)
                    {
                        foreach (Control ctrl in control.Controls)
                        {
                            ctrl.Enabled = false;
                        }
                    }
                    control.Enabled = false;
                }
            }
        }
        protected void UpdateRec()
        {
            int res = 0;
            string query = "";
            // Person
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("@Id", _PersonId);
            query = "UPDATE [Person] SET ";
            dict.Add("@Surname", Surname); query += "Surname=@Surname";
            dict.Add("@Name", PersonName); query += ", Name=@Name";
            dict.Add("@SecondName", SecondName); query += ", SecondName=@SecondName";
            dict.Add("@Sex", Sex); query += ", Sex=@Sex";
            dict.Add("@BirthPlace", BirthPlace); query += ", BirthPlace=@BirthPlace";
            dict.Add("@BirthDate", BirthDate); query += ", BirthDate=@BirthDate";
            dict.Add("@NationalityId", Nationality); query += ", NationalityId=@NationalityId";
            dict.Add("@PassportTypeId", PassportType); query += ", PassportTypeId=@PassportTypeId";
            dict.Add("@PassportSeries", PassportSeries); query += ", PassportSeries=@PassportSeries";
            dict.Add("@PassportNumber", PassportNumber); query += ", PassportNumber=@PassportNumber";
            dict.Add("@PassportDate", PassportDate); query += ", PassportDate=@PassportDate";
            dict.Add("@PassportAuthor", PassportAuthor); query += ", PassportAuthor=@PassportAuthor";
            dict.Add("@PassportCode", PassportCode); query += ", PassportCode=@PassportCode";
            query += "  WHERE Id=@Id"; 
            try
            {
                if (query != "")
                    res = Util.BDC.ExecuteQuery(query, dict);
            }
            catch
            {
                RadMessageBox.Show("Не удалось одобрить заявление", "Ошибка", MessageBoxButtons.OK, RadMessageIcon.Error);
            }
            if (res != -1)
            {
                //PersonContacts
                query = "UPDATE [PersonContacts] SET ";
                dict = new Dictionary<string, object>(); dict.Add("@Id", _PersonId);
                //string _Email = this.Email; dict.Add("@Email", _Email); query += " Email=@Email";

                dict.Add("@Phone", MainPhone); query += " Phone=@Phone";
                dict.Add("@Mobiles", AddPhone); query += ", Mobiles=@Mobiles";
                dict.Add("@CountryId", Country); query += ", CountryId=@CountryId";
                dict.Add("@RegionId", RegionId); query += ", RegionId=@RegionId";
                dict.Add("@Code", PostCode); query += ", Code=@Code";
                dict.Add("@City", City); query += ", City=@City";
                dict.Add("@Street", Street); query += ", Street=@Street";
                dict.Add("@House", House); query += ", House=@House";
                dict.Add("@Korpus", Korpus); query += ", Korpus=@Korpus";
                dict.Add("@Flat", Flat); query += ", Flat=@Flat";

                dict.Add("@CodeReal", PostIndexReal); query += ", CodeReal=@CodeReal";
                dict.Add("@CityReal", CityReal); query += ", CityReal=@CityReal";
                dict.Add("@StreetReal", StreetReal); query += ", StreetReal=@StreetReal";
                dict.Add("@HouseReal", HouseReal); query += ", HouseReal=@HouseReal";
                dict.Add("@KorpusReal", KorpusReal); query += ", KorpusReal=@KorpusReal";
                dict.Add("@FlatReal", FlatReal); query += ", FlatReal=@FlatReal";
                query += "  WHERE PersonId=@Id"; 
                try
                {
                    res = Util.BDC.ExecuteQuery(query, dict);
                }
                catch
                {
                    RadMessageBox.Show("Не удалось одобрить заявление", "Ошибка", MessageBoxButtons.OK, RadMessageIcon.Error);
                }
                if (res != -1)
                {
                    query = "UPDATE [PersonEducationDocument] SET ";
                    dict = new Dictionary<string, object>(); dict.Add("@Id", _PersonId);
                    dict.Add("@SchoolTypeId", 1); query += " SchoolTypeId=@SchoolTypeId";
                    dict.Add("@RegionEducId", RegionEducId); query += ", RegionEducId=@RegionEducId";
                    dict.Add("@SchoolExitClassId", SchoolExitClass); query += ", SchoolExitClassId=@SchoolExitClassId";
                    dict.Add("@CountryEducId", CountryEduc); query += ", CountryEducId=@CountryEducId";
                    dict.Add("@SchoolName", SchoolName); query += ", SchoolName=@SchoolName";
                    dict.Add("@SchoolNum", SchoolNum); query += ", SchoolNum=@SchoolNum";
                    dict.Add("@SchoolCity", SchoolCity); query += ", SchoolCity=@SchoolCity";
                    dict.Add("@Series", DocumentSeries); query += ", Series=@Series";
                    dict.Add("@Number", DocumentNumber); query += ", Number=@Number";
                    query += "  WHERE PersonId=@Id";
                    try
                    {
                        res = Util.BDC.ExecuteQuery(query, dict);
                        if (res != -1)
                            RadMessageBox.Show("Все ок", "Ошибка", MessageBoxButtons.OK, RadMessageIcon.Info);
                    }
                    catch
                    {
                        RadMessageBox.Show("Не удалось одобрить заявление", "Ошибка", MessageBoxButtons.OK, RadMessageIcon.Error);
                    }
                }
            }
        }
        protected void InsertRec()
        {
            int res = 0;
            string query = "";
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("@Id", _PersonId);

            query = "insert into [User] (Id, Email) VALUES (@Id, @Email)";
            //EMAIL!!!!!!
            dict.Add("@Email", _PersonId.ToString());
            res = Util.BDC.ExecuteQuery(query, dict);

            query = "insert into [Person] (Id, UserId, RegistrationStage, AbiturientTypeId) VALUES (@Id, @Id, 100, 1)";
            res = Util.BDC.ExecuteQuery(query, dict);
            if (res != -1)
            {
                query = "Insert into [PersonContacts] (PersonId) Values (@Id)";
                res = Util.BDC.ExecuteQuery(query, dict);
                if (res != -1)
                {
                    query = "Insert into [PersonEducationDocument] (PersonId) Values (@Id)";
                    res = Util.BDC.ExecuteQuery(query, dict);
                    if (res != -1)
                    {
                        query = "Insert into [PersonAddInfo] (PersonId) Values (@Id)";
                        Util.BDC.ExecuteQuery(query, dict);
                    }
                }
            }
        }
        private bool CheckValues()
        {
            bool result = false;
            string ans = "";
            int checkYear = DateTime.Now.Year - 12;
            if ((this.Surname == "") || (!Regex.IsMatch(this.Surname, @"^[А-Яа-яёЁ\-\s]+$")))
            {
                radPageView1.SelectedPage = radPageViewPage1;
                tbSurname.Select();
                ans = "Фамилия не введена или неправильный формат.";
            }
            else if ((this.PersonName == "") || (!Regex.IsMatch(this.PersonName, @"^[А-Яа-яёЁ\-\s]+$")))
            {
                radPageView1.SelectedPage = radPageViewPage1;
                tbName.Select();
                ans = "Имя не введено или неправильный формат.";
            }
            else if (!Regex.IsMatch(this.SecondName, @"^[А-Яа-яёЁ\-\s]*$"))
            {
                radPageView1.SelectedPage = radPageViewPage1;
                tbSecondName.Select();
                ans = "Неправильный формат.";
            }
            else if ((rbMale.IsChecked == false) && (rbFemale.IsChecked == false))
            {
                radPageView1.SelectedPage = radPageViewPage1;
                rbMale.Select(); 
                ans = "Выберите значение";
            }
            else if (this.BirthDate == null)
            {
                radPageView1.SelectedPage = radPageViewPage1;
                dtpBirthDate.Select();
                ans = "Неправильно указана дата.";
            }
            else if (this.dtpBirthDate.Value.Year > checkYear || this.dtpBirthDate.Value.Year < 1920)
            {
                radPageView1.SelectedPage = radPageViewPage1;
                dtpBirthDate.Select();
                ans = "Неправильно указана дата.";
            }
            else if (this.BirthPlace == String.Empty)
            {
                radPageView1.SelectedPage = radPageViewPage1;
                tbBirthPlace.Select();
                ans = "Введите значение.";
            } 
            else if (this.dtpPassportDate.Value.Year > DateTime.Now.Year || this.dtpPassportDate.Value.Year < 1970)
            {
                radPageView1.SelectedPage = radPageViewPage1;
                dtpPassportDate.Select();
                ans = "Неправильно указана дата.";
            }
            else if (PassportSeries.Length ==0)
            {
                radPageView1.SelectedPage = radPageViewPage1;
                tbPassportSeries.Select();
                ans = "Введите серию паспорта абитуриента";
            } 
            else if (PassportSeries.Length > 10)
            {
                radPageView1.SelectedPage = radPageViewPage1;
                tbPassportSeries.Select();
                ans = "Слишком длинное значение серии паспорта абитуриента";
            }
            else if (PassportNumber.Length == 0)
            {
                radPageView1.SelectedPage = radPageViewPage1;
                tbPassportNumber.Select();
                ans = "Введите номер паспорта абитуриента";
            }
            else if (PassportNumber.Length > 20)
            {
                radPageView1.SelectedPage = radPageViewPage1;
                tbPassportNumber.Select();
                ans = "Слишком длинное значение номера паспорта абитуриента";
            }
            else if (this.MainPhone == "")
            {
                radPageView1.SelectedPage = radPageViewPage2;
                tbMainPhone.Select();
                ans = "Введите номер телефона.";
            } 
            else if (this.PostCode == "")
            {
                radPageView1.SelectedPage = radPageViewPage2;
                tbPostCode.Select();
                ans = "Введите индекс.";
            }
            else if (this.City == "")
            {
                radPageView1.SelectedPage = radPageViewPage2;
                tbCity.Select();
                ans = "Введите город.";
            }
              
            else if (this.Street == "")
            {
                radPageView1.SelectedPage = radPageViewPage2;
                tbStreet.Select();
                ans = "Введите улицу.";
            }
            else if (this.House == "")
            {
                radPageView1.SelectedPage = radPageViewPage2;
                tbHouse.Select();
                ans = "Введите номер домa.";
            }
            else
            {
                result = true;
            }

            if (ans != "")
            {
                DialogResult ds = RadMessageBox.Show(this, ans, "Ошибка данных", MessageBoxButtons.OK, RadMessageIcon.Error);
            }
            return result;
        }

        private bool SaveRecord()
        {
            if (!CheckValues())
                return false;
            if (_PersonId == Guid.Empty)
            {
                _PersonId = Guid.NewGuid();
            }
            return true;
        }
       
        private void ddlCountryEduc_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            if (_isModified)
            {
                if ("193" != ddlCountryEduc.SelectedValue.ToString())
                { 
                    ddlRegionEduc.Enabled = false;
                    FillRegionForeign(ref ddlRegionEduc, CountryEduc.Value);
                }
                else
                { 
                    ddlRegionEduc.Enabled = true;
                    FillRegionRus(ref ddlRegionEduc);
                }
            }

        }
        private void FillRegionRus(ref RadDropDownList ddl)
        {
            string query = "SELECT Region.Id, Region.Name FROM Region WHERE [RegionNumberStringValue] IS NOT NULL";
            DataTable tbl = Util.BDC.GetDataTable(query, null);
            var bind = (from DataRow rw in tbl.Rows
                    select new
                    {
                        Id = rw.Field<int>("Id"),
                        Name = rw.Field<string>("Name")
                    }).ToDictionary(x => (object)x.Id, y => y.Name);
            ddl.FillCombo(bind); 
        }
        private void FillRegionForeign(ref RadDropDownList ddl, int CountryId)
        {
            string query = "SELECT Region.Id, Region.Name FROM Region INNER JOIN Country ON Country.RegionId=Region.Id WHERE Country.Id=@CountryId";
            
            DataTable tbl = Util.BDC.GetDataTable(query, new Dictionary<string, object>() { { "@CountryId", CountryId } });
            var bind = (from DataRow rw in tbl.Rows
                        select new
                        {
                            Id = rw.Field<int>("Id"),
                            Name = rw.Field<string>("Name")
                        }).ToDictionary(x => (object)x.Id, y => y.Name);
            ddl.FillCombo(bind); 
        }

        private void ddlCountry_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            if (_isModified)
            {
                if ("193" != ddlCountry.SelectedValue.ToString())
                { 
                    ddlRegion.Enabled = false;
                    FillRegionForeign(ref ddlRegion, Country.Value);
                }
                else
                { 
                    ddlRegion.Enabled = true;
                    FillRegionRus(ref ddlRegion);
                }
            }
        }

        private void btnOlympAdd_Click(object sender, EventArgs e)
        {
            OlympCard addOlCard = new OlympCard(_PersonId,"");
            addOlCard.OnUpdate += FillGridOlymp;
            addOlCard.Show();
            //if (!addOlCard.IsDisposed)
                
        }
        private void rgvOlymp_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            if ((rgvOlymp.CurrentCell != null) && (rgvOlymp.CurrentCell.RowIndex > -1) && (rgvOlymp.CurrentCell.RowIndex < rgvOlymp.RowCount))
            {
                string Id = rgvOlymp.Rows[rgvOlymp.CurrentCell.RowIndex].Cells["Id"].Value.ToString();
                OlympCard addOlCard = new OlympCard(_PersonId, Id);
                addOlCard.OnUpdate += FillGridOlymp;
                addOlCard.Show();
            }
        }
        private void btnOlympDelete_Click(object sender, EventArgs e)
        {
            if ((rgvOlymp.CurrentCell != null) && (rgvOlymp.CurrentCell.RowIndex > -1) && (rgvOlymp.CurrentCell.RowIndex < rgvOlymp.RowCount))
            {
                string Id = rgvOlymp.Rows[rgvOlymp.CurrentCell.RowIndex].Cells["Id"].Value.ToString();
                string query = "Delete from [Olympiads] where Id=@Id and PersonId=@PersonId";
                // Person
                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict.Add("@PersonId", _PersonId);
                dict.Add("@Id", new Guid(Id));
                try
                {
                    int res = Util.BDC.ExecuteQuery(query, dict);
                }
                catch
                {
                    RadMessageBox.Show("Не удалось удалить олимпиаду", "Ошибка", MessageBoxButtons.OK, RadMessageIcon.Error);
                }
                FillGridOlymp();
            }
        }

        private void rgvApplications_CurrentCellChanged(object sender, CurrentCellChangedEventArgs e)
        {
            if ((rgvApplications.CurrentCell != null) && (rgvApplications.CurrentCell.RowIndex > -1))
            {
                string Id = rgvApplications.Rows[rgvApplications.CurrentCell.RowIndex].Cells["EntryClassId"].Value.ToString();
                if (Id!=SchoolExitClass.ToString())
                {
                    rlAppError.Visible = true;
                    AppErrorProvider.SetError(this.rlAppError, rlAppError.Text.ToString());
                }
                else
                {
                    rlAppError.Visible = false;
                    AppErrorProvider.SetError(this.rlAppError, String.Empty);
                }
            }
        }
        private void btnNewApp_Click(object sender, EventArgs e)
        {
            var abCard = new AbiturientCard("", _PersonId);
            abCard.OnUpdate += FillGridApplications;
            if (!abCard.IsDisposed)
                abCard.Show();
        }
        private void rgvApplications_CellDoubleClick(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            string Id = rgvApplications.Rows[e.RowIndex].Cells["Id"].Value.ToString();
            Util.OpenAbiturientCard(Util.MainForm, Id, _PersonId);
        }
    }
}
