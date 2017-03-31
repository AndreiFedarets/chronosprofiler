namespace Chronos.Core
{
	public enum SessionState : byte
	{
		Closed = 0,
		Profiling = 1,
		Paused = 2,
		Decoding = 3,
	}
}
