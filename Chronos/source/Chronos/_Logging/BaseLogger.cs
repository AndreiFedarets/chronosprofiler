using Chronos.Settings;
using System.Diagnostics;

namespace Chronos
{
    public abstract class BaseLogger : ILogger
    {
        public SourceLevels MinimalSourceLevels { get; private set; }

        protected string ApplicationName { get; private set; }

        internal virtual void Initialize(ILoggerSettings settings, string applicationName)
        {
            MinimalSourceLevels = settings.LogLevel;
            ApplicationName = applicationName;
        }

        public bool ShouldLog(TraceEventType eventType)
        {
            switch (MinimalSourceLevels)
            {
                case SourceLevels.All:
                    return true;
                case SourceLevels.Critical:
                    return (int)eventType <= (int)TraceEventType.Critical;
                case SourceLevels.Error:
                    return (int)eventType <= (int)TraceEventType.Error;
                case SourceLevels.Warning:
                    return (int)eventType <= (int)TraceEventType.Warning;
                case SourceLevels.Information:
                    return (int)eventType <= (int)TraceEventType.Information;
                case SourceLevels.Verbose:
                    return (int)eventType <= (int)TraceEventType.Verbose;
            }
            return false;
        }

        public void Log(TraceEventType eventType, string source, string message)
        {
            if (!ShouldLog(eventType))
            {
                return;
            }
            LogInternal(eventType, source, message);
        }

        protected abstract void LogInternal(TraceEventType eventType, string source, string message);
    }
}
