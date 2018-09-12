using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Chronos.Remote.IO
{
    public sealed class FileSystemAccessor : RemoteBaseObject, IFileSystemAccessor
    {
        private readonly Dictionary<string, DirectoryInfo> _directoryCache;
        private readonly Dictionary<string, FileInfo> _fileCache;
        private readonly bool _readOnly;

        public FileSystemAccessor(bool readOnly)
        {
            _directoryCache = new Dictionary<string, DirectoryInfo>();
            _fileCache = new Dictionary<string, FileInfo>();
            _readOnly = readOnly;
        }

        public bool ReadOnly
        {
            get { return _readOnly; }
        }

        public DirectoryInfo[] GetLogicalDrives()
        {
            string[] drives = System.IO.Directory.GetLogicalDrives();
            DirectoryInfo[] directories = drives.Select(GetDirectory).ToArray();
            return directories;
        }

        public bool DirectoryExists(string fullName)
        {
            return System.IO.Directory.Exists(fullName);
        }

        public DirectoryInfo GetDirectory(string fullName)
        {
            string key = fullName.ToLowerInvariant();
            DirectoryInfo directoryInfo;
            lock (_directoryCache)
            {
                if (!_directoryCache.TryGetValue(key, out directoryInfo))
                {
                    directoryInfo = new DirectoryInfo(fullName, this);
                    _directoryCache.Add(key, directoryInfo);
                }
            }
            return directoryInfo;
        }

        public DirectoryInfo[] GetDirectories(string path)
        {
            string[] pathes = System.IO.Directory.GetDirectories(path);
            DirectoryInfo[] directories = pathes.Select(GetDirectory).ToArray();
            return directories;
        }

        public DirectoryInfo[] GetDirectories(string path, string searchPattern)
        {
            string[] pathes = System.IO.Directory.GetDirectories(path, searchPattern);
            DirectoryInfo[] directories = pathes.Select(GetDirectory).ToArray();
            return directories;
        }

        public bool FileExists(string fullName)
        {
            return System.IO.File.Exists(fullName);
        }

        public FileInfo GetFile(string fullName)
        {
            string key = fullName.ToLowerInvariant();
            FileInfo fileInfo;
            lock (_directoryCache)
            {
                if (!_fileCache.TryGetValue(key, out fileInfo))
                {
                    fileInfo = new FileInfo(fullName, this);
                    _fileCache.Add(key, fileInfo);
                }
            }
            return fileInfo;
        }

        public FileInfo[] GetFiles(string path)
        {
            string[] pathes = System.IO.Directory.GetFiles(path);
            FileInfo[] files = pathes.Select(GetFile).ToArray();
            return files;
        }

        public FileInfo[] GetFiles(string path, string searchPattern)
        {
            string[] pathes = System.IO.Directory.GetFiles(path, searchPattern);
            FileInfo[] files = pathes.Select(GetFile).ToArray();
            return files;
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
            Icon icon = null;
            if (FileExists(fullName))
            {
                icon = Icon.ExtractAssociatedIcon(fullName);//FileSystemInfoExtensions.GetIcon(fullName, size);
            }
            else if (DirectoryExists(fullName))
            {
                icon = FileSystemInfoExtensions.GetIcon(fullName, size);
                
            }
            using (MemoryStream stream = new MemoryStream())
            {
                if (icon != null)
                {
                    Bitmap bitmap = icon.ToBitmap();
                    bitmap.Save(stream, ImageFormat.Png);
                }
                return stream.ToArray();
            }
        }
    }
}
