using System;

namespace Chronos.Accessibility.IO
{
    [Serializable]
    public sealed class DirectoryInfo : FileSystemInfo
    {
        internal DirectoryInfo(string fullName, IFileSystemAccessor accessor)
            : base (fullName.Length == 2 ? fullName + System.IO.Path.DirectorySeparatorChar : fullName, accessor)
        {
        }

        public override bool Exists
        {
            get { return Accessor.DirectoryExists(FullName); }
        }

        public override string Name
        {
            get
            {
                var name = FullName.Length <= 3 ? FullName : base.Name;
                return name;
            }
        }

        public bool IsRoot
        {
            get { return object.Equals(Root, this); }
        }

        public DirectoryInfo[] GetDirectories()
        {
            DirectoryInfo[] directories = Accessor.GetDirectories(FullName);
            return directories;
        }

        public DirectoryInfo[] GetDirectories(string searchPattern)
        {
            DirectoryInfo[] directories = Accessor.GetDirectories(FullName, searchPattern);
            return directories;
        }

        public FileInfo[] GetFiles()
        {
            FileInfo[] files = Accessor.GetFiles(FullName);
            return files;
        }

        public FileInfo[] GetFiles(string searchPattern)
        {
            FileInfo[] files = Accessor.GetFiles(FullName, searchPattern);
            return files;
        }

        public FileSystemInfo[] GetFileSystemInfos()
        {
            FileSystemInfo[] infos = Accessor.GetFileSystemInfos(FullName);
            return infos;
        }
    }
}
