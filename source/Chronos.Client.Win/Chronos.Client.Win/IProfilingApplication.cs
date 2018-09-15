using System;

namespace Chronos.Client.Win
{
    public interface IProfilingApplication : IApplicationBase
    {
        /// <summary>
        /// Get session state.
        /// </summary>
        SessionState SessionState { get; }

        event EventHandler<SessionStateEventArgs> SessionStateChanged;

        IProfilingTimer ProfilingTimer { get; }

        void FlushData();
    }
}
