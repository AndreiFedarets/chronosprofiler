namespace Adenium
{
    public interface IWindowsManager
    {
        void ShowWindow(IViewModel viewModel);

        bool? ShowDialog(IViewModel viewModel);

        void ShowPopup(IViewModel viewModel);
    }
}
