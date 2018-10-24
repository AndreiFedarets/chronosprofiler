
using Adenium;

namespace Chronos.Client.Win.ViewModels.Start
{
    [EnableContract(typeof(DialogContract))]
    public class StartPageViewModel : GridViewModel, IDialogContractConsumer
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

        protected override void OnInitialize()
        {
            base.OnInitialize();
            ActivateItem(new HostApplicationSelectViewModel(HostApplicationSelector));
            ActivateItem(new PlaceholderViewModel(new ProfilingTargetContent(ProfilingTarget, this, HostApplicationSelector)));
            ActivateItem(new ProfilingTypesViewModel(Application, ConfigurationSettings));
        }

        void IDialogContractConsumer.OnReadyChanged(bool ready)
        {
            CanCreateConfiguration = ready;
        }
    }
}
