using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;

namespace Chronos
{
    [Serializable]
    public class ProfilingTargetSettings : ExportSettings
    {
        private static readonly Guid ProfileChildProcessIndex;
        private static readonly Guid FileFullNameIndex;
        private static readonly Guid EnvironmentVariablesIndex;
        private static readonly Guid ArgumentsIndex;
        private static readonly Guid WorkingDirectoryIndex;
        private static readonly Guid ConsoleSessionIndex;

        static ProfilingTargetSettings()
        {
            FileFullNameIndex = new Guid("5E84236E-304B-411C-8A56-1CC02F49D79D");
            EnvironmentVariablesIndex = new Guid("0F488802-DADA-402B-8734-7C23FC9EB14F");
            ArgumentsIndex = new Guid("3249D1C5-187A-4E6C-B88B-BDFEA02BA2C9");
            WorkingDirectoryIndex = new Guid("315A6B36-B9F4-4567-B2BE-A45E1E3C29E2");
            ConsoleSessionIndex = new Guid("6724AE49-474A-4950-B450-920B5776F559");
            ProfileChildProcessIndex = new Guid("ADFF8AA5-C65F-4034-81E5-FE682574ED64");
        }

        public ProfilingTargetSettings(Guid uid)
            : base(uid)
        {
            EnvironmentVariables = new StringDictionary();
            Arguments = string.Empty;
            FileFullName = string.Empty;
            WorkingDirectory = string.Empty;
            ConsoleSession = 0;
            ProfileChildProcess = false;
        }

        public ProfilingTargetSettings(Dictionary<Guid, DynamicSettingsValue> properties)
            : base(properties)
        {
            
        }

        public int ConsoleSession
        {
            get { return Get<int>(ConsoleSessionIndex); }
            set { Set(ConsoleSessionIndex, value); }
        }

        public string FileFullName
        {
            get { return Get<string>(FileFullNameIndex); }
            set
            {
                Set(FileFullNameIndex, value);
                if (!string.IsNullOrWhiteSpace(value))
                {
                    WorkingDirectory = Path.GetDirectoryName(value);
                }
            }
        }

        public StringDictionary EnvironmentVariables
        {
            get { return Get<StringDictionary>(EnvironmentVariablesIndex); }
            set { Set(EnvironmentVariablesIndex, value); }
        }

        public string Arguments
        {
            get { return Get<string>(ArgumentsIndex); }
            set { Set(ArgumentsIndex, value); }
        }

        public string WorkingDirectory
        {
            get { return Get<string>(WorkingDirectoryIndex); }
            set { Set(WorkingDirectoryIndex, value); }
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
            if (!Contains(ArgumentsIndex))
            {
                throw new TempException();
            }
            if (!Contains(ProfileChildProcessIndex))
            {
                throw new TempException();
            }
        }
    }
}
