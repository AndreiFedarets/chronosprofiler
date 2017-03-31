using System;
using Chronos.Core;
using Chronos.Core.Internal;

namespace Chronos.Daemon.Internal
{
	internal class ProcessShadow : MarshalByRefObject, IProcessShadow
	{
	    private readonly IProfilerAgentClient _agentClient;

		public ProcessShadow(ICallstackLoader callstackLoader,IProfilerAgentClient agentClient)
		{
			AppDomains = new AppDomainCollection();
			Assemblies = new AssemblyCollection();
			Modules = new ModuleCollection();
			Classes = new ClassCollection();
			Functions = new FunctionCollection();
			Exceptions = new ExceptionCollection();
			Threads = new ThreadCollection();
            SqlRequests = new SqlRequestCollection();
			Callstacks = new CallstackCollection();
			PerformanceCounters = new PerformanceCounterCollection();
			CallstackLoader = callstackLoader;
			ThreadTraces = new ThreadTraceCollection(Threads, Callstacks, CallstackLoader);
			ReferencesAnalyzer = new ReferencesAnalyzer(this);
		    _agentClient = agentClient;
		}

		public IAppDomainCollection AppDomains { get; private set; }

		public IAssemblyCollection Assemblies { get; private set; }

		public IModuleCollection Modules { get; private set; }

		public IClassCollection Classes { get; private set; }

		public IFunctionCollection Functions { get; private set; }

		public IExceptionCollection Exceptions { get; private set; }

		public IThreadCollection Threads { get; private set; }

        public ISqlRequestCollection SqlRequests { get; private set; }

		public ICallstackCollection Callstacks { get; private set; }

		public ProcessInfo ProcessInfo { get; set; }

		public IPerformanceCounterCollection PerformanceCounters { get; private set; }

		public ICallstackLoader CallstackLoader { get; private set; }

		public IThreadTraceCollection ThreadTraces { get; private set; }

		public IReferencesAnalyzer ReferencesAnalyzer { get; private set; }

		public override object InitializeLifetimeService()
		{
			return null;
		}

        public void ReloadUnits()
        {
            const UnitType unitType = UnitType.AppDomain | UnitType.Assembly | UnitType.Module | UnitType.Class |
                                      UnitType.Function | UnitType.Thread | UnitType.Exception | UnitType.SqlRequest;
            _agentClient.FlushUnits(unitType);
        }

        public void ReloadAll()
        {
            ReloadUnits();
            ThreadTraces.Reload();
        }
    }
}
