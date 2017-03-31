namespace Chronos.Client.Win.Common.EventsTree
{
    public interface IEventFormatter
    {
        byte EventType { get; }

        string FormatName(IEvent @event);
    }
}
