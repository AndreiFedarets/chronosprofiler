using System.Windows.Controls;

namespace Adenium
{
    public static class ViewsManager
    {
        public const string ViewContentPartName = "ViewContent";

        public static View LocateViewForModel(object viewModel)
        {
            View view = (View)Caliburn.Micro.ViewLocator.LocateForModel(viewModel, null, null);
            Caliburn.Micro.ViewModelBinder.Bind(viewModel, view, null);
            return view;
        }

        public static ContentControl FindViewContent(View view)
        {
            ContentControl contentControl = view.FindFirstChild<ContentControl>(ViewContentPartName);
            if (contentControl == null)
            {
                contentControl = view;
            }
            return contentControl;
        }
    }
}
