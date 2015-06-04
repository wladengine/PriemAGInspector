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

namespace PriemAGInspector
{
    public partial class AbiturientCard : Telerik.WinControls.UI.RadForm
    {
        private Guid _AbiturientId;
        private Guid _PersonId;
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
                    FillGrid();
                    btnSaveApp.Enabled = false;
                    btnSaveApp.Visible = false;
                }
                else
                {
                    SetAllFieldsEnabled();
                    SetInitFields();
                    FillGrid();
                }
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
            }
             
        }
        private void FillComboOtherSpecialization()
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

            ddlPrioritySpecialization.FillCombo(source);
        }

        private void FillGrid()
        {
            string query = "SELECT Id, FileName AS 'Имя файла', Comment AS 'Комментарий',  'общ' AS Тип FROM PersonFile WHERE PersonId=@PersonId " +
                " UNION SELECT Id, FileName AS 'Имя файла', Comment AS 'Комментарий', 'к заяв' AS Тип FROM ApplicationFile WHERE ApplicationId=@AppId ";
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("@AppId", _AbiturientId);
            dic.Add("@PersonId", _PersonId);
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
                this.Text = rw.Field<string>("Surname") ?? "" + " " + rw.Field<string>("Name") ?? ""
                    + " " + rw.Field<string>("SecondName") ?? "" + " - карточка заявления";
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
            query = "SELECT ProfileId FROM AG_qAbiturient WHERE PersonId=@PersonId AND Priority=(SELECT MAX(Priority) FROM AG_Application WHERE PersonId=@PersonId)";
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
            FillComboManualExam();
            FillComboOtherSpecialization();
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
            //sfd.InitialDirectory = Util.TemplateFolder;

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
            byte[] buffer = (byte[])Util.BDC.GetValue(query, new Dictionary<string, object>() {{"@Id", fileId}});

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
                this.Text = rw.Field<string>("Surname") ?? "" + " " + rw.Field<string>("Name") ?? ""
                    + " " + rw.Field<string>("SecondName") ?? "" + " - карточка заявления";
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
                    [Enabled], [EntryType], [HostelEduc], [DateOfDisable],
                    [EntryId], [DateOfStart], [ManualExamId], [IsApprovedByComission]) VALUES (";
                            Dictionary<string, object> dict = new Dictionary<string, object>();
                            dict.Add("@Id", _AbiturientId); query += " @Id";
                            dict.Add("@PersonId", _PersonId); query += ", @PersonId";
                            dict.Add("@Priority", 0); query += ", @Priority";
                            dict.Add("@Enabled", 1); query += ", @Enabled";
                            dict.Add("@EntryType", DBNull.Value); query += ", @EntryType";
                            dict.Add("@HostelEduc", chbHostelEduc.Checked); query += ", @HostelEduc";
                            dict.Add("@DateOfDisable", DBNull.Value); query += ", @DateOfDisable";
                            dict.Add("@EntryId", GetEntryId()); query += ", @EntryId";
                            dict.Add("@DateOfStart", DateTime.Now); query += ", @DateOfStart";
                            dict.Add("@ManualExamId", DBNull.Value); query += ", @ManualExamId";
                            dict.Add("@IsApprovedByComission", 0); query += ", @IsApprovedByComission )";
                            try
                            {
                                int res = Util.BDC.ExecuteQuery(query, dict);
                                if (res != -1)
                                {
                                    btnSaveApp.Enabled = false;
                                    btnSaveApp.Visible = false;

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
            string query = @"select Id from AG_Application where PersonId = @PersonId and EntryId = @EntryId and Enabled=1";
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
            string query = @"select Id from [AG_qAbiturient] where PersonId = @PersonId and EntryClassId=@EntryClassId and Enabled=1";
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.AddVal("@PersonId", _PersonId);
            dic.AddVal("@EntryClassId", ClassId);
            DataTable tbl = Util.BDC.GetDataTable(query, dic);
            int count = (int)tbl.Rows.Count;
            query="SELECT IntValue FROM [SchoolExitClass] where Id = @EntryClassId";
            tbl = Util.BDC.GetDataTable(query, dic);
            int Class = (int)tbl.Rows[0].Field<int>("IntValue");
            int maxcount = 0;
            switch (Class+1)
            { 
                case 8: { maxcount = 1; break; }
                case 9: { maxcount = 1; break; }
                case 10: { maxcount = 2;  break; }
                default: { return false; } 
            }
            if (count >= maxcount)
                return false;
            return true;
        }

        private bool ProgramCheck()
        {
            string query = @"select Id from AG_Application where PersonId = @PersonId and EntryId <> @EntryId and Enabled=1";
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.AddVal("@PersonId", _PersonId);
            dic.AddVal("@EntryId", GetEntryId());
            DataTable tbl = Util.BDC.GetDataTable(query, dic);
            int count = (int)tbl.Rows.Count;
            if (count > 0)
                return false;
            return true;
        }

        private void btnLoadFile_Click(object sender, EventArgs e)
        {
            LoadFilesCard filCard= new LoadFilesCard(_PersonId);
            filCard.OnUpdate += FillGrid;
            if (!filCard.IsDisposed)
                filCard.Show();
             
        }
    }
}
