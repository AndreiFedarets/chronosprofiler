using Chronos.Client.Win.ViewModels;
using Chronos.Client.Win.ViewModels.Common.StandaloneApplication;
using Chronos.Client.Win.ViewModels.Start;

namespace Chronos.Client.Win.Common.StandaloneApplication
{
    public class ProfilingTargetAdapter : IProfilingTargetAdapter
    {
        public ViewModel CreateConfigurationViewModel(StartPageViewModel pageViewModel)
        {
            IApplicationBase application = pageViewModel.Application;
            ConfigurationSettings settings = pageViewModel.ConfigurationSettings;
            IHostApplicationSelector selector = pageViewModel.HostApplicationSelector;
            ViewModel viewModel = new ProfilingTargetSettingsViewModel(application, settings, selector);
            return viewModel;
        }
    }
}
