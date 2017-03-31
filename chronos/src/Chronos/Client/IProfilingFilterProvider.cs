using System.Collections.Generic;
using Chronos.Core;

namespace Chronos.Client
{
	public interface IProfilingFilterProvider
	{
		IEnumerable<ProfilingFilter> Load();

		ProfilingFilter Create(string filterName);

        void Delete(ProfilingFilter filter);

        void Save(ProfilingFilter filter);

        void SetAsDefault(ProfilingFilter filter);

	    void Restore(ProfilingFilter filter);

		ProfilingFilter GetDefault();

		ProfilingFilter Find(FilterType filterType, List<string> items);
	}
}
