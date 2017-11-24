using System;
using Chronos.Communication.Native;

namespace Chronos.Daemon
{
    /// <summary>
    /// Represents Daemon application
    /// </summary>
    public interface IApplication : IChronosApplication
    {
        bool SaveOnClose { get; set; }

        /// <summary>
        /// Get current profiling time
        /// </summary>
        uint CurrentProfilingTime { get; }

        /// <summary>
        /// Get state of session associated with this Daemon instance
        /// </summary>
        SessionState SessionState { get; }

        /// <summary>
        /// Get requests client that allows to invoke remote procedures on profiled process side.
        /// </summary>
        IRequestClient AgentClient { get; }

        /// <summary>
        /// Get profiled process information
        /// </summary>
        ProcessInformation ProfiledProcess { get; }

        /// <summary>
        /// Occurs when state of associated session was changed.
        /// </summary>
        event EventHandler<SessionStateEventArgs> SessionStateChanged;

        /// <summary>
        /// Start profiling session.
        /// Can be called when session state is SessionState.Closed.
        /// If session already is in decoding or profiling state, the call will be ignored.
        /// </summary>
        /// <param name="profiledProcessId">Id of profiled process</param>
        /// <param name="agentApplicationUid">Uid of agent application inside profiled process</param>
        void StartProfiling(int profiledProcessId, Guid agentApplicationUid, uint profilingBeginTime);

        /// <summary>
        /// Start decoding session.
        /// Can be called when session state is SessionState.Closed.
        /// If session already is in decoding or profiling state, the call will be ignored.
        /// </summary>
        void StartDecoding(ILifetimeSponsor sponsor);

        /// <summary>
        /// Stop profiling session and close profiled process. Daemon will not be closed.
        /// Daemon state will be changed to SessionState.Decoding
        /// </summary>
        void StopProfiling();

        /// <summary>
        /// Save profiling session and all profiling data.
        /// Data will be saved only when session state is SessionState.Decoding.
        /// So you cannot save session until profiled process is running.
        /// </summary>
        void SaveSession();

        /// <summary>
        /// Remove profiling session and all profiling data.
        /// Data will be removed only when session state is SessionState.Decoding.
        /// So you cannot remove session until profiled process is running.
        /// </summary>
        void RemoveSession();

        /// <summary>
        /// Load profiling data from agent when profiling is active. Does nothing when decoding is active.
        /// </summary>
        void ReloadData();
    }
}
