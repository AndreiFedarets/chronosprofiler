using Chronos.Common.EventsTree;

namespace Chronos.Client.Win.ViewModels.Common.EventsTree
{
    public interface IEventsTreeViewModel
    {
        IEvent SelectedEvent { get; set; }
    }
}
