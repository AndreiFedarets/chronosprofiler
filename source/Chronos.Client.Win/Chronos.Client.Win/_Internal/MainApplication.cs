using System;
using System.Collections.Generic;
using Adenium;
using Adenium.Layouting;
using Chronos.Client.Win.ViewModels.Home;

namespace Chronos.Client.Win
{
    internal sealed class MainApplication : ApplicationBase, IMainApplication
    {
        public MainApplication()
            : base(Guid.NewGuid())
        {
        }

        public MainApplication(bool processOwner)
            : base(Guid.NewGuid(), processOwner)
        {
        }

        protected override IContainerViewModel BuildMainViewModel()
        {
            return Container.Resolve<HomePageViewModel>();
        }

        protected override void ConfigureContainer(IContainer container)
        {
            container.RegisterInstance<IMainApplication>(this);
            base.ConfigureContainer(container);
        }

        protected override void RunInternal()
        {
            base.RunInternal();
            Host.ConnectionManager connectionManager = new Host.ConnectionManager();
            connectionManager.RestoreConnections(HostApplications, ApplicationSettings.HostConnections);
            Sessions.SessionCreated += OnSessionCreated;
            RunProfilingTargets();
            RunFrameworks();
            RunProfilingTypes();
            RunProductivities();
        }

        private void RunProfilingTargets()
        {
            List<IProfilingTargetAdapter> adapters = new List<IProfilingTargetAdapter>();
            foreach (IProfilingTarget profilingTarget in ProfilingTargets)
            {
                IProfilingTargetAdapter adapter = profilingTarget.GetWinAdapter();
                adapters.Add(adapter);
            }
            foreach (IProfilingTargetAdapter adapter in adapters)
            {
                ILayoutProvider layoutProvider = adapter as ILayoutProvider;
                if (layoutProvider != null)
                {
                    CompositeLayoutProvider.Register(layoutProvider);
                }
            }
        }

        private void RunFrameworks()
        {
            List<IFrameworkAdapter> adapters = new List<IFrameworkAdapter>();
            foreach (IFramework framework in Frameworks)
            {
                IFrameworkAdapter adapter = framework.GetWinAdapter();
                adapters.Add(adapter);
            }
            foreach (IFrameworkAdapter adapter in adapters)
            {
                ILayoutProvider layoutProvider = adapter as ILayoutProvider;
                if (layoutProvider != null)
                {
                    CompositeLayoutProvider.Register(layoutProvider);
                }
            }
        }

        private void RunProfilingTypes()
        {
            List<IProfilingTypeAdapter> adapters = new List<IProfilingTypeAdapter>();
            foreach (IProfilingType profilingType in ProfilingTypes)
            {
                IProfilingTypeAdapter adapter = profilingType.GetWinAdapter();
                adapters.Add(adapter);
            }
            foreach (IProfilingTypeAdapter adapter in adapters)
            {
                ILayoutProvider layoutProvider = adapter as ILayoutProvider;
                if (layoutProvider != null)
                {
                    CompositeLayoutProvider.Register(layoutProvider);
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
                ILayoutProvider layoutProvider = adapter as ILayoutProvider;
                if (layoutProvider != null)
                {
                    CompositeLayoutProvider.Register(layoutProvider);
                }
            }
        }

        private void OnSessionCreated(object sender, SessionEventArgs e)
        {
            //TODO: is it correct place for this call?
            ApplicationManager.Profiling.RunOrActivateApplication(e.Session.Uid);
        }

        public override void Dispose()
        {
            Sessions.SessionCreated -= OnSessionCreated;
            base.Dispose();
            Host.ApplicationManager.Shutdown();
            Daemon.ApplicationManager.Shutdown();
        }
    }
}
