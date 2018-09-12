using System;
using System.Configuration;
using System.Diagnostics;

namespace Chronos.Config
{
    public class ConfigurationProvider : IConfigurationProvider
    {
        private static readonly object Lock;
        private static IConfigurationProvider _current;
        private System.Configuration.Configuration _configuration;

        static ConfigurationProvider()
        {
            Lock = new object();
        }

        private ConfigurationProvider()
        {
            
        }

        public static IConfigurationProvider Current
        {
            get
            {
                if (_current == null)
                {
                    lock (Lock)
                    {
                        if (_current == null)
                        {
                            ConfigurationProvider current = new ConfigurationProvider();
                            current.Initialize();
                            _current = current;
                        }
                    }
                }
                return _current;
            }
        }

        public DaemonSection Daemon { get; private set; }

        public CommunicationSection Communication { get; private set; }

        public ExtensionsSection Extensions { get; private set; }

        public LoggingSection Logging { get; private set; }

        private void Initialize()
        {
            string dllConfigPath = GetType().Assembly.Location;
            try
            {
                _configuration = ConfigurationManager.OpenExeConfiguration(dllConfigPath);
                Communication = (CommunicationSection)_configuration.GetSection("communication");
                Extensions = (ExtensionsSection) _configuration.GetSection("extensions");
                Daemon = (DaemonSection)_configuration.GetSection("daemon");
                Logging = (LoggingSection)_configuration.GetSection("logging");
            }
            catch (Exception exception)
            {
                LoggingProvider.Current.Log(TraceEventType.Critical, exception);
                throw;
            }
        }
    }
}
