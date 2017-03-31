using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using Microsoft.Web.Administration;

namespace Chronos.Extension.ProfilingTarget.InternetInformationService
{
	public class InternetInformationService : IInternetInformationService
	{
		private readonly ServiceController _w3SvcService;
		private readonly ServiceController _wasService;
		private readonly ServerManager _serverManager;

		public InternetInformationService()
		{
			_serverManager = new ServerManager();
			_w3SvcService = new ServiceController(Constants.W3SVCServiceName);
			_wasService = new ServiceController(Constants.WASServiceName);
            ApplicationPools = new ApplicationPoolCollection(_serverManager.ApplicationPools);
		}

        public IApplicationPoolCollection ApplicationPools { get; private set; }

		public void Start()
		{
			_wasService.StartSafe();
			_w3SvcService.StartSafe();
		}

		public void Stop()
		{
			_w3SvcService.StopSafe();
			_wasService.StopSafe();
		}

		public void AppendEnvironmentVariables(StringDictionary variables)
		{
			Process process = Process.GetProcessesByName(Constants.ServicesProcessName).First();
			StringDictionary servicesVariables = process.StartInfo.EnvironmentVariables;//.GetEnvironmentVariables();
			StringDictionary combinedVariables = EnvironmentVariablesConverter.Merge(servicesVariables, variables);
			_wasService.SetEnvironmentVariables(combinedVariables);
			_w3SvcService.SetEnvironmentVariables(combinedVariables);
		}

		public void RemoveEnvironmentVariables(StringDictionary variables)
		{
			Process process = Process.GetProcessesByName(Constants.ServicesProcessName).First();
			StringDictionary servicesVariables = process.StartInfo.EnvironmentVariables;//.GetEnvironmentVariables();
			StringDictionary combinedVariables = EnvironmentVariablesConverter.Exclude(servicesVariables, variables);
			_wasService.SetEnvironmentVariables(combinedVariables);
			_w3SvcService.SetEnvironmentVariables(combinedVariables);
		}

		public StringDictionary GetEnvironmentVariables()
		{
			Process process = Process.GetProcessesByName(Constants.ServicesProcessName).First();
			StringDictionary servicesVariables = process.StartInfo.EnvironmentVariables;
			return servicesVariables;
		}


		public void CloseHostProcesses()
		{
			Process[] hosts = Process.GetProcessesByName(Constants.HostProcessName);
			foreach (Process host in hosts)
			{
				if (!host.HasExited)
				{
					host.Kill();
				}
			}
		}
	}
}
