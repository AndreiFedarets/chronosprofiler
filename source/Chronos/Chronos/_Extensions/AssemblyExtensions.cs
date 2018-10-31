using System.IO;
using System.Reflection;

namespace Chronos
{
    public static class AssemblyExtensions
    {
        public static string GetAssemblyPath(this Assembly assembly)
        {
            string path = assembly.Location;
            path = Path.GetDirectoryName(path);
            return path;
        }
    }
}
