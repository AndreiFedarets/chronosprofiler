using Chronos.Common.EventsTree;
using Chronos.DotNet.BasicProfiler;
using Chronos.DotNet.TracingProfiler;

namespace Chronos.Proxy.DotNet.TracingProfiler
{
    internal sealed class ThreadDestroyEventMessage : ProxyBaseObject<IThreadDestroyEventMessage>, IThreadDestroyEventMessage
    {
        private IThreadCollection _threads;

        public ThreadDestroyEventMessage(IThreadDestroyEventMessage remoteObject)
            : base(remoteObject)
        {
        }

        [ServiceProxyInitializationMethod]
        public void SetDependencies(IThreadCollection threads)
        {
            _threads = threads;
        }

        public string BuildMessage(IEvent @event)
        {
            return Chronos.DotNet.TracingProfiler.ThreadDestroyEventMessage.BuildMessageInternal(@event, _threads);
        }
    }
}
