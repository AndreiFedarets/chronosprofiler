using System;
using System.Collections.Generic;

namespace Chronos
{
    internal delegate void ConfigurationEventHandler(object sender, ConfigurationEventArgs e);

    internal sealed class ConfigurationCollection : RemoteBaseObject, IConfigurationCollection
    {
        private readonly Host.IApplication _application;
        private readonly IProfilingTargetCollection _profilingTargets;
        private readonly IFrameworkCollection _frameworks;
        private readonly IProfilingTypeCollection _profilingTypes;
        private readonly Dictionary<Guid, Configuration> _collection;
        private readonly RemoteEventHandler<ConfigurationEventArgs> _configurationCreatedEvent;
        private readonly RemoteEventHandler<ConfigurationEventArgs> _configurationRemovedEvent;

        public ConfigurationCollection(IProfilingTargetCollection profilingTargets, IFrameworkCollection frameworks, IProfilingTypeCollection profilingTypes, Host.IApplication application)
        {
            _profilingTargets = profilingTargets;
            _frameworks = frameworks;
            _profilingTypes = profilingTypes;
            _application = application;
            _collection = new Dictionary<Guid, Configuration>();
            _configurationCreatedEvent = new RemoteEventHandler<ConfigurationEventArgs>(this);
            _configurationRemovedEvent = new RemoteEventHandler<ConfigurationEventArgs>(this);
        }

        public IConfiguration this[Guid uid]
        {
            get
            {
                lock (Lock)
                {
                    VerifyDisposed();
                    Configuration configuration;
                    if (!_collection.TryGetValue(uid, out configuration))
                    {
                        throw new ConfigurationNotFoundException(uid);
                    }
                    return configuration;
                }
            }
        }

        public int Count
        {
            get
            {
                lock (Lock)
                {
                    VerifyDisposed();
                    return _collection.Count;
                }
            }
        }

        public event EventHandler<ConfigurationEventArgs> ConfigurationCreated
        {
            add { _configurationCreatedEvent.Add(value); }
            remove { _configurationCreatedEvent.Remove(value); }
        }

        public event EventHandler<ConfigurationEventArgs> ConfigurationRemoved
        {
            add { _configurationRemovedEvent.Add(value); }
            remove { _configurationRemovedEvent.Remove(value); }
        }

        public IConfiguration Create(ConfigurationSettings settings)
        {
            lock (Lock)
            {
                VerifyDisposed();
                settings.Validate();
                if (_collection.ContainsKey(settings.ConfigurationUid))
                {
                    throw new ConfigurationAlreadyExistsException(settings.ConfigurationUid);
                }
                IProfilingTarget profilingTarget = _profilingTargets[settings.ProfilingTargetSettings.Uid];
                Configuration configuration = new Configuration(settings, this, profilingTarget, _frameworks, _profilingTypes, _application);
                _collection.Add(configuration.Uid, configuration);
                _configurationCreatedEvent.Invoke(() => new ConfigurationEventArgs(configuration));
                return configuration;
            }
        }

        public bool Contains(Guid uid)
        {
            lock (Lock)
            {
                VerifyDisposed();
                return _collection.ContainsKey(uid);
            }
        }

        public IEnumerator<IConfiguration> GetEnumerator()
        {
            List<IConfiguration> configurations;
            lock (Lock)
            {
                VerifyDisposed();
                configurations = new List<IConfiguration>(_collection.Values);
            }
            return configurations.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override void Dispose()
        {
            lock (Lock)
            {
                VerifyDisposed();
                _configurationCreatedEvent.Dispose();
                _configurationRemovedEvent.Dispose();
                foreach (Configuration configuration in _collection.Values)
                {
                    configuration.Dispose();
                }
                _collection.Clear();
                base.Dispose();
            }
        }

        public void OnConfigurationRemoved(Configuration configuration)
        {
            lock (Lock)
            {
                VerifyDisposed();
                _collection.Remove(configuration.Uid);
                _configurationRemovedEvent.Invoke(() => new ConfigurationEventArgs(configuration));
            }
        }
    }
}
