namespace Chronos.Client.Win.DotNet.BasicProfiler
{
    public sealed class AppDomainInfo : Model.UnitBase
    {
        public AppDomainInfo(Daemon.DotNet.BasicProfiler.AppDomainInfo appDomainInfo)
            : base(appDomainInfo)
        {
        }

        private Daemon.DotNet.BasicProfiler.AppDomainInfo DaemonAppDomainInfo
        {
            get { return ((Daemon.DotNet.BasicProfiler.AppDomainInfo)DaemonUnit); }
        }
    }
}
