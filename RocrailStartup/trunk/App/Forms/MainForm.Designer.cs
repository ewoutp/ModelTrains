namespace MGV.RocRailStartup.Forms
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.lvTracks = new System.Windows.Forms.ListView();
            this.lbSelecteerBaan = new System.Windows.Forms.Label();
            this.cmdStartRocrail = new System.Windows.Forms.Button();
            this.cmdStartRocview = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cbTheme = new System.Windows.Forms.ComboBox();
            this.lbVersion = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.miTools = new System.Windows.Forms.ToolStripMenuItem();
            this.miLanguage = new System.Windows.Forms.ToolStripMenuItem();
            this.miLangEN = new System.Windows.Forms.ToolStripMenuItem();
            this.nToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.miFolders = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.miUninstallRocrail = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvTracks
            // 
            resources.ApplyResources(this.lvTracks, "lvTracks");
            this.lvTracks.HideSelection = false;
            this.lvTracks.MultiSelect = false;
            this.lvTracks.Name = "lvTracks";
            this.lvTracks.UseCompatibleStateImageBehavior = false;
            this.lvTracks.View = System.Windows.Forms.View.List;
            this.lvTracks.SelectedIndexChanged += new System.EventHandler(this.lvTracks_SelectedIndexChanged);
            // 
            // lbSelecteerBaan
            // 
            resources.ApplyResources(this.lbSelecteerBaan, "lbSelecteerBaan");
            this.lbSelecteerBaan.Name = "lbSelecteerBaan";
            // 
            // cmdStartRocrail
            // 
            resources.ApplyResources(this.cmdStartRocrail, "cmdStartRocrail");
            this.cmdStartRocrail.Name = "cmdStartRocrail";
            this.cmdStartRocrail.UseVisualStyleBackColor = true;
            this.cmdStartRocrail.Click += new System.EventHandler(this.cmdStartRocrail_Click);
            // 
            // cmdStartRocview
            // 
            resources.ApplyResources(this.cmdStartRocview, "cmdStartRocview");
            this.cmdStartRocview.Name = "cmdStartRocview";
            this.cmdStartRocview.UseVisualStyleBackColor = true;
            this.cmdStartRocview.Click += new System.EventHandler(this.cmdStartRocview_Click);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // cbTheme
            // 
            resources.ApplyResources(this.cbTheme, "cbTheme");
            this.cbTheme.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTheme.FormattingEnabled = true;
            this.cbTheme.Name = "cbTheme";
            this.cbTheme.SelectedIndexChanged += new System.EventHandler(this.cbTheme_SelectedIndexChanged);
            // 
            // lbVersion
            // 
            resources.ApplyResources(this.lbVersion, "lbVersion");
            this.lbVersion.Name = "lbVersion";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miTools});
            resources.ApplyResources(this.menuStrip1, "menuStrip1");
            this.menuStrip1.Name = "menuStrip1";
            // 
            // miTools
            // 
            this.miTools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miLanguage,
            this.miFolders,
            this.toolStripSeparator1,
            this.miUninstallRocrail});
            this.miTools.Name = "miTools";
            resources.ApplyResources(this.miTools, "miTools");
            // 
            // miLanguage
            // 
            this.miLanguage.DoubleClickEnabled = true;
            this.miLanguage.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miLangEN,
            this.nToolStripMenuItem});
            this.miLanguage.Name = "miLanguage";
            resources.ApplyResources(this.miLanguage, "miLanguage");
            // 
            // miLangEN
            // 
            this.miLangEN.Name = "miLangEN";
            resources.ApplyResources(this.miLangEN, "miLangEN");
            this.miLangEN.Click += new System.EventHandler(this.miLangEN_Click);
            // 
            // nToolStripMenuItem
            // 
            this.nToolStripMenuItem.Name = "nToolStripMenuItem";
            resources.ApplyResources(this.nToolStripMenuItem, "nToolStripMenuItem");
            this.nToolStripMenuItem.Click += new System.EventHandler(this.nToolStripMenuItem_Click);
            // 
            // miFolders
            // 
            this.miFolders.Name = "miFolders";
            resources.ApplyResources(this.miFolders, "miFolders");
            this.miFolders.Click += new System.EventHandler(this.miFolders_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // miUninstallRocrail
            // 
            this.miUninstallRocrail.Name = "miUninstallRocrail";
            resources.ApplyResources(this.miUninstallRocrail, "miUninstallRocrail");
            this.miUninstallRocrail.Click += new System.EventHandler(this.cmdUninstall_Click);
            // 
            // MainForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lbVersion);
            this.Controls.Add(this.cbTheme);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmdStartRocview);
            this.Controls.Add(this.cmdStartRocrail);
            this.Controls.Add(this.lbSelecteerBaan);
            this.Controls.Add(this.lvTracks);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lvTracks;
        private System.Windows.Forms.Label lbSelecteerBaan;
        private System.Windows.Forms.Button cmdStartRocrail;
        private System.Windows.Forms.Button cmdStartRocview;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbTheme;
        private System.Windows.Forms.Label lbVersion;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem miTools;
        private System.Windows.Forms.ToolStripMenuItem miFolders;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem miUninstallRocrail;
        private System.Windows.Forms.ToolStripMenuItem miLanguage;
        private System.Windows.Forms.ToolStripMenuItem miLangEN;
        private System.Windows.Forms.ToolStripMenuItem nToolStripMenuItem;
    }
}

