using Chronos.Settings;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Chronos
{
    internal sealed class CompositeLogger : ILogger
    {
        private readonly List<ILogger> _loggers;

        public CompositeLogger()
        {
            _loggers = new List<ILogger>();
        }

        public SourceLevels MinimalSourceLevels
        {
            get
            {
                if (_loggers.Count == 0)
                {
                    return SourceLevels.Off;
                }
                SourceLevels minimalSourceLevels = _loggers[0].MinimalSourceLevels;
                if (_loggers.Count == 1 || minimalSourceLevels == SourceLevels.All)
                {
                    return minimalSourceLevels;
                }
                foreach (ILogger logger in _loggers)
                {
                    if (logger.MinimalSourceLevels == SourceLevels.All)
                    {
                        return SourceLevels.All;
                    }
                    if (logger.MinimalSourceLevels > minimalSourceLevels)
                    {
                        minimalSourceLevels = logger.MinimalSourceLevels;
                    }
                }
                return minimalSourceLevels;
            }
        }

        public void Initialize(ILoggingSettings settings, string applicationName)
        {
            if (settings.FileLogger != null && settings.FileLogger.IsEnabled)
            {
                FileLogger fileLogger = new FileLogger();
                fileLogger.Initialize(settings.FileLogger, applicationName);
                _loggers.Add(fileLogger);
            }
            if (settings.EventLogger != null && settings.EventLogger.IsEnabled)
            {
                EventLogger eventLogger = new EventLogger();
                eventLogger.Initialize(settings.EventLogger, applicationName);
                _loggers.Add(eventLogger);
            }
        }

        public bool ShouldLog(TraceEventType eventType)
        {
            return _loggers.Any(x => x.ShouldLog(eventType));
        }

        public void Log(TraceEventType eventType, string source, string message)
        {
            foreach (ILogger logger in _loggers)
            {
                logger.Log(eventType, source, message);
            }
        }
    }
}
