using System;

namespace Chronos.Proxy
{
    internal sealed class Session : ProxyBaseObject<ISession>, ISession
    {
        private readonly Chronos.Host.IApplication _application;
        private readonly LazyValue<ServiceContainer> _serviceContainer;
        private readonly LazyValue<ProfilingTimer> _profilingTimer;
        private readonly RemoteEventRouter<SessionStateEventArgs> _sessionStateChangedEventSink;

        public Session(ISession session, Chronos.Host.IApplication application)
            : base(session)
        {
            _application = application;
            _serviceContainer = new LazyValue<ServiceContainer>(() => new ServiceContainer(RemoteObject.ServiceContainer));
            _profilingTimer = new LazyValue<ProfilingTimer>(() => new ProfilingTimer(RemoteObject.ProfilingTimer.BeginProfilingTime));
            _sessionStateChangedEventSink = new RemoteEventRouter<SessionStateEventArgs>(RemoteObject, "SessionStateChanged", this);
            Uid = Execute(() => RemoteObject.Uid);
        }

        public Guid Uid { get; private set; }

        public IProfilingTimer ProfilingTimer
        {
            get { return _profilingTimer.Value; }
        }

        public IServiceContainer ServiceContainer
        {
            get { return _serviceContainer.Value; }
        }

        public Guid ConfigurationUid
        {
            get { return Execute(() => RemoteObject.ConfigurationUid); }
        }

        public SessionState State
        {
            get { return Execute(() => RemoteObject.State); }
        }

        public bool IsActive
        {
            get { return Execute(() => RemoteObject.IsActive); }
        }

        public bool SaveOnClose
        {
            get { return Execute(() => RemoteObject.SaveOnClose); }
            set { Execute(() => RemoteObject.SaveOnClose = value); }
        }

        public Chronos.Host.IApplication Application
        {
            get { return _application; }
        }

        public event EventHandler<SessionStateEventArgs> SessionStateChanged
        {
            add { _sessionStateChangedEventSink.Event += value; }
            remove { _sessionStateChangedEventSink.Event -= value; }
        }

        public ProcessInformation GetProfiledProcessInformation()
        {
            return Execute(() => RemoteObject.GetProfiledProcessInformation());
        }

        public ConfigurationSettings GetConfigurationSettings()
        {
            return Execute(() => RemoteObject.GetConfigurationSettings());
        }

        public void StartDecoding(ILifetimeSponsor sponsor)
        {
            Execute(() => RemoteObject.StartDecoding(sponsor));
        }

        public void StopProfiling()
        {
            Execute(() => RemoteObject.StopProfiling());
        }

        public void CloseSession()
        {
            Execute(() => RemoteObject.CloseSession());
        }

        public void SaveSession()
        {
            Execute(() => RemoteObject.SaveSession());
        }

        public void RemoveSession()
        {
            Execute(() => RemoteObject.RemoveSession());
        }

        public void FlushData()
        {
            Execute(() => RemoteObject.FlushData());
        }

        public override void Dispose()
        {
            ExecuteDispose(() =>
            {
                _serviceContainer.Dispose();
            });
            base.Dispose();
        }
    }
}
