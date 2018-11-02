namespace Adenium.Layouting
{
    internal sealed class MultiViewModelFactory : ViewModelFactoryBase
    {
        public MultiViewModelFactory(IViewModel targetViewModel, string typeName)
            : base(targetViewModel, typeName)
        {
        }

        public override IViewModel CreateViewModel<T1, T2, T3>(T1 dependency1, T2 dependency2, T3 dependency3)
        {
            return CreateViewModelInternal(dependency1, dependency2, dependency3);
        }
    }
}
