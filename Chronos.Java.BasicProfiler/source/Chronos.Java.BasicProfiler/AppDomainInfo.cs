using System;
using Chronos.Model;

namespace Chronos.Java.BasicProfiler
{
    [Serializable]
    public sealed class AppDomainInfo : UnitBase
    {
        public AppDomainInfo(AppDomainNativeInfo appDomainInfo)
            : base(appDomainInfo)
        {
        }

        private AppDomainNativeInfo AppDomainNativeInfo
        {
            get { return (AppDomainNativeInfo)NativeUnit; }
        }

        internal void SetDependencies()
        {
            
        }
    }
}
