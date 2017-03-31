using Chronos.Core;
using Chronos.Core.Internal;

namespace Chronos.Client
{
	public class EventNameFormatter : IEventNameFormatter
	{
	    private readonly IProcessShadow _processShadow;

        public EventNameFormatter(IProcessShadow processShadow)
	    {
            _processShadow = processShadow;
	    }


        public string FormatName(byte[] eventData)
        {
            Event @event = new Event(null, eventData, 0, 0, null, null);
            return FormatName(@event);
        }

        public string FormatName(IEvent @event)
        {
            switch (@event.EventType)
			{
				case EventType.ThreadTrace:
					return ThreadTrace(@event);
                case EventType.AppDomainCreation:
                    return AppDomainCreation(@event);
                case EventType.AppDomainShutdown:
                    return AppDomainShutdown(@event);
                case EventType.AssemblyLoad:
                    return AssemblyLoad(@event);
                case EventType.AssemblyUnload:
                    return AssemblyUnload(@event);
                case EventType.ModuleLoad:
                    return ModuleLoad(@event);
                case EventType.ModuleUnload:
                    return ModuleUnload(@event);
                case EventType.ClassLoad:
                    return ClassLoad(@event);
                case EventType.ClassUnload:
                    return ClassUnload(@event);
                case EventType.ThreadCreate:
                    return ThreadCreated(@event);
                case EventType.ThreadDestroy:
                    return ThreadDestroyed(@event);
                case EventType.FunctionCall:
                    return FunctionCall(@event);
                case EventType.ExceptionThrown:
                    return ExceptionThrown(@event);
                case EventType.GarbageCollection:
					return GarbageCollection(@event);
            }
            return string.Empty;
        }

		private string ThreadTrace(IEvent @event)
		{
			ThreadInfo threadInfo = @event.ThreadInfo;
			return string.Format("#{0} {1} ({2} ms)", threadInfo.Id, threadInfo.Name, @event.Time);
		}

        public string AppDomainCreation(IEvent coreEvent)
        {
            AppDomainInfo info = _processShadow.AppDomains[coreEvent.Unit];
            return string.Format("AppDomain creation: {0} ({1} ms, {2})", info.Name, coreEvent.Time, info.LoadResult);
        }

        public string AppDomainShutdown(IEvent coreEvent)
        {
			AppDomainInfo info = _processShadow.AppDomains[coreEvent.Unit];
            return string.Format("AppDomain shutdown: {0} ({1} ms, {2})", info.Name, coreEvent.Time, info.LoadResult);
        }

        public string AssemblyLoad(IEvent coreEvent)
        {
			AssemblyInfo info = _processShadow.Assemblies[coreEvent.Unit];
            return string.Format("Assembly load: {0} ({1} ms, {2})", info.Name, coreEvent.Time, info.LoadResult);
        }

        public string AssemblyUnload(IEvent coreEvent)
        {
			AssemblyInfo info = _processShadow.Assemblies[coreEvent.Unit];
            return string.Format("Assembly unload: {0} ({1} ms, {2})", info.Name, coreEvent.Time, info.LoadResult);
        }

        public string ModuleLoad(IEvent coreEvent)
        {
			ModuleInfo info = _processShadow.Modules[coreEvent.Unit];
            return string.Format("Module load: {0} ({1} ms, {2})", info.Name, coreEvent.Time, info.LoadResult);
        }

        public string ModuleUnload(IEvent coreEvent)
        {
			ModuleInfo info = _processShadow.Modules[coreEvent.Unit];
            return string.Format("Module unload: {0} ({1} ms, {2})", info.Name, coreEvent.Time, info.LoadResult);
        }

        public string ClassLoad(IEvent coreEvent)
        {
			ClassInfo info = _processShadow.Classes[coreEvent.Unit];
            return string.Format("Class load: {0} ({1} ms, {2})", info.Name, coreEvent.Time, info.LoadResult);
        }

        public string ClassUnload(IEvent coreEvent)
        {
			ClassInfo info = _processShadow.Classes[coreEvent.Unit];
            return string.Format("Class unload: {0} ({1} ms, {2})", info.Name, coreEvent.Time, info.LoadResult);
        }

        public string ThreadCreated(IEvent coreEvent)
        {
			ThreadInfo info = _processShadow.Threads[coreEvent.Unit];
            return string.Format("Thread creation: {0}", info.Name);
        }

        public string ThreadDestroyed(IEvent coreEvent)
        {
			ThreadInfo info = _processShadow.Threads[coreEvent.Unit];
            return string.Format("Thread destroy: {0}", info.Name);
        }

        public string FunctionCall(IEvent coreEvent)
        {
			FunctionInfo functionInfo = _processShadow.Functions[coreEvent.Unit];
			ClassInfo classInfo = _processShadow.Classes[functionInfo.ClassManangedId, functionInfo.BeginLifetime];
            string className;
            className = classInfo == null ? "<UNKNOWN>" : classInfo.Name;
            return string.Format("{0}.{1} ({2} ms, {3} hits)", className, functionInfo.Name, coreEvent.Time, coreEvent.Hits);
        }

        public string ExceptionThrown(IEvent coreEvent)
        {
			ExceptionInfo info = _processShadow.Exceptions[coreEvent.Unit];
			if (info != null)
			{
				return string.Format("Exception thrown: {0} (catched: {1})", info.Name, info.IsCatched);
			}
			else
			{
				return "<UNKNOWN EXCEPTION>";
			}
        }

        public string GarbageCollection(IEvent coreEvent)
        {
            return string.Format("Garbage collection ({0} ms)", coreEvent.Time);
        }

        public string ManagedToUnmanagedTransition(IEvent coreEvent)
        {
			FunctionInfo info = _processShadow.Functions[coreEvent.Unit];
            return string.Format("Managed to unmanaged transition {0} ({1} ms)", info.Name, coreEvent.Time);
        }

        public string UnmanagedToManagedTransition(IEvent coreEvent)
        {
			FunctionInfo info = _processShadow.Functions[coreEvent.Unit];
            return string.Format("Unmanaged to managed transition {0} ({1} ms)", info.Name, coreEvent.Time);
        }
    }
}
