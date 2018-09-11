using System;
using System.Linq;

namespace Chronos.Remote.IO
{
    public sealed class FileSystemFilterEntry
    {
        public FileSystemFilterEntry(string displayName, string[] extensions)
        {
            DisplayName = displayName;
            Extensions = extensions;
        }

        public string DisplayName { get; private set; }

        public string[] Extensions { get; private set; }

        public static FileSystemFilterEntry FromFullName(string fullName)
        {
            string[] parts = fullName.Split(new[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
            FileSystemFilterEntry entry = null;
            if (parts.Length == 2)
            {
                string displayName = parts[0];
                string[] extensions = ExtractExtensions(parts[1]);
                entry = new FileSystemFilterEntry(displayName, extensions);
            }
            return entry;
        }

        private static string[] ExtractExtensions(string extensionsString)
        {
            string[] extensions = extensionsString.Split(new []{";"}, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < extensions.Length; i++)
            {
                string extension = extensions[i];
                if (extension.StartsWith("*"))
                {
                    extension = extension.Substring(1).ToLowerInvariant();
                    extensions[i] = extension;
                }
            }
            return extensions;
        }

        public FileInfo FilterFile(FileInfo file)
        {
            FileInfo filtered = null;
            string fileExtension = file.Extension;
            if (Extensions.Any(x => string.Equals(fileExtension, x, StringComparison.OrdinalIgnoreCase)))
            {
                filtered = file;
            }
            return filtered;
        }
    }
}
