using System.Collections.Generic;
using System.Linq;
using Chronos.Accessibility.IIS;
using Chronos.Client.Win.ViewModels.Start;

namespace Chronos.Client.Win.ViewModels.Common.WebApplication
{
    public class ProfilingTargetSettingsViewModel : ProfilingTargetSettingsBaseViewModel
    {
        private readonly Chronos.Common.WebApplication.ProfilingTargetSettings _profilingTargetSettings;

        public ProfilingTargetSettingsViewModel(ConfigurationSettings configurationSettings, IHostApplicationSelector applicationSelector)
            : base(configurationSettings, applicationSelector)
        {
            _profilingTargetSettings = new Chronos.Common.WebApplication.ProfilingTargetSettings(configurationSettings.ProfilingTargetSettings);
            InitializeApplicationPools();
        }

        public override string DisplayName
        {
            get { return "Target AppPool"; }
            set { }
        }

        public override bool DialogReady
        {
            get { return !string.IsNullOrWhiteSpace(SelectedApplicationPool); }
        }

        public IEnumerable<string> ApplicationPools { get; private set; }

        public string SelectedApplicationPool
        {
            get { return _profilingTargetSettings.ApplicationPool; }
            set
            {
                _profilingTargetSettings.ApplicationPool = value;
                NotifyOfPropertyChange(() => SelectedApplicationPool);
                NotifyContractSourceChanged();
            }
        }

        private void InitializeApplicationPools()
        {
            Host.IApplication selectedApplication = SelectedApplication;
            if (selectedApplication == null)
            {
                ApplicationPools = Enumerable.Empty<string>();
            }
            else
            {
                IInternetInformationServiceAccessor service = selectedApplication.ServiceContainer.Resolve<IInternetInformationServiceAccessor>();
                ApplicationPools = service.GetApplicationPools();
            }
            SelectedApplicationPool = string.Empty;
        }
    }
}
