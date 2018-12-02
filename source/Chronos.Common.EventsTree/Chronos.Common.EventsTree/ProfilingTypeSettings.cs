using System;

namespace Chronos.Common.EventsTree
{
    [Serializable]
    public class ProfilingTypeSettings : Chronos.ProfilingTypeSettings
    {
        public const ushort DefaultEventsMaxDepth = 10000;
        public const uint DefaultEventsBufferSize = 512 * 1024;
        private static readonly Guid EventsMaxDepthIndex;
        private static readonly Guid EventsBufferSizeIndex;

        static ProfilingTypeSettings()
        {
            EventsMaxDepthIndex = new Guid("4D411B84-7536-441E-8A6E-5839F055119B");
            EventsBufferSizeIndex = new Guid("1471ECF1-7670-4C4D-B0F8-DB4540003D9A");
        }

        public ProfilingTypeSettings(Chronos.ProfilingTypeSettings profilingTypeSettings)
            : base(profilingTypeSettings.GetProperties())
        {
        }

        public ushort EventsMaxDepth
        {
            get { return Get<ushort>(EventsMaxDepthIndex); }
            set { Set(EventsMaxDepthIndex, value); }
        }

        public uint EventsBufferSize
        {
            get { return Get<uint>(EventsBufferSizeIndex); }
            set { Set(EventsBufferSizeIndex, value); }
        }

        public override void Initialize()
        {
            base.Initialize();
            if (!Contains(EventsMaxDepthIndex))
            {
                EventsMaxDepth = DefaultEventsMaxDepth;
            }
            if (!Contains(EventsBufferSizeIndex))
            {
                EventsBufferSize = DefaultEventsMaxDepth;
            }
        }
    }
}
