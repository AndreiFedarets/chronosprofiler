namespace Chronos.Settings
{
    public interface IExtensionSettings : IDirectorySettings
    {
        bool IsEnabled { get; set; }
    }
}
