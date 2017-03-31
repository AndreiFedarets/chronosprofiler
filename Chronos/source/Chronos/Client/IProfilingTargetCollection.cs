using System;
using System.Collections.Generic;

namespace Chronos.Client
{
    public interface IProfilingTargetCollection : IEnumerable<IProfilingTarget>
    {
        IProfilingTarget this[Guid uid] { get; }

        bool Contains(Guid uid);
    }
}
