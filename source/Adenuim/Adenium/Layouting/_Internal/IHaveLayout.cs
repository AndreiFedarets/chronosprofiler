namespace Adenium.Layouting
{
    internal interface IHaveLayout
    {
        ViewModelLayout Layout { get; }

        void AssignLayout(ViewModelLayout layout);
    }
}
