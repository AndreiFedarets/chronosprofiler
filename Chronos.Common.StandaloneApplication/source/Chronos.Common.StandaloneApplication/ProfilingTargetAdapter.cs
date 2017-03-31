using System;
using System.Diagnostics;
using System.IO;

namespace Chronos.Common.StandaloneApplication
{
    public class ProfilingTargetAdapter : IProfilingTargetAdapter
    {
        public IProfilingTargetController CreateController(ConfigurationSettings settings)
        {
            ProfilingTargetSettings profilingTargetSettings = new ProfilingTargetSettings(settings.ProfilingTargetSettings);
            ProfilingTargetController controller = new ProfilingTargetController(profilingTargetSettings);
            return controller;
        }

        public bool CanStartProfiling(ConfigurationSettings settings, int processId)
        {
            using (Process process = Process.GetProcessById(processId))
            {
                ProfilingTargetSettings profilingTargetSettings = new ProfilingTargetSettings(settings.ProfilingTargetSettings);
                string targetProcessName = Path.GetFileNameWithoutExtension(profilingTargetSettings.FileFullName);
                if (!string.Equals(process.ProcessName, targetProcessName, StringComparison.InvariantCultureIgnoreCase))
                {
                    return false;
                }
            }
            return true;
        }

        public void ProfilingStarted(ConfigurationSettings configurationSettings, SessionSettings sessionSettings, int processId)
        {
        }
    }
}
