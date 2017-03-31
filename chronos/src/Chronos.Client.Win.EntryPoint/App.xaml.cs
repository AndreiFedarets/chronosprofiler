using System.Windows;

namespace Chronos.Client.Win.EntryPoint
{
	public partial class App
	{
		private readonly Bootstrapper _bootstrapper;

		public App()
		{
			_bootstrapper = new Bootstrapper();
		}

		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);
			_bootstrapper.Run();
		}

		protected override void OnExit(ExitEventArgs e)
		{
			_bootstrapper.Shutdown();
			base.OnExit(e);
		}
	}
}
