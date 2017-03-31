using System;
using System.Collections.Generic;
using Chronos.Core;

namespace Chronos.Extensibility
{
	public interface IProfilingTarget
	{
		event Action Changed;

		object Icon { get; }

		string DisplayName { get; }

		object GetView();

		void Start(string sessionName, SessionState initialState, ClrEventsMask events, bool profileSqlQueries, FilterType filterType, List<string> filters);

		bool CanStart();
	}
}
