namespace Adenium
{
    public abstract class SyncCommandBase : ExtendedCommandBase
    {
        internal override void ExecuteInternal(object parameter)
        {
            Execute(parameter);
        }
    }
}
