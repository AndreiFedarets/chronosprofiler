using System.Diagnostics;

namespace Chronos.Settings
{
    public interface ILoggingSettings
    {
        IFileLoggerSettings FileLogger { get; }

        IEventLoggerSettings EventLogger { get; }
    }
}
