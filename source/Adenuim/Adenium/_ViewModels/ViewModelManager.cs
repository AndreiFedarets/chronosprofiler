using Caliburn.Micro;

namespace Adenium
{
    public sealed class ViewModelManager : IViewModelManager
    {
        private readonly IWindowManager _windowsManager;

        public ViewModelManager(IWindowManager windowsManager)
        {
            _windowsManager = windowsManager;
        }

        public void ShowWindow(IViewModel viewModel)
        {
            _windowsManager.ShowWindow(viewModel);
        }

        public bool? ShowDialog(IViewModel viewModel)
        {
            return _windowsManager.ShowDialog(viewModel);
        }

        public void ShowPopup(IViewModel viewModel)
        {
            _windowsManager.ShowPopup(viewModel);
        }
    }
}
