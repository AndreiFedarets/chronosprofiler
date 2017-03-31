using Chronos.Common.EventsTree;
using Chronos.DotNet.BasicProfiler;
using Chronos.DotNet.TracingProfiler;

namespace Chronos.Proxy.DotNet.TracingProfiler
{
    internal sealed class ManagedFunctionCallEventMessage : ProxyBaseObject<IManagedFunctionCallEventMessage>, IManagedFunctionCallEventMessage
    {
        private IFunctionCollection _functions;

        public ManagedFunctionCallEventMessage(IManagedFunctionCallEventMessage remoteObject)
            : base(remoteObject)
        {
        }

        [ServiceProxyInitializationMethod]
        public void SetDependencies(IFunctionCollection functions)
        {
            _functions = functions;
        }

        public string BuildMessage(IEvent @event)
        {
            return Chronos.DotNet.TracingProfiler.ManagedFunctionCallEventMessage.BuildMessageInternal(@event, _functions);
        }
    }
}
