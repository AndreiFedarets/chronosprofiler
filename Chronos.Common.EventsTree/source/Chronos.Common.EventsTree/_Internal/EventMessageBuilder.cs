
namespace Chronos.Common.EventsTree
{
    internal sealed class EventMessageBuilder : RemoteBaseObject, IEventMessageBuilder
    {
        private readonly EventMessageBuilderInternal _eventMessageBuilder;

        public EventMessageBuilder()
        {
            _eventMessageBuilder = new EventMessageBuilderInternal();
        }

        public IEventMessage this[byte eventType]
        {
            get { return _eventMessageBuilder[eventType]; }
        }

        public string BuildMessage(IEvent @event)
        {
            return _eventMessageBuilder.BuildMessage(@event);
        }

        public void RegisterMessage(byte eventType, IEventMessage builder)
        {
            _eventMessageBuilder.RegisterMessage(eventType, builder);
        }
    }
}
