using System;
using System.Collections.Generic;

namespace Chronos.Client.Win.Common.EventsTree
{
    internal class EventsTree : IEventsTree
    {
        private readonly IEventsTreeBuilder _eventsTreeBuilder;
        private readonly byte[] _data;
        private List<IEvent> _events;
        private Lazy<uint> _time;

        public EventsTree(uint threadUid, byte[] data)
        {
            ThreadUid = threadUid;
            _data = data;
            _eventsTreeBuilder = new EventsTreeBuilder();
            _time = new Lazy<uint>(() =>
                                       {
                                           uint time = 0;
                                           foreach (IEvent child in Children)
                                           {
                                               time += child.Time;
                                           }
                                           return time;
                                       });
        }

        public uint ThreadUid { get; private set; }

        public uint Hits
        {
            get { return 1; }
        }

        public uint Time
        {
            get { return _time.Value; }
        }

        public long StackFullTime
        {
            get { return 0; }
        }

        public byte EventType
        {
            get { return byte.MaxValue - 1; }
        }

        public bool HasChildren
        {
            get { return _data.Length > 0; }
        }

        public uint Unit
        {
            get { return ThreadUid; }
        }

        public double Percent
        {
            get
            {
                return 0;
                //if (StackFullTime == 0)
                //{
                //    return 0;
                //}
                //return Math.Round((((double)Time) / ((double)StackFullTime)) * 100, 1);
            }
        }

        public IEnumerable<IEvent> Children
        {
            get { return _events ?? (_events = Load()); }
        }

        public IEvent Parent
        {
            get { return null; }
        }

        private List<IEvent> Load()
        {
            List<IEvent> events = _eventsTreeBuilder.BuildChildren(this, _data, _time, ThreadUid);
            return events;
        }
    }
}
