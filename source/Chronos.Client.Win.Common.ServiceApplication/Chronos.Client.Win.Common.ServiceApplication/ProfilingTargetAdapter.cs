using Chronos.Client.Win.ViewModels;
using Chronos.Client.Win.ViewModels.Common.ServiceApplication;
using Chronos.Client.Win.ViewModels.Start;

namespace Chronos.Client.Win.Common.ServiceApplication
{
    public class ProfilingTargetAdapter : IProfilingTargetAdapter
    {
        public ViewModel CreateConfigurationViewModel(StartPageViewModel pageViewModel)
        {
            ConfigurationSettings settings = pageViewModel.ConfigurationSettings;
            IHostApplicationSelector selector = pageViewModel.HostApplicationSelector;
            ViewModel viewModel = new ProfilingTargetSettingsViewModel(settings, selector);
            return viewModel;
        }
    }
}
