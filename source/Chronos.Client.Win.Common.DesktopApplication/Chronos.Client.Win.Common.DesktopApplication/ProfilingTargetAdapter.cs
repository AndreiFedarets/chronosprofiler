using Adenium;
using Chronos.Client.Win.ViewModels.Common.DesktopApplication;
using Chronos.Client.Win.ViewModels.Start;

namespace Chronos.Client.Win.Common.DesktopApplication
{
    public class ProfilingTargetAdapter : IProfilingTargetAdapter
    {
        public IViewModel CreateConfigurationViewModel(IContainerViewModel pageViewModel)
        {
            StartPageViewModel startPageViewModel = (StartPageViewModel) pageViewModel;
            IApplicationBase application = startPageViewModel.Application;
            ConfigurationSettings settings = startPageViewModel.ConfigurationSettings;
            IHostApplicationSelector selector = startPageViewModel.HostApplicationSelector;
            ViewModel viewModel = new ProfilingTargetSettingsViewModel(application, settings, selector);
            return viewModel;
        }
    }
}
