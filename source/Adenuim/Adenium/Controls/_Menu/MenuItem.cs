using System.Windows;

namespace Adenium.Controls
{
    internal sealed class MenuItem : System.Windows.Controls.MenuItem
    {
        private readonly Layouting.MenuItem _viewModel;

        static MenuItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MenuItem), new FrameworkPropertyMetadata(typeof(System.Windows.Controls.MenuItem)));
        }

        internal MenuItem(Layouting.MenuItem viewModel)
        {
            _viewModel = viewModel;
            BindViewModel();
        }

        private void BindViewModel()
        {
            Header = _viewModel.Text;
        }

        protected override void OnClick()
        {
            if (Items.IsEmpty)
            {
                _viewModel.OnAction();
            }
            base.OnClick();
        }
    }
}
