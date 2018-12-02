using Chronos.Common.EventsTree;
using Chronos.DotNet.SqlProfiler;

namespace Chronos.Proxy.DotNet.SqlProfiler
{
    internal sealed class SqlQueryEventMessage : ProxyBaseObject<ISqlQueryEventMessage>, ISqlQueryEventMessage
    {
        private ISqlQueryCollection _queries;

        public SqlQueryEventMessage(ISqlQueryEventMessage remoteObject)
            : base(remoteObject)
        {
        }

        [ServiceProxyInitializationMethod]
        public void SetDependencies(ISqlQueryCollection queries)
        {
            _queries = queries;
        }

        public string BuildMessage(IEvent @event)
        {
            return Chronos.DotNet.SqlProfiler.SqlQueryEventMessage.BuildMessageInternal(@event, _queries);
        }
    }
}
