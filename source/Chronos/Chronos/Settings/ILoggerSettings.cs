using System.Diagnostics;

namespace Chronos.Settings
{
    public interface ILoggerSettings
    {
        SourceLevels LogLevel { get; set; }

        bool IsEnabled { get; set; }
    }
}
