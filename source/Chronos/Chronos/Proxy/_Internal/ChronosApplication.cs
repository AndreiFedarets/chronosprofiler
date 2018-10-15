using System;

namespace Chronos.Proxy
{
    internal class ChronosApplication<T> : ProxyBaseObject<T>, IChronosApplication where T : IChronosApplication
    {
        private readonly LazyValue<ServiceContainer> _serviceContainer;
        private readonly RemoteEventRouter<ApplicationStateEventArgs> _applicationStateChangedEventSink;

        protected ChronosApplication(T remoteObject)
            : base(remoteObject)
        {
            _applicationStateChangedEventSink = new RemoteEventRouter<ApplicationStateEventArgs>(RemoteObject, "ApplicationStateChanged", this);
            _serviceContainer = new LazyValue<ServiceContainer>(() => new ServiceContainer(remoteObject.ServiceContainer));
        }

        public EnvironmentInformation EnvironmentInformation
        {
            get { return Execute(() => RemoteObject.EnvironmentInformation); }
        }

        public Guid Uid
        {
            get { return Execute(() => RemoteObject.Uid); }
        }

        public ApplicationState ApplicationState
        {
            get { return Execute(() => RemoteObject.ApplicationState); }
        }

        public TimeSpan StartupTime
        {
            get { return Execute(() => RemoteObject.StartupTime); }
        }

        public IServiceContainer ServiceContainer
        {
            get { return _serviceContainer.Value; }
        }

        public event EventHandler<ApplicationStateEventArgs> ApplicationStateChanged
        {
            add { _applicationStateChangedEventSink.Event += value; }
            remove { _applicationStateChangedEventSink.Event -= value; }
        }

        public void Run()
        {
            Execute(() => RemoteObject.Run());
        }

        public void Close()
        {
            Execute(() => RemoteObject.Close());
        }

        public string Ping(string message)
        {
            return Execute(() => RemoteObject.Ping(message));
        }

        public override void Dispose()
        {
            ExecuteDispose(() =>
            {
                _applicationStateChangedEventSink.Dispose();
                _serviceContainer.Dispose();
            });
            base.Dispose();
        }
    }
}
