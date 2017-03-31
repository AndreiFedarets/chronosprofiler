using System;

namespace Rhiannon.Logging
{
	public static class LoggingProvider
	{
		private static ILogger _current;
		private static readonly object Lock;

		static LoggingProvider()
		{
			Lock = new object();
			Current = new FileLogger();
		}

		public static ILogger Current
		{
			get
			{
				lock (Lock)
				{
					return _current;
				}
			}
			set
			{
				lock (Lock)
				{
					if (value == null)
					{
						value = new DefaultLogger();
					}
					_current = value;
				}
			}
		}

		public static void Log(Exception exception, Policy policy)
		{
			if (_current != null)
			{
				_current.Log(exception, policy);
			}
		}

		public static void Log(string message, string functionality, Policy policy, LogEntryType logEntryType)
		{
			if (_current != null)
			{
				_current.Log(message, functionality, policy, logEntryType);
			}
		}

		public static void LogIf(bool condition, string message, string functionality, Policy policy, LogEntryType logEntryType)
		{
			if (_current != null)
			{
				_current.LogIf(condition, message, functionality, policy, logEntryType);
			}
		}
	}
}
