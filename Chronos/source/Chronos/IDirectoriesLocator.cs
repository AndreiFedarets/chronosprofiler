using System.Collections.Generic;
using System.IO;

namespace Chronos
{
    public interface IDirectoriesLocator
    {
        IEnumerable<DirectoryInfo> ExtensionsDirectories { get; }

        DirectoryInfo ProfilingResultsDirectory { get; }
    }
}
