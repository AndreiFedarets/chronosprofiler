using System;
using System.Drawing;
using Caliburn.Micro;
using Chronos.Client.Win.ViewModels;

namespace Chronos.Client.Win.DotNet.TracingProfiler
{
    internal class DotNetPerformanceAdapter : PropertyChangedBase, IMenuItemAdapter
    {
        private ProfilingResultsViewModel _profilingResultsViewModel;
        private readonly ISession _session;
        private EventsTreeViewModel _viewModel;

        public DotNetPerformanceAdapter(ISession session)
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
            get { return ".NET Performance"; }
        }

        public bool IsViewModelInitialized
        {
            get { return _viewModel != null; }
        }

        public EventsTreeViewModel ViewModel
        {
            get
            {
                if (_viewModel == null)
                {
                    IEventTreeCollection eventTree = _session.ServiceContainer.Resolve<IEventTreeCollection>();
                    IEventFormatter eventFormatter = _session.ServiceContainer.Resolve<IEventFormatter>();
                    _viewModel = new EventsTreeViewModel(eventTree, eventFormatter);
                }
                return _viewModel;
            }
        }

        public void Execute()
        {
            _profilingResultsViewModel.ActivateItem(ViewModel);
        }

        public void OnViewModelAttached(object viewModel)
        {
            _profilingResultsViewModel = (ProfilingResultsViewModel) viewModel;
        }
    }
}
