using System.Collections.Generic;
using System.Linq;
using Chronos.Core;

namespace Chronos.Daemon.Proxy
{
	internal class CallstackCollection : UnitCollection<CallstackInfo>, ICallstackCollection
	{
        public CallstackCollection(IProcessShadow processShadow, IUnitCollection<CallstackInfo> collection)
            : base(processShadow, collection, (uint)Core.UnitType.Callstack)
		{
		}

		public uint TotalTime
		{
			get { return (uint)this.Sum(x => x.EndLifetime - x.BeginLifetime); }
		}

		public CallstackInfo[] ThreadCallstacks(uint threadId)
		{
		    List<CallstackInfo> callstacks = Snapshot();
            return callstacks.Where(x => x.ThreadId == threadId).ToArray();
		}
	}
}
