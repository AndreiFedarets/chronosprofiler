using System;
using System.Threading;

namespace Rhiannon.Threading.Internal
{
	internal class Thread : IThread
	{
		private readonly System.Threading.Thread _thread;

		public Thread(Action action)
			: this(action, ApartmentState.MTA)
		{
		}

		public Thread(Action action, ApartmentState state)
		{
			ThreadStart @delegate = new ThreadStart(action);
			_thread = new System.Threading.Thread(@delegate);
			_thread.SetApartmentState(state);
		}

		public void Start()
		{
			_thread.Start();
		}

		public void Start(ApartmentState state)
		{
			_thread.SetApartmentState(state);
			_thread.Start();
		}

		public bool IsAlive
		{
			get { return _thread.IsAlive; }
		}

		public void Stop()
		{
			_thread.Abort();
		}
	}
}
