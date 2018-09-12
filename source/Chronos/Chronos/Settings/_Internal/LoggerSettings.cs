using System.Diagnostics;
using System.Xml.Linq;

namespace Chronos.Settings
{
    internal abstract class LoggerSettings : SettingsElement, ILoggerSettings
    {
        private const string LogLevelAttributeName = "LogLevel";
        private const string IsEnabledAttributeName = "IsEnabled";

        protected LoggerSettings(XElement element)
            : base(element)
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

        public SourceLevels LogLevel
        {
            get
            {
                XAttribute attribute = Element.Attribute(LogLevelAttributeName);
                return attribute.ValueAsEnum<SourceLevels>();
            }
            set
            {
                XAttribute attribute = Element.Attribute(LogLevelAttributeName);
                attribute.Value = value.ToString();
            }
        }
    }
}
