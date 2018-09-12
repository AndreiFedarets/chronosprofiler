namespace Chronos.Settings
{
    public interface IFileSystemSettings
    {
        IDirectorySettingsCollection Extensions { get; }

        IDirectorySettings ProfilingResults { get; }
    }
}
