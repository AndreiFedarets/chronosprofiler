namespace Rhiannon.Windows.Presentation.Commands
{
	public abstract class SyncCommandBase : ExtendedCommandBase
	{
		internal override void ExecuteInternal(object parameter, IViewBase view)
		{
			Execute(parameter);
		}
	}
}
