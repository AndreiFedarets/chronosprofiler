using System.Configuration;

namespace Chronos.Config
{
    public class ExtensionsDirectoryElement : ConfigurationElement
    {
        private const string PathPropertyName = "path";

        [ConfigurationProperty(PathPropertyName, IsRequired = true)]
        public string Path
        {
            get { return (string)this[PathPropertyName]; }
            set { this[PathPropertyName] = value; }
        }
    }
}
