using System;
using System.Collections.Generic;

namespace Chronos.Core
{
	public interface IPerformanceCounterCollection : IEnumerable<IPerformanceCounter>
	{
		IPerformanceCounter this[Guid counterToken] { get; }

		IPerformanceCounter Register(PerformanceCounterInfo counterInfo);

		void UpdateValues();
	}
}
