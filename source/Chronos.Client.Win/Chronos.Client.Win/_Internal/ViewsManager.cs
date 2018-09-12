using Caliburn.Micro;
using System.Windows.Controls;

namespace Chronos.Client.Win
{
    internal static class ViewsManager
    {
        public static Views.View LocateViewForModel(object viewModel)
        {
            Views.View view = (Views.View)ViewLocator.LocateForModel(viewModel, null, null);
            ViewModelBinder.Bind(viewModel, view, null);
            return view;
        }

        public static ContentControl FindViewContent(Views.View view)
        {
            ContentControl contentControl = view.FindFirstChild<ContentControl>("ViewContent");
            if (contentControl == null)
            {
                contentControl = view;
            }
            return contentControl;
        }
    }
}
