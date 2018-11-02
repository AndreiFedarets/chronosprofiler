using Adenium;
using Adenium.Layouting;
using Chronos.Client.Win.DotNet.SqlProfiler.Properties;

namespace Chronos.Client.Win.DotNet.SqlProfiler.Menu
{
    internal sealed class SqlQueriesMenuItem : MenuControlHandlerBase
    {
        public override string GetText()
        {
            return Resources.SqlQueriesMenuItem_Text;
        }

        public override void OnAction()
        {
            IContainerViewModel viewModel = (IContainerViewModel) OwnerViewModel;
            viewModel.ActivateItem(Constants.ViewModels.SqlQueriesViewModel);
        }
    }
}
