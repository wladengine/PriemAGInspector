namespace PriemAGInspector
{
    partial class LoadFilesCard
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ddlFileTypeList = new Telerik.WinControls.UI.RadDropDownList();
            this.btnLoad = new Telerik.WinControls.UI.RadButton();
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            this.radLabel2 = new Telerik.WinControls.UI.RadLabel();
            this.radLabel3 = new Telerik.WinControls.UI.RadLabel();
            this.tbFileComment = new Telerik.WinControls.UI.RadTextBox();
            this.openDialog = new Telerik.WinControls.UI.RadBrowseEditor();
            ((System.ComponentModel.ISupportInitialize)(this.ddlFileTypeList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnLoad)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbFileComment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.openDialog)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // ddlFileTypeList
            // 
            this.ddlFileTypeList.DropDownAnimationEnabled = true;
            this.ddlFileTypeList.Location = new System.Drawing.Point(113, 36);
            this.ddlFileTypeList.Name = "ddlFileTypeList";
            this.ddlFileTypeList.ShowImageInEditorArea = true;
            this.ddlFileTypeList.Size = new System.Drawing.Size(340, 20);
            this.ddlFileTypeList.TabIndex = 1;
            this.ddlFileTypeList.Text = "radDropDownList1";
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(173, 169);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(130, 24);
            this.btnLoad.TabIndex = 3;
            this.btnLoad.Text = "Загрузить";
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // radLabel1
            // 
            this.radLabel1.Location = new System.Drawing.Point(12, 12);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(86, 18);
            this.radLabel1.TabIndex = 2;
            this.radLabel1.Text = "Выберите файл";
            // 
            // radLabel2
            // 
            this.radLabel2.Location = new System.Drawing.Point(38, 36);
            this.radLabel2.Name = "radLabel2";
            this.radLabel2.Size = new System.Drawing.Size(60, 18);
            this.radLabel2.TabIndex = 3;
            this.radLabel2.Text = "Тип файла";
            // 
            // radLabel3
            // 
            this.radLabel3.Location = new System.Drawing.Point(20, 62);
            this.radLabel3.Name = "radLabel3";
            this.radLabel3.Size = new System.Drawing.Size(78, 18);
            this.radLabel3.TabIndex = 4;
            this.radLabel3.Text = "Комментарий";
            // 
            // tbFileComment
            // 
            this.tbFileComment.Location = new System.Drawing.Point(113, 62);
            this.tbFileComment.Multiline = true;
            this.tbFileComment.Name = "tbFileComment";
            // 
            // 
            // 
            this.tbFileComment.RootElement.StretchVertically = true;
            this.tbFileComment.Size = new System.Drawing.Size(340, 101);
            this.tbFileComment.TabIndex = 2;
            this.tbFileComment.TabStop = false;
            // 
            // openDialog
            // 
            this.openDialog.Location = new System.Drawing.Point(113, 9);
            this.openDialog.Name = "openDialog";
            this.openDialog.Size = new System.Drawing.Size(340, 20);
            this.openDialog.TabIndex = 0;
            // 
            // LoadFilesCard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(474, 204);
            this.Controls.Add(this.openDialog);
            this.Controls.Add(this.tbFileComment);
            this.Controls.Add(this.radLabel3);
            this.Controls.Add(this.radLabel2);
            this.Controls.Add(this.radLabel1);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.ddlFileTypeList);
            this.Name = "LoadFilesCard";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "LoadFilesCard";
            this.ThemeName = "ControlDefault";
            ((System.ComponentModel.ISupportInitialize)(this.ddlFileTypeList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnLoad)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbFileComment)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.openDialog)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadDropDownList ddlFileTypeList;
        private Telerik.WinControls.UI.RadButton btnLoad;
        private Telerik.WinControls.UI.RadLabel radLabel1;
        private Telerik.WinControls.UI.RadLabel radLabel2;
        private Telerik.WinControls.UI.RadLabel radLabel3;
        private Telerik.WinControls.UI.RadTextBox tbFileComment;
        private Telerik.WinControls.UI.RadBrowseEditor openDialog;
    }
}
