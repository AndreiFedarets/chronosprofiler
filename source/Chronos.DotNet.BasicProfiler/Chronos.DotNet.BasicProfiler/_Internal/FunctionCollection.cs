using Chronos.Common;

namespace Chronos.DotNet.BasicProfiler
{
    internal sealed class FunctionCollection : UnitCollectionBase<FunctionInfo, FunctionNativeInfo>, IFunctionCollection
    {
        private IClassCollection _classes;

        public void SetDependencies(IClassCollection classes)
        {
            _classes = classes;
        }

        protected override FunctionInfo Convert(FunctionNativeInfo nativeUnit)
        {
            return new FunctionInfo(nativeUnit, _classes);
        }
    }
}
