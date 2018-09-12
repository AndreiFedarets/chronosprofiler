using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Chronos.Common.EventsTree
{
    public sealed class StaticEventTreeCollection : ReadOnlyCollection<ISingleEventTree>, IEventTreeCollection
    {
        public StaticEventTreeCollection(IEnumerable<ISingleEventTree> eventTrees)
            : base(new List<ISingleEventTree>(eventTrees))
        {
            MinTime = Items.Min(x => x.Time);
            MaxTime = Items.Max(x => x.Time);
        }

        public StaticEventTreeCollection(ISingleEventTree eventTree)
            : base(new List<ISingleEventTree>{ eventTree })
        {
            MinTime = eventTree.Time;
            MaxTime = eventTree.Time;
        }

        public uint MinTime { get; private set; }

        public uint MaxTime { get; private set; }

        public event EventHandler<EventTreeEventArgs> CollectionUpdated;
    }
}
