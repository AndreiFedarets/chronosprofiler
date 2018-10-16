using Chronos.Common.EventsTree;

namespace Chronos.Client.Win.Controls.Common.EventsTree
{
    public interface IEventSearchAdapter
    {
        bool Match(IEvent @event);
    }
}
