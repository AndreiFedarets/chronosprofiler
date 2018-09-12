using System.Collections.Generic;

namespace Chronos.Client.Win.Common.EventsTree
{
    public interface IEventsTreeCollection : IEnumerable<IEventsTree>
    {
        void Refresh();
    }
}
