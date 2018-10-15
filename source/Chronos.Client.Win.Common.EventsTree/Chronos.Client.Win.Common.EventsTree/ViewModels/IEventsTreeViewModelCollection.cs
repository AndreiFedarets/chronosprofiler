using System.Collections.Generic;
using Chronos.Common.EventsTree;

namespace Chronos.Client.Win.ViewModels.Common.EventsTree
{
    [PublicService]
    public interface IEventsTreeViewModelCollection : IEnumerable<IEventsTreeViewModel>
    {
        IEventsTreeViewModel Open();

        IEventsTreeViewModel Open(IEventTreeCollection collection);
    }
}
