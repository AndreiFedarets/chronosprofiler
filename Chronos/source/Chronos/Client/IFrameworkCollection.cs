using System;
using System.Collections.Generic;

namespace Chronos.Client
{
    public interface IFrameworkCollection : IEnumerable<IFramework>
    {
        IFramework this[Guid uid] { get; }

        bool Contains(Guid uid);
    }
}
