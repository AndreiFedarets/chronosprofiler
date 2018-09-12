using System;
using System.Collections.Generic;

namespace Chronos.Client
{
    public interface IProfilingTypeCollection : IEnumerable<IProfilingType>
    {
        IProfilingType this[Guid uid] { get; }

        bool Contains(Guid uid);
    }
}
