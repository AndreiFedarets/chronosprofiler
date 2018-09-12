namespace Chronos.Client.Win.ViewModels
{
    public abstract class PlaceholderContent
    {
        public abstract string DisplayName { get; }

        public abstract ViewModel CreateViewModel();
    }
}
