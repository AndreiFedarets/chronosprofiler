using System.Collections.Generic;

namespace Chronos.Core
{
	public interface IPerformanceCounter : IEnumerable<CounterPoint>
	{
		PerformanceCounterInfo CounterInfo { get; }

		void UpdateValues();

		CounterPoint[] GetAfterTime(long time);
	}
}
