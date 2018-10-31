using System.Windows.Input;

namespace Adenium.ViewModels
{
    public sealed class TabItemViewModel : GridViewModel
    {
        public TabItemViewModel(IViewModel viewModel)
        {
            MainViewModel = viewModel;
            CloseCommand = new SyncCommand(Close);
        }

        public IViewModel MainViewModel { get; private set; }

        public override string DisplayName
        {
            get { return MainViewModel.DisplayName; }
        }

        public ICommand CloseCommand { get; private set; }

        public void ActivateMainViewModel()
        {
            ActivateItem(MainViewModel);
        }

        public void DeactivateMainViewModel()
        {
            DeactivateItem(MainViewModel, true);
        }

        public void Close()
        {
            Parent.DeactivateItem(MainViewModel, true);
        }
    }
}
