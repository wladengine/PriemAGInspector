namespace PriemAGInspector
{
    partial class HistoryCard
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
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn1 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn2 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn3 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn4 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn5 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.Data.SortDescriptor sortDescriptor1 = new Telerik.WinControls.Data.SortDescriptor();
            this.rgvHistory = new Telerik.WinControls.UI.RadGridView();
            ((System.ComponentModel.ISupportInitialize)(this.rgvHistory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // rgvHistory
            // 
            this.rgvHistory.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rgvHistory.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.rgvHistory.Cursor = System.Windows.Forms.Cursors.Default;
            this.rgvHistory.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.rgvHistory.ForeColor = System.Drawing.Color.Black;
            this.rgvHistory.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.rgvHistory.Location = new System.Drawing.Point(12, 12);
            // 
            // rgvHistory
            // 
            this.rgvHistory.MasterTemplate.AllowAddNewRow = false;
            this.rgvHistory.MasterTemplate.AllowColumnReorder = false;
            gridViewTextBoxColumn1.AllowSort = false;
            gridViewTextBoxColumn1.EnableExpressionEditor = false;
            gridViewTextBoxColumn1.FieldName = "Action";
            gridViewTextBoxColumn1.HeaderText = "Действие";
            gridViewTextBoxColumn1.Name = "Action";
            gridViewTextBoxColumn1.ReadOnly = true;
            gridViewTextBoxColumn1.Width = 57;
            gridViewTextBoxColumn2.EnableExpressionEditor = false;
            gridViewTextBoxColumn2.FieldName = "OldValue";
            gridViewTextBoxColumn2.HeaderText = "Старое значение";
            gridViewTextBoxColumn2.Name = "OldValue";
            gridViewTextBoxColumn2.ReadOnly = true;
            gridViewTextBoxColumn2.Width = 97;
            gridViewTextBoxColumn3.EnableExpressionEditor = false;
            gridViewTextBoxColumn3.FieldName = "NewValue";
            gridViewTextBoxColumn3.HeaderText = "Новое значение";
            gridViewTextBoxColumn3.Name = "NewValue";
            gridViewTextBoxColumn3.ReadOnly = true;
            gridViewTextBoxColumn3.Width = 93;
            gridViewTextBoxColumn4.EnableExpressionEditor = false;
            gridViewTextBoxColumn4.FieldName = "Time";
            gridViewTextBoxColumn4.HeaderText = "Время";
            gridViewTextBoxColumn4.Name = "Time";
            gridViewTextBoxColumn4.ReadOnly = true;
            gridViewTextBoxColumn4.Width = 41;
            gridViewTextBoxColumn5.EnableExpressionEditor = false;
            gridViewTextBoxColumn5.FieldName = "Owner";
            gridViewTextBoxColumn5.HeaderText = "Автор";
            gridViewTextBoxColumn5.Name = "Owner";
            gridViewTextBoxColumn5.ReadOnly = true;
            gridViewTextBoxColumn5.Width = 39;
            this.rgvHistory.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewTextBoxColumn1,
            gridViewTextBoxColumn2,
            gridViewTextBoxColumn3,
            gridViewTextBoxColumn4,
            gridViewTextBoxColumn5});
            sortDescriptor1.PropertyName = "column1";
            this.rgvHistory.MasterTemplate.SortDescriptors.AddRange(new Telerik.WinControls.Data.SortDescriptor[] {
            sortDescriptor1});
            this.rgvHistory.Name = "rgvHistory";
            this.rgvHistory.ReadOnly = true;
            this.rgvHistory.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.rgvHistory.ShowGroupPanel = false;
            this.rgvHistory.ShowNoDataText = false;
            this.rgvHistory.Size = new System.Drawing.Size(462, 496);
            this.rgvHistory.TabIndex = 0;
            // 
            // HistoryCard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(486, 520);
            this.Controls.Add(this.rgvHistory);
            this.Name = "HistoryCard";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "История изменений и событий";
            this.ThemeName = "ControlDefault";
            ((System.ComponentModel.ISupportInitialize)(this.rgvHistory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadGridView rgvHistory;
    }
}
