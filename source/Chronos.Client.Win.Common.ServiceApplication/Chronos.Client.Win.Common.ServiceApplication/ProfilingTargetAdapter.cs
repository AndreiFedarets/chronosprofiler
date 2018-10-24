using Adenium;
using Chronos.Client.Win.ViewModels.Common.ServiceApplication;
using Chronos.Client.Win.ViewModels.Start;

namespace Chronos.Client.Win.Common.ServiceApplication
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
