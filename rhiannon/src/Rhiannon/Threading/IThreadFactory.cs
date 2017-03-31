using System;
using System.Threading;

namespace Rhiannon.Threading
{
	public interface IThreadFactory
	{
		IThread Create(Action action);

		IThread Create(Action action, ApartmentState apartmentState);

		void Invoke(Action action);

		void BeginInvoke(Action action);

		T Invoke<T>(Func<T> func);

		void CloseAll();
	}
}
