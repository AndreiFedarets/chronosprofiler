using Chronos.Client.Win.Controls.Common.EventsTree;
using Chronos.Common.EventsTree;
using Chronos.DotNet.BasicProfiler;

namespace Chronos.Client.Win.DotNet.FindReference
{
    internal sealed class ModuleEventSearchAdapter : IEventSearchAdapter
    {
        private readonly IFunctionCollection _functions;
        private readonly ModuleInfo _moduleInfo;

        public ModuleEventSearchAdapter(ModuleInfo moduleInfo, IFunctionCollection functions)
        {
            _moduleInfo = moduleInfo;
            _functions = functions;
        }

        public string SearchText
        {
            get { return _moduleInfo.Name; }
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
            ModuleInfo moduleInfo = classInfo.Module;
            if (moduleInfo == null)
            {
                return false;
            }
            return moduleInfo.Uid == _moduleInfo.Uid;
        }
    }
}
