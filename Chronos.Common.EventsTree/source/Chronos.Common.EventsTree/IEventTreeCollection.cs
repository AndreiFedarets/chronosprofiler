using System;
using System.Collections.Generic;

namespace Chronos.Common.EventsTree
{
    [PublicService(typeof(Proxy.EventTreeCollection))]
    public interface IEventTreeCollection : IEnumerable<ISingleEventTree>
    {
        event EventHandler<EventTreeEventArgs> CollectionUpdated;
    }
}
