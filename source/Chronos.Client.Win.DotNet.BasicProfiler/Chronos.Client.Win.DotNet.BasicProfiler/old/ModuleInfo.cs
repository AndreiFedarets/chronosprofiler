using Chronos.Client.Win.Model;

namespace Chronos.Client.Win.DotNet.BasicProfiler
{
    public sealed class ModuleInfo : UnitBase
    {
        private readonly IAssemblyCollection _assemblyCollection;

        public ModuleInfo(Daemon.DotNet.BasicProfiler.ModuleInfo moduleInfo, IAssemblyCollection assemblyCollection)
            : base(moduleInfo)
        {
            _assemblyCollection = assemblyCollection;
        }

        private Daemon.DotNet.BasicProfiler.ModuleInfo DaemonModuleInfo
        {
            get { return ((Daemon.DotNet.BasicProfiler.ModuleInfo)DaemonUnit); }
        }

        public AssemblyInfo Assembly
        {
            get { return _assemblyCollection[DaemonModuleInfo.AssemblyId, BeginLifetime]; }
        }
    }
}
