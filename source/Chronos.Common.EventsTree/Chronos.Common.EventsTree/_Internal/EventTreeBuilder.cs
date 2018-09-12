using System;
using System.Collections.Generic;

namespace Chronos.Common.EventsTree
{
    [Serializable]
    internal sealed class EventTreeBuilder : IEventTreeBuilder
    {
        static EventTreeBuilder()
        {
            Current = new EventTreeBuilder();
        }

        public static IEventTreeBuilder Current { get; private set; }

        public unsafe List<IEvent> BuildChildren(IEvent parent, byte[] data, int offset)
        {
            List<IEvent> events = new List<IEvent>();
            fixed (byte* dataPointer = data)
            {
                byte* parentPointer = dataPointer + offset;
                short parentDepth = NativeEventHelper.GetDepth(parentPointer);
                short targetDepth = (short)(parentDepth + 1);
                offset += NativeEventHelper.EventSize;
                for (;
                    offset < data.Length && NativeEventHelper.GetDepth(dataPointer + offset) > parentDepth;
                    offset += NativeEventHelper.EventSize)
                {
                    if (NativeEventHelper.GetDepth(dataPointer + offset) == targetDepth)
                    {
                        IEvent @event = new Event(parent, data, offset, this);
                        events.Add(@event);
                    }
                }
            }
            return events;
        }

        public unsafe List<IEvent> BuildChildren(IEventTree parent, byte[] data)
        {
            List<IEvent> events = new List<IEvent>();
            fixed (byte* dataPointer = data)
            {
                for (int offset = 0; offset < data.Length; offset += NativeEventHelper.EventSize)
                {
                    if (NativeEventHelper.GetDepth(dataPointer + offset) == 0)
                    {
                        IEvent @event = new Event(parent, data, offset, this);
                        events.Add(@event);
                    }
                }
            }
            return events;
        }
    }
}
