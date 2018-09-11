using Chronos.Common.EventsTree;

namespace Chronos.DotNet.TracingProfiler
{
    [PublicService(typeof(Proxy.DotNet.TracingProfiler.ThreadCreateEventMessage))]
    public interface IThreadCreateEventMessage : IEventMessage
    {
    }
}
