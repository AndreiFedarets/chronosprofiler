using System.Collections.Generic;
using System.Linq;
using Chronos.Accessibility.WS;
using Chronos.Client.Win.Common.ServiceApplication.Properties;
using Chronos.Client.Win.ViewModels.Start;

namespace Chronos.Client.Win.ViewModels.Common.ServiceApplication
{
    public class ProfilingTargetSettingsViewModel : ProfilingTargetSettingsBaseViewModel
    {
        private readonly Chronos.Common.ServiceApplication.ProfilingTargetSettings _profilingTargetSettings;
        private WindowsServiceInfo _selectedWindowsService;

        public ProfilingTargetSettingsViewModel(ConfigurationSettings configurationSettings,
            IHostApplicationSelector applicationSelector)
            : base(configurationSettings, applicationSelector)
        {
            _profilingTargetSettings = new Chronos.Common.ServiceApplication.ProfilingTargetSettings(configurationSettings.ProfilingTargetSettings);
            InitializeWindowsServices();
        }

        public override bool DialogReady
        {
            get { return SelectedWindowsService != null; }
        }

        public bool HasPermissions
        {
            get
            {
                if (!IsApplicationSelected)
                {
                    return false;
                }
                IWindowsServicesAccessor services = SelectedApplication.ServiceContainer.Resolve<IWindowsServicesAccessor>();
                return services.HasPermissions;
            }
        }

        public override bool ShowNotificationMessage
        {
            get { return base.ShowNotificationMessage || !HasPermissions; }
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
                return string.Empty;
            }
        }

        public WindowsServiceInfo SelectedWindowsService
        {
            get { return _selectedWindowsService; }
            set
            {
                _selectedWindowsService = value;
                _profilingTargetSettings.ServiceName = _selectedWindowsService == null
                    ? string.Empty
                    : _selectedWindowsService.ServiceName;
                NotifyOfPropertyChange(() => SelectedWindowsService);
                NotifyContractSourceChanged();
            }
        }

        public IEnumerable<WindowsServiceInfo> WindowsServices { get; private set; }

        protected override void OnSelectedApplicationChanged()
        {
            base.OnSelectedApplicationChanged();
            NotifyOfPropertyChange(() => ShowNotificationMessage);
        }

        private void InitializeWindowsServices()
        {
            Host.IApplication selectedApplication = SelectedApplication;
            if (selectedApplication == null)
            {
                WindowsServices = Enumerable.Empty<WindowsServiceInfo>();
            }
            else
            {
                IWindowsServicesAccessor service =
                    selectedApplication.ServiceContainer.Resolve<IWindowsServicesAccessor>();
                WindowsServices = service.GetServices();
            }
            SelectedWindowsService = null;
        }
    }
}
;