namespace Chronos.Client.Win.Common.EventsTree
{
    public interface IEventsFormatter
    {
        string FormatName(IEvent @event);

        void RegisterFormatter(IEventFormatter formatter);
    }
}
