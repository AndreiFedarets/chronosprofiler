#pragma once
#include <Chronos.Common.EventsTree/Chronos.Common.EventsTree.Agent.h>
#include <Chronos.DotNet.BasicProfiler/Chronos.DotNet.BasicProfiler.Agent.h>

#ifdef CHRONOS_DOTNET_TRACING_PROFILER_EXPORT_API
#define CHRONOS_DOTNET_TRACING_PROFILER_API __declspec(dllexport) 
#else
#define CHRONOS_DOTNET_TRACING_PROFILER_API __declspec(dllimport) 
#endif

namespace Chronos
{
	namespace Agent
	{
		namespace DotNet
		{
			namespace TracingProfiler
			{
// ==================================================================================================================================================
				class CHRONOS_DOTNET_API EventType
				{
					public:
						static const __byte AppDomainCreation = 0x01;
						static const __byte AppDomainShutdown = 0x02;
						static const __byte AssemblyLoad = 0x03;
						static const __byte AssemblyUnload = 0x04;
						static const __byte ModuleLoad = 0x05;
						static const __byte ModuleUnload = 0x06;
						static const __byte ClassLoad = 0x07;
						static const __byte ClassUnload = 0x08;
						static const __byte ThreadCreate = 0x09;
						static const __byte ThreadDestroy = 0x0A;
						static const __byte GarbageCollection = 0x0B;
						static const __byte ExceptionThrown = 0x0C;
						static const __byte ManagedFunctionCall = 0x0D;
						static const __byte ManagedToUnmanagedTransition = 0x0E;
						static const __byte UnmanagedToManagedTransition = 0x0F;
				};

// ==================================================================================================================================================
			}
		}
	}
}