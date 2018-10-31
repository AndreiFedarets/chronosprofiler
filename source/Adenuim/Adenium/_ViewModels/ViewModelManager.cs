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
    }
}
