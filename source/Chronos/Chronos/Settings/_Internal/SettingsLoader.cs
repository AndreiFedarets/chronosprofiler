using System.IO;
using System.Xml.Linq;

namespace Chronos.Settings
{
    internal static class SettingsLoader
    {
        private const string SettingsFileName = "settings.xml";

        public static XDocument Load()
        {
            string settingsFile = typeof(SettingsLoader).Assembly.Location;
            settingsFile = Path.GetDirectoryName(settingsFile);
            settingsFile = Path.Combine(settingsFile, SettingsFileName);
            using(FileStream stream = new FileStream(settingsFile, FileMode.Open, FileAccess.Read))
	        {
                XDocument document = XDocument.Load(stream);
                return document;
	        }
        }
    }
}
