using System;
using Chronos.Communication;
using Chronos.Communication.Native;

namespace Chronos.Host
{
    internal sealed class Application : ChronosApplication, IApplication
    {
        public static readonly Guid ApplicationUid;
        private ConfigurationCollection _configurations;
        private SessionCollection _sessions;
        private FrameworkCollection _frameworks;
        private ProfilingTypeCollection _profilingTypes;
        private ProfilingTargetCollection _profilingTargets;
        private RequestServer _requestServer;

        static Application()
        {
            ApplicationUid = new Guid("1B6E1937-B1A8-4E54-B2E0-D4DC1CD7642A");
        }

        public Application(bool processOwner)
            : base(processOwner)
        {
        }

        public override Guid Uid
        {
            get { return ApplicationUid; }
        }

        public IConfigurationCollection Configurations
        {
            get { return _configurations; }
        }

        public ISessionCollection Sessions
        {
            get { return _sessions; }
        }

        public IFrameworkCollection Frameworks
        {
            get { return _frameworks; }
        }

        public IProfilingTargetCollection ProfilingTargets
        {
            get { return _profilingTargets; }
        }

        public IProfilingTypeCollection ProfilingTypes
        {
            get { return _profilingTypes; }
        }

        public IRequestServer RequestServer
        {
            get { return _requestServer; }
        }

        protected override void RunInternal()
        {
            _frameworks = new FrameworkCollection(Catalog.Frameworks, ExportLoader);
            _profilingTypes = new ProfilingTypeCollection(Catalog.ProfilingTypes, ExportLoader);
            _profilingTargets = new ProfilingTargetCollection(Catalog.ProfilingTargets, ExportLoader);
            _configurations = new ConfigurationCollection(_profilingTargets, _frameworks, this);
            _sessions = new SessionCollection(this);
            _requestServer = new RequestServer();

            _requestServer.RegisterHandler(new StartProfilingSessionRequestHandler(
                _configurations, _sessions, _frameworks, _profilingTypes, _profilingTargets));

            //TODO: Host must listen all possible types of streams (NamePipes, SharedMemory, RPC) _
            //TODO: _ because agents may use different transports, depending on apps they profile
            IServerStream serverStream = Connector.Native.StreamFactory.CreateInvokeStream();
            _requestServer.Run(serverStream);

            ServiceContainer.Register(new Remote.IO.FileSystemAccessor(true));
        }

        public override void Dispose()
        {
            _requestServer.Dispose();
            _configurations.Dispose();
            _sessions.Dispose();
            _frameworks.Dispose();
            _profilingTargets.Dispose();
            _profilingTypes.Dispose();
            base.Dispose();
        }
    }
}
