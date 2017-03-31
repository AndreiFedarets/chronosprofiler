using System.IO;
using System.Linq;

namespace Chronos
{
    public static class DirectoryInfoExtensions
    {
        public static FileInfo[] GetFilesFixed(this DirectoryInfo directoryinfo, string searchPattern)
        {
            if (((searchPattern.Length - (searchPattern.LastIndexOf('.') + 1)) == 3) && !
            searchPattern.Substring(searchPattern.LastIndexOf('.')).Contains('*'))
                return directoryinfo.GetFiles(searchPattern).ToList().FindAll
                (F => F.Extension.Length == 4).ToArray();
            return directoryinfo.GetFiles(searchPattern);
        }

        public static FileInfo[] GetFilesFixed(this DirectoryInfo directoryinfo, string searchPattern, SearchOption searchOption)
        {
            if (((searchPattern.Length - (searchPattern.LastIndexOf('.') + 1)) == 3) && !
            searchPattern.Substring(searchPattern.LastIndexOf('.')).Contains('*'))
                return directoryinfo.GetFiles(searchPattern, searchOption).ToList().FindAll
                (f => f.Extension.Length == 4).ToArray();
            return directoryinfo.GetFiles(searchPattern);
        }
    }
}
