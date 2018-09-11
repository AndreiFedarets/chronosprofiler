using Chronos.Settings;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;

namespace Chronos
{
    public sealed class FileLogger : BaseLogger
    {
        private readonly object _lock;
        private DirectoryInfo _logsDirectory;
        private StreamWriter _writer;

        public FileLogger()
        {
            _lock = new object();
        }

        internal override void Initialize(ILoggerSettings settings, string applicationName)
        {
            base.Initialize(settings, applicationName);
            _logsDirectory = ((IFileLoggerSettings)settings).LogsDirectory.GetDirectory();
            if (!_logsDirectory.Exists)
            {
                _logsDirectory.Create();
            }
        }

        private StreamWriter GetWriter()
        {
            lock (_lock)
            {
                if (_writer == null)
                {
                    string fileName;
                    using (Process process = Process.GetCurrentProcess())
                    {
                        fileName = string.Format("{0}.{1}.log", ApplicationName, Environment.TickCount.ToString(CultureInfo.InvariantCulture));
                    }
                    _writer = new StreamWriter(Path.Combine(_logsDirectory.FullName, fileName));
                }
                return _writer;
            }
        }

        protected override void LogInternal(TraceEventType eventType, string source, string message)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("EventType: {0}{1}", eventType, Environment.NewLine);
            builder.AppendFormat("Source: {0}{1}", source, Environment.NewLine);
            builder.AppendFormat("Message: {0}{1}", message, Environment.NewLine);
            builder.AppendLine("====================================================================================");
            StreamWriter writer = GetWriter();
            writer.Write(builder);
            writer.Flush();
        }
    }
}
