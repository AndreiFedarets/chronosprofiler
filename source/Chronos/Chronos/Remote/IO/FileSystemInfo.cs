using System;
using System.Drawing;
using System.IO;

namespace Chronos.Remote.IO
{
    [Serializable]
    public abstract class FileSystemInfo
    {
        private DirectoryInfo _parent;
        private DirectoryInfo _root;
        private Bitmap _icon;

        protected FileSystemInfo(string fullName, IFileSystemAccessor accessor)
        {
            FullName = System.IO.Path.GetFullPath(fullName);
            Accessor = accessor;
        }

        protected IFileSystemAccessor Accessor { get; private set; }

        public string FullName { get; private set; }

        public virtual string Name
        {
            get { return System.IO.Path.GetFileName(FullName); }
        }

        public virtual string Path
        {
            get { return System.IO.Path.GetDirectoryName(FullName); }
        }

        public abstract bool Exists { get; }

        public Bitmap Icon
        {
            get
            {
                if (_icon == null)
                {
                    byte[] bytes = Accessor.GetIconBytes(FullName, IconSize.Large);
                    if (bytes != null && bytes.Length > 0)
                    {
                        using (MemoryStream ms = new MemoryStream(bytes))
                        {
                            _icon = new Bitmap(ms);
                        }
                    }
                }
                return _icon;
            }
        }

        public DirectoryInfo Parent
        {
            get
            {
                if (_parent == null && !string.IsNullOrWhiteSpace(Path))
                {
                    _parent = new DirectoryInfo(Path, Accessor);
                }
                return _parent;
            }
        }

        public DirectoryInfo Root
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Path))
                {
                    return (DirectoryInfo)this;
                }
                if (_root == null)
                {
                    string rootPath = System.IO.Path.GetPathRoot(Path);
                    _root = new DirectoryInfo(rootPath, Accessor);
                }
                return _root;
            }
        }

        internal void ChangeFileSystemAccessor(IFileSystemAccessor accessor)
        {
            Accessor = accessor;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != GetType())
            {
                return false;
            }
            return Equals((FileSystemInfo) obj);
        }

        protected bool Equals(FileSystemInfo other)
        {
            return string.Equals(FullName, other.FullName, StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode()
        {
            return (FullName != null ? FullName.GetHashCode() : 0);
        }
    }
}
