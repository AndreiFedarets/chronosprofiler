using Chronos.Common.EventsTree;
using Chronos.DotNet.BasicProfiler;

namespace Chronos.DotNet.TracingProfiler
{
    internal sealed class ManagedFunctionCallEventMessage : RemoteBaseObject, IManagedFunctionCallEventMessage 
    {
        private readonly IFunctionCollection _functions;

        public ManagedFunctionCallEventMessage(IFunctionCollection functions)
        {
            _functions = functions;
        }

        public string BuildMessage(IEvent @event)
        {
            return BuildMessageInternal(@event, _functions);
        }

        internal static string BuildMessageInternal(IEvent @event, IFunctionCollection functions)
        {
            FunctionInfo functionInfo = functions[@event.Unit];
            if (functionInfo == null)
            {
                return "<UNKNOWN FUNCTION CALL>";
            }
            return functionInfo.FullName;
        }
    }
}
