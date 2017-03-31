using System.Collections.Generic;

namespace Chronos.Extensibility
{
	public class ProfilingStrategyCollection : IProfilingStrategyCollection
	{
		private readonly IList<IProfilingStrategy> _strategies;

		public ProfilingStrategyCollection()
		{
			_strategies = new List<IProfilingStrategy>();
		}

		public void Register(IProfilingStrategy strategy)
		{
			_strategies.Add(strategy);
		}

		public IEnumerator<IProfilingStrategy> GetEnumerator()
		{
			return _strategies.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
