using System.IO;
using System.Reflection;

namespace Chronos.DotNet.TracingProfiler
{
    internal static class ExclusionsProvider
    {
        private const string DefaultExclusionsFileName = "DefaultExclusions.txt";

        public static string[] GetDefaultExclusions()
        {
            string filePath = Assembly.GetExecutingAssembly().GetAssemblyPath();
            string fileFullName = Path.Combine(filePath, DefaultExclusionsFileName);
            if (!File.Exists(fileFullName))
            {
                return new string[0];
            }
            string[] exclusions = File.ReadAllLines(fileFullName);
            return exclusions;
        }
    }
}
