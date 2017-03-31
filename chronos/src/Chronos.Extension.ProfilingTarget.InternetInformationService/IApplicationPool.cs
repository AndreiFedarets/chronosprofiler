namespace Chronos.Extension.ProfilingTarget.InternetInformationService
{
	public interface IApplicationPool
	{
		string Name { get; }

	    void Restart();
	}
}
