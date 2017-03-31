using System;
using Chronos.Model;

namespace Chronos.DotNet.BasicProfiler
{
    [Serializable]
    public sealed class AssemblyInfo : UnitBase
    {
        private IAppDomainCollection _appDomains;

        public AssemblyInfo(AssemblyNativeInfo assemblyInfo, IAppDomainCollection appDomains)
            : base(assemblyInfo)
        {
            SetDependencies(appDomains);
        }

        private AssemblyNativeInfo AssemblyNativeInfo
        {
            get { return (AssemblyNativeInfo)NativeUnit; }
        }

        public AppDomainInfo AppDomain
        {
            get { return _appDomains[AssemblyNativeInfo.AppDomainId, BeginLifetime]; }
        }

        internal void SetDependencies(IAppDomainCollection appDomains)
        {
            _appDomains = appDomains;
        }
    }
}
