using System;
using System.Collections.Generic;

namespace Chronos.Common.EventsTree
{
    [Serializable]
    internal sealed class SingleEventTree : Event, ISingleEventTree
    {
        private readonly byte[] _data;
        private readonly Lazy<uint> _time;

        public SingleEventTree(ulong eventsTreeUid, uint threadUid, uint threadOsId, uint beginLifetime, uint endLifetime, byte[] data)
            : base(null, data, 0, new EventTreeBuilder())
        {
            EventTreeUid = eventsTreeUid;
            ThreadUid = threadUid;
            ThreadOsId = threadOsId;
            BeginLifetime = beginLifetime;
            EndLifetime = endLifetime;
            _data = data;
            _time = new Lazy<uint>(GetTime);
        }

        public uint ThreadUid { get; private set; }

        public ulong EventTreeUid { get; private set; }

        public uint ThreadOsId { get; private set; }

        public uint BeginLifetime { get; private set; }

        public uint EndLifetime { get; private set; }

        private uint GetTime()
        {
            uint time = 0;
            foreach (IEvent child in Children)
            {
                time += child.Time;
            }
            return time;
        }

        public byte[] GetBinaryData()
        {
            return _data;
        }

        private List<IEvent> Load()
        {
            List<IEvent> events = EventTreeBuilder.Current.BuildChildren(this, _data);
            return events;
        }
    }
}
