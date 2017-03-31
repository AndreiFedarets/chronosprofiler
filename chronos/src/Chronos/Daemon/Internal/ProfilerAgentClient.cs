using Chronos.Communication.NamedPipe;
using Chronos.Core;

namespace Chronos.Daemon.Internal
{
    internal class ProfilerAgentClient : IProfilerAgentClient
    {
        private AgentProcessManager _agentProcessManager;
        private ServerInvoke _serverInvoke;

        public void Initialize(AgentProcessManager clientProcessManager)
        {
            _agentProcessManager = clientProcessManager;
            _serverInvoke = new ServerInvoke(PipeNameFormatter.GetAgentServerPipeName(clientProcessManager.ProcessInfo.ProcessId));
            _agentProcessManager.ProcessShutdown += OnAgentProcessShutdown;
        }

        private void OnAgentProcessShutdown()
        {
            _agentProcessManager.ProcessShutdown -= OnAgentProcessShutdown;
            _serverInvoke.Dispose();
            _serverInvoke = null;
        }

        public void FlushUnits(UnitType unitType)
        {
            if (_serverInvoke != null)
            {
                _serverInvoke.Invoke((long) AgentFunctionCode.FlushUnits, (uint) unitType);
            }
        }

        public AgentState GetState()
        {
            AgentState state = _serverInvoke == null ? AgentState.Closed : _serverInvoke.Invoke<AgentState>((long)AgentFunctionCode.GetState);
            return state;
        }

        public void SetState(AgentState state)
        {
            if (_serverInvoke != null)
            {
                _serverInvoke.Invoke((long)AgentFunctionCode.SetState, (byte)state);
            }
        }
    }
}
