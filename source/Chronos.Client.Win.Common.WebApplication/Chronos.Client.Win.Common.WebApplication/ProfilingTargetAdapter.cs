using Adenium;
using Chronos.Client.Win.ViewModels.Common.WebApplication;
using Chronos.Client.Win.ViewModels.Start;

namespace Chronos.Client.Win.Common.WebApplication
{
    public class ProfilingTargetAdapter : IProfilingTargetAdapter
    {
        public IViewModel CreateConfigurationViewModel(IContainerViewModel pageViewModel)
        {
            StartPageViewModel startPageViewModel = (StartPageViewModel) pageViewModel;
            ConfigurationSettings settings = startPageViewModel.ConfigurationSettings;
            IHostApplicationSelector selector = startPageViewModel.HostApplicationSelector;
            ViewModel viewModel = new ProfilingTargetSettingsViewModel(settings, selector);
            return viewModel;
        }
    }
}
