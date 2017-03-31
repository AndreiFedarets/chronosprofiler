using Chronos.Core;

namespace Chronos.Daemon.Proxy
{
	internal class AppDomainCollection : UnitCollection<AppDomainInfo>, IAppDomainCollection
	{
		public AppDomainCollection(IProcessShadow processShadow, IUnitCollection<AppDomainInfo> collection)
            : base(processShadow, collection, (uint)Core.UnitType.AppDomain)
		{
		}
	}
}
