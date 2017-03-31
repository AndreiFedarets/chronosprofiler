using Chronos.Communication.Managed;
using System;
using System.Collections.Generic;

namespace Chronos.Host
{
    public sealed class ApplicationCollection : RemoteBaseObject, IApplicationCollection
    {
        private readonly RemoteEventHandler<ApplicationEventArgs> _applicationConnectedEvent;
        private readonly RemoteEventHandler<ApplicationEventArgs> _applicationDisconnectedEvent;
        private readonly List<IApplication> _collection;

        public ApplicationCollection()
        {
            _applicationConnectedEvent = new RemoteEventHandler<ApplicationEventArgs>(this);
            _applicationDisconnectedEvent = new RemoteEventHandler<ApplicationEventArgs>(this);
            _collection = new List<IApplication>();
        }

        public event EventHandler<ApplicationEventArgs> ApplicationConnected
        {
            add { _applicationConnectedEvent.Add(value); }
            remove { _applicationConnectedEvent.Remove(value); }
        }

        public event EventHandler<ApplicationEventArgs> ApplicationDisconnected
        {
            add { _applicationDisconnectedEvent.Add(value); }
            remove { _applicationDisconnectedEvent.Remove(value); }
        }

        public IApplication Connect(ConnectionSettings connectionSettings)
        {
            lock (Lock)
            {
                VerifyDisposed();
                IApplication application = ApplicationManager.Connect(connectionSettings);
                IApplication existingApplication = FindApplication(application.EnvironmentInformation);
                if (existingApplication == null)
                {
                    _collection.Add(application);
                    _applicationConnectedEvent.Invoke(() => new ApplicationEventArgs(application));
                }
                else
                {
                    application = existingApplication;
                }
                return application;
            }
        }

        public IApplication ConnectLocal(bool runIfNotLaunched)
        {
            lock (Lock)
            {
                VerifyDisposed();
                IApplication application = null;
                ConnectionSettings connectionSettings = ApplicationManager.CreateLocalConnectionSettings();
                if (ApplicationManager.CheckConnection(connectionSettings))
                {
                    application = Connect(connectionSettings);
                }
                else if (runIfNotLaunched)
                {
                    application = ApplicationManager.Run();
                    _collection.Add(application);
                    _applicationConnectedEvent.Invoke(() => new ApplicationEventArgs(application));
                }
                return application;
            }
        }

        public bool Disconnect(IApplication application)
        {
            lock (Lock)
            {
                VerifyDisposed();
                EnvironmentInformation environmentInformation = application.EnvironmentInformation;
                application = FindApplication(environmentInformation);
                if (application == null)
                {
                    return false;
                }
                bool result = _collection.Remove(application);
                if (result)
                {
                    _applicationDisconnectedEvent.Invoke(() => new ApplicationEventArgs(application));
                }
                return result;
            }
        }

        public IEnumerator<IApplication> GetEnumerator()
        {
            List<IApplication> collection;
            lock (Lock)
            {
                VerifyDisposed();
                collection = new List<IApplication>(_collection);
            }
            return collection.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private IApplication FindApplication(EnvironmentInformation environmentInformation)
        {
            foreach (IApplication application in _collection)
            {
                EnvironmentInformation temp = application.EnvironmentInformation;
                if (environmentInformation.Equals(temp))
                {
                    return application;
                }
            }
            return null;
        }

        public override void Dispose()
        {
            lock (Lock)
            {
                VerifyDisposed();
                _applicationConnectedEvent.Dispose();
                _applicationDisconnectedEvent.Dispose();
                foreach (IApplication application in _collection)
                {
                    application.TryDispose();
                }
                base.Dispose();
            }
        }
    }
}
