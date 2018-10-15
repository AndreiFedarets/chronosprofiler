
namespace Chronos.Client.Win.ViewModels.Start
{
    [Contracts.EnableContract(typeof(Contracts.Dialog.Contract))]
    public class StartPageViewModel : GridViewModel, Contracts.Dialog.IContractConsumer
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
            set { }
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

        protected override void BuildLayout()
        {
            base.BuildLayout();
            TryAdd(new HostApplicationSelectViewModel(HostApplicationSelector));
            TryAdd(new PlaceholderViewModel(new ProfilingTargetContent(ProfilingTarget, this, HostApplicationSelector)));
            TryAdd(new ProfilingTypesViewModel(Application, ConfigurationSettings));
        }

        public override void Dispose()
        {
            HostApplicationSelector.TryDispose();
            base.Dispose();
        }

        void Contracts.Dialog.IContractConsumer.OnReadyChanged(bool ready)
        {
            CanCreateConfiguration = ready;
        }
    }
}
