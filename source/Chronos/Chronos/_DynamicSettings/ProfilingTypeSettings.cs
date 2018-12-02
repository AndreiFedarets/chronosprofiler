using System;
using System.Collections.Generic;

namespace Chronos
{
    [Serializable]
    public class ProfilingTypeSettings : ExportSettings
    {
        private static readonly Guid DataMarkerIndex;
        private static readonly Guid DependenciesIndex;
        private static readonly Guid FrameworkUidIndex;

        static ProfilingTypeSettings()
        {
            DataMarkerIndex = new Guid("60B683C6-1686-4CCB-B965-856D72E12662");
            DependenciesIndex = new Guid("DEABC098-FD0C-486F-BDEC-2FB369C29058");
            FrameworkUidIndex = new Guid("FF216060-30AA-4E54-8DF0-BAB4BEB2CF6F");
        }

        public ProfilingTypeSettings(Guid uid)
            : base(uid)
        {
            Initialize();
        }

        public ProfilingTypeSettings(Dictionary<Guid, DynamicSettingsValue> collection)
            : base(collection)
        {
            Initialize();
        }

        public byte DataMarker
        {
            get { return Get<byte>(DataMarkerIndex); }
            set { Set(DataMarkerIndex, value); }
        }

        public Guid FrameworkUid
        {
            get { return Get<Guid>(FrameworkUidIndex); }
            set { Set(FrameworkUidIndex, value); }
        }

        public Guid[] Dependencies
        {
            get { return Get<Guid[]>(DependenciesIndex); }
            set { Set(DependenciesIndex, value); }
        }

        public override DynamicSettings Clone()
        {
            ProfilingTypeSettings settings = new ProfilingTypeSettings(CloneProperties());
            return settings;
        }

        public override void Validate()
        {
            base.Validate();
            if (!Contains(DataMarkerIndex))
            {
                throw new TempException();
            }
            if (!Contains(DependenciesIndex))
            {
                throw new TempException();
            }
            if (!Contains(FrameworkUidIndex) || FrameworkUid == Guid.Empty)
            {
                throw new TempException();
            }
        }

        public virtual void Initialize()
        {
            if (!Contains(DataMarkerIndex))
            {
                DataMarker = 0;
            }
            if (!Contains(DependenciesIndex))
            {
                Dependencies = new Guid[0];
            }
            if (!Contains(FrameworkUidIndex))
            {
                FrameworkUid = Guid.Empty;
            }
        }
    }
}
