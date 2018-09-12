using System;
using Chronos.Client.Win.ViewModels;
using Chronos.Client.Win.ViewModels.Profiling;

namespace Chronos.Client.Win.Menu.Specialized
{
    public abstract class ProfilingMenuItemBase : MenuItem
    {
        protected readonly ProfilingViewModel ProfilingViewModel;
        private readonly Lazy<ViewModel> _viewModel;

        protected ProfilingMenuItemBase(ProfilingViewModel profilingViewModel)
        {
            ProfilingViewModel = profilingViewModel;
            _viewModel = new Lazy<ViewModel>(GetViewModel);
        }

        protected abstract ViewModel GetViewModel();

        public override void OnAction()
        {
            ProfilingViewModel.Activate(_viewModel.Value);
        }
    }
}
