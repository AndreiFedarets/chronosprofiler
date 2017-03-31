using System;
using System.Diagnostics;

namespace Chronos.Daemon.Internal
{
	internal class ProcessTimer : IProcessTimer
	{
		private readonly Process _process;

		public ProcessTimer(int proceeId)
		{
			_process = Process.GetProcessById(proceeId);
		}

		public uint GetTime()
		{
			TimeSpan running = DateTime.Now - _process.StartTime;
			return (uint)running.TotalMilliseconds;
		}

		public void Dispose()
		{
			_process.Dispose();
		}
	}
}
