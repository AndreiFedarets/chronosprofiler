using System;
using System.Collections.Generic;

namespace Chronos
{
    [Serializable]
    public class GatewaySettings : DynamicSettings
    {
        private static readonly Guid SyncStreamsCountIndex;
        private static readonly Guid AsyncStreamsCountIndex;

        static GatewaySettings()
        {
            SyncStreamsCountIndex = new Guid("{435FED7B-0F19-46E3-B10F-C8D7B676A3C9}");
            AsyncStreamsCountIndex = new Guid("{A7017CB0-965A-4F14-A524-5C0A6E3F53E4}");
        }

        public GatewaySettings()
        {
            SyncStreamsCount = 10;
            AsyncStreamsCount = 1;
        }

        public GatewaySettings(Dictionary<Guid, DynamicSettingsValue> properties)
            : base(properties)
        {
        }

        public uint SyncStreamsCount
        {
            get { return Get<uint>(SyncStreamsCountIndex); }
            set { Set(SyncStreamsCountIndex, value); }
        }

        public uint AsyncStreamsCount
        {
            get { return Get<uint>(AsyncStreamsCountIndex); }
            set { Set(AsyncStreamsCountIndex, value); }
        }

        public uint StreamsCount
        {
            get { return SyncStreamsCount + AsyncStreamsCount; }
        }

        public override DynamicSettings Clone()
        {
            GatewaySettings settings = new GatewaySettings(CloneProperties());
            return settings;
        }

        public override void Validate()
        {
            base.Validate();
            if (!Contains(SyncStreamsCountIndex) || SyncStreamsCount == 0)
            {
                throw new TempException();
            }
            if (!Contains(AsyncStreamsCountIndex) || AsyncStreamsCount == 0)
            {
                throw new TempException();
            }
        }
    }
}
