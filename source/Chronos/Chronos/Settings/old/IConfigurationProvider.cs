namespace Chronos.Config
{
    public interface IConfigurationProvider
    {
        DaemonSection Daemon { get; }

        CommunicationSection Communication { get; }

        ExtensionsSection Extensions { get; }

        LoggingSection Logging { get; }
    }
}
