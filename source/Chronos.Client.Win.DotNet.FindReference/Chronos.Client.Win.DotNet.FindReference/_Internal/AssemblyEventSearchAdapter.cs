using Chronos.Client.Win.Controls.Common.EventsTree;
using Chronos.Common.EventsTree;
using Chronos.DotNet.BasicProfiler;

namespace Chronos.Client.Win.DotNet.FindReference
{
    internal sealed class AssemblyEventSearchAdapter : IEventSearchAdapter
    {
        private readonly IFunctionCollection _functions;
        private readonly AssemblyInfo _assemblyInfo;

        public AssemblyEventSearchAdapter(AssemblyInfo assemblyInfo, IFunctionCollection functions)
        {
            _assemblyInfo = assemblyInfo;
            _functions = functions;
        }

        public string SearchText
        {
            get { return _assemblyInfo.Name; }
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
            AssemblyInfo assemblyInfo = classInfo.Assembly;
            if (assemblyInfo == null)
            {
                return false;
            }
            return assemblyInfo.Uid == _assemblyInfo.Uid;
        }
    }
}
