namespace Chronos.Core
{
	public sealed class AppDomainCollection : UnitCollection<AppDomainInfo>, IAppDomainCollection
	{
        public override uint UnitType
		{
            get { return (uint)Core.UnitType.AppDomain; }
		}
	}
}
