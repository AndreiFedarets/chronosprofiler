using System;

namespace Adenium.ViewModels
{
    public abstract class PlaceholderViewModel : ViewModel, IContractProxy
    {
        private IViewModel _underlyingViewModel;

        public object UnderlyingObject
        {
            get { return UnderlyingViewModel; }
        }

        public override string DisplayName
        {
            get { return UnderlyingViewModel.DisplayName; }
        }

        public override string ViewModelUid
        {
            get { return UnderlyingViewModel.ViewModelUid; }
        }

        public override Guid InstanceId
        {
            get { return UnderlyingViewModel.InstanceId; }
        }

        internal IViewModel UnderlyingViewModel
        {
            get
            {
                if (_underlyingViewModel == null)
                {
                    _underlyingViewModel = CreateViewModel();
                }
                return _underlyingViewModel;
            }
        }

        public event EventHandler UnderlyingViewModelChanged;

        public event EventHandler<ContractProxyObjectChangedEventArgs> UnderlyingObjectChanged;

        protected abstract IViewModel CreateViewModel();

        protected void InvalidateViewModel()
        {
            IViewModel oldViewModel = _underlyingViewModel;
            _underlyingViewModel = null;
            if (oldViewModel != null)
            {
                oldViewModel.Dispose();
            }
            //Force viewModel creation
            object newViewModel = UnderlyingViewModel;
            UnderlyingViewModelChanged.SafeInvoke(this, EventArgs.Empty);
            ContractProxyObjectChangedEventArgs.RaiseEvent(UnderlyingObjectChanged, this, oldViewModel, newViewModel);
        }

        //private readonly PlaceholderContent _content;
        //private IViewModel _underlyingViewModel;

        //public PlaceholderViewModel(PlaceholderContent content)
        //{
        //    _content = content;
        //    _content.ViewModelChanged += OnContentViewModelChanged;
        //}

        //public object UnderlyingObject
        //{
        //    get { return UnderlyingViewModel; }
        //}

        //public override string DisplayName
        //{
        //    get
        //    {
        //        if (!string.IsNullOrEmpty(_content.DisplayName))
        //        {
        //            return _content.DisplayName;
        //        }
        //        IViewModel viewModel = UnderlyingViewModel;
        //        if (viewModel != null)
        //        {
        //            return viewModel.DisplayName;
        //        }
        //        return base.DisplayName;
        //    }
        //}

        //public override string ViewModelUid
        //{
        //    get 
        //    {
        //        IViewModel viewModel = UnderlyingViewModel;
        //        if (viewModel == null)
        //        {
        //            return base.ViewModelUid;
        //        }
        //        return viewModel.ViewModelUid;
        //    }
        //}

        //public override Guid InstanceId
        //{
        //    get
        //    {
        //        IViewModel viewModel = UnderlyingViewModel;
        //        if (viewModel == null)
        //        {
        //            return base.InstanceId;
        //        }
        //        return viewModel.InstanceId;
        //    }
        //}

        //internal IViewModel UnderlyingViewModel
        //{
        //    get
        //    {
        //        if (_underlyingViewModel == null)
        //        {
        //            _underlyingViewModel = _content.CreateViewModel();
        //        }
        //        return _underlyingViewModel;
        //    }
        //}

        //public event EventHandler UnderlyingViewModelChanged;

        //public event EventHandler<ContractProxyObjectChangedEventArgs> UnderlyingObjectChanged;

        //private void OnContentViewModelChanged(object sender, EventArgs e)
        //{
        //    object oldViewModel = _underlyingViewModel;
        //    _underlyingViewModel.Dispose();
        //    _underlyingViewModel = null;
        //    EventHandler eventHandler = UnderlyingViewModelChanged;
        //    if (eventHandler != null)
        //    {
        //        eventHandler(this, e);
        //    }
        //    //Force viewModel creation
        //    object newViewModel = UnderlyingViewModel;
        //    ContractProxyObjectChangedEventArgs.RaiseEvent(UnderlyingObjectChanged, this, oldViewModel, newViewModel);
        //}
    }
}
