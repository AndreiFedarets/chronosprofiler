using Adenium;
using Adenium.Layouting;
using Chronos.Client.Win.ViewModels.Start;

namespace Chronos.Client.Win.Common.DesktopApplication
{
    public class ProfilingTargetAdapter : IProfilingTargetAdapter, ILayoutProvider
    {
        void ILayoutProvider.ConfigureContainer(IContainer container)
        {
        }

        string ILayoutProvider.GetLayout(IViewModel viewModel)
        {
            StartPageViewModel startPageViewModel = viewModel as StartPageViewModel;
            if (startPageViewModel != null && startPageViewModel.ProfilingTarget.GetWinAdapter() == this)
            {
                return LayoutFileReader.ReadViewModelLayout(viewModel);
            }
            return string.Empty;
        }
    }
}
