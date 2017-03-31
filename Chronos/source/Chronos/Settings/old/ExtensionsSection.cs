using System.Configuration;

namespace Chronos.Config
{
    public class ExtensionsSection : ConfigurationSection
    {
        private const string DirectoriesPropertyName = "directories";

        [ConfigurationProperty(DirectoriesPropertyName, IsRequired = false)]
        public ExtensionsDirectoryElementCollection Directories
        {
            get { return (ExtensionsDirectoryElementCollection)this[DirectoriesPropertyName]; }
            set { this[DirectoriesPropertyName] = value; }
        }

    }
}
