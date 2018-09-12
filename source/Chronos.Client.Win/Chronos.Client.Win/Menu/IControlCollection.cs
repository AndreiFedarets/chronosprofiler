using System.Collections.Generic;
using System.Collections.Specialized;

namespace Chronos.Client.Win.Menu
{
    public interface IControlCollection : IControl, IEnumerable<IControl>, INotifyCollectionChanged
    {
        IControlCollection Add(IControl control);

        IControlCollection Remove(IControl control);

        IControl FindChild(string id);
    }
}
