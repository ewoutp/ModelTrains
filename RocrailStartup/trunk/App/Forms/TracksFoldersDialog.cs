using System;
using System.Linq;
using System.Windows.Forms;

namespace MGV.RocRailStartup.Forms
{
    public partial class TracksFoldersDialog : Form
    {
        /// <summary>
        /// Default ctor
        /// </summary>
        public TracksFoldersDialog()
        {
            InitializeComponent();
            cmdRemove.Enabled = false;
        }

        /// <summary>
        /// Gets / sets the configured folders
        /// </summary>
        public string[] Folders
        {
            get
            {
                return lvFolders.Items.Cast<ListViewItem>().Select(x => (string)x.Tag).ToArray();
            }
            set
            {
                lvFolders.BeginUpdate();
                lvFolders.Items.Clear();
                if (value != null)
                {
                    lvFolders.Items.AddRange(value.Select(x => new ListViewItem(x) { Tag = x }).ToArray());
                }
                lvFolders.EndUpdate();
            }
        }

        /// <summary>
        /// Add a new folder
        /// </summary>
        private void cmdAdd_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog(this) == DialogResult.OK)
            {
                var path = folderBrowserDialog1.SelectedPath;
                lvFolders.Items.Add(new ListViewItem(path) { Tag = path });
            }
        }

        /// <summary>
        /// Remove selected folders
        /// </summary>
        private void cmdRemove_Click(object sender, EventArgs e)
        {
            var selection = lvFolders.SelectedItems.Cast<ListViewItem>().ToArray();
            foreach (var item in selection)
            {
                lvFolders.Items.Remove(item);
            }
        }

        /// <summary>
        /// Selection has changed
        /// </summary>
        private void lvFolders_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmdRemove.Enabled = (lvFolders.SelectedItems.Count > 0);
        }
    }
}
