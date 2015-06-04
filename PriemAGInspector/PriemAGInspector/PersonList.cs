using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Telerik.WinControls;
using Telerik.WinControls.UI;
using WordOut;
using Excel = Microsoft.Office.Interop.Excel;


namespace PriemAGInspector
{
    public partial class PersonList : Telerik.WinControls.UI.RadRibbonForm
    {
        public int? ClassId { get; set; }
        public int? ProgramId { get; set; }
        public int? RegionId
        {
            get 
            {
                try
                {
                    if ((int)ddlRegion.Id() == -1)
                        return null;
                }
                catch { ddlRegion.SelectedIndex = -1; return null; }
                return (int)ddlRegion.Id(); 
            }
            set { ddlRegion.Id(value); }
        }

        public PersonList()
        {
            InitializeComponent();
            this.Icon = PriemAGInspector.Properties.Resources.List;
            this.ShowIcon = true;
            this.RibbonBar.StartButtonImage = Properties.Resources.school_bus_icon32px;
            InitGrid();
            FillGrid();
            FillComboClass();
            FillComboProfile();
            FillComboRegion();
        }

        private void FillComboClass()
        {
            string query = "SELECT Id, Name FROM AG_EntryClass WHERE AG_EntryClass.Id IN (SELECT EntryClassId FROM AG_qEntry) ORDER BY 2";
            System.Data.DataTable tbl = Util.BDC.GetDataTable(query, null);
            Dictionary<object, string> bind = (from DataRow rw in tbl.Rows
                                               select new
                                               {
                                                   Id = rw.Field<int>("Id"),
                                                   Name = rw.Field<string>("Name")
                                               }).ToDictionary(x => (object)x.Id, y => y.Name);

            ddlClass.FillCombo(bind);
        }
        private void FillComboProgram()
        {
            string query = "SELECT DISTINCT ProgramId, ProgramName FROM AG_qEntry WHERE EntryClassId=@EntryClassId ORDER BY 2";
            int iClassId = (int)ddlClass.Id();
            System.Data.DataTable tbl = Util.BDC.GetDataTable(query, new Dictionary<string, object>() { { "@EntryClassId", iClassId } });
            Dictionary<object, string> bind = (from DataRow rw in tbl.Rows
                                               select new
                                               {
                                                   Id = rw.Field<int>("ProgramId"),
                                                   Name = rw.Field<string>("ProgramName")
                                               }).ToDictionary(x => (object)x.Id, y => y.Name);
            
            ddlProgram.FillCombo(bind);
        }
        private void FillComboProfile()
        {
            string query = "SELECT ProfileId, ProfileName FROM AG_qEntry WHERE EntryClassId=@EntryClassId AND ProgramId=@ProgramId";
            int iProgramId = (int)ddlProgram.Id();
            int iClassId = (int)ddlClass.Id();
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("@EntryClassId", iClassId);
            dic.Add("@ProgramId", iProgramId);
            System.Data.DataTable tbl = Util.BDC.GetDataTable(query, dic);
            Dictionary<object, string> bind = (from DataRow rw in tbl.Rows
                                               select new
                                               {
                                                   Id = rw.Field<int>("ProfileId"),
                                                   Name = rw.Field<string>("ProfileName")
                                               }).ToDictionary(x => (object)x.Id, y => y.Name);
            
            ddlProfile.FillCombo(bind);
            if (bind.Count > 1 && chbProgram.Enabled && chbProgram.Checked)
                chbProfile.Enabled = true;
        }
        private void FillComboRegion()
        {
            string query = "SELECT -1 AS Id, 'Bce' AS Name UNION SELECT Id, Name FROM Region";
            System.Data.DataTable tbl = Util.BDC.GetDataTable(query, null);
            Dictionary<object, string> bind = (from DataRow rw in tbl.Rows
                                               select new
                                               {
                                                   Id = rw.Field<int>("Id"),
                                                   Name = rw.Field<string>("Name")
                                               }).ToDictionary(x => (object)x.Id, y => y.Name);

            ddlRegion.FillCombo(bind);
        }

