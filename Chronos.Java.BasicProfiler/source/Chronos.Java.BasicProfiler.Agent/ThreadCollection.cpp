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
				HRESULT ThreadCollection::InitializeUnitSpecial(ThreadInfo* unit)
				{
					unit->InitializeMetadata(_metadataProvider);
					return S_OK;
				}

				HRESULT ThreadCollection::GetCurrentThreadInfo(ThreadInfo** unit)
				{
					ThreadID threadId;
					HRESULT result = _metadataProvider->GetCurrentThreadId(&threadId);
					__RETURN_IF_FAILED(result);
					*unit = GetUnit(threadId);
					if (*unit == null)
					{
						return E_FAIL;
					}
					return S_OK;
				}

				const __string* ThreadCollection::MainThreadName = new __string(L"Main Thread");
				const __string* ThreadCollection::FinalizationThreadName = new __string(L"Finalization Thread");
				const __string* ThreadCollection::WorkerThreadName = new __string(L"Worker Thread");

				const __guid ThreadCollection::ServiceToken = Converter::ConvertStringToGuid(L"{9CC19383-CFFF-4B6F-8E68-934EB3134E29}");
			}
		}
	}
}