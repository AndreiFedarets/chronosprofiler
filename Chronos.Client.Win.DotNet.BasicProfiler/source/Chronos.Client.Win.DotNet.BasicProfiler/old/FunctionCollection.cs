using Chronos.Client.Win.Model;

namespace Chronos.Client.Win.DotNet.BasicProfiler
{
    internal class FunctionCollection : UnitCollectionBase<FunctionInfo, Daemon.DotNet.BasicProfiler.FunctionInfo>, IFunctionCollection
    {
        private readonly IClassCollection _classes;

        public FunctionCollection(IClassCollection classes)
        {
            _classes = classes;
        }

        protected override FunctionInfo CreateClientUnit(Daemon.DotNet.BasicProfiler.FunctionInfo daemonUnit)
        {
            return new FunctionInfo(daemonUnit, _classes);
        }
    }
}
