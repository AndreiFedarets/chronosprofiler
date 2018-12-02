using System;

namespace Chronos.DotNet.TracingProfiler
{
    [Serializable]
    public class ProfilingTypeSettings : Chronos.ProfilingTypeSettings
    {
        private static readonly Guid ExclusionsIndex;
        private static readonly Guid EnableExclusionsIndex;

        static ProfilingTypeSettings()
        {
            ExclusionsIndex = new Guid("9049FDEB-CE96-447F-A5C1-73B77D2BCD43");
            EnableExclusionsIndex = new Guid("B9BDDBCF-2AAE-4A9F-97C9-187E0E558FAE");
        }

        public ProfilingTypeSettings(Chronos.ProfilingTypeSettings profilingTypeSettings)
            : base(profilingTypeSettings.GetProperties())
        {
        }

        public bool EnableExclusions
        {
            get { return Get<bool>(EnableExclusionsIndex); }
            set { Set(EnableExclusionsIndex, value); }
        }

        public string[] Exclusions
        {
            get { return Get<string[]>(ExclusionsIndex); }
            set { Set(ExclusionsIndex, value); }
        }

        public override void Initialize()
        {
            base.Initialize();
            if (!Contains(EnableExclusionsIndex))
            {
                //EnableExclusions = false;
                EnableExclusions = true;
            }
            if (!Contains(ExclusionsIndex))
            {
                Exclusions = new string[0];
            }
        }
    }
}
