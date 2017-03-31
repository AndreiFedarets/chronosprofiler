using System.Threading;
using Rhiannon.Extensions;
using Rhiannon.Threading;

namespace Rhiannon.Windows.Presentation.Commands
{
	public abstract class AsyncCommandBase : ExtendedCommandBase
	{
		protected AsyncCommandBase(ITaskFactory taskFactory)
		{
			TaskFactory = taskFactory;
		}

        protected AsyncCommandBase()
        {
            
        }

		protected ITaskFactory TaskFactory { get; private set; }

		internal sealed override void ExecuteInternal(object parameter, IViewBase view)
		{
			ITask task = TaskFactory.Create
			(
				() =>
				{
					if (view != null)
					{
						view.IsBusy = true;
					}
				},
				() => Execute(parameter),
				() =>
				{
					if (view != null)
					{
						view.IsBusy = false;
					}
				},
				e => { }
            );
			task.Start(ApartmentState.STA);
		}
	}
}
