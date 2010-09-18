using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using MGV.RocRailStartup.Preferences;
using MGV.RocRailStartup.Rocrail;

namespace MGV.RocRailStartup.Forms
{
    public partial class MainForm : Form
    {
        private Rocrail.Rocrail rocrail;

        /// <summary>
        /// Default ctor
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            lbVersion.Text = this.GetType().Assembly.GetName().Version.ToString();
        }

        /// <summary>
        /// Load time initialization
        /// </summary>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Load user preferences
            var prefs = UserPreferences.Preferences;

            // Detect rocrail
            ReloadRocrail(true);

            // Are there configured tracks?
            if (string.IsNullOrEmpty(prefs.TracksFolders))
            {
                // Let user choose tracks folders
                using (var dialog = new TracksFoldersDialog())
                {
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        prefs.TracksFolders = string.Join(Path.PathSeparator.ToString(), dialog.Folders);
                        UserPreferences.SaveNow();
                    }
                }
            }

            // Find possible tracks
            ReloadTracks();

            // Update controls
            UpdateEnabledState();
        }

        /// <summary>
        /// Load all tracks
        /// </summary>
        private void ReloadTracks()
        {
            // Find possible tracks
            var prefs = UserPreferences.Preferences;
            lvTracks.BeginUpdate();
            lvTracks.Items.Clear();
            foreach (var folder in LoadTrackFolders())
            {
                var item = lvTracks.Items.Add(folder.Name);
                item.Tag = folder.FullName;
                if (prefs.Track == folder.Name)
                {
                    item.Selected = true;
                }
            }
            if ((lvTracks.Items.Count > 0) && (lvTracks.SelectedItems.Count == 0))
            {
                // Select first entry
                lvTracks.Items[0].Selected = true;
            }
            lvTracks.EndUpdate();
        }

        /// <summary>
        /// Load rocrail settings
        /// </summary>
        private void ReloadRocrail(bool errorIfNotFound)
        {
            try
            {
                // Load user preferences
                var prefs = UserPreferences.Preferences;

                this.rocrail = new Rocrail.Rocrail();
                this.Text += string.Format(" [{0}]", rocrail.DisplayName);

                // Load themes
                foreach (var theme in rocrail.Themes)
                {
                    cbTheme.Items.Add(theme);
                }
                var currentTheme = prefs.Theme;
                if (!string.IsNullOrEmpty(currentTheme))
                {
                    cbTheme.SelectedIndex = cbTheme.FindStringExact(currentTheme);
                }
                else if (cbTheme.Items.Count > 0)
                {
                    cbTheme.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                if (errorIfNotFound)
                {
                    MessageBox.Show(string.Format("Cannot find Rocrail: {0}", ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Gets the selected track folder.
        /// </summary>
        private string SelectedFolder
        {
            get
            {
                var item = (lvTracks.SelectedItems.Count > 0) ? lvTracks.SelectedItems[0] : null;
                return (item != null) ? (string)item.Tag : null;
            }
        }

        /// <summary>
        /// Gets the selected theme name
        /// </summary>
        private string SelectedTheme
        {
            get { return cbTheme.SelectedItem as string; }
        }

        /// <summary>
        /// Update the state of the controls
        /// </summary>
        private void UpdateEnabledState()
        {
            var selection = this.SelectedFolder;
            cmdStartRocrail.Enabled = (rocrail != null) && (selection != null);
            cmdStartRocview.Enabled = (rocrail != null) && (selection != null) && (this.SelectedTheme != null);
            miUninstallRocrail.Enabled = (rocrail != null);
        }

        /// <summary>
        /// Start rocrail using the select track
        /// </summary>
        private void cmdStartRocrail_Click(object sender, EventArgs e)
        {
            var folder = this.SelectedFolder;
            if ((rocrail != null) && (folder != null))
            {
                try
                {
                    var ini = new RocrailIni(folder);
                    var images = GetImagesFolder(Path.GetDirectoryName(folder));
                    rocrail.StartRocrail(folder, images, ini.IsModulePlan);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format("Cannot start Rocrail: {0}", ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Start Rocview with the selected theme
        /// </summary>
        private void cmdStartRocview_Click(object sender, EventArgs e)
        {
            var theme = this.SelectedTheme;
            var folder = this.SelectedFolder;
            if ((rocrail != null) && (!string.IsNullOrEmpty(theme)) && (folder != null))
            {
                try
                {
                    var ini = new RocrailIni(folder);
                    rocrail.StartRocview(folder, theme, ini.IsModulePlan);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format("Cannot start Rocview: {0}", ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Find all possible track folders.
        /// </summary>
        private IEnumerable<DirectoryInfo> LoadTrackFolders()
        {
            foreach (var fld in GetTracksFolders())
            {
                var tracksFolder = new DirectoryInfo(fld);
                if (tracksFolder.Exists)
                {
                    foreach (var folder in tracksFolder.GetDirectories())
                    {
                        // Look for rocrail.ini file
                        var iniFile = new FileInfo(Path.Combine(folder.FullName, "rocrail.ini"));
                        if (iniFile.Exists)
                        {
                            // We found a track folder
                            yield return folder;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Get the root folder that contains all track folders.
        /// </summary>
        private static string[] GetTracksFolders()
        {
            var folders = UserPreferences.Preferences.TracksFolders;
            if (!string.IsNullOrEmpty(folders)) { return folders.Split(Path.PathSeparator); }
            // No setting found
            return new string[0];
        }

        /// <summary>
        /// Selection changed.
        /// </summary>
        private void lvTracks_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateEnabledState();
            var track = this.SelectedFolder;
            if (track != null) { track = Path.GetFileName(track); }
            UserPreferences.Preferences.Track = track;
            UserPreferences.SaveNow();
        }

        /// <summary>
        /// Selected theme changed
        /// </summary>
        private void cbTheme_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateEnabledState();
            UserPreferences.Preferences.Theme = this.SelectedTheme;
            UserPreferences.SaveNow();
        }

        /// <summary>
        /// Get an images folder for the given folder.
        /// </summary>
        private string GetImagesFolder(string folder)
        {
            while (!string.IsNullOrEmpty(folder))
            {
                string result = Path.Combine(folder, "Images");
                if (Directory.Exists(result))
                {
                    return result;
                }
                else
                {
                    folder = Path.GetDirectoryName(folder);
                }
            }
            return rocrail.DefaultImagesFolder;
        }

        private void cmdUninstall_Click(object sender, EventArgs e)
        {
            try
            {
                rocrail.Uninstall();
                ReloadRocrail(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Uninstall failed: " + ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Configure tracks folders
        /// </summary>
        private void miFolders_Click(object sender, EventArgs e)
        {
            var prefs = UserPreferences.Preferences;
            using (var dialog = new TracksFoldersDialog())
            {
                dialog.Folders = GetTracksFolders();
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    prefs.TracksFolders = string.Join(Path.PathSeparator.ToString(), dialog.Folders);
                    UserPreferences.SaveNow();
                    ReloadTracks();
                }
            }

        }

        /// <summary>
        /// Set Language to English
        /// </summary>
        private void miLangEN_Click(object sender, EventArgs e)
        {
            SetLanguage(Languages.English);
        }

        private void nToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetLanguage(Languages.Dutch);
        }

        private void SetLanguage(string language)
        {
            UserPreferences.Preferences.Language = language;
            UserPreferences.SaveNow();
        }
    }
}
