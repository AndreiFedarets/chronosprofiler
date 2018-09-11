using Chronos.Proxy;

namespace Chronos.Common.EventsTree.Proxy
{
    internal sealed class EventMessageBuilder : ProxyBaseObject<IEventMessageBuilder>, IEventMessageBuilder
    {
        private IServiceContainer _container;
        private readonly EventMessageBuilderInternal _eventMessageBuilder;

        public EventMessageBuilder(IEventMessageBuilder remoteObject)
            : base(remoteObject)
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

        [ServiceProxyInitializationMethod]
        public void SetDependencies(IServiceContainer container)
        {
            _container = container;
            InitializeCollection();
        }

        private void InitializeCollection()
        {
            for (short i = byte.MinValue; i < byte.MaxValue; i++)
            {
                byte eventType = (byte) i;
                IEventMessage eventMessage = RemoteObject[eventType];
                if (eventMessage != null)
                {
                    IEventMessage eventMessageProxy = (IEventMessage)_container.BuildServiceProxy(eventMessage);
                    if (eventMessageProxy != null)
                    {
                        eventMessage = eventMessageProxy;
                    }
                    _eventMessageBuilder[eventType] = eventMessage;
                }
            }
        }
    }
}
