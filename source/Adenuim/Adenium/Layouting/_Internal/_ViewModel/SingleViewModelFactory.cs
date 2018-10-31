using System;

namespace Adenium.Layouting
{
    internal sealed class SingleViewModelFactory : ViewModelFactoryBase
    {
        private readonly Lazy<IViewModel> _viewModel;

        public SingleViewModelFactory(IContainerViewModel parentViewModel, string typeName, IContainer container)
            : base(parentViewModel, typeName, container)
        {
            _viewModel = new Lazy<IViewModel>(CreateViewModelInternal);
        }

        public override IViewModel CreateViewModel()
        {
            return _viewModel.Value;
        }
    }
}
