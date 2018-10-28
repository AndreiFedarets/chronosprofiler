using System;
using Adenium;
using Adenium.Layouting;

namespace Chronos.Client.Win.Menu.Specialized
{
    public abstract class ProfilingMenuItemBase : MenuControlHandlerBase
    {
        protected IProfilingApplication Application;
        private readonly Lazy<IViewModel> _viewModel;

        protected ProfilingMenuItemBase(IProfilingApplication application)
        {
            Application = application;
            _viewModel = new Lazy<IViewModel>(GetViewModel);
        }

        protected abstract IViewModel GetViewModel();

        public override void OnAction()
        {
            Application.MainViewModel.ActivateItem(_viewModel.Value);
        }
    }
}
