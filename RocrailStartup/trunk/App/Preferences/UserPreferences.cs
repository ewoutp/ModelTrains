using System.Configuration;
using Microsoft.Win32;

namespace MGV.RocRailStartup.Preferences
{
    /// <summary>
    /// User settings
    /// </summary>
    [SettingsProvider(typeof(CustomSettingsProvider))]
    internal sealed class UserPreferences : ApplicationSettingsBase
    {
        private static UserPreferences instance;

        #region Public statics
        /// <summary>
        /// Gets the global instance.
        /// </summary>
        public static UserPreferences Preferences
        {
            get
            {
                if (instance == null)
                {
                    lock (typeof(UserPreferences))
                    {
                        if (instance == null)
                        {
                            instance = (UserPreferences)SettingsBase.Synchronized(new UserPreferences());
                        }
                    }
                }
                return instance;
            }
        }

        /// <summary>
        /// Save the preferences now
        /// </summary>
        public static void SaveNow()
        {
            if (instance != null)
            {
                instance.Save();
            }
        }
        #endregion

        /// <summary>
        /// Selected theme name
        /// </summary>
        [UserScopedSetting(),
        DefaultSettingValue("")]
        public string Theme
        {
            get { return (string)this["Theme"]; }
            set { this["Theme"] = value; }
        }

        /// <summary>
        /// Selected UI language
        /// </summary>
        [UserScopedSetting(),
        DefaultSettingValue("en-US")]
        public string Language
        {
            get { return (string)this["Language"]; }
            set { this["Language"] = value; }
        }

        /// <summary>
        /// Selected track name
        /// </summary>
        [UserScopedSetting(),
        DefaultSettingValue("")]
        public string Track
        {
            get { return (string)this["Track"]; }
            set { this["Track"] = value; }
        }

        /// <summary>
        /// Gets folders containing tracks
        /// </summary>
        [UserScopedSetting(), 
        DefaultSettingValue("")]
        public string TracksFolders
        {
            get { return (string)this["TracksFolders"]; }
            set { this["TracksFolders"] = value; }
        }

        /// <summary>
        /// Settings provider for user prefs.
        /// </summary>
        private sealed class CustomSettingsProvider : RegistryAssemblySettingsProvider
        {
            private static readonly string[] KEYS = new string[] { @"Software\ModelSpoorGroepVenlo\RocRailStartup\Preferences" };

            /// <summary>
            /// Try to read the latest path
            /// </summary>
            /// <param name="writable"></param>
            /// <returns></returns>
            protected override string GetSubKeyPath(bool writable)
            {
                // Always write to latest key
                if (writable) { return KEYS[0]; }

                // Read from the latest available key
                foreach (var key in KEYS)
                {
                    var regKey = Registry.CurrentUser.OpenSubKey(key);
                    if (regKey != null) { regKey.Close(); return key; }
                }

                // No key found, use latest
                return KEYS[0];
            }
        }
    }
}
