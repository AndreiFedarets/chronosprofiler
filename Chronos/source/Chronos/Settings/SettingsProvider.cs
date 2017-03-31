namespace Chronos.Settings
{
    public static class SettingsProvider
    {
        private static readonly object Lock;
        private static IApplicationSettings _current;

        static SettingsProvider()
        {
            Lock = new object();
        }

        public static IApplicationSettings Current
        {
            get
            {
                if (_current == null)
                {
                    lock (Lock)
                    {
                        if (_current == null)
                        {
                            ApplicationSettings current = new ApplicationSettings();
                            current.Initialize();
                            _current = current;
                        }
                    }
                }
                return _current;
            }
        }
    }
}
