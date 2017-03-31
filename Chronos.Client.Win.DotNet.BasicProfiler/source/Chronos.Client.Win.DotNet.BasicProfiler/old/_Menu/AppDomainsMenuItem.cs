using Chronos.Client.Win.DotNet.BasicProfiler.Models;
using Chronos.Client.Win.Model;

namespace Chronos.Client.Win.DotNet.BasicProfiler
{
    internal class AppDomainsMenuItem : UnitsMenuAdapter
    {
        public AppDomainsMenuItem(ISession session)
            : base(session)
        {
        }

        protected override IUnitsModel CreateModel(ISession session)
        {
            IAppDomainCollection units = session.ServiceContainer.Resolve<IAppDomainCollection>();
            return new AppDomainsModel(units);
        }
    }
}
