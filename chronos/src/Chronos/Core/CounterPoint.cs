using System;

namespace Chronos.Core
{
	[Serializable]
	[System.Diagnostics.DebuggerDisplay("[{Time}; {Value}]")]
	public class CounterPoint
	{
		public CounterPoint()
		{
			
		}

		public CounterPoint(long time, double value)
		{
			Time = time;
			Value = value;
		}

		public long Time { get; set; }

		public double Value { get; set; }
	}
}
