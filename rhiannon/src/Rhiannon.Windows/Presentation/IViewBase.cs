using System;
using System.Windows.Media;
using Rhiannon.Resources;
using Rhiannon.Windows.Presentation.Commands;

namespace Rhiannon.Windows.Presentation
{
	public interface IViewBase : IDisposable
	{
		string Title { get; set; }

		ImageSource Icon { get; set; }

		double Height { get; set; }

		double Width { get; set; }

		bool IsBusy { get; set; }

		IViewModelBase ViewModel { get; }

		IResourceProvider ResourceProvider { get; }

		KeyboardCommandColection KeyboardCommands { get; }

		event Action ViewLoaded;

		event Action<IViewBase> LostFocus;

		event Action CloseRequest;

		void Invoke(Action action);

		void BeginInvoke(Action action);

		void Close();

		void ApplyResources();

		string Uid { get; }
	}
}
