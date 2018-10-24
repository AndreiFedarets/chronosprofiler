namespace Adenium.Menu
{
    internal interface ICompositeControl : IMenuControl
    {
        void MergeWith(IMenuControl control);
    }
}
