using System;
using Chronos.Core;

namespace Chronos.Daemon
{
	public interface IDaemonApplication
	{
		SessionState State { get; }

		event Action StateChanged;

		event Action Exited;

		IProcessShadow ProcessShadow { get; }

		ProcessInfo GetProcessInfo();

		void Close(bool save);

        void StartProfiling(int processId, uint syncTime);

		void StartDecoding();

		string Ping(string message);

		int ProcessId { get; }

	    void PauseProfiling();

	    void ContinueProfiling();

        void StopProfiling();
    }
}
