using System;
using Chronos.Common;

namespace Chronos.DotNet.BasicProfiler
{
    [Serializable]
    public sealed class FunctionInfo : UnitBase
    {
        private IClassCollection _classes;

        public FunctionInfo(FunctionNativeInfo functionInfo, IClassCollection classes)
            : base(functionInfo)
        {
            SetDependencies(classes);
        }

        public string FullName
        {
            get
            {
                ClassInfo classInfo = Class;
                if (classInfo == null)
                {
                    return Name;
                }
                return string.Concat(classInfo.Name, ".", Name);
            }
        }

        private FunctionNativeInfo FunctionNativeInfo
        {
            get { return (FunctionNativeInfo)NativeUnit; }
        }

        public ClassInfo Class
        {
            get
            {
                ClassInfo classInfo;
                if (FunctionNativeInfo.ClassId == 0)
                {
                    classInfo = _classes.FindByTypeToken(FunctionNativeInfo.ModuleId, FunctionNativeInfo.TypeToken);
                }
                else
                {
                    classInfo = _classes[FunctionNativeInfo.ClassId, BeginLifetime];
                }
                return classInfo;
            }
        }

        internal void SetDependencies(IClassCollection classes)
        {
            _classes = classes;
        }
    }
}
