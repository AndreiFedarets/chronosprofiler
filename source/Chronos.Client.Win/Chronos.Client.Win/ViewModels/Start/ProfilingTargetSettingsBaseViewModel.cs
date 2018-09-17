using System;

namespace Chronos.Client.Win.ViewModels.Start
{
    public abstract class ProfilingTargetSettingsBaseViewModel : ViewModel, Contracts.Dialog.IContractSource
    {
        private readonly ProfilingTargetSettings _profilingTargetSettings;
        private readonly IHostApplicationSelector _applicationSelector;

        protected ProfilingTargetSettingsBaseViewModel(ConfigurationSettings configurationSettings, IHostApplicationSelector applicationSelector)
        {
            _applicationSelector = applicationSelector;
            _profilingTargetSettings = configurationSettings.ProfilingTargetSettings;
            _applicationSelector.SelectionChanged += ApplicationSelectorSelectionChanged;
        }

        public abstract bool Ready { get; }

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
        }

        protected void NotifyContractSourceChanged()
        {
            ContractSourceChanged.SafeInvoke(this, EventArgs.Empty);
        }
    }
}
