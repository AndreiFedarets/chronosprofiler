using System.Collections.Generic;
using System.Collections.Specialized;

namespace Chronos.Client.Win.DotNet.BasicProfiler
{
    internal class UnitCollection<TClient, TUnit>  : INotifyCollectionChanged, IEnumerable<TClient>
        where TClient : UnitBase
        where TUnit : Daemon.DotNet.BasicProfiler.UnitBase
    {
        private readonly Daemon.DotNet.BasicProfiler.IUnitCollection<TUnit> _unitCollection;

        public UnitCollection(Daemon.DotNet.BasicProfiler.IUnitCollection<TUnit> unitCollection)
        {
            _unitCollection = unitCollection;
        }
    }
}
