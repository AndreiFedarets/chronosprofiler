using System.Collections.Generic;

namespace Chronos.Extensibility
{
	public interface IProfilingTargetCollection : IEnumerable<IProfilingTarget>
	{
		void Register(IProfilingTarget target);
	}
}
