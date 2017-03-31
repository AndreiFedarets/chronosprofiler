using System;
using System.Collections.Generic;

namespace Chronos
{
    [Serializable]
    public sealed class ProfilingTypeSettingsCollection : UniqueSettingsCollection<ProfilingTypeSettings>
    {
        public ProfilingTypeSettingsCollection()
            : this(new Dictionary<Guid, ProfilingTypeSettings>())
        {
        }

        private ProfilingTypeSettingsCollection(Dictionary<Guid, ProfilingTypeSettings> collection)
            : base(collection)
        {
        }

        public ProfilingTypeSettingsCollection(IEnumerable<ProfilingTypeSettings> collection)
            : base(collection)
        {
        }

        public ProfilingTypeSettingsCollection Clone()
        {
            Dictionary<Guid, ProfilingTypeSettings> collection = CloneSettings();
            ProfilingTypeSettingsCollection settings = new ProfilingTypeSettingsCollection(collection);
            return settings;
        }

        public override void Validate()
        {
            base.Validate();
            lock (Collection)
            {
                List<byte> usedDataMarkers = new List<byte>();
                foreach (ProfilingTypeSettings settings in Collection.Values)
                {
                    if (usedDataMarkers.Contains(settings.DataMarker))
                    {
                        throw new TempException();
                    }
                    usedDataMarkers.Add(settings.DataMarker);
                }
            }
        }

        protected override void OnSettingsAdded(ProfilingTypeSettings element)
        {
            base.OnSettingsAdded(element);
            RemapDataMarkers();
        }

        protected override void OnSettingsRemoved(ProfilingTypeSettings element)
        {
            base.OnSettingsRemoved(element);
            RemapDataMarkers();
        }

        private void RemapDataMarkers()
        {
            byte dataMarker = 0;
            foreach (ProfilingTypeSettings settings in Collection.Values)
            {
                settings.DataMarker = dataMarker;
                dataMarker++;
            }
        }

    }
}
