using System;
using Adenium.Menu;

namespace Adenium.Controls
{
    internal static class MenuControlConverter
    {
        public static System.Windows.Controls.Control Convert(IMenuControl viewModel)
        {
            if (viewModel is IMenuItem)
            {
                return new MenuItem((IMenuItem) viewModel);
            }
            throw new Exception(string.Format("{0} is unknown control type", viewModel));
        }
    }
}
