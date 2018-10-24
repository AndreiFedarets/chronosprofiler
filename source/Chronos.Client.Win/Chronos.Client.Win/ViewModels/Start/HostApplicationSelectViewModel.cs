using System;
using System.Collections.Generic;
using Adenium;

namespace Chronos.Client.Win.ViewModels.Start
{
    public sealed class HostApplicationSelectViewModel : ViewModel, IDialogContractSource
    {
        private readonly IHostApplicationSelector _hostApplicationSelector;

        public HostApplicationSelectViewModel(IHostApplicationSelector hostApplicationSelector)
        {
            _hostApplicationSelector = hostApplicationSelector;
            _hostApplicationSelector.SelectionChanged += OnApplicationSelectionChanged;
        }

        public IEnumerable<Host.IApplication> Applications
        {
            get { return _hostApplicationSelector; }
        }

        public override string DisplayName
        {
            get { return "Target Machine"; }
            set { }
        }

        public bool DialogReady
        {
            get { return _hostApplicationSelector.SelectedApplication != null; }
        }

        public Host.IApplication SelectedApplication
        {
            get { return _hostApplicationSelector.SelectedApplication; }
            set { _hostApplicationSelector.SelectedApplication = value; }
        }

        public event EventHandler ContractSourceChanged;

        private void OnApplicationSelectionChanged(object sender, EventArgs e)
        {
            NotifyOfPropertyChange(() => SelectedApplication);
            ContractSourceChanged.SafeInvoke(this, EventArgs.Empty);
        }

        public override void Dispose()
        {
            base.Dispose();
            _hostApplicationSelector.SelectionChanged -= OnApplicationSelectionChanged;
        }
    }
}
