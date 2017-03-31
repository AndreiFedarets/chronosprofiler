using System;
using System.Diagnostics;

namespace Rhiannon.Logging
{
	public class DefaultLogger : ILogger
	{
		public void Log(Exception exception, Policy policy)
		{
			if (policy == Policy.Core)
			{
				Debugger.Break();
			}
		}

		public void Log(string message, string functionality, Policy policy, LogEntryType logEntryType)
		{
			Debug.WriteLine(string.Format("{0}: {1}", functionality, message), logEntryType.ToString());
		}

		public void LogIf(bool condition, string message, string functionality, Policy policy, LogEntryType logEntryType)
		{
			if (condition)
			{
				Log(message, functionality, policy, logEntryType);
			}
		}
	}
}
