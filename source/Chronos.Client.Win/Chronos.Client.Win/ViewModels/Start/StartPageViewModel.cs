using Layex;
using Layex.Contracts;
using Layex.ViewModels;

namespace Chronos.Client.Win.ViewModels.Start
{
    [ViewModel(Constants.ViewModels.Start)]
    public class StartPageViewModel : ItemsViewModel, IDialogViewModel
    {
        private bool _startProfilingImmediately;
        private bool _isReady;

        public StartPageViewModel(IMainApplication application, IProfilingTarget profilingTarget)
        {
            ProfilingTarget = profilingTarget;
            Application = application;
            ConfigurationSettings = new ConfigurationSettings(profilingTarget.Definition.Uid);
            HostApplicationSelector = new HostApplicationSelector(application.HostApplications);
            _startProfilingImmediately = true;
            _isReady = false;
        }

        public override string DisplayName
        {
            get { return "Create Configuration"; }
        }

        public bool CanCreateConfiguration
        {
            get { return _isReady; }
            private set
            {
                _isReady = value;
                NotifyOfPropertyChange(() => CanCreateConfiguration);
            }
        }

        public bool? DialogResult
        {
            get { return _isReady; }
        }

        public IMainApplication Application { get; private set; }

        public ConfigurationSettings ConfigurationSettings { get; private set; }

        public IHostApplicationSelector HostApplicationSelector { get; private set; }

        public IProfilingTarget ProfilingTarget { get; private set; }
        
        public bool StartProfilingImmediately
        {
            get { return _startProfilingImmediately; }
            set
            {
                _startProfilingImmediately = value;
                NotifyOfPropertyChange(() => StartProfilingImmediately);
            }
        }

        public void CreateConfiguration()
        {
            Host.IApplication application = HostApplicationSelector.SelectedApplication;
            IConfiguration configuration = application.Configurations.Create(ConfigurationSettings);
            if (StartProfilingImmediately)
            {
                configuration.Activate();
            }
            TryClose(true);
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();
            DependencyContainer.RegisterInstance(ConfigurationSettings);
            DependencyContainer.RegisterInstance(HostApplicationSelector);
            DependencyContainer.RegisterInstance(ProfilingTarget);
        }

        void IDialogContractConsumer.OnReadyChanged(bool ready)
        {
            CanCreateConfiguration = ready;
        }
    }
}
