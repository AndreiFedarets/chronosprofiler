using System;
using Chronos.Configuration;
using Chronos.Core;
using Chronos.Storage;

namespace Chronos.Daemon.Internal
{
    internal class ProfilingServer : IProfilingServer
    {
        private readonly IProcessShadow _processShadow;
        private readonly ISessionStorage _sessionStorage;
        private readonly Guid _sessionToken;
        private readonly IProcessor _processor;
        private readonly ICommonConfiguration _configuration;
        private readonly IProfilerAgentClient _agentClient;
        private ProcessPerformanceWatcher _performanceWatcher;
        private AgentProcessManager _agentProcessManager;
        private ThreadStreamPool _threadStreamPool;
        private UnitStreamPool _unitStreamPool;
        private IThreadStreamProcessor _threadStreamProcessor;

        public ProfilingServer(IProcessShadow processShadow, ISessionStorage sessionStorage, IProfilerAgentClient agentClient, Guid sessionToken, ICommonConfiguration configuration)
        {
            _processShadow = processShadow;
            _sessionToken = sessionToken;
            _sessionStorage = sessionStorage;
            _processor = new Processor();
            _configuration = configuration;
            _agentClient = agentClient;
            IsLaunched = false;
        }

        public bool IsLaunched { get; private set; }

        public AgentState State
        {
            get
            {
                if (IsLaunched)
                {
                    return _agentClient.GetState();
                }
                return AgentState.Closed;
            }
            set
            {
                if (IsLaunched)
                {
                    if (value == AgentState.Closed)
                    {
                        _agentProcessManager.Close();
                    }
                    else
                    {
                        _agentClient.SetState(value);   
                    }
                }
            }
        }

        public void Start(AgentProcessManager clientProcessManager)
        {
            if (IsLaunched)
            {
                throw new InvalidOperationException();
            }
            _agentProcessManager = clientProcessManager;
            _processShadow.ProcessInfo = _agentProcessManager.ProcessInfo;
            _threadStreamProcessor = new NativeThreadStreamProcessor(_processShadow.Callstacks, _processShadow.Functions, _processor, _sessionStorage, _configuration.Daemon.ThreadMergersCount);
            _threadStreamPool = new ThreadStreamPool(_sessionToken, _threadStreamProcessor, _configuration.Daemon.CallPageSize, _configuration.Daemon.ThreadStreamsPool);
            _unitStreamPool = new UnitStreamPool(_sessionToken, _processShadow);
            _performanceWatcher = new ProcessPerformanceWatcher(_agentProcessManager.ProcessInfo.ProcessId, _configuration.Daemon.PerformanceSamplingPeriod, _processShadow.PerformanceCounters);
            _agentClient.Initialize(_agentProcessManager);
            IsLaunched = true;
        }

        public void Dispose()
        {
            if (IsLaunched)
            {
                _processShadow.ProcessInfo = _agentProcessManager.ProcessInfo;
                _threadStreamPool.Dispose();
                _unitStreamPool.Dispose();
                _agentProcessManager.Dispose();
                _performanceWatcher.Dispose();
                _threadStreamProcessor.Dispose();
                IsLaunched = false;
            }
        }
    }
}