using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Chronos.Communication.Remoting;
using Chronos.Configuration;
using Chronos.Core;

namespace Chronos.Daemon
{
	public static class DaemonProvider
	{
		public static void Run(ICommonConfiguration configuration, Guid configurationToken, Guid sessionToken)
		{
			if (CheckConnection(configurationToken, sessionToken))
			{
				throw new InvalidOperationException("host already running");
			}
			switch (configuration.Daemon.Runtype)
			{
				case DaemonRuntype.Inplace:
					RunInplace(null, configurationToken, sessionToken);
					break;
				case DaemonRuntype.Application:
					RunApplication(configuration, configurationToken, sessionToken);
					break;
				default:
					RunInplace(null, configurationToken, sessionToken);
					break;
			}
		}

		public static bool CheckConnection(Guid configurationToken, Guid sessionToken)
		{
			try
			{
				const string message = "test";
				IDaemonApplication application = RemotingFactory.Current.Connect<IDaemonApplication>(sessionToken.ToString());
				return string.Equals(application.Ping(message), message, StringComparison.InvariantCultureIgnoreCase);
			}
			catch (Exception)
			{
				return false;
			}
		}

		public static IDaemonApplication RunInplace(Action close, Guid configurationToken, Guid sessionToken)
		{
			Internal.DaemonApplication application = new Internal.DaemonApplication(close, configurationToken, sessionToken);
			RemotingFactory.Current.Share<IDaemonApplication>(application, sessionToken.ToString());
			return application;
		}

		private static void RunApplication(ICommonConfiguration configuration, Guid configurationToken, Guid sessionToken)
		{
			string path = configuration.Daemon.BinaryPath;
			string name = configuration.Daemon.EntryPoint;
			string fullName = Path.Combine(path, name);
			Process process = new Process();
			process.StartInfo = new ProcessStartInfo(fullName);
			process.StartInfo.EnvironmentVariables[Constants.EnvironmentVariablesNames.ConfigurationTokenSettingName] = configurationToken.ToString();
			process.StartInfo.EnvironmentVariables[Constants.EnvironmentVariablesNames.SessionTokenSettingName] = sessionToken.ToString();
			process.StartInfo.UseShellExecute = false;
			process.Start();
			while (!CheckConnection(configurationToken, sessionToken))
			{
				Thread.Sleep(100);
			}
		}

		public static IDaemonApplication Connect(Guid configurationToken, Guid sessionToken)
		{
			if (!CheckConnection(configurationToken, sessionToken))
			{
				throw new InvalidOperationException("daemon is not running");
			}
			IDaemonApplication application = RemotingFactory.Current.Connect<IDaemonApplication>(sessionToken.ToString());
			application = new Proxy.DaemonApplication(application);
			return application;
		}
	}
}
