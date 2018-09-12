using System.Collections.Generic;

namespace Chronos.Model
{
    public interface IUnitCollectionStorage : IEnumerable<IUnitCollection>
    {
        void Register();
    }
}
