using Chronos.Client.Win.ViewModels;
using Chronos.Client.Win.ViewModels.Common.IISApplication;
using Chronos.Client.Win.ViewModels.Start;

namespace Chronos.Client.Win.Common.IISApplication
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
