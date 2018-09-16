namespace Chronos.Accessibility.IO
{
    [PublicService(typeof(Proxy.Accessibility.IO.FileSystemAccessor))]
    public interface IFileSystemAccessor
    {
        bool ReadOnly { get; }

        DirectoryInfo[] GetLogicalDrives();

        DirectoryInfo GetDirectory(string fullName);

        DirectoryInfo[] GetDirectories(string path);

        DirectoryInfo[] GetDirectories(string path, string searchPattern);

        FileInfo GetFile(string fullName);

        FileInfo[] GetFiles(string path);

        FileInfo[] GetFiles(string path, string searchPattern);

        FileSystemInfo[] GetFileSystemInfos(string path);

        bool DirectoryExists(string fullName);

        bool FileExists(string fullName);

        byte[] GetIconBytes(string fullName, IconSize size);
    }
}
