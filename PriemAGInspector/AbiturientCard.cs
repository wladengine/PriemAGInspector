using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace PriemAGInspector
{
    public partial class AbiturientCard : Telerik.WinControls.UI.RadForm
    {
        private Guid _AbiturientId;
        private Guid _PersonId;
        private Guid _CommId;
        private string _Email;
        public event OnUpdateHandler OnUpdate;

        public AbiturientCard(string id)
            : this(id,Guid.Empty)
        { }
        public AbiturientCard(Guid id)
            : this(null, id)
        { }
        public AbiturientCard(string id, Guid personid)
        {
            if (personid == Guid.Empty)
            {
                RadMessageBox.Show(this, "Вы не можете добавить заявление, пока не занесете абитуриента в базу.", "Ошибка данных", MessageBoxButtons.OK, RadMessageIcon.Error);
                this.Close();
            }
            else
            {
                _PersonId = personid;                
                InitializeComponent();
                this.Icon = PriemAGInspector.Properties.Resources.Application;
                FillComboClass();
                if ((id != null)&&(id != string.Empty))
                {
                    _AbiturientId = new Guid (id);
                    FillCard(); 
                    btnSaveApp.Enabled = false;
                    btnSaveApp.Visible = false;
                    btnPrintApp.Enabled = true;
                    btnPrintApp.Visible = true;
                }
                else
                {
                    SetAllFieldsEnabled();
                    SetInitFields(); 
                    btnPrintApp.Visible = btnPrintApp.Enabled = false;
                }
                _CommId = GetCommitId();
                FillGrid();
            }
        }
        private void FillComboClass()
        {
            FillComboClass(null);
        } 
        private void FillComboClass(int? ExitClassId)
        {
            string query = "SELECT Id, Name FROM AG_EntryClass";
            if (ExitClassId != null)
            {
                query += " where ExitClassNum="+ExitClassId.ToString();
            }
            DataTable tbl = Util.BDC.GetDataTable(query, null);
            if (tbl.Rows.Count > 0)
            {
                Dictionary<object, string> source =
                    (from DataRow rw in tbl.Rows
                     select new
                     {
                         Id = rw.Field<int>("Id"),
                         Name = rw.Field<string>("Name")
                     }).ToDictionary(x => (object)x.Id, y => y.Name); 
                ddlClass.FillCombo(source);
            }
        }

        private void FillComboProgram()
        {
            string query = "SELECT DISTINCT ProgramId, ProgramName FROM AG_qEntry WHERE EntryClassId=@EntryClassId";
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.AddVal("@EntryClassId", ClassId);
            DataTable tbl = Util.BDC.GetDataTable(query, dic);
            Dictionary<object, string> source =
                (from DataRow rw in tbl.Rows
                 select new
                 {
                     Id = rw.Field<int>("ProgramId"),
                     Name = rw.Field<string>("ProgramName")
                 }).ToDictionary(x => (object)x.Id, y => y.Name); 
            ddlProgram.FillCombo(source);
            query = @"select ProgramId from [AG_qAbiturient] where PersonId = @PersonId and Enabled=1 and EntryClassId=@EntryClassId";
            tbl = Util.BDC.GetDataTable(query, new Dictionary<string, object> { { "@PersonId", _PersonId }, { "@EntryClassId", ClassId } });
            if (tbl.Rows.Count > 0)
            {
                ProgramId = (int)tbl.Rows[0].Field<int>("ProgramId");
            }
        }
        private void FillComboObrazProgram()
        {
            string query = "SELECT DISTINCT ObrazProgramId, ObrazProgramName FROM AG_qEntry WHERE EntryClassId=@EntryClassId AND ProgramId=@ProgramId";
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.AddVal("@EntryClassId", ClassId);
            dic.AddVal("@ProgramId", ProgramId);
            DataTable tbl = Util.BDC.GetDataTable(query, dic);
            Dictionary<object, string> source =
                (from DataRow rw in tbl.Rows
                 select new
                 {
                     Id = rw.Field<int>("ObrazProgramId"),
                     Name = rw.Field<string>("ObrazProgramName")
                 }).ToDictionary(x => (object)x.Id, y => y.Name);

            ddlObrazProgram.FillCombo(source);
             
        }
        private void FillComboProfile()
        {
            string query = "SELECT DISTINCT ProfileId, ProfileName FROM AG_qEntry WHERE EntryClassId=@EntryClassId " + 
                " AND ProgramId=@ProgramId AND ObrazProgramId=@ObrazProgramId";
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.AddVal("@EntryClassId", ClassId);
            dic.AddVal("@ProgramId", ProgramId);
            dic.AddVal("@ObrazProgramId", ObrazProgramId);
            DataTable tbl = Util.BDC.GetDataTable(query, dic);
            Dictionary<object, string> source =
                (from DataRow rw in tbl.Rows
                 select new
                 {
                     Id = rw.Field<int>("ProfileId"),
                     Name = rw.Field<string>("ProfileName")
                 }).ToDictionary(x => (object)x.Id, y => y.Name);

            ddlProfile.FillCombo(source);
        }
        private void FillComboManualExam()
        {
            Guid EntryId = GetEntryId();
            string query = "Select HasManualExams from AG_Entry where Id=@Id";
            DataTable tbl = Util.BDC.GetDataTable(query, new Dictionary<string, object> { { "@Id", EntryId } });
            if (tbl.Rows.Count > 0)
            {
                bool _HasExams =  tbl.Rows[0].Field<Boolean?>("HasManualExams") ?? false;
                if (_HasExams)
                {
                    query = "SELECT ExamId, AG_ManualExam.Name FROM AG_ManualExamInAG_Entry INNER JOIN AG_ManualExam on ExamId=AG_ManualExam.Id where EntryId = @EntryId";
                    tbl = Util.BDC.GetDataTable(query, new Dictionary<string, object> { { "@EntryId", EntryId } });
                    Dictionary<object, string> source =
                        (from DataRow rw in tbl.Rows
                         select new
                         {
                             Id = rw.Field<int>("ExamId"),
                             Name = rw.Field<string>("Name")
                         }).ToDictionary(x => (object)x.Id, y => y.Name);

                    ddlManualExam.FillCombo(source);
                }
                else
                {
                    ddlManualExam.FillCombo(null);
                    ddlManualExam.Text = "нет";
                }
            }
             
        }
        private void FillComboOtherSpecialization()
        {
            string query = @"
select  Distinct ProfileId, ProfileName, MIN(Priority) as Priority
 from (
( SELECT DISTINCT ProfileId, ProfileName, 99 as Priority FROM AG_qEntry WHERE EntryClassId=@EntryClassId 
 AND ProgramId=@ProgramId AND ObrazProgramId=@ObrazProgramId and ProfileId = @ProfileId )
UNION (
select DISTINCT ProfileId, ProfileName, Priority FROM AG_qEntry join AG_Application on AG_Application.EntryId = AG_qEntry.Id
 where PersonId = @PersonId
)) as A group by ProfileId, ProfileName";
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.AddVal("@EntryClassId", ClassId);
            dic.AddVal("@ProgramId", ProgramId);
            dic.AddVal("@ProfileId", ProfileId);
            dic.AddVal("@ObrazProgramId", ObrazProgramId);
            dic.AddVal("@PersonId", _PersonId);
            DataTable tbl = Util.BDC.GetDataTable(query, dic);
            Dictionary<object, string> source =
                (from DataRow rw in tbl.Rows
                 select new
                 {
                     Id = rw.Field<int>("ProfileId"),
                     Name = rw.Field<string>("ProfileName"),
                     Priority = rw.Field<int>("Priority")
                 }).OrderBy(x=>x.Priority).ToDictionary(x => (object)x.Id, y => y.Name);
            ddlPrioritySpecialization.FillCombo(source);
        }

        private void FillGrid()
        {
            string query = "SELECT Id, FileName AS 'Имя файла', Comment AS 'Комментарий',  'общ' AS Тип FROM PersonFile WHERE PersonId=@PersonId " +
                " UNION SELECT Id, FileName AS 'Имя файла', Comment AS 'Комментарий', 'к заяв' AS Тип FROM ApplicationFile WHERE ApplicationId=@AppId or CommitId=@CommId";
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("@AppId", _AbiturientId);
            dic.Add("@PersonId", _PersonId);
            dic.Add("@CommId", _CommId);
            DataTable tbl = Util.BDC.GetDataTable(query, dic);
            rgvFiles.DataSource = tbl;
            rgvFiles.Columns["Id"].IsVisible = false;
            rgvFiles.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
            ((GridViewTextBoxColumn)rgvFiles.Columns["Комментарий"]).Multiline = true;
            rgvFiles.BestFitColumns();
        }
        private void FillCard()
        {
            string query = "SELECT AG_qAbiturient.Id, PersonId, Surname, Name, SecondName, EntryClassId, ProgramId, ObrazProgramId, ProfileId, " + 
                " HasManualExams, ManualExamId, Enabled, DateOfStart, DateOfDisable, IsApprovedByComission, HostelEduc, [User].Email FROM AG_qAbiturient " +
                " INNER JOIN AG_qPerson AS Person ON Person.Id=AG_qAbiturient.PersonId " + 
                " INNER JOIN [User] ON [User].Id=Person.Id " +
                " WHERE AG_qAbiturient.Id=@Id";
            DataTable tbl = Util.BDC.GetDataTable(query, new Dictionary<string, object>() { { "@Id", _AbiturientId } });
            if (tbl.Rows.Count < 1)
            {
                RadMessageBox.Show("Не удалось открыть заявление");
            }
            else
            {
                DataRow rw = tbl.Rows[0];
                this.Text = (rw.Field<string>("Surname") ?? "") + " " + (rw.Field<string>("Name") ?? "") + " " + (rw.Field<string>("SecondName") ?? "") + " - карточка заявления";
                lblFIO.Text = rw.Field<string>("Surname") + " " + rw.Field<string>("Name") + " " + (rw.Field<string>("SecondName") ?? "");
                _PersonId = rw.Field<Guid>("PersonId");
                _Email = rw.Field<string>("Email");
                ClassId = rw.Field<int>("EntryClassId");
                ProgramId = rw.Field<int>("ProgramId");
                ObrazProgramId = rw.Field<int>("ObrazProgramId");
                ProfileId = rw.Field<int>("ProfileId");
                HostelEduc = rw.Field<bool>("HostelEduc");
                HasManualExam = rw.Field<bool?>("HasManualExams") ?? false;
                
                if (HasManualExam)
                {
                    FillComboManualExam();
                    //ddlManualExam.Visible = true;
                    int iManualExamId = rw.Field<int?>("ManualExamId") ?? 0;
                    if (iManualExamId == 0)
                    {
                        ddlManualExam.Text = "не указано";
                    }
                    else
                    {
                        ManualExamId = iManualExamId;
                    }
                }
                else
                {
                    ddlManualExam.Enabled = false;
                    ddlManualExam.Text = "нет";
                }
                if (rw.Field<bool>("Enabled"))
                {
                    lblApplicationStatus.Text = "Подано " + rw.Field<DateTime>("DateOfStart").ToShortDateString() + " " + rw.Field<DateTime>("DateOfStart").ToShortTimeString();
                    lblApplicationStatus.ForeColor = Color.Green;
                }
                else
                {
                    if (rw.Field<DateTime?>("DateOfDisable").HasValue)
                    {
                        lblApplicationStatus.Text = "Отозвано " + (rw.Field<DateTime>("DateOfDisable").ToShortDateString() ?? "") + " " + rw.Field<DateTime>("DateOfDisable").ToShortTimeString();
                        lblApplicationStatus.ForeColor = Color.Red;
                    }
                    else
                    {
                        lblApplicationStatus.Text = "Отозвано ";
                        lblApplicationStatus.ForeColor = Color.Red;
                    }
                }

                lblIsApprovedByComission.Visible = rw.Field<bool>("IsApprovedByComission");
            }

            //PriorityProfile
            query = "SELECT ProfileId FROM AG_qAbiturient WHERE PersonId=@PersonId AND Priority=(SELECT MIN(Priority) FROM AG_Application WHERE PersonId=@PersonId)";
            PrioritySpecializationId = (int)Util.BDC.GetValue(query, new Dictionary<string, object>() { { "@PersonId", _PersonId } });
        }

        private void ddlClass_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            FillComboProgram();
        }
        private void ddlProgram_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            FillComboObrazProgram();
        }
        private void ddlObrazProgram_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            FillComboProfile();
        }
        private void ddlProfile_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            FillComboManualExam();
            FillComboOtherSpecialization();
        }

        private void btnEmail_Click(object sender, EventArgs e)
        {
            new EmailForm(_Email, "", lblFIO.Text).ShowDialog();
        }
        private void btnApproveApplication_Click(object sender, EventArgs e)
        {
            if (_AbiturientId != Guid.Empty)
            {
                try
                {
                    string query = "UPDATE [AG_Application] SET IsApprovedByComission='true' WHERE Id=@Id";
                    int res = Util.BDC.ExecuteQuery(query, new Dictionary<string, object>() { { "@Id", _AbiturientId } });
                    if (res != -1)
                        lblIsApprovedByComission.Visible = true;
                }
                catch
                {
                    RadMessageBox.Show("Не удалось одобрить заявление", "Ошибка", MessageBoxButtons.OK, RadMessageIcon.Error);
                }
            }
        }
        private void btnHistory_Click(object sender, EventArgs e)
        {
            new HistoryCard(_PersonId).Show();
        }

        private void SetAllFieldsEnabled()
        {
            //foreach (RadPageViewPage page in this.Controls)
            //{
                foreach (Control control in this.Controls)
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
           //}
        }
        private void SetInitFields()
        {

            string query = "SELECT Surname, Person.Name, SecondName, SchoolExitClass.IntValue From [Person] INNER JOIN [PersonEducationDocument] on PersonId=Person.Id INNER JOIN SchoolExitClass on SchoolExitClassId=SchoolExitClass.Id where Person.Id=@Id";
            DataTable tbl = Util.BDC.GetDataTable(query, new Dictionary<string, object>() { { "@Id", _PersonId } });
            if (tbl.Rows.Count < 1)
            {
                RadMessageBox.Show("Не удалось открыть заявление");
            }
            else
            {
                DataRow rw = tbl.Rows[0];
                this.Text = (rw.Field<string>("Surname") ?? "") + " " + (rw.Field<string>("Name") ?? "")
                    + " " + (rw.Field<string>("SecondName") ?? "") + " - карточка заявления";
                lblFIO.Text = rw.Field<string>("Surname") + " " + rw.Field<string>("Name") + " " + (rw.Field<string>("SecondName") ?? "");
                ClassId = rw.Field<int>("IntValue");
                FillComboClass(rw.Field<int>("IntValue"));
            }
            tbl = Util.BDC.GetDataTable(@"select Email from [User] where Id=@Id", new Dictionary<string, object>() { { "@Id", _PersonId } });
            _Email = tbl.Rows[0].Field<string>("Email").ToString();
            lblApplicationStatus.Visible = false;
            lblIsApprovedByComission.Visible = false;
            btnApproveApplication.Visible = false;
        }

        private void btnSaveApp_Click(object sender, EventArgs e)
        {
            if (btnSaveApp.Enabled == true)
            {
                if (Dublicate())
                {
                    if (AppCount())
                    {
                        if (ProgramCheck())
                        { 
                            _AbiturientId = Guid.NewGuid();
                            string query = @"insert into [AG_Application] ([Id], [PersonId], [Priority],
                    [Enabled], [HostelEduc], [DateOfDisable],
                    [EntryId], [DateOfStart], [ManualExamId], [IsApprovedByComission], [isCommited], [CommitId]) VALUES (";
                            Dictionary<string, object> dict = new Dictionary<string, object>();
                            dict.Add("@Id", _AbiturientId); query += " @Id";
                            dict.Add("@PersonId", _PersonId); query += ", @PersonId";
                            dict.Add("@Priority", PriorityChanger()); query += ", @Priority";
                            dict.Add("@Enabled", 1); query += ", @Enabled";
                            dict.Add("@HostelEduc", chbHostelEduc.Checked); query += ", @HostelEduc";
                            dict.Add("@DateOfDisable", DBNull.Value); query += ", @DateOfDisable";
                            dict.Add("@EntryId", GetEntryId()); query += ", @EntryId";
                            dict.Add("@DateOfStart", DateTime.Now); query += ", @DateOfStart";
                            if (ManualExamId.HasValue)
                            {
                                dict.Add("@ManualExamId", ManualExamId.Value); query += ", @ManualExamId";
                            }
                            else
                            {
                                dict.Add("@ManualExamId", DBNull.Value); query += ", @ManualExamId";
                            }
                            dict.Add("@IsApprovedByComission", 0); query += ", @IsApprovedByComission";
                            dict.Add("@isCommited", 1); query += ", @isCommited";
                            dict.Add("@CommitId", _CommId); query += ", @CommitId )";
                            try
                            {
                                int res = Util.BDC.ExecuteQuery(query, dict);
                                if (res != -1)
                                {
                                    btnSaveApp.Enabled = false;
                                    btnSaveApp.Visible = false;
                                    btnPrintApp.Enabled = true;
                                    btnPrintApp.Visible = true;
                                    ddlManualExam.Enabled = ddlClass.Enabled = ddlObrazProgram.Enabled 
                                        = ddlProfile.Enabled = ddlPrioritySpecialization.Enabled 
                                        = ddlProgram.Enabled = chbHostelEduc.Enabled = false;
                                    UpdateCommit();
                                    lblApplicationStatus.Visible = true;
                                }

                                if (OnUpdate != null)
                                    OnUpdate();
                            }
                            catch { }
                            FillCard();
                            btnApproveApplication.Visible = true;
                        }
                        else 
                        {
                            RadMessageBox.Show("У абитуриента уже есть заявление на другое направление", "Ошибка", MessageBoxButtons.OK, RadMessageIcon.Error);
                        }
                    }
                    else
                    {
                        RadMessageBox.Show("Достигнуто макисмальное количество заявлений", "Ошибка", MessageBoxButtons.OK, RadMessageIcon.Error);
                    }
                }
                else
                {
                    RadMessageBox.Show("Такое заявление уже подавалось", "Ошибка", MessageBoxButtons.OK, RadMessageIcon.Error);
                }
            }
        }
        private void btnPrintApp_Click(object sender, EventArgs e)
        {
            if (btnPrintApp.Enabled)
            {
                string fName = Util.TemplateFolder + @"\Заявление (" + lblFIO.Text + ").pdf";
                using (FileStream fs = new FileStream(fName, FileMode.OpenOrCreate))
                {
                    BinaryWriter bw = new BinaryWriter(fs);
                    byte[] bindata;
                    bindata = GetApplicationBlockPDF_AG(_CommId, string.Format(@"{0}\Data\", Application.StartupPath));
                    bw.Write(bindata);
                    bw.Flush();
                    bw.Close(); 
                }
                System.Diagnostics.Process.Start(fName);
            }
        }
        private Guid GetEntryId()
        {
            Guid Id = new Guid();
            string query = "Select Id from AG_Entry where ProgramId=@ProgramId and ObrazProgramId=@ObrazProgramId and ProfileId=@ProfileId and "+
                " EntryClassId=@EntryClassId";
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("@ProgramId", ProgramId);
            dict.Add("@ObrazProgramId", ObrazProgramId);
            dict.Add("@ProfileId", ProfileId);
            dict.Add("@EntryClassId", ClassId);
            Id = (Guid)Util.BDC.GetValue(query, dict); 
            return Id;
        }

        private bool Dublicate()
        {
            // проверяем в нашем коммите (IsCommited = 1 - один возможный коммит) у нашего человека (PersonId) существующее заявление (EntryId) + (Enabled=1 подано)
            // если количество строк результата запроса превышает 0, то заявление уже подано
            string query = @"select Id from AG_Application where PersonId = @PersonId and EntryId = @EntryId and Enabled=1 and IsCommited=1";
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.AddVal("@PersonId", _PersonId);
            dic.AddVal("@EntryId", GetEntryId()); 
            DataTable tbl = Util.BDC.GetDataTable(query, dic);
            int count = (int)tbl.Rows.Count;
            if (count > 0)
                return false;
            return true;
        }
        private bool AppCount()
        {
            // Проверим количество заявлений: PersonId (Наш человек) + IsCommited=1 (единственный действующий коммит) + EntryClassId (выбор класса) + Enabled=1 (оно не отозвано)
            string query = @"select Id from [AG_qAbiturient] where PersonId = @PersonId and EntryClassId=@EntryClassId and Enabled=1 and IsCommited=1";
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.AddVal("@PersonId", _PersonId);
            dic.AddVal("@EntryClassId", ClassId);
            DataTable tbl = Util.BDC.GetDataTable(query, dic);
            int count = (int)tbl.Rows.Count;
            int maxcount = 2;
            if (count >= maxcount)
                return false;
            return true;
        }
        
        private bool ProgramCheck()
        {
            /* такое ограничение было снято
            // проверяем не подал ли наш человек (PersonId) заявление на другую программу (ProgramId) в этот же класс (EntryClassId)
            // (+Enabled=1 только среди не отозванных заявлений)
            string query = @"select Id from AG_qAbiturient where PersonId = @PersonId and ProgramId <> @ProgramId 
                                            and Enabled=1 AND EntryClassId=@EntryClassId";
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.AddVal("@PersonId", _PersonId);
            dic.AddVal("@ProgramId", ProgramId);
            dic.AddVal("@EntryClassId", ClassId);
            DataTable tbl = Util.BDC.GetDataTable(query, dic);
            int count = (int)tbl.Rows.Count;
            if (count > 0)
                return false;
            */
            return true;
        }
        private Guid GetCommitId()
        {
            Guid ComId = new Guid();
            // Выберем CommitId из таблицы для нашего человека (PersonId), где заявление подано в выбранный класс (EntryClassId) и оно не отозвано
            // (все неотозванные заявления в определенный класс объединяем в один коммит) [верно?]
            string query = "select CommitId from AG_qAbiturient where PersonId = @PersonId and EntryClassId=@EntryClassId and Enabled=1 and isCommited = 1";
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.AddVal("@PersonId", _PersonId);
            dic.AddVal("@EntryClassId", ClassId);
            DataTable tbl = Util.BDC.GetDataTable(query, dic);
            if (tbl.Rows.Count > 0)
            {
                if (tbl.Rows[0].Field<Guid?>("CommitId") != null)
                {
                    ComId = (Guid)tbl.Rows[0].Field<Guid>("CommitId");
                }
                else
                {
                    // [когда заявления в базе в нужный класс есть, но нет CommitId (=null)]
                    // обновим таблицу, объединим в один коммит (isCommited, CommitId) все заявления (неотозванные Enabled = 1) в выбранный класс (EntryClassId)
                    query = @"update AG_Application set isCommited = 1, CommitId = @CommitId 
                              from AG_Application INNER JOIN AG_qAbiturient on AG_Application.Id=AG_qAbiturient.Id 
                              where AG_Application.PersonId = @PersonId and AG_Application.Enabled = 1 and EntryClassId=@EntryClassId";
                    ComId = Guid.NewGuid();
                    dic.AddVal("@CommitId", ComId);
                    int res = Util.BDC.ExecuteQuery(query, dic);
                }
            }
            else
            {
                ComId = Guid.NewGuid();
            }
            return ComId;
        }
        private bool UpdateCommit()
        {
            // Для всех заявлений нашего человека (PersonId) поставить IsCommited = 0
            // где класс не равен выбранному классу (EntryClassId)
            string query = @"update AG_Application set IsCommited = 0 
                            from AG_Application INNER JOIN AG_qAbiturient on AG_Application.Id=AG_qAbiturient.Id 
                            where AG_Application.PersonId = @PersonId and EntryClassId<>@EntryClassId";
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.AddVal("@PersonId", _PersonId);
            dic.AddVal("@EntryClassId", ClassId);
            int res = Util.BDC.ExecuteQuery(query, dic);
            if (res != -1)
                return true;
            return false;
        }

        private void btnLoadFile_Click(object sender, EventArgs e)
        {
            LoadFilesCard filCard= new LoadFilesCard(_PersonId);
            filCard.OnUpdate += FillGrid;
            if (!filCard.IsDisposed)
                filCard.Show();
             
        }
        private void rgvFiles_CellDoubleClick(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            Guid Id = (Guid)rgvFiles.Rows[e.RowIndex].Cells["Id"].Value;
            string query = "SELECT FileName, FileData FROM AllFiles WHERE Id=@Id";
            DataTable tbl = Util.BDC.GetDataTable(query, new Dictionary<string, object>() { { "@Id", Id } });
            if (tbl.Rows.Count < 1)
            {
                RadMessageBox.Show("Файл не найден");
            }
            string filename = Util.TemplateFolder + tbl.Rows[0].Field<string>("FileName");
            using (FileStream fs = new FileStream(filename, FileMode.Create))
            {
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    bw.Write(tbl.Rows[0].Field<byte[]>("FileData"));
                    bw.Flush();
                    bw.Close();
                }
            }
            System.Diagnostics.Process.Start(filename);
        }
        private void btnSaveFile_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog(); 

            if (rgvFiles.SelectedRows.Count == 0)
            {
                RadMessageBox.Show("Не выбрано ни одного файла");
                return;
            }
            int iRwIndex = rgvFiles.SelectedRows[0].Index;
            string fileName = rgvFiles.Rows[iRwIndex].Cells["Имя файла"].Value.ToString();
            sfd.FileName = fileName;

            Guid fileId = (Guid)rgvFiles.Rows[iRwIndex].Cells["Id"].Value;
            string query = "SELECT FileData FROM AllFiles WHERE Id=@Id";
            byte[] buffer = (byte[])Util.BDC.GetValue(query, new Dictionary<string, object>() { { "@Id", fileId } });

            DialogResult dr = sfd.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                using (FileStream fs = new FileStream(sfd.FileName, FileMode.Create))
                {
                    using (BinaryWriter bw = new BinaryWriter(fs))
                    {
                        bw.Write(buffer);
                        bw.Flush();
                        bw.Close();
                    }
                }
            }
        }
        private int PriorityChanger()
        {
            // У нас есть заявления от абитуриента? 
             string query = @"SELECT Id 
                              FROM  AG_qAbiturient
                              WHERE PersonId=@PersonId and Enabled=1 and CommitId=@CommitId";
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.AddVal("@PersonId", _PersonId);
            dic.AddVal("@CommitId", _CommId);
            DataTable tbl = Util.BDC.GetDataTable(query, dic);
            // заявлений нет, значит, первое  будет приоритетным
            if (tbl.Rows.Count == 0)
            {
                return 1;
            }
            else
            {
                // заявления от абитуриента есть, тогда проверим, совпадает ли приоритетное с тем, на которое поступаем:
                if (PrioritySpecializationId == ProfileId)
                {
                    // очевидно, что мы меняем приоритет у уже поданного заявления, надо уточнить:
                    string question = "Вы хотите сменить приоритетную специализацию с '";
                    query = @"select ProfileName from AG_qAbiturient where PersonId = @PersonId and CommitId = @CommitId and Priority=1 and isCommited=1";
                    tbl = Util.BDC.GetDataTable(query, dic);
                    question += tbl.Rows[0].Field<string>("ProfileName");
                    question += "' на '";
                    query = @"select Name from AG_Profile where Id=@ProfileId";
                    dic.AddVal("@ProfileId", ProfileId);
                    tbl = Util.BDC.GetDataTable(query, dic);
                    question += tbl.Rows[0].Field<string>("Name") + "'?";
                    var result = RadMessageBox.Show(this, question, "", MessageBoxButtons.YesNo, RadMessageIcon.Question);
                    // да, меняем, текущее заявление более приоритетное
                    if (result == DialogResult.Yes)
                    {
                        query = @"update AG_Application set Priority=2 where PersonId=@PersonId and CommitId=@CommitId and isCommited=1";
                        Util.BDC.ExecuteQuery(query, dic);
                        return 1;
                    }
                    // нет, не меняем. Заявление (текущее) будет менее приоритетным
                    else
                    {
                        return 2;
                    }
                }
                else
                {
                    // если тещущее!= выбранному приоритетному. 
                    // у человека есть заявление на приоритеное направление? 
                    query = @"select ProfileId from AG_qAbiturient where PersonId = @PersonId and CommitId = @CommitId and Priority=1 and isCommited=1";
                    tbl = Util.BDC.GetDataTable(query, dic); 
                    if (PrioritySpecializationId == tbl.Rows[0].Field<int>("ProfileId"))
                    {
                        // если есть, то текущее - менее приоритетное
                        return 2;
                    }
                    else
                    {
                        // если его нет, то придется снова спросить, какое из двух оставить приоритетным
                        string question = "Вы хотите сменить приоритетную специализацию с '";
                        query = @"select ProfileName from AG_qAbiturient where PersonId = @PersonId and CommitId = @CommitId and Priority=1 and isCommited=1";
                        tbl = Util.BDC.GetDataTable(query, dic);
                        question += tbl.Rows[0].Field<string>("ProfileName");
                        question += "' на '";
                        query = @"select Name from AG_Profile where Id=@ProfileId";
                        dic.AddVal("@ProfileId", ProfileId);
                        tbl = Util.BDC.GetDataTable(query, dic);
                        question += tbl.Rows[0].Field<string>("Name") + "'?";
                        var result = RadMessageBox.Show(this, question, "", MessageBoxButtons.YesNo, RadMessageIcon.Question);
                        if (result == DialogResult.Yes)
                        {
                            query = @"update AG_Application set Priority=2 where PersonId=@PersonId and CommitId=@CommitId and isCommited=1";
                            Util.BDC.ExecuteQuery(query, dic);
                            return 1;
                        }
                        else
                        {
                            return 2;
                        }
                    }
                }
            }  
        }
        public static byte[] GetApplicationBlockPDF_AG(Guid commitId, string dirPath)
        {
            string query = @"select 
AG_Application.PersonId, 
AG_Application.Barcode,
AG_Program.Name as Profession,
AG_ObrazProgram.Num as ObrazProgram,
AG_Profile.Name as ObrazProgramName,
AG_Profile.Name as Specialization,
AG_EntryClass.Num as ClassNum,
(case when (ManualExamId is null) then 'нет' else AG_ManualExam.Name end) as ManualExam
from AG_Application 
join AG_Entry on AG_Entry.Id = AG_Application.EntryId
join AG_Program on AG_Entry.ProgramId = AG_Program.Id
join AG_ObrazProgram on AG_Entry.ObrazProgramId = AG_ObrazProgram.Id
join AG_Profile on AG_Entry.ProfileId = AG_Profile.Id
join AG_EntryClass on AG_Entry.EntryClassId = AG_EntryClass.Id
left join AG_ManualExam on AG_Application.ManualExamId = AG_ManualExam.Id
 
WHERE [AG_Application].CommitId=@Id and AG_Application.IsCommited =1 and AG_Application.Enabled = 1
ORDER BY [AG_Application].Priority";

            DataTable tbl = Util.BDC.GetDataTable(query, new Dictionary<string, object>() { { "@Id", commitId } });

            var abitList =
                (from DataRow rw in tbl.Rows
                 select new
                 {
                     PersonId = rw.Field<Guid?>("PersonId"),
                     Barcode = rw.Field<int>("Barcode"),
                     Profession = rw.Field<string>("Profession"),
                     ObrazProgram = rw.Field<string>("ObrazProgram"),
                     ObrazProgramName = rw.Field<string>("ObrazProgramName"),
                     Specialization = rw.Field<string>("Specialization"),
                     ClassNum = rw.Field<string>("ClassNum"),
                     ManualExam = rw.Field<string>("ManualExam")
                 }).ToList();

            Guid? PersonId = abitList.FirstOrDefault().PersonId;
            query = @"SELECT Surname, Person.Name, SecondName, PersonAddInfo.HostelEduc , [BirthDate],
PersonAddInfo.HasPrivileges, Sex,
Code, Street  ,House  ,Korpus  ,Flat, City,
Country.Name as CountryName, CountryId,
Region.Name as RegionName,
[User].Email,[Phone] ,[Mobiles],
PersonEducationDocument.SchoolName , PersonEducationDocument.SchoolNum, 
SchoolCity
FROM Person 
INNER JOIN PersonAddInfo ON PersonAddInfo.PersonId = Person.Id
INNER JOIN PersonContacts ON PersonContacts.PersonId = Person.Id 
INNER JOIN PersonEducationDocument ON PersonEducationDocument.PersonId = Person.Id
INNER JOIN Country ON Country.Id = PersonContacts.CountryId
INNER JOIN Region ON Region.Id = PersonContacts.RegionId
INNER JOIN [User] ON [User].Id = Person.Id
WHERE Person.Id=@Id";
            tbl = Util.BDC.GetDataTable(query, new Dictionary<string, object>() { { "@Id", PersonId } });

            var person =
                (from DataRow rw in tbl.Rows
                 select new
                 {
                     Surname = rw.Field<string>("Surname"),
                     Name = rw.Field<string>("Name"),
                     Sex = rw.Field<bool>("Sex"),
                     BirthDate = rw.Field<DateTime>("BirthDate"),
                     SecondName = rw.Field<string>("SecondName"),
                     HostelEduc = rw.Field<bool>("HostelEduc"),
                     HasPrivileges = rw.Field<bool>("HasPrivileges"),
                     PostCode = rw.Field<string>("Code"),
                     Street = rw.Field<string>("Street"),
                     Korpus = rw.Field<string>("Korpus"),
                     House = rw.Field<string>("House"),
                     Flat = rw.Field<string>("Flat"),
                     City = rw.Field<string>("City"),
                     Region = rw.Field<string>("RegionName"),
                     Country = rw.Field<string>("CountryName"),
                     IsRussia = rw.Field<int>("CountryId") == 193,
                     Email = rw.Field<string>("Email"),
                     Phone = rw.Field<string>("Phone"),
                     Mobiles = rw.Field<string>("Mobiles"),
                     SchoolName = rw.Field<string>("SchoolName"),
                     SchoolNum = rw.Field<string>("SchoolNum"),
                     SchoolCity = rw.Field<string>("SchoolCity")
                 }).FirstOrDefault();

            query = @"select DocumentSeries, DocumentNumber, DocumentDate,
OlympName.Name as OlympName,
OlympSubject.Name as OlympSubject
from Olympiads
join OlympName on OlympName.Id = Olympiads.OlympNameId
join OlympSubject on OlympSubject.Id = Olympiads.OlympSubjectId
where Olympiads.PersonId = PersonId ";
            tbl = Util.BDC.GetDataTable(query, new Dictionary<string, object>() { { "@Id", PersonId } });

            var Olympiads =
                (from DataRow rw in tbl.Rows
                 select new
                 {
                     DocumentSeries = rw.Field<string>("DocumentSeries"),
                     DocumentNumber = rw.Field<string>("DocumentNumber"),
                     DocumentDate = rw.Field<DateTime?>("DocumentDate"),
                     OlympName = rw.Field<string>("OlympName"),
                     OlympSubject = rw.Field<string>("OlympSubject")
                 }).ToList();

            MemoryStream ms = new MemoryStream();
            string dotName = "ApplicationAG_2015.pdf";

            byte[] templateBytes;
            using (FileStream fs = new FileStream(dirPath + dotName, FileMode.Open, FileAccess.Read))
            {
                templateBytes = new byte[fs.Length];
                fs.Read(templateBytes, 0, templateBytes.Length);
            }

            PdfReader pdfRd = new PdfReader(templateBytes);
            PdfStamper pdfStm = new PdfStamper(pdfRd, ms);
            AcroFields acrFlds = pdfStm.AcroFields;

            acrFlds.SetField("FIO", ((person.Surname ?? "") + " " + (person.Name ?? "") + " " + (person.SecondName ?? "")).Trim());
            acrFlds.SetField("ObrazProgramYear", abitList.FirstOrDefault().ObrazProgram);
            acrFlds.SetField("EntryClass", abitList.FirstOrDefault().ClassNum);
            if (person.HostelEduc)
                acrFlds.SetField("HostelAbitYes", "1");
            else
                acrFlds.SetField("HostelAbitNo", "1");

            int inc = 0;
            bool hasSecondApp = abitList.Count() > 1;
            foreach (var abit in abitList)
            {
                string code = (800000 + abit.Barcode).ToString();
                inc++;
                string i = inc.ToString();
                if (hasSecondApp)
                {
                    acrFlds.SetField("chbIsPriority" + i, "1");
                    hasSecondApp = false;
                }

                if (abit.ClassNum.IndexOf("10") < 0)
                    acrFlds.SetField("chbSchoolLevel1" + i, "1");
                else
                    acrFlds.SetField("chbSchoolLevel2" + i, "1");

                acrFlds.SetField("RegNum" + i, code);
                acrFlds.SetField("Profession" + i, abit.Profession);
                acrFlds.SetField("ObrazProgram" + i, abit.ObrazProgramName);
                acrFlds.SetField("ManualExam" + i, abit.ManualExam);
            }

            pdfStm.FormFlattening = true;
            pdfStm.Close();
            pdfRd.Close();

            List<byte[]> lstFilesBinary = new List<byte[]>();
            lstFilesBinary.Add(ms.ToArray());

            //
            foreach (var abit in abitList)
            {
                dotName = "Addon_AG.pdf";
                MemoryStream ms2 = new MemoryStream();
                using (FileStream fs = new FileStream(dirPath + dotName, FileMode.Open, FileAccess.Read))
                {
                    templateBytes = new byte[fs.Length];
                    fs.Read(templateBytes, 0, templateBytes.Length);
                }

                PdfReader pdfRd2 = new PdfReader(templateBytes);
                PdfStamper pdfStm2 = new PdfStamper(pdfRd2, ms2);
                acrFlds = pdfStm2.AcroFields;
                // Top
                acrFlds.SetField("LicenseProgram", abit.Profession);
                acrFlds.SetField("ObrazProgram", abit.ObrazProgramName);
                acrFlds.SetField("Class", abitList.FirstOrDefault().ClassNum);

                //FIO
                for (int i = 0; i < 10; i++)
                {
                    if (i < person.Surname.Length)
                        acrFlds.SetField("Surname" + i.ToString(), person.Surname[i].ToString());
                    if (i < person.Name.Length)
                        acrFlds.SetField("Name" + i.ToString(), person.Name[i].ToString());
                    if (i < person.SecondName.Length)
                        acrFlds.SetField("SecondName" + i.ToString(), person.SecondName[i].ToString());
                }
                // Birthdate
                string bDate = person.BirthDate.Year.ToString();
                for (int i = 0; i < 4; i++)
                {
                    acrFlds.SetField("Year" + i.ToString(), bDate[i].ToString());
                }
                bDate = person.BirthDate.Month.ToString();
                if (bDate.Length == 1)
                    bDate = "0" + bDate;
                for (int i = 0; i < 2; i++)
                {
                    acrFlds.SetField("Month" + i.ToString(), bDate[i].ToString());
                }
                bDate = person.BirthDate.Day.ToString();
                if (bDate.Length == 1)
                    bDate = "0" + bDate;
                for (int i = 0; i < 2; i++)
                    acrFlds.SetField("Day" + i.ToString(), bDate[i].ToString());
                // Adress
                for (int i = 0; i < (person.PostCode.Length > 6 ? 6 : person.PostCode.Length); i++)
                    acrFlds.SetField("PostCode" + i.ToString(), person.PostCode[i].ToString());
                string Address =
                        string.Format("{0}{1}", (person.IsRussia ? (person.Region + ", ") ?? "" : person.Country + ", "),
                        (person.IsRussia ? (person.Region == person.City?"":person.City + ", ") : (person.City + ", ")) ?? "") 
                        +
                        string.Format("{0}{1}{2}{3}", (person.Street+" ")?? "", person.House == string.Empty ? "" : "д." + person.House+" ",
                        person.Korpus == string.Empty ? "" : "корп." + person.Korpus+" ",
                        person.Flat == string.Empty ? "" : "кв." + person.Flat + " ");
                for (int i = 0; i < 46; i++)
                {
                    if (i == Address.Length)
                        break;
                    acrFlds.SetField("Adress" + i.ToString(), Address[i].ToString());
                }

                for (int i = 0; i < 20; i++)
                {
                    if (!String.IsNullOrEmpty(person.Email))
                        if (i < person.Email.Length)
                            acrFlds.SetField("Email" + i.ToString(), person.Email[i].ToString());
                    if (!String.IsNullOrEmpty(person.Phone))
                        if (i < person.Phone.Length)
                            acrFlds.SetField("Phone" + i.ToString(), person.Phone[i].ToString());
                    if (!String.IsNullOrEmpty(person.Mobiles))
                        if (i < person.Mobiles.Length)
                            acrFlds.SetField("Mobile" + i.ToString(), person.Mobiles[i].ToString());
                }
                string SchoolNameNum = person.SchoolName + (string.IsNullOrEmpty(person.SchoolNum) ? "("+(person.SchoolNum.StartsWith("№")?person.SchoolNum:"№"+person.SchoolNum)+")":"");
                for (int i = 0; i < 20; i++)
                {
                    if (i == SchoolNameNum.Length)
                        break;
                    acrFlds.SetField("School_" + i.ToString(), SchoolNameNum[i].ToString());
                }
                string SchoolAdress = person.SchoolCity;
                for (int i = 0; i < 40; i++)
                {
                    if (i == SchoolAdress.Length)
                        break;
                    acrFlds.SetField("SchoolAdress_" + i.ToString(), SchoolAdress[i].ToString());
                } 

                pdfStm2.FormFlattening = true;
                pdfStm2.Close();
                pdfRd2.Close();
                //
                lstFilesBinary.Add(ms2.ToArray());

            }

            MemoryStream merge_ms = new MemoryStream();
            Document document = new Document(PageSize.A4);
            PdfWriter writer = PdfWriter.GetInstance(document, merge_ms);
            document.Open();

            foreach (byte[] doc in lstFilesBinary)
            {
                PdfReader reader = new PdfReader(doc);
                int n = reader.NumberOfPages;

                PdfContentByte cb = writer.DirectContent;
                PdfImportedPage page;

                for (int i = 0; i < n; i++)
                {
                    document.NewPage();
                    page = writer.GetImportedPage(reader, i + 1);
                    cb.AddTemplate(page, 1f, 0, 0, 1f, 0, 0);
                }
            }

            document.Close();
            // 

            return merge_ms.ToArray();
        }
        
    }
}
