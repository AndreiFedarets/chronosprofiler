using System.ComponentModel;
using System.Windows;

namespace Rhiannon.Windows.Presentation
{
	public interface IWindow
	{
		WindowState WindowState { get; set; }

		object Content { get; set; }

		bool CanMaximize { get; set; }

		bool CanMinimize { get; set; }

		WindowStartupLocation WindowStartupLocation { get; set; }

		void Open();

		bool? OpenDialog();

		IViewBase View { get; }

		bool? DialogResult { get; }

		T GetViewAs<T>() where T : IViewBase;

		double Height { get; set; }

		double Width { get; set; }

		double MinHeight { get; set; }

		double MinWidth { get; set; }

		//WindowStyle WindowStyle { get; set; }

		//ResizeMode ResizeMode { get; set; }

		bool Topmost { get; set; }

		event CancelEventHandler Closing;
	}
}
