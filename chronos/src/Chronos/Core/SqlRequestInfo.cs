using System;

namespace Chronos.Core
{
	[Serializable]
	public sealed class SqlRequestInfo : UnitBase
	{
		public SqlRequestInfo(uint id, ulong managedId, uint beginLifetime, uint endLifetime, string fullName)
			: base(id, managedId, beginLifetime, endLifetime, fullName)
		{
		}

	    public SqlRequestInfo()
	    {
	        
	    }

        public uint Duration { get; set; }

	}
}
