namespace Chronos.Settings
{
    public interface IFileLoggerSettings : ILoggerSettings
    {
        IDirectorySettings LogsDirectory { get; }
    }
}
