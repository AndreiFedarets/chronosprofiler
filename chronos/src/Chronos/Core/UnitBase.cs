using System;

namespace Chronos.Core
{
	[Serializable]
	public class UnitBase
	{
	    protected IProcessShadow ProcessShadow;

		public UnitBase(uint id, ulong managedId, uint beginLifetime, uint endLifetime, string fullName)
		{
			Id = id;
			ManagedId = managedId;
			BeginLifetime = beginLifetime;
			EndLifetime = endLifetime;
			Name = fullName;
		}

		public UnitBase()
            : this(0, 0, 0, 0, "<UNKNOWN>")
		{
			
		}

		public uint Id { get; set; }

		public ulong ManagedId { get; set; }

		public uint BeginLifetime { get; set; }

		public uint EndLifetime { get; set; }

	    public uint Lifetime
	    {
            get { return EndLifetime - BeginLifetime; }
	    }

		public string Name { get; set; }

		public int Revision { get; set; }

        internal void SetProcessShadow(IProcessShadow processShadow)
        {
            ProcessShadow = processShadow;
        }
	}
}
