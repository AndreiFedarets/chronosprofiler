namespace Chronos.Host
{
    /// <summary>
    /// Represents Host Server application
    /// </summary>
    public interface IApplication : IChronosApplication
    {
        /// <summary>
        /// Collection of created Configurations
        /// </summary>
        IConfigurationCollection Configurations { get; }

        /// <summary>
        /// Collection of created Sessions
        /// </summary>
        ISessionCollection Sessions { get; }

        /// <summary>
        /// Collection of available for profiling Frameworks
        /// </summary>
        IFrameworkCollection Frameworks { get; }

        /// <summary>
        /// Collection of available Profiling Targets (target applications) from all Frameworks
        /// </summary>
        IProfilingTargetCollection ProfilingTargets { get; }

        /// <summary>
        /// Collection of available Profiling Types from all Frameworks
        /// </summary>
        IProfilingTypeCollection ProfilingTypes { get; }
    }
}
