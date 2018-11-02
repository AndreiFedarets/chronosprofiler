using Chronos.Client.Win.Controls.Common.EventsTree;
using Chronos.Common.EventsTree;
using Chronos.DotNet.BasicProfiler;

namespace Chronos.Client.Win.DotNet.FindReference
{
    internal sealed class ClassEventSearchAdapter : IEventSearchAdapter
    {
        private readonly IFunctionCollection _functions;
        private readonly ClassInfo _classInfo;

        public ClassEventSearchAdapter(ClassInfo classInfo, IFunctionCollection functions)
        {
            _classInfo = classInfo;
            _functions = functions;
        }

        public string SearchText
        {
            get { return _classInfo.Name; }
        }

        public bool Match(EventTreeItem item)
        {
            IEvent @event = item.Event;
            if (@event.EventType != Chronos.DotNet.TracingProfiler.Constants.EventType.ManagedFunctionCall)
            {
                return false;
            }
            FunctionInfo functionInfo = _functions[@event.Unit];
            if (functionInfo == null || functionInfo.Class == null)
            {
                return false;
            }
            ClassInfo classInfo = functionInfo.Class;
            if (classInfo == null)
            {
                return false;
            }
            return classInfo.Uid == _classInfo.Uid;
        }
    }
}
