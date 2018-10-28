using System;
using Adenium.Layouting;

namespace Adenium.Controls
{
    internal static class MenuControlConverter
    {
        public static System.Windows.Controls.Control Convert(IMenuControl viewModel)
        {
            if (viewModel is Layouting.MenuItem)
            {
                return new MenuItem((Layouting.MenuItem)viewModel);
            }
            throw new Exception(string.Format("{0} is unknown control type", viewModel));
        }
    }
}
