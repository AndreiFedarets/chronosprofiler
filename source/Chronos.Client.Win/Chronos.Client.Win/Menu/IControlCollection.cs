using System.Collections.Generic;
using System.Collections.Specialized;

namespace Chronos.Client.Win.Menu
{
    public interface IControlCollection : IControl, IEnumerable<IControl>, INotifyCollectionChanged
    {
        IControl this[string id] { get; }

        IControlCollection Add(IControl control);

        IControlCollection Remove(IControl control);

        IControl FindChild(string id);
    }
}
