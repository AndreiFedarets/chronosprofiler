#pragma once
#include <Chronos.Agent.h>

#ifdef CHRONOS_COMMON_EXPORT_API
#define CHRONOS_COMMON_API __declspec(dllexport) 
#else
#define CHRONOS_COMMON_API __declspec(dllimport) 
#endif


namespace Chronos
{
	namespace Agent
	{
		namespace Common
		{
// ==================================================================================================================================================
			class CHRONOS_COMMON_API FrameworkAdapter : public IFrameworkAdapter
			{
				public:
					FrameworkAdapter();
					~FrameworkAdapter();
					HRESULT BeginInitialize(FrameworkSettings* frameworkSettings, ProfilingTargetSettings* profilingTargetSettings);
					HRESULT ExportServices(ServiceContainer* container);
					HRESULT ImportServices(ServiceContainer* container);
					HRESULT EndInitialize();
					const static __guid FrameworkUid;
			};

// ==================================================================================================================================================
		}
	}
}