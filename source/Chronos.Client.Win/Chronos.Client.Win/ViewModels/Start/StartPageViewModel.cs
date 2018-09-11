namespace Chronos.Client.Win.ViewModels.Start
{
    public class StartPageViewModel : GridViewModel, Contracts.Dialog.IContractConsumer
    {
        private bool _startProfilingImmediately;

        public StartPageViewModel(IMainApplication application, IProfilingTarget profilingTarget)
        {
            ProfilingTarget = profilingTarget;
            Application = application;
            ConfigurationSettings = new ConfigurationSettings(profilingTarget.Definition.Uid);
            HostApplicationSelector = new HostApplicationSelector(application.HostApplications);
            _startProfilingImmediately = true;
        }

        public override string DisplayName
        {
            get { return "Create Configuration"; }
            set { }
        }

        public IMainApplication Application { get; private set; }

        public ConfigurationSettings ConfigurationSettings { get; private set; }

        public IHostApplicationSelector HostApplicationSelector { get; private set; }

        public IProfilingTarget ProfilingTarget { get; private set; }
        
        //public IEnumerable<Host.IApplication> HostApplications
        //{
        //    get { return ProfilingTarget.SupportedApplications; }
        //}

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
            Add(new HostApplicationSelectViewModel(HostApplicationSelector));
            Add(new PlaceholderViewModel(new ProfilingTargetContent(ProfilingTarget, this)));
            Add(new ProfilingTypesViewModel(Application, ConfigurationSettings));
        }

        public override void Dispose()
        {
            HostApplicationSelector.TryDispose();
            base.Dispose();
        }
    }
}
