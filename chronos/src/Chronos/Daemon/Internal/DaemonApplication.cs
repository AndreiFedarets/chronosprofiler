using System;
using System.Diagnostics;
using Chronos.Communication.Remoting;
using Chronos.Configuration;
using Chronos.Core;
using Chronos.CustomMarshalers;
using Rhiannon.Extensions;
using Chronos.Storage;

namespace Chronos.Daemon.Internal
{
    internal class DaemonApplication : MarshalByRefObject, IDaemonApplication, IDisposable
    {
        private readonly EventActionsHolder _stateChangedEvent;
        private readonly EventActionsHolder _exitedEvent;
        private readonly SessionStorage _sessionStorage;
        private readonly DisposableScope _disposable;
        private readonly ProfilingServer _profilingServer;
        private readonly ProcessShadow _processShadow;
        private AgentProcessManager _agentProcessManager;
        private readonly ICommonConfiguration _configuration;
        private readonly Processor _processor;
        private readonly ProfilerAgentClient _agentClient;
        private SessionState _state;
        private readonly Action _close;

        public DaemonApplication(Action close, Guid configurationToken, Guid sessionToken)
        {
            _close = close;
            _stateChangedEvent = new EventActionsHolder();
            _exitedEvent = new EventActionsHolder();
            CustomMarshalersInitializer.Initialize();
            ConfigurationToken = configurationToken;
            SessionToken = sessionToken;
            _configuration = ConfigurationProvider.Load();
            _sessionStorage = new SessionStorage(sessionToken, _configuration);
            _processor = new Processor();
            _disposable = new DisposableScope();
            _agentClient = new ProfilerAgentClient();
            ICallstackLoader callstackLoader = new CallstackLoader(_sessionStorage, _processor);
            _processShadow = new ProcessShadow(callstackLoader, _agentClient);
            _profilingServer = new ProfilingServer(_processShadow, _sessionStorage, _agentClient, sessionToken, _configuration);
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }

        public Guid SessionToken { get; private set; }

        public Guid ConfigurationToken { get; private set; }

        public SessionState State
        {
            get { return _state; }
            private set
            {
                _state = value;
                _stateChangedEvent.Invoke();
            }
        }

        public IProcessShadow ProcessShadow
        {
            get
            {
                if (State == SessionState.Closed)
                {
                    return null;
                }
                return _processShadow;
            }
        }

        public event Action StateChanged
        {
            add { _stateChangedEvent.Add(value); }
            remove { _stateChangedEvent.Remove(value); }
        }

        public event Action Exited
        {
            add { _exitedEvent.Add(value); }
            remove { _exitedEvent.Remove(value); }
        }

        public ProcessInfo GetProcessInfo()
        {
            if (State == SessionState.Closed)
            {
                return ProcessInfo.Empty;
            }
            return _agentProcessManager.ProcessInfo;
        }

        public void Close(bool save)
        {
            if (State == SessionState.Profiling || State == SessionState.Paused)
            {
                _agentProcessManager.Close();
            }
            _close.SafeInvoke();
        }

        public void StartProfiling(int processId, uint syncTime)
        {
            if (State != SessionState.Closed)
            {
                return;
            }
            _agentProcessManager = new AgentProcessManager(processId, syncTime);
            _agentProcessManager.ProcessShutdown += OnClientProcessShutdown;
            _profilingServer.Start(_agentProcessManager);
            State = SessionState.Profiling;
        }

        public void StartDecoding()
        {
            if (State == SessionState.Closed)
            {
                //_processShadowStorage.Load(_processShadow);
            }
        }

        private void OnClientProcessShutdown()
        {
            _profilingServer.Dispose();
            State = SessionState.Decoding;
            //_processShadowStorage.Save(_processShadow);
        }

        void IDisposable.Dispose()
        {
            _disposable.Dispose();
        }

        public string Ping(string message)
        {
            return message;
        }


        public int ProcessId
        {
            get
            {
                using (Process process = Process.GetCurrentProcess())
                {
                    return process.Id;
                }
            }
        }

        public void PauseProfiling()
        {
            if (_profilingServer.IsLaunched)
            {
                _profilingServer.State = AgentState.Paused;
                State = SessionState.Paused;
            }
        }

        public void ContinueProfiling()
        {
            if (_profilingServer.IsLaunched)
            {
                _profilingServer.State = AgentState.Profiling;
                State = SessionState.Profiling;
            }
        }

        public void StopProfiling()
        {
            if (_profilingServer.IsLaunched)
            {
                _profilingServer.State = AgentState.Closed;
                State = SessionState.Closed;
            }
        }
    }
}
