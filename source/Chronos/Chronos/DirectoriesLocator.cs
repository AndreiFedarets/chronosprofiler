using System.Collections.Generic;
using System.IO;
using Chronos.Settings;

namespace Chronos
{
    public class DirectoriesLocator : IDirectoriesLocator
    {
        private readonly IApplicationSettings  _configurationProvider;

        public DirectoriesLocator(IConfigurationProvider configurationProvider)
        {
            _configurationProvider = configurationProvider;
            ProfilingResultsDirectory = new DirectoryInfo(@"C:\ChronosProfiler2\");
            if (!ProfilingResultsDirectory.Exists)
            {
                ProfilingResultsDirectory.Create();
            }
        }

        public DirectoriesLocator()
            : this(SettingsProvider.Current)
        {
        }

        public DirectoryInfo ProfilingResultsDirectory { get; private set; }

        public IEnumerable<DirectoryInfo> ExtensionsDirectories
        {
            get
            {
                List<DirectoryInfo> directories = new List<DirectoryInfo>();
                ExtensionsDirectoryElementCollection collection = _configurationProvider.Extensions.Directories;
                foreach (ExtensionsDirectoryElement element in collection)
                {
                    string absolutePath = ResolvePath(element.Path);
                    DirectoryInfo directoryInfo = new DirectoryInfo(absolutePath);
                    directories.Add(directoryInfo);
                }
                return directories;
            }
        }

        private string ResolvePath(string relativePath)
        {
            return relativePath;
        }
    }
}
