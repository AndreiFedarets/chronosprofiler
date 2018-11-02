using Chronos.Common;
using Chronos.Common.Proxy;
using Chronos.Proxy;

namespace Chronos.DotNet.BasicProfiler.Proxy
{
    internal sealed class ClassCollection : UnitCollectionProxyBase<ClassInfo>, IClassCollection
    {
        private IModuleCollection _modules;

        public ClassCollection(IUnitCollection<ClassInfo> remoteObject)
            : base(remoteObject)
        {
        }

        [ServiceProxyInitializationMethod]
        public void SetDependencies(IModuleCollection modules)
        {
            _modules = modules;
            foreach (ClassInfo unit in this)
            {
                unit.SetDependencies(_modules);
            }
        }

        protected override ClassInfo Convert(ClassInfo unit)
        {
            unit.SetDependencies(_modules);
            return unit;
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
    }
}
