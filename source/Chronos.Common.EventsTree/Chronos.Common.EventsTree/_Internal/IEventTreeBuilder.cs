using System;
using System.Collections.Generic;

namespace Chronos.Common.EventsTree
{
    internal interface IEventTreeBuilder
    {
        List<IEvent> BuildChildren(IEvent parent, byte[] data, int offset);

        List<IEvent> BuildChildren(IEventTree parent, byte[] data);
    }
}
