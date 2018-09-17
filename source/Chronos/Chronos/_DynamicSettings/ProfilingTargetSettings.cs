using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Chronos
{
    [Serializable]
    public class ProfilingTargetSettings : ExportSettings
    {
        private static readonly Guid ProfileChildProcessIndex;
        private static readonly Guid EnvironmentVariablesIndex;
        private static readonly Guid ConsoleSessionIndex;

        static ProfilingTargetSettings()
        {
            EnvironmentVariablesIndex = new Guid("0F488802-DADA-402B-8734-7C23FC9EB14F");
            ConsoleSessionIndex = new Guid("6724AE49-474A-4950-B450-920B5776F559");
            ProfileChildProcessIndex = new Guid("ADFF8AA5-C65F-4034-81E5-FE682574ED64");
        }

        public ProfilingTargetSettings(Guid uid)
            : base(uid)
        {
            Initialize();
        }

        public ProfilingTargetSettings(Dictionary<Guid, DynamicSettingsValue> properties)
            : base(properties)
        {
            Initialize();
        }

        public int ConsoleSession
        {
            get { return Get<int>(ConsoleSessionIndex); }
            set { Set(ConsoleSessionIndex, value); }
        }

        public StringDictionary EnvironmentVariables
        {
            get { return Get<StringDictionary>(EnvironmentVariablesIndex); }
            set { Set(EnvironmentVariablesIndex, value); }
        }

        public bool ProfileChildProcess
        {
            get { return Get<bool>(ProfileChildProcessIndex); }
            set { Set(ProfileChildProcessIndex, value); }
        }

        public override DynamicSettings Clone()
        {
            ProfilingTargetSettings settings = new ProfilingTargetSettings(CloneProperties());
            return settings;
        }

        public override void Validate()
        {
            if (!Contains(EnvironmentVariablesIndex))
            {
                throw new TempException();
            }
            if (!Contains(ProfileChildProcessIndex))
            {
                throw new TempException();
            }
        }

        public virtual void Initialize()
        {
            if (!Contains(EnvironmentVariablesIndex))
            {
                EnvironmentVariables = new StringDictionary();   
            }
            if (!Contains(ConsoleSessionIndex))
            {
                ConsoleSession = 0;
            }
            if (!Contains(ProfileChildProcessIndex))
            {
                ProfileChildProcess = false;   
            }
        }
    }
}
