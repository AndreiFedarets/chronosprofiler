using System;

namespace Chronos
{
    public interface ISession
    {
        /// <summary>
        /// Session unique token.
        /// </summary>
        Guid Uid { get; }

        /// <summary>
        /// Get current profiling time
        /// </summary>
        uint CurrentProflingTime { get; }

        /// <summary>
        /// Unique token of configuration under which this session was started.
        /// </summary>
        Guid ConfigurationUid { get; }

        /// <summary>
        /// Get session state.
        /// </summary>
        SessionState State { get; }

        /// <summary>
        /// Returns 'True' if session State is SessionState.Profiling or SessionState.Decoding, otherwise 'False'.
        /// </summary>
        bool IsActive { get; }

        /// <summary>
        /// Get or set flag-notification, that profiling data should or should not be saved when profiling stopped.
        /// </summary>
        bool SaveOnClose { get; set; }

        /// <summary>
        /// Get Host Application where this Configuration was created
        /// </summary>
        Host.IApplication Application { get; }

        /// <summary>
        /// Get container with shared services that provides profiling data.
        /// If session is not active returns null.
        /// </summary>
        IServiceContainer ServiceContainer { get; }

        /// <summary>
        /// Get profiled process information
        /// </summary>
        /// <returns>Information about profiled process, or null if session is not active</returns>
        ProcessInformation GetProfiledProcessInformation();

        /// <summary>
        /// Get configuration settings.
        /// This method returns copy of configuration settings to leave them immutable.
        /// </summary>
        /// <returns>Configuration settings</returns>
        /// <seealso cref="ConfigurationSettings"/>
        ConfigurationSettings GetConfigurationSettings();

        /// <summary>
        /// Start decoding session.
        /// Can be called when session state is SessionState.Closed or SessionState.Profiling.
        /// If session already is in decoding state, the call will be ignored.
        /// </summary>
        void StartDecoding(ILifetimeSponsor sponsor);

        /// <summary>
        /// Stop profiling session and close profiled process. Daemon will not be closed.
        /// Daemon state will be changed to SessionState.Decoding
        /// </summary>
        void StopProfiling();

        /// <summary>
        /// Close session and all related resource. Profiled process will be closed as well as daemon.
        /// Profiling data will be saved if property SaveData is 'True', otherwise data will be removed.
        /// Session state will be changed to SessionState.Closed
        /// </summary>
        void CloseSession();

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
        /// If session is in Profiling state call of this method forces cached data sending from Agent application (on profiled process side) to Daemon application.
        /// If session state is not Profiling - does nothing
        /// </summary>
        void FlushData();
    }
}
