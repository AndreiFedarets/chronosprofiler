using Chronos.Extensibility;
using Chronos.Prerequisites;

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
