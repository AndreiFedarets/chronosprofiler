using Chronos.DotNet.BasicProfiler;
using Chronos.Model;

namespace Chronos.DotNet.ExceptionMonitor
{
    internal sealed class ManagedExceptionCollection : UnitCollectionBase<ManagedExceptionInfo, ManagedExceptionNativeInfo>, IManagedExceptionCollection
    {
        private IClassCollection _classes;
        private IFunctionCollection _functions;

        public void SetDependencies(IClassCollection classes, IFunctionCollection functions)
        {
            _classes = classes;
            _functions = functions;
        }

        protected override ManagedExceptionInfo Convert(ManagedExceptionNativeInfo nativeUnit)
        {
            return new ManagedExceptionInfo(nativeUnit, _classes, _functions);
        }
    }
}
