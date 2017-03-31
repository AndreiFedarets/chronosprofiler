using System;
using System.Collections.Generic;
using Chronos.Core;

namespace Chronos.Extensibility
{
    public interface IProfilingStrategy
    {
        event Action Changed;

        string DisplayName { get; }

        ClrEventsMask EventsMask { get; }

        FilterType FilterType { get; }

        bool ProfileSqlQueries { get; }

        List<string> Filters { get; }

        object GetView();
    }
}
