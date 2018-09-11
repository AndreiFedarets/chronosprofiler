using System;
using Chronos.Communication.Native;

namespace Chronos.Daemon
{
    internal class DaemonClosedStrategy : RemoteBaseObject, IDaemonStrategy
    {
        private readonly Application _application;

        public DaemonClosedStrategy(Application application)
        {
            _application = application;
        }

        public IProfilingTimer ProfilingTimer
        {
            get { return null; } 
        }

        public SessionState SessionState
        {
            get { return SessionState.Closed; }
        }

        public IRequestClient AgentRequestClient
        {
            get { return null; }
        }

        public ProcessInformation ProcessInformation
        {
            get { return null; }
        }

        public void StartProfiling(int profiledProcessId, Guid agentApplicationUid, uint profilingBeginTime)
        {
            IDaemonStrategy strategy = _application.SwitchStrategy(SessionState.Profiling);
            strategy.StartProfiling(profiledProcessId, agentApplicationUid, profilingBeginTime);
        }

        public void ReloadData()
        {
            //throw new InvalidOperationException(string.Format("This operation is not avaliable in {0} state", SessionState));
        }

        public void StartDecoding()
        {
            IDaemonStrategy strategy = _application.SwitchStrategy(SessionState.Decoding);
            strategy.StartDecoding();
        }

        public void StopProfiling()
        {
            //throw new InvalidOperationException(string.Format("This operation is not avaliable in {0} state", SessionState));
        }

        public void SaveSession()
        {
            //throw new InvalidOperationException(string.Format("This operation is not avaliable in {0} state", SessionState));
        }

        public void RemoveSession()
        {
            //throw new InvalidOperationException(string.Format("This operation is not avaliable in {0} state", SessionState));
        }
    }
}
