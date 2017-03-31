using System;
using Rhiannon.Threading.Internal;

namespace Rhiannon.Threading
{
	public class TaskFactory : ITaskFactory
	{
		public TaskFactory(IThreadFactory threadFactory)
		{
			ThreadFactory = threadFactory;
		}

		public IThreadFactory ThreadFactory { get; private set; }

		public ITask Create(Action prepare, Action action, Action callback, Action<Exception> exception)
		{
			return new Task(prepare, action, callback, exception, ThreadFactory);
		}
	}
}
