#include "stdafx.h"
#include "Chronos.DotNet.Agent.Internal.h"

namespace Chronos
{
	namespace Agent
	{
		namespace DotNet
		{
			FrameworkAdapter::FrameworkAdapter()
			{
				_profilingEvents = new RuntimeProfilingEvents();
				_metadataProvider = new Reflection::RuntimeMetadataProvider();
				_frameworkSettings = null;
			}

			FrameworkAdapter::~FrameworkAdapter()
			{
				__FREEOBJ(_metadataProvider);
				__FREEOBJ(_profilingEvents);
				__FREEOBJ(_metadataProvider);
			}

			HRESULT FrameworkAdapter::BeginInitialize(FrameworkSettings* frameworkSettings, ProfilingTargetSettings* profilingTargetSettings)
			{
				_frameworkSettings = frameworkSettings;
				__bool profileChildProcess = false;
				__RETURN_IF_FAILED( profilingTargetSettings->GetProfileChildProcess(&profileChildProcess) );
				if (!profileChildProcess)
				{
					__string enableProfilingEnvironmentVariable(L"COR_ENABLE_PROFILING");
					EnvironmentVariables::Remove(enableProfilingEnvironmentVariable);
				}
				return S_OK;
			}

			HRESULT FrameworkAdapter::ExportServices(ServiceContainer* container)
			{
				__REGISTER_SERVICE(container, RuntimeProfilingEvents, _profilingEvents);
				__REGISTER_SERVICE(container, Reflection::RuntimeMetadataProvider, _metadataProvider);
				return S_OK;
			}

			HRESULT FrameworkAdapter::ImportServices(ServiceContainer* container)
			{
				return S_OK;
			}

			HRESULT FrameworkAdapter::EndInitialize()
			{
				return S_OK;
			}

			const __guid FrameworkAdapter::FrameworkUid = Converter::ConvertStringToGuid(L"{6711F85A-3CD1-4E76-8502-C3D4404E47C8}");
		}
	}
}