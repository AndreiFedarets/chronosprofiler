namespace Adenium.Layouting
{
    internal sealed class ViewModelReference
    {
        private readonly IViewModelFactory _factory;

        public ViewModelReference(ViewModelActivation activation, int order, IViewModelFactory factory)
        {
            Activation = activation;
            Order = order;
            _factory = factory;
        }

        public ViewModelActivation Activation { get; private set;  }

        public int Order { get; private set; }

        public IViewModel CreateViewModel()
        {
            return _factory.CreateViewModel();
        }
    }
}
