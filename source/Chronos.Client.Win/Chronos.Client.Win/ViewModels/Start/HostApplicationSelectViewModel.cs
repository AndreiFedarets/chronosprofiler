using System.Collections.Generic;

namespace Chronos.Client.Win.ViewModels.Start
{
    public sealed class HostApplicationSelectViewModel : ViewModel
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

        public Host.IApplication SelectedApplication
        {
            get { return _hostApplicationSelector.SelectedApplication; }
            set { _hostApplicationSelector.SelectedApplication = value; }
        }

        private void OnApplicationSelectionChanged(object sender, System.EventArgs e)
        {
            NotifyOfPropertyChange(() => SelectedApplication);
        }

        public override void Dispose()
        {
            base.Dispose();
            _hostApplicationSelector.SelectionChanged -= OnApplicationSelectionChanged;
        }
    }
}
