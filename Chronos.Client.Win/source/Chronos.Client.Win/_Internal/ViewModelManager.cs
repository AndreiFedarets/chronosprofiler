using Caliburn.Micro;
using System;

namespace Chronos.Client.Win.ViewModels
{
    internal sealed class ViewModelManager : IViewModelManager
    {
        private readonly IWindowManager _windowsManager;

        public ViewModelManager(IWindowManager windowsManager)
        {
            _windowsManager = windowsManager;
            if (Current != null)
            {
                throw new TempException();
            }
            Current = this;
        }

        internal static ViewModelManager Current { get; private set; }

        public event EventHandler<ViewModelEventArgs> ViewAttached;

        public event EventHandler<ViewModelEventArgs> ViewDeattached;
        
        public void ShowWindow(ViewModel viewModel)
        {
            _windowsManager.ShowWindow(viewModel);
        }

        public bool? ShowDialog(ViewModel viewModel)
        {
            return _windowsManager.ShowDialog(viewModel);
        }

        public void ShowPopup(ViewModel viewModel)
        {
            _windowsManager.ShowPopup(viewModel);
        }

        internal void OnViewAttached(ViewModel sender, ViewModel viewModel)
        {
            EventHandler<ViewModelEventArgs> handler = ViewAttached;
            if (handler != null)
            {
                handler(sender, new ViewModelEventArgs(viewModel));
            }
        }

        internal void OnViewDeattached(ViewModel sender, ViewModel viewModel)
        {
            EventHandler<ViewModelEventArgs> handler = ViewAttached;
            if (handler != null)
            {
                handler(sender, new ViewModelEventArgs(viewModel));
            }
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
