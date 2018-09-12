using Chronos.Client.Win.Menu;

namespace Chronos.Client.Win.Controls
{
    internal static class MenuControlConverter
    {
        public static System.Windows.Controls.Control Convert(IControl viewModel)
        {
            if (viewModel is IMenuItem)
            {
                return new MenuItem((IMenuItem) viewModel);
            }
            throw new TempException(string.Format("{0} is unknown control type", viewModel));
        }
    }
}
