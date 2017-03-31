using System;
using System.Diagnostics;
using System.IO;
using System.ServiceProcess;
using System.Threading;
using Chronos.Communication.Remoting;
using Chronos.Configuration;

namespace Chronos.Host
{
	public static class HostProvider
	{
		public static void Run(ICommonConfiguration configuration)
		{
			if (CheckConnection())
			{
				throw new InvalidOperationException("host already running");
			}
			switch (configuration.Host.Runtype)
			{
				case HostRuntype.Inplace:
					RunInplace(null);
					break;
				case HostRuntype.Application:
					RunApplication(configuration);
					break;
				case HostRuntype.Service:
					RunService();
					break;
				default:
					RunInplace(null);
					break;
			}
		}

		public static IHostApplication RunInplace(Action close)
		{
			Internal.HostApplication application = new Internal.HostApplication(close);
			RemotingFactory.Current.Share<IHostApplication>(application, Internal.HostApplication.Token.ToString());
			return application;
		}

		private static void RunApplication(ICommonConfiguration configuration)
		{
			string path = configuration.Host.BinaryPath;
			const string name = "Chronos.Host.EntryPoint.Application.exe";
			string fullName = Path.Combine(path, name);
			Process process = new Process();
			process.StartInfo = new ProcessStartInfo(fullName);
			process.StartInfo.UseShellExecute = false;
			process.Start();
			while (!CheckConnection())
			{
				Thread.Sleep(100);
			}
		}

		private static void RunService()
		{
			const string name = "Chronos Host Service";
			ServiceController controller = new ServiceController(name);
			controller.Start();
			while (!CheckConnection())
			{
				Thread.Sleep(100);
			}
		}

		public static bool CheckConnection()
		{
			try
			{
				const string message = "test";
				IHostApplication application = RemotingFactory.Current.Connect<IHostApplication>(Internal.HostApplication.Token.ToString());
				return string.Equals(application.Ping(message), message, StringComparison.InvariantCultureIgnoreCase);
			}
			catch (Exception)
			{
				return false;
			}
		}

		public static IHostApplication Connect(IRemotingExecutor remotingExecutor)
		{
			if (!CheckConnection())
			{
				throw new InvalidOperationException("host is not running");
			}
			IHostApplication application = RemotingFactory.Current.Connect<IHostApplication>(Internal.HostApplication.Token.ToString());
			application = new Proxy.HostApplication(application, remotingExecutor);
			return application;
		}
	}
}
