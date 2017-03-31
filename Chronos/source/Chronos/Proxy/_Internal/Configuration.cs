using System;

namespace Chronos.Proxy
{
    internal sealed class Configuration : ProxyBaseObject<IConfiguration>, IConfiguration
    {
        private readonly Chronos.Host.IApplication _application;

        public Configuration(IConfiguration configuration, Chronos.Host.IApplication application)
            : base(configuration)
        {
            _application = application;
            Uid = Execute(() => RemoteObject.Uid);
        }

        public Guid Uid { get; private set; }

        public string Name
        {
            get { return Execute(() => RemoteObject.Name); }
        }

        public bool IsActive
        {
            get { return Execute(() => RemoteObject.IsActive); }
        }

        public Chronos.Host.IApplication Application
        {
            get { return _application; }
        }

        public ConfigurationSettings ConfigurationSettings
        {
            get
            {
                return Execute(() => RemoteObject.ConfigurationSettings);
            }
        }

        public void Activate()
        {
            Execute(() => RemoteObject.Activate());
        }

        public void Deactivate()
        {
            Execute(() => RemoteObject.Deactivate());
        }

        public void Remove()
        {
            Execute(() => RemoteObject.Remove());
        }
    }
}
