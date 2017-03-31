namespace Rhiannon.Threading
{
	public interface IThread
	{
		bool IsAlive { get; }

		void Start();

		void Stop();
	}
}
