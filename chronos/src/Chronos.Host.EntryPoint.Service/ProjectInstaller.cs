using System.ComponentModel;
using System.Configuration.Install;


namespace Chronos.Host.EntryPoint.Service
{
	[RunInstaller(true)]
	public partial class ProjectInstaller :Installer
	{
		public ProjectInstaller()
		{
			InitializeComponent();
		}
	}
}
