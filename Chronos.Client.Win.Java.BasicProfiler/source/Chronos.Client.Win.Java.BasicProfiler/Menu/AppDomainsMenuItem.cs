using Chronos.Client.Win.Java.BasicProfiler.Properties;
using Chronos.Client.Win.Menu.Specialized;
using Chronos.Client.Win.Models;
using Chronos.Client.Win.Models.Java.BasicProfiler;
using Chronos.Client.Win.ViewModels.Profiling;
using Chronos.Java.BasicProfiler;

namespace Chronos.Client.Win.Menu.Java.BasicProfiler
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
            //IAppDomainCollection collection = ProfilingViewModel.Application.ServiceContainer.Resolve<IAppDomainCollection>();
            //AppDomainsModel model = new AppDomainsModel(collection);
            //return model;
            return null;
        }
    }
}
