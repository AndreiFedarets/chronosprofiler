using Chronos.Extensibility;

namespace Chronos
{
    public interface IProfilingTarget
    {
        ProfilingTargetDefinition Definition { get; }

        bool HasAgent { get; }

        string GetAgentDll(ProcessPlatform processPlatform);
    }
}
