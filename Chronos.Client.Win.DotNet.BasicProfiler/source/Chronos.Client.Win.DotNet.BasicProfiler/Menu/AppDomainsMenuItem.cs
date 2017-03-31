using Chronos.Client.Win.DotNet.BasicProfiler.Properties;
using Chronos.Client.Win.Menu.Specialized;
using Chronos.Client.Win.Models;
using Chronos.Client.Win.Models.DotNet.BasicProfiler;
using Chronos.Client.Win.ViewModels.Profiling;
using Chronos.DotNet.BasicProfiler;

namespace Chronos.Client.Win.Menu.DotNet.BasicProfiler
{
    internal sealed class AppDomainsMenuItem : UnitsMenuItemBase
    {
        public AppDomainsMenuItem(ProfilingViewModel profilingViewModel)
            : base(profilingViewModel)
        {
        }

        public override string Text
        {
            get { return Resources.AppDomainsMenuItem_Text; }
            protected set { }
        }

        protected override IUnitsModel GetModel()
        {
            IAppDomainCollection collection = ProfilingViewModel.Application.ServiceContainer.Resolve<IAppDomainCollection>();
            AppDomainsModel model = new AppDomainsModel(collection);
            return model;
        }
    }
}
