using System.Xml.Linq;

namespace Chronos.Settings
{
    internal sealed class LoggingSettings : SettingsElement, ILoggingSettings
    {
        private const string FileLoggerElementName = "FileLogger";
        private const string EventLoggerElementName = "EventLogger";

        public LoggingSettings(XElement element)
            : base(element)
        {
            if (Element.Element(FileLoggerElementName) != null)
            {
                FileLogger = new FileLoggerSettings(Element.Element(FileLoggerElementName));
            }
            if (Element.Element(EventLoggerElementName) != null)
            {
                EventLogger = new EventLoggerSettings(Element.Element(EventLoggerElementName));
            }
        }

        public IFileLoggerSettings FileLogger { get; private set; }

        public IEventLoggerSettings EventLogger { get; private set; }
    }
}
