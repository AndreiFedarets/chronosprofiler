using Adenium;
using Adenium.Layouting;
using Chronos.Client.Win.Common.EventsTree.Properties;

namespace Chronos.Client.Win.Common.EventsTree.Menu
{
    internal sealed class TimelineMenuItem : MenuControlHandlerBase
    {
        public override string GetText()
        {
            return Resources.TimelineMenuItem_Text;
        }

        public override void OnAction()
        {
            IContainerViewModel viewModel = (IContainerViewModel)OwnerViewModel;
            viewModel.ActivateItem(Constants.ViewModels.TimelineViewModel);
            base.OnAction();
        }
    }
}
