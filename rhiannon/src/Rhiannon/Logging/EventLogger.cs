using System;
using System.Diagnostics;

namespace Rhiannon.Logging
{
	public class EventLogger : ILogger
	{
		private const string LogName = "Application";
		private static readonly string ProcessName;

		static EventLogger()
		{
			using (Process process = Process.GetCurrentProcess())
			{
				ProcessName = process.ProcessName;
			}
		}

		public void Log(Exception exception, Policy policy)
		{
			if (!EventLog.SourceExists(ProcessName))
			{
				EventLog.CreateEventSource(ProcessName, LogName);
			}
			EventLog.WriteEntry(ProcessName, exception.ToString(), EventLogEntryType.Error);
		}

		public void Log(string message, string functionality, Policy policy, LogEntryType logEntryType)
		{
			if (!EventLog.SourceExists(ProcessName))
			{
				EventLog.CreateEventSource(ProcessName, LogName);
			}
			EventLog.WriteEntry(ProcessName, message, EventLogEntryType.Error);
		}

		public void LogIf(bool condition, string message, string functionality, Policy policy, LogEntryType logEntryType)
		{
			if (condition)
			{
				if (!EventLog.SourceExists(ProcessName))
				{
					EventLog.CreateEventSource(ProcessName, LogName);
				}
				EventLog.WriteEntry(ProcessName, message, EventLogEntryType.Error);
			}
		}
	}
}
