using Chronos.Client.Win.Models;
using Chronos.Client.Win.ViewModels;
using Chronos.Client.Win.ViewModels.Profiling;

namespace Chronos.Client.Win.Menu.Specialized
{
    public abstract class UnitsMenuItemBase : MenuItem
    {
        protected readonly ProfilingViewModel ProfilingViewModel;
        private UnitsViewModel _viewModel;

        protected UnitsMenuItemBase(ProfilingViewModel profilingViewModel)
        {
            ProfilingViewModel = profilingViewModel;
        }

        private UnitsViewModel ViewModel
        {
            get
            {
                if (_viewModel == null)
                {
                    IUnitsModel model = GetModel();
                    _viewModel = new UnitsViewModel(model);
                }
                if (!ProfilingViewModel.Contains(_viewModel))
                {
                    ProfilingViewModel.Add(_viewModel);
                }
                return _viewModel;
            }
        }

        protected abstract IUnitsModel GetModel();

        public override void OnAction()
        {
            ProfilingViewModel.Activate(ViewModel);
        }
    }
}
