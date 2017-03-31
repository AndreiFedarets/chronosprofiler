using System.Xml.Linq;

namespace Chronos.Settings
{
    internal sealed class EventLoggerSettings : LoggerSettings, IEventLoggerSettings
    {
        public EventLoggerSettings(XElement element)
            : base(element)
        {
        }
    }
}
