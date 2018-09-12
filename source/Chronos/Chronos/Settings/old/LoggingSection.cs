using System.Configuration;
using System.Diagnostics;

namespace Chronos.Config
{
    public class LoggingSection : ConfigurationSection
    {
        private const string LogLevelProperty = "loglevel";
        private const string LoggerProperty = "logger";

        [ConfigurationProperty(LogLevelProperty, IsRequired = false, DefaultValue = SourceLevels.Off)]
        public SourceLevels LogLevel
        {
            get { return (SourceLevels)this[LogLevelProperty]; }
            set { this[LogLevelProperty] = value; }
        }

        [ConfigurationProperty(LoggerProperty, IsRequired = false)]
        public LoggerElement Logger
        {
            get { return (LoggerElement)this[LoggerProperty]; }
            set { this[LoggerProperty] = value; }
        }
    }
}
