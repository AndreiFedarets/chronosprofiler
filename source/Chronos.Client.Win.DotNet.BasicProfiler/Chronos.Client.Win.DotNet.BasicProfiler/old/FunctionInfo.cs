using Chronos.Client.Win.Model;

namespace Chronos.Client.Win.DotNet.BasicProfiler
{
    public sealed class FunctionInfo : UnitBase
    {
        private readonly IClassCollection _classCollection;

        public FunctionInfo(Daemon.DotNet.BasicProfiler.FunctionInfo functionInfo, IClassCollection classCollection)
            : base(functionInfo)
        {
            _classCollection = classCollection;
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

        private Daemon.DotNet.BasicProfiler.FunctionInfo DaemonFunctionInfo
        {
            get { return ((Daemon.DotNet.BasicProfiler.FunctionInfo)DaemonUnit); }
        }

        public ClassInfo Class
        {
            get
            {
                ClassInfo classInfo;
                if (DaemonFunctionInfo.ClassId == 0)
                {
                    classInfo = _classCollection.FindByTypeToken(DaemonFunctionInfo.ModuleId, DaemonFunctionInfo.TypeToken);
                }
                else
                {
                    classInfo = _classCollection[DaemonFunctionInfo.ClassId, BeginLifetime];
                }
                return classInfo;
            }
        }
    }
}
