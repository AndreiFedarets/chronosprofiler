using System.Collections.Generic;
using Chronos.Extensibility;

namespace Chronos.Client
{
    public interface IProfilingType
    {
        ProfilingTypeDefinition Definition { get; }

        /// <summary>
        /// Target Framework
        /// </summary>
        IFramework Framework { get; }

        /// <summary>
        /// Get collection of Host Application that supports this Profiling Type
        /// </summary>
        IEnumerable<Host.IApplication> SupportedApplications { get; }

        /// <summary>
        /// Get availability of Profiling Type. 
        /// True if there is any active Host Application that supports this Profiling Type, otherwise - False
        /// </summary>
        bool IsAvailable { get; }

        /// <summary>
        /// 
        /// </summary>
        bool IsTechnical { get; }
    }
}
