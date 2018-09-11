using Chronos.Client.Win.Model;

namespace Chronos.Client.Win.DotNet.BasicProfiler
{
    internal class AssemblyCollection : UnitCollectionBase<AssemblyInfo, Daemon.DotNet.BasicProfiler.AssemblyInfo>, IAssemblyCollection
    {
        private readonly IAppDomainCollection _appDomains;

        public AssemblyCollection(IAppDomainCollection appDomains)
        {
            _appDomains = appDomains;
        }

        protected override AssemblyInfo CreateClientUnit(Daemon.DotNet.BasicProfiler.AssemblyInfo daemonUnit)
        {
            return new AssemblyInfo(daemonUnit, _appDomains);
        }
    }
}
