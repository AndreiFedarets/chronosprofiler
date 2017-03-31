using System;
using System.Collections.Generic;
using System.Linq;

namespace Chronos.Core
{
	public class PerformanceCounterCollection :IPerformanceCounterCollection
	{
		private readonly IList<IPerformanceCounter> _counters;

		public PerformanceCounterCollection()
		{
			_counters = new List<IPerformanceCounter>();
		}

		public IPerformanceCounter this[Guid counterToken]
		{
			get { return _counters.FirstOrDefault(x => x.CounterInfo.Token == counterToken); }
		}

		public IPerformanceCounter Register(PerformanceCounterInfo counterInfo)
		{
			IPerformanceCounter counter = new PerformanceCounter(counterInfo);
			_counters.Add(counter);
			return counter;
		}

		public IEnumerator<IPerformanceCounter> GetEnumerator()
		{
			return _counters.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public void UpdateValues()
		{
			foreach (IPerformanceCounter counter in _counters)
			{
				counter.UpdateValues();
			}
		}
	}
}
