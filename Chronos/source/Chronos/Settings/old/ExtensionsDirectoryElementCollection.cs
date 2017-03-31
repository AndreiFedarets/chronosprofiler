using System.Configuration;

namespace Chronos.Config
{
    public class ExtensionsDirectoryElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ExtensionsDirectoryElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ExtensionsDirectoryElement) element).Path.ToLowerInvariant();
        }
    }
}
