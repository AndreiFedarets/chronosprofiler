using System;
using Chronos.Core;

namespace Chronos.Daemon.Internal
{
    internal interface IProfilingServer : IDisposable
    {
        bool IsLaunched { get; }

        void Start(AgentProcessManager agentProcessManager);

        AgentState State { get; set; }
    }
}
