using Adenium;
using Adenium.Layouting;
using Chronos.Client.Win.ViewModels.Start;

namespace Chronos.Client.Win.Common.ServiceApplication
{
    public class ProfilingTargetAdapter : IProfilingTargetAdapter, ILayoutProvider
    {
        void ILayoutProvider.ConfigureContainer(IViewModel targetViewModel, IContainer container)
        {
        }

        string ILayoutProvider.GetLayout(IViewModel targetViewModel)
        {
            StartPageViewModel startPageViewModel = targetViewModel as StartPageViewModel;
            if (startPageViewModel != null && startPageViewModel.ProfilingTarget.GetWinAdapter() == this)
            {
                return LayoutFileReader.ReadViewModelLayout(targetViewModel);
            }
            return string.Empty;
        }
    }
}
