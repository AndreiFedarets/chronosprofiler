using Chronos.Client.Win.DotNet.BasicProfiler.Properties;
using Chronos.Client.Win.Menu.Specialized;
using Chronos.Client.Win.Models;
using Chronos.Client.Win.Models.DotNet.BasicProfiler;
using Chronos.DotNet.BasicProfiler;

namespace Chronos.Client.Win.Menu.DotNet.BasicProfiler
{
    internal sealed class AppDomainsMenuItem : UnitsMenuItemBase
    {
        public AppDomainsMenuItem(IProfilingApplication application)
            : base(application)
        {
        }

        public override string Text
        {
            get { return Resources.AppDomainsMenuItem_Text; }
            protected set { }
        }

        protected override IUnitsModel GetModel()
        {
            IAppDomainCollection collection = Application.ServiceContainer.Resolve<IAppDomainCollection>();
            AppDomainsModel model = new AppDomainsModel(collection);
            return model;
        }
    }
}
