using Chronos.Core;

namespace Chronos.Daemon.Internal
{
    internal interface IProfilerAgentClient
    {
        void Initialize(AgentProcessManager agentProcessManager);

        AgentState GetState();

        void SetState(AgentState state);

        void FlushUnits(UnitType unitType);
    }
}
