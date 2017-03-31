using System;

namespace Rhiannon.Threading
{
	public interface ITaskFactory
	{
		IThreadFactory ThreadFactory { get; }

		ITask Create(Action prepare, Action action, Action callback, Action<Exception> exception);
	}
}
