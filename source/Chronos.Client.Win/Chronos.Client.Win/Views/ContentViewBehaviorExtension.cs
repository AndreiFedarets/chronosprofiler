using Chronos.Client.Win.ViewModels;

namespace Chronos.Client.Win.Views
{
    public sealed class ContentViewBehaviorExtension : IViewBehaviorExtension
    {
        private readonly View _view;
        private readonly ViewModel _viewModel;

        public ContentViewBehaviorExtension(View view, ViewModel viewModel)
        {
            _view = view;
            _viewModel = viewModel;
        }

        public void Initialize()
        {
            
        }

        public void Dispose()
        {
            
        }
    }
}
