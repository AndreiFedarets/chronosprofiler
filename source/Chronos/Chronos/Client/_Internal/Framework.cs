using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using Chronos.Extensibility;

namespace Chronos.Client
{
    internal sealed class Framework : PropertyChangedBase, IFramework, IFrameworkAdapter, IWrapper
    {
        private IFrameworkAdapter _adapter;
        private readonly string _applicationCode;
        private readonly FrameworkDefinition _definition;
        private readonly IExportLoader _exportLoader;
        private readonly IProfilingTypeCollection _profilingTypes;
        private readonly Host.IApplicationCollection _hostApplications;
        private readonly ObservableCollection<Host.IApplication> _supportedHostApplications;

        public Framework(FrameworkDefinition definition, IExportLoader exportLoader, string applicationCode, 
            IProfilingTypeCollection profilingTypes, Host.IApplicationCollection hostApplications)
        {
            _definition = definition;
            _exportLoader = exportLoader;
            _applicationCode = applicationCode;
            _profilingTypes = profilingTypes;
            _hostApplications = hostApplications;
            _supportedHostApplications = InitializeSupportedApplications(hostApplications);
            _supportedHostApplications.CollectionChanged += OnSupportedHostApplicationsCollectionChanged;
            _hostApplications.ApplicationConnected += OnHostApplicationConnected;
            _hostApplications.ApplicationDisconnected += OnHostApplicationDisconnected;
        }

        public FrameworkDefinition Definition
        {
            get
            {
                VerifyDisposed();
                return _definition;
            }
        }

        private IFrameworkAdapter Adapter
        {
            get
            {
                if (_adapter == null)
                {
                    ExportDefinition exportDefinition = Definition.Exports.FindByApplication(_applicationCode);
                    _adapter = _exportLoader.Load<IFrameworkAdapter>(exportDefinition);
                }
                return _adapter;
            }
        }

        public IEnumerable<IProfilingType> ProfilingTypes
        {
            get
            {
                VerifyDisposed();
                return _profilingTypes.Where(x => x.Framework == this);
            }
        }

        public bool IsHidden
        {
            get
            {
                VerifyDisposed();
                return ProfilingTypes.All(x => x.Definition.IsHidden);
            }
        }

        public bool IsAvailable
        {
            get
            {
                lock (Lock)
                {
                    VerifyDisposed();
                    return _supportedHostApplications.Any();
                }
            }
        }

        public IEnumerable<Host.IApplication> SupportedApplications
        {
            get
            {
                lock (Lock)
                {
                    VerifyDisposed();
                    return _supportedHostApplications;
                }
            }
        }

        object IWrapper.UndrelyingObject
        {
            get { return Adapter; }
        }

        public override void Dispose()
        {
            lock (Lock)
            {
                VerifyDisposed();
                _hostApplications.ApplicationConnected -= OnHostApplicationConnected;
                _hostApplications.ApplicationDisconnected -= OnHostApplicationDisconnected;
                _supportedHostApplications.CollectionChanged -= OnSupportedHostApplicationsCollectionChanged;
                //Do not use Adapter because it may cause initialization (but it's not needed here)
                _adapter.TryDispose();
                base.Dispose();
            }
        }

        //object IFrameworkAdapter.CreateSettingsPresentation(FrameworkSettings frameworkSettings)
        //{
        //    lock (Lock)
        //    {
        //        VerifyDisposed();
        //        return Adapter.CreateSettingsPresentation(frameworkSettings);
        //    }
        //}

        //public object CreateSettingsPresentation(FrameworkSettings frameworkSettings)
        //{
        //    return ((IFrameworkAdapter)this).CreateSettingsPresentation(frameworkSettings);
        //}

        private void OnSupportedHostApplicationsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            lock (Lock)
            {
                VerifyDisposed();
                NotifyOfPropertyChange(() => IsAvailable);
            }
        }

        private ObservableCollection<Host.IApplication> InitializeSupportedApplications(Host.IApplicationCollection applications)
        {
            ObservableCollection<Host.IApplication> supportedApplications = new ObservableCollection<Host.IApplication>();
            foreach (Host.IApplication application in applications)
            {
                if (IsSupportedApplication(application))
                {
                    supportedApplications.Add(application);
                }
            }
            return supportedApplications;
        }

        private void OnHostApplicationConnected(object sender, Host.ApplicationEventArgs e)
        {
            lock (Lock)
            {
                VerifyDisposed();
                if (IsSupportedApplication(e.Application))
                {
                    _supportedHostApplications.Add(e.Application);
                }
            }
        }

        private bool IsSupportedApplication(Host.IApplication application)
        {
            bool isAvailable = false;
            try
            {
                isAvailable = application.ProfilingTargets.Contains(Definition.Uid);
            }
            catch (Exception exception)
            {
                LoggingProvider.Current.Log(TraceEventType.Warning, exception);
            }
            return isAvailable;
        }

        private void OnHostApplicationDisconnected(object sender, Host.ApplicationEventArgs e)
        {
            lock (Lock)
            {
                VerifyDisposed();
                Host.IApplication hostApplication = e.Application;
                Host.IApplication supportedApplication = _supportedHostApplications.FirstOrDefault(x => x.Uid == hostApplication.Uid);
                if (supportedApplication != null)
                {
                    _supportedHostApplications.Remove(supportedApplication);
                }
            }
        }
    }
}
