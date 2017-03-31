using System;

namespace Chronos.Core
{
	[Serializable]
	public class SessionInfo
	{
		public SessionInfo(Guid configurationToken, Guid sessionToken, SessionState sessionState, ProcessInfo processInfo)
		{
			ConfigurationToken = configurationToken;
			SessionToken = sessionToken;
			State = sessionState;
			ProcessInfo = processInfo;
		}

		public SessionInfo()
		{
			
		}

		public SessionState State { get; set; }

		public Guid SessionToken { get; private set; }

		public Guid ConfigurationToken { get; private set; }

		public ProcessInfo ProcessInfo { get; private set; }

		public ProcessPlatform ProcessPlatform { get; private set; }
	}
}
