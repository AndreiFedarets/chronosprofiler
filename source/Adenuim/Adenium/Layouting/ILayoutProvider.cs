namespace Adenium.Layouting
{
    public interface ILayoutProvider
    {
        IActivator Activator { get; }

        string GetLayout(IViewModel viewModel);
    }
}
