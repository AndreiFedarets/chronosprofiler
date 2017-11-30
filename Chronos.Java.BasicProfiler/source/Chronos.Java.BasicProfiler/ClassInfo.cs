using System;
using Chronos.Model;

namespace Chronos.Java.BasicProfiler
{
    [Serializable]
    public sealed class ClassInfo : UnitBase
    {
        private IModuleCollection _modules;

        public ClassInfo(ClassNativeInfo classInfo, IModuleCollection modules)
            : base(classInfo)
        {
            SetDependencies(modules);
        }

        public string FullName
        {
            get { return string.Concat(Name, ".", Name); }
        }

        public string Namespace
        {
            get { return ClassNativeInfo.Namespace; }
        }

        private ClassNativeInfo ClassNativeInfo
        {
            get { return (ClassNativeInfo)NativeUnit; }
        }

        public uint TypeToken
        {
            get { return ClassNativeInfo.TypeToken; }
        }

        public ModuleInfo Module
        {
            get { return _modules[ClassNativeInfo.ModuleId, BeginLifetime]; }
        }

        public AssemblyInfo Assembly
        {
            get { return Module.Assembly; }
        }

        internal void SetDependencies(IModuleCollection modules)
        {
            _modules = modules;
        }
    }
}
