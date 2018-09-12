using System;

namespace Chronos.Host
{
    /// <summary>
    /// Represents external API via named pipes for profiling agents
    /// </summary>
    public interface IHostRequestServer
    {
        /// <summary>
        /// Start profiling session. Session will be created and added to collection of sessions.
        /// Daemon application will be started.
        /// </summary>
        /// <param name="configurationToken">Token of configuration for which session will be created</param>
        /// <param name="processId">Id of profiled process</param>
        /// <returns>Session token</returns>
        SessionSettings StartProfilingSession(Guid configurationToken, int processId);
    }
}
