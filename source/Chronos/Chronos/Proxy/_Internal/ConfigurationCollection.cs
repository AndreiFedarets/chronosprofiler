using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Chronos.Proxy
{
    internal sealed class ConfigurationCollection : ProxyBaseObject<IConfigurationCollection>, IConfigurationCollection
    {
        private readonly Chronos.Host.IApplication _application;
        private readonly Dictionary<Guid, Configuration> _collection;
        private readonly RemoteEventRouter<ConfigurationEventArgs> _configurationCreatedEventSink;
        private readonly RemoteEventRouter<ConfigurationEventArgs> _configurationRemovedEventSink;
        private Delegate _configurationCreated;
        private Delegate _configurationRemoved;
        private volatile bool _initialized;

        public ConfigurationCollection(IConfigurationCollection configurations, Chronos.Host.IApplication application)
            : base(configurations)
        {
            _initialized = false;
            _application = application;
            _collection = new Dictionary<Guid, Configuration>();
            _configurationCreatedEventSink = new RemoteEventRouter<ConfigurationEventArgs>(RemoteObject, "ConfigurationCreated", this);
            _configurationRemovedEventSink = new RemoteEventRouter<ConfigurationEventArgs>(RemoteObject, "ConfigurationRemoved", this);
        }

        public IConfiguration this[Guid uid]
        {
            get
            {
                lock (_collection)
                {
                    VerifyDisposed();
                    Configuration configuration;
                    if (_collection.TryGetValue(uid, out configuration))
                    {
                        return configuration;
                    }
                    IConfiguration remoteConfiguration = Execute(() => RemoteObject[uid]);
                    configuration = new Configuration(remoteConfiguration, _application);
                    _collection.Add(uid, configuration);
                    return configuration;
                }
            }
        }

        public int Count
        {
            get
            {
                VerifyDisposed();
                if (_initialized)
                {
                    lock (_collection)
                    {
                        return _collection.Count;
                    }
                }
                return Execute(() => RemoteObject.Count);
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

        public IConfiguration Create(ConfigurationSettings settings)
        {
            VerifyDisposed();
            IConfiguration remoteConfiguration = Execute(() => RemoteObject.Create(settings));
            lock (_collection)
            {
                VerifyDisposed();
                Configuration configuration;
                if (!_collection.TryGetValue(remoteConfiguration.Uid, out configuration))
                {
                    configuration = new Configuration(remoteConfiguration, _application);
                    _collection.Add(configuration.Uid, configuration);
                }
                return configuration;
            }
        }

        public bool Contains(Guid uid)
        {
            VerifyDisposed();
            lock (_collection)
            {
                if (_collection.ContainsKey(uid))
                {
                    return true;
                }
            }
            return Execute(() => RemoteObject.Contains(uid));
        }

        public IEnumerator<IConfiguration> GetEnumerator()
        {
            List<IConfiguration> collection;
            lock (_collection)
            {
                VerifyDisposed();
                Initialize();
                collection = new List<IConfiguration>(_collection.Values);
            }
            return collection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override void Dispose()
        {
            ExecuteDispose(() =>
            {
                _configurationCreatedEventSink.Dispose();
                _configurationRemovedEventSink.Dispose();
                List<Configuration> conllection;
                lock (_collection)
                {
                    conllection = new List<Configuration>(_collection.Values);
                    _collection.Clear();
                }
                foreach (Configuration configuration in conllection)
                {
                    configuration.Dispose();
                }
            });
            base.Dispose();
        }

        private void Initialize()
        {
            if (_initialized)
            {
                return;
            }
            List<IConfiguration> remoteConfigurations = Execute(() => RemoteObject.ToList());
            if (remoteConfigurations.Count == _collection.Count)
            {
                return;
            }
            foreach (IConfiguration remoteConfiguration in remoteConfigurations)
            {
                Configuration configuration = new Configuration(remoteConfiguration, _application);
                if (!_collection.ContainsKey(configuration.Uid))
                {
                    _collection.Add(configuration.Uid, configuration);
                }
            }
            _initialized = true;
        }

        private void OnRemoteConfigurationCreated(object sender, ConfigurationEventArgs e)
        {
            Configuration configuration;
            IConfiguration remoteConfiguration = e.Configuration;
            Guid configurationUid;
            try
            {
                configurationUid = remoteConfiguration.Uid;
            }
            catch (Exception exception)
            {
                LoggingProvider.Current.Log(TraceEventType.Error, exception);
                return;
            }
            lock (_collection)
            {
                VerifyDisposed();
                if (!_collection.TryGetValue(configurationUid, out configuration))
                {
                    configuration = new Configuration(remoteConfiguration, _application);
                    _collection.Add(configurationUid, configuration);
                }
            }
            ConfigurationEventArgs.RaiseEvent((EventHandler<ConfigurationEventArgs>)_configurationCreated, this, configuration);
        }

        private void OnRemoteConfigurationRemoved(object sender, ConfigurationEventArgs e)
        {
            Configuration configuration;
            IConfiguration remoteConfiguration = e.Configuration;
            Guid configurationUid;
            try
            {
                configurationUid = remoteConfiguration.Uid;
            }
            catch (Exception exception)
            {
                LoggingProvider.Current.Log(TraceEventType.Error, exception);
                return;
            }
            lock (_collection)
            {
                VerifyDisposed();
                if (_collection.TryGetValue(configurationUid, out configuration))
                {
                    _collection.Remove(configurationUid);
                }
            }
            if (configuration == null)
            {
                configuration = new Configuration(remoteConfiguration, _application);
            }
            ConfigurationEventArgs.RaiseEvent((EventHandler<ConfigurationEventArgs>)_configurationRemoved, this, configuration);
            configuration.Dispose();
        }
    }
}
