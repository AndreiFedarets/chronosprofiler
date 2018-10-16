using Caliburn.Micro;

namespace Chronos.Client.Win.ViewModels
{
    internal sealed class ViewModelManager : IViewModelManager
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

        //public static class Store
        //{
        //    private static readonly Dictionary<Guid, IViewModelActivator> Activators;

        //    static Store()
        //    {
        //        Activators = new Dictionary<Guid, IViewModelActivator>();
        //    }

        //    public static void Register(IViewModelActivator activator)
        //    {
        //        lock (Activators)
        //        {
        //            Guid viewModelTypeId = activator.ViewModelTypeId;
        //            if (Activators.ContainsKey(activator.ViewModelTypeId))
        //            {
        //                throw new TempException();
        //            }
        //            Activators.Add(viewModelTypeId, activator);
        //        }
        //    }

        //    internal static IViewModelActivator Resolve(Guid typeId)
        //    {
        //        lock (Activators)
        //        {
        //            IViewModelActivator activator;
        //            if (!Activators.TryGetValue(typeId, out activator))
        //            {
        //                throw new TempException();
        //            }
        //            return activator;
        //        }
        //    }
        //}
    }
}
