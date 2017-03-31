namespace Chronos.Settings
{
    public interface IApplicationSettings
    {
        ILoggingSettings Logging { get; }

        ICrashDumpSettings CrashDump { get; }

        IExtensionSettingsCollection Extensions { get; }

        IProfilingResultsSettings ProfilingResults { get; }

        IConnectionSettingsCollection HostConnections { get; }
    }
}
