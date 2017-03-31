using Chronos.Core;

namespace Chronos.Extensibility
{
	public interface ISessionActivator
	{
		void Activate();

		void Deactivate();

		bool Validate();

	    void OnProcessConnected(int processId);

		ActivationSettings Settings { get; }

		ConfigurationState GetState();
	}
}
