using System;
using System.Threading;
using Rhiannon.Logging;

namespace Rhiannon.Threading.Internal
{
	internal class Task : ITask
	{
		private readonly Action _action;
		private readonly Action _prepare;
		private readonly Action _callback;
		private readonly Action<Exception> _exception;
		private readonly IThreadFactory _threadFactory;
		private IThread _thread;
		private bool _executing;

		public Task(Action prepare, Action action, Action callback, Action<Exception> exception, IThreadFactory threadFactory)
		{
			_prepare = prepare;
			_action = action;
			_callback = callback;
			_exception = exception;
			_threadFactory = threadFactory;
		}

		public bool Executing
		{
			get { return _executing; }
		}

		public void Start()
		{
			Start(ApartmentState.MTA);
		}

		public void Start(ApartmentState state)
		{
			GetThread(state).Start();
		}

		private void Execute()
		{
			try
			{
				_executing = true;
				if (_prepare != null)
				{
					_prepare();
				}
				if (_action != null)
				{
					_action();
				}
			}
			catch(ThreadAbortException)
			{
					
			}
			catch (Exception exception)
			{
				if (_exception != null)
				{
					_exception(exception);
				}
				else
				{
					LoggingProvider.Current.Log(exception, Policy.Presentation);
				}
			}
			finally
			{
				if (_callback != null)
				{
					_callback();
				}
				_executing = false;
			}
		}

		public void Stop(bool throwIfStopped)
		{
			if (_thread != null && _thread.IsAlive)
			{
				_thread.Stop();
				_thread = null;	
			}
			else
			{
				if (throwIfStopped)
				{
					throw new InvalidOperationException();	
				}
			}
		}

		private IThread GetThread(ApartmentState state)
		{
			return _thread ?? (_thread = _threadFactory.Create(Execute, state));
		}
	}
}
