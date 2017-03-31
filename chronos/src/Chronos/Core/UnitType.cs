using System;
namespace Chronos.Core
{
    [Flags]
	public enum UnitType : uint
	{
		AppDomain = 1,
		Assembly = 2,
		Module = 4,
		Class = 8,
		Function = 16,
		Thread = 32,
		Exception = 64,
		Process = 128,
        Callstack = 256,
        SqlRequest = 512,
	}
}
