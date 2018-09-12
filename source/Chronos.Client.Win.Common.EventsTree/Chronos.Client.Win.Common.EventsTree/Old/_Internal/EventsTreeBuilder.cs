using System;
using System.Collections.Generic;
using Chronos.Common.EventsTree;

namespace Chronos.Client.Win.Common.EventsTree
{
    internal class EventsTreeBuilder : IEventsTreeBuilder
    {
        public unsafe List<IEvent> BuildChildren(IEvent parent, byte[] data, int offset, Lazy<uint> parentTime, uint threadUid)
        {
            List<IEvent> events = new List<IEvent>();
            fixed (byte* dataPointer = data)
            {
                byte* parentPointer = dataPointer + offset;
                short parentDepth = NativeEventHelper.GetDepth(parentPointer);
                short targetDepth = (short) (parentDepth + 1);
                offset += NativeEventHelper.EventSize;
                for (;
                    offset < data.Length && NativeEventHelper.GetDepth(dataPointer + offset) > parentDepth;
                    offset += NativeEventHelper.EventSize)
                {
                    if (NativeEventHelper.GetDepth(dataPointer + offset) == targetDepth)
                    {
                        IEvent @event = new Event(parent, data, offset, parentTime, threadUid, this);
                        events.Add(@event);
                    }
                }
            }
            return events;
        }

        public unsafe List<IEvent> BuildChildren(IEvent parent, byte[] data, Lazy<uint> parentTime, uint threadUid)
        {
            List<IEvent> events = new List<IEvent>();
            fixed (byte* dataPointer = data)
            {
                for (int offset = 0; offset < data.Length; offset += NativeEventHelper.EventSize)
                {
                    if (NativeEventHelper.GetDepth(dataPointer + offset) == 0)
                    {
                        IEvent @event = new Event(parent, data, offset, parentTime, threadUid, this);
                        events.Add(@event);
                    }
                }
            }
            return events;
        }
    }
}
