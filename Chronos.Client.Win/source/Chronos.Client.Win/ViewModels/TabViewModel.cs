using System;

namespace Chronos.Client.Win.ViewModels
{
    public class TabViewModel : PageViewModel
    {
        private ViewModel _activeViewModel;

        public ViewModel ActiveViewModel
        {
            get { return _activeViewModel; }
            private set 
            {
                _activeViewModel = value;
                NotifyOfPropertyChange(() => ActiveViewModel);
                OnActiveViewModelChanged();
            }
        }

        public event EventHandler ActiveViewModelChanged;

        public bool Activate(Guid instanceId)
        {
            ViewModel viewModel = FindByInstanceId(instanceId);
            return Activate(viewModel);
        }

        public bool Activate(ViewModel viewModel)
        {
            if (viewModel == null || !Items.Contains(viewModel))
            {
                return false;
            }
            ActiveViewModel = viewModel;
            return true;
        }

        protected virtual void OnActiveViewModelChanged()
        {
            EventHandler handler = ActiveViewModelChanged;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

    }
}
