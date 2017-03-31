using System.Collections.Generic;
using System;

namespace Chronos.Core
{
	[Serializable]
	public class ConfigurationSettings 
	{
		public ConfigurationSettings()
		{
			Name = string.Empty;
			Token = Guid.NewGuid();
			InitialState = SessionState.Closed;
			Events = ClrEventsMask.MonitorNone;
			TargetProcessName = string.Empty;
			ProcessTargetArguments = string.Empty;
			FilterType = FilterType.Include;
			FilterItems = new List<string>();
			UseFastHooks = false;
		    ProfileChildProcess = false;
		    ProfileSql = false;
		}

		public ConfigurationSettings(string name, Guid token, SessionState initialState, ClrEventsMask events, string targetProcessName,
            string processTargetArguments, FilterType filterType, List<string> filterItems, bool useFastHooks, bool profileChildProcess, bool profileSql)
		{
			Name = name;
			Token = token;
			InitialState = initialState;
			Events = events;
			TargetProcessName = targetProcessName;
			ProcessTargetArguments = processTargetArguments;
			FilterType = filterType;
			FilterItems = filterItems;
            UseFastHooks = useFastHooks;
            ProfileChildProcess = profileChildProcess;
		    ProfileSql = profileSql;
		}

		public string Name { get; set; }

		public Guid Token { get; set; }

		public SessionState InitialState { get; set; }

		public ClrEventsMask Events { get; set; }

		public string TargetProcessName { get; set; }

		public string ProcessTargetArguments { get; set; }

		public FilterType FilterType { get; set; }

		public bool UseFastHooks { get; set; }

        public bool ProfileSql { get; set; }

		public List<string> FilterItems { get; set; }

        public bool ProfileChildProcess { get; set; }

		public void RefreshToken()
		{
			Token = Guid.NewGuid();
		}
	}
}
