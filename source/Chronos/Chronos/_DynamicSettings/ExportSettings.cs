using System;
using System.Collections.Generic;

namespace Chronos
{
    [Serializable]
    public abstract class ExportSettings : UniqueSettings
    {
        private static readonly Guid AgentDllIndex;
        
        static ExportSettings()
        {
            AgentDllIndex = new Guid("5EEB5AEB-20D7-436D-AB60-9DB07834BB72");
        }

        protected ExportSettings(Guid uid)
            : base(uid)
        {
            AgentDll = string.Empty;
        }

        protected ExportSettings(Dictionary<Guid, DynamicSettingsValue> collection)
            : base(collection)
        {
        }

        public string AgentDll
        {
            get { return Get<string>(AgentDllIndex); }
            set { Set(AgentDllIndex, value); }
        }

        public override void Validate()
        {
            base.Validate();
            //NOTE: AgentDll should be presented, but it can be empty - this is correct case (e.g. Performance Counters)
            if (!Contains(AgentDllIndex))
            {
                throw new TempException();
            }
        }
    }
}