        private void InitGrid()
        {
            //rgvList.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
            
            rgvList.Columns["FIO"].AutoSizeMode = Telerik.WinControls.UI.BestFitColumnMode.SymmaryRowCells;
            //rgvList.Columns["Name"].AutoSizeMode = Telerik.WinControls.UI.BestFitColumnMode.DisplayedCells;
            //rgvList.Columns["SecondName"].AutoSizeMode = Telerik.WinControls.UI.BestFitColumnMode.DisplayedCells;
            //rgvList.Columns["BirthDate"].AutoSizeMode = Telerik.WinControls.UI.BestFitColumnMode.DisplayedCells;

            //grouping
            ColumnGroupsViewDefinition def = new ColumnGroupsViewDefinition();
            def.ColumnGroups.Add(new GridViewColumnGroup(""));
            def.ColumnGroups[0].ShowHeader = false;
            def.ColumnGroups[0].Rows.Add(new GridViewColumnGroupRow());
            def.ColumnGroups[0].Rows[0].Columns.Add(rgvList.Columns["FIO"]);

            def.ColumnGroups.Add(new GridViewColumnGroup("Дата и место рождения"));
            def.ColumnGroups[1].ShowHeader = true;
            def.ColumnGroups[1].Rows.Add(new GridViewColumnGroupRow());
            def.ColumnGroups[1].Rows[0].Columns.Add(rgvList.Columns["BirthDate"]);
            def.ColumnGroups[1].Rows[0].Columns.Add(rgvList.Columns["BirthPlace"]);

            def.ColumnGroups.Add(new GridViewColumnGroup("Контакты"));
            def.ColumnGroups[2].ShowHeader = true;
            def.ColumnGroups[2].Rows.Add(new GridViewColumnGroupRow());
            def.ColumnGroups[2].Rows[0].Columns.Add(rgvList.Columns["RegionName"]);
            def.ColumnGroups[2].Rows[0].Columns.Add(rgvList.Columns["Phone"]);
            def.ColumnGroups[2].Rows[0].Columns.Add(rgvList.Columns["Email"]);

            def.ColumnGroups.Add(new GridViewColumnGroup(""));
            def.ColumnGroups[3].ShowHeader = false;
            def.ColumnGroups[3].Rows.Add(new GridViewColumnGroupRow());
            def.ColumnGroups[3].Rows[0].Columns.Add(rgvList.Columns["AppDate"]);

            def.ColumnGroups.Add(new GridViewColumnGroup(""));
            def.ColumnGroups[4].ShowHeader = false;
            def.ColumnGroups[4].Rows.Add(new GridViewColumnGroupRow());
            def.ColumnGroups[4].Rows[0].Columns.Add(rgvList.Columns["LastAction"]);

            rgvList.ViewDefinition = def;
        }
        private void FillGrid()
        {
            //---------!!!!!!!!!!!!!!!!!-----------------
            //заменить Person_2013 -> Person!!!
            string query = @"SELECT DISTINCT Person.Id, Surname + ' ' + Person.Name + ' ' + ISNULL(SecondName, '') AS FIO, 
convert(nvarchar,BirthDate, 104) AS BirthDate, BirthPlace, 
[Region].Name AS RegionName, PersonContacts.Phone, [User].Email,
Person.Barcode, convert(date, DateOfStart) AS AppDate,
(SELECT MAX(LoadDate) from [AllFiles] AS AF WHERE AF.PersonId=Person.Id) AS LastAction
FROM Person 
LEFT JOIN [AG_Application] ON [AG_Application].PersonId = Person.Id 
left JOIN [User] ON [User].Id = Person.Id
left JOIN [PersonContacts] ON [PersonContacts].PersonId = Person.Id 
left JOIN [PersonAddInfo] ON [PersonAddInfo].PersonId = Person.Id
INNER JOIN [AG_qEntry] ON [AG_qEntry].Id = [AG_Application].EntryId
INNER JOIN Region ON Region.Id = PersonContacts.RegionId 
WHERE Person.RegistrationStage=@RegistrationStage ";
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("@RegistrationStage", 100);
            if (chbClass.Checked)
            {
                query += " AND EntryClassId=@EntryClassId ";
                dic.Add("@EntryClassId", (int)ddlClass.Id());
            }
            if (chbProgram.Checked)
            {
                query += " AND ProgramId=@ProgramId ";
                dic.Add("@ProgramId", (int)ddlProgram.Id());
            }
            if (chbHasOlympiads.Checked)
            {
                query += " AND Person.Id IN (SELECT AG_Olympiads.PersonId FROM AG_Olympiads) ";
            }

            if (chbProfile.Checked)
            {
                query += " AND ProfileId=@ProfileId ";
                dic.Add("@ProfileId", (int)ddlProfile.Id());
            }
            if (chbHasPriveleges.Checked)
            {
                query += " AND EXISTS (SELECT Id FROM AllFiles WHERE AllFiles.PersonId=Person.Id) ";
            }
            if (chbHasTwoApps.Checked)
            {
                query += " AND EXISTS (SELECT PersonId FROM [AG_Application] Group BY PersonId, Enabled HAVING PersonId=Person.Id AND Enabled='True' AND COUNT(Id)='2' )";
            }
            if (RegionId.HasValue)
            {
                query += " AND PersonContacts.RegionId=@RegionId ";
                dic.Add("@RegionId", RegionId);
            }
            if (chbMale.Checked)
            {
                query += " AND Person.Sex=1";
            }
            if (chbFemale.Checked)
            {
                query += "AND Person.Sex=0";
            }
            if (chbHasPriveleges.Checked)
            {
                query += " AND EXISTS(SELECT * FROM AG_PersonPrivilege WHERE AG_PersonPrivilege.PersonId=Person.Id) ";
            }
            if (chbNeedHostel.Checked)
            {
                query += " AND EXISTS (SELECT * FROM AG_Application WHERE PersonId=Person.Id AND HostelEduc=1)";
            }

            string sOrderBy = " ORDER BY AppDate";

            System.Data.DataTable tbl = Util.BDC.GetDataTable(query + sOrderBy, dic);
            rgvList.DataSource = tbl;
            lblCount.Text = rgvList.Rows.Count.ToString();
            rgvList.BestFitColumns();
        }

