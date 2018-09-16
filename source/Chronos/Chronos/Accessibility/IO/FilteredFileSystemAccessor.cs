using System.Collections.Generic;

namespace Chronos.Accessibility.IO
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
            DirectoryInfo[] directories = _accessor.GetLogicalDrives();
            ChangeFileSystemAccessor(directories);
            return directories;
        }

        public bool DirectoryExists(string fullName)
        {
            return _accessor.DirectoryExists(fullName);
        }

        public DirectoryInfo GetDirectory(string fullName)
        {
            DirectoryInfo directory = _accessor.GetDirectory(fullName);
            ChangeFileSystemAccessor(new[] { directory });
            return directory;
        }

        public DirectoryInfo[] GetDirectories(string path)
        {
            DirectoryInfo[] directories = _accessor.GetDirectories(path);
            ChangeFileSystemAccessor(directories);
            return directories;
        }

        public DirectoryInfo[] GetDirectories(string path, string searchPattern)
        {
            DirectoryInfo[] directories = _accessor.GetDirectories(path, searchPattern);
            ChangeFileSystemAccessor(directories);
            return directories;
        }

        public bool FileExists(string fullName)
        {
            FileInfo file = GetFile(fullName);
            FileInfo filteredFile = Filter.FilterFile(file);
            return filteredFile != null;
        }

        public FileInfo GetFile(string fullName)
        {
            FileInfo file = _accessor.GetFile(fullName);
            ChangeFileSystemAccessor(new [] {file} );
            return file;
        }

        public FileInfo[] GetFiles(string path)
        {
            FileInfo[] files = _accessor.GetFiles(path);
            FileInfo[] filteredFiles = Filter.FilterFiles(files);
            ChangeFileSystemAccessor(filteredFiles);
            return filteredFiles;
        }

        public FileInfo[] GetFiles(string path, string searchPattern)
        {
            FileInfo[] files = _accessor.GetFiles(path, searchPattern);
            FileInfo[] filteredFiles = Filter.FilterFiles(files);
            ChangeFileSystemAccessor(filteredFiles);
            return filteredFiles;
        }

        public FileSystemInfo[] GetFileSystemInfos(string path)
        {
            List<FileSystemInfo> fileSystemInfos = new List<FileSystemInfo>();
            fileSystemInfos.AddRange(GetDirectories(path));
            fileSystemInfos.AddRange(GetFiles(path));
            ChangeFileSystemAccessor(fileSystemInfos);
            return fileSystemInfos.ToArray();
        }

        public byte[] GetIconBytes(string fullName, IconSize size)
        {
            byte[] bytes = _accessor.GetIconBytes(fullName, size);
            return bytes;
        }

        private void ChangeFileSystemAccessor(IEnumerable<FileSystemInfo> items)
        {
            foreach (FileSystemInfo item in items)
            {
                item.ChangeFileSystemAccessor(this);
            }
        }
    }
}
