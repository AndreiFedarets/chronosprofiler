using Chronos.Client.Win.Controls.Common.EventsTree;

namespace Chronos.Client.Win.ViewModels.Common.EventsTree
{
    public interface IEventsTreeViewModel
    {
        IEventSearch EventSearch { get; }
    }
}
