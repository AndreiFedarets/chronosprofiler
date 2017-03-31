using System;
using System.Diagnostics;
using System.IO;
using Chronos.Configuration;

namespace Chronos.Installation
{
	public class ProfilerInstaller : IProfilerInstaller
	{
		private readonly ICommonConfiguration _configuration;

		public ProfilerInstaller(ICommonConfiguration configuration)
		{
			_configuration = configuration;
		}

		public void InstallAgent()
		{
			ExecuteInternal(Constants.Installation.Regsvr32Name, FormatArgsForProfiler32Install);
			ExecuteInternal(Constants.Installation.Regsvr32Name, FormatArgsForProfiler64Install);
		}

		public void UninstallAgent()
		{
			ExecuteInternal(Constants.Installation.Regsvr32Name, FormatArgsForProfiler32Uninstall);
			ExecuteInternal(Constants.Installation.Regsvr32Name, FormatArgsForProfiler64Uninstall);
		}

		public void InstallService()
		{
			ExecuteInternal(GetInstallUtillFullName(), FormatArgsForServiceInstall);
		}

		public void UninstallService()
		{
			ExecuteInternal(GetInstallUtillFullName(), FormatArgsForServiceUninstall);
		}

		public void InstallProfiler()
		{
			InstallAgent();
			//InstallService();
		}

		public void UninstallProfiler()
		{
			UninstallAgent();
			//UninstallService();
		}

		private void ExecuteInternal(string executable, Func<string> getArgsFunc)
		{
			Process process = new Process();
			process.StartInfo.FileName = executable;
			process.StartInfo.Arguments = getArgsFunc();
			process.StartInfo.UseShellExecute = false;
			process.StartInfo.CreateNoWindow = true;
			process.StartInfo.RedirectStandardOutput = false;
			process.Start();
			process.WaitForExit();
			process.Close();
		}

		private string FormatArgsForProfiler32Install()
		{
			string fileFullName = Path.Combine(_configuration.Agent.BinaryPath, _configuration.Agent.EntryPoint32);
			return string.Format("/s \"{0}\"", fileFullName);
		}

		private string FormatArgsForProfiler32Uninstall()
		{
			string fileFullName = Path.Combine(_configuration.Agent.BinaryPath, _configuration.Agent.EntryPoint32);
			return string.Format("/s /u \"{0}\"", fileFullName);
		}

		private string FormatArgsForProfiler64Install()
		{
			string fileFullName = Path.Combine(_configuration.Agent.BinaryPath, _configuration.Agent.EntryPoint64);
			return string.Format("/s \"{0}\"", fileFullName);
		}

		private string FormatArgsForProfiler64Uninstall()
		{
			string fileFullName = Path.Combine(_configuration.Agent.BinaryPath, _configuration.Agent.EntryPoint64);
			return string.Format("/s /u \"{0}\"", fileFullName);
		}

		private string FormatArgsForServiceInstall()
		{
			string fileFullName = Path.Combine(_configuration.Host.BinaryPath, _configuration.Host.EntryPoint);
			return string.Format("\"{0}\"", fileFullName);
		}

		private string FormatArgsForServiceUninstall()
		{
			string fileFullName = Path.Combine(_configuration.Host.BinaryPath, _configuration.Host.EntryPoint);
			return string.Format("/u \"{0}\"", fileFullName);
		}

		private string GetInstallUtillFullName()
		{
			string path = Runtime.GetDotNetInstallationFolder();
			return Path.Combine(path, Constants.Installation.InstallUtilName);
		}


		//public void StartServer()
		//{
		//    ServiceController controller = new ServiceController("Chronos Host Service");
		//    if (controller.Status != ServiceControllerStatus.Running)
		//    {
		//        controller.Start();
		//    }
		//    controller.WaitForStatus(ServiceControllerStatus.Running, new TimeSpan(0, 0, 0, 20));
		//}

		//public bool IsServerStarted 
		//{
		//    get
		//    {
		//        try
		//        {
		//            ServiceController controller = new ServiceController("Chronos Host Service");
		//            return controller.Status == ServiceControllerStatus.Running;
		//        }
		//        catch (Exception)
		//        {
		//            return false;
		//        }
		//    }
		//}

		//public void StopServer()
		//{
		//    ServiceController controller = new ServiceController("Chronos Host Service");
		//    if (controller.Status != ServiceControllerStatus.Stopped)
		//    {
		//        controller.Stop();
		//    }
		//    controller.WaitForStatus(ServiceControllerStatus.Stopped, new TimeSpan(0, 0, 0, 20));
		//}
	}
}
