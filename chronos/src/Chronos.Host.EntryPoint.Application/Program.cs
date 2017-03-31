using System;
using System.Diagnostics;
using System.Windows.Forms;
using Chronos.Communication.Remoting;

namespace Chronos.Host.EntryPoint.Application
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			using (Process process = Process.GetCurrentProcess())
			{
				string arguments = process.StartInfo.Arguments;
				if (string.IsNullOrEmpty(arguments) || !arguments.Contains("-h"))
				{
					NotifyIcon notifyIcon = new NotifyIcon();
					notifyIcon.Icon = Properties.Resources.main;
					notifyIcon.Visible = true;
					MenuItem exitMenuItem = new MenuItem(Properties.Resources.Exit, OnExitClick);
					notifyIcon.ContextMenu = new ContextMenu(new[] { exitMenuItem });
				}
			}
			RemotingFactory.Current.Initialize(new IpcChannelFactory());
			HostProvider.RunInplace(System.Windows.Forms.Application.Exit);
			System.Windows.Forms.Application.Run();
			
		}

		private static void OnExitClick(object sender, EventArgs eventArgs)
		{
			System.Windows.Forms.Application.Exit();
		}
	}
}
