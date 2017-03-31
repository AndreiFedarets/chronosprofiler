using System;
using System.Drawing;
using Caliburn.Micro;
using Chronos.Client.Win.Model;
using Chronos.Client.Win.ViewModels;

namespace Chronos.Client.Win.DotNet.SqlProfiler
{
    internal abstract class UnitsMenuAdapter : PropertyChangedBase, IMenuItemAdapter
    {
        private ProfilingResultsViewModel _profilingResultsViewModel;
        private UnitsViewModel _viewModel;
        private readonly ISession _session;

        protected UnitsMenuAdapter(ISession session)
        {
            _session = session;
            Uid = Guid.NewGuid();
        }

        public Guid Uid { get; private set; }

        public virtual Bitmap Icon
        {
            get { return null; }
        }

        public virtual bool IsEnabled
        {
            get { return true; }
        }

        public virtual bool IsVisible
        {
            get { return true; }
        }

        public virtual string Text
        {
            get { return ViewModel.DisplayName; }
        }

        private UnitsViewModel ViewModel
        {
            get
            {
                if (_viewModel == null)
                {
                    IUnitsModel model = CreateModel(_session);
                    _viewModel = new UnitsViewModel(model);
                }
                return _viewModel;
            }
        }

        protected abstract IUnitsModel CreateModel(ISession session);

        public void Execute()
        {
            _profilingResultsViewModel.ActivateItem(ViewModel);
        }

        public void OnViewModelAttached(object viewModel)
        {
            _profilingResultsViewModel = (ProfilingResultsViewModel)viewModel;
        }
    }
}
