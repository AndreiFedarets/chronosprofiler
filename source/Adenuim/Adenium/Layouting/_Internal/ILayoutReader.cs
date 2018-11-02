namespace Adenium.Layouting
{
    internal interface ILayoutReader
    {
        bool SupportsContentType(string layoutContent);

        ViewModelLayout Read(string layoutContent, IViewModel targetViewModel, IContainer scopeContainer);
    }
}
