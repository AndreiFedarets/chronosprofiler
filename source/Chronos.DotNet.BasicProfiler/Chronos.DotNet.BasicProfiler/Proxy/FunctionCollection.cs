using Chronos.Common;
using Chronos.Common.Proxy;
using Chronos.Proxy;

namespace Chronos.DotNet.BasicProfiler.Proxy
{
    internal sealed class FunctionCollection : UnitCollectionProxyBase<FunctionInfo>, IFunctionCollection
    {
        private IClassCollection _classes;

        public FunctionCollection(IUnitCollection<FunctionInfo> remoteObject)
            : base(remoteObject)
        {
        }

        [ServiceProxyInitializationMethod]
        public void SetDependencies(IClassCollection classes)
        {
            _classes = classes;
            foreach (FunctionInfo unit in this)
            {
                unit.SetDependencies(_classes);
            }
        }

        protected override FunctionInfo Convert(FunctionInfo unit)
        {
            unit.SetDependencies(_classes);
            return unit;
        }
    }
}
