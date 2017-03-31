using System;

namespace Chronos.Core
{
	[Serializable]
	public class PerformanceCounterInfo
	{
		public PerformanceCounterInfo()
		{
		}

		public PerformanceCounterInfo(string categoryName, string counterName, string instanceName)
			: this(categoryName, counterName, instanceName, Guid.NewGuid())
		{
		}

		public PerformanceCounterInfo(string categoryName, string counterName, string instanceName, Guid token)
		{
			CategoryName = categoryName;
			CounterName = counterName;
			InstanceName = instanceName;
			Token = token;
		}

		public string CategoryName { get; set; }

		public string CounterName { get; set; }

		public string InstanceName { get; set; }

		public Guid Token { get; set; }
	}
}
