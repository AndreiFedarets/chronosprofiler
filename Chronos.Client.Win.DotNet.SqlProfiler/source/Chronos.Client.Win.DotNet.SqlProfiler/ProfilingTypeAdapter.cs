using Chronos.Client.Win.ViewModels;

namespace Chronos.Client.Win.DotNet.SqlProfiler
{
    public sealed class ProfilingTypeAdapter : IProfilingTypeAdapter
    {
        public IProfilingTypeSession CreateDecodingSession(ProfilingTypeSettings profilingTypeSettings)
        {
            return new ProfilingTypeSession();
        }

        public IProfilingTypeSettingsViewModel CreateSettingsViewModel(ProfilingTypeSettings profilingTypeSettings)
        {
            return null;
        }
    }
}
