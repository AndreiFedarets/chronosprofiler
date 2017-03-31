using Chronos.Core;

namespace Chronos.DotNet.BasicProfiler.Client.Win
{
    public partial class AppDomainsWidget
    {
        public UnitCollectionModel<AppDomainInfo> Model { get; private set; }

        public ISession Session
        {
            get { return Context.Session; }
        }

        protected override void Initialize()
        {
            base.Initialize();
            IAppDomainCollection unitCollection = Session.Daemon.Container.Resolve<IAppDomainCollection>();
            Model = new UnitCollectionModel<AppDomainInfo>(unitCollection);
        }
    }
}
