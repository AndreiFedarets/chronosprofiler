using System;
using System.Collections.Generic;
using Chronos.Communication.Native;

namespace Chronos.Daemon
{
    internal sealed class Application : ChronosApplication, IApplication
    {
        private readonly Dictionary<SessionState, IDaemonStrategy> _strategies;
        private ProfilingTypeCollection _profilingTypes;
        private readonly Guid _sessionUid;
        private volatile IDaemonStrategy _currentStrategy;
        private volatile SessionState _previousSessionState;
        private readonly Proxy.LifetimeSponsorCollection _lifetimeSponsors;

        public Application(bool processOwner, Guid sessionUid)
            : base(processOwner)
        {
            _sessionUid = sessionUid;
            _strategies = new Dictionary<SessionState, IDaemonStrategy>();
            _strategies.Add(SessionState.Closed, new DaemonClosedStrategy(this));
            _currentStrategy = _strategies[SessionState.Closed];
            _previousSessionState = SessionState;
            _lifetimeSponsors = new Proxy.LifetimeSponsorCollection(Constants.Remoting.SponsorshipTimeout);
            _lifetimeSponsors.SponsorshipEnded += OnSponsorshipEnded;
            _lifetimeSponsors.Start();
        }

        /// <summary>
        /// Session unique identifier
        /// </summary>
        public override Guid Uid
        {
            get { return _sessionUid; }
        }

        public uint CurrentProfilingTime 
        {
            get { return _currentStrategy.CurrentProfilingTime; }
        }
        
        public SessionState SessionState
        {
            get
            {
                IDaemonStrategy currentStrategy;
                lock (_strategies)
                {
                    currentStrategy = _currentStrategy;
                }
                return currentStrategy.SessionState;
            }
        }

        public IRequestClient AgentClient
        {
            get
            {
                IDaemonStrategy currentStrategy;
                lock (_strategies)
                {
                    currentStrategy = _currentStrategy;
                }
                return currentStrategy.AgentRequestClient;
            }
        }

        public ProcessInformation ProfiledProcess
        {
            get
            {
                IDaemonStrategy currentStrategy;
                lock (_strategies)
                {
                    currentStrategy = _currentStrategy;
                }
                return currentStrategy.ProcessInformation;
            }
        }

        public bool SaveOnClose { get; set; }

        //TODO: remove it from here
        public ConfigurationSettings ConfigurationSettings { get; private set; }

        public event EventHandler<SessionStateEventArgs> SessionStateChanged;

        public void StartProfiling(int profiledProcessId, Guid agentApplicationUid, uint profilingBeginTime)
        {
            //System.Diagnostics.Debugger.Launch();
            _currentStrategy.StartProfiling(profiledProcessId, agentApplicationUid, profilingBeginTime);
        }

        public void StartDecoding(ILifetimeSponsor sponsor)
        {
            sponsor = Proxy.ProxyBaseObjectHelper.ResolveRealRemoteObject<ILifetimeSponsor>(sponsor);
            _lifetimeSponsors.RegisterSponsor(sponsor);
            _currentStrategy.StartDecoding();
        }

        public void StopProfiling()
        {
            _currentStrategy.StopProfiling();
        }

        public void SaveSession()
        {
            _currentStrategy.SaveSession();
        }

        public void RemoveSession()
        {
            _currentStrategy.RemoveSession();
        }

        public void ReloadData()
        {
            _currentStrategy.ReloadData();
        }

        internal IDaemonStrategy SwitchStrategy(SessionState state)
        {
            lock (_strategies)
            {
                if (SessionState != state)
                {
                    _currentStrategy = _strategies[state];
                }
            }
            return _currentStrategy;
        }

        public void RaiseSessionStateChanged()
        {
            SessionState currentState;
            lock (_strategies)
            {
                currentState = _currentStrategy.SessionState;
            }
            //Profiling
            if (currentState == Chronos.SessionState.Profiling)
            {
                _lifetimeSponsors.Stop();
            }
            //Closed or Decoding
            else
            {
                _lifetimeSponsors.Start();
            }
            if (_previousSessionState != currentState)
            {
                _previousSessionState = currentState;
                SessionStateEventArgs.RaiseEvent(SessionStateChanged, this, _previousSessionState, currentState);
            }
        }

        protected override void RunInternal()
        {
            _profilingTypes = new ProfilingTypeCollection(Catalog.ProfilingTypes, ExportLoader);
            _strategies.Add(SessionState.Decoding, new DaemonDecodingStrategy(this));
            _strategies.Add(SessionState.Profiling, new DaemonProfilingStrategy(this, _profilingTypes));
            Host.IApplication hostApplication = Host.ApplicationManager.ConnectLocal();
            ISession session = hostApplication.Sessions[Uid];
            ConfigurationSettings = session.GetConfigurationSettings();
            hostApplication.TryDispose();
        }

        protected override void CloseInternal()
        {
            lock (_strategies)
            {
                if (SessionState != SessionState.Closed)
                {
                    StopProfiling();
                    if (SaveOnClose)
                    {
                        SaveSession();
                    }
                }
            }
            base.CloseInternal();
        }

        protected override bool CanClose()
        {
            return !_lifetimeSponsors.ShouldStayAlive();
        }

        public override void Dispose()
        {
            _profilingTypes.Dispose();
            foreach (IDaemonStrategy strategy in _strategies.Values)
            {
                strategy.TryDispose();
            }
            base.Dispose();
        }

        private void OnSponsorshipEnded(object sender, EventArgs eventArgs)
        {
            if (SessionState != Chronos.SessionState.Profiling)
            {
                Close();
            }
            else
            {
                System.Diagnostics.Debug.Assert(false);
            }
        }
    }
}
