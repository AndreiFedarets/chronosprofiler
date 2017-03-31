using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;

namespace Rhiannon.Logging
{
	public class FileLogger : ILogger
	{
		private readonly object _lock;
		private StreamWriter _writer;

		public FileLogger()
		{
			_lock = new object();
		}

		public void Log(Exception exception, Policy policy)
		{
			StreamWriter writer = GetWriter();
			writer.WriteLine("====================================================================");
			writer.WriteLine(exception.ToString());
			writer.Flush();
		}

		public void Log(string message, string functionality, Policy policy, LogEntryType logEntryType)
		{
			StreamWriter writer = GetWriter();
			writer.WriteLine("====================================================================");
			writer.WriteLine(message);
			writer.Flush();
		}

		public void LogIf(bool condition, string message, string functionality, Policy policy, LogEntryType logEntryType)
		{
			if (condition)
			{
				StreamWriter writer = GetWriter();
				writer.WriteLine("====================================================================");
				writer.WriteLine(message);
				writer.Flush();
			}
		}

		private StreamWriter GetWriter()
		{
			lock (_lock)
			{
				if (_writer == null)
				{
					string path = @"C:\ChronosProfiler";
					if (!Directory.Exists(path))
					{
						Directory.CreateDirectory(path);
					}
					string fileName;
					using (Process process = Process.GetCurrentProcess())
					{
						fileName = string.Format("{0}.{1}.log", process.ProcessName, Environment.TickCount.ToString(CultureInfo.InvariantCulture));
					}
					_writer = new StreamWriter(Path.Combine(path, fileName));
				}
				return _writer;
			}
		}
	}
}
