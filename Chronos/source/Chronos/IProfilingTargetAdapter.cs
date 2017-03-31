namespace Chronos
{
    public interface IProfilingTargetAdapter
    {
        IProfilingTargetController CreateController(ConfigurationSettings settings);

        bool CanStartProfiling(ConfigurationSettings settings, int processId);

        void ProfilingStarted(ConfigurationSettings configurationSettings, SessionSettings sessionSettings, int processId);
    }
}
