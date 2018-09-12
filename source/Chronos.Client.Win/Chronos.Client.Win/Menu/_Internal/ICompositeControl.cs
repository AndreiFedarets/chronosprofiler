namespace Chronos.Client.Win.Menu
{
    internal interface ICompositeControl : IControl
    {
        void MergeWith(IControl control);
    }
}
