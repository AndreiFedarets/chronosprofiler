using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Windows;
using Chronos.Accessibility.IO;

namespace Chronos.Client.Win.ViewModels.Common
{
    public sealed class OpenFileViewModel : ViewModel
    {
        private readonly ObservableCollection<object> _fileSystemInfos;
        private readonly FilteredFileSystemAccessor _accessor;
        private readonly ParentDirectoryInfo _parentDirectory;
        private DirectoryInfo _currentDirectory;
        private DirectoryInfo _selectedDrive;
        private object _selectedFileSystemInfo;
        private string _currentDirectoryPath;
        private string _filter;

        public OpenFileViewModel(IFileSystemAccessor accessor)
        {
            _accessor = new FilteredFileSystemAccessor(accessor);
            _parentDirectory = new ParentDirectoryInfo();
            _fileSystemInfos = new ObservableCollection<object>();
            Drives = _accessor.GetLogicalDrives();
            SelectedDrive = Drives.FirstOrDefault();
            InitialDirectory = Properties.Settings.Default.OpenFileViewLastUsedPath;
        }

        public override string DisplayName
        {
            get { return "Open File"; }
            set { }
        }

        public string Name
        {
            get
            {
                string name;
                if (_selectedFileSystemInfo is FileInfo)
                {
                    name = ((FileInfo)_selectedFileSystemInfo).Name;
                }
                else
                {
                    name = string.Empty;
                }
                return name;
            }
        }

        public IEnumerable<DirectoryInfo> Drives { get; private set; }

        public DirectoryInfo SelectedDrive
        {
            get { return _selectedDrive; }
            set
            {
                _selectedDrive = value;
                CurrentDirectory = value;
                NotifyOfPropertyChange(() => SelectedDrive);
            }
        }

        public DirectoryInfo CurrentDirectory
        {
            get { return _currentDirectory; }
            private set
            {
                if (!_selectedDrive.Equals(value.Root))
                {
                    _selectedDrive = value.Root;
                    NotifyOfPropertyChange(() => SelectedDrive);
                }
                _fileSystemInfos.Clear();
                if (!value.IsRoot)
                {
                    _fileSystemInfos.Add(_parentDirectory);
                }
                try
                {
                    _currentDirectory = value;
                    foreach (FileSystemInfo info in value.GetFileSystemInfos())
                    {
                        _fileSystemInfos.Add(info);
                    }
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
                finally
                {
                    SelectedFileSystemInfo = _fileSystemInfos.FirstOrDefault(x => x != _parentDirectory);
                    NotifyOfPropertyChange(() => CurrentDirectory);
                    NotifyOfPropertyChange(() => FileSystemInfos);
                    CurrentDirectoryPath = CurrentDirectory.FullName;
                    FileSystemInfosChanged.SafeInvoke(this, EventArgs.Empty);
                }
            }
        }

        public IEnumerable<object> FileSystemInfos
        {
            get { return _fileSystemInfos; }
        }

        public object SelectedFileSystemInfo
        {
            get { return _selectedFileSystemInfo; }
            set
            {
                _selectedFileSystemInfo = value;
                NotifyOfPropertyChange(() => Name);
                NotifyOfPropertyChange(() => SelectedFileSystemInfo);
            }
        }

        public IEnumerable<FileSystemFilterEntry> Filters
        {
            get { return _accessor.Filter.Entries; }
        }

        public FileSystemFilterEntry SelectedFilter
        {
            get { return _accessor.Filter.SelectedEntry; }
            set { _accessor.Filter.SelectedEntry = value; }
        }

        public string CurrentDirectoryPath
        {
            get { return _currentDirectoryPath; }
            private set
            {
                _currentDirectoryPath = value;
                NotifyOfPropertyChange(() => CurrentDirectoryPath);
            }
        }

        public string Filter
        {
            get { return _filter; }
            set
            {
                _filter = value;
                _accessor.Filter.Setup(_filter);
                NotifyOfPropertyChange(() => Filters);
                NotifyOfPropertyChange(() => SelectedFilter);
            }
        }

        public string InitialDirectory { get; set; }

        public string FileName { get; set; }

        internal void OpenParentFileSystemInfo()
        {
            if (SelectedFileSystemInfo == SelectedDrive)
            {
                return;
            }
            SelectedFileSystemInfo = _parentDirectory;
            OpenSelectedFileSystemInfo();
        }

        public event EventHandler FileSystemInfosChanged;

        internal void OpenSelectedFileSystemInfo()
        {
            OpenFileSystemInfo(SelectedFileSystemInfo);
        }

        internal void OpenFileSystemInfo(object fileSystemInfo)
        {
            if (fileSystemInfo is DirectoryInfo)
            {
                CurrentDirectory = (DirectoryInfo)fileSystemInfo;
            }
            else if (fileSystemInfo is ParentDirectoryInfo)
            {
                CurrentDirectory = CurrentDirectory.Parent;
            }
            else if (fileSystemInfo is FileInfo)
            {
                FileName = ((FileInfo)fileSystemInfo).FullName;
                TryClose(true);
            }
        }

        internal void OpenFileSystemInfo(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return;
            }
            object fileSystemInfo;
            if (_accessor.DirectoryExists(path))
            {
                fileSystemInfo = _accessor.GetDirectory(path);
            }
            else if (_accessor.FileExists(path))
            {
                fileSystemInfo = _accessor.GetFile(path);
            }
            else
            {
                string errorMessage = string.Format("Path '{0}' does not exists", path);
                MessageBox.Show(errorMessage);
                return;
            }
            OpenFileSystemInfo(fileSystemInfo);
        }

        protected override void OnDeactivate(bool close)
        {
            Properties.Settings.Default.OpenFileViewLastUsedPath = CurrentDirectoryPath;
            base.OnDeactivate(close);
        }

        internal void Initialize()
        {
            if (!string.IsNullOrWhiteSpace(InitialDirectory))
            {
                OpenFileSystemInfo(InitialDirectory);
            }
        }

        private class ParentDirectoryInfo
        {
            public string Name
            {
                get { return @"[ . . ]"; }
            }

            public Bitmap Icon
            {
                get
                {
                    //TODO: return arrow icon
                    return null;
                }
            }
        }
    }
}
