using System.Collections.Generic;
using System.Collections.Specialized;

namespace Chronos.Client.Win.Menu
{
    public interface IMenuCollection : IEnumerable<IMenu>, INotifyCollectionChanged
    {
        IMenu this[string id] { get; }

        void Add(IMenu menu);
    }
}
