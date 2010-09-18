using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Microsoft.Win32;

namespace MGV.RocRailStartup.Rocrail
{
    internal class Rocrail
    {
        private static readonly string[] REGKEYS = new string[] { 
            @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{D0242B5C-9C14-41A4-8D38-545B0F7D9825}_is1",
            @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Rocrail_is1",
        };
        private readonly string folder;
        private readonly string name;
        private readonly string uninstallString;

        /// <summary>
        /// Default ctor
        /// </summary>
        public Rocrail()
        {
            this.folder = FindRocrailLocation(out this.name, out this.uninstallString);
        }

        /// <summary>
        /// Gets the display name found in the install info
        /// </summary>
        internal string DisplayName { get { return name; } }

        /// <summary>
        /// Standard images folder
        /// </summary>
        internal string DefaultImagesFolder
        {
            get
            {
                var dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"Rocrail\Images");
                if (Directory.Exists(dir)) { return dir; }
                return Path.Combine(folder, "Images");
            }
        }

        /// <summary>
        /// Standard themese folder
        /// </summary>
        internal string DefaultThemesFolder
        {
            get
            {
                var dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"Rocrail\svg\themes");
                if (Directory.Exists(dir)) { return dir; }
                return Path.Combine(folder, @"svg\themes");
            }
        }

        /// <summary>
        /// Try to uninstall rocrail.
        /// </summary>
        internal void Uninstall()
        {
            if (uninstallString != null)
            {
                ProcessStartInfo info = new ProcessStartInfo();
                info.FileName = this.uninstallString;
                Process.Start(info);
            }
        }

        /// <summary>
        /// Start rocrail in the given folder.
        /// </summary>
        internal void StartRocrail(string workingFolder, string imagesFolder, bool modulePlan)
        {
            // Build the command line
            var cmdline = string.Format("-console {0} -w {1} -img {2} -t {3} -l {4}",
                (modulePlan ? "-modplan " : ""),
                QuoteArgument(workingFolder), 
                QuoteArgument(imagesFolder),
                QuoteArgument(@"temp\rocrail.trace"),
                QuoteArgument(this.folder));

            // Start rocrail
            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = Path.Combine(this.folder, "rocrail.exe");
            info.Arguments = cmdline;
            info.WorkingDirectory = workingFolder;
            Process.Start(info);
        }

        /// <summary>
        /// Start rocview in the given folder.
        /// </summary>
        internal void StartRocview(string workingFolder, string theme, bool modulePlan)
        {
            // Build the command line
            var cmdline = string.Format("{0} -icons {1} -theme {2} -t temp\\rocview.trace", 
                (modulePlan ? "-modview " : ""),
                QuoteArgument(Path.Combine(this.folder, "icons")), 
                QuoteArgument(Path.Combine(this.DefaultThemesFolder, theme)));

            // Start rocview
            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = Path.Combine(this.folder, "rocview.exe");
            info.Arguments = cmdline;
            info.WorkingDirectory = workingFolder;
            Process.Start(info);
        }

        /// <summary>
        /// Gets all install themes
        /// </summary>
        internal IEnumerable<string> Themes
        {
            get
            {
                var directory = new DirectoryInfo(this.DefaultThemesFolder);
                foreach (var folder in directory.GetDirectories())
                {
                    yield return folder.Name;
                }
            }
        }

        /// <summary>
        /// Get the rocrail installation folder form the registry.
        /// </summary>
        private static string FindRocrailLocation(out string name, out string uninstallString)
        {
            string folder = null;
            name = null;
            uninstallString = null;
            foreach (var regkey in REGKEYS)
            {
                using (var key = Registry.LocalMachine.OpenSubKey(regkey))
                {
                    if (key != null)
                    {
                        folder = key.GetValue("InstallLocation") as string;
                        name = key.GetValue("DisplayName") as string;
                        uninstallString = key.GetValue("UninstallString") as string;
                        if ((folder != null) && (name != null)) { return folder; }
                    }
                }
            }
            if (folder == null)
            {
                throw new ApplicationException("Rocrail is not installed");
            }
            return folder;
        }

        /// <summary>
        /// Quotes a command line argument if it contains a single quote or a
        /// space.
        /// </summary>
        /// <param name="argument">The command line argument.</param>
        /// <returns>
        /// A quoted command line argument if <paramref name="argument" /> 
        /// contains a single quote or a space; otherwise, 
        /// <paramref name="argument" />.
        /// </returns>
        private static string QuoteArgument(string argument)
        {
            if (argument.IndexOf("\"") > -1)
            {
                // argument is already quoted
                return argument;
            }
            else if (argument.IndexOf("'") > -1 || argument.IndexOf(" ") > -1)
            {
                // argument contains space and is not quoted, so quote it
                return '\"' + argument + '\"';
            }
            else
            {
                return argument;
            }
        }
    }
}
