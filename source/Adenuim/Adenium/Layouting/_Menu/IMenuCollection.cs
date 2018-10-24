using System.Collections.Generic;

namespace Adenium.Layouting
{
    public interface IMenuCollection : IEnumerable<IMenu>
    {
        IMenu this[string id] { get; }
    }
}
