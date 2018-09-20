using Chronos.Extensibility;
using Chronos.Prerequisites;

namespace Chronos.Proxy
{
    internal sealed class ProfilingType : ProxyBaseObject<IProfilingType>, IProfilingType
    {
        public ProfilingType(IProfilingType remoteObject)
            : base(remoteObject)
        {
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
            get { return Execute(() => RemoteObject.Prerequisites); }
        }

        public string GetAgentDll(ProcessPlatform processPlatform)
        {
            return Execute(() => RemoteObject.GetAgentDll(processPlatform));
        }
    }
}
