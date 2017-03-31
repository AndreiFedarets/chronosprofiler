using System;
using System.IO;
using Chronos.Core;
using Rhiannon.Logging;
using Rhiannon.Serialization;

namespace Chronos.Storage
{
	internal class ConfigurationStorage
	{
		private const string ConfigurationFileName = "configuration";
		private readonly FileInfo _configurationFile;
		private readonly DirectoryInfo _configurationDirectory;
		private readonly ISerializerFactory _serializerFactory;

		public ConfigurationStorage(DirectoryInfo configurationDirectory, ISerializerFactory serializerFactory)
		{
			Token = new Guid(configurationDirectory.Name);
			_configurationDirectory = configurationDirectory;
			_configurationFile = new FileInfo(Path.Combine(_configurationDirectory.FullName, ConfigurationFileName));
			_serializerFactory = serializerFactory;
			SessionStorages = new SessionStorageCollection(configurationDirectory, this);
		}

		public Guid Token { get; private set; }

		public SessionStorageCollection SessionStorages { get; private set; }

		public ConfigurationInfo GetInfo()
		{
			ConfigurationInfo configuration = null;
			try
			{
				ISerializer serializer = _serializerFactory.Create<ConfigurationInfo>(SerializerType.Binary);
				configuration = serializer.Deserialize<ConfigurationInfo>(_configurationFile);
			}
			catch (Exception exception)
			{
				LoggingProvider.Log(exception, Policy.Data);
			}
			return configuration;
		}

		public void SetInfo(ConfigurationInfo configuration)
		{
			try
			{
				ISerializer serializer = _serializerFactory.Create<ConfigurationInfo>(SerializerType.Binary);
				serializer.Serialize(_configurationFile);
			}
			catch (Exception exception)
			{
				LoggingProvider.Log(exception, Policy.Data);
			}
		}

		public void Remove()
		{
			_configurationDirectory.Delete();
		}
	}
}
