using System;

namespace ChronosBuildTool.Model
{
    internal sealed class ExternalsFolder
    {
        public ExternalsFolder(string debug, string release)
        {
            Debug = debug;
            Release = release;
        }

        public string Debug { get; private set; }

        public string Release { get; private set; }

        public string this[Configuration configuration]
        {
            get
            {
                switch (configuration)
                {
                    case Configuration.Debug:
                        return Debug;
                    case Configuration.Release:
                        return Release;
                    default:
                        throw new ArgumentException("configuration");
                }
            }
        }
    }
}
