using System;

namespace Rhiannon.Logging
{
	public interface ILogger
	{
		void Log(Exception exception, Policy policy);

		void Log(string message, string functionality, Policy policy, LogEntryType logEntryType);

		void LogIf(bool condition, string message, string functionality, Policy policy, LogEntryType logEntryType);
	}
}
