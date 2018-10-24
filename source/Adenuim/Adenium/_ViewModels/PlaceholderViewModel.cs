using System;

namespace Adenium
{
    public class PlaceholderViewModel : ViewModel, IContractProxy
    {
        private readonly PlaceholderContent _content;
        private IViewModel _underlyingViewModel;

        public PlaceholderViewModel(PlaceholderContent content)
        {
            _content = content;
            _content.ViewModelChanged += OnContentViewModelChanged;
        }

        public object UnderlyingObject
        {
            get { return UnderlyingViewModel; }
        }

        public override string DisplayName
        {
            get
            {
                if (!string.IsNullOrEmpty(_content.DisplayName))
                {
                    return _content.DisplayName;
                }
                IViewModel viewModel = UnderlyingViewModel;
                if (viewModel != null)
                {
                    return viewModel.DisplayName;
                }
                return base.DisplayName;
            }
        }

        public override Guid TypeId
        {
            get 
            {
                IViewModel viewModel = UnderlyingViewModel;
                if (viewModel == null)
                {
                    return base.TypeId;
                }
                return viewModel.TypeId;
            }
        }

        public override Guid InstanceId
        {
            get
            {
                IViewModel viewModel = UnderlyingViewModel;
                if (viewModel == null)
                {
                    return base.InstanceId;
                }
                return viewModel.InstanceId;
            }
        }

        internal IViewModel UnderlyingViewModel
        {
            get
            {
                if (_underlyingViewModel == null)
                {
                    _underlyingViewModel = _content.CreateViewModel();
                }
                return _underlyingViewModel;
            }
        }

        public event EventHandler UnderlyingViewModelChanged;

        public event EventHandler<ContractProxyObjectChangedEventArgs> UnderlyingObjectChanged;

        private void OnContentViewModelChanged(object sender, EventArgs e)
        {
            object oldViewModel = _underlyingViewModel;
            _underlyingViewModel.Dispose();
            _underlyingViewModel = null;
            EventHandler eventHandler = UnderlyingViewModelChanged;
            if (eventHandler != null)
            {
                eventHandler(this, e);
            }
            //Force viewModel creation
            object newViewModel = UnderlyingViewModel;
            ContractProxyObjectChangedEventArgs.RaiseEvent(UnderlyingObjectChanged, this, oldViewModel, newViewModel);
        }
    }
}
