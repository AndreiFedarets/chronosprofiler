namespace Adenium.Layouting
{
    public interface ILayoutProvider
    {
        void ConfigureContainer(IViewModel targetViewModel, IContainer container);

        string GetLayout(IViewModel targetViewModel);
    }
}
