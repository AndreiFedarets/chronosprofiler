namespace Chronos.Client.Win.DotNet.BasicProfiler
{
    public sealed class AssemblyInfo : Model.UnitBase
    {
        private readonly IAppDomainCollection _appDomainCollection;

        public AssemblyInfo(Daemon.DotNet.BasicProfiler.AssemblyInfo assemblyInfo, IAppDomainCollection appDomainCollection)
            : base(assemblyInfo)
        {
            _appDomainCollection = appDomainCollection;
        }

        private Daemon.DotNet.BasicProfiler.AssemblyInfo DaemonAssemblyInfo
        {
            get { return ((Daemon.DotNet.BasicProfiler.AssemblyInfo)DaemonUnit); }
        }

        public AppDomainInfo AppDomain
        {
            get { return _appDomainCollection[DaemonAssemblyInfo.AppDomainId, BeginLifetime]; }
        }
    }
}
