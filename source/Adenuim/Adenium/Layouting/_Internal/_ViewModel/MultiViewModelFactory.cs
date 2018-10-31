namespace Adenium.Layouting
{
    internal sealed class MultiViewModelFactory : ViewModelFactoryBase
    {
        public MultiViewModelFactory(IContainerViewModel parentViewModel, string typeName, IContainer container)
            : base(parentViewModel, typeName, container)
        {
        }

        public override IViewModel CreateViewModel()
        {
            return CreateViewModelInternal();
        }
    }
}
