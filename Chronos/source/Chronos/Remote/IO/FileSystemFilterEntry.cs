using System;

namespace Chronos.Remote.IO
{
    public sealed class FileSystemFilterEntry
    {
        public FileSystemFilterEntry(string displayName, string extension)
        {
            DisplayName = displayName;
            Extension = extension;
        }

        public string DisplayName { get; private set; }

        public string Extension { get; private set; }

        public static FileSystemFilterEntry FromFullName(string fullName)
        {
            string[] parts = fullName.Split(new[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
            FileSystemFilterEntry entry = null;
            if (parts.Length == 2)
            {
                entry = new FileSystemFilterEntry(parts[0], parts[1]);
            }
            return entry;
        }

        public FileInfo FilterFile(FileInfo file)
        {
            //TODO: implement
            return file;
        }
    }
}
