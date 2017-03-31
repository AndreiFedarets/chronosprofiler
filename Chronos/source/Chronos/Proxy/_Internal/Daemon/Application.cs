using Chronos.Communication.Native;
using System;

namespace Chronos.Proxy.Daemon
{
    internal sealed class Application : ChronosApplication<Chronos.Daemon.IApplication>, Chronos.Daemon.IApplication
    {
        private readonly LazyValue<RequestClient> _agentClient;
        private readonly RemoteEventRouter<SessionStateEventArgs> _sessionStateChangedEventSink;

        public Application(Chronos.Daemon.IApplication application)
            : base(application)
        {
            _agentClient = new LazyValue<RequestClient>(() => new RequestClient(application.AgentClient));
            _sessionStateChangedEventSink = new RemoteEventRouter<SessionStateEventArgs>(RemoteObject, "SessionStateChanged", this);
        }

        public bool SaveOnClose
        {
            get { return Execute(() => RemoteObject.SaveOnClose); }
            set { Execute(() => RemoteObject.SaveOnClose = value); }
        }

        public SessionState SessionState
        {
            get { return Execute(() => RemoteObject.SessionState); }
        }

        public IRequestClient AgentClient
        {
            get { return _agentClient.Value; }
        }

        public ProcessInformation ProfiledProcess
        {
            get { return Execute(() => RemoteObject.ProfiledProcess); }
        }

        public event EventHandler<SessionStateEventArgs> SessionStateChanged
        {
            add { _sessionStateChangedEventSink.Event += value; }
            remove { _sessionStateChangedEventSink.Event -= value; }
        }

        public void StartProfiling(int profiledProcessId, Guid agentApplicationUid)
        {
            Execute(() => RemoteObject.StartProfiling(profiledProcessId, agentApplicationUid));
        }

        public void StartDecoding(ILifetimeSponsor sponsor)
        {
            Execute(() => RemoteObject.StartDecoding(sponsor));
        }

        public void StopProfiling()
        {
            Execute(() => RemoteObject.StopProfiling());
        }

        public void SaveSession()
        {
            Execute(() => RemoteObject.SaveSession());
        }

        public void RemoveSession()
        {
            Execute(() => RemoteObject.RemoveSession());
        }

        public void ReloadData()
        {
            Execute(() => RemoteObject.ReloadData());
        }

        public override void Dispose()
        {
            VerifyDisposed();
            _sessionStateChangedEventSink.Dispose();
            _agentClient.Dispose();
            base.Dispose();
        }

        public bool CanStayAlive()
        {
            return true;
        }
    }
}
