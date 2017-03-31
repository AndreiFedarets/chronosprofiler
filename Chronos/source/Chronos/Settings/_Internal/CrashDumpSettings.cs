using System.Xml.Linq;

namespace Chronos.Settings
{
    internal sealed class CrashDumpSettings : SettingsElement, ICrashDumpSettings
    {
        private const string DumpsDirectoryElementName = "DumpsDirectory";
        private const string IsEnabledAttributeName = "IsEnabled";

        public CrashDumpSettings(XElement element)
            : base(element)
        {
            if (Element.Element(DumpsDirectoryElementName) != null)
            {
                DumpsDirectory = new DirectorySettings(Element.Element(DumpsDirectoryElementName));
            }
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

        public IDirectorySettings DumpsDirectory { get; private set; }
    }
}
