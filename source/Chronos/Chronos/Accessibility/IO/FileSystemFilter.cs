using System;
using System.Collections.Generic;
using System.Linq;

namespace Chronos.Accessibility.IO
{
    public class FileSystemFilter
    {
        private const string DefaultFilter = "(*.*)|*.*";
        private readonly List<FileSystemFilterEntry> _entries;
        private FileSystemFilterEntry _selectedEntry;

        public FileSystemFilter()
        {
            _entries = new List<FileSystemFilterEntry>();
        }

        public IEnumerable<FileSystemFilterEntry> Entries
        {
            get { return _entries; }
        }

        public FileSystemFilterEntry SelectedEntry
        {
            get { return _selectedEntry; }
            set
            {
                if (_entries.Contains(value))
                {
                    _selectedEntry = value;
                }
            }
        }

        public void Setup(string filter)
        {
            _entries.Clear();
            if (string.IsNullOrWhiteSpace(filter))
            {
                filter = DefaultFilter;
            }
            string[] fullNames = filter.Split(new [] {";"}, StringSplitOptions.RemoveEmptyEntries);
            foreach (string fullName in fullNames)
            {
                FileSystemFilterEntry entry = FileSystemFilterEntry.FromFullName(fullName);
                if (entry != null)
                {
                    _entries.Add(entry);
                }
            }
            _selectedEntry = _entries.FirstOrDefault();
        }

        public FileInfo[] FilterFiles(FileInfo[] files)
        {
            List<FileInfo> filteredFiles = new List<FileInfo>();
            foreach (FileInfo file in files)
            {
                FileInfo filteredFile = FilterFile(file);
                if (filteredFile != null)
                {
                    filteredFiles.Add(filteredFile);
                }
            }
            return filteredFiles.ToArray();
        }

        public FileInfo FilterFile(FileInfo file)
        {
            FileInfo filteredFile = null;
            if (_selectedEntry != null)
            {
                filteredFile = _selectedEntry.FilterFile(file);
            }
            return filteredFile;
        }
    }
}
