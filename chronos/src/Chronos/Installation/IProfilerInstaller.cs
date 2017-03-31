namespace Chronos.Installation
{
	public interface IProfilerInstaller
	{
		void InstallProfiler();

		void UninstallProfiler();

		void InstallAgent();

		void UninstallAgent();

		void InstallService();

		void UninstallService();
	}
}
