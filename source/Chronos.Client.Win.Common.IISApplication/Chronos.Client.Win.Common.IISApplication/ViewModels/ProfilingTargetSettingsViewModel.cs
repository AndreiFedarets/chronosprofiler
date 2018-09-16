using System;
using System.Collections.Generic;
using System.Linq;
using Chronos.Accessibility.IIS;

namespace Chronos.Client.Win.ViewModels.Common.IISApplication
{
    public class ProfilingTargetSettingsViewModel : ViewModel
    {
        private readonly Chronos.Common.IISApplication.ProfilingTargetSettings _profilingTargetSettings;
        private readonly IHostApplicationSelector _applicationSelector;

        public ProfilingTargetSettingsViewModel(ConfigurationSettings configurationSettings,
            IHostApplicationSelector applicationSelector)
        {
            _profilingTargetSettings = new Chronos.Common.IISApplication.ProfilingTargetSettings(configurationSettings.ProfilingTargetSettings);
            _applicationSelector = applicationSelector;
            _applicationSelector.SelectionChanged += ApplicationSelectorSelectionChanged;
            InitializeApplicationPools();
        }
        
        public Host.IApplication SelectedApplication
        {
            get { return _applicationSelector.SelectedApplication; }
        }

        public IEnumerable<string> ApplicationPools { get; private set; }

        public string SelectedApplicationPool
        {
            get { return _profilingTargetSettings.ApplicationPool; }
            set
            {
                _profilingTargetSettings.ApplicationPool = value;
                NotifyOfPropertyChange(() => SelectedApplicationPool);
            }
        }

        public bool ProfileChildProcess
        {
            get { return _profilingTargetSettings.ProfileChildProcess; }
            set
            {
                _profilingTargetSettings.ProfileChildProcess = value;
                NotifyOfPropertyChange(() => ProfileChildProcess);
            }
        }

        public override void Dispose()
        {
            _applicationSelector.SelectionChanged -= ApplicationSelectorSelectionChanged;
            base.Dispose();
        }

        private void ApplicationSelectorSelectionChanged(object sender, EventArgs e)
        {
            NotifyOfPropertyChange(() => SelectedApplication);
            InitializeApplicationPools();
        }

        private void InitializeApplicationPools()
        {
            Host.IApplication selectedApplication = SelectedApplication;
            if (selectedApplication == null)
            {
                ApplicationPools = Enumerable.Empty<string>();
                SelectedApplicationPool = string.Empty;
            }
            else
            {
                IInternetInformationService service = selectedApplication.ServiceContainer.Resolve<IInternetInformationService>();
                ApplicationPools = service.GetApplicationPools();
                SelectedApplicationPool = ApplicationPools.FirstOrDefault();
            }
        }
    }
}
