using System.Collections.Generic;
using Chronos.Extensibility;

namespace Chronos.Client
{
    public interface IProfilingTarget
    {
        ProfilingTargetDefinition Definition { get; }

        /// <summary>
        /// Get collection of Host Application that supports this Profiling Target
        /// </summary>
        IEnumerable<Host.IApplication> SupportedApplications { get; }

        /// <summary>
        /// Get availability of Profiling Target.
        /// True if there is any active Host Application that supports this Profiling Target, otherwise - False
        /// </summary>
        bool IsAvailable { get; }
    }
}
