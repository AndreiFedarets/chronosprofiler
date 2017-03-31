namespace Chronos.Communication.NamedPipe
{
	public enum ResultCode : long
	{
		Ok = 0,
		UnknownError = 1,
		SessionPoolNotAvailable = 2,
		ConfigurationNotFound = 3,
		InvalidActivationSettings = 4,
		InvalidDaemonToken = 5,
		SessionNotFound = 6,
		NotSupported = 7,
	}
}
