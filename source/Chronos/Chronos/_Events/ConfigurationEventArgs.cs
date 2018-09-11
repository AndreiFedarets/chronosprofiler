using System;

namespace Chronos
{
    [Serializable]
    public sealed class ConfigurationEventArgs : EventArgs
    {
        public ConfigurationEventArgs(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; private set; }

        public static void RaiseEvent(EventHandler<ConfigurationEventArgs> handler, object sender, IConfiguration configuration)
        {
            EventExtensions.RaiseEventSafeAndSilent(handler, sender, () => new ConfigurationEventArgs(configuration));
        }
    }
}
