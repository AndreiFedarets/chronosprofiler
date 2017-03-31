using System.Collections.Generic;
using System.Drawing;

namespace Chronos.Remote.IO
{
    public sealed class FilteredFileSystemAccessor : IFileSystemAccessor
    {
        private readonly IFileSystemAccessor _accessor;

        public FilteredFileSystemAccessor(IFileSystemAccessor accessor)
        {
            _accessor = accessor;
            Filter = new FileSystemFilter();
        }

        public bool ReadOnly
        {
            get { return _accessor.ReadOnly; }
        }

        public FileSystemFilter Filter { get; private set; }

        public DirectoryInfo[] GetLogicalDrives()
        {
            return _accessor.GetLogicalDrives();
        }

        public bool DirectoryExists(string fullName)
        {
            return _accessor.DirectoryExists(fullName);
        }

        public DirectoryInfo GetDirectory(string fullName)
        {
            return _accessor.GetDirectory(fullName);
        }

        public DirectoryInfo[] GetDirectories(string path)
        {
            return _accessor.GetDirectories(path);
        }

        public DirectoryInfo[] GetDirectories(string path, string searchPattern)
        {
            return _accessor.GetDirectories(path, searchPattern);
        }

        public bool FileExists(string fullName)
        {
            FileInfo file = GetFile(fullName);
            FileInfo filteredFile = Filter.FilterFile(file);
            return filteredFile != null;
        }

        public FileInfo GetFile(string fullName)
        {
            return _accessor.GetFile(fullName);
        }

        public FileInfo[] GetFiles(string path)
        {
            FileInfo[] files = _accessor.GetFiles(path);
            FileInfo[] filteredFiles = Filter.FilterFiles(files);
            return filteredFiles;
        }

        public FileInfo[] GetFiles(string path, string searchPattern)
        {
            FileInfo[] files = _accessor.GetFiles(path, searchPattern);
            FileInfo[]  filteredFiles = Filter.FilterFiles(files);
            return filteredFiles;
        }

        public FileSystemInfo[] GetFileSystemInfos(string path)
        {
            List<FileSystemInfo> fileSystemInfos = new List<FileSystemInfo>();
            fileSystemInfos.AddRange(GetDirectories(path));
            fileSystemInfos.AddRange(GetFiles(path));
            return fileSystemInfos.ToArray();
        }

        public byte[] GetIconBytes(string fullName, IconSize size)
        {
            byte[] bytes = _accessor.GetIconBytes(fullName, size);
            return bytes;
        }
    }
}
