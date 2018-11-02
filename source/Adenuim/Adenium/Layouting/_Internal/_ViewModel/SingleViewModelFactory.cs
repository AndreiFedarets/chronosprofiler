namespace Adenium.Layouting
{
    internal sealed class SingleViewModelFactory : ViewModelFactoryBase
    {
        private readonly object _lock;
        private IViewModel _viewModel;

        public SingleViewModelFactory(IViewModel targetViewModel, string typeName)
            : base(targetViewModel, typeName)
        {
            _lock = new object();
        }

        public override IViewModel CreateViewModel<T1, T2, T3>(T1 dependency1, T2 dependency2, T3 dependency3)
        {
            if (_viewModel == null)
            {
                lock (_lock)
                {
                    if (_viewModel == null)
                    {
                        _viewModel = CreateViewModelInternal(dependency1, dependency2, dependency3);
                    }
                }
            }
            return _viewModel;
        }
    }
}
