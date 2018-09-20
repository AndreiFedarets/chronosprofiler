using System;
using System.Collections.Generic;
using System.Linq;
using Chronos.Accessibility.WS;
using Chronos.Client.Win.ViewModels.Start;

namespace Chronos.Client.Win.ViewModels.Common.ServiceApplication
{
    public class ProfilingTargetSettingsViewModel : ProfilingTargetSettingsBaseViewModel
    {
        private readonly Chronos.Common.ServiceApplication.ProfilingTargetSettings _profilingTargetSettings;
        private WindowsServiceInfo _selectedWindowsService;
        private List<WindowsServiceInfo> _windowsServices;

        public ProfilingTargetSettingsViewModel(ConfigurationSettings configurationSettings, IHostApplicationSelector applicationSelector)
            : base(configurationSettings, applicationSelector)
        {
            _profilingTargetSettings = new Chronos.Common.ServiceApplication.ProfilingTargetSettings(configurationSettings.ProfilingTargetSettings);
            InitializeWindowsServices();
        }

        public override bool DialogReady
        {
            get { return SelectedWindowsService != null; }
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

        public IEnumerable<WindowsServiceInfo> WindowsServices
        {
            get { return _windowsServices;}
        }

        private void InitializeWindowsServices()
        {
            Host.IApplication selectedApplication = SelectedApplication;
            if (selectedApplication == null)
            {
                _windowsServices = new List<WindowsServiceInfo>();
            }
            else
            {
                IWindowsServicesAccessor service = selectedApplication.ServiceContainer.Resolve<IWindowsServicesAccessor>();
                _windowsServices = service.GetServices();
                _windowsServices.Sort((x, y) => string.CompareOrdinal(x.DisplayName, y.DisplayName));
            }
            NotifyOfPropertyChange(() => WindowsServices);
            SelectedWindowsService = null;
        }
    }
}
;