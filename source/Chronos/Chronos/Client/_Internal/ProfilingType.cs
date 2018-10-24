using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Chronos.Extensibility;

namespace Chronos.Client
{
    internal sealed class ProfilingType : RemoteBaseObject, IProfilingType, IProfilingTypeAdapter, IWrapper
    {
        private const string TechnicalAttributeName = "Technical";
        private IProfilingTypeAdapter _adapter;
        private readonly string _applicationCode;
        private readonly ProfilingTypeDefinition _definition;
        private readonly IExportLoader _exportLoader;
        private readonly IFrameworkCollection _frameworks;
        private readonly Host.IApplicationCollection _hostApplications;
        private readonly ObservableCollection<Host.IApplication> _supportedHostApplications;

        public ProfilingType(ProfilingTypeDefinition definition, IExportLoader exportLoader, string applicationCode,
            IFrameworkCollection frameworks, Host.IApplicationCollection hostApplications)
        {
            _definition = definition;
            _exportLoader = exportLoader;
            _applicationCode = applicationCode;
            _frameworks = frameworks;
            _hostApplications = hostApplications;
            _supportedHostApplications = new ObservableCollection<Host.IApplication>();
            _hostApplications.ApplicationConnected += OnHostApplicationConnected;
            _hostApplications.ApplicationDisconnected += OnHostApplicationDisconnected;
        }

        private IProfilingTypeAdapter Adapter
        {
            get
            {
                if (_adapter == null)
                {
                    ExportDefinition exportDefinition = Definition.Exports.FindByApplication(_applicationCode);
                    _adapter = _exportLoader.Load<IProfilingTypeAdapter>(exportDefinition);
                }
                return _adapter;
            }
        }

        public IFramework Framework
        {
            get
            {
                VerifyDisposed();
                return _frameworks[Definition.FrameworkUid];
            }
        }

        public ProfilingTypeDefinition Definition
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
                VerifyDisposed();
                return _hostApplications;
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

        public bool IsTechnical
        {
            get
            {
                VerifyDisposed();
                bool value = (bool)Definition.Attributes.GetAttributeValue(TechnicalAttributeName, false);
                return value;
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
                _adapter.TryDispose();
                base.Dispose();
            }
        }

        private void OnHostApplicationConnected(object sender, Host.ApplicationEventArgs e)
        {
            lock (Lock)
            {
                VerifyDisposed();
                Host.IApplication hostApplication = e.Application;
                bool isAvailable = false;
                try
                {
                    isAvailable = hostApplication.ProfilingTargets.Contains(Definition.Uid);
                }
                catch (Exception exception)
                {
                    LoggingProvider.Current.Log(TraceEventType.Warning, exception);
                }
                if (isAvailable)
                {
                    _supportedHostApplications.Add(e.Application);
                }
            }
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
