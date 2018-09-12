using System;
using System.Xml.Linq;

namespace Chronos.Settings
{
    internal sealed class FileSystemSettings : SettingsElement, IFileSystemSettings
    {
        private const string ExtensionsElementName = "Extensions";
        private const string ProfilingResultsElementName = "ProfilingResuls";

        public FileSystemSettings(XElement element)
            : base(element)
        {
            Extensions = new DirectorySettingsCollection(element.Element(ExtensionsElementName));
            ProfilingResults = new DirectorySettings(element.Element(ProfilingResultsElementName));
        }

        public IDirectorySettingsCollection Extensions { get; private set; }

        public IDirectorySettings ProfilingResults { get; private set; }
    }
}
