using System;

namespace Chronos.Client.Win.ViewModels
{
    public sealed class ViewModelEventArgs : EventArgs
    {
        public ViewModelEventArgs(ViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        public ViewModel ViewModel { get; private set; }
    }
}
