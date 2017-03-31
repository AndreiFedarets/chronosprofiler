using System;

namespace Chronos.Core
{
	public class IntermediateEvent
	{
		public static readonly IntermediateEvent Empty = default(IntermediateEvent);
		//Size
		private const int EventTypeSize = sizeof (byte);
		private const int UnitSize = sizeof (uint);
		private const int TimeSize = sizeof (uint);
		private const int DepthSize = sizeof (short);
		private const int HitsSize = sizeof (uint);

		public const int RwSize = EventTypeSize + UnitSize + TimeSize + DepthSize;
		public const int ImSize = EventTypeSize + UnitSize + TimeSize + DepthSize + HitsSize;
		//Offset
		private const int EventTypeOffset = 0;
		private const int UnitOffset = EventTypeOffset + EventTypeSize;
		private const int TimeOffset = UnitOffset + UnitSize;
		//We store depth and hits at the same place, depth is used only once - when bilding stack, after that here will be hits
		private const int DepthOffset = TimeOffset + TimeSize;
		private const int HitsOffset = DepthOffset + DepthSize;

		//EventType
		public static EventType GetEventType(byte[] @event)
		{
			return (EventType)@event[EventTypeOffset];
		}

		unsafe public static EventType GetEventType(byte* @event)
		{
			return (EventType)@event[EventTypeOffset];
		}

		//Unit
		unsafe public static uint GetUnit(byte[] @event)
		{
			fixed (byte* eventPointer = @event)
			{
				return GetUnit(eventPointer);
			}
		}

		unsafe public static uint GetUnit(byte* @event)
		{
			return *((uint*)(@event + UnitOffset));
		}

		//Time
		unsafe public static uint GetTime(byte[] @event)
		{
			fixed (byte* eventPointer = @event)
			{
				return GetTime(eventPointer);
			}
		}

		unsafe public static uint GetTime(byte* @event)
		{
			return *((uint*)(@event + TimeOffset));
		}

		//Depth
		unsafe public static short GetDepth(byte[] @event)
		{
			fixed (byte* eventPointer = @event)
			{
				return GetDepth(eventPointer);
			}
		}

		unsafe public static short GetDepth(byte* @event)
		{
			return *((short*)(@event + DepthOffset));
		}

		//Hits
		unsafe public static uint GetHits(byte[] @event)
		{
			fixed (byte* eventPointer = @event)
			{
				return GetHits(eventPointer);
			}
		}

		unsafe public static uint GetHits(byte* @event)
		{
			return *((uint*)(@event + HitsOffset));
		}

		//Token
		public static long GetToken(byte[] @event)
		{
			long token = GetUnit(@event);
			byte[] tokenBytes = BitConverter.GetBytes(token);
			tokenBytes[4] = (byte)GetEventType(@event);
			return BitConverter.ToInt64(tokenBytes, 0);
		}

		unsafe public static long GetToken(byte* @event)
		{
			long token = GetUnit(@event);
			byte[] tokenBytes = BitConverter.GetBytes(token);
			tokenBytes[4] = (byte)GetEventType(@event);
			return BitConverter.ToInt64(tokenBytes, 0);
		}

	}
}
