using System.Collections.Generic;

namespace Chronos.Client.Win
{
	public class ProfilingFilterItemCollection : IEnumerable<ProfilingFilterItem>
	{
		private readonly IList<ProfilingFilterItem> _filterItems;

		public ProfilingFilterItemCollection(ProfilingFilter filter)
		{
			_filterItems = new List<ProfilingFilterItem>();
			Initialize(filter);
		}

		public ProfilingFilterItemCollection(IEnumerable<ProfilingFilterItem> filterItems)
		{
			_filterItems = new List<ProfilingFilterItem>(filterItems);
		}

		private void Initialize(ProfilingFilter filter)
		{
			foreach (AssemblyName item in filter.Items)
			{
				_filterItems.Add(new ProfilingFilterItem(item));
			}
		}

		public IEnumerator<ProfilingFilterItem> GetEnumerator()
		{
			return _filterItems.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
