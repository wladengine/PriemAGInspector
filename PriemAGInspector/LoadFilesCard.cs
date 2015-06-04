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
    public partial class LoadFilesCard : Telerik.WinControls.UI.RadForm
    {
        private Guid _PersonId;
        private Guid _fileId; 
        public event OnUpdateHandler OnUpdate;

        public LoadFilesCard(Guid id)
        {
            this.Name = "Загрузка файла";
            _PersonId = id;
            InitializeComponent();
            if (id == Guid.Empty)
                this.Close();
            FillComponent();
        }

        private void FillComponent()
        {
            string query = "SELECT [Id] ,[Name]  FROM [PersonFileType] ";
            DataTable tbl = Util.BDC.GetDataTable(query, null);
            Dictionary<object, string> source =
                (from DataRow rw in tbl.Rows
                 select new
                 {
                     Id = rw.Field<int>("Id"),
                     Name = rw.Field<string>("Name")
                 }).ToDictionary(x => (object)x.Id, y => y.Name);
            ddlFileTypeList.FillCombo(source);

        }

        private void btnLoad_Click(object sender, EventArgs e)
        { 
            try
            {
                _fileId = Guid.NewGuid();
                string Comment = tbFileComment.Text.ToString();
                int FileTypeId = (int)ddlFileTypeList.SelectedValue;
                string File = openDialog.Value.ToString();
                if ((File != String.Empty) && (File != null))
                {
                    //размер файла
                    int fileSize = Convert.ToInt32(new FileInfo(File).Length);
                    tbFileComment.Text = fileSize.ToString();

                    // имя файла
                    string filename="";
                    int lastSlashPos = 0;
                    lastSlashPos = File.LastIndexOfAny(new char[] { '\\', '/' });
                    if (lastSlashPos > 0)
                        filename = File.Substring(lastSlashPos + 1);

                    // читаем байтики
                    byte[] fileData = new byte[fileSize];
                    fileData = System.IO.File.ReadAllBytes(File);

                    // читаем расширение
                    string fileext = "";
                    try
                    {
                        fileext = filename.Substring(filename.LastIndexOf('.'));
                    }
                    catch
                    {
                        fileext = "";
                    }

                    string query = "INSERT INTO PersonFile (Id, PersonId, FileName, FileData, FileSize, FileExtention, LoadDate, Comment, MimeType, PersonFileTypeId) " +
                     " VALUES (@Id, @PersonId, @FileName, @FileData, @FileSize, @FileExtention, @LoadDate, @Comment, @MimeType, @PersonFileTypeId)";
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("@Id", _fileId);
                    dic.Add("@PersonId", _PersonId);
                    dic.Add("@FileName", filename);
                    dic.Add("@FileData", fileData);
                    dic.Add("@FileSize", fileSize);
                    dic.Add("@FileExtention", fileext); 
                    dic.Add("@LoadDate", DateTime.Now);
                    dic.Add("@Comment", Comment); 
                    dic.Add("@MimeType", Util.GetMimeFromExtention(fileext));
                    dic.Add("@PersonFileTypeId", FileTypeId);
                    int res = Util.BDC.ExecuteQuery(query, dic);
                    if (res != -1)
                    {
                        if (OnUpdate != null)
                            OnUpdate();
                        this.Close();
                    }
                }
            }
            catch
            {
            }

        }
 
    }
}
