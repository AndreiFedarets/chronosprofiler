using Chronos.Common.EventsTree;

namespace Chronos.DotNet.SqlProfiler
{
    [PublicService(typeof(Chronos.Proxy.DotNet.SqlProfiler.SqlQueryEventMessage))]
    public interface ISqlQueryEventMessage : IEventMessage
    {
    }
}
