using System;
using Chronos.Communication.Native;

namespace Chronos.Daemon
{
    internal interface IDaemonStrategy
    {
        SessionState SessionState { get; }

        IRequestClient AgentRequestClient { get; }

        ProcessInformation ProcessInformation { get; }

        void StartProfiling(int profiledProcessId, Guid agentApplicationUid);

        void ReloadData();

        void StartDecoding();

        void StopProfiling();

        void SaveSession();

        void RemoveSession();
    }
}
