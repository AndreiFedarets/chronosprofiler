namespace Chronos.Common.EventsTree
{
    public interface IEventMessage
    {
        string BuildMessage(IEvent @event);
    }
}
