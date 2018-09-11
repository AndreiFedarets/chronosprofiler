using Chronos.Java.BasicProfiler;
using Chronos.Model;

namespace Chronos.Proxy.Model.Java.BasicProfiler
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
