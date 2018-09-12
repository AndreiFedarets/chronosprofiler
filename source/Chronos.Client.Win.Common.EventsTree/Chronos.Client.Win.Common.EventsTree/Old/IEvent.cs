using System.Collections.Generic;

namespace Chronos.Client.Win.Common.EventsTree
{
    public interface IEvent
    {
        uint ThreadUid { get; }

        uint Hits { get; }

        uint Time { get; }

        long StackFullTime { get; }

        byte EventType { get; }

        bool HasChildren { get; }

        uint Unit { get; }

        double Percent { get; }

        IEnumerable<IEvent> Children { get; }

        IEvent Parent { get; }
    }
}
