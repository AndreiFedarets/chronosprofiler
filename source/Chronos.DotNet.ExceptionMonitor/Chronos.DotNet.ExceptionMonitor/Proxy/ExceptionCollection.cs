using Chronos.DotNet.BasicProfiler;
using Chronos.Model;
using Chronos.Proxy;
using Chronos.Proxy.Model;

namespace Chronos.DotNet.ExceptionMonitor.Proxy
{
    internal sealed class ExceptionCollection : UnitCollectionProxyBase<ExceptionInfo>, IExceptionCollection
    {
        private IClassCollection _classes;
        private IFunctionCollection _functions;

        public ExceptionCollection(IUnitCollection<ExceptionInfo> remoteObject)
            : base(remoteObject)
        {
        }

        [ServiceProxyInitializationMethod]
        public void SetDependencies(IClassCollection classes, IFunctionCollection functions)
        {
            _classes = classes;
            _functions = functions;
            foreach (ExceptionInfo unit in this)
            {
                unit.SetDependencies(_classes, _functions);
            }
        }

        protected override ExceptionInfo Convert(ExceptionInfo unit)
        {
            unit.SetDependencies(_classes, _functions);
            return unit;
        }
    }
}
