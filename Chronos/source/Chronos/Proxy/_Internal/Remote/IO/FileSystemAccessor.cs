using System.Collections.Generic;
using System.Drawing;
using Chronos.Remote.IO;

namespace Chronos.Proxy.Remote.IO
{
    internal sealed class FileSystemAccessor : ProxyBaseObject<IFileSystemAccessor>, IFileSystemAccessor
    {
        public FileSystemAccessor(IFileSystemAccessor remoteObject)
            : base(remoteObject)
        {
        }

        public bool ReadOnly
        {
            get { return Execute(() => RemoteObject.ReadOnly); }
        }

        public DirectoryInfo[] GetLogicalDrives()
        {
            DirectoryInfo[] directories = Execute(() => RemoteObject.GetLogicalDrives());
            ChangeFileSystemAccessor(directories);
            return directories;
        }

        public bool DirectoryExists(string fullName)
        {
            return Execute(() => RemoteObject.DirectoryExists(fullName));
        }

        public DirectoryInfo GetDirectory(string fullName)
        {
            DirectoryInfo directory = Execute(() => RemoteObject.GetDirectory(fullName));
            directory.ChangeFileSystemAccessor(this);
            return directory;
        }

        public DirectoryInfo[] GetDirectories(string path)
        {
            DirectoryInfo[] directories = Execute(() => RemoteObject.GetDirectories(path));
            ChangeFileSystemAccessor(directories);
            return directories;
        }

        public DirectoryInfo[] GetDirectories(string path, string searchPattern)
        {
            DirectoryInfo[] directories = Execute(() => RemoteObject.GetDirectories(path, searchPattern));
            ChangeFileSystemAccessor(directories);
            return directories;
        }

        public bool FileExists(string fullName)
        {
            return Execute(() => RemoteObject.FileExists(fullName));
        }

        public FileInfo GetFile(string fullName)
        {
            FileInfo file = Execute(() => RemoteObject.GetFile(fullName));
            file.ChangeFileSystemAccessor(this);
            return file;
        }

        public FileInfo[] GetFiles(string path)
        {
            FileInfo[] files = Execute(() => RemoteObject.GetFiles(path));
            ChangeFileSystemAccessor(files);
            return files;
        }

        public FileInfo[] GetFiles(string path, string searchPattern)
        {
            FileInfo[] files = Execute(() => RemoteObject.GetFiles(path, searchPattern));
            ChangeFileSystemAccessor(files);
            return files;
        }

        public FileSystemInfo[] GetFileSystemInfos(string path)
        {
            FileSystemInfo[] infos = Execute(() => RemoteObject.GetFileSystemInfos(path));
            ChangeFileSystemAccessor(infos);
            return infos;
        }

        private void ChangeFileSystemAccessor(IEnumerable<FileSystemInfo> fileSystemInfos)
        {
            foreach (FileSystemInfo fileSystemInfo in fileSystemInfos)
            {
                fileSystemInfo.ChangeFileSystemAccessor(this);
            }
        }

        public byte[] GetIconBytes(string fullName, IconSize size)
        {
            byte[] bytes = Execute(() => RemoteObject.GetIconBytes(fullName, size));
            return bytes;
        }
    }
}
