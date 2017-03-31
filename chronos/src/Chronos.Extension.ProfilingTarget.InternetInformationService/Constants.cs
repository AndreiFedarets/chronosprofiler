namespace Chronos.Extension.ProfilingTarget.InternetInformationService
{
	public static class Constants
	{
		public static class ViewNames
		{
			public static class Win
			{
				public const string InternetInformationService = "Profiler_ProfilingTarget_InternetInformationServiceView";
			}
		}

		public static class ResourcesKeys
		{
            public const string DisplayNameKey = "Profiler_ProfilingTarget_InternetInformationServiceView_Title";
            public const string IconKey = "Profiler_ProfilingTarget_InternetInformationServiceView_Icon";
		}

		public const byte ActivatorCode = 1;

		public const string HostProcessName = "w3wp.exe";
		public const string W3SVCServiceName = "W3SVC";
		public const string WASServiceName = "WAS";
		public const string ServicesProcessName = "services";
	}
}
