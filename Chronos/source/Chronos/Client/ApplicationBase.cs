namespace Chronos.Client
{
    public abstract class ApplicationBase : ChronosApplication
    {
        private Host.ApplicationCollection _hostApplications;
        private FrameworkCollection _frameworks;
        private ProfilingTypeCollection _profilingTypes;
        private ProfilingTargetCollection _profilingTargets;
        private ConfigurationCollection _configurations;
        private SessionCollection _sessions;
        private ProductivityCollection _productivities;

        protected ApplicationBase()
            : this(true)
        {
        }

        protected ApplicationBase(bool processOwner)
            : base(processOwner)
        {
        }

        protected abstract string ApplicationCode { get; }

        public Host.IApplicationCollection HostApplications
        {
            get { return _hostApplications; }
        }

        public IConfigurationCollection Configurations
        {
            get { return _configurations; }
        }

        public ISessionCollection Sessions
        {
            get { return _sessions; }
        }

        public IProductivityCollection Productivities
        {
            get { return _productivities; }
        }

        public IProfilingTargetCollection ProfilingTargets
        {
            get { return _profilingTargets; }
        }

        public IProfilingTypeCollection ProfilingTypes
        {
            get { return _profilingTypes; }
        }

        public IFrameworkCollection Frameworks
        {
            get { return _frameworks; }
        }

        protected override void RunInternal()
        {
            _hostApplications = new Host.ApplicationCollection();
            _profilingTypes = new ProfilingTypeCollection(Catalog.ProfilingTypes, ExportLoader, ApplicationCode, _hostApplications);
            _frameworks = new FrameworkCollection(Catalog.Frameworks, ExportLoader, ApplicationCode, _hostApplications);
            _profilingTargets = new ProfilingTargetCollection(Catalog.ProfilingTargets, ExportLoader, ApplicationCode, _hostApplications);
            _productivities = new ProductivityCollection(Catalog.Productivities, ExportLoader, ApplicationCode);
            _profilingTypes.SetupDependencies(_frameworks);
            _frameworks.SetupDependencies(_profilingTypes);
            _configurations = new ConfigurationCollection(_hostApplications);
            _sessions = new SessionCollection(_hostApplications);
        }

        public override void Dispose()
        {
            _configurations.Dispose();
            _sessions.Dispose();
            _frameworks.Dispose();
            _profilingTypes.Dispose();
            _profilingTargets.Dispose();
            _hostApplications.Dispose();
            base.Dispose();
        }
    }
}
