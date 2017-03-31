using System;
using System.Collections.Generic;
using System.IO;
using Chronos.Configuration;
using Chronos.Core;
using Rhiannon.Serialization;

namespace Chronos.Storage
{
	public class FileStorage : IStorage
	{
		private readonly ConfigurationStorageCollection _configurations;

		public FileStorage(ICommonConfiguration configuration, ISerializerFactory serializerFactory)
		{
			DirectoryInfo configurationsDirectory = new DirectoryInfo(configuration.Host.DataPath);
			_configurations = new ConfigurationStorageCollection(configurationsDirectory, serializerFactory);
		}

		public IList<ConfigurationInfo> GetConfigurations()
		{
			IList<ConfigurationInfo> configurations = new List<ConfigurationInfo>();
			foreach (ConfigurationStorage storage in _configurations)
			{
				ConfigurationInfo configuration = storage.GetInfo();
				configurations.Add(configuration);

			}
			return configurations;
		}

		public IList<SessionInfo> GetSessions(Guid configurationToken)
		{
			IList<SessionInfo> sessions = new List<SessionInfo>();
			ConfigurationStorage configurationStorage = _configurations[configurationToken];
			foreach (SessionStorage storage in configurationStorage.SessionStorages)
			{
				SessionInfo session = storage.GetSessionInfo();
				sessions.Add(session);
			}
			return sessions;
		}

		public void SaveConfiguration(ConfigurationInfo configuration)
		{
			Guid configurationToken = configuration.ConfigurationSettings.Token;
			ConfigurationStorage storage = _configurations[configurationToken];
			if (storage == null)
			{
				storage = _configurations.Create(configurationToken);
			}
			storage.SetInfo(configuration);
		}

		public void RemoveConfiguration(Guid token)
		{
			_configurations.Remove(token);
		}


		public ISessionStorage CreateSessionStorage(Guid sessionToken)
		{
			throw new NotImplementedException();
		}
	}
}
