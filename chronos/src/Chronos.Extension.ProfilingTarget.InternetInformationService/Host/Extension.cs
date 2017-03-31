using Chronos.Extensibility;

namespace Chronos.Extension.ProfilingTarget.InternetInformationService.Host
{
	public class Extension : IExtension
	{
		private readonly ISessionActivatorProvider _sessionActivatorProvider;

		public Extension(ISessionActivatorProvider sessionActivatorProvider)
		{
			_sessionActivatorProvider = sessionActivatorProvider;
		}

		public void Initialize()
		{
			_sessionActivatorProvider.Register(typeof(SessionActivator), Constants.ActivatorCode);
		}
	}
}
