using System;
using Chronos.Model;

namespace Chronos.DotNet.BasicProfiler
{
    [Serializable]
    public sealed class ModuleInfo : UnitBase
    {
        private IAssemblyCollection _assemblies;

        public ModuleInfo(ModuleNativeInfo moduleInfo, IAssemblyCollection assemblies)
            : base(moduleInfo)
        {
            SetDependencies(assemblies);
        }

        private ModuleNativeInfo ModuleNativeInfo
        {
            get { return (ModuleNativeInfo)NativeUnit; }
        }

        public AssemblyInfo Assembly
        {
            get { return _assemblies[ModuleNativeInfo.AssemblyId, BeginLifetime]; }
        }

        internal void SetDependencies(IAssemblyCollection assemblies)
        {
            _assemblies = assemblies;
        }
    }
}
