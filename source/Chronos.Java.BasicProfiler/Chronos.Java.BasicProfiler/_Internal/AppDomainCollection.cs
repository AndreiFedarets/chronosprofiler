using Chronos.Model;

namespace Chronos.Java.BasicProfiler
{
    internal sealed class AppDomainCollection : UnitCollectionBase<AppDomainInfo, AppDomainNativeInfo>, IAppDomainCollection
    {
        protected override AppDomainInfo Convert(AppDomainNativeInfo nativeUnit)
        {
            return new AppDomainInfo(nativeUnit);
        }
    }
}
