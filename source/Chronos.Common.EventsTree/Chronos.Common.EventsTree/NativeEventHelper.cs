using System;
namespace Chronos.Common.EventsTree
{
    public class NativeEventHelper
    {
        //Size
        private const int EventTypeSize = sizeof(byte);
        private const int DepthSize = sizeof(short);
        private const int UnitSize = sizeof (uint);
        private const int TimeSize = sizeof (uint);
        private const int HitsSize = sizeof (uint);

        public const int EventSize = EventTypeSize + DepthSize + UnitSize + TimeSize + HitsSize;
        //Offset
        private const int EventTypeOffset = 0;
        private const int DepthOffset = EventTypeOffset + EventTypeSize;
        private const int UnitOffset = DepthOffset + DepthSize;
        private const int TimeOffset = UnitOffset + UnitSize;
        private const int HitsOffset = TimeOffset + TimeSize;

        //We store depth and hits at the same place, depth is used only once - when bilding stack, after that here will be hits
        

        //EventType ==============================================================
        public static byte GetEventType(byte[] @event)
        {
            return @event[EventTypeOffset];
        }

        public static unsafe byte GetEventType(byte* @event)
        {
            return @event[EventTypeOffset];
        }
        //------------------------------------------------------------------------
        public static void SetEventType(byte[] @event, byte value)
        {
            @event[EventTypeOffset] = value;
        }

        public static unsafe void SetEventType(byte* @event, byte value)
        {
            @event[EventTypeOffset] = value;
        }
        //========================================================================

        //Unit ===================================================================
        public static unsafe uint GetUnit(byte[] @event)
        {
            fixed (byte* eventPointer = @event)
            {
                return GetUnit(eventPointer);
            }
        }

        public static unsafe uint GetUnit(byte* @event)
        {
            return *((uint*) (@event + UnitOffset));
        }
        //------------------------------------------------------------------------
        public static unsafe void SetUnit(byte[] @event, uint value)
        {
            fixed (byte* eventPointer = @event)
            {
                SetUnit(eventPointer, value);
            }
        }

        public static unsafe void SetUnit(byte* @event, uint value)
        {
            *((uint*)(@event + UnitOffset)) = value;
        }
        //========================================================================

        //Time ===================================================================
        public static unsafe uint GetTime(byte[] @event)
        {
            fixed (byte* eventPointer = @event)
            {
                return GetTime(eventPointer);
            }
        }

        public static unsafe uint GetTime(byte* @event)
        {
            return *((uint*) (@event + TimeOffset));
        }
        //------------------------------------------------------------------------
        public static unsafe void SetTime(byte[] @event, uint value)
        {
            fixed (byte* eventPointer = @event)
            {
                SetTime(eventPointer, value);
            }
        }

        public static unsafe void SetTime(byte* @event, uint value)
        {
            *((uint*)(@event + TimeOffset)) = value;
        }
        //========================================================================

        //Depth ==================================================================
        public static unsafe short GetDepth(byte[] @event)
        {
            fixed (byte* eventPointer = @event)
            {
                return GetDepth(eventPointer);
            }
        }

        public static unsafe short GetDepth(byte* @event)
        {
            return *((short*) (@event + DepthOffset));
        }

        //------------------------------------------------------------------------
        public static unsafe void SetDepth(byte[] @event, short value)
        {
            fixed (byte* eventPointer = @event)
            {
                SetDepth(eventPointer, value);
            }
        }

        public static unsafe void SetDepth(byte* @event, short value)
        {
            *((short*)(@event + DepthOffset)) = value;
        }
        //========================================================================

        //Hits ===================================================================
        public static unsafe uint GetHits(byte[] @event)
        {
            fixed (byte* eventPointer = @event)
            {
                return GetHits(eventPointer);
            }
        }

        public static unsafe uint GetHits(byte* @event)
        {
            return *((uint*) (@event + HitsOffset));
        }
        //------------------------------------------------------------------------
        public static unsafe void SetHits(byte[] @event, uint value)
        {
            fixed (byte* eventPointer = @event)
            {
                SetHits(eventPointer, value);
            }
        }

        public static unsafe void SetHits(byte* @event, uint value)
        {
            *((uint*)(@event + HitsOffset)) = value;
        }
        //========================================================================

        //EventHash ==============================================================
        public static unsafe ulong GetEventHash(byte[] @event)
        {
            fixed (byte* eventPointer = @event)
            {
                return GetEventHash(eventPointer);
            }
        }

        public static unsafe ulong GetEventHash(byte* @event)
        {
            ulong h = 0;
            byte* p = (byte*)&h;
            *(p + 3) = GetEventType(@event);
            *((uint*)(p + 4)) = GetUnit(@event);
            return h;
        }
        //========================================================================

        //Percent
        public static double GetEventPercent(IEvent @event)
        {
            IEvent parent = @event.Parent;
            if (parent == null || parent.Time == 0)
            {
                return 0;
            }
            return Math.Round((((double)@event.Time) / ((double)parent.Time)) * 100, 1);
        }

        //Percent
        public static double GetEventPercent(uint time, uint minTime, uint maxTime)
        {
            uint lenght = maxTime - minTime;
            if (lenght == 0)
            {
                return 100;
            }
            return Math.Round((((double)(time - minTime)) / ((double)lenght)) * 100, 1);
        }

        //Fake Event
        public static unsafe byte[] CreateEvent(byte eventType, short depth, uint unit, uint time, uint hits)
        {
            byte[] data = new byte[EventSize];
            fixed (byte* dataPointer = data)
            {
                SetEventType(dataPointer, eventType);
                SetDepth(dataPointer, depth);
                SetUnit(dataPointer, unit);
                SetTime(dataPointer, time);
                SetHits(dataPointer, hits);
            }
            return data;
        }
    }
}
