using System;
using System.Diagnostics;
using System.Runtime.Remoting;
using Chronos.Communication;
using Chronos.Extensibility;
using System.Threading;
using Chronos.Settings;

namespace Chronos
{
    public abstract class ChronosApplication : RemoteBaseObject, IChronosApplication
    {
        private ObjRef _shared;
        private readonly bool _processOwner;
        private readonly Process _currentProcess;
        private readonly IApplicationSettings _applicationSettings;
        private readonly ExportLoader _exportLoader;
        private readonly ExtensionAssemblyResolver _assemblyResolver;
        private readonly EnvironmentInformation _environmentInformation;
        private IServiceContainer _serviceContainer;
        private Catalog _catalog;
        private volatile ApplicationState _applicationState;

        protected ChronosApplication(bool processOwner)
            : base(true)
        {
            _processOwner = processOwner;
            _applicationState = ApplicationState.NotStarted;
            _applicationSettings = new ApplicationSettings();
            LoggingProvider.Initialize(_applicationSettings.Logging, GetType().FullName);
            _currentProcess = Process.GetCurrentProcess();
            _assemblyResolver = new ExtensionAssemblyResolver();
            _exportLoader = new ExportLoader(_assemblyResolver, this);
            _environmentInformation = new EnvironmentInformation();
        }

        public ApplicationState ApplicationState
        {
            get { return _applicationState; }
            private set
            {
                ApplicationState previuosState = _applicationState;
                _applicationState = value;
                if (previuosState != _applicationState)
                {
                    ApplicationStateEventArgs.RaiseEvent(ApplicationStateChanged, this, previuosState, _applicationState);
                }
            }
        }

        public abstract Guid Uid { get; }

        public EnvironmentInformation EnvironmentInformation
        {
            get { return _environmentInformation; }
        }

        public IApplicationSettings ApplicationSettings
        {
            get { return _applicationSettings; }
        }

        public Catalog Catalog
        {
            get { return _catalog; }
        }

        public IServiceContainer ServiceContainer
        {
            get
            {
                //TODO: Lock?
                if (_serviceContainer == null)
                {
                    _serviceContainer = CreateServiceContainer();
                }
                return _serviceContainer;
            }
        }

        public TimeSpan StartupTime { get; private set; }

        protected IExportLoader ExportLoader
        {
            get { return _exportLoader; }
        }

        protected IExtensionAssemblyResolver AssemblyResolver
        {
            get { return _assemblyResolver; }
        }

        public event EventHandler<ApplicationStateEventArgs> ApplicationStateChanged;

        public void Run()
        {
            lock (Lock)
            {
                if (_applicationState == ApplicationState.NotStarted)
                {
                    Stopwatch stopwatch = Stopwatch.StartNew();
                    ApplicationState = ApplicationState.Starting;
                    try
                    {
                        SetupCrashLogging();
                        Connector.Initialize(Uid);
                        _catalog = LoadCatalog();
                        RunInternal();
                        _shared = Connector.Managed.Share(this, Uid);
                    }
                    catch(Exception exception)
                    {
                        LoggingProvider.Current.Log(TraceEventType.Critical, exception);
                        throw;
                    }
                    finally
                    {
                        StartupTime = stopwatch.Elapsed;
                        ApplicationState = ApplicationState.Started;
                        string message = string.Format("Application {0} started at {1} (Startup time is {2})", GetType().FullName, DateTime.Now, StartupTime);
                        LoggingProvider.Current.Log(TraceEventType.Information, message);
                    }
                }
            }
        }

        private void SetupCrashLogging()
        {
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
            CrashLogger.Setup(_applicationSettings.CrashDump);
        }

        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception exception = e.ExceptionObject as Exception;
            if (exception != null)
            {
                LoggingProvider.Current.Log(TraceEventType.Critical, exception);
            }
            else
            {
                LoggingProvider.Current.Log(TraceEventType.Critical, "Unknown Failure");
            }
        }

        public void Close()
        {
            lock (Lock)
            {
                if (!CanClose())
                {
                    return;
                }
                if (_applicationState == ApplicationState.Started)
                {
                    ApplicationState = ApplicationState.Closing;
                    try
                    {
                        CloseInternal();
                        _assemblyResolver.Dispose();
                        _currentProcess.Dispose();
                        AppDomain.CurrentDomain.UnhandledException -= OnUnhandledException;
                        _shared = null;
                        ApplicationState = ApplicationState.Closed;
                    }
                    catch (Exception exception)
                    {
                        LoggingProvider.Current.Log(System.Diagnostics.TraceEventType.Critical, exception);
                        throw;
                    }
                    finally
                    {
                        Dispose();
                        DisposableTracker.TraceUndisposedResources();
                    }
                    if (_processOwner)
                    {
                        //TODO: Replace this code with more appropriate process closing
                        ThreadPool.QueueUserWorkItem(c =>
                            {
                                Thread.Sleep(1000);
                                Environment.Exit(0);
                            });
                    }
                }
            }
        }

        public string Ping(string message)
        {
            return message;
        }

        protected virtual IServiceContainer CreateServiceContainer()
        {
            return new ServiceContainer();
        }

        protected abstract void RunInternal();

        protected virtual void CloseInternal()
        {
        }

        protected virtual bool CanClose()
        {
            return true;
        }

        protected virtual Catalog LoadCatalog()
        {
            CatalogAggregator catalogAggregator = new CatalogAggregator(AssemblyResolver);
            catalogAggregator.ReadExtensions(ApplicationSettings.Extensions);
            return catalogAggregator.GetCatalog();
        }

        public override void Dispose()
        {
            if (_serviceContainer != null)
            {
                _serviceContainer.TryDispose();
            }
            base.Dispose();
        }
    }
}
