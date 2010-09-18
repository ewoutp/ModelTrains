using System.IO;
using System.Xml;

namespace MGV.RocRailStartup.Rocrail
{
    /// <summary>
    /// rocrail.ini configuration wrapper
    /// </summary>
    public class RocrailIni
    {
        private readonly XmlDocument document;

        /// <summary>
        /// Default ctor
        /// </summary>
        public RocrailIni(string folder)
        {
            this.document = new XmlDocument();
            this.document.Load(Path.Combine(folder, "rocrail.ini"));
        }

        /// <summary>
        /// Is the configured plan a module plan?
        /// </summary>
        public bool IsModulePlan
        {
            get
            {
                var root = document.DocumentElement;
                var createmodplan = root.GetAttribute("createmodplan");
                return (createmodplan == "true");
            }
        }
    }
}
