using Chronos.Core;
using Chronos.Core.Internal;

namespace Chronos.Daemon.Proxy
{
    internal class ProcessShadow : IProcessShadow
    {
        private readonly IProcessShadow _processShadow;

        public ProcessShadow(IProcessShadow processShadow)
        {
            _processShadow = processShadow;
            AppDomains = new AppDomainCollection(this, processShadow.AppDomains);
            Assemblies = new AssemblyCollection(this, processShadow.Assemblies);
            Modules = new ModuleCollection(this, processShadow.Modules);
            Classes = new ClassCollection(this, processShadow.Classes);
            Functions = new FunctionCollection(this, processShadow.Functions);
            Exceptions = new ExceptionCollection(this, processShadow.Exceptions);
            Threads = new ThreadCollection(this, processShadow.Threads);
            SqlRequests = new SqlRequestCollection(this, processShadow.SqlRequests);
            Callstacks = new CallstackCollection(this, processShadow.Callstacks);
            CallstackLoader = new CallstackLoader(processShadow.CallstackLoader);
            ThreadTraces = new ThreadTraceCollection(Threads, Callstacks, CallstackLoader);
            ReferencesAnalyzer = new ReferencesAnalyzer(this);
        }

        public ProcessInfo ProcessInfo
        {
            get { return _processShadow.ProcessInfo; }
            set { _processShadow.ProcessInfo = value; }
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

        public IPerformanceCounterCollection PerformanceCounters { get; private set; }

        public ICallstackLoader CallstackLoader { get; private set; }

        public IThreadTraceCollection ThreadTraces { get; private set; }

        public IReferencesAnalyzer ReferencesAnalyzer { get; private set; }

        public void ReloadAll()
        {
            ReloadUnits();
            ThreadTraces.Reload();
        }

        public void ReloadUnits()
        {
            _processShadow.ReloadUnits();
        }
    }
}
