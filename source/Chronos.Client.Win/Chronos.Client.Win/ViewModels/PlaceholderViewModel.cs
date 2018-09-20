using System;
using Chronos.Client.Win.Contracts;

namespace Chronos.Client.Win.ViewModels
{
    public class PlaceholderViewModel : ViewModel, IContractProxy
    {
        private EventHandler<ContractProxyObjectChangedEventArgs> _underlyingObjectChanged;
        private readonly PlaceholderContent _content;
        private ViewModel _underlyingViewModel;
        private PageViewModel _page;

        public PlaceholderViewModel(PlaceholderContent content)
        {
            _content = content;
            _content.ViewModelChanged += OnContentViewModelChanged;
        }

        object IContractProxy.UnderlyingObject
        {
            get { return UnderlyingViewModel; }
        }

        event EventHandler<ContractProxyObjectChangedEventArgs> IContractProxy.UnderlyingObjectChanged 
        {
            add { _underlyingObjectChanged += value; }
            remove { _underlyingObjectChanged -= value; }
        }

        internal ViewModel UnderlyingViewModel 
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

        public override string DisplayName
        {
            get
            {
                if (!string.IsNullOrEmpty(_content.DisplayName))
                {
                    return _content.DisplayName;
                }
                ViewModel viewModel = UnderlyingViewModel;
                if (viewModel == null)
                {
                    return base.DisplayName;
                }
                return viewModel.DisplayName;
            }
        }

        public override Guid TypeId
        {
            get 
            {
                ViewModel viewModel = UnderlyingViewModel;
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
                ViewModel viewModel = UnderlyingViewModel;
                if (viewModel == null)
                {
                    return base.InstanceId;
                }
                return viewModel.InstanceId;
            }
        }

        public override PageViewModel Page
        {
            get { return _page; }
            internal set 
            {
                _page = value;
                ViewModel viewModel = UnderlyingViewModel;
                if (viewModel != null)
                {
                    viewModel.Page = value; 
                }
            }
        }

        public event EventHandler UnderlyingViewModelChanged;

        protected internal override void OnAttached()
        {
            ViewModel viewModel = UnderlyingViewModel;
            if (viewModel != null)
            {
                viewModel.OnAttached();
            }
        }

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
            if (_underlyingObjectChanged != null)
            {
                _underlyingObjectChanged(this, new ContractProxyObjectChangedEventArgs(oldViewModel, newViewModel));
            }
        }
    }
}
