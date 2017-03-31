namespace Chronos.Core
{
	public interface IProfilerAgentSelector
	{
		string SelectProcessAgent(string processFullName);

		string SelectProcessAgent(ProcessPlatform processPlatform);

		string GetCurrentSystemProcessAgent();
	}
}
