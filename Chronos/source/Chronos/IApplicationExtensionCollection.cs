using System;
using System.Collections.Generic;

namespace Chronos
{
    public interface IApplicationExtensionCollection : IEnumerable<IApplicationExtension>
    {
        IApplicationExtension this[Guid uid] { get; }

        bool Contains(Guid uid);

        bool TryGet(Guid uid, out IApplicationExtension item);
    }
}