        private void ddlClass_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            FillComboProgram();
        }
        private void ddlProgram_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            FillComboProfile();
            FillGrid();
        }
        private void ddlProfile_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            FillGrid();
        }

        private void rgvList_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            if ((rgvList.CurrentCell != null) && (rgvList.CurrentCell.RowIndex > -1) && (rgvList.CurrentCell.RowIndex < rgvList.RowCount))
            {
                string Id = rgvList.Rows[rgvList.CurrentCell.RowIndex].Cells["Id"].Value.ToString();
                Util.OpenPersonCard(Util.MainForm, Id, FillGrid); 
            }
        }
        
        private void chbClass_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            ddlClass.Enabled = chbClass.Checked;
            chbProgram.Enabled = chbClass.Checked;
            if (!chbClass.Checked)
                chbProgram.Checked = false;
            
            FillGrid();
        }
        private void chbProgram_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            ddlProgram.Enabled = chbProgram.Checked;
            FillGrid();
            FillComboProfile();
        }
        private void chbHasOlympiads_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            FillGrid();
        }
        private void chbProfile_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            ddlProfile.Enabled = chbProfile.Checked;
            FillGrid();
        }
        private void chbHasFiles_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            FillGrid();
        }
        private void chbHasTwoApps_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            FillGrid();
        }

        private void tbFIO_TextChanged(object sender, EventArgs e)
        {
            rgvList.FindVal("FIO", tbFIO.Text);
        }

        private void rmiWord_Click(object sender, EventArgs e)
        {
            try
            {
                WordDoc wd = new WordDoc(Util.DataFilesFolder + "PersonList.dot", true);

                wd.Fields["DATE"].Text = DateTime.Now.ToShortDateString();
                string sFilters = "";
                if (chbClass.Checked)
                    sFilters += "Класс поступления: " + ddlClass.Text;
                if (chbProgram.Checked)
                    sFilters += "\nНаправление: " + ddlProgram.Text;

                if (sFilters != "")
                    wd.Fields["FILTERS"].Text = sFilters;

                TableDoc td = wd.Tables[0];
                int i = 0;
                foreach (GridViewRowInfo rwinfo in rgvList.Rows)
                {
                    td.AddRow(1);
                    td[0, i + 1] = (i + 1).ToString();
                    td[1, i + 1] = rgvList.Rows[i].Cells["FIO"].Value.ToString();
                    td[2, i + 1] = rgvList.Rows[i].Cells["BirthDate"].Value.ToString();
                    td[3, i + 1] = rgvList.Rows[i].Cells["BirthPlace"].Value.ToString();
                    i++;
                }
            }
            catch (Exception ex)
            {
                RadMessageBox.Show("Ошибка при выводе в Word:\r\n" + ex.Message, "Ошибка", MessageBoxButtons.OK, RadMessageIcon.Error);
            }
        }
        private void rmiExcell_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Файлы Excel (.xls)|*.xls";
            if (sfd.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    List<string> columnList = new List<string>();
                    foreach (GridViewColumn column in rgvList.Columns)
                        if (column.IsVisible)
                            columnList.Add(column.Name);

                    Excel.Application exc = new Excel.Application();
                    Excel.Workbook wb = exc.Workbooks.Add(System.Reflection.Missing.Value);
                    Excel.Worksheet ws = (Excel.Worksheet)exc.ActiveSheet;

                    ws.Name = "Список лиц (ЛК АГ)";

                    ws.Cells[1, 1] = "№ п/п";
                    for (int j = 0; j < columnList.Count; j++)
                        ws.Cells[1, j + 2] = rgvList.Columns[columnList[j]].HeaderText;

                    int i = 0;
                    // печать из грида
                    foreach (GridViewRowInfo dgvr in rgvList.Rows)
                    {
                        ws.Cells[i + 2, 1] = (i + 1).ToString();
                        for (int j = 0; j < columnList.Count; j++)
                            ws.Cells[i + 2, j + 2] = "'" + rgvList.Rows[i].Cells[columnList[j]].Value.ToString();

                        i++;
                    }

                    wb.SaveAs(sfd.FileName, Excel.XlFileFormat.xlExcel7,
                        System.Reflection.Missing.Value,
                        System.Reflection.Missing.Value,
                        System.Reflection.Missing.Value,
                        System.Reflection.Missing.Value,
                        Excel.XlSaveAsAccessMode.xlExclusive,
                        System.Reflection.Missing.Value,
                        System.Reflection.Missing.Value,
                        System.Reflection.Missing.Value,
                        System.Reflection.Missing.Value,
                        System.Reflection.Missing.Value);
                    exc.Visible = true;
                    //по идее, из приложения Excel выходить не надо, пользователь в силах это сделать и самостоятельно, когда насмотрится на свой отсчёт
                    //exc.Quit();
                    //exc = null;
                }
                catch (System.Runtime.InteropServices.COMException exc)
                {
                    RadMessageBox.Show(exc.Message, "Ошибка", MessageBoxButtons.OK, RadMessageIcon.Error);
                }
            }
        }
        private void rmiClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void rmiExport_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Файлы Excel (.xls)|*.xls";
            if (sfd.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    string query = "SELECT DISTINCT Surname, Person.Name, SecondName, DAY(BirthDate) AS [Day], MONTH(BirthDate) AS [Month], YEAR(BirthDate) AS [Year], " +
                        " EntryClassName AS Class, ISNULL(PersonContacts.City, '') AS City, ISNULL(PersonEducationDocument.SchoolName, '') AS SchoolName, Region.Name AS RegionName" +
                        " FROM Person INNER JOIN PersonEducationDocument ON PersonEducationDocument.PersonId = Person.Id INNER JOIN PersonContacts ON PersonContacts.PersonId=Person.Id " +
                        " INNER JOIN AG_qAbiturient ON AG_qAbiturient.PersonId = Person.Id LEFT OUTER JOIN Region ON Region.Id = PersonContacts.RegionId" +
                        " WHERE AG_qAbiturient.Enabled = 'True' ORDER BY [Year], [Month], [Day]";


                    Excel.Application exc = new Excel.Application();
                    Excel.Workbook wb = exc.Workbooks.Add(System.Reflection.Missing.Value);
                    Excel.Worksheet ws = (Excel.Worksheet)exc.ActiveSheet;

                    ws.Name = "Список для шифрования";

                    ws.Cells[1, 1] = "Фамилия";
                    ws.Cells[1, 2] = "Имя";
                    ws.Cells[1, 3] = "Отчество";
                    ws.Cells[1, 4] = "День рождения";
                    ws.Cells[1, 5] = "Месяц рождения";
                    ws.Cells[1, 6] = "Год рождения";
                    ws.Cells[1, 7] = "Класс обучения";
                    ws.Cells[1, 8] = "Наименование учебного заведения";
                    ws.Cells[1, 9] = "Субъект РФ";
                    ws.Cells[1, 10] = "Населенный пункт";
                    ws.Cells[1, 11] = "Сельская местность";
                    ws.Cells[1, 12] = "Дата проведения";
                    ws.Cells[1, 13] = "Примечание";

                    DataTable tbl = Util.BDC.GetDataTable(query, null);

                    int i = 0;
                    // печать из грида
                    foreach (DataRow rw in tbl.Rows)
                    {
                        ws.Cells[i + 2, 1] = rw.Field<string>("Surname");
                        ws.Cells[i + 2, 2] = rw.Field<string>("Name");
                        ws.Cells[i + 2, 3] = rw.Field<string>("SecondName");
                        ws.Cells[i + 2, 4] = rw.Field<int>("Day").ToString();
                        ws.Cells[i + 2, 5] = rw.Field<int>("Month").ToString();
                        ws.Cells[i + 2, 6] = rw.Field<int>("Year").ToString();
                        ws.Cells[i + 2, 7] = rw.Field<string>("Class");
                        ws.Cells[i + 2, 8] = rw.Field<string>("SchoolName");
                        ws.Cells[i + 2, 9] = rw.Field<string>("RegionName");
                        ws.Cells[i + 2, 10] = rw.Field<string>("City");

                        i++;
                    }

                    wb.SaveAs(sfd.FileName, Excel.XlFileFormat.xlExcel7,
                        System.Reflection.Missing.Value,
                        System.Reflection.Missing.Value,
                        System.Reflection.Missing.Value,
                        System.Reflection.Missing.Value,
                        Excel.XlSaveAsAccessMode.xlExclusive,
                        System.Reflection.Missing.Value,
                        System.Reflection.Missing.Value,
                        System.Reflection.Missing.Value,
                        System.Reflection.Missing.Value,
                        System.Reflection.Missing.Value);
                    exc.Visible = true;
                    //по идее, из приложения Excel выходить не надо, пользователь в силах это сделать и самостоятельно, когда насмотрится на свой отсчёт
                    //exc.Quit();
                    //exc = null;
                }
                catch (System.Runtime.InteropServices.COMException exc)
                {
                    RadMessageBox.Show(exc.Message, "Ошибка", MessageBoxButtons.OK, RadMessageIcon.Error);
                }
            }
        }
        /*
        private void rmiExport_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Файлы Excel (.xls)|*.xls";
            if (sfd.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    string query = "SELECT DISTINCT Surname, Person.Name, SecondName, DAY(BirthDate) AS [Day], MONTH(BirthDate) AS [Month], YEAR(BirthDate) AS [Year], " +
                        " ISNULL(PassportSeries, '') + ' №' + ISNULL(PassportNumber, '') AS Passport, SP_EntryClass.Name AS Class, ISNULL(Person.City, '') AS City, ISNULL(Person.SchoolName, '') AS SchoolName" +
                        " FROM Person " +
                        " INNER JOIN AG_qAbiturient ON AG_qAbiturient.PersonId = Person.Id " +
                        " WHERE AG_qAbiturient.Enabled = 'True' ORDER BY [Year], [Month], [Day]";


                    Excel.Application exc = new Excel.Application();
                    Excel.Workbook wb = exc.Workbooks.Add(System.Reflection.Missing.Value);
                    Excel.Worksheet ws = (Excel.Worksheet)exc.ActiveSheet;

                    ws.Name = "Список для шифрования";

                    ws.Cells[1, 1] = "№ п/п";
                    ws.Cells[1, 2] = "Фамилия";
                    ws.Cells[1, 3] = "Имя";
                    ws.Cells[1, 4] = "Отчество";
                    ws.Cells[1, 5] = "День рождения";
                    ws.Cells[1, 6] = "Месяц рождения";
                    ws.Cells[1, 7] = "Год рождения";
                    ws.Cells[1, 8] = "Серия и номер паспорта";
                    ws.Cells[1, 9] = "Класс обучения";
                    ws.Cells[1, 10] = "Название экзамена";
                    ws.Cells[1, 11] = "Дата проведения";
                    ws.Cells[1, 12] = "Субъект РФ";
                    ws.Cells[1, 13] = "Населенный пункт";
                    ws.Cells[1, 14] = "Наименование учебного заведения";

                    DataTable tbl = Util.BDC.GetDataTable(query, null);

                    int i = 0;
                    // печать из грида
                    foreach (DataRow rw in tbl.Rows)
                    {
                        ws.Cells[i + 2, 1] = (i + 1).ToString();
                        ws.Cells[i + 2, 2] = rw.Field<string>("Surname");
                        ws.Cells[i + 2, 3] = rw.Field<string>("Name");
                        ws.Cells[i + 2, 4] = rw.Field<string>("SecondName");
                        ws.Cells[i + 2, 5] = rw.Field<int>("Day").ToString();
                        ws.Cells[i + 2, 6] = rw.Field<int>("Month").ToString();
                        ws.Cells[i + 2, 7] = rw.Field<int>("Year").ToString();
                        ws.Cells[i + 2, 8] = rw.Field<string>("Passport");
                        ws.Cells[i + 2, 9] = rw.Field<string>("Class");

                        ws.Cells[i + 2, 13] = rw.Field<string>("City");
                        ws.Cells[i + 2, 14] = rw.Field<string>("SchoolName");

                        i++;
                    }

                    wb.SaveAs(sfd.FileName, Excel.XlFileFormat.xlExcel7,
                        System.Reflection.Missing.Value,
                        System.Reflection.Missing.Value,
                        System.Reflection.Missing.Value,
                        System.Reflection.Missing.Value,
                        Excel.XlSaveAsAccessMode.xlExclusive,
                        System.Reflection.Missing.Value,
                        System.Reflection.Missing.Value,
                        System.Reflection.Missing.Value,
                        System.Reflection.Missing.Value,
                        System.Reflection.Missing.Value);
                    exc.Visible = true;
                    //по идее, из приложения Excel выходить не надо, пользователь в силах это сделать и самостоятельно, когда насмотрится на свой отсчёт
                    //exc.Quit();
                    //exc = null;
                }
                catch (System.Runtime.InteropServices.COMException exc)
                {
                    RadMessageBox.Show(exc.Message, "Ошибка", MessageBoxButtons.OK, RadMessageIcon.Error);
                }
            }
        }
         */

        private void radButtonElement3_Click(object sender, EventArgs e)
        {
            FillGrid();
        }

        private void chbMale_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            FillGrid();
        }
        private void chbFemale_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            FillGrid();
        }
        private void chbNeedHostel_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            FillGrid();
        }
        private void ddlRegion_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            FillGrid();
        }

        private void btnNewPerson_Click(object sender, EventArgs e)
        {
            Util.OpenPersonCard(Util.MainForm, null, FillGrid); 
        }
    }
}

