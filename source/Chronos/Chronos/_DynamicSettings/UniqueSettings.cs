using System;
using System.Collections.Generic;

namespace Chronos
{
    [Serializable]
    public abstract class UniqueSettings : DynamicSettings
    {
        private static readonly Guid UidIndex;

        static UniqueSettings()
        {
            UidIndex = new Guid("E36CFA00-0712-45B4-8423-17846C318208");
        }
        
        protected UniqueSettings(Guid uid)
        {
            Uid = uid;
        }

        protected UniqueSettings(Dictionary<Guid, DynamicSettingsValue> collection)
            : base(collection)
        {
        }

        public Guid Uid
        {
            get { return Get<Guid>(UidIndex); }
            set { Set(UidIndex, value); }
        }

        public override void Validate()
        {
            base.Validate();
            if (!Contains(UidIndex) || Uid == Guid.Empty)
            {
                throw new TempException();
            }
        }
    }
}
