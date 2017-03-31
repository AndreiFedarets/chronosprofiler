using System;

namespace Chronos.Core
{
	[Serializable]
	public sealed class ThreadInfo : UnitBase
	{
		public ThreadInfo(uint id, ulong managedId, uint beginLifetime, uint endLifetime, string fullName, int osThreadId)
			: base(id, managedId, beginLifetime, endLifetime, fullName)
		{
			OSThreadId = osThreadId;
		}

		public ThreadInfo()
		{
		    OSThreadId = 0;
		}

		public int OSThreadId { get; set; }
	}
}
