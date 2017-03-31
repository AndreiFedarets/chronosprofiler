namespace Chronos.Core
{
	public interface IProcessShadow
	{
		ProcessInfo ProcessInfo { get; set; }

		IAppDomainCollection AppDomains { get; }

		IAssemblyCollection Assemblies { get; }

		IModuleCollection Modules { get; }

		IClassCollection Classes { get; }

		IFunctionCollection Functions { get; }

		IExceptionCollection Exceptions { get; }

		IThreadCollection Threads { get; }

        ISqlRequestCollection SqlRequests { get; }

		ICallstackCollection Callstacks { get; }

		IPerformanceCounterCollection PerformanceCounters { get; }

		ICallstackLoader CallstackLoader { get; }

		IThreadTraceCollection ThreadTraces { get; }

		IReferencesAnalyzer ReferencesAnalyzer { get; }

	    void ReloadUnits();

        void ReloadAll();
    }
}
