namespace PriemAGInspector
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.radMenu1 = new Telerik.WinControls.UI.RadMenu();
            this.radMenuItem1 = new Telerik.WinControls.UI.RadMenuItem();
            this.rmiPersons = new Telerik.WinControls.UI.RadMenuItem();
            this.rmiApplications = new Telerik.WinControls.UI.RadMenuItem();
            this.radStatusStrip1 = new Telerik.WinControls.UI.RadStatusStrip();
            this.statusLabel = new Telerik.WinControls.UI.RadLabelElement();
            ((System.ComponentModel.ISupportInitialize)(this.radMenu1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radStatusStrip1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radMenu1
            // 
            this.radMenu1.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.radMenuItem1});
            this.radMenu1.Location = new System.Drawing.Point(0, 0);
            this.radMenu1.Name = "radMenu1";
            this.radMenu1.Size = new System.Drawing.Size(807, 20);
            this.radMenu1.TabIndex = 0;
            this.radMenu1.Text = "radMenu1";
            // 
            // radMenuItem1
            // 
            this.radMenuItem1.AccessibleDescription = "Списки";
            this.radMenuItem1.AccessibleName = "Списки";
            this.radMenuItem1.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.rmiPersons,
            this.rmiApplications});
            this.radMenuItem1.Name = "radMenuItem1";
            this.radMenuItem1.Text = "Списки";
            this.radMenuItem1.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // rmiPersons
            // 
            this.rmiPersons.AccessibleDescription = "rmiPersons";
            this.rmiPersons.AccessibleName = "rmiPersons";
            this.rmiPersons.Name = "rmiPersons";
            this.rmiPersons.Text = "Люди";
            this.rmiPersons.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.rmiPersons.Click += new System.EventHandler(this.rmiPersons_Click);
            // 
            // rmiApplications
            // 
            this.rmiApplications.AccessibleDescription = "rmiApplications";
            this.rmiApplications.AccessibleName = "rmiApplications";
            this.rmiApplications.Name = "rmiApplications";
            this.rmiApplications.Text = "Заявления";
            this.rmiApplications.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // radStatusStrip1
            // 
            this.radStatusStrip1.AutoSize = true;
            this.radStatusStrip1.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.statusLabel});
            this.radStatusStrip1.LayoutStyle = Telerik.WinControls.UI.RadStatusBarLayoutStyle.Stack;
            this.radStatusStrip1.Location = new System.Drawing.Point(0, 531);
            this.radStatusStrip1.Name = "radStatusStrip1";
            this.radStatusStrip1.Size = new System.Drawing.Size(807, 24);
            this.radStatusStrip1.TabIndex = 2;
            this.radStatusStrip1.Text = "radStatusStrip1";
            // 
            // statusLabel
            // 
            this.statusLabel.AccessibleDescription = "InfoHere";
            this.statusLabel.AccessibleName = "InfoHere";
            this.statusLabel.Name = "statusLabel";
            this.radStatusStrip1.SetSpring(this.statusLabel, false);
            this.statusLabel.Text = "InfoHere";
            this.statusLabel.TextWrap = true;
            this.statusLabel.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(807, 555);
            this.Controls.Add(this.radStatusStrip1);
            this.Controls.Add(this.radMenu1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.Name = "MainForm";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Приём заявлений в АГ";
            ((System.ComponentModel.ISupportInitialize)(this.radMenu1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radStatusStrip1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadMenu radMenu1;
        private Telerik.WinControls.UI.RadMenuItem radMenuItem1;
        private Telerik.WinControls.UI.RadMenuItem rmiPersons;
        private Telerik.WinControls.UI.RadMenuItem rmiApplications;
        private Telerik.WinControls.UI.RadStatusStrip radStatusStrip1;
        private Telerik.WinControls.UI.RadLabelElement statusLabel;
    }
}

