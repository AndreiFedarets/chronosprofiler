using System.Collections.Generic;
using Chronos.Extensibility;

namespace Chronos.Client
{
    public interface IFramework
    {
        FrameworkDefinition Definition { get; }

        /// <summary>
        /// Get availability of Framework. 
        /// True if there is any active Host Application that supports this Framework, otherwise - False
        /// </summary>
        bool IsAvailable { get; }

        bool IsHidden { get; }

        /// <summary>
        /// Get collection of Host Application that supports this Profiling Type
        /// </summary>
        IEnumerable<Host.IApplication> SupportedApplications { get; }

        /// <summary>
        /// List of Profiling Types for the Framework
        /// </summary>
        IEnumerable<IProfilingType> ProfilingTypes { get; }
    }
}
