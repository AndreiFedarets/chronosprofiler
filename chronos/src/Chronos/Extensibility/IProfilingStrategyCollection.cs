using System.Collections.Generic;

namespace Chronos.Extensibility
{
	public interface IProfilingStrategyCollection : IEnumerable<IProfilingStrategy>
	{
		void Register(IProfilingStrategy strategy);
	}
}
