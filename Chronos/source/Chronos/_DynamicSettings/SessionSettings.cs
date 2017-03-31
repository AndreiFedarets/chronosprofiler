using System;
using System.Collections.Generic;

namespace Chronos
{
    [Serializable]
    public sealed class SessionSettings : UniqueSettings
    {
        private static readonly Guid ProfilingTargetSettingsIndex;
        private static readonly Guid FrameworksSettingsIndex;
        private static readonly Guid ProfilingTypesSettingsIndex;
        private static readonly Guid GatewaySettingsIndex;

        static SessionSettings()
        {
            ProfilingTargetSettingsIndex = new Guid("71438E30-4BB7-4A10-BE9F-B2531B690930");
            FrameworksSettingsIndex = new Guid("{ABC75C78-E219-4652-ACD7-5A3749E0CC7E}");
            ProfilingTypesSettingsIndex = new Guid("{71477AC3-3114-43B5-8C06-91889837C6A0}");
            GatewaySettingsIndex = new Guid("{EF63A005-A7FC-4386-A8B1-68418C01671A}");
        }

        public SessionSettings(Guid sessionUid, ProfilingTargetSettings profilingTargetSettings, FrameworkSettingsCollection frameworksSettings,
            ProfilingTypeSettingsCollection profilingTypesSettings, GatewaySettings gatewaySettings)
            : base(sessionUid)
        {
            ProfilingTargetSettings = profilingTargetSettings;
            ProfilingTypesSettings = profilingTypesSettings;
            FrameworksSettings = frameworksSettings;
            GatewaySettings = gatewaySettings;
        }

        public SessionSettings(Dictionary<Guid, DynamicSettingsValue> collection)
            : base(collection)
        {
        }

        public GatewaySettings GatewaySettings
        {
            get { return Get<GatewaySettings>(GatewaySettingsIndex); }
            private set { Set(GatewaySettingsIndex, value); }
        }

        public FrameworkSettingsCollection FrameworksSettings
        {
            get { return Get<FrameworkSettingsCollection>(FrameworksSettingsIndex); }
            private set { Set(FrameworksSettingsIndex, value); }
        }

        public ProfilingTypeSettingsCollection ProfilingTypesSettings
        {
            get { return Get<ProfilingTypeSettingsCollection>(ProfilingTypesSettingsIndex); }
            private set { Set(ProfilingTypesSettingsIndex, value); }
        }

        public ProfilingTargetSettings ProfilingTargetSettings
        {
            get { return Get<ProfilingTargetSettings>(ProfilingTargetSettingsIndex); }
            private set { Set(ProfilingTargetSettingsIndex, value); }
        }

        public override void Validate()
        {
            FrameworksSettings.Validate();
            ProfilingTypesSettings.Validate();
            GatewaySettings.Validate();
        }

        public override DynamicSettings Clone()
        {
            return new SessionSettings(CloneProperties());
        }
    }
}
