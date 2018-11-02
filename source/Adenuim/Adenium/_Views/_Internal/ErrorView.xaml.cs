using System.Windows;

namespace Adenium
{
    public partial class ErrorView 
    {
        public ErrorView(UIElement element)
        {
            InitializeComponent();
            ContentControl.Content = element;
        }
    }
}
