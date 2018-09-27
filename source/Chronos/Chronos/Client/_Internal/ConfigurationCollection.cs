using System;

namespace Chronos.Client
{
    internal sealed class ConfigurationCollection : ObservableDictionary<Guid, IConfiguration>, IConfigurationCollection
    {
        private readonly Host.IApplicationCollection _hostApplications;
        private Delegate _configurationCreated;
        private Delegate _configurationRemoved;

        public ConfigurationCollection(Host.IApplicationCollection hostApplications)
        {
            _hostApplications = hostApplications;
            InitializeCollection();
        }

        public IConfiguration this[Guid configurationUid]
        {
            get
            {
                lock (Lock)
                {
                    VerifyDisposed();
                    IConfiguration configuration;
                    if (!TryGetValue(configurationUid, out configuration))
                    {
                        throw new ConfigurationNotFoundException(configurationUid);
                    }
                    return configuration;
                }
            }
        }

        public event EventHandler<ConfigurationEventArgs> ConfigurationCreated
        {
            add
            {
                VerifyDisposed();
                _configurationCreated = Delegate.Combine(_configurationCreated, value);
            }
            remove
            {
                VerifyDisposed();
                _configurationCreated = Delegate.Remove(_configurationCreated, value);
            }
        }

        public event EventHandler<ConfigurationEventArgs> ConfigurationRemoved
        {
            add
            {
                VerifyDisposed();
                _configurationRemoved = Delegate.Combine(_configurationRemoved, value);
            }
            remove
            {
                VerifyDisposed();
                _configurationRemoved = Delegate.Remove(_configurationRemoved, value);
            }
        }

        public IConfiguration Create(ConfigurationSettings configurationSettings)
        {
            throw new NotSupportedException("This operation is not supported directly from Client. Please use Host application to create Configuration.");
        }

        public bool Contains(Guid configurationToken)
        {
            lock (Lock)
            {
                VerifyDisposed();
                return ContainsKey(configurationToken);
            }
        }

        public override void Dispose()
        {
            lock (Lock)
            {
                VerifyDisposed();
                _hostApplications.ApplicationConnected -= OnHostApplicationConnected;
                _hostApplications.ApplicationDisconnected -= OnHostApplicationDisconnected;
                foreach (Host.IApplication application in _hostApplications)
                {
                    DisposeApplication(application);
                }
                base.Dispose();
            }
        }

        private void InitializeCollection()
        {
            lock (Lock)
            {
                VerifyDisposed();
                _hostApplications.ApplicationConnected += OnHostApplicationConnected;
                _hostApplications.ApplicationDisconnected += OnHostApplicationDisconnected;
                foreach (Host.IApplication hostApplication in _hostApplications)
                {
                    InitializeApplication(hostApplication);
                }
            }
        }

        private void OnHostApplicationConnected(object sender, Host.ApplicationEventArgs e)
        {
            Host.IApplication hostApplication = e.Application;
            lock (Lock)
            {
                VerifyDisposed();
                InitializeApplication(hostApplication);
            }
        }

        private void InitializeApplication(Host.IApplication hostApplication)
        {
            IConfigurationCollection hostConfigurations = hostApplication.Configurations;
            hostConfigurations.ConfigurationCreated += OnHostConfigurationCreated;
            hostConfigurations.ConfigurationRemoved += OnHostConfigurationRemoved;
            foreach (IConfiguration hostConfiguration in hostConfigurations)
            {
                Add(hostConfiguration.Uid, hostConfiguration);
            }
        }

        private void OnHostApplicationDisconnected(object sender, Host.ApplicationEventArgs e)
        {
            Host.IApplication hostApplication = e.Application;
            lock (Lock)
            {
                VerifyDisposed();
                DisposeApplication(hostApplication);
            }
        }

        private void DisposeApplication(Host.IApplication hostApplication)
        {
            IConfigurationCollection hostConfigurations = hostApplication.Configurations;
            hostConfigurations.ConfigurationCreated -= OnHostConfigurationCreated;
            hostConfigurations.ConfigurationRemoved -= OnHostConfigurationRemoved;
            foreach (IConfiguration configuration in this)
            {
                if (configuration.Application == hostApplication)
                {
                    Remove(configuration.Uid);
                }
            }
        }

        private void OnHostConfigurationCreated(object sender, ConfigurationEventArgs e)
        {
            IConfiguration hostConfiguration = e.Configuration;
            lock (Lock)
            {
                VerifyDisposed();
                if (!ContainsKey(hostConfiguration.Uid))
                {
                    Add(hostConfiguration.Uid, hostConfiguration);
                }
            }
            ConfigurationEventArgs.RaiseEvent((EventHandler<ConfigurationEventArgs>)_configurationCreated, this, hostConfiguration);
        }

        private void OnHostConfigurationRemoved(object sender, ConfigurationEventArgs e)
        {
            IConfiguration hostConfiguration = e.Configuration;
            bool configurationRemoved;
            lock (Lock)
            {
                VerifyDisposed();
                configurationRemoved = Remove(hostConfiguration.Uid);
            }
            if (configurationRemoved)
            {
                ConfigurationEventArgs.RaiseEvent((EventHandler<ConfigurationEventArgs>)_configurationRemoved, this, hostConfiguration);
            }
        }
    }
}
