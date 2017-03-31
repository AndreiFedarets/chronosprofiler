using System;
using System.Diagnostics;
using System.Text;

namespace Chronos
{
    public interface ILogger
    {
        SourceLevels MinimalSourceLevels { get; }

        bool ShouldLog(TraceEventType eventType);

        void Log(TraceEventType eventType, string source, string message);
    }

    public static class LoggerExtensions
    {
        public static void Log(this ILogger logger, TraceEventType eventType, string message)
        {
            Log(logger, eventType, false, message);
        }

        public static void Log(this ILogger logger, TraceEventType eventType, string message, params object[] args)
        {
            Log(logger, eventType, false, message, args);
        }

        public static void Log(this ILogger logger, TraceEventType eventType, bool enableStack, string message)
        {
            if (logger.ShouldLog(eventType))
            {
                if (enableStack)
                {
                    message += Environment.NewLine;
                    message += GenerateStack();
                }
                logger.Log(eventType, string.Empty, message);
            }
        }

        public static void Log(this ILogger logger, TraceEventType eventType, bool enableStack, string message, params object[] args)
        {
            if (logger.ShouldLog(eventType))
            {
                if (enableStack)
                {
                    message += Environment.NewLine;
                    message += GenerateStack();
                }
                logger.Log(eventType, string.Empty, string.Format(message, args));
            }
        }

        public static void Log(this ILogger logger, TraceEventType eventType, Exception exception)
        {
            logger.Log(eventType, string.Empty, exception.ToString());
        }

        private static string GenerateStack()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("StackTrace:");
            StackTrace stackTrace = new StackTrace();
            StackFrame[] frames = stackTrace.GetFrames();
            for (int i = 2; i < frames.Length; i++)
            {
                StackFrame frame = frames[i];
                builder.AppendLine("     " + frame.GetMethod().GetFullName());
            }
            return builder.ToString();
        }
    }
}
