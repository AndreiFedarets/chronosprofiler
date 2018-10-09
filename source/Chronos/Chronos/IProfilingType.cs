using Chronos.Extensibility;

namespace Chronos
{
    public interface IProfilingType
    {
        ProfilingTypeDefinition Definition { get; }

        bool HasAgent { get; }

        IPrerequisiteCollection Prerequisites { get; }

        string GetAgentDll(ProcessPlatform processPlatform);
    }
}
