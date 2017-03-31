using Chronos.Client.Win.ViewModels;

namespace Chronos.Client.Win.DotNet.SqlProfiler
{
    internal sealed class ProfilingTypeSession : IProfilingTypeSession
    {
        private readonly MsSqlQueryCollection _msSqlQueries;

        public ProfilingTypeSession()
        {
            _msSqlQueries = new MsSqlQueryCollection();
        }

        public void ExportServices(IServiceContainer container)
        {
            container.Register<IMsSqlQueryCollection>(_msSqlQueries);
        }

        public void ImportServices(IServiceContainer container)
        {
            Daemon.DotNet.SqlProfiler.IMsSqlQueryCollection daemonMsSqlQueries =
                container.Resolve<Daemon.DotNet.SqlProfiler.IMsSqlQueryCollection>();
            _msSqlQueries.Initialize(daemonMsSqlQueries);

        }

        public void IntegrateViewModel(object profilingResultsViewModel)
        {
            ProfilingResultsViewModel viewModel = (ProfilingResultsViewModel)profilingResultsViewModel;
            IMenuItem basicInformationMenuItem = viewModel.Menu.AddMenuItem(new SqlMenuItem());
            basicInformationMenuItem.AddMenuItem(new MsSqlQueriesMenuItem(viewModel.Session));
        }

        public void ReloadData()
        {

        }
    }
}
