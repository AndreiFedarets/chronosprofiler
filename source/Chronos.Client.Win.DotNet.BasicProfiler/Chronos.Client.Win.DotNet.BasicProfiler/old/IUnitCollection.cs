using System.Collections.Generic;

namespace Chronos.Client.Win.DotNet.BasicProfiler
{
    public interface IUnitCollection<T> : IEnumerable<T> where T : Model.UnitBase
    {
        T this[ulong uid] { get; }

        T this[ulong id, ulong lifetime] { get; }

        int Count { get; }
    }
}
