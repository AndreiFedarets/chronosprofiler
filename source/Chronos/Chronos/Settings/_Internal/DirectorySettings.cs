using System;
using System.IO;
using System.Xml.Linq;

namespace Chronos.Settings
{
    internal class DirectorySettings : SettingsElement, IDirectorySettings
    {
        private const string PathAttributeName = "Path";

        public DirectorySettings(XElement element)
            : base(element)
        {
        }

        public string Path
        {
            get
            {
                XAttribute attribute = Element.Attribute(PathAttributeName);
                return attribute.Value;
            }
            set
            {
                XAttribute attribute = Element.Attribute(PathAttributeName);
                attribute.Value = value;
            }
        }

        public DirectoryInfo GetDirectory()
        {
            string path = Environment.ExpandEnvironmentVariables(Path);
            DirectoryInfo directory = new DirectoryInfo(path);
            return directory;
        }
    }
}
