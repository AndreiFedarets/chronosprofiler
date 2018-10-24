using System;

namespace Adenium
{
    public sealed class ViewModelEventArgs : EventArgs
    {
        public ViewModelEventArgs(IViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        public IViewModel ViewModel { get; private set; }

        internal static void RaiseEvent(EventHandler<ViewModelEventArgs> handler, object sender, ViewModelEventArgs eventArgs)
        {
            if (handler != null)
            {
                handler(sender, eventArgs);
            }
        }

        internal static void RaiseEvent(EventHandler<ViewModelEventArgs> handler, object sender, IViewModel viewModel)
        {
            if (handler != null)
            {
                handler(sender, new ViewModelEventArgs(viewModel));
            }
        }
    }
}
