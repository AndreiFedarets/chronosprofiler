using System;
using System.ComponentModel;
using System.Drawing;

namespace Chronos.Client.Win.DotNet.TracingProfiler
{
    internal class PerformanceMenuItem : IMenuItemAdapter
    {
        public PerformanceMenuItem()
        {
            Uid = Guid.NewGuid();
        }

        public Bitmap Icon
        {
            get { return null; }
        }

        public bool IsEnabled
        {
            get { return true; }
        }

        public bool IsVisible
        {
            get { return true; }
        }

        public string Text
        {
            get { return "Performance"; }
        }

        public Guid Uid { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void Execute()
        {
            
        }

        public void OnViewModelAttached(object viewModel)
        {
            
        }
    }
}
