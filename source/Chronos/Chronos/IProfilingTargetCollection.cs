using System;
using System.Collections.Generic;

namespace Chronos
{
    public interface IProfilingTargetCollection : IEnumerable<IProfilingTarget>
    {
        IProfilingTarget this[Guid uid] { get; }

        bool Contains(Guid uid);

        bool TryGet(Guid uid, out IProfilingTarget item);
    }
}
