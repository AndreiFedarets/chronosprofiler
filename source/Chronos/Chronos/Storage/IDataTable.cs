using System.Collections.Generic;

namespace Chronos.Storage
{
    public interface IDataTable<T> : IEnumerable<T>
    {
        void AddOrUpdate(T item);

        void AddOrUpdate(T[] items);

        //List<T> Select(IEnumerable<object> ids);
    }
}
