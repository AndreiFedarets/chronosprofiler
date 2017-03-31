using System;

namespace Chronos.Core
{
	[Flags]
	public enum ClrEventsMask : uint
	{
		MonitorNone = 0,
		MonitorFunctionUnloads = 0x1,
		MonitorClassLoads = 0x2,
		MonitorModuleLoads = 0x4,
		MonitorAssemblyLoads = 0x8,
		MonitorAppDomainLoads = 0x10,
		MonitorJitCompilation = 0x20,
		MonitorExceptions = 0x40,
		MonitorGc = 0x80,
		MonitorObjectAllocated = 0x100,
		MonitorThreads = 0x200,
		MonitorRemoting = 0x400,
		MonitorCodeTransitions = 0x800,
		MonitorEnterLeave = 0x1000,
		MonitorCcw = 0x2000,
		MonitorRemotingCookie = 0x4000 | MonitorRemoting,
		MonitorRemotingAsync = 0x8000 | MonitorRemoting,
		MonitorSuspends = 0x10000,
		MonitorCacheSearches = 0x20000,
		[Obsolete]
		MonitorClrExceptions = 0x1000000,
		MonitorAll = 0x107ffff,
		EnableReJit = 0x40000,
		EnableInprocDebugging = 0x80000,
		EnableJitMaps = 0x100000,
		DisableInlining = 0x200000,
		DisableOptimizations = 0x400000,
		EnableObjectAllocated = 0x800000,
		EnableFunctionArgs = 0x2000000,
		EnableFunctionRetval = 0x4000000,
		EnableFrameInfo = 0x8000000,
		EnableStackSnapshot = 0x10000000,
		UseProfileImages = 0x20000000,
		All = 0x3fffffff,
		MonitorImmutable = MonitorCodeTransitions | MonitorRemoting | MonitorRemotingCookie | MonitorRemotingAsync | MonitorGc | EnableReJit | EnableInprocDebugging | EnableJitMaps | DisableOptimizations | DisableInlining | EnableObjectAllocated | EnableFunctionArgs | EnableFunctionRetval | EnableFrameInfo | EnableStackSnapshot | UseProfileImages
	}
}