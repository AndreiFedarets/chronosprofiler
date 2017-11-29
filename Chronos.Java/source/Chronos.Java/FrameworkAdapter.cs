using System;
using System.IO;
using System.Reflection;

namespace Chronos.Java
{
    public class FrameworkAdapter : IFrameworkAdapter, IInitializable, IDisposable
    {
        public FrameworkAdapter()
        {
        }

        public void Initialize(object application)
        {
        }

        public void Dispose()
        {
        }

        public void ConfigureForProfiling(ConfigurationSettings configurationSettings)
        {
            string arguments = configurationSettings.ProfilingTargetSettings.Arguments;
            arguments = string.Format("{0} -agentpath:\"{1}\"", arguments, GetAgentLibraryFullName());
            configurationSettings.ProfilingTargetSettings.Arguments = arguments;
        }

        private string GetAgentLibraryFullName()
        {
            string fullName = Assembly.GetCallingAssembly().Location;
            fullName = Path.GetDirectoryName(fullName);
            fullName = Path.Combine(fullName, Constants.AgentLibraryName);
            return fullName;
        }
    }
}
