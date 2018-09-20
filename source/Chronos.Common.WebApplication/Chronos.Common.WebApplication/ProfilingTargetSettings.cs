using System;

namespace Chronos.Common.WebApplication
{
    [Serializable]
    public class ProfilingTargetSettings : Chronos.ProfilingTargetSettings
    {
        private static readonly Guid ApplicationPoolIndex;

        static ProfilingTargetSettings()
        {
            ApplicationPoolIndex = new Guid("3014AD9A-6CB9-4D24-9CE9-F699F055D981");
        }

        public ProfilingTargetSettings(Chronos.ProfilingTargetSettings profilingTargetSettings)
            : base(profilingTargetSettings.GetProperties())
        {
        }

        public string ApplicationPool
        {
            get { return Get<string>(ApplicationPoolIndex); }
            set { Set(ApplicationPoolIndex, value); }
        }

        public override void Validate()
        {
            base.Validate();
            if (!Contains(ApplicationPoolIndex))
            {
                throw new TempException();
            }
        }

        public override void Initialize()
        {
            base.Initialize();
            if (!Contains(ApplicationPoolIndex))
            {
                ApplicationPool = string.Empty;
            }
        }
    }
}
