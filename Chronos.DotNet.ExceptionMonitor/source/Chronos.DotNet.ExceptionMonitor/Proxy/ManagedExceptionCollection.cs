using Chronos.DotNet.BasicProfiler;
using Chronos.Model;
using Chronos.Proxy;
using Chronos.Proxy.Model;

namespace Chronos.DotNet.ExceptionMonitor.Proxy
{
    internal sealed class ManagedExceptionCollection : UnitCollectionProxyBase<ManagedExceptionInfo>, IManagedExceptionCollection
    {
        private IClassCollection _classes;
        private IFunctionCollection _functions;

        public ManagedExceptionCollection(IUnitCollection<ManagedExceptionInfo> remoteObject)
            : base(remoteObject)
        {
        }

        [ServiceProxyInitializationMethod]
        public void SetDependencies(IClassCollection classes, IFunctionCollection functions)
        {
            _classes = classes;
            _functions = functions;
            foreach (ManagedExceptionInfo unit in this)
            {
                unit.SetDependencies(_classes, _functions);
            }
        }

        protected override ManagedExceptionInfo Convert(ManagedExceptionInfo unit)
        {
            unit.SetDependencies(_classes, _functions);
            return unit;
        }
    }
}
