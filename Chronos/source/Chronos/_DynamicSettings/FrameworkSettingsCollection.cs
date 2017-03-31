using System;
using System.Collections.Generic;

namespace Chronos
{
    [Serializable]
    public sealed class FrameworkSettingsCollection : UniqueSettingsCollection<FrameworkSettings>
    {
        public FrameworkSettingsCollection()
            : this(new Dictionary<Guid, FrameworkSettings>())
        {
        }

        private FrameworkSettingsCollection(Dictionary<Guid, FrameworkSettings> collection)
            : base(collection)
        {
        }

        public FrameworkSettingsCollection(IEnumerable<FrameworkSettings> collection)
            : base(collection)
        {
        }

        public FrameworkSettingsCollection Clone()
        {
            Dictionary<Guid, FrameworkSettings> collection = CloneSettings();
            FrameworkSettingsCollection settings = new FrameworkSettingsCollection(collection);
            return settings;
        }
    }
}
