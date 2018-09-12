using System;
using System.Collections.Generic;

namespace Chronos
{
    public interface IProfilingTypeCollection : IEnumerable<IProfilingType>
    {
        IProfilingType this[Guid uid] { get; }

        bool Contains(Guid uid);

        bool TryGet(Guid uid, out IProfilingType item);
    }
}
