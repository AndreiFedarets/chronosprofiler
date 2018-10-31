using System.Windows;
using System.Windows.Controls;
using Adenium.ViewModels;

namespace Adenium
{
    public sealed class PlaceholderViewBehaviorExtension : IViewBehaviorExtension
    {
        private readonly View _view;
        private readonly PlaceholderViewModel _viewModel;

        public PlaceholderViewBehaviorExtension(View view, PlaceholderViewModel viewModel)
        {
            _view = view;
            _viewModel = viewModel;
        }

        public void Initialize()
        {
            _viewModel.UnderlyingViewModelChanged += OnUnderlyingViewModelChanged;
            BuildLayout();
        }

        private void OnUnderlyingViewModelChanged(object sender, System.EventArgs e)
        {
            
        }

        public void Dispose()
        {
            _viewModel.UnderlyingViewModelChanged -= OnUnderlyingViewModelChanged;
            ClearLayout();
        }

        private void ClearLayout()
        {
            ContentControl contentControl = ViewsManager.FindViewContent(_view);
            if (contentControl.Content == null)
            {
                return;
            }
            TabControl control = (TabControl)contentControl.Content;
            contentControl.Content = null;
            control.Items.Clear();
        }

        private void BuildLayout()
        {
            _view.Content = null;
            IViewModel underlyingViewModel = _viewModel.UnderlyingViewModel;
            if (underlyingViewModel == null)
            {
                return;
            }
            View view = ViewsManager.LocateViewForModel(underlyingViewModel);
            if (view != null)
            {
                _view.DisplayPanel = view.DisplayPanel;
                view.DisplayPanel = false;
                view.Margin = new Thickness();
                view.Padding = new Thickness();
                view.Style = null;
                _view.Content = view;
                CloneViewProperties(view);
            }
        }

        private void CloneViewProperties(View view)
        {
            if (!view.ContentMinHeight.IsNaNOrZero())
            {
                _view.ContentMinHeight = view.ContentMinHeight;
            }
            if (!view.ContentHeight.IsNaNOrZero())
            {
                _view.ContentHeight = view.ContentHeight;
            }
            if (!view.ContentMaxHeight.IsNaNOrZero())
            {
                _view.ContentMaxHeight = view.ContentMaxHeight;
            }

            if (!view.ContentMinWidth.IsNaNOrZero())
            {
                _view.ContentMinWidth = view.ContentMinWidth;
            }
            if (!view.ContentWidth.IsNaNOrZero())
            {
                _view.ContentWidth = view.ContentWidth;
            }
            if (!view.ContentMaxWidth.IsNaNOrZero())
            {
                _view.ContentMaxWidth = view.ContentMaxWidth;
            }
        }
    }
}
