#include "stdafx.h"
#include "Chronos.Java.BasicProfiler.Agent.h"

namespace Chronos
{
	namespace Agent
	{
		namespace Java
		{
			namespace BasicProfiler
			{
				ThreadInfo::ThreadInfo()
					: _osThreadId(0)
				{
				}
				
				void ThreadInfo::PrepareClose()
				{
					_osThreadId = GetMetadata()->GetOsThreadId();
				}

				__uint ThreadInfo::GetOsThreadId()
				{
					if (GetIsAlive() && _osThreadId == 0)
					{
						_osThreadId = GetMetadata()->GetOsThreadId();
					}
					return _osThreadId;
				}

				Reflection::ThreadMetadata* ThreadInfo::GetMetadataInternal()
				{
					Reflection::ThreadMetadata* metadata;
					jthread thread = __UID_TO_JTHREAD(Id);
					_metadataProvider->GetThread(thread, &metadata);
					return metadata;
				}

				void ThreadInfo::OnLoaded()
				{
					PrepareClose();
				}
			}
		}
	}
}