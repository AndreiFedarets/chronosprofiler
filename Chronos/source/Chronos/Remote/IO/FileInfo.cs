using System;

namespace Chronos.Remote.IO
{
    [Serializable]
    public sealed class FileInfo : FileSystemInfo
    {
        internal FileInfo(string fullName, IFileSystemAccessor accessor)
            : base (fullName, accessor)
        {
        }

        public override bool Exists
        {
            get { return Accessor.FileExists(FullName); }
        }
    }
}
