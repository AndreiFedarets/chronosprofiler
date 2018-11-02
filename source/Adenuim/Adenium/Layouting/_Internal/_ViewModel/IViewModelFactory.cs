namespace Adenium.Layouting
{
    internal interface IViewModelFactory
    {
        IViewModel CreateViewModel<T1, T2, T3>(T1 dependency1, T2 dependency2, T3 dependency3);
    }
}
