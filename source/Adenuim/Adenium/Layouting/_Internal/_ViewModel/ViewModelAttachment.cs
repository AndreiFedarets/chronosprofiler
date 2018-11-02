namespace Adenium.Layouting
{
    internal sealed class ViewModelAttachment
    {
        private readonly IViewModelFactory _factory;

        public ViewModelAttachment(string id, ViewModelActivation activation, int order, string viewType, IViewModelFactory factory)
        {
            Id = id;
            Activation = activation;
            Order = order;
            ViewType = viewType;
            _factory = factory;
        }

        public string Id { get; private set; }

        public ViewModelActivation Activation { get; private set;  }

        public int Order { get; private set; }

        public string ViewType { get; private set; }

        public IViewModel CreateViewModel<T1, T2, T3>(T1 dependency1, T2 dependency2, T3 dependency3)
        {
            return _factory.CreateViewModel(dependency1, dependency2, dependency3);
        }

        public IViewModel CreateViewModel()
        {
            return _factory.CreateViewModel<object, object, object>(null, null, null);
        }
    }
}
