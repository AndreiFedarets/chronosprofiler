using System.Xml.Linq;

namespace Chronos.Settings
{
    internal sealed class ExtensionSettings : DirectorySettings, IExtensionSettings
    {
        private const string IsEnabledAttributeName = "IsEnabled";
        private const string IsRecursiveAttributeName = "IsRecursive";

        public ExtensionSettings(XElement element) :
            base(element)
        {
        }

        public bool IsEnabled
        {
            get
            {
                XAttribute attribute = Element.Attribute(IsEnabledAttributeName);
                return attribute.ValueAsBoolean();
            }
            set
            {
                XAttribute attribute = Element.Attribute(IsEnabledAttributeName);
                attribute.Value = value.ToString();
            }
        }

        public bool IsRecursive
        {
            get
            {
                XAttribute attribute = Element.Attribute(IsRecursiveAttributeName);
                return attribute.ValueAsBoolean();
            }
            set
            {
                XAttribute attribute = Element.Attribute(IsRecursiveAttributeName);
                attribute.Value = value.ToString();
            }
        }
    }
}
