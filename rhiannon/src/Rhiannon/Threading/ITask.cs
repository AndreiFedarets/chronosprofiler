using System.Threading;

namespace Rhiannon.Threading
{
	public interface ITask
	{
		bool Executing { get; }

		void Start();

		void Start(ApartmentState state);

		void Stop(bool throwIfStopped);
	}
}
