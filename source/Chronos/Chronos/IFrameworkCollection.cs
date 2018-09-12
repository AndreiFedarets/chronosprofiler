using System;
using System.Collections.Generic;

namespace Chronos
{
    public interface IFrameworkCollection : IEnumerable<IFramework>
    {
        IFramework this[Guid uid] { get; }

        bool Contains(Guid uid);

        bool TryGet(Guid uid, out IFramework item);
    }
}
