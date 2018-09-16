using System;

namespace Chronos.Common.ServiceApplication
{
    [Serializable]
    public class ProfilingTargetSettings : Chronos.ProfilingTargetSettings
    {
        private static readonly Guid ServiceNameIndex;

        static ProfilingTargetSettings()
        {
            ServiceNameIndex = new Guid("35AED0CD-CAEB-4E5E-9BA7-E2ECFEAA8763");
        }

        public ProfilingTargetSettings(Chronos.ProfilingTargetSettings profilingTargetSettings)
            : base(profilingTargetSettings.GetProperties())
        {
        }

        public string ServiceName
        {
            get { return Get<string>(ServiceNameIndex); }
            set { Set(ServiceNameIndex, value); }
        }

        public override void Validate()
        {
            base.Validate();
            if (!Contains(ServiceNameIndex))
            {
                throw new TempException();
            }
        }

        public override void Initialize()
        {
            base.Initialize();
            if (!Contains(ServiceNameIndex))
            {
                ServiceName = string.Empty;   
            }
        }
    }
}
