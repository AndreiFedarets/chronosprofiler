using System;
using System.Diagnostics;
using System.Text;

namespace Chronos
{
    public sealed class DefaultLogger : ILogger
    {
        public SourceLevels MinimalSourceLevels
        {
            get { return SourceLevels.All; }
        }

        public bool ShouldLog(TraceEventType eventType)
        {
            return true;
        }

        public void Log(TraceEventType eventType, string source, string message)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("EventType: {0}{1}", eventType, Environment.NewLine);
            builder.AppendFormat("Source: {0}{1}", source, Environment.NewLine);
            builder.AppendFormat("Message: {0}{1}", message, Environment.NewLine);
            builder.AppendLine("====================================================================================");
            Debug.WriteLine(builder.ToString());
        }
    }
}
