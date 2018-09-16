using System;
using System.IO;

namespace Chronos.Common.StandaloneApplication
{
    [Serializable]
    public class ProfilingTargetSettings : Chronos.ProfilingTargetSettings
    {
        private static readonly Guid FileFullNameIndex;
        private static readonly Guid WorkingDirectoryIndex;
        private static readonly Guid ArgumentsIndex;

        static ProfilingTargetSettings()
        {
            FileFullNameIndex = new Guid("5E84236E-304B-411C-8A56-1CC02F49D79D");
            WorkingDirectoryIndex = new Guid("315A6B36-B9F4-4567-B2BE-A45E1E3C29E2");
            ArgumentsIndex = new Guid("3249D1C5-187A-4E6C-B88B-BDFEA02BA2C9");
        }

        public ProfilingTargetSettings(Chronos.ProfilingTargetSettings profilingTargetSettings)
            : base(profilingTargetSettings.GetProperties())
        {
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

        public string WorkingDirectory
        {
            get { return Get<string>(WorkingDirectoryIndex); }
            set { Set(WorkingDirectoryIndex, value); }
        }

        public string Arguments
        {
            get { return Get<string>(ArgumentsIndex); }
            set { Set(ArgumentsIndex, value); }
        }

        public override void Validate()
        {
            base.Validate();
            if (!Contains(FileFullNameIndex))
            {
                throw new TempException();
            }
            if (!Contains(ArgumentsIndex))
            {
                throw new TempException();
            }
        }

        public override void Initialize()
        {
            base.Initialize();
            if (!Contains(FileFullNameIndex))
            {
                FileFullName = string.Empty;   
            }
            if (!Contains(WorkingDirectoryIndex))
            {
                WorkingDirectory = string.Empty;   
            }
            if (!Contains(ArgumentsIndex))
            {
                Arguments = string.Empty;   
            }
        }
    }
}
