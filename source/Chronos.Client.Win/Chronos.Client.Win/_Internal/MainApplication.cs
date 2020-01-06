using Layex;
using Layex.ViewModels;
using System;
using System.Collections.Generic;

namespace Chronos.Client.Win
{
    internal sealed class MainApplication : ApplicationBase, IMainApplication
    {
        public MainApplication(bool processOwner)
            : base(Guid.NewGuid(), processOwner)
        {
        }

        protected override void ShowMainViewModel()
        {
            Container.Resolve<IViewModelManager>().Activate(Constants.ViewModels.Home);
        }

        protected override void ConfigureContainer(IDependencyContainer container)
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
        }

        private void RunFrameworks()
        {
            List<IFrameworkAdapter> adapters = new List<IFrameworkAdapter>();
            foreach (IFramework framework in Frameworks)
            {
                IFrameworkAdapter adapter = framework.GetWinAdapter();
                adapters.Add(adapter);
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
        }

        private void RunProductivities()
        {
            List<IProductivityAdapter> adapters = new List<IProductivityAdapter>();
            foreach (IProductivity productivity in Productivities)
            {
                IProductivityAdapter adapter = productivity.GetWinAdapter();
                adapters.Add(adapter);
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
