using System.Xml.Linq;

namespace Chronos.Settings
{
    internal sealed class DirectorySettingsCollection : SettingsCollectionElement<IDirectorySettings>, IDirectorySettingsCollection
    {
        private const string ChildElementName = "Directory";

        public DirectorySettingsCollection(XElement element)
            : base(element, ChildElementName)
        {
        }

        protected override IDirectorySettings CreateItem(XElement element)
        {
            return new DirectorySettings(element);
        }
    }
}
