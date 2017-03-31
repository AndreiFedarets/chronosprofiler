using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Chronos.Storage
{
	internal class SessionStorageCollection : IEnumerable<SessionStorage>
	{
		private readonly IList<SessionStorage> _storages;
		private readonly DirectoryInfo _sessionsDirectory;
		private readonly ConfigurationStorage _configurationStorage;

		public SessionStorageCollection(DirectoryInfo sessionsDirectory, ConfigurationStorage configurationStorage)
		{
			_storages = new List<SessionStorage>();
			_sessionsDirectory = sessionsDirectory;
			_configurationStorage = configurationStorage;
		}

		public SessionStorage this[Guid sessionToken]
		{
			get
			{
				SessionStorage storage = _storages.FirstOrDefault(x => x.Token == sessionToken);
				return storage;
			}
		}

		public void Refresh()
		{
			_storages.Clear();
			_sessionsDirectory.Refresh();
			if (!_sessionsDirectory.Exists)
			{
				_sessionsDirectory.Create();
			}
			foreach (DirectoryInfo directory in _sessionsDirectory.GetDirectories())
			{
				SessionStorage storage = new SessionStorage(directory, _configurationStorage);
				_storages.Add(storage);
			}
		}

		public IEnumerator<SessionStorage> GetEnumerator()
		{
			return _storages.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
