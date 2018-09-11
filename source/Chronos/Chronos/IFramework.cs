using Chronos.Extensibility;

namespace Chronos
{
    public interface IFramework
    {
        FrameworkDefinition Definition { get; }

        bool HasAgent { get; }

        string GetAgentDll(ProcessPlatform processPlatform);
    }
}