using System.Xml.Linq;

namespace Chronos.Settings
{
    internal sealed class ExtensionSettingsCollection : SettingsCollectionElement<IExtensionSettings>, IExtensionSettingsCollection
    {
        private const string ChildElementName = "Extension";

        public ExtensionSettingsCollection(XElement element)
            : base(element, ChildElementName)
        {
        }

        protected override IExtensionSettings CreateItem(XElement element)
        {
            return new ExtensionSettings(element);
        }
    }
}
