using System;
using System.Collections.Generic;
using System.Linq;

namespace Chronos.Common.EventsTree
{
    [Serializable]
    internal class Event : IEvent
    {
        private readonly IEventTreeBuilder _eventsTreeBuilder;
        private readonly byte[] _data;
        private readonly int _dataOffset;
        private List<IEvent> _children;

        public Event(IEvent parent, byte[] data, int dataOffset, IEventTreeBuilder eventsTreeBuilder)
        {
            Parent = parent;
            _data = data;
            _dataOffset = dataOffset;
            _eventsTreeBuilder = eventsTreeBuilder;
        }

        public IEvent Parent { get; private set; }

        public IEnumerable<IEvent> Children
        {
            get
            {
                if (_children == null)
                {
                    _children = _eventsTreeBuilder.BuildChildren(this, _data, _dataOffset).ToList();
                }
                return _children;
            }
        }

        public bool HasChildren
        {
            get { return Children.Any(); }
        }

        public unsafe uint Hits
        {
            get
            {
                fixed (byte* dataPointer = _data)
                {
                    return NativeEventHelper.GetHits(dataPointer + _dataOffset);
                }
            }
        }

        public unsafe uint Time
        {
            get
            {
                fixed (byte* dataPointer = _data)
                {
                    return NativeEventHelper.GetTime(dataPointer + _dataOffset);
                }
            }
        }

        public unsafe byte EventType
        {
            get
            {
                fixed (byte* dataPointer = _data)
                {
                    return NativeEventHelper.GetEventType(dataPointer + _dataOffset);
                }
            }
        }

        public unsafe uint Unit
        {
            get
            {
                fixed (byte* dataPointer = _data)
                {
                    return NativeEventHelper.GetUnit(dataPointer + _dataOffset);
                }
            }
        }

        public unsafe ulong EventHash
        {
            get
            {
                fixed (byte* dataPointer = _data)
                {
                    return NativeEventHelper.GetEventHash(dataPointer + _dataOffset);
                }
            }
        }
    }
}
