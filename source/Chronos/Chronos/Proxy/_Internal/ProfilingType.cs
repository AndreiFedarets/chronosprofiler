using Chronos.Extensibility;

namespace Chronos.Proxy
{
    internal sealed class ProfilingType : ProxyBaseObject<IProfilingType>, IProfilingType
    {
        private readonly LazyValue<PrerequisiteCollection> _prerequisites;

        public ProfilingType(IProfilingType remoteObject)
            : base(remoteObject)
        {
            _prerequisites = new LazyValue<PrerequisiteCollection>(() => new PrerequisiteCollection(remoteObject.Prerequisites));
        }

        public ProfilingTypeDefinition Definition
        {
            get { return Execute(() => RemoteObject.Definition); }
        }

        public bool HasAgent
        {
            get { return Execute(() => RemoteObject.HasAgent); }
        }

        public IPrerequisiteCollection Prerequisites
        {
            get { return _prerequisites.Value; }
        }

        public string GetAgentDll(ProcessPlatform processPlatform)
        {
            return Execute(() => RemoteObject.GetAgentDll(processPlatform));
        }
    }
}
