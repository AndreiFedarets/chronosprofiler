using System.Windows;
using System.Windows.Controls;
using Adenium.Controls;
using Adenium.Layouting;
using Caliburn.Micro;

namespace Adenium
{
    public sealed class CustomWindowManager : WindowManager, IWindowsManager
    {
        public CustomWindowManager()
        {
            UseCustomWindow = false;
        }

        private bool UseCustomWindow { get; set; }

        protected override Window EnsureWindow(object model, object view, bool isDialog)
        {
            Window window = view as Window;
            if (window == null)
            {
                window = InstanceWindow();
                window.Content = view;
                window.SizeToContent = SizeToContent.Manual;
                window.SetValue(Caliburn.Micro.View.IsGeneratedProperty, true);
                CloneSize(window, view);
                Window window2 = InferOwnerOf(window);
                if (window2 != null)
                {
                    window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                    window.Owner = window2;
                }
                else
                {
                    window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                }
            }
            else
            {
                Window window3 = InferOwnerOf(window);
                if (window3 != null && isDialog)
                {
                    window.Owner = window3;
                }
            }
            return window;
        }

        private void CloneSize(Window window, object view)
        {
            Control viewControl = view as Control;
            if (viewControl == null)
            {
                return;
            }
            Thickness padding = (Thickness)viewControl.GetValue(Control.PaddingProperty);
            Thickness margin = viewControl.Margin;
            double extraHeight = margin.Top + margin.Bottom + padding.Top + padding.Bottom + SystemParameters.WindowCaptionHeight + SystemParameters.ResizeFrameHorizontalBorderHeight * 2;
            double extraWidth = margin.Left + margin.Right + padding.Left + padding.Right + SystemParameters.ResizeFrameVerticalBorderWidth * 2;
            if (!double.IsNaN(viewControl.MinHeight) && !double.IsInfinity(viewControl.MinHeight))
            {
                window.MinHeight = viewControl.MinHeight + extraHeight;
                window.Height = window.MinHeight;
            }
            if (!double.IsNaN(viewControl.MinWidth) && !double.IsInfinity(viewControl.MinWidth))
            {
                window.MinWidth = viewControl.MinWidth + extraWidth;
                window.Width = window.MinWidth;
            }
            if (!double.IsNaN(viewControl.MaxHeight) && !double.IsInfinity(viewControl.MaxHeight))
            {
                window.MaxHeight = viewControl.MaxHeight = extraHeight;
            }
            if (!double.IsNaN(viewControl.MaxWidth) && !double.IsInfinity(viewControl.MaxWidth))
            {
                window.MaxWidth = viewControl.MaxWidth + extraWidth;
            }
            if (!double.IsNaN(viewControl.Height) && !double.IsInfinity(viewControl.Height))
            {
                window.Height = viewControl.Height + extraHeight;
            }
            if (!double.IsNaN(viewControl.Width) && !double.IsInfinity(viewControl.Width))
            {
                window.Width = viewControl.Width + extraWidth;
            }
        }

        private Window InstanceWindow()
        {
            Window window;
            if (UseCustomWindow)
            {
                window = new CustomWindow();
            }
            else
            {
                window = new Window();
            }
            return window;
        }

        public void ShowWindow(IViewModel viewModel)
        {
            ViewModelBuilder.Build(viewModel);
            base.ShowWindow(viewModel);
        }

        public bool? ShowDialog(IViewModel viewModel)
        {
            ViewModelBuilder.Build(viewModel);
            return base.ShowDialog(viewModel);
        }

        public void ShowPopup(IViewModel viewModel)
        {
            ViewModelBuilder.Build(viewModel);
            base.ShowPopup(viewModel);
        }
    }
}
