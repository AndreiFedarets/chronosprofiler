using System;
using System.Collections.Generic;
using System.Linq;
using Chronos.Accessibility.WS;

namespace Chronos.Client.Win.ViewModels.Common.ServiceApplication
{
    public class ProfilingTargetSettingsViewModel : ViewModel, Contracts.Dialog.IContractSource
    {
        private readonly Chronos.Common.ServiceApplication.ProfilingTargetSettings _profilingTargetSettings;
        private readonly IHostApplicationSelector _applicationSelector;
        private WindowsServiceInfo _selectedWindowsService;

        public ProfilingTargetSettingsViewModel(ConfigurationSettings configurationSettings,
            IHostApplicationSelector applicationSelector)
        {
            _profilingTargetSettings = new Chronos.Common.ServiceApplication.ProfilingTargetSettings(configurationSettings.ProfilingTargetSettings);
            _applicationSelector = applicationSelector;
            _applicationSelector.SelectionChanged += ApplicationSelectorSelectionChanged;
            InitializeWindowsServices();
        }

        public string ServiceName
        {
            get { return _profilingTargetSettings.ServiceName; }
            set
            {
                _profilingTargetSettings.ServiceName = value;
                NotifyOfPropertyChange(() => ServiceName);
            }
        }

        public WindowsServiceInfo SelectedWindowsService
        {
            get { return _selectedWindowsService; }
            set
            {
                _selectedWindowsService = value;
                _profilingTargetSettings.ServiceName = _selectedWindowsService == null ? string.Empty : _selectedWindowsService.ServiceName;
                NotifyOfPropertyChange(() => SelectedWindowsService);
            }
        }

        public IEnumerable<WindowsServiceInfo> WindowsServices { get; private set; }

        public bool ProfileChildProcess
        {
            get { return _profilingTargetSettings.ProfileChildProcess; }
            set
            {
                _profilingTargetSettings.ProfileChildProcess = value;
                NotifyOfPropertyChange(() => ProfileChildProcess);
            }
        }

        public Host.IApplication SelectedApplication
        {
            get { return _applicationSelector.SelectedApplication; }
        }

        public event EventHandler ContractSourceChanged;

        public override void Dispose()
        {
            _applicationSelector.SelectionChanged -= ApplicationSelectorSelectionChanged;
            base.Dispose();
        }

        private void ApplicationSelectorSelectionChanged(object sender, System.EventArgs e)
        {
            NotifyOfPropertyChange(() => SelectedApplication);
            InitializeWindowsServices();
        }

        private void InitializeWindowsServices()
        {
            Host.IApplication selectedApplication = SelectedApplication;
            if (selectedApplication == null)
            {
                WindowsServices = Enumerable.Empty<WindowsServiceInfo>();
                SelectedWindowsService = null;
            }
            else
            {
                IWindowsServicesAccessor service = selectedApplication.ServiceContainer.Resolve<IWindowsServicesAccessor>();
                WindowsServices = service.GetServices();
                SelectedWindowsService = WindowsServices.FirstOrDefault();
            }
        }
    }
}
