using Chronos.Model;
using Chronos.Proxy;

namespace Chronos.DotNet.BasicProfiler
{
    internal sealed class ClassCollection : UnitCollectionBase<ClassInfo, ClassNativeInfo>, IClassCollection
    {
        private IModuleCollection _modules;

        public void SetDependencies(IModuleCollection modules)
        {
            _modules = modules;
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

        protected override ClassInfo Convert(ClassNativeInfo nativeUnit)
        {
            return new ClassInfo(nativeUnit, _modules);
        }
    }
}
