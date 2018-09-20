using System.Collections.Generic;
using System.Linq;
using Chronos.Accessibility.IIS;
using Chronos.Client.Win.Common.WebApplication.Properties;
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

        public override bool DialogReady
        {
            get { return !string.IsNullOrWhiteSpace(SelectedApplicationPool); }
        }

        public bool HasPermissions
        {
            get
            {
                if (!IsApplicationSelected)
                {
                    return false;
                }
                IInternetInformationServiceAccessor service = SelectedApplication.ServiceContainer.Resolve<IInternetInformationServiceAccessor>();
                return service.HasPermissions;
            }
        }

        public bool IsInternetInformationServiceInstalled
        {
            get
            {
                if (!IsApplicationSelected)
                {
                    return false;
                }
                IInternetInformationServiceAccessor internetInformationService = SelectedApplication.ServiceContainer.Resolve<IInternetInformationServiceAccessor>();
                return internetInformationService.IsAvailable;
            }
        }

        public override bool ShowNotificationMessage
        {
            get { return base.ShowNotificationMessage || !IsInternetInformationServiceInstalled || !HasPermissions; }
        }

        public override string NotificationMessage
        {
            get
            {
                string message = base.NotificationMessage;
                if (!string.IsNullOrWhiteSpace(message))
                {
                    return message;
                }
                if (!HasPermissions)
                {
                    return Resources.PermissionsErrorMessage;
                }
                if (!IsInternetInformationServiceInstalled)
                {
                    return Resources.MissingInternetInformationServiceErrorMessage;
                }
                return string.Empty;
            }
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
