namespace Chronos.Common.EventsTree
{
    [PublicService(typeof(Proxy.EventMessageBuilder))]
    public interface IEventMessageBuilder
    {
        IEventMessage this[byte eventType] { get; }

        string BuildMessage(IEvent @event);

        void RegisterMessage(byte eventType, IEventMessage builder);
    }
}
