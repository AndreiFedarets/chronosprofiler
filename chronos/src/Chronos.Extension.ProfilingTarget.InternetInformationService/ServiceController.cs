using System.Collections.Specialized;
using System.ServiceProcess;
using System.Threading;
using Microsoft.Win32;

namespace Chronos.Extension.ProfilingTarget.InternetInformationService
{
	public class ServiceController : System.ServiceProcess.ServiceController
	{
		public const string ServicesRegisteryPath = "SYSTEM\\CurrentControlSet\\Services\\";
		public const string ServiceEnvironmentKeyName = "Environment";

		public ServiceController(string serviceName)
			: base(serviceName)
		{
		}

		public void StartSafe()
		{
			Refresh();
			if (Status == ServiceControllerStatus.Stopped)
			{
				Start();
				while (Status != ServiceControllerStatus.Running)
				{
					Thread.Sleep(1);
					Refresh();
				}
			}
		}

		public void StopSafe()
		{
			Refresh();
			if (Status == ServiceControllerStatus.Running)
			{
				Stop();
				while (Status != ServiceControllerStatus.Stopped)
				{
					Thread.Sleep(1);
					Refresh();
				}
			}
		}

		private RegistryKey GetLocalMachineKey()
		{
			string serviceKeyPath = ServicesRegisteryPath + ServiceName;
			RegistryKey localMachine = Registry.LocalMachine;
			RegistryKey key = localMachine.OpenSubKey(serviceKeyPath, true);
			return key;
		}

		public void SetEnvironmentVariables(StringDictionary variables)
		{
			RegistryKey key = GetLocalMachineKey();
			if (key != null)
			{
				key.SetValue(ServiceEnvironmentKeyName, EnvironmentVariablesConverter.Convert(variables));
			}
		}

		public void RemoveEnvironmentVariables()
		{
			RegistryKey key = GetLocalMachineKey();
			if (key != null)
			{
				key.DeleteValue(ServiceEnvironmentKeyName, false);
			}
		}
	}
}
