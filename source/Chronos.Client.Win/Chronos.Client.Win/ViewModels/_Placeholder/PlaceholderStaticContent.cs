namespace Chronos.Client.Win.ViewModels
{
    public sealed class PlaceholderStaticContent : PlaceholderContent
    {
        private readonly ViewModel _viewModel;

        public PlaceholderStaticContent(ViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override string DisplayName
        {
            get { return _viewModel.DisplayName; }
        }

        public override ViewModel CreateViewModel()
        {
            return _viewModel;
        }
    }
}
