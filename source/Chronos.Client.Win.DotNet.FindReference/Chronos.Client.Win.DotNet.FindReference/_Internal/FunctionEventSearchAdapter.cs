using Chronos.Client.Win.Controls.Common.EventsTree;
using Chronos.Common.EventsTree;
using Chronos.DotNet.BasicProfiler;

namespace Chronos.Client.Win.DotNet.FindReference
{
    internal sealed class FunctionEventSearchAdapter : IEventSearchAdapter
    {
        private readonly FunctionInfo _functionInfo;

        public FunctionEventSearchAdapter(FunctionInfo functionInfo)
        {
            _functionInfo = functionInfo;
        }

        public bool Match(IEvent @event)
        {
            return @event.EventType == Chronos.DotNet.TracingProfiler.Constants.EventType.ManagedFunctionCall &&
                   @event.Unit == _functionInfo.Uid;
        }
    }
}
