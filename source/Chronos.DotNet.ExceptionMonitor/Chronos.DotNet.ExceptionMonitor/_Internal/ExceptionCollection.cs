using Chronos.DotNet.BasicProfiler;
using Chronos.Model;

namespace Chronos.DotNet.ExceptionMonitor
{
    internal sealed class ExceptionCollection : UnitCollectionBase<ExceptionInfo, ExceptionNativeInfo>, IExceptionCollection
    {
        private IClassCollection _classes;
        private IFunctionCollection _functions;

        public void SetDependencies(IClassCollection classes, IFunctionCollection functions)
        {
            _classes = classes;
            _functions = functions;
        }

        protected override ExceptionInfo Convert(ExceptionNativeInfo nativeUnit)
        {
            return new ExceptionInfo(nativeUnit, _classes, _functions);
        }
    }
}
