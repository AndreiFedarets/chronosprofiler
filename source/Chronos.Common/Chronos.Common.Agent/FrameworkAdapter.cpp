#include "stdafx.h"
#include "Chronos.Common.Agent.h"

namespace Chronos
{
	namespace Agent
	{
		namespace Common
		{
			FrameworkAdapter::FrameworkAdapter()
			{
			}

			FrameworkAdapter::~FrameworkAdapter()
			{
			}

			HRESULT FrameworkAdapter::BeginInitialize(FrameworkSettings* frameworkSettings, ProfilingTargetSettings* profilingTargetSettings)
			{
				return S_OK;
			}

			HRESULT FrameworkAdapter::ExportServices(ServiceContainer* container)
			{
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

			HRESULT FrameworkAdapter::SubscribeEvents()
			{
				return S_OK;
			}

			HRESULT FrameworkAdapter::FlushData()
			{
				return S_OK;
			}

			const __guid FrameworkAdapter::FrameworkUid = Converter::ConvertStringToGuid(L"{DEF6FD49-CD6D-484F-A2F3-4406CE9178F9}");
		}
	}
}