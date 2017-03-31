using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using Chronos.Communication.Remoting;

namespace Chronos.Daemon.EntryPoint
{
	static class Program
	{
		[STAThread]
		static void Main()
		{
			Guid sessionToken;
			Guid configurationToken;
			using (Process current = Process.GetCurrentProcess())
			{
				try
				{
					string daemonTokenString = current.StartInfo.EnvironmentVariables[Core.Constants.EnvironmentVariablesNames.SessionTokenSettingName];
					sessionToken = new Guid(daemonTokenString);
					string sessionTokenString = current.StartInfo.EnvironmentVariables[Core.Constants.EnvironmentVariablesNames.ConfigurationTokenSettingName];
					configurationToken = new Guid(sessionTokenString);
				}
				catch (Exception)
				{
					return;
				}
			}
			if (sessionToken == Guid.Empty || configurationToken == Guid.Empty)
			{
				return;
			}
			RemotingFactory.Current.Initialize(new IpcChannelFactory());
			DaemonProvider.RunInplace(Exit, configurationToken, sessionToken);
			Application.Run();
		}

		static void Exit()
		{
			ThreadPool.QueueUserWorkItem(s =>
			{
				Thread.Sleep(100);
				Application.Exit();
			});
		}
	}
}
