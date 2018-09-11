using System.Xml.Linq;

namespace Chronos.Settings
{
    internal sealed class HostConnectionSettingsCollection : SettingsCollectionElement<IConnectionSettings>, IConnectionSettingsCollection
    {
        private const string ChildElementName = "Connection";
        private const string RunLocalAttributeName = "RunLocal";

        public HostConnectionSettingsCollection(XElement element)
            : base(element, ChildElementName)
        {
        }

        public bool RunLocal
        {
            get
            {
                XAttribute attribute = Element.Attribute(RunLocalAttributeName);
                return attribute.ValueAsBoolean();
            }
            set
            {
                XAttribute attribute = Element.Attribute(RunLocalAttributeName);
                attribute.Value = value.ToString();
            }
        }

        protected override IConnectionSettings CreateItem(XElement element)
        {
            return new ConnectionSettings(element);
        }
    }
}
