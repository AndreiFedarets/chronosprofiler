using System.IO;

namespace Chronos.Settings
{
    public interface IDirectorySettings
    {
        string Path { get; set; }

        DirectoryInfo GetDirectory();
    }
}
