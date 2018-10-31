using Adenium;
using Adenium.Layouting;

namespace Chronos.Client.Win.DotNet.SqlProfiler
{
    public sealed class ProfilingTypeAdapter : IProfilingTypeAdapter, ILayoutProvider
    {
        void ILayoutProvider.ConfigureContainer(IContainer container)
        {
        }

        string ILayoutProvider.GetLayout(IViewModel viewModel)
        {
            return LayoutFileReader.ReadViewModelLayout(viewModel);
        }
    }
}
