using System;

namespace Chronos.Host
{
	public interface IHostApplication : IDisposable
	{
		IConfigurationCollection Configurations { get; }

		ISessionCollection Sessions { get; }

		string MachineName { get; }

		void Quit();

		string Ping(string message);
	}
}
