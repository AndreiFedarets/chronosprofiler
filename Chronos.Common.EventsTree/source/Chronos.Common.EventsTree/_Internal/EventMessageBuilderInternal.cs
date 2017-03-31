using System.Diagnostics;

namespace Chronos.Common.EventsTree
{
    internal sealed class EventMessageBuilderInternal
    {
        private readonly IEventMessage[] _builders;

        public EventMessageBuilderInternal()
        {
            _builders = new IEventMessage[byte.MaxValue];
        }

        public IEventMessage this[byte eventType]
        {
            get { return _builders[eventType]; }
            set { _builders[eventType] = value; }
        }

        public string BuildMessage(IEvent @event)
        {
            IEventMessage builder = _builders[@event.EventType];
            string message = builder == null ? "<UNKNOWN_EVENT_TYPE>" : builder.BuildMessage(@event);
            return message;
        }

        public void RegisterMessage(byte eventType, IEventMessage builder)
        {
            if (_builders[eventType] != null)
            {
                LoggingProvider.Current.Log(TraceEventType.Warning, string.Format("Formatter with id {0} is already registered", eventType));
            }
            _builders[eventType] = builder;
        }
    }
}
