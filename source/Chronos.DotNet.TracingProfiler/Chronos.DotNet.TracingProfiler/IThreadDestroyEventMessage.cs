using Chronos.Common.EventsTree;

namespace Chronos.DotNet.TracingProfiler
{
    [PublicService(typeof(Proxy.DotNet.TracingProfiler.ThreadDestroyEventMessage))]
    public interface IThreadDestroyEventMessage : IEventMessage
    {
    }
}
