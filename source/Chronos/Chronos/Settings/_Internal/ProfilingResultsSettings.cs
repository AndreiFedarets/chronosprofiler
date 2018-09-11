using System.Xml.Linq;

namespace Chronos.Settings
{
    internal sealed class ProfilingResultsSettings : DirectorySettings, IProfilingResultsSettings
    {
        public ProfilingResultsSettings(XElement element)
            : base(element)
        {
        }
    }
}
