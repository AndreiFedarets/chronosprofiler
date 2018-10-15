using System.Windows.Input;
using Chronos.Client.Win.Commands;

namespace Chronos.Client.Win.ViewModels
{
    public class TabItemViewModel : PlaceholderViewModel
    {
        private readonly TabViewModel _ownerViewModel;

        public TabItemViewModel(ViewModel viewModel, TabViewModel ownerViewModel)
            : base(new PlaceholderStaticContent(viewModel))
        {
            _ownerViewModel = ownerViewModel;
            CloseCommand = new SyncCommand(Close);
        }

        public ICommand CloseCommand { get; private set; }

        public void Close()
        {
            _ownerViewModel.Remove(UnderlyingViewModel);
            Dispose();
        }
    }
}
