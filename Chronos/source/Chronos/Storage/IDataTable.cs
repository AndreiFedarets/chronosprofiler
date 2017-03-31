using System.Collections.Generic;

namespace Chronos.Storage
{
    public interface IDataTable<T> : IEnumerable<T>
    {
        void Add(T item);

        void Add(IEnumerable<T> items);

        List<T> Select(IEnumerable<object> ids);
    }
}
