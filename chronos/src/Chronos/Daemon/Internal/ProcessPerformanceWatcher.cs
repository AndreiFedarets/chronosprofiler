using System;
using System.Diagnostics;
using System.Threading;
using Chronos.Core;

namespace Chronos.Daemon.Internal
{
	internal class ProcessPerformanceWatcher : IDisposable
	{
		private readonly IPerformanceCounterCollection _performanceCounters;
		private readonly int _samplingPeriod;
		private readonly Process _process;
		private readonly Thread _thread;
		private volatile bool _isAlive;

		public ProcessPerformanceWatcher(int processId, int samplingPeriod, IPerformanceCounterCollection performanceCounters)
		{
			_isAlive = true;
			_process = Process.GetProcessById(processId);
			_samplingPeriod = samplingPeriod;
			_performanceCounters = performanceCounters;
			//_performanceCounters.Register(new PerformanceCounterInfo("Процесс", "% загруженности процессора", _process.ProcessName));
			//_performanceCounters.Register(new PerformanceCounterInfo("Process", "% Processor Time", _process.ProcessName));
			_performanceCounters = performanceCounters;
			_thread = new Thread(UpdateCountersInternal);
			_thread.Start();
		}

		private void UpdateCountersInternal()
		{
			while (_isAlive)
			{
				_performanceCounters.UpdateValues();
				Thread.Sleep(_samplingPeriod);
			}
		}

		public void Dispose()
		{
			_isAlive = false;
			_thread.Join();
		}
	}
}
