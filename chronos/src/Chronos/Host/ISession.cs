using System;
using Chronos.Core;
using Chronos.Daemon;

namespace Chronos.Host
{
	public interface ISession : IDisposable
	{
		Guid Token { get; }

		SessionState State { get; }

		ProcessInfo ProcessInfo { get; }

		ActivationSettings ActivationSettings { get; }

		ConfigurationSettings ConfigurationSettings { get; }

		bool IsActive { get; }

		bool IsSaved { get; }

		void Remove();

		IDaemonApplication StartDecoding();

        /// <summary>
        /// Stop profiling session and close profiled process. Daemon will not be closed.
        /// Daemon <see cref="State">state</see> will be changed to <seealso cref="Chronos.Core.SessionState.Decoding">SessionState.Decoding</seealso>
        /// </summary>
        void StopProfiling();

        /// <summary>
        /// Stop profiling session (if it is active), close profiled process and close Daemon.
        /// Daemon <see cref="State">state</see> will be changed to <seealso cref="Chronos.Core.SessionState.Closed">SessionState.Closed</seealso>
        /// </summary>
        /// <param name="save">Flag indicates what to do with profiling data - save or not</param>
        void Close(bool save);

        void Close();

    }
}
