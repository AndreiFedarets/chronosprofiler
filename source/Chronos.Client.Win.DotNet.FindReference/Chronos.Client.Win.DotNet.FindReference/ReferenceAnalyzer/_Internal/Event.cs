using System;
using System.Collections.Generic;
using System.Linq;
using Chronos.Common.EventsTree;

namespace Chronos.Client.Win.Common.EventsTree
{
    internal class Event : IEvent
    {
        private readonly IEventsTreeBuilder _eventsTreeBuilder;
        private readonly byte[] _data;
        private readonly int _offset;
        private readonly Lazy<uint> _parentTime;
        private readonly Lazy<uint> _time;
        private List<IEvent> _children;

        public Event(IEvent parent, byte[] data, int offset, Lazy<uint> parentTime, uint threadUid, IEventsTreeBuilder eventsTreeBuilder)
        {
            Parent = parent;
            _data = data;
            _offset = offset;
            _parentTime = parentTime;
            ThreadUid = threadUid;
            _eventsTreeBuilder = eventsTreeBuilder;
            _time = new Lazy<uint>(GetTime);
        }

        public uint ThreadUid { get; private set; }

        public long StackFullTime
        {
            get { return _parentTime.Value; }
        }

        public double Percent
        {
            get
            {
                if (StackFullTime == 0)
                {
                    return 0;
                }
                return Math.Round((((double) Time)/((double) StackFullTime))*100, 1);
            }
        }

        public int Offset
        {
            get { return _offset; }
        }

        public unsafe uint Hits
        {
            get
            {
                fixed (byte* dataPointer = _data)
                {
                    return NativeEventHelper.GetHits(dataPointer + _offset);
                }
            }
        }

        private unsafe uint GetTime()
        {
            fixed (byte* dataPointer = _data)
            {
                return NativeEventHelper.GetTime(dataPointer + _offset);
            }
        }

        public uint Time
        {
            get
            {
                return _time.Value;
            }
        }

        public unsafe byte EventType
        {
            get
            {
                fixed (byte* dataPointer = _data)
                {
                    return NativeEventHelper.GetEventType(dataPointer + _offset);
                }
            }
        }

        public unsafe uint Unit
        {
            get
            {
                fixed (byte* dataPointer = _data)
                {
                    return NativeEventHelper.GetUnit(dataPointer + _offset);
                }
            }
        }

        public IEvent Parent { get; private set; }

        public IEnumerable<IEvent> Children
        {
            get
            {
                if (_children == null)
                {
                    _children = _eventsTreeBuilder.BuildChildren(this, _data, _offset, _time, ThreadUid).ToList();
                }
                return _children;
            }
        }

        public bool HasChildren
        {
            get { return Children.Any(); }
        }
    }
}
