using Chronos.Client.Win.Model;

namespace Chronos.Client.Win.DotNet.BasicProfiler
{
    internal class ClassCollection : UnitCollectionBase<ClassInfo, Daemon.DotNet.BasicProfiler.ClassInfo>, IClassCollection
    {
        private readonly IModuleCollection _modules;
        private readonly IAssemblyCollection _assemblies;

        public ClassCollection(IModuleCollection modules, IAssemblyCollection assemblies)
        {
            _modules = modules;
            _assemblies = assemblies;
        }

        public ClassInfo FindByTypeToken(ulong moduleId, uint typeToken)
        {
            foreach (ClassInfo classInfo in this)
            {
                if (classInfo.TypeToken == typeToken && classInfo.Module.Id == moduleId)
                {
                    return classInfo;
                }
            }
            return null;
        }

        protected override ClassInfo CreateClientUnit(Daemon.DotNet.BasicProfiler.ClassInfo daemonUnit)
        {
            return new ClassInfo(daemonUnit, _modules, _assemblies);
        }
    }
}
