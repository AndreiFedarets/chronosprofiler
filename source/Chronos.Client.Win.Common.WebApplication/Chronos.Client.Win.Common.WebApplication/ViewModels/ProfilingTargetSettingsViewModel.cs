using Chronos.Accessibility.IIS;
using Chronos.Client.Win.ViewModels.Start;
using Layex.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace Chronos.Client.Win.Common.WebApplication.ViewModels
{
    [ViewModel(Constants.ViewModels.ProfilingTargetSettings)]
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
