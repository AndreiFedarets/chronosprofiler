using Chronos.Client.Win.Model;

namespace Chronos.Client.Win.DotNet.BasicProfiler
{
    internal class AppDomainCollection : UnitCollectionBase<AppDomainInfo, Daemon.DotNet.BasicProfiler.AppDomainInfo>, IAppDomainCollection
    {
        protected override AppDomainInfo CreateClientUnit(Daemon.DotNet.BasicProfiler.AppDomainInfo daemonUnit)
        {
            return new AppDomainInfo(daemonUnit);
        }
    }
}
