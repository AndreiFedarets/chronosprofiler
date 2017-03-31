using Chronos.Extensibility;

namespace Chronos.Proxy
{
    internal sealed class ProfilingTarget : ProxyBaseObject<IProfilingTarget>, IProfilingTarget
    {
        public ProfilingTarget(IProfilingTarget remoteObject)
            : base(remoteObject)
        {
        }

        public ProfilingTargetDefinition Definition
        {
            get { return Execute(() => RemoteObject.Definition); }
        }

        public bool HasAgent
        {
            get { return Execute(() => RemoteObject.HasAgent); }
        }

        public string GetAgentDll(ProcessPlatform processPlatform)
        {
            return Execute(() => RemoteObject.GetAgentDll(processPlatform));
        }
    }
}
