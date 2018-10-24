namespace Adenium.Menu
{
    internal interface ICompositeControlCollection : IMenuControlCollection, ICompositeControl
    {
        void MergeWith(IMenuControlCollection control);
    }
}
