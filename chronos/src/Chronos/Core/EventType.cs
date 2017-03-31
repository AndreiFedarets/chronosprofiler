namespace Chronos.Core
{
	public enum EventType : byte
	{
		ThreadTrace = 0x00,
		FunctionCall = 0x01,
		AppDomainCreation = 0x02,
		AppDomainShutdown = 0x03,
		AssemblyLoad = 0x04,
		AssemblyUnload = 0x05,
		ModuleLoad = 0x06,
		ModuleUnload = 0x07,
		ClassLoad = 0x08,
		ClassUnload = 0x09,
		ThreadCreate = 0x0A,
		ThreadDestroy = 0x0B,
		GarbageCollection = 0x0C,
		ExceptionThrown = 0x0D
	}
}
