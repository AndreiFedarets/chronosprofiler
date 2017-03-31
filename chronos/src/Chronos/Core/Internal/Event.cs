using System;
using System.Collections.Generic;
using System.Linq;

namespace Chronos.Core.Internal
{
	internal class Event : IEvent
	{
		private readonly IEventsTreeBuilder _eventsTreeBuilder;
		private readonly byte[] _data;
		private readonly int _offset;
		private List<IEvent> _children;

		public Event(IEvent parent, byte[] data, int offset, uint parentTime, ThreadInfo threadInfo, IEventsTreeBuilder eventsTreeBuilder)
		{
		    Parent = parent;
			_data = data;
			_offset = offset;
			StackFullTime = parentTime;
			ThreadInfo = threadInfo;
			_eventsTreeBuilder = eventsTreeBuilder;
		}

		public ThreadInfo ThreadInfo { get; private set; }

		public long StackFullTime { get; private set; }

		public double Percent
		{
			get
			{
				if (StackFullTime == 0)
				{
					return 0;
				}
				return Math.Round((((double)Time) / ((double)StackFullTime)) * 100, 1);
			}	
		}

		public int Offset
		{
			get { return _offset; }
		}

		unsafe public uint Hits
		{
			get
			{
				fixed (byte* dataPointer = _data)
				{
					return IntermediateEvent.GetHits(dataPointer + _offset);
				}
			}
		}

		unsafe public uint Time
		{
			get
			{
				fixed (byte* dataPointer = _data)
				{
					return IntermediateEvent.GetTime(dataPointer + _offset);
				}
			}
		}

		unsafe public EventType EventType
		{
			get
			{
				fixed (byte* dataPointer = _data)
				{
					return IntermediateEvent.GetEventType(dataPointer + _offset);
				}
			}
		}

		unsafe public uint Unit
		{
			get
			{
				fixed (byte* dataPointer = _data)
				{
					return IntermediateEvent.GetUnit(dataPointer + _offset);
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
					_children = _eventsTreeBuilder.BuildChildren(this, _data, _offset, Time, ThreadInfo).ToList();
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
