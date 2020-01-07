using Layex;
using Layex.Layouts;
using System;
using System.Collections.Generic;

namespace Chronos.Client.Win
{
    internal sealed class MainApplication : ApplicationBase, IMainApplication
    {
        private readonly CompositeLayoutProvider _layoutProvider;
        private BuiltinDependencyContainer _container;

        public MainApplication(bool processOwner)
            : base(Guid.NewGuid(), processOwner, Constants.ViewModels.Home)
        {
            _layoutProvider = new CompositeLayoutProvider();
            _layoutProvider.RegisterProvider(new FileSystemLayoutProvider());
        }

        protected override IDependencyContainer Container
        {
            get { return _container; }
        }

        protected override ILayoutProvider LayoutProvider
        {
            get { return _layoutProvider; }
        }

        private void ConfigureContainer()
        {
            _container = new BuiltinDependencyContainer();
            _container.RegisterInstance<IApplicationBase>(this);
            _container.RegisterInstance<IMainApplication>(this);
            //container.RegisterInstance<IApplicationSettings>(ApplicationSettings);
        }

        private void RestoreConnection()
        {
            Host.ConnectionManager connectionManager = new Host.ConnectionManager();
            connectionManager.RestoreConnections(HostApplications, ApplicationSettings.HostConnections);
            Sessions.SessionCreated += OnSessionCreated;
        }

        protected override void RunInternal()
        {
            base.RunInternal();
            RestoreConnection();
            ConfigureContainer();
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
            //foreach (IProfilingTargetAdapter adapter in adapters)
            //{
            //    ILayoutProvider layoutProvider = adapter as ILayoutProvider;
            //    if (layoutProvider != null)
            //    {
            //        _layoutProvider.RegisterProvider(layoutProvider);
            //    }
            //}
        }

        private void RunFrameworks()
        {
            List<IFrameworkAdapter> adapters = new List<IFrameworkAdapter>();
            foreach (IFramework framework in Frameworks)
            {
                IFrameworkAdapter adapter = framework.GetWinAdapter();
                adapters.Add(adapter);
            }
            //foreach (IFrameworkAdapter adapter in adapters)
            //{
            //    ILayoutProvider layoutProvider = adapter as ILayoutProvider;
            //    if (layoutProvider != null)
            //    {
            //        _layoutProvider.RegisterProvider(layoutProvider);
            //    }
            //}
        }

        private void RunProfilingTypes()
        {
            List<IProfilingTypeAdapter> adapters = new List<IProfilingTypeAdapter>();
            foreach (IProfilingType profilingType in ProfilingTypes)
            {
                IProfilingTypeAdapter adapter = profilingType.GetWinAdapter();
                adapters.Add(adapter);
            }
            //foreach (IProfilingTypeAdapter adapter in adapters)
            //{
            //    ILayoutProvider layoutProvider = adapter as ILayoutProvider;
            //    if (layoutProvider != null)
            //    {
            //        _layoutProvider.RegisterProvider(layoutProvider);
            //    }
            //}
        }

        private void RunProductivities()
        {
            List<IProductivityAdapter> adapters = new List<IProductivityAdapter>();
            foreach (IProductivity productivity in Productivities)
            {
                IProductivityAdapter adapter = productivity.GetWinAdapter();
                adapters.Add(adapter);
            }
            //foreach (IProductivityAdapter adapter in adapters)
            //{
            //    ILayoutProvider layoutProvider = adapter as ILayoutProvider;
            //    if (layoutProvider != null)
            //    {
            //        _layoutProvider.RegisterProvider(layoutProvider);
            //    }
            //}
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
