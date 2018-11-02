using Adenium;
using Adenium.Layouting;
using Chronos.Client.Win.DotNet.ExceptionMonitor.Properties;

namespace Chronos.Client.Win.DotNet.ExceptionMonitor.Menu
{
    internal sealed class ExceptionsMenuItem : MenuControlHandlerBase
    {
        public override string GetText()
        {
            return Resources.ExceptionsMenuItem_Text;
        }

        public override void OnAction()
        {
            IContainerViewModel viewModel = (IContainerViewModel)OwnerViewModel;
            viewModel.ActivateItem(Constants.ViewModels.ExceptionsViewModel);
        }
    }
}
