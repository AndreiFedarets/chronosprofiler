using Adenium;
using Adenium.Layouting;
using Chronos.Client.Win.DotNet.BasicProfiler.Properties;

namespace Chronos.Client.Win.DotNet.BasicProfiler.Menu
{
    internal sealed class AppDomainsMenuItem : MenuControlHandlerBase
    {
        public override string GetText()
        {
            return Resources.AppDomainsMenuItem_Text;
        }

        public override void OnAction()
        {
            IContainerViewModel viewModel = (IContainerViewModel)OwnerViewModel;
            viewModel.ActivateItem(Constants.ViewModels.AppDomainsViewModel);
        }
    }
}
