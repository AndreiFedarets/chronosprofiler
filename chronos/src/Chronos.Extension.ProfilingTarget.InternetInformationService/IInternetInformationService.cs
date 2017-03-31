using System.Collections.Specialized;

namespace Chronos.Extension.ProfilingTarget.InternetInformationService
{
	public interface IInternetInformationService
	{
        IApplicationPoolCollection ApplicationPools { get; }

		StringDictionary GetEnvironmentVariables();

		void CloseHostProcesses();
	}
}
