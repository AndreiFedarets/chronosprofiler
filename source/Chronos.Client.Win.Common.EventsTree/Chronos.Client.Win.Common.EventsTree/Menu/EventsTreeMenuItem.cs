using Adenium;
using Adenium.Layouting;
using Chronos.Client.Win.Common.EventsTree.Properties;

namespace Chronos.Client.Win.Common.EventsTree.Menu
{
    internal sealed class EventsTreeMenuItem : MenuControlHandlerBase
    {
        public override string GetText()
        {
            return Resources.EventsTreeMenuItem_Text;
        }

        public override void OnAction()
        {
            IContainerViewModel viewModel = (IContainerViewModel) OwnerViewModel;
            viewModel.ActivateItem(Constants.ViewModels.EventsTreeViewModel);
        }
    }
}
