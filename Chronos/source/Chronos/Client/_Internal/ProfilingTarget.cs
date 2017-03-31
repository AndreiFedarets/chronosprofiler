using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using Chronos.Extensibility;

namespace Chronos.Client
{
    internal sealed class ProfilingTarget : PropertyChangedBase, IProfilingTarget, IProfilingTargetAdapter, IWrapper
    {
        private IProfilingTargetAdapter _adapter;
        private readonly string _applicationCode;
        private readonly ProfilingTargetDefinition _definition;
        private readonly IExportLoader _exportLoader;
        private readonly Host.IApplicationCollection _hostApplications;
        private readonly ObservableCollection<Host.IApplication> _supportedHostApplications;

        public ProfilingTarget(ProfilingTargetDefinition definition, IExportLoader exportLoader,
            string applicationCode, Host.IApplicationCollection hostApplications)
        {
            _hostApplications = hostApplications;
            _definition = definition;
            _exportLoader = exportLoader;
            _applicationCode = applicationCode;
            _supportedHostApplications = InitializeSupportedApplications(hostApplications);
            _supportedHostApplications.CollectionChanged += OnSupportedHostApplicationsCollectionChanged;
            _hostApplications.ApplicationConnected += OnHostApplicationConnected;
            _hostApplications.ApplicationDisconnected += OnHostApplicationDisconnected;
        }

        private IProfilingTargetAdapter Adapter
        {
            get
            {
                if (_adapter == null)
                {
                    ExportDefinition exportDefinition = Definition.Exports.FindByApplication(_applicationCode);
                    _adapter = _exportLoader.Load<IProfilingTargetAdapter>(exportDefinition);
                }
                return _adapter;
            }
        }

        public ProfilingTargetDefinition Definition
        {
            get
            {
                VerifyDisposed();
                return _definition;
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

        object IWrapper.UndrelyingObject
        {
            get { return Adapter; }
        }

        //object IProfilingTargetAdapter.CreateSettingsPresentation(ProfilingTargetSettings profilingTargetSettings)
        //{
        //    lock (Lock)
        //    {
        //        VerifyDisposed();
        //        return Adapter.CreateSettingsPresentation(profilingTargetSettings);
        //    }
        //}

        //public object CreateSettingsPresentation(ProfilingTargetSettings profilingTargetSettings)
        //{
        //    return ((IProfilingTargetAdapter) this).CreateSettingsPresentation(profilingTargetSettings);
        //}

        public override void Dispose()
        {
            lock (Lock)
            {
                VerifyDisposed();
                _hostApplications.ApplicationConnected -= OnHostApplicationConnected;
                _hostApplications.ApplicationDisconnected -= OnHostApplicationDisconnected;
                _supportedHostApplications.CollectionChanged -= OnSupportedHostApplicationsCollectionChanged;
                _adapter.TryDispose();
                base.Dispose();
            }
        }

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
