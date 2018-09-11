namespace Chronos.Client.Win.Menu
{
    public interface IMenuItem : IControlCollection, IAction
    {
        string Text { get; }
    }
}
