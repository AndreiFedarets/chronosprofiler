namespace Chronos.Settings
{
    public interface ICrashDumpSettings
    {
        bool IsEnabled { get; set; }

        IDirectorySettings DumpsDirectory { get; }
    }
}
