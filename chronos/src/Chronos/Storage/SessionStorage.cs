using System;
using System.IO;
using Chronos.Configuration;
using Rhiannon.Extensions;

namespace Chronos.Storage
{
	internal class SessionStorage : ISessionStorage
	{
		private const string UnitsDirectoryName = "units";
		private const string CallstacksDirectoryName = "callstacks";
		private const string ProcessInfoFileName = "process";
		private readonly DirectoryInfo _sessionDirectory;
		private readonly DirectoryInfo _unitsDirectory;
		private readonly DirectoryInfo _callstacksDirectory;
		private readonly ICommonConfiguration _commonConfiguration;

		public SessionStorage(Guid sessionToken, ICommonConfiguration commonConfiguration)
		{
			_commonConfiguration = commonConfiguration;
			string sessionPath = Path.Combine(_commonConfiguration.Host.SessionsLocation, sessionToken.ToString());
			string unitsPath = Path.Combine(sessionPath, UnitsDirectoryName);
			string callstacksPath = Path.Combine(sessionPath, CallstacksDirectoryName);
			_sessionDirectory = new DirectoryInfo(sessionPath);
			_unitsDirectory = new DirectoryInfo(unitsPath);
			_callstacksDirectory = new DirectoryInfo(callstacksPath);
			if (!_sessionDirectory.Exists)
			{
				_sessionDirectory.Create();
			}
			if (!_unitsDirectory.Exists)
			{
				_unitsDirectory.Create();
			}
			if (!_callstacksDirectory.Exists)
			{
				_callstacksDirectory.Create();
			}
		}

		public void Delete()
		{
			_sessionDirectory.Refresh();
			if (_sessionDirectory.Exists)
			{
				_sessionDirectory.Delete(true);
			}
		}

		public void WriteCallstack(uint id, Stream source)
		{
			string fileFullName = Path.Combine(_callstacksDirectory.FullName, id.ToString("D10"));
			using (FileStream destination = new FileStream(fileFullName, FileMode.Create, FileAccess.Write))
			{
				source.CopyTo(destination);
			}
		}

		public Stream ReadCallstack(uint id)
		{
			string fileFullName = Path.Combine(_callstacksDirectory.FullName, id.ToString("D10"));
			FileStream destination = new FileStream(fileFullName, FileMode.Open, FileAccess.Read);
			return destination;
		}

		//private readonly ConfigurationStorage _configurationStorage;

		//public SessionStorage(DirectoryInfo sessionDirectory, ConfigurationStorage configurationStorage)
		//{
		//    Token = new Guid(sessionDirectory.Name);
		//    _sessionDirectory = sessionDirectory;
		//    _configurationStorage = configurationStorage;
		//}

		//public Guid Token { get; private set; }

		//public SessionInfo GetSessionInfo()
		//{
		//    Guid configurationToken = _configurationStorage.Token;
		//    ProcessInfo processInfo = GetProcessInfo();
		//    SessionInfo session = new SessionInfo(configurationToken, Token, SessionState.Closed, processInfo);
		//    return session;
		//}

		//private ProcessInfo GetProcessInfo()
		//{
		//    FileInfo processInfoFile = GetProcessInfoFile();
		//    using (FileStream stream = processInfoFile.OpenRead())
		//    {
		//        ProcessInfo processInfo = MarshalingManager.Demarshal<ProcessInfo>(stream);
		//        return processInfo;
		//    }
		//}

		//private DirectoryInfo GetUnitsDirectory()
		//{
		//    DirectoryInfo unitsDirectory = new DirectoryInfo(Path.Combine(_sessionDirectory.FullName, UnitsDirectoryName));
		//    return unitsDirectory;
		//}

		//private FileInfo GetProcessInfoFile()
		//{
		//    DirectoryInfo unitsDirectory = GetUnitsDirectory();
		//    FileInfo processInfoFile = new FileInfo(Path.Combine(unitsDirectory.FullName, ProcessInfoFileName));
		//    return processInfoFile;
		//}
	}
}
