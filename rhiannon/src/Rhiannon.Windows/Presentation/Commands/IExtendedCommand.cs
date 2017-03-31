using System.Windows.Input;
using System;

namespace Rhiannon.Windows.Presentation.Commands
{
	public interface IExtendedCommand : ICommand, IDisposable
	{
		string Id { get; }
	}
}
