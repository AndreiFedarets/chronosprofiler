using System.Collections.Generic;
using System.Linq;
using Chronos.Accessibility.IIS;
using Chronos.Client.Win.ViewModels.Start;

namespace Chronos.Client.Win.ViewModels.Common.IISApplication
{
    public class ProfilingTargetSettingsViewModel : ProfilingTargetSettingsBaseViewModel
    {
        private readonly Chronos.Common.IISApplication.ProfilingTargetSettings _profilingTargetSettings;

        public ProfilingTargetSettingsViewModel(ConfigurationSettings configurationSettings, IHostApplicationSelector applicationSelector)
            : base(configurationSettings, applicationSelector)
        {
            _profilingTargetSettings = new Chronos.Common.IISApplication.ProfilingTargetSettings(configurationSettings.ProfilingTargetSettings);
            InitializeApplicationPools();
        }

        public override bool Ready
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
                IInternetInformationService service = selectedApplication.ServiceContainer.Resolve<IInternetInformationService>();
                ApplicationPools = service.GetApplicationPools();
            }
            SelectedApplicationPool = string.Empty;
        }
    }
}
