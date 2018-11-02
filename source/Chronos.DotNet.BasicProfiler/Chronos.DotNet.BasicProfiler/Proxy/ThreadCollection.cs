using Chronos.Common;
using Chronos.Common.Proxy;
using Chronos.Proxy;

namespace Chronos.DotNet.BasicProfiler.Proxy
{
    internal sealed class ThreadCollection : UnitCollectionProxyBase<ThreadInfo>, IThreadCollection
    {
        public ThreadCollection(IUnitCollection<ThreadInfo> remoteObject)
            : base(remoteObject)
        {
        }

        [ServiceProxyInitializationMethod]
        public void SetDependencies()
        {
            foreach (ThreadInfo unit in this)
            {
                unit.SetDependencies();
            }
        }

        protected override ThreadInfo Convert(ThreadInfo unit)
        {
            unit.SetDependencies();
            return unit;
        }
    }
}
