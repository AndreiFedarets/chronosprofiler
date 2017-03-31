using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Rhiannon.Serialization;

namespace Chronos.Storage
{
	internal class ConfigurationStorageCollection : IEnumerable<ConfigurationStorage>
	{
		private readonly IList<ConfigurationStorage> _storages;
		private readonly DirectoryInfo _configurationsDirectory;
		private readonly ISerializerFactory _serializerFactory;

		public ConfigurationStorageCollection(DirectoryInfo configurationsDirectory, ISerializerFactory serializerFactory)
		{
			_storages = new List<ConfigurationStorage>();
			_configurationsDirectory = configurationsDirectory;
			_serializerFactory = serializerFactory;
		}

		public ConfigurationStorage this[Guid configurationToken]
		{
			get
			{
				ConfigurationStorage storage = _storages.FirstOrDefault(x => x.Token == configurationToken);
				return storage;
			}
		}

		public void Refresh()
		{
			_storages.Clear();
			_configurationsDirectory.Refresh();
			if (!_configurationsDirectory.Exists)
			{
				_configurationsDirectory.Create();
			}
			foreach (DirectoryInfo directory in _configurationsDirectory.GetDirectories())
			{
				ConfigurationStorage storage = new ConfigurationStorage(directory, _serializerFactory);
				_storages.Add(storage);
			}
		}

		public ConfigurationStorage Create(Guid configurationToken)
		{
			DirectoryInfo directory = new DirectoryInfo(Path.Combine(_configurationsDirectory.FullName, configurationToken.ToString()));
			ConfigurationStorage storage = new ConfigurationStorage(directory, _serializerFactory);
			_storages.Add(storage);
			return storage;
		}

		public void Remove(Guid token)
		{
			ConfigurationStorage storage = this[token];
			_storages.Remove(storage);
			storage.Remove();
		}

		public IEnumerator<ConfigurationStorage> GetEnumerator()
		{
			return _storages.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
