namespace Adenium.Layouting
{
    public interface ILayoutProvider
    {
        void ConfigureContainer(IContainer container);

        string GetLayout(IViewModel viewModel);
    }
}
