using Chronos.Settings;
using System.IO;

namespace Chronos
{
    internal static class CrashLogger
    {
        public static void Setup(ICrashDumpSettings settings)
        {
            if (!settings.IsEnabled)
            {
                return;
            }
            DirectoryInfo dumpsDirectory = settings.DumpsDirectory.GetDirectory();
            if (!dumpsDirectory.Exists)
            {
                dumpsDirectory.Create();
            }
            AgentLibrary library = new AgentLibrary();
            library.SetupCrashDumpLogger(dumpsDirectory.FullName);
        }
    }
}
