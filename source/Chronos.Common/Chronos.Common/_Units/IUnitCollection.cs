using System;
using System.Collections;
using System.Collections.Generic;

namespace Chronos.Common
{
    public interface IUnitCollection : IEnumerable
    {
        int Count { get; }
    }

    public interface IUnitCollection<T> : IEnumerable<T>, IUnitCollection where T : UnitBase
    {
        T this[uint uid] { get; }

        T this[ulong id, ulong lifetime] { get; }

        event EventHandler<UnitCollectionEventArgs<T>> UnitsUpdated;
    }
}
