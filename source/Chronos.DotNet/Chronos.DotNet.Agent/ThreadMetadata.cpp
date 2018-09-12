#include "stdafx.h"
#include "Chronos.DotNet.Agent.h"

namespace Chronos
{
	namespace Agent
	{
		namespace DotNet
		{
			namespace Reflection
			{
				ThreadMetadata::ThreadMetadata(ICorProfilerInfo2* corProfilerInfo, RuntimeMetadataProvider* provider, ThreadID threadId)
				{
					_corProfilerInfo = corProfilerInfo;
					_provider = provider;
					_threadId = threadId;
					_name = null;
					_threadHandle = 0;
					_osThreadId = 0;
				}

				ThreadMetadata::~ThreadMetadata()
				{
					__FREEOBJ(_name);
				}

				ThreadID ThreadMetadata::GetId()
				{
					return _threadId;
				}

				__string* ThreadMetadata::GetName()
				{
					Initialize();
					return _name;
				}

				HANDLE ThreadMetadata::GetThreadHandle()
				{
					Initialize();
					return _threadHandle;
				}
				
				__uint ThreadMetadata::GetOsThreadId()
				{
					Initialize();
					return _osThreadId;
				}
				
				void ThreadMetadata::Initialize()
				{
					if (_name == null)
					{
						//const __uint NameMaxLength = 1000;
						//__wchar nativeName[NameMaxLength];

						_corProfilerInfo->GetThreadInfo(_threadId, (DWORD*)&_osThreadId);
						_corProfilerInfo->GetHandleFromThread(_threadId, &_threadHandle);

						_name = new __string();
					}
				}
			}
		}
	}
}