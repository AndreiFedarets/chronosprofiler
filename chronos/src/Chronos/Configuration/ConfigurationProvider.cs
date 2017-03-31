using System.Configuration;
using System.IO;
using System;
using System.Xml.Serialization;

namespace Chronos.Configuration
{
	public static class ConfigurationProvider
	{
		public static ICommonConfiguration Load(string path)
		{
			path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
			FileInfo fileInfo = new FileInfo(path);
			XmlSerializer serializer = new XmlSerializer(typeof(CommonConfiguration));
			CommonConfiguration configuration;
			using (Stream stream = fileInfo.OpenRead())
			{
				configuration = (CommonConfiguration)serializer.Deserialize(stream);
			}
			UpdateAgentConfiguration(fileInfo, configuration.Agent);
			UpdateHostConfiguration(fileInfo, configuration.Host);
			UpdateDaemonConfiguration(fileInfo, configuration.Daemon);
			return configuration;
		}

		public static ICommonConfiguration Load()
		{
			string configurationFile = ConfigurationManager.AppSettings["configurationFile"];
			return Load(configurationFile);
		}

		private static void UpdateDaemonConfiguration(FileInfo configurationFile, DaemonConfiguration configuration)
		{
			configuration.BinaryPath = Path.Combine(configurationFile.DirectoryName, configuration.BinaryPath);
		}

		private static void UpdateHostConfiguration(FileInfo configurationFile, HostConfiguration configuration)
		{
			configuration.BinaryPath = Path.Combine(configurationFile.DirectoryName, configuration.BinaryPath);
		}

		private static void UpdateAgentConfiguration(FileInfo configurationFile, AgentConfiguration configuration)
		{
			configuration.BinaryPath = Path.Combine(configurationFile.DirectoryName, configuration.BinaryPath);
		}
	}
}
