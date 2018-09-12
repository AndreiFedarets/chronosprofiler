using Chronos.Common.EventsTree;
using Chronos.DotNet.BasicProfiler;
using Chronos.DotNet.TracingProfiler;

namespace Chronos.Proxy.DotNet.TracingProfiler
{
    internal sealed class ThreadCreateEventMessage : ProxyBaseObject<IThreadCreateEventMessage>, IThreadCreateEventMessage
    {
        private IThreadCollection _threads;

        public ThreadCreateEventMessage(IThreadCreateEventMessage remoteObject)
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
            return Chronos.DotNet.TracingProfiler.ThreadCreateEventMessage.BuildMessageInternal(@event, _threads);
        }
    }
}
