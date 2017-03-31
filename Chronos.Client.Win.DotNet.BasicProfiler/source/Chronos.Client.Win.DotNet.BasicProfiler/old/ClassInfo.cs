using Chronos.Client.Win.Model;

namespace Chronos.Client.Win.DotNet.BasicProfiler
{
    public sealed class ClassInfo : UnitBase
    {
        private readonly IModuleCollection _modules;
        private readonly IAssemblyCollection _assemblies;

        public ClassInfo(Daemon.DotNet.BasicProfiler.ClassInfo classInfo, IModuleCollection modules, IAssemblyCollection assemblies)
            : base(classInfo)
        {
            _modules = modules;
            _assemblies = assemblies;
        }

        private Daemon.DotNet.BasicProfiler.ClassInfo DaemonClassInfo
        {
            get { return ((Daemon.DotNet.BasicProfiler.ClassInfo)DaemonUnit); }
        }

        public uint TypeToken
        {
            get { return DaemonClassInfo.TypeToken; }
        }

        public ModuleInfo Module
        {
            get { return _modules[DaemonClassInfo.ModuleId, BeginLifetime]; }
        }

        public AssemblyInfo Assembly
        {
            get { return Module.Assembly; }
        }
    }
}
