using Adenium;
using Adenium.Layouting;
using Chronos.DotNet.SqlProfiler;

namespace Chronos.Client.Win.DotNet.SqlProfiler
{
    public sealed class ProfilingTypeAdapter : IProfilingTypeAdapter, ILayoutProvider, IServiceConsumer
    {
        private ISqlQueryCollection _sqlQueries;

        void ILayoutProvider.ConfigureContainer(IViewModel targetViewModel, IContainer container)
        {
            container.RegisterInstance(_sqlQueries);
        }

        string ILayoutProvider.GetLayout(IViewModel targetViewModel)
        {
            return LayoutFileReader.ReadViewModelLayout(targetViewModel);
        }

        void IServiceConsumer.ExportServices(IServiceContainer container)
        {
        }

        void IServiceConsumer.ImportServices(IServiceContainer container)
        {
            _sqlQueries = container.Resolve<ISqlQueryCollection>();
        }
    }
}
