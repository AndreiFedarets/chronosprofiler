using System.Windows;
using Rhiannon.Windows.Presentation;

namespace Rhiannon.Windows.Views.MessageBox
{
	public interface IView : IViewBase
    {
        MessageBoxImage Image { get; set; }

        string Message { get; set; }
	}
}
