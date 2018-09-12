using System;

namespace Chronos.Common.StandaloneApplication
{
    [Serializable]
    public class ProfilingTargetSettings : Chronos.ProfilingTargetSettings
    {
        public ProfilingTargetSettings(Chronos.ProfilingTargetSettings profilingTargetSettings)
            : base(profilingTargetSettings.GetProperties())
        {
        }
    }
}
