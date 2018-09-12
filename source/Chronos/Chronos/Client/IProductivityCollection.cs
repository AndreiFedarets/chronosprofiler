using System;
using System.Collections.Generic;

namespace Chronos.Client
{
    public interface IProductivityCollection : IEnumerable<IProductivity>
    {
        IProductivity this[Guid uid] { get; }

        bool Contains(Guid uid);
    }
}
