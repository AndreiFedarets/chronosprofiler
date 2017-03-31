using Chronos.Core;

namespace Chronos.Daemon.Proxy
{
	internal class ThreadCollection : UnitCollection<ThreadInfo>, IThreadCollection
	{
        public ThreadCollection(IProcessShadow processShadow, IUnitCollection<ThreadInfo> collection)
            : base(processShadow, collection, (uint)Core.UnitType.Thread)
		{
		}
	}
}
