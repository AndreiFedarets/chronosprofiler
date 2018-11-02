using System.Collections.Generic;
using Chronos.Accessibility.IO;

namespace Chronos.Client.Win.ViewModels.Common
{
    public class OpenFileViewModelSettings
    {
        public OpenFileViewModelSettings(IFileSystemAccessor fileSystemAccessor)
        {
            FileSystemAccessor = fileSystemAccessor;
            Filters = new List<string>();
        }

        public IFileSystemAccessor FileSystemAccessor { get; private set; }

        public List<string> Filters { get; private set; }

        public string InitialDirectory { get; set; }

        public string FileName { get; set; }
    }
}
