using System;
using Chronos.Client.Win.Contracts;

namespace Chronos.Client.Win.ViewModels
{
    public class PlaceholderViewModel : ViewModel, IContractProxy
    {
        private PageViewModel _page;
        private ViewModel _underlyingViewModel;
        private PlaceholderContent _content;

        public PlaceholderViewModel(PlaceholderContent content)
        {
            _content = content;
        }

        object IContractProxy.UnderlyingObject
        {
            get { return UnderlyingViewModel; }
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

        protected internal override void OnAttached()
        {
            ViewModel viewModel = UnderlyingViewModel;
            if (viewModel != null)
            {
                viewModel.OnAttached();
            }
        }
    }
}
