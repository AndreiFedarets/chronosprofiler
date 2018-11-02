using System;
using System.Collections.Generic;

namespace Chronos.Client.Win.Common.EventsTree
{
    internal interface IEventsTreeBuilder
    {
        List<IEvent> BuildChildren(IEvent parent, byte[] data, int offset, Lazy<uint> parentTime, uint threadUid);

        List<IEvent> BuildChildren(IEvent parent, byte[] data, Lazy<uint> parentTime, uint threadUid);
    }
}
