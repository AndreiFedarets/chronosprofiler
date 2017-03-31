using System.Collections.Generic;

namespace Chronos.Core.Internal
{
	internal class EventsTreeBuilder : IEventsTreeBuilder
	{
        unsafe public List<IEvent> BuildChildren(IEvent parent, byte[] data, int offset, uint parentTime, ThreadInfo threadInfo)
		{
			List<IEvent> events = new List<IEvent>();
			fixed (byte* dataPointer = data)
			{
				byte* parentPointer = dataPointer + offset;
				short parentDepth = IntermediateEvent.GetDepth(parentPointer);
				short targetDepth = (short)(parentDepth + 1);
				offset += IntermediateEvent.ImSize;
				for (; offset < data.Length && IntermediateEvent.GetDepth(dataPointer + offset) > parentDepth; offset += IntermediateEvent.ImSize)
				{
					if (IntermediateEvent.GetDepth(dataPointer + offset) == targetDepth)
					{
						IEvent @event = new Event(parent, data, offset, parentTime, threadInfo, this);
						events.Add(@event);
					}
				}
			}
			return events;
		}

        unsafe public List<IEvent> BuildChildren(IEvent parent, byte[] data, uint parentTime, ThreadInfo threadInfo)
		{
			List<IEvent> events = new List<IEvent>();
			fixed (byte* dataPointer = data)
			{
				for (int offset = 0; offset < data.Length; offset += IntermediateEvent.ImSize)
				{
					if (IntermediateEvent.GetDepth(dataPointer + offset) == 0)
					{
						IEvent @event = new Event(parent, data, offset, parentTime, threadInfo, this);
						events.Add(@event);
					}
				}
			}
			return events;
		}
	}
}
