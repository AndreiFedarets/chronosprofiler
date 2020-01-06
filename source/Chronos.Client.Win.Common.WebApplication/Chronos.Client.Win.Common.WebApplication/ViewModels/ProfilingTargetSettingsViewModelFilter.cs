using Chronos.Client.Win.ViewModels.Start;
using Layex.Layouts;
using Layex.ViewModels;

namespace Chronos.Client.Win.Common.WebApplication.ViewModels
{
    internal sealed class ProfilingTargetSettingsViewModelFilter : ILayoutItemFilter
    {
        public bool CanDisplayItem(IItemsViewModel viewModel)
        {
            StartPageViewModel startPageViewModel = viewModel as StartPageViewModel;
            return startPageViewModel != null && startPageViewModel.ProfilingTarget.GetWinAdapter() is ProfilingTargetAdapter;
        }
    }
}
