using System;
using System.Collections.Generic;

namespace Chronos
{
    [Serializable]
    public class FrameworkSettings : ExportSettings
    {
        public FrameworkSettings(Guid uid)
            : base(uid)
        {
        }

        public FrameworkSettings(Dictionary<Guid, DynamicSettingsValue> collection)
            : base(collection)
        {
        }

        public override DynamicSettings Clone()
        {
            FrameworkSettings settings = new FrameworkSettings(CloneProperties());
            return settings;
        }
    }
}
