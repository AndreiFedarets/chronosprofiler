using System.ComponentModel;
using System;
using Rhiannon.Windows.Presentation.Commands;

namespace Rhiannon.Windows.Presentation
{
	public interface IViewModelBase : INotifyPropertyChanged, IDisposable
	{
		IViewBase View { get; }

		void ForceNotify();

		void Initialize(IViewBase view);

		void InitializeKeyboardCommands(KeyboardCommandColection keyboardCommands);
	}
}
