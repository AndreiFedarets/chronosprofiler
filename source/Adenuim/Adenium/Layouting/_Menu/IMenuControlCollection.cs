using System.Collections.Generic;

namespace Adenium.Layouting
{
    public interface IMenuControlCollection : IMenuControl, IEnumerable<IMenuControl>
    {
        IMenuControl this[string id] { get; }
    }
}
