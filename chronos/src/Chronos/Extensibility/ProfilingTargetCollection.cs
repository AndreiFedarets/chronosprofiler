using System.Collections.Generic;

namespace Chronos.Extensibility
{
	public class ProfilingTargetCollection : IProfilingTargetCollection
	{
		private readonly IList<IProfilingTarget> _targets;

		public ProfilingTargetCollection()
		{
			_targets = new List<IProfilingTarget>();
		}

		public void Register(IProfilingTarget target)
		{
			_targets.Add(target);
		}

		public IEnumerator<IProfilingTarget> GetEnumerator()
		{
			return _targets.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
