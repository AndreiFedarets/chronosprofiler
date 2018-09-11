using System.Diagnostics;

namespace Chronos
{
    public sealed class EventLogger : BaseLogger, ILogger
    {
        private const string LogName = "Chronos Profiler";

        internal override void Initialize(Settings.ILoggerSettings settings, string applicationName)
        {
            base.Initialize(settings, applicationName);
        }

        protected override void LogInternal(TraceEventType eventType, string source, string message)
        {
            //if (!EventLog.SourceExists(source))
            //{
            //    EventLog.CreateEventSource(source, LogName);
            //}
            //EventLogEntryType entryType = ConvertTraceToEventLogType(eventType);
            //EventLog.WriteEntry(source, message, entryType);
        }
    }
}
