namespace Chronos.Core
{
	public interface ICallstackCollection : IUnitCollection<CallstackInfo>
	{
		uint TotalTime { get; }

		CallstackInfo[] ThreadCallstacks(uint threadId);
	}
}
