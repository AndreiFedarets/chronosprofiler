using Chronos.Extensibility;
using Chronos.Prerequisites;

namespace Chronos
{
    public interface IFramework
    {
        FrameworkDefinition Definition { get; }

        bool HasAgent { get; }

        IPrerequisiteCollection Prerequisites { get; }

        string GetAgentDll(ProcessPlatform processPlatform);
    }
}