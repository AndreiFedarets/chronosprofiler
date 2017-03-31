using System.Collections.Generic;
using System.Linq;

namespace Chronos.Core
{
	public class PerformanceCounter : IPerformanceCounter
	{
		private readonly System.Diagnostics.PerformanceCounter _counter;
		private readonly IList<CounterPoint> _points;

		public PerformanceCounter(PerformanceCounterInfo counterInfo)
		{
			CounterInfo = counterInfo;
			_points = new List<CounterPoint>();
			_counter = Initialize(CounterInfo);
		}

		private static System.Diagnostics.PerformanceCounter Initialize(PerformanceCounterInfo counterInfo)
		{
			System.Diagnostics.PerformanceCounter counter = new System.Diagnostics.PerformanceCounter(counterInfo.CategoryName, counterInfo.CounterName, counterInfo.InstanceName);
			return counter;
		}

		public void UpdateValues()
		{
			System.Diagnostics.CounterSample sample = _counter.NextSample();
			float value = _counter.NextValue();
			long time = sample.CounterTimeStamp;
			_points.Add(new CounterPoint(time, value));
		}

		public CounterPoint[] GetAfterTime(long time)
		{
			CounterPoint[] points = _points.Where(x => x.Time >= time).ToArray();
			return points;
		}

		public PerformanceCounterInfo CounterInfo { get; private set; }

		public IEnumerator<CounterPoint> GetEnumerator()
		{
			return _points.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
