using System.Windows.Input;

namespace Adenium
{
    internal sealed class TabViewControlDataContext
    {
        public TabViewControlDataContext(TabViewModel viewModel)
        {
            ViewModel = viewModel;
            CloseCommand = new SyncCommand<IViewModel>(CloseViewModel);
        }

        public TabViewModel ViewModel { get; private set; }

        public ICommand CloseCommand { get; private set; }

        private void CloseViewModel(IViewModel viewModel)
        {
            ViewModel.DeactivateItem(viewModel, true);
        }
    }
}
