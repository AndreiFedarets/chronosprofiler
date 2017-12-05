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
					jthread thread;
					jint result = _metadataProvider->GetCurrentThread(&thread);
					__JRETURN_IF_FAILED(result);
					*unit = GetUnit(__JOBJECT_TO_UID(thread));
					if (*unit == null)
					{
						return E_FAIL;
					}
					return S_OK;
				}

				const __string* ThreadCollection::MainThreadName = new __string(L"Main Thread");
				const __string* ThreadCollection::FinalizationThreadName = new __string(L"Finalization Thread");
				const __string* ThreadCollection::WorkerThreadName = new __string(L"Worker Thread");

				const __guid ThreadCollection::ServiceToken = Converter::ConvertStringToGuid(L"{A499080B-C6EF-4340-BF64-EC431B0D056F}");
			}
		}
	}
}