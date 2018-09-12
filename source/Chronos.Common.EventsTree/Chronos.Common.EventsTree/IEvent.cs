using System.Collections.Generic;

namespace Chronos.Common.EventsTree
{
    public interface IEvent
    {
        uint Hits { get; }

        uint Time { get; }

        byte EventType { get; }

        uint Unit { get; }

        ulong EventHash { get; }

        bool HasChildren { get; }

        IEnumerable<IEvent> Children { get; }

        IEvent Parent { get; }
    }
}
