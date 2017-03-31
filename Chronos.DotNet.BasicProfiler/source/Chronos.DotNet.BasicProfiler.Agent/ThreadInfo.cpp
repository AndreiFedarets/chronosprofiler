#include "stdafx.h"
#include "Chronos.DotNet.BasicProfiler.Agent.h"

namespace Chronos
{
	namespace Agent
	{
		namespace DotNet
		{
			namespace BasicProfiler
			{
				ThreadInfo::ThreadInfo()
					: _handle(0), _osThreadId(0)
				{
				}
				
				void ThreadInfo::PrepareClose()
				{
					_handle = GetMetadata()->GetThreadHandle();
					_osThreadId = GetMetadata()->GetOsThreadId();
				}

				HANDLE ThreadInfo::GetThreadHandle()
				{
					if (GetIsAlive() && _handle == 0)
					{
						_handle = GetMetadata()->GetThreadHandle();
					}
					return _handle;
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
					_metadataProvider->GetThread(Id, &metadata);
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