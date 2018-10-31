using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Adenium
{
    public static class AssemblyExtensions
    {
        public static bool IsWPFAssembly(this Assembly assembly)
        {
            return assembly.GetReferencedAssemblies().Any(
                    x => string.Equals(x.Name, "PresentationFramework", StringComparison.OrdinalIgnoreCase));
        }

        public static string GetAssemblyPath(this Assembly assembly)
        {
            string path = assembly.Location;
            path = Path.GetDirectoryName(path);
            return path;
        }
    }
}
