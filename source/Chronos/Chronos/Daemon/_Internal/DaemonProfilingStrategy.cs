using System;
using Chronos.Communication;
using Chronos.Communication.Native;
using Chronos.Storage;
using System.IO;

namespace Chronos.Daemon
{
    internal class DaemonProfilingStrategy : RemoteBaseObject, IDaemonStrategy
    {
        private readonly IAgentLibrary _agentLibrary;
        private readonly Application _application;
        private readonly IProfilingTypeCollection _profilingTypes;
        private ProfilingTypeManager _profilingTypesManager;
        private RequestServer _requestServer;
        private RequestClient _agentClient;
        private IGatewayServer _gatewayServer;
        private ProfiledProcessManager _profiledProcess;
        private IDataStorage _storage;
        private ProfilingTimer _profilingTimer;

        public DaemonProfilingStrategy(Application application, IProfilingTypeCollection profilingTypes)
        {
            _agentLibrary = new AgentLibrary();
            _application = application;
            _profilingTypes = profilingTypes;
            SessionState = SessionState.Profiling;
        }

        public IProfilingTimer ProfilingTimer
        { 
            get { return _profilingTimer; }
        }

        public SessionState SessionState { get; private set; }

        public IRequestClient AgentRequestClient
        {
            get { return _agentClient; }
        }

        public ProcessInformation ProcessInformation { get; private set; }

        public void StartProfiling(int profiledProcessId, Guid agentApplicationUid, uint profilingBeginTime)
        {
            //Profiling is already started or decoding in progress
            if (_profiledProcess != null)
            {
                return;
            }
            _profilingTimer = new ProfilingTimer(profilingBeginTime);
            DirectoryInfo profilingResultsDirectory = _application.ApplicationSettings.ProfilingResults.GetDirectory();
            string profilingResultsFile = Path.Combine(profilingResultsDirectory.FullName, _application.Uid.ToString("N"));
            _storage = new DataStorage(profilingResultsFile);
            _profiledProcess = new ProfiledProcessManager(profiledProcessId);
            _profiledProcess.Exited += OnProfiledProcessExited;
            ProcessInformation = _profiledProcess.GetProcessInformation();
            IStreamFactory streamFactory = Connector.Native.StreamFactory;
            _requestServer = new RequestServer();
            _requestServer.Run(streamFactory.CreateInvokeStream());
            _gatewayServer = new NativeGatewayServer(_agentLibrary, Connector.ApplicationUid, _application.ConfigurationSettings.GatewaySettings); //new ManagedGatewayServer(streamFactory, _application.Settings.GatewaySettings);//
            _agentClient = new RequestClient(streamFactory.OpenInvokeStream(agentApplicationUid));
            ConfigurationSettings configurationSettings = _application.ConfigurationSettings;
            ProfilingTypeSettingsCollection profilingTypesSettings = configurationSettings.ProfilingTypesSettings;
            _profilingTypesManager = new ProfilingTypeManager(_profilingTypes, profilingTypesSettings);
            _profilingTypesManager.AttachStorage(_storage);
            _profilingTypesManager.ExportServices(_application.ServiceContainer);
            _profilingTypesManager.ImportServices(_application.ServiceContainer);
            _profilingTypesManager.StartProfiling(_gatewayServer);
            _gatewayServer.Lock();
            _gatewayServer.Start();
            _application.RaiseSessionStateChanged();
        }

        public void ReloadData()
        {
            if (SessionState == SessionState.Profiling)
            {
                _agentClient.Invoke(new Guid("D0A43C7E-DDB7-4BAD-8418-7967EBB85EE5"), typeof(void));
            }
            _profilingTypesManager.ReloadData();
        }

        private void OnProfiledProcessExited(object sender, EventArgs e)
        {
            StopProfiling();
        }

        public void StartDecoding()
        {
        }

        public void StopProfiling()
        {
            if (SessionState == SessionState.Profiling)
            {
                SessionState = SessionState.Decoding;
                //TODO: replace with closing from profiling target, because only it knows how to close profiled process safely.
                _requestServer.Dispose();
                _profiledProcess.Close();
                _profiledProcess.Dispose();
                _agentClient.Dispose();
                _profilingTypesManager.StopProfiling();
                _gatewayServer.Dispose();
                _agentLibrary.TryDispose();
                _profiledProcess.Exited -= OnProfiledProcessExited;
                _application.RaiseSessionStateChanged();
                ReloadData();
            }
        }

        public void SaveSession()
        {
            throw new NotImplementedException();
        }

        public void RemoveSession()
        {
            if (SessionState == SessionState.Profiling)
            {
                StopProfiling();
            }
            throw new NotImplementedException();
        }
    }
}
