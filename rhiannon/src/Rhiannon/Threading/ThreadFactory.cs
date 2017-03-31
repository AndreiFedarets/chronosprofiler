using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Threading;
using Thread = Rhiannon.Threading.Internal.Thread;

namespace Rhiannon.Threading
{
	public class ThreadFactory : IThreadFactory
	{
		private readonly Dispatcher _dispatcher;
		private readonly IList<IThread> _threads;

		public ThreadFactory(Dispatcher dispatcher)
		{
			_dispatcher = dispatcher;
			_threads = new List<IThread>();
		}

		public IThread Create(Action action)
		{
			IThread thread = new Thread(action);
			_threads.Add(thread);
			return thread;
		}

		public IThread Create(Action action, ApartmentState state)
		{
			IThread thread = new Thread(action, state);
			_threads.Add(thread);
			return thread;
		}

		public void Invoke(Action action)
		{
			ThreadStart @delegate = new ThreadStart(action);
			_dispatcher.Invoke(@delegate);
		}

		public void BeginInvoke(Action action)
		{
			ThreadStart @delegate = new ThreadStart(action);
			_dispatcher.BeginInvoke(@delegate);
		}

		public T Invoke<T>(Func<T> func)
		{
			return (T)_dispatcher.Invoke(func);
		}


		public void CloseAll()
		{
			foreach (IThread thread in _threads)
			{
				thread.Stop();
			}
		}
	}
}
