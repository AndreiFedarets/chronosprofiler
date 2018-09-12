using System;
using System.Collections.Generic;

namespace Chronos.Common.EventsTree
{
    [PublicService(typeof(Proxy.EventTreeCollection))]
    public interface IEventTreeCollection : IEnumerable<ISingleEventTree>
    {
        uint MinTime { get; }

        uint MaxTime { get; }

        event EventHandler<EventTreeEventArgs> CollectionUpdated;
    }
}
