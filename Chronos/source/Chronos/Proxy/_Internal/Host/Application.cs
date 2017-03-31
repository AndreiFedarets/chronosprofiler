namespace Chronos.Proxy.Host
{
    internal sealed class Application : ChronosApplication<Chronos.Host.IApplication>, Chronos.Host.IApplication
    {
        private readonly LazyValue<ConfigurationCollection> _configurations;
        private readonly LazyValue<SessionCollection> _sessions;
        private readonly LazyValue<FrameworkCollection> _frameworks;
        private readonly LazyValue<ProfilingTargetCollection> _profilingTargets;
        private readonly LazyValue<ProfilingTypeCollection> _profilingTypes;

        public Application(Chronos.Host.IApplication application)
            : base(application)
        {
            _configurations = new LazyValue<ConfigurationCollection>(() => new ConfigurationCollection(RemoteObject.Configurations, this));
            _sessions = new LazyValue<SessionCollection>(() => new SessionCollection(RemoteObject.Sessions, this));
            _frameworks = new LazyValue<FrameworkCollection>(() => new FrameworkCollection(RemoteObject.Frameworks));
            _profilingTargets = new LazyValue<ProfilingTargetCollection>(() => new ProfilingTargetCollection(RemoteObject.ProfilingTargets));
            _profilingTypes = new LazyValue<ProfilingTypeCollection>(() => new ProfilingTypeCollection(RemoteObject.ProfilingTypes));
        }

        public IConfigurationCollection Configurations
        {
            get { return _configurations.Value; }
        }

        public ISessionCollection Sessions
        {
            get { return _sessions.Value; }
        }

        public IFrameworkCollection Frameworks
        {
            get { return _frameworks.Value; }
        }

        public IProfilingTargetCollection ProfilingTargets
        {
            get { return _profilingTargets.Value; }
        }

        public IProfilingTypeCollection ProfilingTypes
        {
            get { return _profilingTypes.Value; }
        }

        public override void Dispose()
        {
            VerifyDisposed();
            _configurations.Dispose();
            _sessions.Dispose();
            _frameworks.Dispose();
            _profilingTypes.Dispose();
            _profilingTargets.Dispose();
            base.Dispose();
        }
    }
}
