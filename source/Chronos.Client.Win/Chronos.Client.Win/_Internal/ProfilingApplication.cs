using Chronos.Extensibility;
using Layex;
using Layex.ViewModels;
using System;
using System.Collections.Generic;

namespace Chronos.Client.Win
{
    internal sealed class ProfilingApplication : ApplicationBase, IProfilingApplication, ILifetimeSponsor
    {
        private ISession _session;
        private ConfigurationSettings _configurationSettings;
        private readonly Guid _sessionUid;

        public ProfilingApplication(Guid sessionUid, bool processOwner)
            : base(sessionUid.ReverseBits(), processOwner)
        {
            _sessionUid = sessionUid;
        }

        public SessionState SessionState
        {
            get
            {
                SessionState state = SessionState.Closed;
                if (_session != null)
                {
                    state = _session.State;
                }
                return state;
            }
        }

        public IProfilingTimer ProfilingTimer { get; private set; }

        public event EventHandler<SessionStateEventArgs> SessionStateChanged;

        protected override void ShowMainViewModel()
        {
            Container.Resolve<IViewModelManager>().Activate(Constants.ViewModels.Profiling);
        }

        protected override void ConfigureContainer(IDependencyContainer container)
        {
            container.RegisterInstance<IServiceContainer>(_session.ServiceContainer);
            container.RegisterInstance<IProfilingApplication>(this);
            container.RegisterInstance(ProfilingTimer);
            base.ConfigureContainer(container);
        }

        protected override void RunInternal()
        {
            base.RunInternal();
            RunProfilingTarget();
            RunFrameworks();
            RunProfilingTypes();
            RunProductivities();
        }

        private void RunProfilingTarget()
        {
            ProfilingTargetSettings profilingTargetSettings = _configurationSettings.ProfilingTargetSettings;
            IProfilingTarget profilingTarget = ProfilingTargets[profilingTargetSettings.Uid];
            IProfilingTargetAdapter adapter = profilingTarget.GetWinAdapter();
            IServiceConsumer serviceConsumer = adapter as IServiceConsumer;
            if (serviceConsumer != null)
            {
                serviceConsumer.ExportServices(_session.ServiceContainer);
                serviceConsumer.ImportServices(_session.ServiceContainer);
            }
        }

        private void RunFrameworks()
        {
            FrameworkSettingsCollection frameworksSettings = _configurationSettings.FrameworksSettings;
            List<IFrameworkAdapter> adapters = new List<IFrameworkAdapter>();
            foreach (FrameworkSettings frameworkSetting in frameworksSettings)
            {
                IFramework framework = Frameworks[frameworkSetting.Uid];
                IFrameworkAdapter adapter = framework.GetWinAdapter();
                adapters.Add(adapter);
            }
            foreach (IFrameworkAdapter adapter in adapters)
            {
                IServiceConsumer serviceConsumer = adapter as IServiceConsumer;
                if (serviceConsumer != null)
                {
                    serviceConsumer.ExportServices(_session.ServiceContainer);
                }
            }
            foreach (IFrameworkAdapter adapter in adapters)
            {
                IServiceConsumer serviceConsumer = adapter as IServiceConsumer;
                if (serviceConsumer != null)
                {
                    serviceConsumer.ImportServices(_session.ServiceContainer);
                }
            }
        }

        private void RunProfilingTypes()
        {
            ProfilingTypeSettingsCollection profilingTypesSettings = _configurationSettings.ProfilingTypesSettings;
            List<IProfilingTypeAdapter> adapters = new List<IProfilingTypeAdapter>();
            foreach (ProfilingTypeSettings profilingTypeSettings in profilingTypesSettings)
            {
                IProfilingType profilingType = ProfilingTypes[profilingTypeSettings.Uid];
                IProfilingTypeAdapter adapter = profilingType.GetWinAdapter();
                adapters.Add(adapter);
            }
            foreach (IProfilingTypeAdapter adapter in adapters)
            {
                IServiceConsumer serviceConsumer = adapter as IServiceConsumer;
                if (serviceConsumer != null)
                {
                    serviceConsumer.ExportServices(_session.ServiceContainer);
                }
            }
            foreach (IProfilingTypeAdapter adapter in adapters)
            {
                IServiceConsumer serviceConsumer = adapter as IServiceConsumer;
                if (serviceConsumer != null)
                {
                    serviceConsumer.ImportServices(_session.ServiceContainer);
                }
            }
        }

        private void RunProductivities()
        {
            List<IProductivityAdapter> adapters = new List<IProductivityAdapter>();
            foreach (IProductivity productivity in Productivities)
            {
                IProductivityAdapter adapter = productivity.GetWinAdapter();
                adapters.Add(adapter);
            }
            foreach (IProductivityAdapter adapter in adapters)
            {
                IServiceConsumer serviceConsumer = adapter as IServiceConsumer;
                if (serviceConsumer != null)
                {
                    serviceConsumer.ExportServices(_session.ServiceContainer);
                }
            }
            foreach (IProductivityAdapter adapter in adapters)
            {
                IServiceConsumer serviceConsumer = adapter as IServiceConsumer;
                if (serviceConsumer != null)
                {
                    serviceConsumer.ImportServices(_session.ServiceContainer);
                }
            }
        }

        protected override IServiceContainer CreateServiceContainer()
        {
            return _session.ServiceContainer;
        }

        //TODO: reimplement
        protected override Catalog LoadCatalog()
        {
            Catalog catalog = base.LoadCatalog();
            _session = FindAndStartSession(_sessionUid);
            if (_session == null)
            {
                throw new TempException();
            }
            _session.SessionStateChanged += OnSessionStateChanged;
            _configurationSettings = _session.GetConfigurationSettings();
            catalog = CatalogFilter.Filter(catalog, _configurationSettings);
            return catalog;
        }

        private ISession FindAndStartSession(Guid sessionUid)
        {
            ISession session = null;
            Host.IApplicationCollection hostApplications = new Host.ApplicationCollection();
            //TODO: bad design - you need to reload the list of Host Applications from settings
            hostApplications.ConnectLocal(false);
            foreach (Host.IApplication hostApplication in hostApplications)
            {
                if (hostApplication.Sessions.Contains(sessionUid))
                {
                    session = hostApplication.Sessions[sessionUid];
                    break;
                }
            }
            if (session != null)
            {
                session.StartDecoding(this);
                ProfilingTimer = session.ProfilingTimer;
            }
            //hostApplications.TryDispose();
            return session;
        }

        private void OnSessionStateChanged(object sender, SessionStateEventArgs eventArgs)
        {
            SessionStateChanged?.Invoke(this, eventArgs);
        }

        public void FlushData()
        {
            _session.FlushData();
        }

        public bool ShouldStayAlive()
        {
            return true;
        }

        public override void Dispose()
        {
            base.Dispose();
            IDisposable disposable = _session as IDisposable;
            if (disposable != null)
            {
                disposable.Dispose();
            }
            _session = null;
        }

        internal static Guid GenerateApplicationUid(Guid sessionUid)
        {
            return sessionUid.ReverseBits();
        }
    }
}
