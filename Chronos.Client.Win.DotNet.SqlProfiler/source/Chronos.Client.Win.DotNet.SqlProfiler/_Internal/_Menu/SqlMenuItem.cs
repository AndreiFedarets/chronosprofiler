using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Chronos.Client.Win.DotNet.SqlProfiler
{
    internal class SqlMenuItem : IMenuItemAdapter
    {
        public SqlMenuItem()
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
            get { return "Sql"; }
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
