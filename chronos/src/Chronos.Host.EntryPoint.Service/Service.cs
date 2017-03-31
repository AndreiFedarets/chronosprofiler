using System.ServiceProcess;

namespace Chronos.Host.EntryPoint.Service
{
	public partial class Service : ServiceBase
	{
		public Service()
		{
			InitializeComponent();
		}

		protected override void OnStart(string[] args)
		{
			HostProvider.RunInplace(Stop);
		}

		protected override void OnStop()
		{
			//HostProvider.Shutdown();
		}
	}
}
