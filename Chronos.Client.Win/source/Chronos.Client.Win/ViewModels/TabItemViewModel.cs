using Chronos.Client.Win.Commands;
using System.Windows.Input;

namespace Chronos.Client.Win.ViewModels
{
    public class TabItemViewModel : GridViewModel
    {
        private readonly ViewModel _mainViewModel;
        private readonly TabViewModel _ownerViewModel;

        public TabItemViewModel(ViewModel viewModel, TabViewModel ownerViewModel)
        {
            _mainViewModel = viewModel;
            _ownerViewModel = ownerViewModel;
            CloseCommand = new SyncCommand(Close);
            Add(_mainViewModel);
        }

        public ICommand CloseCommand { get; private set; }

        internal ViewModel MainViewModel
        {
            get { return _mainViewModel; }
        }

        public override string DisplayName
        {
            get { return _mainViewModel.DisplayName; }
        }

        protected override void BuildLayout()
        {

        }

        public void Close()
        {
            _ownerViewModel.Remove(MainViewModel);
            Dispose();
        }
    }
}
