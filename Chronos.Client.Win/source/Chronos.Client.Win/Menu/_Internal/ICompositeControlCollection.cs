namespace Chronos.Client.Win.Menu
{
    internal interface ICompositeControlCollection : IControlCollection, ICompositeControl
    {
        void MergeWith(IControlCollection control);
    }
}
