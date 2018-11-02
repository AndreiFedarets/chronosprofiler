using Adenium;
using Adenium.Layouting;
using Chronos.Client.Win.DotNet.BasicProfiler.Properties;

namespace Chronos.Client.Win.DotNet.BasicProfiler.Menu
{
    internal sealed class ClassesMenuItem : MenuControlHandlerBase
    {
        public override string GetText()
        {
            return Resources.ClassesMenuItem_Text;
        }

        public override void OnAction()
        {
            IContainerViewModel viewModel = (IContainerViewModel)OwnerViewModel;
            viewModel.ActivateItem(Constants.ViewModels.ClassesViewModel);
        }
    }
}
