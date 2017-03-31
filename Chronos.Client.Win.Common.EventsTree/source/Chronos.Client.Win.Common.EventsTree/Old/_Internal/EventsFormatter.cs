using System.Diagnostics;

namespace Chronos.Client.Win.Common.EventsTree
{
    public class EventsFormatter : IEventsFormatter
    {
        private readonly IEventFormatter[] _formatters;

        public EventsFormatter()
        {
            _formatters = new IEventFormatter[byte.MaxValue];
        }

        public string FormatName(IEvent @event)
        {
            IEventFormatter formatter = _formatters[@event.EventType];
            if (formatter == null)
            {
                return "<UNKNOWN EVENT>";
            }
            return formatter.FormatName(@event);
        }

        public void RegisterFormatter(IEventFormatter formatter)
        {
            if (_formatters[formatter.EventType] != null)
            {
                LoggingProvider.Current.Log(TraceEventType.Warning, string.Format("Formatter with id {0} is already registered", formatter.EventType));
            }
            _formatters[formatter.EventType] = formatter;
        }
    }
}
