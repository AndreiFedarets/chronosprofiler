using Chronos.Client.Win.DotNet.BasicProfiler.Properties;
using Chronos.Client.Win.Menu.Specialized;
using Chronos.Client.Win.Models;
using Chronos.Client.Win.Models.DotNet.BasicProfiler;
using Chronos.DotNet.BasicProfiler;

namespace Chronos.Client.Win.DotNet.BasicProfiler.Menu
{
    internal sealed class AppDomainsMenuItem : UnitsMenuItemBase
    {
        public AppDomainsMenuItem(IProfilingApplication application)
            : base(application)
        {
        }

        public override string GetText()
        {
            return Resources.AppDomainsMenuItem_Text;
        }

        protected override IUnitsListModel GetModel()
        {
            IAppDomainCollection collection = Application.ServiceContainer.Resolve<IAppDomainCollection>();
            AppDomainsModel model = new AppDomainsModel(collection);
            return model;
        }
    }
}
