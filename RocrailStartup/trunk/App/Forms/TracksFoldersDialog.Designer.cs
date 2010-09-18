namespace MGV.RocRailStartup.Forms
{
    partial class TracksFoldersDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TracksFoldersDialog));
            this.label1 = new System.Windows.Forms.Label();
            this.lvFolders = new System.Windows.Forms.ListView();
            this.chFolder = new System.Windows.Forms.ColumnHeader();
            this.cmdRemove = new System.Windows.Forms.Button();
            this.cmdAdd = new System.Windows.Forms.Button();
            this.cmdOk = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AccessibleDescription = null;
            this.label1.AccessibleName = null;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Font = null;
            this.label1.Name = "label1";
            // 
            // lvFolders
            // 
            this.lvFolders.AccessibleDescription = null;
            this.lvFolders.AccessibleName = null;
            resources.ApplyResources(this.lvFolders, "lvFolders");
            this.lvFolders.BackgroundImage = null;
            this.lvFolders.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chFolder});
            this.lvFolders.Font = null;
            this.lvFolders.Name = "lvFolders";
            this.lvFolders.UseCompatibleStateImageBehavior = false;
            this.lvFolders.View = System.Windows.Forms.View.Details;
            this.lvFolders.SelectedIndexChanged += new System.EventHandler(this.lvFolders_SelectedIndexChanged);
            // 
            // chFolder
            // 
            resources.ApplyResources(this.chFolder, "chFolder");
            // 
            // cmdRemove
            // 
            this.cmdRemove.AccessibleDescription = null;
            this.cmdRemove.AccessibleName = null;
            resources.ApplyResources(this.cmdRemove, "cmdRemove");
            this.cmdRemove.BackgroundImage = null;
            this.cmdRemove.Font = null;
            this.cmdRemove.Name = "cmdRemove";
            this.cmdRemove.UseVisualStyleBackColor = true;
            this.cmdRemove.Click += new System.EventHandler(this.cmdRemove_Click);
            // 
            // cmdAdd
            // 
            this.cmdAdd.AccessibleDescription = null;
            this.cmdAdd.AccessibleName = null;
            resources.ApplyResources(this.cmdAdd, "cmdAdd");
            this.cmdAdd.BackgroundImage = null;
            this.cmdAdd.Font = null;
            this.cmdAdd.Name = "cmdAdd";
            this.cmdAdd.UseVisualStyleBackColor = true;
            this.cmdAdd.Click += new System.EventHandler(this.cmdAdd_Click);
            // 
            // cmdOk
            // 
            this.cmdOk.AccessibleDescription = null;
            this.cmdOk.AccessibleName = null;
            resources.ApplyResources(this.cmdOk, "cmdOk");
            this.cmdOk.BackgroundImage = null;
            this.cmdOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOk.Font = null;
            this.cmdOk.Name = "cmdOk";
            this.cmdOk.UseVisualStyleBackColor = true;
            // 
            // cmdCancel
            // 
            this.cmdCancel.AccessibleDescription = null;
            this.cmdCancel.AccessibleName = null;
            resources.ApplyResources(this.cmdCancel, "cmdCancel");
            this.cmdCancel.BackgroundImage = null;
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Font = null;
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            // 
            // folderBrowserDialog1
            // 
            resources.ApplyResources(this.folderBrowserDialog1, "folderBrowserDialog1");
            // 
            // TracksFoldersDialog
            // 
            this.AcceptButton = this.cmdOk;
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = null;
            this.CancelButton = this.cmdCancel;
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOk);
            this.Controls.Add(this.cmdAdd);
            this.Controls.Add(this.cmdRemove);
            this.Controls.Add(this.lvFolders);
            this.Controls.Add(this.label1);
            this.Font = null;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = null;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TracksFoldersDialog";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListView lvFolders;
        private System.Windows.Forms.Button cmdRemove;
        private System.Windows.Forms.Button cmdAdd;
        private System.Windows.Forms.Button cmdOk;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.ColumnHeader chFolder;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
    }
}